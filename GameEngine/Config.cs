using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.System;
using SFML.Graphics;

namespace GameEngine
{
    public static class Config
    {
        // General
        public const string FONT_ARIAL_PATH = @"Ressources\Fonts\arial.ttf";
        public static readonly Font FONT_ARIAL = new Font(FONT_ARIAL_PATH);
        public const string PATH_IMAGE_TEST = @"Ressources\Images\color_gradient.png";

        public const float ZOOM_IN_FACTOR = 0.875f;
        public const float ZOOM_OUT_FACTOR = 1.125f;

        public const float SCROLL_OFFSET_DEFAULT = 100.0f;

        public static readonly Color COLOR_DEFAULT_CLEAR = Color.White;

        public const string SAVE_PATH = @"C:\workspace\08-dev-ressources\GameEngine_Saves";

        // GameEditor window
        public const string WORLD_EDITOR_WINDOW_TEXT = "C# - SFML - GameEngine - GameEditor";
        public const uint WORLD_EDITOR_WIDTH = 1200;
        public const uint WORLD_EDITOR_HEIGHT = 800;
        /// <summary>
        /// 1 frame each 16 ms => 60 fps
        /// </summary>
        public const long WORLD_EDITOR_FRAME_DURATION = 16;

        public const float WORLD_EDITOR_TOOLBAR_TOP_WIDTH = 800.0f;
        public const float WORLD_EDITOR_TOOLBAR_TOP_HEIGHT = 50.0f;

        public const float WORLD_EDITOR_TOOLBAR_RIGHT_WIDTH = 300.0f;
        public const float WORLD_EDITOR_TOOLBAR_RIGHT_HEIGHT = 550.0f;

        // Textures
        public const string RESSOURCES_ENTITIES_PATH = @"Ressources\Textures\Entities";
        public const string RESSOURCES_GROUND_PATH = @"Ressources\Textures\Ground";
        public const string RESSOURCES_PROP_PATH = @"Ressources\Textures\Prop";

        public const string DEFAULT_TEXTURE_PATH = @"Ressources\texture_not_found.png";

        public const string DEFAULT_TEXTURE_GROUND_PATH = @"Ressources\Textures\Ground\SimpleSummer\Group_6\Top-Down Simple Summer_Ground 53.png";

        // World
        public const float WORLD_TILE_SIZE = 64.0f;
        public const int WORLD_SIZE_X = 50;
        public const int WORLD_SIZE_Y = 50;


        // NpcEntities
        public const int NB_SPRITE_PER_LINE = 8;
        public const int NB_SPRITE_PER_COLUMN = 4;

        public class DefaultColors
        {
            public static readonly Color White = new Color(255, 255, 255);
            public static readonly Color LightGray = new Color(130, 130, 130);
        }
    }
}
