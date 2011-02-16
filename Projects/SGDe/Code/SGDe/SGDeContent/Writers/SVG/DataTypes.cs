using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGDeContent.Writers.SVG
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

    public struct SpecificNumber
    {
        public float Number;
        public NumberType Type;

        public SpecificNumber(float value, NumberType type)
        {
            this.Number = value;
            this.Type = type;
        }

        public SpecificNumber ConvertTo(NumberType type)
        {
            if (this.Type == type || type == NumberType.Precent || this.Type == NumberType.Precent || type == NumberType.Pixel || this.Type == NumberType.Pixel)
            {
                //Either type conversion is not needed or it's arbitrary
                return this;
            }
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
            return default(SpecificNumber);
        }
    }
}
