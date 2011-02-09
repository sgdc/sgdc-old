using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SGDeB
{
    public enum FormLocation
    {
        Default = 0x0,
        Right = 0x1,
        Bottom = 0x2,
        CenterX = 0x4,
        CenterY = 0x8,

        UpperLeft = Default,
        UpperRight = Right,
        LowerLeft = Bottom,
        LowerRight = UpperRight | LowerLeft,
        Center = CenterX | CenterY
    }

    public static class MDIUtil
    {
        private const int RIGHT_OFFSET = 15;
        private const int BOTTOM_OFFSET = RIGHT_OFFSET * 4;

        public static void AddControlInGeneralLocation(Form parent, Form child, FormLocation loc)
        {
            child.MdiParent = parent;
            System.Drawing.Point location = child.Location;
            if ((loc & FormLocation.Right) == FormLocation.Right)
            {
                location.X = parent.Width - (child.Width + child.Margin.Right + parent.Margin.Right + RIGHT_OFFSET);
            }
            if ((loc & FormLocation.Bottom) == FormLocation.Bottom)
            {
                location.Y = parent.Height - (child.Height + child.Margin.Bottom + parent.Margin.Bottom + BOTTOM_OFFSET);
            }
            if ((loc & FormLocation.CenterX) == FormLocation.CenterX)
            {
                location.X = ((parent.Width - parent.Margin.Horizontal) / 2) - ((child.Width - child.Margin.Horizontal) / 2);
            }
            if ((loc & FormLocation.CenterY) == FormLocation.CenterY)
            {
                location.Y = ((parent.Height - parent.Margin.Vertical) / 2) - ((child.Height - child.Margin.Vertical) / 2);
            }
            child.Location = location;
            child.Show();
        }

        public static void SetTitle(string game, string formName, Form form)
        {
            form.Text = string.Format("[{0}] {1}", game, formName);
        }
    }
}
