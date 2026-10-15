using GameEngine.Core.Textures;
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
    public class ToolbarRight_AddProp : SfmlPartialWindow
    {
        private const float SIZE_X = WorldEditorConfig.TB_RIGHT_WIDTH;
        private const float SIZE_Y = WorldEditorConfig.TB_RIGHT_HEIGHT;
        private const float POS_X = WorldEditorConfig.RA_WIDTH;
        private const float POS_Y = WorldEditorConfig.TB_TOP_HEIGHT;

        private TextureMainGroup _mainGroupSelected;
        private TextureDetails? _textureDetailsSelected = null;

        private SfmlDropdown _dropdownMainTextureGroups;

        public ToolbarRight_AddProp(RenderWindow parentWindow) : base(new Vector2f(POS_X, POS_Y), new Vector2f(SIZE_X, SIZE_Y), parentWindow)
        {
            this.ScrollableVertical = true;
            this.ScrollableWithMouseWheel = true;

            _mainGroupSelected = TextureManager.LibraryProps.MainGroups.First();

            _dropdownMainTextureGroups = new SfmlDropdown(
                name: "dropdownMainTextureGroups",
                position: new Vector2f(10.0f, 10.0f),
                width: WorldEditorConfig.TB_RIGHT_WIDTH - 20.0f
                );

            foreach (TextureMainGroup mainGroup in TextureManager.LibraryGround.MainGroups)
            {
                _dropdownMainTextureGroups.AddItem(
                    name: mainGroup.Name,
                    textStr: mainGroup.Name,
                    isSelected: mainGroup.Name == _mainGroupSelected.Name
                    );
            }

            this.AddControl(_dropdownMainTextureGroups);

            SetTextureImages();
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

        private void ImgProp_MouseButtonReleased(object? sender, MouseButtonEventArgs e)
        {
            if (sender == null)
                return;

            if (sender is not SfmlImage)
                return;

            SfmlImage image = (SfmlImage)sender;

            TextureDetails? textureSelected = TextureManager.LibraryProps.GetTexture(image.Name);

            if (textureSelected == null)
                return;

            _textureDetailsSelected = textureSelected;

            GameEditor.CurrentPropToAddOnWorld_Set(_textureDetailsSelected);

            SetTextureImages();
        }

        #endregion

        private void SetTextureImages()
        {
            List<SfmlControl> ctrlsToRemove = this.Controls.Where(x => x is SfmlImage or SfmlLabel).ToList();

            foreach (SfmlControl ctrlToRemove in ctrlsToRemove)
            {
                this.RemoveControl(ctrlToRemove);
                ctrlToRemove.Dispose();
            }

            Vector2f pos = new Vector2f(10.0f, 40.0f);

            foreach (TextureSubGroup subGroup in _mainGroupSelected.SubGroups)
            {
                SfmlLabel lblSubGroup = new SfmlLabel(
                    name: subGroup.Name,
                    textStr: subGroup.Name,
                    position: pos
                    );

                this.AddControl(lblSubGroup);

                pos.Y += 20.0f;

                Vector2f img_size = new Vector2f(86.0f, 86.0f);

                float img_add_x = img_size.X + 10.0f;
                float img_add_y = img_size.Y + 10.0f;

                int counter = 0;

                foreach (TextureDetails texture in subGroup.Textures)
                {
                    SfmlImage img = new SfmlImage(
                        name: texture.Name,
                        position: pos,
                        size: img_size,
                        image: texture.Image
                        );
                    img.HasBorder = true;
                    img.BorderColor = _textureDetailsSelected != null && texture.Name == _textureDetailsSelected.Name ? Color.Red : Color.Black;
                    img.Clickable = true;
                    img.MouseButtonReleased += ImgProp_MouseButtonReleased;

                    this.AddControl(img);

                    if (counter < 2)
                    {
                        pos.X += img_add_x;
                        ++counter;
                    }
                    else
                    {
                        pos.X = 10.0f;
                        pos.Y += img_add_y;
                        counter = 0;
                    }
                }

                pos.X = 10.0f;

                if (subGroup.Textures.Count % 3 != 0)
                    pos.Y += img_add_y;
            }
        }
    }
}
