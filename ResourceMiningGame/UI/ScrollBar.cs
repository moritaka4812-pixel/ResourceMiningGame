using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Color = Microsoft.Xna.Framework.Color;
using Microsoft.Xna.Framework.Graphics;

namespace ResourceMiningGame.UI
{
    public class ScrollBar
    {
        public Rectangle BarRect; // スクロールできる範囲全体
        public Rectangle HandleRect; // 映している範囲を示すスクロールバー

        public void Update(int scrollY, int contentHeight, int viewHeight) // スクロールバーの位置を更新
        {
            float ratio = (float)scrollY / (contentHeight - viewHeight); // コンテンツのどの位置にスクロールバーがあるか（0~1.0）
            int handleY = BarRect.Y + (int)(ratio * (BarRect.Height - HandleRect.Height)); // スクロールバーがコンテンツ全体のどの高さにあるかを計算

            HandleRect = new Rectangle(
                BarRect.X,
                handleY,
                HandleRect.Width,
                HandleRect.Height
                );
        }

        public void Draw(SpriteBatch sb, Texture2D white)
        {
            sb.Draw(white, BarRect, Color.DarkGray);
            sb.Draw(white, HandleRect, Color.White);
        }
    }
}
