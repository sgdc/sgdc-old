using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace SGDE.Content.Code.Library
{
    /// <summary>
    /// A function is the basic unit of code that can be invoked in ActionScript.
    /// </summary>
    public sealed class Function : Object
    {
        internal MethodInfo method;

        /*
        Function.prototype.toString
        Function.prototype.apply
        Function.prototype.call
        Function.length //What the heck is this?
         */
    }
}
