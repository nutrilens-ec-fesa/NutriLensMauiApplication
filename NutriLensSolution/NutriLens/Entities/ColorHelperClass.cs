using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutriLens.Entities
{
    public static class ColorHelperClass
    {

        private static Color Azure { get => Color.FromRgb(0xF0, 0xFF, 0xFF); }
        private static Color Black { get => Color.FromRgb(0x00, 0x00, 0x00); }
        private static Color Pink { get => Color.FromRgb(0xFF, 0xC0, 0xCB); }
        private static Color Red { get => Color.FromRgb(0xFF, 0x00, 0x00); }

        public static Color ValidFieldColor { get => Black; }
        public static Color InvalidFieldColor { get => Red; }
    }
}
