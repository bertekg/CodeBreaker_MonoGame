using Microsoft.Xna.Framework.Graphics;

namespace CodeBreaker_MonoGame.Interface
{
    internal interface IScene
    {
        public void Update(double deltaTime);
        public void Draw(SpriteBatch spriteBatch);
    }
}
