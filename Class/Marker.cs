using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CodeBreaker_MonoGame.Class
{
    internal class Marker
    {
        private Texture2D _sprite;
        private int _index;
        private int _maxIndex;
        private Vector2 _startPosition;
        private Vector2 _endPosition;
        private Vector2 _position;

        public Marker(Texture2D sprite, int maxMarkerIndex, Vector2 startPosition, Vector2 endPosition)
        {
            _sprite = sprite;
            _index = 0;
            _maxIndex = maxMarkerIndex;
            _startPosition = startPosition;
            _endPosition = endPosition;
        }
        public void Update()
        {
            _position.X = MathHelper.Lerp(_startPosition.X, _endPosition.X, CountLerpAmout());
            _position.Y = MathHelper.Lerp(_startPosition.Y, _endPosition.Y, CountLerpAmout());
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_sprite, _position, Color.Purple);
        }
        public void MoveMarker(int index)
        {
            _index = index;
        }
        private float CountLerpAmout()
        {
            return (float)_index / (float)_maxIndex;
        }
    }
}
