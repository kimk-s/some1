using System;
using System.Drawing;
using R3;

namespace Some1.Resources
{
    public static partial class R
    {
        private static readonly StringManager _stringManager = new();
        private static readonly ReactiveProperty<Culture> _culture = new(Some1.Culture.en_US);
        private static readonly ReactiveProperty<Theme> _theme = new();

        public static ReadOnlyReactiveProperty<Culture> Culture => _culture;

        public static ReadOnlyReactiveProperty<Theme> Theme => _theme;

        public static void SetCulture(Culture value)
        {
            if (value == Some1.Culture.None)
            {
                value = Some1.Culture.en_US;
            }

            _culture.Value = value;
        }

        public static void SetTheme(Theme value)
        {
            _theme.Value = value;
        }

        public static string GetString(string id)
        {
            return GetString(id, Culture.CurrentValue);
        }

        public static string GetString(string id, Culture culture)
        {
            return _stringManager.Get(id, culture);
        }

        public static string GetFilePath(string id)
        {
            return GetFilePath(id, Culture.CurrentValue);
        }

        public static string GetFilePath(string id, Culture culture)
        {
            return culture == Some1.Culture.en_US ? id : $"{id}.{culture.GetName()}";
        }

        public static Color GetColor(ColorId id)
        {
            return GetColor(id, Theme.CurrentValue);
        }

        public static Color GetColor(ColorId id, Theme theme) => theme switch
        {
            Some1.Theme.Light => id switch
            {
                ColorId.PaletteA1 => Color.FromArgb(0xffffff),
                ColorId.PaletteA2 => Color.FromArgb(0xf5f5f5),
                ColorId.PaletteA3 => Color.FromArgb(0xeeeeee),
                ColorId.PaletteA4 => Color.FromArgb(0xe0e0e0),
                ColorId.PaletteA5 => Color.FromArgb(0xbdbdbd),

                ColorId.PaletteB1 => Color.FromArgb(0x000000),
                ColorId.PaletteB2 => Color.FromArgb(0x424242),
                ColorId.PaletteB3 => Color.FromArgb(0x616161),
                ColorId.PaletteB4 => Color.FromArgb(0x757575),
                ColorId.PaletteB5 => Color.FromArgb(0x9e9e9e),

                ColorId.PaletteC1 => Color.FromArgb(0x2196f3),
                ColorId.PaletteC2 => Color.FromArgb(0x64b5f6),
                ColorId.PaletteC3 => Color.FromArgb(0x90caf9),
                ColorId.PaletteC4 => Color.FromArgb(0xbbdefb),
                ColorId.PaletteC5 => Color.FromArgb(0xe3f2fd),

                ColorId.EnergyHealthMine => Color.FromArgb(0x76ff03),
                ColorId.EnergyHealthAlly => Color.FromArgb(0x00b0ff),
                ColorId.EnergyHealthEnemy => Color.FromArgb(0xff1744),
                ColorId.EnergyMana => Color.FromArgb(0xffea00),
                ColorId.EnergyStamina => Color.FromArgb(0xff9100),

                ColorId.Blocker => Color.FromArgb(0x000000),

                _ => throw new InvalidOperationException(),
            },
            Some1.Theme.Dark => id switch
            {
                ColorId.PaletteA1 => Color.FromArgb(0x000000),
                ColorId.PaletteA2 => Color.FromArgb(0x424242),
                ColorId.PaletteA3 => Color.FromArgb(0x616161),
                ColorId.PaletteA4 => Color.FromArgb(0x757575),
                ColorId.PaletteA5 => Color.FromArgb(0x9e9e9e),

                ColorId.PaletteB1 => Color.FromArgb(0xffffff),
                ColorId.PaletteB2 => Color.FromArgb(0xf5f5f5),
                ColorId.PaletteB3 => Color.FromArgb(0xeeeeee),
                ColorId.PaletteB4 => Color.FromArgb(0xe0e0e0),
                ColorId.PaletteB5 => Color.FromArgb(0xbdbdbd),

                ColorId.PaletteC1 => Color.FromArgb(0x2196f3),
                ColorId.PaletteC2 => Color.FromArgb(0x1e88e5),
                ColorId.PaletteC3 => Color.FromArgb(0x1976d2),
                ColorId.PaletteC4 => Color.FromArgb(0x1565c0),
                ColorId.PaletteC5 => Color.FromArgb(0x0d47a1),

                ColorId.EnergyHealthMine => Color.FromArgb(0x76ff03),
                ColorId.EnergyHealthAlly => Color.FromArgb(0x00b0ff),
                ColorId.EnergyHealthEnemy => Color.FromArgb(0xff1744),
                ColorId.EnergyMana => Color.FromArgb(0xffea00),
                ColorId.EnergyStamina => Color.FromArgb(0xff9100),

                ColorId.Blocker => Color.FromArgb(0x333333),

                _ => throw new InvalidOperationException(),
            },
            _ => throw new InvalidOperationException(),
        };
    }
}
