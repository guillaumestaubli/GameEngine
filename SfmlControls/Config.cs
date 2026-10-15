using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfmlControls
{
    internal static class Config
    {
        internal const string FONT_PATH_ARIAL = @"Ressources\arial.ttf";
        internal static readonly Font FONT_ARIAL = new Font(FONT_PATH_ARIAL);
        internal const uint DEFAULT_CHARACTER_SIZE = 12u;

        internal static readonly Font DEFAULT_FONT = FONT_ARIAL;
        internal static readonly Color DEFAULT_BACKGROUND_COLOR = Color.White;
        internal static readonly Color DEFAULT_BACKGROUND_COLOR_TEXTBOX = new Color(220, 220, 220);
        internal static readonly Color DEFAULT_BORDER_COLOR = Color.Black;
        internal static readonly Color DEFAULT_TEXT_COLOR = Color.Black;
        internal static readonly Color DEFAULT_CLEAR_COLOR = Color.White;

        internal const float ZOOM_IN_FACTOR = 0.875f;
        internal const float ZOOM_OUT_FACTOR = 1.125f;

        internal const float DEFAULT_SCROLL_OFFSET = 100.0f;

        internal const long DEFAULT_FRAME_DURATION = 16; // [ms] => 1000 / 16 = 62.5 fps
    }
}
