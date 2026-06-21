using ResourceMiningGame.Core;
using ResourceMiningGame.Input;
using Rect = Microsoft.Xna.Framework.Rectangle;
using Color = Microsoft.Xna.Framework.Color;

namespace ResourceMiningGame.GameUI
{
    public class WorldPanel : WorldUIElement
    {
        public WorldContainer Container;
        public Color BackgroundColor { get; set; }
        public WorldPanel(int w, int h, Camera camera)
            : base(camera)
        {
            Width = w;
            Height = h;
            Container = new WorldContainer(camera);
            Container.ParentWorld = this;
            BackgroundColor = Color.Gray;
        }

        public void AddChild(WorldUIElement child)
        {
            Container.AddChild(child);
        }

        public override bool UpdateWorld(MouseInput mouse)
        {
            if (!Visible) return false;

            Vector2 worldPos = Camera.ScreenToWorld(mouse.Current.Position.ToVector2());
            bool hit = HitTestWorld(worldPos);

            bool consumed = false;

            // パネル自身のクリック処理（必要なら）
            if (hit && mouse.LeftClicked())
                consumed |= OnLeftClick(mouse);

            // 子要素の更新
            consumed |= Container.UpdateWorld(mouse);

            // ホバー処理
            if (hit)
                consumed |= OnHover(mouse);
            else
                OnHoverExit();

            return consumed;
        }


        public override void DrawWorld(SpriteBatch sb)
        {
            if (!Visible) return;

            var r = GetWorldRectWorldSpace();
            sb.Draw(whiteTex, r, BackgroundColor);

            Container.DrawWorld(sb);
        }

    }
}
