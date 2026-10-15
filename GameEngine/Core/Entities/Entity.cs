using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.Graphics;
using SFML.System;

using GameEngine.Core.Textures;
using GameEngine.Utils;

namespace GameEngine.Core.Entities
{
    public abstract class Entity
    {
        private string _name;

        private int _spritePartCounter = 0;

        private Vector2f _size;
        private Vector2f _origin;
        private Vector2f _globalPosition;

        private Directions _currentDirection = Directions.South;
        private Actions _currentAction;

        private Stopwatch _clock = new Stopwatch();
        private Sprite? _currentSprite = null;
        private TextureSubGroup _textureSubGroup;
        private List<EntityTexture> _entityTextures = new List<EntityTexture>();
        protected List<Drawable> _spriteBoundsDrawable = new List<Drawable>();
        protected List<Vector2i> _currentSpriteGlobalBoundsPoints = new List<Vector2i>();

        #region GET_SET

        public string Name { get => _name; set => _name = value; }
        public Vector2f Size { get => _size; set => _size = value; }
        public Vector2f Origin { get => _origin; set => _origin = value; }
        public Vector2f GlobalPosition { get => _globalPosition; set => _globalPosition = value; }
        public Directions CurrentDirection { get => _currentDirection; }
        public Actions CurrentAction { get => _currentAction; }
        public TextureSubGroup TextureSubGroup { get => _textureSubGroup; }
        public List<Vector2i> CurrentSpriteGlobalBoundsPoints { get => _currentSpriteGlobalBoundsPoints; }

        #endregion

        public Entity(string name, Vector2f globalPosition, TextureSubGroup textureGroup, Vector2f size)
        {
            _name = name;
            _size = size;
            _origin = new Vector2f(_size.X / 2.0f, _size.Y / 2.0f);
            _globalPosition = globalPosition;
            _textureSubGroup = textureGroup;

            foreach (Actions action in Enum.GetValues(typeof(Actions)))
            {
                EntityTextureInfos? infos = EntityTextureConfig.EntityTextureInfos.FirstOrDefault(x => x.SubGroup == textureGroup.Name && x.Action == action);

                if (infos == null)
                    continue;

                _entityTextures.Add(new EntityTexture(infos, this));
            }

            _currentAction = _entityTextures.FirstOrDefault() != null ? _entityTextures.First().Action : Actions.Idle;

            UpdateCurrentSpriteGlobalBoundsPoints();
        }

        /// <summary>
        /// Returns the sprite of the entity with it's origin in the center of the bounds of the sprite
        /// </summary>
        /// <returns></returns>
        public Sprite GetSprite()
        {
            EntityTexture entityTexture = _entityTextures.First(x => x.Action == _currentAction);

            if (_clock.IsRunning == false)
                _clock.Start();

            if (_clock.ElapsedMilliseconds >= entityTexture.AnimationIntervalsByDirection[_currentDirection])
            {
                ++_spritePartCounter;

                if (_spritePartCounter >= entityTexture.TextureRectByDirection[_currentDirection].Length)
                {
                    _spritePartCounter = 0;
                }

                _clock.Restart();
            }

            Texture texture = entityTexture.Texture.Texture;
            IntRect rect = entityTexture.TextureRectByDirection[_currentDirection][_spritePartCounter];

            Sprite sprite = new Sprite(texture, rect);
            sprite.Origin = new Vector2f(sprite.GetGlobalBounds().Size.X / 2.0f, sprite.GetGlobalBounds().Size.Y / 2.0f);
            sprite.Position = _globalPosition;

            _currentSprite = sprite;

            GenerateSpriteBoundsDrawable();

            return sprite;
        }

        public Drawable GetDrawableGlobalBounds()
        {
            if (_currentSprite == null)
                return new VertexArray();

            FloatRect spriteBounds = _currentSprite.GetGlobalBounds();

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

        public bool ChangeAction(Actions action)
        {
            if(_currentAction != action)
            {
                if (_entityTextures.FirstOrDefault(x => x.Action == action) == null)
                {
                    Console.WriteLine($"> {_name} - Entity.ChangeAction - Action \"{action.ToString()}\" not found");
                    return false;
                }

                _currentAction = action;
                _spritePartCounter = 0;

                UpdateCurrentSpriteGlobalBoundsPoints();
            }

            return true;
        }

        public bool ChangeDirection(Directions direction)
        {
            if (_currentDirection != direction)
            {
                _currentDirection = direction;
                _spritePartCounter = 0;

                UpdateCurrentSpriteGlobalBoundsPoints();
            }

            return true;
        }

        private void GenerateSpriteBoundsDrawable()
        {
            _spriteBoundsDrawable = new List<Drawable>();

            EntityTexture texture = _entityTextures.First(x => x.Action == _currentAction);

            List<Vector2i> pointsBorder = texture.SpriteLocalBoundPointsByDirection[_currentDirection][_spritePartCounter];

            VertexArray vertexArray = new VertexArray(PrimitiveType.LineStrip);

            IntRect rect = texture.TextureRectByDirection[_currentDirection][_spritePartCounter];
            Vector2f origin = _globalPosition - new Vector2f(rect.Size.X / 2.0f, rect.Size.Y / 2.0f);

            foreach (Vector2i point in pointsBorder)
            {
                Vertex vertex = new Vertex(origin + (Vector2f)point, Color.Red);

                vertexArray.Append(vertex);
            }

            // add the first point to visually complete the loop
            vertexArray.Append(new Vertex(origin + (Vector2f)pointsBorder.First(), Color.Red));

            _spriteBoundsDrawable.Add(vertexArray);
        }

        private void UpdateCurrentSpriteGlobalBoundsPoints()
        {
            _currentSpriteGlobalBoundsPoints = new List<Vector2i>();

            EntityTexture texture = _entityTextures.First(x => x.Action == _currentAction);

            List<Vector2i> pointsBorder = texture.SpriteLocalBoundPointsByDirection[_currentDirection][_spritePartCounter];

            IntRect rect = texture.TextureRectByDirection[_currentDirection][_spritePartCounter];
            Vector2f origin = _globalPosition - new Vector2f(rect.Size.X / 2.0f, rect.Size.Y / 2.0f);

            foreach (Vector2i point in pointsBorder)
            {
                Vector2i globalPoint = (Vector2i)origin + point;
                _currentSpriteGlobalBoundsPoints.Add(globalPoint);
            }
        }

        public void Move(Vector2f move)
        {
            this._globalPosition += move;

            UpdateCurrentSpriteGlobalBoundsPoints();
        }
    }

    public enum Directions
    {
        South = 0,
        West = 1,
        East = 2,
        North = 3
    }

    public enum Actions
    {
        Idle = 0,
        Attack = 1,
        Walk = 2,
        WalkAttack = 3,
        Run = 4,
        RunAttack = 5,
        Hurt = 6,
        Death = 7
    }
}
