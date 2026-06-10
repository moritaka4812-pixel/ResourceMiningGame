using Microsoft.Xna.Framework.Input;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Point = Microsoft.Xna.Framework.Point;

namespace ResourceMiningGame.Input
{
    public class MouseInput
    {
        public MouseState Current { get; private set; } //現在のマウス状態
        public MouseState Previous { get; private set; } //1フレーム前のマウス状態

        public MouseInput()
        {
            Current = Mouse.GetState();
        }
        public void Update() 
        {
            // マウス状態を更新
            Previous = Current;
            Current = Mouse.GetState();
        }

        public bool LeftClicked() //左クリックの入力処理
        {
            return Current.LeftButton == ButtonState.Pressed &&
                   Previous.LeftButton == ButtonState.Released;
        }

        public bool RightClicked() //右クリックの入力処理
        {
            return Current.RightButton == ButtonState.Pressed &&
                   Previous.RightButton == ButtonState.Released;
        }

        public bool MiddleClicked() //ホイールクリックの入力処理
        {
            return Current.MiddleButton == ButtonState.Pressed &&
                   Previous.MiddleButton == ButtonState.Released;
        }

        public int ScrollDelta() //マウスホイールのスクロール変化量
        {
            return Current.ScrollWheelValue - Previous.ScrollWheelValue;
        }

        public Point PointDelta() //マウス位置の変化量
        {
            return Current.Position - Previous.Position;
        }
    }
}
