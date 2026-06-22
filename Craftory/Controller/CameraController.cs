using Craftory.Core;
using Craftory.Input;
using SharpDX.XInput;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace Craftory.Controller
{
    public class CameraController //入力をカメラ操作用の値に変換するクラス
    {
        public Vector2 MoveDirection { get; private set; }
        public float ZoomDelta { get; private set; }
        public Vector2 DragDelta { get; private set; }

        public void Update(InputManager input) //カメラ操作の意図[移動・ズーム・ドラッグ]を更新
        {
            // Zoom
            int scroll = input.Mouse.ScrollDelta();
            ZoomDelta = scroll > 0 ? 0.1f : scroll < 0 ? -0.1f : 0f;

            //WASD
            Vector2 move = Vector2.Zero;
            if (input.keyboard.IsDown(Keys.W)) move.Y -= 1;
            if (input.keyboard.IsDown(Keys.S)) move.Y += 1;
            if (input.keyboard.IsDown(Keys.A)) move.X -= 1;
            if (input.keyboard.IsDown(Keys.D)) move.X += 1;
            MoveDirection = move;

            // Drag
            if (input.Mouse.Current.MiddleButton == ButtonState.Pressed)
                DragDelta = input.Mouse.PointDelta().ToVector2();
            else
                DragDelta = Vector2.Zero;
        }
    }
}
