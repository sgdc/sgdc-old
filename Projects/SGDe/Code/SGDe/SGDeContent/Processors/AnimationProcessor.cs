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
        public static Animation Process(XmlElement input, double version, ContentProcessorContext context)
        {
            Animation animation = new Animation();
            string innerText = SGDEProcessor.GetInnerText(input);
            if (ContentTagManager.TagMatches("ANIMATION_GLOBAL", innerText, version))
            {
                animation.BuiltIn = false;
                animation.ID = int.Parse(ContentTagManager.GetXMLAtt("GENERAL_ID", version, input).Value);
            }
            else if (ContentTagManager.TagMatches("ANIMATION_LOCAL", innerText, version))
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
                    if (ContentTagManager.TagMatches("ANIMATION_LOCAL_SET", element.Name, version))
                    {
                        #region AnimationSet

                        AnimationSet set = new AnimationSet();
                        set.Index = index++;
                        XmlAttribute at = ContentTagManager.GetXMLAtt("ANIMATION_LOCAL_DEFAULT", version, element);
                        if (at != null)
                        {
                            set.Default = bool.Parse(at.Value);
                        }
                        at = ContentTagManager.GetXMLAtt("ANIMATION_LOCAL_FPS", version, element);
                        if (at != null)
                        {
                            set.FPS = float.Parse(at.Value);
                        }
                        at = ContentTagManager.GetXMLAtt("GENERAL_DEVELOPER_ID", version, element);
                        if (at != null)
                        {
                            set.DevID(at, set);
                        }
                        int fcount = 0;
                        //This happens twice, first gets the frame count...
                        foreach (XmlElement frame in element)
                        {
                            if (ContentTagManager.TagMatches("ANIMATION_LOCAL_FRAME", frame.Name, version))
                            {
                                at = ContentTagManager.GetXMLAtt("ANIMATION_LOCAL_FRAME_FRAMECOUNT", version, frame);
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
                        Rectangle? region = null;
                        foreach (XmlElement frame in element)
                        {
                            if (ContentTagManager.TagMatches("ANIMATION_LOCAL_FRAME", frame.Name, version))
                            {
                                #region Frame

                                AnimationFrame aframe = null;
                                at = ContentTagManager.GetXMLAtt("ANIMATION_LOCAL_FRAME_CONTINUE", version, frame);
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
                                at = ContentTagManager.GetXMLAtt("ANIMATION_LOCAL_FRAME_FRAMECOUNT", version, frame);
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
                                at = ContentTagManager.GetXMLAtt("ANIMATION_LOCAL_FRAME_EFFECT", version, frame);
                                if (at != null)
                                {
                                    aframe.Effect = Utils.ParseEnum<Microsoft.Xna.Framework.Graphics.SpriteEffects>(at.Value, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, context.Logger);

                                    if (aframe.Effect != Microsoft.Xna.Framework.Graphics.SpriteEffects.None)
                                    {
                                        aframe.Used |= SGDE.Content.Readers.AnimationReader.EffectUsed;
                                    }
                                    else
                                    {
                                        aframe.Used &= (byte)((~SGDE.Content.Readers.AnimationReader.EffectUsed) & 0xFF);
                                    }
                                }
                                at = ContentTagManager.GetXMLAtt("ANIMATION_LOCAL_FRAME_COLOR", version, frame);
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
                                at = ContentTagManager.GetXMLAtt("ANIMATION_LOCAL_FRAME_FRAME", version, frame);
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
                                        if (region.HasValue)
                                        {
                                            if (ivals[2] != region.Value.Width || ivals[3] != region.Value.Height)
                                            {
                                                error = true;
                                                context.Logger.LogWarning(null, null, Messages.Animation_RegionMustRemainSame);
                                            }
                                        }
                                        if (!error)
                                        {
                                            aframe.Region = new Rectangle(ivals[0], ivals[1], ivals[2], ivals[3]);
                                            aframe.Used |= SGDE.Content.Readers.AnimationReader.RegionUsed;
                                            if (!region.HasValue)
                                            {
                                                region = new Rectangle(0, 0, aframe.Region.Width, aframe.Region.Height); //Only care about height and width
                                            }
                                        }
                                    }
                                }
                                at = ContentTagManager.GetXMLAtt("ANIMATION_LOCAL_FRAME_ORIGIN", version, frame);
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
                                at = ContentTagManager.GetXMLAtt("ANIMATION_LOCAL_FRAME_SCALE", version, frame);
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
                                at = ContentTagManager.GetXMLAtt("ANIMATION_LOCAL_FRAME_ROTATION", version, frame);
                                if (at != null)
                                {
                                    bool radian = false;
                                    string value = at.Value;
                                    at = ContentTagManager.GetXMLAtt("ANIMATION_LOCAL_FRAME_ROTATION_FORMAT", version, frame);
                                    if (at != null)
                                    {
                                        if (ContentTagManager.TagMatches("ANIMATION_LOCAL_FRAME_ROTATION_FORMAT_RADIAN", at.Value, version))
                                        {
                                            radian = true;
                                        }
                                        else if (ContentTagManager.TagMatches("ANIMATION_LOCAL_FRAME_ROTATION_FORMAT_DEGREE", at.Value, version))
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
                                            XmlAttribute tempnode = frame.OwnerDocument.CreateAttribute(ContentTagManager.GetTagValue("ANIMATION_LOCAL_FRAME_ROTATION", version));
                                            //Replace "Pi" with "Math.PI" for easier execution
                                            int nindex = value.IndexOf("PI", StringComparison.OrdinalIgnoreCase);
                                            if (nindex >= 0)
                                            {
                                                string piValue = value.Substring(nindex, 2);
                                                value = value.Replace(piValue, CodeProcessor.AbsReference("Math.PI"));
                                            }
                                            tempnode.Value = value;

                                            SGDeContent.DataTypes.Code.Code code = CodeProcessor.Process(tempnode, version, context);
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
