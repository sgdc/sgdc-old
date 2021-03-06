﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGDE.Content.Code.Library
{
    /// <summary>
    /// The Array class lets you access and manipulate arrays.
    /// </summary>
    public class Array : Object
    {
        internal Object[] objA;

        /// <summary>
        /// Specifies case-insensitive sorting for the Array class sorting methods.
        /// </summary>
        public const uint CASEINSENSITIVE = 1 << 0;
        /// <summary>
        /// Specifies descending sorting for the Array class sorting methods.
        /// </summary>
        public const uint DESCENDING = 1 << 1;
        /// <summary>
        /// Specifies the unique sorting requirement for the Array class sorting methods.
        /// </summary>
        public const uint UNIQUESORT = 1 << 2;
        /// <summary>
        /// Specifies that a sort returns an array that consists of array indices.
        /// </summary>
        public const uint RETURNINDEXEDARRAY = 1 << 3;
        /// <summary>
        /// Specifies numeric (instead of character-string) sorting for the Array class sorting methods.
        /// </summary>
        public const uint NUMERIC = 1 << 4;

        /// <summary>
        /// Lets you create an array that contains the specified elements.
        /// </summary>
        /// <param name="values">A comma-separated list of one or more arbitrary values.</param>
        public Array(params Object[] values)
            : base(true, true)
        {
            if (values == null)
            {
                values = new Object[0];
            }
            this.objA = values;
            this.dynamic = this.__proto__.__proto__.__proto__ == null;
        }

#if WINDOWS
        /// <summary>
        /// Lets you create an array of the specified number of elements.
        /// </summary>
        /// <param name="numElements">An integer that specifies the number of elements in the array.</param>
        public Array(int numElements = 0)
            : this(new Object[numElements])
#else
        /// <summary>
        /// Lets you create an array of the specified number of elements.
        /// </summary>
        public Array()
            : this(0)
        {
        }

        /// <summary>
        /// Lets you create an array of the specified number of elements.
        /// </summary>
        /// <param name="numElements">An integer that specifies the number of elements in the array.</param>
        public Array(int numElements)
            : this(new Object[numElements])
#endif
        {
            //Adobe's function throws an error when numElements is less then 0. Not sure about open source stuff right now
        }

        /// <summary>
        /// Returns the string representation of the specified Array.
        /// </summary>
        /// <returns>A string representation of the Array.</returns>
        public override String toString()
        {
            return join(null, false);
        }

        /// <summary>
        /// Returns the string representation of this Array, formatted according to locale-specific conventions.
        /// </summary>
        /// <returns>A string representation of this Array formatted according to local conventions.</returns>
        public override String toLocaleString()
        {
            return join(null, true);
        }

        /// <summary>
        /// Concatenates the elements specified in the parameters with the elements in an array and creates a new array.
        /// </summary>
        /// <param name="args">A value of any data type (such as numbers, elements, or strings) to be concatenated in a new array.</param>
        /// <returns>An array that contains the elements from this array followed by elements from the parameters.</returns>
        public virtual Array concat(params Object[] args)
        {
            Object[] obj = (Object[])this.objA.Clone();
            addTo(ref obj, obj.Length, args, true);
            return new Array(obj);
        }

        private void addTo(ref Object[] obj, int objLen, Object[] args, bool end)
        {
            if (args != null && args.Length > 0)
            {
                global::System.Array.Resize(ref obj, objLen + args.Length);
                if (end)
                {
                    global::System.Array.Copy(args, 0, obj, objLen, args.Length);
                }
                else
                {
                    global::System.Array.Copy(obj, 0, obj, args.Length, objLen);
                    global::System.Array.Copy(args, 0, obj, 0, args.Length);
                }
            }
        }

        /// <summary>
        /// Converts the elements in an array to strings, inserts the specified separator between the elements, concatenates them, and returns the resulting string.
        /// </summary>
        /// <returns>A string consisting of the elements of an array converted to strings and separated by the specified parameter.</returns>
        public virtual String join()
        {
            return join(Number.NaN);
        }

        /// <summary>
        /// Converts the elements in an array to strings, inserts the specified separator between the elements, concatenates them, and returns the resulting string.
        /// </summary>
        /// <param name="sep">A character or string that separates array elements in the returned string. If you omit this parameter, a comma is used as the default separator.</param>
        /// <returns>A string consisting of the elements of an array converted to strings and separated by the specified parameter.</returns>
        public virtual String join(Object sep)
        {
            return join(sep, false);
        }

        private String join(Object sep, bool local)
        {
            StringBuilder bu = new StringBuilder();
            string sepS;
            if (sep == null || !(sep is String))
            {
                sepS = ",";
            }
            else
            {
                sepS = ((String)sep).value;
            }
            foreach (Object obj in this.objA)
            {
                if (obj != null)
                {
                    if (bu.Length > 0)
                    {
                        bu.Append(sepS);
                    }
                    String str = local ? obj.toLocaleString() : obj.toString();
                    bu.Append(str.value);
                }
            }
            return new String(bu.ToString());
        }

        /// <summary>
        /// Removes the last element from an array and returns the value of that element.
        /// </summary>
        /// <returns>The value of the last element (of any data type) in the specified array.</returns>
        public virtual Object pop()
        {
            if (this.objA.Length == 0)
            {
                return null;
            }
            int l = this.objA.Length - 1;
            Object obj = this.objA[l];
            this.objA[l] = null;
            trimArray();
            return obj;
        }

        /// <summary>
        /// Adds one or more elements to the end of an array and returns the new length of the array.
        /// </summary>
        /// <param name="args">One or more values to append to the array.</param>
        /// <returns>An integer representing the length of the new array.</returns>
        public virtual uint push(params Object[] args)
        {
            addTo(ref this.objA, this.objA.Length, args, true);
            return (uint)this.objA.Length;
        }

        /// <summary>
        /// Reverses the array in place.
        /// </summary>
        /// <returns>The new array.</returns>
        public virtual Array reverse()
        {
            int len = this.objA.Length;
            if (len > 0)
            {
                Object obj = this.objA[0];
                for (int i = 0, l = len - 1; i < len; i++, l--)
                {
                    this.objA[i] = this.objA[l];
                }
                this.objA[len - 1] = obj;
            }
            return this;
        }

        /// <summary>
        /// Removes the first element from an array and returns that element.
        /// </summary>
        /// <returns>The first element (of any data type) in an array.</returns>
        public virtual Object shift()
        {
            int len = this.objA.Length;
            if (len == 0)
            {
                return null;
            }
            Object obj = this.objA[0];
            global::System.Array.Copy(this.objA, 1, this.objA, 0, len - 1);
            global::System.Array.Resize(ref this.objA, len - 1);
            return obj;
        }

#if WINDOWS
        /// <summary>
        /// Returns a new array that consists of a range of elements from the original array, without modifying the original array.
        /// </summary>
        /// <param name="startIndex">
        /// A number specifying the index of the starting point for the slice. If startIndex is a negative number, the starting point begins at the end of the array, where -1 is the last element.
        /// </param>
        /// <param name="endIndex">
        /// A number specifying the index of the ending point for the slice. If you omit this parameter, the slice includes all elements from the starting point to the end of the array. If endIndex is a negative number, 
        /// the ending point is specified from the end of the array, where -1 is the last element.
        /// </param>
        /// <returns>An array that consists of a range of elements from the original array.</returns>
        public virtual Array slice(int startIndex = 0, int endIndex = 16777215)
#else
        /// <summary>
        /// Returns a new array that consists of a range of elements from the original array, without modifying the original array.
        /// </summary>
        /// <returns>An array that consists of a range of elements from the original array.</returns>
        public Array slice()
        {
            return slice(0);
        }

        /// <summary>
        /// Returns a new array that consists of a range of elements from the original array, without modifying the original array.
        /// </summary>
        /// <param name="startIndex">
        /// A number specifying the index of the starting point for the slice. If startIndex is a negative number, the starting point begins at the end of the array, where -1 is the last element.
        /// </param>
        /// <returns>An array that consists of a range of elements from the original array.</returns>
        public Array slice(int startIndex)
        {
            return slice(startIndex, 16777215);
        }

        /// <summary>
        /// Returns a new array that consists of a range of elements from the original array, without modifying the original array.
        /// </summary>
        /// <param name="startIndex">
        /// A number specifying the index of the starting point for the slice. If startIndex is a negative number, the starting point begins at the end of the array, where -1 is the last element.
        /// </param>
        /// <param name="endIndex">
        /// A number specifying the index of the ending point for the slice. If you omit this parameter, the slice includes all elements from the starting point to the end of the array. If endIndex is a negative number, 
        /// the ending point is specified from the end of the array, where -1 is the last element.
        /// </param>
        /// <returns>An array that consists of a range of elements from the original array.</returns>
        public Array slice(int startIndex, int endIndex)
#endif
        {
            //Handle the start index
            int origin;
            if (startIndex >= 0)
            {
                origin = startIndex;
            }
            else
            {
                origin = this.objA.Length + startIndex;
            }
            //Handle the end index
            int len;
            if (endIndex < 0)
            {
                endIndex = this.objA.Length + endIndex;
            }
            else if (endIndex > this.objA.Length)
            {
                endIndex = this.objA.Length;
            }
            if (endIndex < origin)
            {
                int t = endIndex;
                endIndex = origin;
                origin = t;
            }
            len = endIndex - origin;
            Object[] obj = new Object[len];
            global::System.Array.Copy(this.objA, origin, obj, 0, len);
            return new Array(obj);
        }

        /// <summary>
        /// Sorts the elements in an array. This method sorts according to Unicode values. (ASCII is a subset of Unicode.)
        /// </summary>
        /// <param name="args">The arguments specifying a comparison function and one or more values that determine the behavior of the sort.</param>
        /// <returns>The return value depends on whether you pass any arguments.</returns>
        public virtual Array sort(params Object[] args)
        {
            SGDESort sort = new SGDESort();
            if (args != null)
            {
                if (args.Length == 1)
                {
                    //One object can be either a primitive or function. Figure it out.
                    if (args[0] is Object.PrimitiveWrapper)
                    {
                        object prim = (args[0] as Object.PrimitiveWrapper).primitive;
                        //We only want uint primitives. An int is usable but then we have to go through type conversions (easy enough) but it could be a float which won't produce the proper result, or some other type that could just have issues.
                        if (prim is uint)
                        {
                            sort.options = (uint)prim;
                        }
                    }
                    else if (args[0] is Function)
                    {
                        sort.fun = (Function)args[0];
                    }
                }
                else if (args.Length >= 2)
                {
                    //Must be in the order of "compareFunction, sortOptions"
                    if (args[0] is Function)
                    {
                        sort.fun = (Function)args[0];
                    }
                    if (args[1] is Object.PrimitiveWrapper)
                    {
                        object prim = (args[1] as Object.PrimitiveWrapper).primitive;
                        if (prim is uint)
                        {
                            sort.options = (uint)prim;
                        }
                    }
                }
            }
            Object[] tmpArray = (Object[])this.objA.Clone();
            Array retArray = null;
            try
            {
                global::System.Array.Sort(tmpArray, sort);
                //Not desired to have in a try/catch (for speed reasons. We're interpreting/emulating enough as it is)
                if ((sort.options & Array.RETURNINDEXEDARRAY) == Array.RETURNINDEXEDARRAY)
                {
                    //Don't modify it, return the indexes (not the fastest)
                    retArray = new Array(tmpArray.Length);
                    int index;
                    for (int i = 0; i < tmpArray.Length; i++)
                    {
                        index = global::System.Array.IndexOf(tmpArray, this.objA[i]);
                        if (index == -1)
                        {
                            //Couldn't find it. Might implement custom Equals function. Do a reference search
                            for (int k = 0; k < tmpArray.Length; i++)
                            {
                                if (object.ReferenceEquals(tmpArray[k], this.objA[i]))
                                {
                                    index = k;
                                    break;
                                }
                            }
                        }
                        retArray[i] = new Object.PrimitiveWrapper(index);
                    }
                }
                else
                {
                    //Modify the array
                    this.objA = tmpArray;
                }
            }
            catch
            {
                if ((sort.options & Array.UNIQUESORT) == Array.UNIQUESORT)
                {
                    //Return an array with the value of "0"
                    retArray = new Array(1);
                    retArray.objA[0] = new Object.PrimitiveWrapper(0);
                }
            }
            return retArray;
        }

        private sealed class SGDESort : IComparer<Object>
        {
            public Function fun;
            public uint options;

            public int Compare(Object x, Object y)
            {
                bool unique = (options & Array.UNIQUESORT) == Array.UNIQUESORT;
                int comp;
                if (fun != null)
                {
                    throw new NotImplementedException(); //This gets "comp"
                    if (comp == 0 && unique)
                    {
                        throw new InvalidOperationException();
                    }
                }
                else
                {
                    string xs = x.toString().value;
                    string ys = y.toString().value;
                    if ((options & Array.NUMERIC) == Array.NUMERIC)
                    {
                        //Only do a numeric comparision if we don't have a specific sort function
                        int xi = 0, yi = 0;
                        bool gotX = false, gotY = false;

                        //Get X value
                        if (x is Object.PrimitiveWrapper)
                        {
                            object prim = ((Object.PrimitiveWrapper)x).primitive;
                            try
                            {
                                xi = Convert.ToInt32(prim);
                                gotX = true;
                            }
                            catch
                            {
                            }
                        }
                        else if (x is Number && Utils.IsInt((Number)x))
                        {
                            gotX = true;
                        }
                        if (!gotX)
                        {
                            if (!int.TryParse(xs, out xi))
                            {
                                throw new InvalidCastException();
                            }
                        }

                        //Get Y value
                        if (y is Object.PrimitiveWrapper)
                        {
                            object prim = ((Object.PrimitiveWrapper)y).primitive;
                            try
                            {
                                yi = Convert.ToInt32(prim);
                                gotY = true;
                            }
                            catch
                            {
                            }
                        }
                        else if (y is Number && Utils.IsInt((Number)y))
                        {
                            gotY = true;
                        }
                        if (!gotY)
                        {
                            if (!int.TryParse(ys, out yi))
                            {
                                throw new InvalidCastException();
                            }
                        }

                        //Compare
                        comp = xi.CompareTo(yi);
                    }
                    else
                    {
#if WINDOWS
                        comp = string.Compare(xs, ys, (options & Array.CASEINSENSITIVE) == Array.CASEINSENSITIVE);
#else
                        comp = string.Compare(xs, ys, ((options & Array.CASEINSENSITIVE) == Array.CASEINSENSITIVE) ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture);
#endif
                    }
                }
                //Finally we want to reverse the sort if descending
                if (comp != 0 && (options & Array.DESCENDING) == Array.DESCENDING)
                {
                    comp = -comp;
                }
                return comp;
            }
        }

        /// <summary>
        /// Adds elements to and removes elements from an array.
        /// </summary>
        /// <param name="startIndex">
        /// An integer that specifies the index of the element in the array where the insertion or deletion begins. You can use a negative integer to specify a position relative to the end of the array (for example, -1 
        /// is the last element of the array).
        /// </param>
        /// <param name="deleteCount">
        /// An integer that specifies the number of elements to be deleted. This number includes the element specified in the startIndex parameter. If you do not specify a value for the deleteCount parameter, the 
        /// method deletes all of the values from the startIndex element to the last element in the array. If the value is 0, no elements are deleted.
        /// </param>
        /// <param name="values">
        ///  An optional list of one or more comma-separated values to insert into the array at the position specified in the startIndex parameter. If an inserted value is of type Array, the array is kept intact and 
        ///  inserted as a single element. For example, if you splice an existing array of length three with another array of length three, the resulting array will have only four elements. One of the elements, 
        ///  however, will be an array of length three.
        /// </param>
        /// <returns>An array containing the elements that were removed from the original array.</returns>
        public virtual Array splice(int startIndex, uint deleteCount, params Object[] values)
        {
            if (startIndex < 0)
            {
                startIndex = this.objA.Length + startIndex;
            }
            Array arr = new Array((int)deleteCount);
            global::System.Array.Copy(this.objA, startIndex, arr.objA, 0, (int)deleteCount);
            if (values != null)
            {
                if (values.Length != deleteCount)
                {
                    if (values.Length - deleteCount < 0)
                    {
                        global::System.Array.Copy(this.objA, startIndex + (int)deleteCount, this.objA, startIndex + values.Length, this.objA.Length - (startIndex + (int)deleteCount));
                    }
                    int objL = this.objA.Length; //To prevent corruption of size when resize occurs
                    global::System.Array.Resize(ref this.objA, objL + values.Length - (int)deleteCount);
                    if (values.Length - deleteCount > 0)
                    {
                        global::System.Array.Copy(this.objA, startIndex + (int)deleteCount, this.objA, startIndex + values.Length, objL - (startIndex + (int)deleteCount));
                    }
                }
                global::System.Array.Copy(values, 0, this.objA, startIndex, values.Length);
                trimArray();
            }
            return arr;
        }

        /// <summary>
        /// Adds one or more elements to the beginning of an array and returns the new length of the array.
        /// </summary>
        /// <param name="args">One or more numbers, elements, or variables to be inserted at the beginning of the array.</param>
        /// <returns>An integer representing the new length of the array.</returns>
        public virtual uint unshift(params Object[] args)
        {
            addTo(ref this.objA, this.objA.Length, args, false);
            return (uint)this.objA.Length;
        }

        /// <summary>
        /// Get or set an <see cref="Object"/> in this Array.
        /// </summary>
        public Object this[Number index]
        {
            get
            {
                if (!this.dynamic)
                {
                    return null;
                }
                int i = (int)global::System.Math.Floor(index.value);
                if (i > this.objA.Length - 1)
                {
                    return null;
                }
                return this.objA[i];
            }
            set
            {
                if (this.dynamic)
                {
                    //Set the data
                    int i = (int)global::System.Math.Floor(index.value);
                    if (i < 0)
                    {
                        //Just to cause the error
                        this.objA[i] = null;
                    }
                    else if (i > this.objA.Length - 1)
                    {
                        //Expand the array
                        global::System.Array.Resize(ref this.objA, i + 1);
                    }
                    this.objA[i] = value;
                    //Cleanup, make sure that only enough data to be needed exists
                    trimArray();
                }
            }
        }

        private void trimArray()
        {
            int l = this.objA.Length;
            for (; l > 0; l--)
            {
                if (this.objA[l - 1] != null)
                {
                    break;
                }
            }
            if (this.objA.Length != l)
            {
                global::System.Array.Resize(ref this.objA, l);
            }
        }

        /// <summary>
        /// A non-negative integer specifying the number of elements in the array.
        /// </summary>
        public uint length
        {
            get
            {
                return (uint)this.objA.Length;
            }
        }

        #region Primitive handlers

        /// <summary>
        /// Convert a primitive array to a Array.
        /// </summary>
        /// <param name="a">The array value to create the Array with.</param>
        /// <returns>The created Array.</returns>
        public static implicit operator Array(global::System.Array a)
        {
            Array arr = new Array(a.Length);
            a.CopyTo(arr.objA, 0);
            return arr;
        }

        /// <summary>
        /// Convert an Array to a primitive array.
        /// </summary>
        /// <param name="a">The Array to convert to a array.</param>
        /// <returns>The retrieved array.</returns>
        public static implicit operator global::System.Array(Array a)
        {
            return a.objA;
        }

        #endregion
    }
}
