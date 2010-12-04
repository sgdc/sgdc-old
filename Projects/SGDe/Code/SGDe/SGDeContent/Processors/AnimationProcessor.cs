using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework.Content.Pipeline;
using SGDeContent.DataTypes;
using Microsoft.Xna.Framework;

namespace SGDeContent.Processors
{
    public class AnimationProcessor
    {
        public static Animation Process(XmlElement input, ContentProcessorContext context)
        {
            Animation animation = new Animation();
            string innerText = SGDEProcessor.GetInnerText(input);
            if (innerText.Equals("Global"))
            {
                animation.BuiltIn = false;
                animation.ID = int.Parse(input.Attributes["ID"].Value);
            }
            else if (innerText.Equals("Local"))
            {
                animation.BuiltIn = true;

                #region Local

                animation.Sets = new List<AnimationSet>();
                int index = 0;
                foreach (XmlNode node in input)
                {
                    if (!(node is XmlElement))
                    {
                        continue;
                    }
                    XmlElement element = node as XmlElement;
                    if (element.Name.Equals("AnimationSet"))
                    {
                        #region AnimationSet

                        AnimationSet set = new AnimationSet();
                        set.Index = index++;
                        XmlAttribute at = element.Attributes["Default"];
                        if (at != null)
                        {
                            set.Default = bool.Parse(at.Value);
                        }
                        at = element.Attributes["FPS"];
                        if (at != null)
                        {
                            set.FPS = float.Parse(at.Value);
                        }
                        at = element.Attributes["DID"];
                        if (at != null)
                        {
                            set.DevID(at, set);
                        }
                        int fcount = 0;
                        //This happens twice, first gets the frame count...
                        foreach (XmlElement frame in element)
                        {
                            if (frame.Name.Equals("Frame"))
                            {
                                at = frame.Attributes["FrameCount"];
                                if (at != null)
                                {
                                    int c = int.Parse(at.Value);
                                    if (c < 1)
                                    {
                                        context.Logger.LogWarning(null, null, Messages.Animation_FPSTooLow, c);
                                    }
                                    else
                                    {
                                        fcount += c;
                                    }
                                }
                                else
                                {
                                    fcount++;
                                }
                            }
                        }
                        //...second actually processes them.
                        set.Frames = new List<AnimationFrame>(fcount);
                        foreach (XmlElement frame in element)
                        {
                            if (frame.Name.Equals("Frame"))
                            {
                                #region Frame

                                AnimationFrame aframe = null;
                                at = frame.Attributes["Continue"];
                                if (at != null)
                                {
                                    bool value = bool.Parse(at.Value);
                                    if (value)
                                    {
                                        if (set.Frames.Count == 0)
                                        {
                                            context.Logger.LogWarning(null, null, Messages.Animation_ContinueAnimationOnFirstFrame);
                                        }
                                        else
                                        {
                                            aframe = new AnimationFrame(set.Frames[set.Frames.Count - 1]);
                                        }
                                    }
                                }
                                else
                                {
                                    aframe = new AnimationFrame();
                                }
                                at = frame.Attributes["FrameCount"];
                                if (at != null)
                                {
                                    int c = int.Parse(at.Value);
                                    if (c >= 1)
                                    {
                                        while (c-- > 0)
                                        {
                                            set.Frames.Add(aframe); //If one changes, they all change
                                        }
                                    }
                                    else
                                    {
                                        set.Frames.Add(aframe);
                                    }
                                }
                                else
                                {
                                    set.Frames.Add(aframe);
                                }
                                at = frame.Attributes["Effect"];
                                if (at != null)
                                {
                                    string[] values = at.Value.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                                    aframe.Effect = Microsoft.Xna.Framework.Graphics.SpriteEffects.None;
                                    Microsoft.Xna.Framework.Graphics.SpriteEffects temp;
                                    for (int i = 0; i < values.Length; i++)
                                    {
                                        if (!Enum.TryParse<Microsoft.Xna.Framework.Graphics.SpriteEffects>(values[i].Trim(), out temp))
                                        {
                                            context.Logger.LogWarning(null, null, Messages.Animation_InvalidSpriteEffect, values[i].Trim());
                                        }
                                        else
                                        {
                                            aframe.Effect |= temp;
                                        }
                                    }

                                    if (aframe.Effect != Microsoft.Xna.Framework.Graphics.SpriteEffects.None)
                                    {
                                        aframe.Used |= SGDE.Content.Readers.AnimationReader.EffectUsed;
                                    }
                                    else
                                    {
                                        aframe.Used &= (byte)((~SGDE.Content.Readers.AnimationReader.EffectUsed) & 0xFF);
                                    }
                                }
                                at = frame.Attributes["Color"];
                                if (at != null)
                                {
                                    try
                                    {
                                        /* Bug causes this not to be set properly. Try 0xFF008000 for example. Supposed to be A:255, R:0, G:128, B:0. Instead green is 255.
                                        Microsoft.Xna.Framework.Graphics.PackedVector.Byte4 packedVector = new Microsoft.Xna.Framework.Graphics.PackedVector.Byte4(); //Preperation
                                        packedVector.PackedValue = Convert.ToUInt32(at.Value, 16); //Keep it clean
                                        aframe.Color = new Color(packedVector.ToVector4()); //Build it
                                         */
                                        Color col = new Color(); //Build
                                        col.PackedValue = Convert.ToUInt32(at.Value, 16); //Keep it clean
                                        aframe.Color = col;
                                        aframe.Used |= SGDE.Content.Readers.AnimationReader.ColorUsed;
                                    }
                                    catch
                                    {
                                        context.Logger.LogWarning(null, null, Messages.Animation_InvalidColor, at.Value);
                                    }
                                }
                                at = frame.Attributes["Frame"];
                                if (at != null)
                                {
                                    string[] values = at.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                    if (values.Length != 4)
                                    {
                                        context.Logger.LogWarning(null, null, Messages.Animation_InvalidRegion);
                                    }
                                    else
                                    {
                                        int[] ivals = new int[4];
                                        bool error = false;
                                        for (int i = 0; i < 4; i++)
                                        {
                                            try
                                            {
                                                ivals[i] = int.Parse(values[i].Trim());
                                            }
                                            catch
                                            {
                                                error = true;
                                                context.Logger.LogWarning(null, null, Messages.CannotParseValue);
                                            }
                                        }
                                        if (!error)
                                        {
                                            aframe.Region = new Rectangle(ivals[0], ivals[1], ivals[2], ivals[3]);
                                            aframe.Used |= SGDE.Content.Readers.AnimationReader.RegionUsed;
                                        }
                                    }
                                }
                                at = frame.Attributes["Origin"];
                                if (at != null)
                                {
                                    string[] values = at.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                    if (values.Length != 2)
                                    {
                                        context.Logger.LogWarning(null, null, Messages.Animation_InvalidOrigin);
                                    }
                                    else
                                    {
                                        float[] fvals = new float[2];
                                        bool error = false;
                                        for (int i = 0; i < 2; i++)
                                        {
                                            try
                                            {
                                                fvals[i] = float.Parse(values[i].Trim());
                                            }
                                            catch
                                            {
                                                error = true;
                                                context.Logger.LogWarning(null, null, Messages.CannotParseValue);
                                            }
                                        }
                                        if (!error)
                                        {
                                            aframe.Origin = new Vector2(fvals[0], fvals[1]);
                                            if (aframe.Origin.X != 0 || aframe.Origin.Y != 0)
                                            {
                                                aframe.Used |= SGDE.Content.Readers.AnimationReader.OriginUsed;
                                            }
                                            else
                                            {
                                                aframe.Used &= (byte)((~SGDE.Content.Readers.AnimationReader.OriginUsed) & 0xFF);
                                            }
                                        }
                                    }
                                }
                                at = frame.Attributes["Scale"];
                                if (at != null)
                                {
                                    string[] values = at.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                    if (values.Length != 2)
                                    {
                                        context.Logger.LogWarning(null, null, Messages.Animation_InvalidScale);
                                    }
                                    else
                                    {
                                        float[] fvals = new float[2];
                                        bool error = false;
                                        for (int i = 0; i < 2; i++)
                                        {
                                            try
                                            {
                                                fvals[i] = float.Parse(values[i].Trim());
                                            }
                                            catch
                                            {
                                                error = true;
                                                context.Logger.LogWarning(null, null, Messages.CannotParseValue);
                                            }
                                        }
                                        if (!error)
                                        {
                                            aframe.Scale = new Vector2(fvals[0], fvals[1]);
                                            if (aframe.Scale.X != 1 || aframe.Scale.Y != 1)
                                            {
                                                aframe.Used |= SGDE.Content.Readers.AnimationReader.ScaleUsed;
                                            }
                                            else
                                            {
                                                aframe.Used &= (byte)((~SGDE.Content.Readers.AnimationReader.ScaleUsed) & 0xFF);
                                            }
                                        }
                                    }
                                }
                                at = frame.Attributes["Rotation"];
                                if (at != null)
                                {
                                    bool radian = false;
                                    string value = at.Value;
                                    at = frame.Attributes["RotationFormat"];
                                    if (at != null)
                                    {
                                        if (at.Value.Equals("Radian"))
                                        {
                                            radian = true;
                                        }
                                        else if (at.Value.Equals("Degree"))
                                        {
                                            radian = false;
                                        }
                                        else
                                        {
                                            context.Logger.LogWarning(null, null, Messages.Animation_UnknownRotationFormat, at.Value);
                                        }
                                    }
                                    if (radian)
                                    {
                                        double radianVal;
                                        try
                                        {
                                            radianVal = double.Parse(value.Trim());
                                            radianVal %= (2 * Math.PI);
                                            if (radianVal != 0)
                                            {
                                                aframe.Used |= SGDE.Content.Readers.AnimationReader.RotationUsed;
                                                aframe.Rotation = (float)radianVal;
                                            }
                                            else if (aframe.Rotation != 0)
                                            {
                                                aframe.Used &= (byte)((~SGDE.Content.Readers.AnimationReader.RotationUsed) & 0xFF);
                                            }
                                        }
                                        catch
                                        {
                                            XmlAttribute tempnode = frame.OwnerDocument.CreateAttribute("Rotation");
                                            //Replace "Pi" with "Math.PI" for easier execution
                                            int nindex = value.IndexOf("PI", StringComparison.OrdinalIgnoreCase);
                                            if (nindex >= 0)
                                            {
                                                string piValue = value.Substring(nindex, 2);
                                                value = value.Replace(piValue, "Math.PI");
                                            }
                                            tempnode.Value = value;

                                            SGDeContent.DataTypes.Code.Code code = CodeProcessor.Process(tempnode, context);
                                            if (code.Constant)
                                            {
                                                radianVal = Convert.ToDouble(code.ConstantValue);
                                                if (radianVal != 0)
                                                {
                                                    aframe.Used |= SGDE.Content.Readers.AnimationReader.RotationUsed;
                                                    aframe.Rotation = (float)radianVal;
                                                }
                                                else if (aframe.Rotation != 0)
                                                {
                                                    aframe.Used &= (byte)((~SGDE.Content.Readers.AnimationReader.RotationUsed) & 0xFF);
                                                }
                                            }
                                            else
                                            {
                                                context.Logger.LogWarning(null, null, Messages.Animation_InvalidRadian);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        try
                                        {
                                            double degree = double.Parse(value.Trim());
                                            degree %= 360;
                                            if (degree != 0)
                                            {
                                                aframe.Used |= SGDE.Content.Readers.AnimationReader.RotationUsed;
                                                aframe.Rotation = (float)((degree * Math.PI) / 180.0); //Could use MathHelper but want higher precision first
                                            }
                                            else if (aframe.Rotation != 0)
                                            {
                                                aframe.Used &= (byte)((~SGDE.Content.Readers.AnimationReader.RotationUsed) & 0xFF);
                                            }
                                        }
                                        catch
                                        {
                                            context.Logger.LogWarning(null, null, Messages.Animation_InvalidDegree);
                                        }
                                    }
                                }
                                set.Used |= aframe.Used;

                                #endregion
                            }
                        }
                        if (set.Frames.Count > 0)
                        {
                            animation.Sets.Add(set);
                        }

                        #endregion
                    }
                }

                #endregion
            }
            else
            {
                throw new InvalidContentException(string.Format(Messages.Animation_UnknownType, innerText));
            }
            return animation;
        }
    }
}
