using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CAT_Library
{
    public static class Colors
    {
        public static void ColorSet(object name, Color color)
        {
            switch (name)
            {
                case PictureBox picture:
                    picture.BackColor = color;
                    break;
                case Button button:
                    button.BackColor = color;
                    break;
            }
        }
    }
}
