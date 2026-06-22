using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Craftory.Input;
using Craftory.Core;
using Rect = Microsoft.Xna.Framework.Rectangle;
using Point = Microsoft.Xna.Framework.Point;
using Color = Microsoft.Xna.Framework.Color;

namespace Craftory.GameUI
{
    public abstract class WorldUIElement
    {
        protected Rect rect;                 // ローカル（親からの相対）ワールド矩形
        public Rect LocalRect => rect;

        public float X { get => rect.X; set => rect = new Rect((int)value, rect.Y, rect.Width, rect.Height); }
        public float Y { get => rect.Y; set => rect = new Rect(rect.X, (int)value, rect.Width, rect.Height); }
        public int Width { get => rect.Width; set => rect = new Rect(rect.X, rect.Y, value, rect.Height); }
        public int Height { get => rect.Height; set => rect = new Rect(rect.X, rect.Y, rect.Width, value); }

        public bool Visible { get; set; } = true;

        public WorldUIElement? ParentWorld { get; set; } = null;

        public Camera Camera { get; }

        protected static Texture2D whiteTex;

        // ====== イベントデリゲート（UIElement と同じ構造の“ワールド版”） ======
        public Func<MouseInput, bool>? OnLeftClickHandler { get; set; }
        public Func<MouseInput, bool>? OnRightClickHandler { get; set; }
        public Func<MouseInput, bool>? OnMiddleClickHandler { get; set; }
        public Func<MouseInput, int, bool>? OnWheelHandler { get; set; }
        public Func<MouseInput, Point, bool>? OnDragHandler { get; set; }
        public Func<MouseInput, bool>? OnHoverHandler { get; set; }

        // 継承先でオーバーライドする用の仮想メソッド
        public virtual bool OnLeftClick(MouseInput mouse) { return false; }
        public virtual bool OnRightClick(MouseInput mouse) { return false; }
        public virtual bool OnMiddleClick(MouseInput mouse) { return false; }
        public virtual bool OnWheel(MouseInput mouse, int delta) { return false; }
        public virtual bool OnDrag(MouseInput mouse, Point delta) { return false; }
        public virtual bool OnHover(MouseInput mouse) { return false; }
        public virtual void OnHoverExit() { }
        // 外部に操作を伝えるイベント
        public event Action? LeftClicked;
        public event Action? RightClicked;
        public event Action? MiddleClicked;
        public event Action? Hovered;
        public event Action? HoveredExited;
        public event Action<int>? WheelScrolled;
        public event Action<Point>? Dragged;

        public WorldUIElement(Camera camera)
        {
            Camera = camera;
        }

        public static void Initialize(GraphicsDevice device)
        {
            whiteTex = new Texture2D(device, 1, 1);
            whiteTex.SetData(new[] { Color.White });
        }

        // 親のワールド座標を加味した「絶対ワールド矩形」
        public Rect GetWorldRectWorldSpace()
        {
            float worldX = rect.X;
            float worldY = rect.Y;

            if (ParentWorld != null)
            {
                var parent = ParentWorld.GetWorldRectWorldSpace();
                worldX += parent.X;
                worldY += parent.Y;
            }

            return new Rect((int)worldX, (int)worldY, rect.Width, rect.Height);
        }

        // ワールド座標でのヒットテスト
        public bool HitTestWorld(Vector2 worldPos)
        {
            return GetWorldRectWorldSpace().Contains(worldPos);
        }

        // ====== ワールド版 Update（UIElement.Update の“座標系だけ差し替えた版”） ======
        public virtual bool UpdateWorld(MouseInput mouse)
        {
            if (!Visible) return false;

            bool consumed = false;

            // マウスのスクリーン座標 → ワールド座標
            Vector2 worldPos = Camera.ScreenToWorld(mouse.Current.Position.ToVector2());

            bool hit = HitTestWorld(worldPos);

            // 左クリック
            if (hit && mouse.LeftClicked())
            {
                LeftClicked?.Invoke();
                if (OnLeftClickHandler != null)
                    consumed |= OnLeftClickHandler(mouse);
                else
                    consumed |= OnLeftClick(mouse);
            }

            // 右クリック
            if (hit && mouse.RightClicked())
            {
                RightClicked?.Invoke();
                if (OnRightClickHandler != null)
                    consumed |= OnRightClickHandler(mouse);
                else
                    consumed |= OnRightClick(mouse);
            }

            // 中クリック
            if (hit && mouse.MiddleClicked())
            {
                MiddleClicked?.Invoke();
                if (OnMiddleClickHandler != null)
                    consumed |= OnMiddleClickHandler(mouse);
                else
                    consumed |= OnMiddleClick(mouse);
            }

            // ホイール
            int wheel = mouse.ScrollDelta();
            if (wheel != 0 && hit)
            {
                WheelScrolled?.Invoke(wheel);
                if (OnWheelHandler != null)
                    consumed |= OnWheelHandler(mouse, wheel);
                else
                    consumed |= OnWheel(mouse, wheel);
            }

            // ドラッグ（必要なら実装）
            // ここでは例として「前フレームとの差分」を使う形を想定
            // if (hit && mouse.LeftDragging()) { ... }

            // ホバー
            if (hit)
            {
                Hovered?.Invoke();
                if (OnHoverHandler != null)
                    consumed |= OnHoverHandler(mouse);
                else
                    consumed |= OnHover(mouse);
            }
            else
            {
                HoveredExited?.Invoke();
                OnHoverExit();
            }

            return consumed;
        }

        protected void RaiseLeftClicked()
        {
            LeftClicked?.Invoke();
        }

        protected void RaiseRightClicked()
        {
            RightClicked?.Invoke();
        }

        protected void RaiseMiddleClicked()
        {
            MiddleClicked?.Invoke();
        }

        protected void RaiseWheelScrolled(int wheel)
        {
            WheelScrolled?.Invoke(wheel);
        }

        // ワールド座標で描画
        public abstract void DrawWorld(SpriteBatch sb);
    }
}
