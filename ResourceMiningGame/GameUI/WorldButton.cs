using Color = Microsoft.Xna.Framework.Color;
using Rect = Microsoft.Xna.Framework.Rectangle;
using ResourceMiningGame.Input;
using ResourceMiningGame.Core;

namespace ResourceMiningGame.GameUI
{
    public class WorldButton : WorldUIElement
    {
        private string text;
        private SpriteFont font;
        public Color TextColor;
        public Color FillColor;
        public Color BorderColor;
        public Color NormalFillColor;
        public Color HoverFillColor;

        public WorldButton(Camera camera, SpriteFont font, string text, Rect rect)
            : base(camera)
        {
            this.X = rect.X;
            this.Y = rect.Y;
            this.Width = rect.Width;
            this.Height = rect.Height;
            this.font = font;
            this.text = text;
            this.TextColor = Color.White;
            this.FillColor = Color.DarkSlateGray;
            this.BorderColor = Color.White;
            this.NormalFillColor = Color.DarkSlateGray;
            this.HoverFillColor = Color.Gray;
        }

        public override void OnHoverExit() //非ホバー時
        {
            FillColor = NormalFillColor;
            BorderColor = Color.White;
        }

        public override bool UpdateWorld(MouseInput mouse)
        {
            var worldPos = Camera.ScreenToWorld(mouse.Current.Position.ToVector2()); 
            var r = GetWorldRectWorldSpace(); 

            if (r.Contains(worldPos))
            {
                OnHoverWorld(worldPos);
                if (mouse.LeftClicked())
                {
                    OnLeftClick(mouse);
                    return true;
                }
            }
            else
            {
                OnHoverExit();
            }
            return false;
        }



        public override void DrawWorld(SpriteBatch sb)
        {
            var r = GetWorldRectWorldSpace();

            sb.Draw(whiteTex, r, FillColor);
            DrawRectangle(sb, r, 3, BorderColor);

            var size = font.MeasureString(text);
            var pos = new Vector2(
                r.X + (r.Width - size.X) / 2,
                r.Y + (r.Height - size.Y) / 2
                );

            sb.DrawString(font, text, pos, TextColor);
        }

        public void OnHoverWorld(Vector2 worldPos)
        {
            var r = GetWorldRectWorldSpace();
            bool hover = r.Contains(worldPos);

            FillColor = hover ? HoverFillColor : NormalFillColor;
            BorderColor = hover ? Color.Yellow : Color.White;
        }

        protected void DrawRectangle(SpriteBatch sb, Rect rect, int thickness, Color color)
        {
            sb.Draw(whiteTex, new Rect(rect.X, rect.Y, rect.Width, thickness), color); // 上
            sb.Draw(whiteTex, new Rect(rect.X, rect.Y, thickness, rect.Height), color); // 左
            sb.Draw(whiteTex, new Rect(rect.X, rect.Y + rect.Height - thickness, rect.Width, thickness), color); // 下
            sb.Draw(whiteTex, new Rect(rect.X + rect.Width - thickness, rect.Y, thickness, rect.Height), color); // 右
        }
    }
}
