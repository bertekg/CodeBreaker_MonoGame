using Microsoft.Xna.Framework.Graphics;

namespace CodeBreaker_MonoGame.Interface
{
    internal interface IScene
    {
        public void Update();
        public void Draw(SpriteBatch spriteBatch);
    }
}
