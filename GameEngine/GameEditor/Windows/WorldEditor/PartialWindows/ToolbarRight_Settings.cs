using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SfmlControls.Controls;
using SfmlControls.Events;
using SfmlControls.Window.PartialWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.GameEditor.Windows.WorldEditor.PartialWindows
{
    public class ToolbarRight_Settings : SfmlPartialWindow
    {
        private const float SIZE_X = WorldEditorConfig.TB_RIGHT_WIDTH;
        private const float SIZE_Y = WorldEditorConfig.TB_RIGHT_HEIGHT;
        private const float POS_X = WorldEditorConfig.RA_WIDTH;
        private const float POS_Y = WorldEditorConfig.TB_TOP_HEIGHT;

        private WorldEditor _worldEditor;

        private SfmlLabel _lblTitle;

        private SfmlCheckbox _chkTiles;
        private SfmlCheckbox _chkTilesBorder;

        private SfmlCheckbox _chkProps;
        private SfmlCheckbox _chkPropsOrigin;
        private SfmlCheckbox _chkPropsGlobalBounds;
        private SfmlCheckbox _chkPropsSpriteBounds;

        private SfmlCheckbox _chkPlayer;
        private SfmlCheckbox _chkPlayerOrigin;
        private SfmlCheckbox _chkPlayerGlobalBounds;
        private SfmlCheckbox _chkPlayerSpriteBounds;

        private SfmlCheckbox _chkEntities;
        private SfmlCheckbox _chkEntitiesOrigin;
        private SfmlCheckbox _chkEntitiesGlobalBounds;
        private SfmlCheckbox _chkEntitiesSpriteBounds;

        public ToolbarRight_Settings(WorldEditor worldEditor) : base(new Vector2f(POS_X, POS_Y), new Vector2f(SIZE_X, SIZE_Y), worldEditor.RenderWindow)
        {
            _worldEditor = worldEditor;

            _lblTitle = new SfmlLabel(
                name: "lblTitle",
                textStr: "World Editor Settings",
                position: new Vector2f(10.0f, 10.0f)
                );
            _lblTitle.Styles = Text.Styles.Bold | Text.Styles.Underlined;
            _lblTitle.CharacterSize = 20u;

            this.AddControl(_lblTitle);

            Vector2f pos = new Vector2f(10.0f, 40.0f);
            Vector2f pos_add = new Vector2f(0.0f, 20.0f);

            _chkTiles = new SfmlCheckbox(
                name: "chkTiles",
                textStr: "Show Tiles",
                position: pos,
                isChecked: _worldEditor.ShowTiles
                );
            _chkTiles.CheckedChanged += (sender, e) => { _worldEditor.ShowTiles = e.IsChecked; };

            this.AddControl(_chkTiles);

            pos += pos_add;

            _chkTilesBorder = new SfmlCheckbox(
                name: "chkTilesBorder",
                textStr: "Show Tiles Borders",
                position: pos,
                isChecked: _worldEditor.ShowTilesBorders
                );
            _chkTilesBorder.CheckedChanged += (sender, e) => { _worldEditor.ShowTilesBorders = e.IsChecked; };

            this.AddControl(_chkTilesBorder);

            pos += pos_add;

            _chkProps = new SfmlCheckbox(
                name: "chkProps",
                textStr: "Show Props",
                position: pos,
                isChecked: _worldEditor.ShowProps
                );
            _chkProps.CheckedChanged += (sender, e) => { _worldEditor.ShowProps = e.IsChecked; };

            this.AddControl(_chkProps);

            pos += pos_add;

            _chkPropsOrigin = new SfmlCheckbox(
                name: "chkPropsOrigin",
                textStr: "Show Props Origin",
                position: pos,
                isChecked: _worldEditor.ShowPropsOrigin
                );
            _chkPropsOrigin.CheckedChanged += (sender, e) => { _worldEditor.ShowPropsOrigin = e.IsChecked; };

            this.AddControl(_chkPropsOrigin);

            pos += pos_add;

            _chkPropsGlobalBounds = new SfmlCheckbox(
                name: "chkPropsGlobalBounds",
                textStr: "Show Props Global Bounds",
                position: pos,
                isChecked: _worldEditor.ShowPropsGlobalBounds
                );
            _chkPropsGlobalBounds.CheckedChanged += (sender, e) => { _worldEditor.ShowPropsGlobalBounds = e.IsChecked; };

            this.AddControl(_chkPropsGlobalBounds);

            pos += pos_add;

            _chkPropsSpriteBounds = new SfmlCheckbox(
                name: "chkPropsSpriteBounds",
                textStr: "Show Props Sprite Bounds",
                position: pos,
                isChecked: _worldEditor.ShowPropsSpriteBounds
                );
            _chkPropsSpriteBounds.CheckedChanged += (sender, e) => { _worldEditor.ShowPropsSpriteBounds = e.IsChecked; };

            this.AddControl(_chkPropsSpriteBounds);

            pos += pos_add;

            _chkPlayer = new SfmlCheckbox(
                name: "chkPlayer",
                textStr: "Show Player",
                position: pos,
                isChecked: _worldEditor.ShowPlayer
                );
            _chkPlayer.CheckedChanged += (sender, e) => { _worldEditor.ShowPlayer = e.IsChecked; };

            this.AddControl(_chkPlayer);

            pos += pos_add;

            _chkPlayerOrigin = new SfmlCheckbox(
                name: "chkPlayerOrigin",
                textStr: "Show Player Origin",
                position: pos,
                isChecked: _worldEditor.ShowPlayerOrigin
                );
            _chkPlayerOrigin.CheckedChanged += (sender, e) => { _worldEditor.ShowPlayerOrigin = e.IsChecked; };

            this.AddControl( _chkPlayerOrigin);

            pos += pos_add;

            _chkPlayerGlobalBounds = new SfmlCheckbox(
                name: "chkPlayerGlobalBounds",
                textStr: "Show Player Global Bounds",
                position: pos,
                isChecked: _worldEditor.ShowPlayerGlobalBounds
                );
            _chkPlayerGlobalBounds.CheckedChanged += (sender, e) => { _worldEditor.ShowPlayerGlobalBounds = e.IsChecked; };

            this.AddControl(_chkPlayerGlobalBounds);

            pos += pos_add;

            _chkPlayerSpriteBounds = new SfmlCheckbox(
                name: "chkPlayerSpriteBounds",
                textStr: "Show Player Sprite Bounds",
                position: pos,
                isChecked: _worldEditor.ShowPlayerSpriteBounds
                );

            _chkPlayerSpriteBounds.CheckedChanged += (sender, e) => { _worldEditor.ShowPlayerSpriteBounds = e.IsChecked; };

            this.AddControl(_chkPlayerSpriteBounds);

            pos += pos_add;

            _chkEntities = new SfmlCheckbox(
                name: "chkEntities",
                textStr: "Show Entities",
                position: pos,
                isChecked: _worldEditor.ShowEntities
                );
            _chkEntities.CheckedChanged += (sender, e) => { _worldEditor.ShowEntities = e.IsChecked; };

            this.AddControl(_chkEntities);

            pos += pos_add;

            _chkEntitiesOrigin = new SfmlCheckbox(
                name: "chkEntitiesOrigin",
                textStr: "Show Entities Origin",
                position: pos,
                isChecked: _worldEditor.ShowEntitiesOrigin
                );
            _chkEntitiesOrigin.CheckedChanged += (sender, e) => { _worldEditor.ShowEntitiesOrigin = e.IsChecked; };

            this.AddControl(_chkEntitiesOrigin);

            pos += pos_add;

            _chkEntitiesGlobalBounds = new SfmlCheckbox(
                name: "chkEntitiesGlobalBounds",
                textStr: "Show Entities Global Bounds",
                position: pos,
                isChecked: _worldEditor.ShowEntitiesGlobalBounds
                );
            _chkEntitiesGlobalBounds.CheckedChanged += (sender, e) => { _worldEditor.ShowEntitiesGlobalBounds = e.IsChecked; };

            this.AddControl(_chkEntitiesGlobalBounds);

            pos += pos_add;

            _chkEntitiesSpriteBounds = new SfmlCheckbox(
                name: "chkEntitiesSpriteBounds",
                textStr: "Show Entities Sprite Bounds",
                position: pos,
                isChecked: _worldEditor.ShowEntitiesSpriteBounds
                );
            _chkEntitiesSpriteBounds.CheckedChanged += (sender, e) => { _worldEditor.ShowEntitiesSpriteBounds = e.IsChecked; };

            this.AddControl(_chkEntitiesSpriteBounds);
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



        #endregion
    }
}
