using GameEngine.Core.Entities;
using GameEngine.Core.Props;
using GameEngine.Core.Textures;
using GameEngine.Core.World;
using GameEngine.Utils;
using SFML;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SfmlControls;
using SfmlControls.Controls;
using SfmlControls.Window.PartialWindow;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.GameEditor.Windows.WorldEditor.PartialWindows
{
    public class RenderArea : SfmlPartialWindow
    {
        private const float POS_X = 0.0f;
        private const float POS_Y = WorldEditorConfig.TB_TOP_HEIGHT;

        private const float SIZE_X = WorldEditorConfig.RA_WIDTH;
        private const float SIZE_Y = WorldEditorConfig.RA_HEIGHT;

        private WorldEditor _worldEditor;

        public RenderArea(WorldEditor worldEditor) : base(new Vector2f(POS_X, POS_Y), new Vector2f(SIZE_X, SIZE_Y), worldEditor.RenderWindow)
        {
            _worldEditor = worldEditor;

            this.ScrollableHorizontal = true;
            this.ScrollableVertical = true;
            this.ScrollableWithMouseDrag = true;
            this.Zoomable = true;
            this.ZoomableWithMouseWheel = true;
            this.IgnoreMaxScrollOffsets = true;

            MouseButtonReleased += RenderArea_MouseButtonReleased;
            KeyPressed += RenderArea_KeyPressed;
        }

        #region OVERRIDE

        public override void DrawOnFrame(ref RenderTexture renderTexture)
        {
            #region DISPLAY_GROUND

            for (int x = 0; x < Config.WORLD_SIZE_X; ++x)
            {
                for (int y = 0; y < Config.WORLD_SIZE_Y; ++y)
                {
                    WorldTile tile = World.Tiles[x, y];

                    if (_worldEditor.ShowTiles)
                    {
                        renderTexture.Draw(tile.Drawable.Background, tile.Drawable.BackgroundRenderStates);
                    }

                    if (_worldEditor.ShowTilesBorders)
                    {
                        renderTexture.Draw(tile.Drawable.Borders);
                    }
                }
            }

            if (GameEditor.CurrentTileForEdit_Outline != null)
            {
                renderTexture.Draw(GameEditor.CurrentTileForEdit_Outline);
            }

            #endregion

            #region DISPLAY_PLAYER

            if (_worldEditor.ShowPlayer)
            {
                renderTexture.Draw(World.PlayerEntity.GetSprite());
            }

            if (_worldEditor.ShowPlayerOrigin)
            {
                renderTexture.Draw(World.PlayerEntity.GetDrawableOrigin());
            }

            if (_worldEditor.ShowPlayerGlobalBounds)
            {
                renderTexture.Draw(World.PlayerEntity.GetDrawableGlobalBounds());
            }

            if (_worldEditor.ShowPlayerSpriteBounds)
            {
                foreach (Drawable drawable in World.PlayerEntity.GetDrawableSpriteBounds())
                {
                    renderTexture.Draw(drawable);
                }
            }

            #endregion

            #region DISPLAY_ENTITIES

            foreach (Entity entity in World.NpcEntities)
            {
                if (_worldEditor.ShowEntities)
                {
                    renderTexture.Draw(entity.GetSprite());
                }

                if (_worldEditor.ShowEntitiesOrigin)
                {
                    renderTexture.Draw(entity.GetDrawableOrigin());
                }

                if (_worldEditor.ShowEntitiesGlobalBounds)
                {
                    renderTexture.Draw(entity.GetDrawableGlobalBounds());
                }

                if (_worldEditor.ShowEntitiesSpriteBounds)
                {
                    foreach (Drawable drawable in entity.GetDrawableSpriteBounds())
                    {
                        renderTexture.Draw(drawable);
                    }
                }
            }

            #endregion

            #region DISPLAY_PROPS

            foreach (Prop prop in World.Props)
            {
                if (_worldEditor.ShowProps)
                {
                    renderTexture.Draw(prop.GetSprite());
                }

                if (_worldEditor.ShowPropsOrigin)
                {
                    renderTexture.Draw(prop.GetDrawableOrigin());
                }

                if (_worldEditor.ShowPropsGlobalBounds)
                {
                    renderTexture.Draw(prop.GetDrawableGlobalBounds());
                }

                if (_worldEditor.ShowPropsSpriteBounds)
                {
                    foreach (Drawable drawable in prop.GetDrawableSpriteBounds())
                    {
                        renderTexture.Draw(drawable);
                    }
                }
            }

            if (GameEditor.CurrentPropForEdit != null)
            {
                foreach (Drawable drawable in GameEditor.CurrentPropForEdit.GetDrawableSpriteBounds())
                {
                    renderTexture.Draw(drawable);
                }
            }

            #endregion
        }

        public override Cursor.CursorType GetCursorType(Vector2f globalPosition)
        {
            if (this.ScrollMouseDragRunning)
                return Cursor.CursorType.SizeAll;
            else
                return Cursor.CursorType.Arrow;
        }

        #endregion

        #region EVENTS

        private void RenderArea_MouseButtonReleased(object? sender, MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Right)
            {
                Vector2f worldPosition = ConvertGlobalPositionToLocalPosition(new Vector2f(e.X, e.Y));

                WorldTile? tile = World.GetTileClicked(worldPosition);

                if (_worldEditor.ShowInspectTileToolbar)
                {
                    if (tile != null)
                    {
                        if (GameEditor.CurrentTileForEdit != null && GameEditor.CurrentTileForEdit.WorldIndex == tile.WorldIndex)
                        {
                            GameEditor.CurrentTileForEdit_Reset();
                        }
                        else
                        {
                            GameEditor.CurrentTileForEdit_Set(tile);
                        }
                    }
                    else
                    {
                        GameEditor.CurrentTileForEdit_Reset();
                    }

                    _worldEditor.Update_ToolbarRight_EditTile();
                }
                else if (_worldEditor.ShowDrawGroundToolbar)
                {
                    if (tile != null)
                    {
                        GameEditor.CurrentTextureToDrawOnTile_ApplyToTile(tile);
                    }
                }
                else if (_worldEditor.ShowAddPropToolbar)
                {
                    GameEditor.CurrentPropToAddOnWorld_ApplyToWorld(worldPosition);
                }
                else if (_worldEditor.ShowAddEntityToolbar)
                {
                    GameEditor.CurrentEntityToAddOnWorld_ApplyToWorld(worldPosition);
                }
                else if (_worldEditor.ShowInspectPropToolbar)
                {
                    Prop? prop = World.GetPropClicked(worldPosition);

                    if(prop != null)
                    {
                        if(GameEditor.CurrentPropForEdit != null && GameEditor.CurrentPropForEdit.Name == prop.Name)
                        {
                            GameEditor.CurrentPropForEdit = null;
                        }
                        else
                        {
                            GameEditor.CurrentPropForEdit = prop;
                        }
                    }
                    else
                    {
                        GameEditor.CurrentPropForEdit = null;
                    }

                    _worldEditor.Update_ToolbarRight_EditProp();
                }
            }
        }

        private void RenderArea_KeyPressed(object? sender, KeyEventArgs e)
        {

        }

        #endregion

        public void CenterViewOnPlayer(PlayerEntity player)
        {
            Vector2f center = player.GlobalPosition;

            this.SetViewCenter(center);
        }
    }
}
