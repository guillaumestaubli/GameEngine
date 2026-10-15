using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML;
using SFML.System;
using SFML.Graphics;
using SFML.Window;

using SfmlControls;
using SfmlControls.Controls;
using SfmlControls.Window.PartialWindow;
using GameEngine.Core.World;
using GameEngine.Core.Textures;
using GameEngine.GameEditor.Windows.WorldEditor.DialogChangeGroundTexture;
using GameEngine.Core.Entities;

namespace GameEngine.GameEditor.Windows.WorldEditor.PartialWindows
{
    public class ToolbarRight_EditTile : SfmlPartialWindow
    {
        private const float SIZE_X = WorldEditorConfig.TB_RIGHT_WIDTH;
        private const float SIZE_Y = WorldEditorConfig.TB_RIGHT_HEIGHT;

        private const float POS_X = WorldEditorConfig.RA_WIDTH;
        private const float POS_Y = WorldEditorConfig.TB_TOP_HEIGHT;

        private SfmlLabel _lblTitle;

        private SfmlLabel _lblIndex;
        private SfmlLabel _lblPosition;
        private SfmlLabel _lblSize;
        private SfmlLabel _lblTexture;

        private SfmlTextbox _txtIndex;
        private SfmlTextbox _txtPosition;
        private SfmlTextbox _txtSize;

        private SfmlImage _imgTexture;

        private SfmlButton _btnChangeTexture;

        public ToolbarRight_EditTile(RenderWindow parentWindow) : base(new Vector2f(POS_X, POS_Y), new Vector2f(SIZE_X, SIZE_Y), parentWindow)
        {
            #region CONTROLS

            Vector2f lbl_pos = new Vector2f(5.0f, 5.0f);
            Vector2f lbl_inter = new Vector2f(0.0f, 30.0f);

            _lblTitle = new SfmlLabel("lblTitle", "Tile Properties :", lbl_pos);
            _lblTitle.Styles = Text.Styles.Bold | Text.Styles.Underlined;
            this.AddControl(_lblTitle);

            lbl_pos += lbl_inter;

            _lblIndex = new SfmlLabel("lblIndex", "Index :", lbl_pos);
            this.AddControl(_lblIndex);

            lbl_pos += lbl_inter;

            _lblPosition = new SfmlLabel("lblPosition", "Position :", lbl_pos);
            this.AddControl(_lblPosition);

            lbl_pos += lbl_inter;

            _lblSize = new SfmlLabel("lblSize", "Size :", lbl_pos);
            this.AddControl(_lblSize);

            lbl_pos += lbl_inter;

            _lblTexture = new SfmlLabel("lblTexture", "EntityTexture :", lbl_pos);
            this.AddControl(_lblTexture);

            Vector2f txt_size = new Vector2f(180.0f, 20.0f);
            Vector2f txt_pos = new Vector2f(100.0f, 33.0f);
            Vector2f txt_inter = new Vector2f(0.0f, 30.0f);

            _txtIndex = new SfmlTextbox("txtIndex", txt_pos, txt_size, string.Empty);
            _txtIndex.IsDisabled = true;
            this.AddControl(_txtIndex);

            txt_pos += txt_inter;

            _txtPosition = new SfmlTextbox("txtPosition", txt_pos, txt_size, string.Empty);
            _txtPosition.IsDisabled = true;
            this.AddControl(_txtPosition);

            txt_pos += txt_inter;

            _txtSize = new SfmlTextbox("txtSize", txt_pos, txt_size, string.Empty);
            _txtSize.IsDisabled = true;
            this.AddControl(_txtSize);

            txt_pos += txt_inter;

            _imgTexture = new SfmlImage("imgTexture", txt_pos, new Vector2f(100.0f, 100.0f), TextureManager.Texture_NotFound.Image);
            _imgTexture.HasBorder = true;
            this.AddControl(_imgTexture);

            txt_pos += new Vector2f(0.0f, 105.0f);

            _btnChangeTexture = new SfmlButton("btnChangeTexture", "Change texture", txt_pos, new Vector2f(100.0f, 20.0f));
            _btnChangeTexture.MouseButtonReleased += BtnChangeTexture_MouseButtonReleased;
            this.AddControl(_btnChangeTexture);

            #endregion

            UpdateControlsDisplayValues();
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

        private static void BtnChangeTexture_MouseButtonReleased(object? sender, MouseButtonEventArgs e)
        {
            // TODO : Show dialog to select a new ground texture for the tile

            Dialog_ChangeGroundTexture dialog_ChangeGroundTexture = new Dialog_ChangeGroundTexture();

            dialog_ChangeGroundTexture.Run();
        }

        #endregion

        public void UpdateControlsDisplayValues()
        {
            if (GameEditor.CurrentTileForEdit == null)
            {
                _txtIndex.TextStr = string.Empty;
                _txtPosition.TextStr = string.Empty;
                _txtSize.TextStr = string.Empty;
                _imgTexture.Image = TextureManager.Texture_NotFound.Image;
                _btnChangeTexture.Clickable = false;
            }
            else
            {
                _txtIndex.TextStr = $"({GameEditor.CurrentTileForEdit.WorldIndex.X};{GameEditor.CurrentTileForEdit.WorldIndex.Y})";
                _txtPosition.TextStr = $"({GameEditor.CurrentTileForEdit.GlobalPosition.X};{GameEditor.CurrentTileForEdit.GlobalPosition.Y})";
                _txtSize.TextStr = $"({GameEditor.CurrentTileForEdit.Size.X};{GameEditor.CurrentTileForEdit.Size.Y})";
                _imgTexture.Image = GameEditor.CurrentTileForEdit.Texture.Image;
                _btnChangeTexture.Clickable = true;

            }
        }
    }
}
