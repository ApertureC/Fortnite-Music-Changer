// Config.cs
// Holds information such as the positions of pixels, change data in here and redo menu setup for new positions.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortnite_Music_WPF
{
    class Config
    {
        public List<System.Drawing.Point> TitleMenuPoints;
        public List<System.Drawing.Color> TitleMenuColors;

        public List<System.Drawing.Point> MainMenuPoints;
        public List<System.Drawing.Color> MainMenuColors;

        public List<System.Drawing.Point> SettingPoints;
        public List<System.Drawing.Color> SettingColors;

        public List<System.Drawing.Point> FriendsPoints;
        public List<System.Drawing.Color> FriendsColors;

        public List<System.Drawing.Point> VictoryPoints;
        public List<System.Drawing.Color> VictoryColors;
        public Config()
        {
            // CONFIG - TITLE MENU
            // Holds the title menu's points which it looks for pixel data.
            TitleMenuPoints = new List<System.Drawing.Point>()
            {
                CreatePoint(1058,28),
                CreatePoint(985, 780),
            };

            // Holds the title menu's colors which it compares to pixel data
            TitleMenuColors = new List<System.Drawing.Color>()
            {
                Properties.Settings.Default.title1,
                Properties.Settings.Default.title2,
            };

            // CONFIG - MAIN MENU
            // Holds the main menu's points which it looks for pixel data.
            MainMenuPoints = new List<System.Drawing.Point>()
            {
                CreatePoint(982 ,1051),
                CreatePoint(1497, 1057),
            };
            MainMenuColors = new List<System.Drawing.Color>()
            {
                Properties.Settings.Default.menu2,
                Properties.Settings.Default.menu3,
            };
            //
            // SETTINGS
            //
            /* NO LONGER USEFUL - MAY BE USEFUL IN THE FUTURE 
             * if (Properties.Settings.Default.stretched)
            {
                SettingPoints = new List<System.Drawing.Point>()
                {
                    StretchPoint(1422,8),
                    StretchPoint(1370,8),
                };
            }
            else
            {
                SettingPoints = new List<System.Drawing.Point>()
                {
                    CreatePoint(1894, 8),
                    CreatePoint(1825, 11),
                };
            }
            SettingColors = new List<System.Drawing.Color>()
            {
                Properties.Settings.Default.menu5,
                Properties.Settings.Default.menu6,
            }; */ 
            //
            //
            //
            FriendsPoints = new List<System.Drawing.Point>()
            {
                CreatePoint(783, 21),
                CreatePoint(3, 30),
            };

            FriendsColors = new List<System.Drawing.Color>()
            {
                Properties.Settings.Default.menu7,
                Properties.Settings.Default.menu8,
            };

            // CONFIG - VICTORY
            VictoryPoints = new List<System.Drawing.Point>();

            if (Properties.Settings.Default.stretched)
            {
                VictoryPoints = new List<System.Drawing.Point>()
                {
                    StretchPoint(680, 347),
                    StretchPoint(805, 240),
                };
            }
            else
            {
                VictoryPoints = new List<System.Drawing.Point>()
                {
                    CreatePoint(911, 251),
                    CreatePoint(1087, 271),
                };
            }

            VictoryColors = new List<System.Drawing.Color>()
            {
                Properties.Settings.Default.victory1,
                Properties.Settings.Default.victory2,
            };
        }

        private System.Drawing.Point StretchPoint(int num, int num2) // Creates a point with stretched values
        {
            return new System.Drawing.Point(Convert.ToInt32(Math.Round(num * (Properties.Settings.Default.ResX / 1440.0))), Convert.ToInt32(Math.Round(num2 * (Properties.Settings.Default.ResY / 1080.0))));
        }

        public System.Drawing.Point CreatePoint(int x, int y) // Creates a point.
        {
            // Creates a point with fort
            return new System.Drawing.Point(Convert.ToInt32(Math.Round(x * Properties.Settings.Default.sfx)), Convert.ToInt32(Math.Round(y * Properties.Settings.Default.sfy)));
        }
    }
}
