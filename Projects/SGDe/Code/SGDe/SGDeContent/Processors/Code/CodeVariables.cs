using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGDeContent.Processors.Code
{
    public static class CodeVariables
    {
        public static Dictionary<string, int> ConstantsAndGlobalVariables;

        static CodeVariables()
        {
            ConstantsAndGlobalVariables = new Dictionary<string, int>();
            ConstantsAndGlobalVariables.Add("GameWidth", 0);
            ConstantsAndGlobalVariables.Add("GameHeight", 1);
            ConstantsAndGlobalVariables.Add("AddToList", 2); //When used in the creation of a global list, adds the item to the list. A global list is one that is not being created within the context of this JavaScript code.
            ConstantsAndGlobalVariables.Add("External ", 3); //Get an external object using the specified DID
        }
    }
}
