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
        private static Color DarkGreen { get => Color.FromRgb(0x00, 0x80, 0x00); }
        private static Color Orange { get => Color.FromRgb(0xFF, 0xA5, 0x00); }
        private static Color OrangeRed { get => Color.FromRgb(0xFF, 0x45, 0x00); }
        private static Color Gray { get => Color.FromRgb(0x80, 0x80, 0x80); }
        private static Color Tan { get => Color.FromRgb(0xD2, 0xB4, 0x8C); }
        private static Color LightBlue { get => Color.FromRgb(0xAD, 0xD8, 0xE6); }
        private static Color DarkMagenta { get => Color.FromRgb(0xFF, 0x00, 0x49); }
        private static Color Purple { get => Color.FromRgb(0xFF, 0x00, 0xE1); }
        private static Color Praegressus { get => Color.FromRgb(0x68, 0x00, 0xFF); }
        private static Color Avocado { get => Color.FromRgb(0xD5, 0xFF, 0x00); }
        private static Color Gold { get => Color.FromRgb(0xFF, 0xC5, 0x00); }
        private static Color BabyBlue { get => Color.FromRgb(0x05, 0x93, 0xFF); }

        public static Color ValidFieldColor { get => Black; }
        public static Color InvalidFieldColor { get => Red; }

        public static Color ProteinColor { get => DarkGreen; }
        public static Color CarbohydratesColor { get => Orange; }
        public static Color CaloriesColor { get => OrangeRed; }
        public static Color FatColor { get => Praegressus; }
        public static Color FibersColor { get => Gold; }
        public static Color SodiumColor{ get => BabyBlue; }
        public static Color CholesterolColor { get => DarkMagenta; }
        public static Color CalciumColor { get => Tan; }
        public static Color IronColor { get => LightBlue; }


    }
}
