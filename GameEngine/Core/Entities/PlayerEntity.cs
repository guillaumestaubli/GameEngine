using GameEngine.Core.Saves;
using GameEngine.Core.Textures;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core.Entities
{
    public class PlayerEntity : Entity
    {
        public PlayerEntity(string name, Vector2f globalPosition, TextureSubGroup textureGroup, Vector2f size)
            : base(name, globalPosition, textureGroup, size)
        {

        }

        public PlayerEntity(PlayerEntitySave save)
            : base(
                  name: save.Name,
                  globalPosition: new Vector2f(save.GlobalPosition_X, save.GlobalPosition_Y),
                  textureGroup: TextureManager.LibraryEntities.GetTextureSubGroup(save.TextureSubGroup),
                  size: new Vector2f(save.Size_X, save.Size_Y)
                  )
        {
            this.ChangeDirection(save.CurrentDirection);
            this.ChangeAction(save.CurrentAction);
        }

        public void CycleAction()
        {
            int max = 7;

            int current = (int)this.CurrentAction;

            Actions newAction;

            do
            {
                current++;

                if (current > max)
                    current = 0;

                newAction = (Actions)current;

            } while (!this.ChangeAction(newAction));
        }
    }
}
