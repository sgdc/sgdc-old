using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGDeContent.DataTypes
{
    public class AnimationSet : DeveloperIDContent
    {
        public bool Default;
        public int Index;

        public float FPS;
        public byte Used;
        public List<AnimationFrame> Frames;
    }
}
