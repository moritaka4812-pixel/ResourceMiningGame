using Craftory.Input;
using Craftory.UI.Core;
using Color = Microsoft.Xna.Framework.Color;
using Rect = Microsoft.Xna.Framework.Rectangle;

namespace Craftory.UI.Elements
{
    public class TextLabel : UIElement
    {
        private SpriteFont font;
        private string text;
        private Color color = Color.White;

        public TextLabel(SpriteFont font, string text, Rect rect)
        {
            this.font = font;
            this.text = text;
            this.rect = rect;
        }

        public override void Draw(SpriteBatch sb)
        {
            if (!Visible) return;
            var size = font.MeasureString(text); //文字列のサイズを取得
            sb.DrawString(font, text, new Vector2(rect.X - size.X / 2, rect.Y - size.Y / 2), color); //中央基準で配置
        }

        public override bool Update(MouseInput mouse)
        {
            return false; //クリック判定なし
        }
    }
}
