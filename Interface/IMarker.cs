using Microsoft.Xna.Framework.Graphics;

namespace CodeBreaker_MonoGame.Interface
{
    internal interface IMarker
    {
        public void MoveMarker(int index);
        public void ChangeSelected();
        public void Update();
        public void Draw(SpriteBatch spriteBatch);
    }
}
