/* Author: Vincent Simonetti
 * Date: 2/4/2011
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
#if WINDOWS
using System.Reflection.Emit;
using System.IO;
#endif

namespace SGDE.Content.Code.Compiler
{
    public sealed class Compiler
    {
#if WINDOWS
        private AssemblyBuilder ab;
        private ModuleBuilder mb;
#endif

        //Error handlers
        private string message;
        private int line, col;

        public Compiler(string name, string version)
        {
#if WINDOWS
            AssemblyName aName = new AssemblyName(name);
            ab = AppDomain.CurrentDomain.DefineDynamicAssembly(aName, AssemblyBuilderAccess.RunAndSave);
            ab.DefineVersionInfoResource(aName.Name, version, "Stevens Game Development Club", "Copyright © Stevens Game Development Club 2011", null);
            mb = ab.DefineDynamicModule(aName.Name, aName.Name + "_AS3.dll");
            ResetError();
#else
            message = Messages.Compiler_Error_WindowsOnly;
#endif
        }

        private void ResetError()
        {
            message = Messages.Compiler_NoError;
            line = col = 0;
        }

        public bool AddCode(string code)
        {
#if WINDOWS
            if (string.IsNullOrWhiteSpace(code))
            {
                return false;
            }
            SourceControl<object> controlFull = new SourceControl<object>(code);
            if (!RemoveComments(ref controlFull))
            {
                return false;
            }
            if (controlFull.Lines.Count == 0)
            {
                //All comments, so code was "compiled", return true
                return true;
            }
            List<SourceControl<string>> names = new List<SourceControl<string>>();
            if (!GetPackages(controlFull, ref names))
            {
                return false;
            }
            //TODO
            return true;
#else
            return false;
#endif
        }

        #region Error information

        public string GetMessage()
        {
            return this.message;
        }

        public int GetLine()
        {
            return this.line;
        }

        public int GetCol()
        {
            return this.col;
        }

        #endregion

#if WINDOWS
        #region Remove Comments

        private bool RemoveComments(ref SourceControl<object> sc)
        {
            bool comment = false;
            int index;
            //Remove comments
            for (int i = 0; i < sc.Lines.Count; i++)
            {
                string line = sc.Lines[i].Line;
                if (comment)
                {
                    index = line.IndexOf("*/");
                    if (index >= 0)
                    {
                        index += 2;
                        sc.Lines[i].Line = new string(' ', index) + line.Substring(index);
                        comment = false;
                        i--; //Just in case
                    }
                    else
                    {
                        sc.Lines[i].Line = string.Empty;
                    }
                }
                else
                {
                    index = line.IndexOf("//");
                    if (index >= 0 && ParaCount(line, index) % 2 == 0)
                    {
                        sc.Lines[i].Line = line.Substring(0, index).TrimEnd();
                    }
                    else
                    {
                        index = line.IndexOf("/*");
                        if (index >= 0 && ParaCount(line, index) % 2 == 0)
                        {
                            int start = index;
                            index = line.IndexOf("*/", index);
                            if (index >= 0)
                            {
                                //Single line comment
                                index += 2;
                                sc.Lines[i].Line = line.Substring(0, start) + new string(' ', index - start) + line.Substring(index);
                                i--; //So the comment checks repeat on single line comments
                            }
                            else
                            {
                                //Multiline comment
                                sc.Lines[i].Line = line.Substring(0, start).TrimEnd();
                                this.line = i;
                                this.col = start;
                                comment = true;
                            }
                        }
                    }
                }
            }
            //If we are still in comment mode then we are missing a end of comment marker. This means the code has been corrupted by the cleaner
            if (comment)
            {
                this.message = Messages.Compiler_Error_NoEndComment;
                return false;
            }
            //Remove all blank lines to speed up processing, line numbers are kept intact
            List<SourceLine> lines = new List<SourceLine>();
            for (int i = 0; i < sc.Lines.Count; i++)
            {
                SourceLine line = sc.Lines[i];
                if (!string.IsNullOrWhiteSpace(line.Line))
                {
                    lines.Add(line);
                }
            }
            sc.Lines = lines;
            ResetError();
            return true;
        }

        private static int ParaCount(string line, int upTo)
        {
            int c = 0;
            for (int i = 0; i < upTo; i++)
            {
                switch (line[i])
                {
                    case '"':
                    case '\'':
                        c++;
                        break;
                }
            }
            return c;
        }

        #endregion

        #region Get Packages

        private bool GetPackages(SourceControl<object> sc, ref List<SourceControl<string>> names)
        {
            bool onPackage = false;
            string package = null;
            int depth = 0;
            List<SourceLine> lines = new List<SourceLine>();
            for (int i = 0; i < sc.Lines.Count; i++)
            {
                SourceLine line = sc.Lines[i];
                if (onPackage)
                {
                    //TODO: Find where package, class, or interface end
                    if (depth == 0)
                    {
                        if (package == null)
                        {
                            package = string.Empty;
                        }
                        names.Add(new SourceControl<string>(lines, package));
                        lines = new List<SourceLine>();
                        package = null;
                    }
                }
                else
                {
                    //TODO: Get package, class, or interface
                }
            }
            ResetError();
            return true;
        }

        #endregion

        #region Get Namespaces

        //TODO

        #endregion

        #region Get Classes/Interfaces

        //TODO

        #endregion

        #region Get Fields

        //TODO

        #endregion

        #region Get Methods

        //TODO

        #endregion

        #region Compile code

        //TODO

        #endregion
#endif

        public override string ToString()
        {
            return this.message;
        }

        /* TODO:
         * Operators: http://help.adobe.com/en_US/FlashPlatform/beta/reference/actionscript/3/operators.html
         * AssembkyBuilder: http://msdn.microsoft.com/en-us/library/system.reflection.emit.assemblybuilder.aspx#Y3359
         * 
         * Have a helper function that converts a delegate into a Function.
         * Figure out how to write out classes (for actual compilation) but can also write out usable functions
         */

        /*
         * Example compile. For this example I simply reformat everything into C# code, in reality I would have data types handle the "formatting" for me. Thus I don't need to compile twice.
         * The example methods don't have any code to them. The actual compiler would have a one final step, which is to process the code. The compiling shouldn't happen instantly. This way
         * a collection of data types can be assembled, making it easier to compile the actual code. When it gets to the code step, any non-existant functions/classes would be turned into a
         * lookup. Like trying to use the Math functions. Another missing step is imports and namespaces
         * 
        -Original
        package com.example.quickstart
        {
            public class Greeter
            {
                // ------- Constructor -------
                public function Greeter(initialName:String = "")
                {
                    // set the name value, if specified
                }
      
                // ------- Properties -------
                public var name:String;
      
                // ------- Methods -------
                public function sayHello():String
                {
                    // create the greeting text and pass it as the return value
                }
            }
        }

        --------------------------------------------
        -Remove comments
        package com.example.quickstart
        {
            public class Greeter
            {
      
                public function Greeter(initialName:String = "")
                {
         
                }
      
      
                public var name:String;
      
      
                public function sayHello():String
                {
         
                }
            }
        }

        --------------------------------------------
        -Get package

        package: "com.example.quickstart"

        public class Greeter
        {
   
            public function Greeter(initialName:String = "")
            {
      
            }
   
   
            public var name:String;
   
   
            public function sayHello():String
            {
      
            }
        }
        
        --------------------------------------------
        -Get class
        
        package: "com.example.quickstart"
        class: "Greeter"
        
        public function Greeter(initialName:String = "")
        {
      
        }
   
   
        public var name:String;
   
   
        public function sayHello():String
        {
      
        }
        
        --------------------------------------------
        -Get fields
        
        package: "com.example.quickstart"
        class: "Greeter"
        fields:
        -public String name
        
        public function Greeter(initialName:String = "")
        {
      
        }
   
   
        
   
   
        public function sayHello():String
        {
      
        }
        
        --------------------------------------------
        -Get methods
        
        package: "com.example.quickstart"
        class: "Greeter"
        fields:
        -public String name
        methods:
        -public void Greeter(String initialName = "")
        -{
        -}
        -public String sayHello()
        -{
        -}
         */
    }
}
