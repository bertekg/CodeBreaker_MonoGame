using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CodeBreaker_MonoGame.Class
{
    public abstract class Component
    {
        public abstract void Draw(SpriteBatch spriteBatch);

        public abstract void Update();
    }
}
