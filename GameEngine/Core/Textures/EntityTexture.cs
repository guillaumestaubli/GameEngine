using GameEngine.Core.Entities;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core.Textures
{
    public class EntityTexture
    {
        public Actions Action { get; set; }
        public TextureDetails Texture { get; set; }
        public Dictionary<Directions, IntRect[]> TextureRectByDirection { get; set; } = new Dictionary<Directions, IntRect[]>();
        public Dictionary<Directions, long> AnimationIntervalsByDirection { get; set; } = new Dictionary<Directions, long>();
        public Dictionary<Directions, List<List<Vector2i>>> SpriteLocalBoundPointsByDirection { get; set; } = new Dictionary<Directions, List<List<Vector2i>>>();

        public EntityTexture(EntityTextureInfos textureInfos, Entity entity)
        {
            Action = textureInfos.Action;

            TextureDetails? texture = TextureManager.LibraryEntities.GetTexture(textureInfos.Name);

            if (texture == null)
                throw new Exception($"Texture \"{textureInfos.Name}\" Not found in TextureManager.LibraryEntities");

            Texture = texture;

            Vector2i rect_size = new Vector2i(0, 0);
            rect_size.X = (int)(Texture.Image.Size.X / textureInfos.NbColumns);
            rect_size.Y = (int)(Texture.Image.Size.Y / textureInfos.NbLines);

            foreach (EntityTextureLineDescription line in textureInfos.TextureLinesDescription)
            {
                IntRect[] rects = new IntRect[line.NbSprites];

                int x_pos = 0;
                int y_pos = line.LineIndex * (int)rect_size.Y;

                //for (x_pos = 0; x_pos < Texture.Image.Size.X; x_pos += rect_size.X)
                for (x_pos = 0; x_pos < rect_size.X * line.NbSprites; x_pos += rect_size.X)
                {
                    rects[x_pos / rect_size.X] = new IntRect(position: new Vector2i(x_pos, y_pos), size: rect_size);
                }

                TextureRectByDirection.Add(line.Direction, rects);
                AnimationIntervalsByDirection.Add(line.Direction, line.AnimationInterval);

                SpriteLocalBoundPointsByDirection.Add(line.Direction, new List<List<Vector2i>>());

                foreach (IntRect rect in rects)
                {
                    Image sprite = new Image((uint)rect.Size.X, (uint)rect.Size.Y, Color.Transparent);

                    sprite.Copy(
                        source: texture.Image,
                        destX: 0,
                        destY: 0,
                        sourceRect: rect,
                        applyAlpha: false
                        );

                    List<Vector2i> pointsBorder = TextureManager.GenerateSpriteAbsoluteBounds(sprite, $"{texture.Name} - {line.Direction}");

                    SpriteLocalBoundPointsByDirection[line.Direction].Add(pointsBorder);
                }
            }
        }
    }
}
