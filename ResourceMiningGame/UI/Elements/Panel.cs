using ResourceMiningGame.Input;
using ResourceMiningGame.UI.Core;
using Color = Microsoft.Xna.Framework.Color;
using Rect = Microsoft.Xna.Framework.Rectangle;

namespace ResourceMiningGame.UI.Elements
{
    public class Panel : UIElement
    {
        public Color BackgroundColor { get; set; }

        private UIContainer container;

        public Panel(int width, int height)
        {
            Width = width;
            Height = height;
            BackgroundColor = Color.Gray;

            container = new UIContainer();
            container.Parent = this; //コンテナの親をこのPanelに
        }

        public void AddChild(UIElement child)
        {
            container.Add(child);
        }

        public void RemoveChild(UIElement child)
        {
            container.Remove(child);
        }

        public void SetBackground(Color color)
        {
            this.BackgroundColor = color;
        }

        public override bool Update(MouseInput mouse)
        {
            if (!Visible) return false;

            //Panel自身のレイアウト更新
            this.RecalculateLayout();

            //子要素のレイアウト更新とUpdate()
            container.RecalculateLayout();
            container.Update(mouse);

            return false;
        }

        public override void Draw(SpriteBatch sb)
        {
            if (!Visible) return;

            //背景
            sb.Draw(whiteTex, new Rect((int)X, (int)Y, Width, Height), BackgroundColor);
            //子要素の描画
            container.Draw(sb);
        }
    }
}
