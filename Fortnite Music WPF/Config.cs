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
                createPoint(1058,28),
                createPoint(985, 780),
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
                createPoint(512,36),
                createPoint(909, 1047),
            };
            if (Properties.Settings.Default.stretched)
            {
                MainMenuPoints.Add(stretchpoint(13, 1055)); // compared to menu 4
            }
            else
            {
                MainMenuPoints.Add(createPoint(20, 1043)); // compared to menu 4
            }
            MainMenuColors = new List<System.Drawing.Color>()
            {
                Properties.Settings.Default.menu2,
                Properties.Settings.Default.menu3,
                Properties.Settings.Default.menu4,
            };
            //
            // SETTINGS
            //
            if (Properties.Settings.Default.stretched)
            {
                SettingPoints = new List<System.Drawing.Point>()
                {
                    stretchpoint(1422,8),
                    stretchpoint(1370,8),
                };
            } 
            else
            {
                SettingPoints = new List<System.Drawing.Point>()
                {
                    createPoint(1897, 10),
                    createPoint(1825, 10),
                };
            }
            SettingColors = new List<System.Drawing.Color>()
            {
                Properties.Settings.Default.menu5,
                Properties.Settings.Default.menu6,
            };
            //
            //
            //
            FriendsPoints = new List<System.Drawing.Point>()
            {
                createPoint(783, 21),
                createPoint(3, 30),
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
                stretchpoint(680, 347);
                stretchpoint(805, 240);
            }
            else
            {
                createPoint(911, 251);
                createPoint(1087, 271);
            }

            VictoryColors = new List<System.Drawing.Color>()
            {
                Properties.Settings.Default.victory1,
                Properties.Settings.Default.victory2,
            };
        }

        private System.Drawing.Point stretchpoint(int num, int num2)
        {
            return new System.Drawing.Point(Convert.ToInt32(Math.Round(num * (Properties.Settings.Default.ResX / 1440.0))), Convert.ToInt32(Math.Round(num2 * (Properties.Settings.Default.ResY / 1080.0))));
        }

        public System.Drawing.Point createPoint(int x, int y)
        {
            // Creates a point with fort
            return new System.Drawing.Point(Convert.ToInt32(Math.Round(x * Properties.Settings.Default.sfx)), Convert.ToInt32(Math.Round(y * Properties.Settings.Default.sfy)));
        }
    }
}
