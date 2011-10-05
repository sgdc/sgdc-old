/* Author: Vincent Simonetti
 * Date: 2/3/2011
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;

namespace SGDE.Content.Code.Library
{
    /// <summary>
    /// The Object class is at the root of the ActionScript class hierarchy.
    /// </summary>
    public class Object
    {
        /// <summary>
        /// A reference to the class object or constructor function for a given object instance.
        /// </summary>
        public Object constructor { get; set; }
        /// <summary>
        /// A reference to the prototype object of a class or function object.
        /// </summary>
        public Object __proto__ { get; internal set; } //There is a static version of this called "prototype". That one will be handled through reflection instead of direct instatiation of class Types. If "prototype" is used on a instantiated class then __proto__ is returned.

        internal bool dynamic;

        /// <summary>
        /// Creates an Object object and stores a reference to the object's constructor method in the object's constructor property.
        /// </summary>
        public Object()
            : this(true, true)
        {
        }

        internal Object(bool getConstructor, bool getType)
        {
            if (getConstructor)
            {
                MethodBase mbase = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod();
                if (mbase is ConstructorInfo)
                {
                    this.constructor = new ConstructorObject((ConstructorInfo)mbase);
                }
            }
            if (getType)
            {
                this.__proto__ = new PrototypeObject(GetType());
            }
            this.dynamic = getType && this.__proto__.__proto__ == null;
        }

        /// <summary>
        /// Returns the string representation of the specified object.
        /// </summary>
        /// <returns>A string representation of the object.</returns>
        public virtual String toString()
        {
            return new String(this.ToString());
        }

        /// <summary>
        /// Returns the string representation of this object, formatted according to locale-specific conventions.
        /// </summary>
        /// <returns>A string representation of this object formatted according to local conventions.</returns>
        public virtual String toLocaleString()
        {
            return this.toString();
        }

        /// <summary>
        /// Returns the primitive value of the specified object.
        /// </summary>
        /// <returns>The primitive value of this object or the object itself.</returns>
        public virtual Object valueOf()
        {
            return this;
        }

        /// <summary>
        /// Indicates whether an object has a specified property defined.
        /// </summary>
        /// <param name="name">The property of the object.</param>
        /// <returns>If the target object has the property specified by the name parameter this value is true, otherwise false.</returns>
        public virtual Boolean hasOwnProperty(String name)
        {
            string str = name.value; //Do this so we are working with .Net String
            Type t = ((PrototypeObject)this.__proto__).prototypeType;
            //TODO: It's easier to check documentation then to try and explain it
            return new Boolean(false);
        }

        /// <summary>
        /// Indicates whether an instance of the Object class is in the prototype chain of the object specified as the parameter.
        /// </summary>
        /// <param name="theClass">The class to which the specified object may refer.</param>
        /// <returns>If the object is in the prototype chain of the object specified by the theClass parameter this value is true, otherwise false.</returns>
        public virtual Boolean isPrototypeOf(Object theClass)
        {
            return new Boolean(((PrototypeObject)this.__proto__).prototypeType.IsInstanceOfType(theClass));
        }

        /// <summary>
        /// Indicates whether the specified property exists and is enumerable.
        /// </summary>
        /// <param name="name">The property of the object.</param>
        /// <returns>If the property specified by the name parameter is enumerable this value is true, otherwise false.</returns>
        public virtual Boolean propertyIsEnumerable(String name)
        {
            if (hasOwnProperty(name))
            {
                //TODO: See if property is enumerable
            }
            return new Boolean(false);
        }

        #region Helper Types

        internal class ConstructorObject : Object
        {
            public ConstructorInfo conInfo;

            public ConstructorObject(ConstructorInfo info)
                : base(false, false)
            {
                this.conInfo = info;
            }

            public override String toString()
            {
                return new String(conInfo.ToString());
            }
        }

        internal class PrototypeObject : Object
        {
            public Type prototypeType;

            public PrototypeObject(Type type)
                : base(false, false)
            {
                if (!typeof(object).Equals(type.BaseType))
                {
                    this.__proto__ = new PrototypeObject(type.BaseType);
                }
                this.prototypeType = type;
            }

            public override String toString()
            {
                return new String(prototypeType.ToString());
            }
        }

        internal class PrimitiveWrapper : Object
        {
            public object primitive;

            public PrimitiveWrapper(object primitive)
            {
                if (!(primitive is ValueType))
                {
                    throw new InvalidOperationException("Value not primitive");
                }
                this.primitive = primitive;
            }

            public override String toString()
            {
                return new String(primitive.ToString());
            }
        }

        #endregion
    }
}
