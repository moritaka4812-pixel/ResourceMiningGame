using ResourceMiningGame.Input;
using Color = Microsoft.Xna.Framework.Color;
using Rect = Microsoft.Xna.Framework.Rectangle;

namespace ResourceMiningGame.UI.Core
{
    public class UIContainer : UIElement
    {
        public List<UIElement> Children { get; private set; } //コンテナが持つ子
        public Color? BackgroundColor { get; set; } //背景色

        public UIContainer() : base()
        {
            Children = new List<UIElement>();
            BackgroundColor = null;
        }

        public virtual void Add(UIElement child) //コンテナに追加
        {
            //child.X += rect.X;
            //child.Y += rect.Y;
            Children.Add(child);
            child.Parent = this;
        }

        public void Remove(UIElement child) //コンテナから削除
        {
            Children.Remove(child);
            child.Parent = null;
        }

        public override bool Update(MouseInput mouse) //コンテナ全体の位置更新
        {
            bool consumed = base.Update(mouse);

            this.RecalculateLayout();

            foreach(var child in Children)
            {
                child.RecalculateLayout();
                consumed |= child.Update(mouse);
            }

            return consumed;
        }

        public override void Draw(SpriteBatch sb) //すべての子要素と背景を描画
        {
            if (!Visible) return;

            if (BackgroundColor.HasValue)
                sb.Draw(whiteTex, rect, BackgroundColor.Value);
            foreach (UIElement child in Children)
            {
                child.Draw(sb);
            }
        }
    }
}
