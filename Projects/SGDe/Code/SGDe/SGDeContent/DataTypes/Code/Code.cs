using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGDeContent.DataTypes.Code
{
    public class Code : ProcessedContent
    {
        /// <summary>
        /// Is a constant value.
        /// </summary>
        public bool Constant;
        /// <summary>
        /// The constant value.
        /// </summary>
        public object ConstantValue;

        /// <summary>
        /// If the execution is variable and could change based on what values are used. Variable values are ones that use time or other similar variables, constant variables are ones that don't change because 
        /// they are constant or rely on "set once" varables like screen width/height.
        /// </summary>
        public bool VariableExecution;

        public List<int> UsedConstantsAndSystemVariables;
    }
}
