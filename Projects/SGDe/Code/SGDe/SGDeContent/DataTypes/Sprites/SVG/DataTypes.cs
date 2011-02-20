using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGDeContent.DataTypes.Sprites.SVG
{
    public enum NumberType
    {
        Pixel,
        Inch,
        CentiMeter,
        MilliMeter,
        Point,
        Pica,
        Precent
    }

    #region SpecificNumber

    public struct SpecificNumber
    {
        public float Number;
        public NumberType Type;

        public SpecificNumber(float value, NumberType type)
        {
            this.Number = value;
            this.Type = type;
        }

        /// <summary>
        /// Parse a SpecificNumber, if no type is applied then it is assumed to be pixels. If it can't parse then it throws an exception.
        /// </summary>
        public static SpecificNumber Parse(string value)
        {
            SpecificNumber num;
            bool good = TryParse(value, out num);
            if (!good)
            {
                throw new FormatException();
            }
            return num;
        }

        /// <summary>
        /// Parse a SpecificNumber, if no type is applied then it is assumed to be pixels.
        /// </summary>
        public static bool TryParse(string value, out SpecificNumber num)
        {
            float fval;
            if (float.TryParse(value, out fval))
            {
                num = new SpecificNumber(fval, NumberType.Pixel);
                return true;
            }
            int index = 0;
            char c = value[index++];
            while ((char.IsDigit(c) || char.ToLower(c) == 'e' || c == '-' || c == '.') && (index < value.Length))
            {
                c = value[index++];
            }
            index--;
            if (!float.TryParse(value.Substring(0, index), out fval))
            {
                num = default(SpecificNumber);
                return false;
            }
            if (value[index] == '%')
            {
                num = new SpecificNumber(fval, NumberType.Precent);
                return true;
            }
            else
            {
                switch (value.Substring(index, 2).ToLower())
                {
                    case "in":
                        num = new SpecificNumber(fval, NumberType.Inch);
                        return true;
                    case "cm":
                        num = new SpecificNumber(fval, NumberType.CentiMeter);
                        return true;
                    case "mm":
                        num = new SpecificNumber(fval, NumberType.MilliMeter);
                        return true;
                    case "pt":
                        num = new SpecificNumber(fval, NumberType.Point);
                        return true;
                    case "pc":
                        num = new SpecificNumber(fval, NumberType.Pica);
                        return true;
                    case "px":
                        num = new SpecificNumber(fval, NumberType.Pixel);
                        return true;
                }
            }
            num = default(SpecificNumber);
            return false;
        }

        public SpecificNumber ConvertTo(NumberType type)
        {
            return ConvertTo(type, -1);
        }

        /// <param name="ratio">//TODO</param>
        public SpecificNumber ConvertTo(NumberType type, float ratio)
        {
            if (this.Type == type)
            {
                return this;
            }
            else if (this.Type == NumberType.Precent || this.Type == NumberType.Pixel)
            {
                if (ratio <= 0)
                {
                    return this;
                }
                //TODO: Precent and Pixel
                /*
                switch (this.Type)
                {
                    case NumberType.Precent:
                        if (type == NumberType.Pixel)
                        {
                            return new SpecificNumber(this.Number / ratio, NumberType.Pixel);
                        }
                        else
                        {
                            return new SpecificNumber(this.Number / ratio, NumberType.Inch).ConvertTo(type);
                        }
                    case NumberType.Pixel:
                        if (type == NumberType.Precent)
                        {
                            return new SpecificNumber(this.Number / ratio, NumberType.Precent);
                        }
                        else
                        {
                            return new SpecificNumber(this.Number / ratio, NumberType.Inch).ConvertTo(type);
                        }
                }
                 */
            }
            else
            {
                switch (type)
                {
                    case NumberType.Inch:
                        switch (this.Type)
                        {
                            case NumberType.CentiMeter:
                                return new SpecificNumber((this.Number * 50) / 127, type);
                            case NumberType.MilliMeter:
                                return new SpecificNumber((this.Number * 5) / 127, type);
                            case NumberType.Point:
                                return new SpecificNumber(this.Number / 127, type);
                            case NumberType.Pica:
                                return new SpecificNumber(this.Number / 6, type);
                        }
                        break;
                    case NumberType.CentiMeter:
                        switch (type)
                        {
                            case NumberType.Inch:
                                return new SpecificNumber((this.Number * 127) / 50, type);
                            case NumberType.MilliMeter:
                                return new SpecificNumber(this.Number / 10, type);
                            case NumberType.Point:
                                return new SpecificNumber(this.Number * 50, type);
                            case NumberType.Pica:
                                return new SpecificNumber(((this.Number * 50) / 127) * 6, type);
                        }
                        break;
                    case NumberType.MilliMeter:
                        switch (type)
                        {
                            case NumberType.Inch:
                                return new SpecificNumber((this.Number * 127) / 5, type);
                            case NumberType.CentiMeter:
                                return new SpecificNumber(this.Number * 10, type);
                            case NumberType.Point:
                                return new SpecificNumber(this.Number * 5, type);
                            case NumberType.Pica:
                                return new SpecificNumber(((this.Number * 5) / 127) * 6, type);
                        }
                        break;
                    case NumberType.Point:
                        switch (type)
                        {
                            case NumberType.Inch:
                                return new SpecificNumber(this.Number * 127, type);
                            case NumberType.CentiMeter:
                                return new SpecificNumber(this.Number / 50, type);
                            case NumberType.MilliMeter:
                                return new SpecificNumber(this.Number / 5, type);
                            case NumberType.Pica:
                                return new SpecificNumber((this.Number / 127) * 6, type);
                        }
                        break;
                    case NumberType.Pica:
                        switch (type)
                        {
                            case NumberType.Inch:
                                return new SpecificNumber(this.Number * 6, type);
                            case NumberType.CentiMeter:
                                return new SpecificNumber(((this.Number / 6) * 127) / 50, type);
                            case NumberType.MilliMeter:
                                return new SpecificNumber(((this.Number / 6) * 127) / 5, type);
                            case NumberType.Point:
                                return new SpecificNumber((this.Number / 6) * 127, type);
                        }
                        break;
                }
            }
            return default(SpecificNumber);
        }
    }

    #endregion

    #region FRectangle

    public struct FRectangle
    {
        public float X, Y, Width, Height;

        public FRectangle(float x, float y, float width, float height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }
    }

    #endregion
}
