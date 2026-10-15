using GameEngine.Core.Saves;
using GameEngine.Core.Textures;
using GameEngine.Utils;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core.Props
{
    public class Prop
    {
        protected string _name;
        protected Vector2f _globalPosition;
        protected Vector2f _size;
        protected Vector2f _origin;
        protected Vector2f _spriteScale;
        protected TextureDetails _texture;
        protected Sprite _sprite;
        protected List<Vector2i> _spriteBoundsPoints = new List<Vector2i>();
        protected List<Drawable> _spriteBoundsDrawable = new List<Drawable>();

        #region GET_SET

        public string Name { get => _name; set => _name = value; }
        public Vector2f GlobalPosition { get => _globalPosition; set => _globalPosition = value; }
        public Vector2f Size { get => _size; set => _size = value; }
        public Vector2f Origin { get => _origin; set => _origin = value; }
        public Vector2f SpriteScale { get => _spriteScale; }
        public TextureDetails Texture { get => _texture; }
        public FloatRect GlobalBounds
        {
            get => new FloatRect(_globalPosition - _size / 2.0f, _size);
        }
        public List<Vector2i> SpriteBoundPoints { get => _spriteBoundsPoints; }

        #endregion

        public Prop(string name, Vector2f globalPosition, TextureDetails texture)
        {
            _name = name;
            _globalPosition = globalPosition;
            _size = (Vector2f)texture.Image.Size;
            _origin = new Vector2f(texture.Image.Size.X / 2.0f, texture.Image.Size.Y / 2.0f);
            _texture = texture;
            _spriteScale = new Vector2f();
            _spriteScale.X = _size.X / _texture.Texture.Size.X;
            _spriteScale.Y = _size.Y / _texture.Texture.Size.Y;

            //_sprite = new Sprite(_texture.Texture);
            //_sprite.Origin = _origin;
            //_sprite.Position = _globalPosition;
            //_sprite.Scale = _spriteScale;
            _sprite = UpdateSprite();

            _spriteBoundsPoints = GenerateSpriteBoundsPoints(_texture.PointsSpriteBorder);
            _spriteBoundsDrawable = GenerateDrawableSpriteBounds();
        }

        public Prop(PropSave save)
        {
            _name = save.Name;
            _size = new Vector2f(save.Size_X, save.Size_Y);
            _origin = new Vector2f(save.Origin_X, save.Origin_Y);
            _globalPosition = new Vector2f(save.GlobalPosition_X, save.GlobalPosition_Y);
            _texture = TextureManager.LibraryProps.GetTexture(save.TextureName);
            _spriteScale = new Vector2f();
            _spriteScale.X = _size.X / _texture.Texture.Size.X;
            _spriteScale.Y = _size.Y / _texture.Texture.Size.Y;

            //_sprite = new Sprite(_texture.Texture);
            //_sprite.Origin = _origin;
            //_sprite.Position = _globalPosition;
            //_sprite.Scale = _spriteScale;
            _sprite = UpdateSprite();

            _spriteBoundsPoints = GenerateSpriteBoundsPoints(_texture.PointsSpriteBorder);
            _spriteBoundsDrawable = GenerateDrawableSpriteBounds();
        }

        public Sprite GetSprite()
        {
            return _sprite;
        }

        public Drawable GetDrawableGlobalBounds()
        {
            FloatRect spriteBounds = _sprite.GetGlobalBounds();

            VertexArray vertexArray = SfmlUtils.GetLineStripRectangle(spriteBounds.Position, spriteBounds.Size, Color.Red);

            return vertexArray;
        }

        public List<Drawable> GetDrawableSpriteBounds()
        {
            return _spriteBoundsDrawable;
        }

        public Drawable GetDrawableOrigin()
        {
            float radius = 3.0f;

            CircleShape circleShape = new CircleShape(radius);
            circleShape.Origin = new Vector2f(radius, radius);
            circleShape.Position = _globalPosition;
            circleShape.FillColor = Color.Magenta;

            return circleShape;
        }

        private List<Drawable> GenerateDrawableSpriteBounds()
        {
            List<Drawable> drawables = new List<Drawable>();

            VertexArray spriteBorder = new VertexArray(PrimitiveType.LineStrip);

            foreach(Vector2f point in _spriteBoundsPoints)
            {
                spriteBorder.Append(new Vertex(point, Color.Red));
            }

            drawables.Add(spriteBorder);

            return drawables;
        }

        private List<Vector2i> GenerateSpriteBoundsPoints(List<Vector2i> localPoints)
        {
            List<Vector2i> globalPoints = new List<Vector2i>();

            Vector2f origin = _globalPosition - _size / 2.0f;

            foreach (Vector2i localPoint in localPoints)
            {
                Vector2i scaledLocalPoint = localPoint;

                scaledLocalPoint.X = (int)((float)localPoint.X * _spriteScale.X);
                scaledLocalPoint.Y = (int)((float)localPoint.Y * _spriteScale.Y);

                Vector2i globalPoint = (Vector2i)origin + scaledLocalPoint;
                globalPoints.Add(globalPoint);
            }

            return globalPoints;
        }

        public void SetGlobalPosition(Vector2f newPosition)
        {
            _globalPosition = newPosition;

            _spriteBoundsPoints.Clear();
            _spriteBoundsDrawable.Clear();

            _spriteBoundsPoints = GenerateSpriteBoundsPoints(_texture.PointsSpriteBorder);
            _spriteBoundsDrawable = GenerateDrawableSpriteBounds();

            _sprite.Dispose();
            _sprite = UpdateSprite();
        }

        public void SetSize(Vector2f newSize)
        {
            _size = newSize;

            _spriteScale.X = _size.X / _texture.Texture.Size.X;
            _spriteScale.Y = _size.Y / _texture.Texture.Size.Y;

            _spriteBoundsPoints.Clear();
            _spriteBoundsDrawable.Clear();

            _spriteBoundsPoints = GenerateSpriteBoundsPoints(_texture.PointsSpriteBorder);
            _spriteBoundsDrawable = GenerateDrawableSpriteBounds();

            _sprite.Dispose();
            _sprite = UpdateSprite();
        }

        private Sprite UpdateSprite()
        {
            Sprite sprite = new Sprite(_texture.Texture);
            sprite.Origin = _origin;
            sprite.Position = _globalPosition;
            sprite.Scale = _spriteScale;

            return sprite;
        }

    }
}
