using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SfmlControls.Controls;
using SfmlControls.Window.PartialWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.GameEditor.Windows.WorldEditor.PartialWindows
{
    public class ToolbarRight_AddEntity : SfmlPartialWindow
    {
        private const float SIZE_X = WorldEditorConfig.TB_RIGHT_WIDTH;
        private const float SIZE_Y = WorldEditorConfig.TB_RIGHT_HEIGHT;
        private const float POS_X = WorldEditorConfig.RA_WIDTH;
        private const float POS_Y = WorldEditorConfig.TB_TOP_HEIGHT;

        public ToolbarRight_AddEntity(RenderWindow parentWindow)
            : base(new Vector2f(POS_X, POS_Y), new Vector2f(SIZE_X, SIZE_Y), parentWindow)
        {

        }

        #region OVERRIDE

        public override void DrawOnFrame(ref RenderTexture renderTexture)
        {

        }

        public override Cursor.CursorType GetCursorType(Vector2f globalPosition)
        {
            Vector2f localPosition = ConvertGlobalPositionToLocalPosition(globalPosition);

            SfmlControl? control = GetControl(localPosition);

            if (control == null)
                return Cursor.CursorType.Arrow;

            return control.GetCursorType(localPosition);
        }

        #endregion

        #region EVENTS

        private void ImgEntity_MouseButtonReleased(object? sender, MouseButtonEventArgs e)
        {

        }

        #endregion

        private void SetTextureImages()
        {

        }
    }
}
