using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGDeContent.DataTypes.Sprites.SVG
{
    public enum AnimationType
    {
        Discrete,
        Linear,
        Paced,
        Spline,
        Unrelated
    }

    public class AnimatableComponent
    {
        public List<object> animationObjs;
        public List<TimeSpan> times;
        public AnimationType aniType;

        public void Add(object value, TimeSpan time)
        {
            if (animationObjs == null)
            {
                animationObjs = new List<object>();
                times = new List<TimeSpan>();
            }
            animationObjs.Add(value);
            times.Add(time);
        }

        //TODO: How do we handle spines, linear tracks, etc?

        //TODO: Many more components to animation
    }
}
