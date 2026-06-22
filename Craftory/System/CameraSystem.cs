using Craftory.Controller;
using Craftory.Core;


namespace Craftory.System
{
    public class CameraSystem
    {
        private Camera camera;
        private CameraController controller;

        public CameraSystem(Camera camera, CameraController controller)
        {
            this.camera = camera;
            this.controller = controller;
        }

        public void Update(Game1 game, float dt)
        {
            controller.Update(game.Input);//入力からカメラの意図[移動・ズーム・ドラッグ]の状態を更新
            //更新された操作意図をCameraに適用して動かす
            if (controller.ZoomDelta != 0)
                camera.ZoomAt(
                    controller.ZoomDelta, game.Input.Mouse.Current.Position.ToVector2(),
                    game.GraphicsDevice.Viewport.Width,
                    game.GraphicsDevice.Viewport.Height);

            if (controller.MoveDirection != Vector2.Zero)
                camera.Move(controller.MoveDirection * 500f * dt / camera.Zoom);

            if (controller.DragDelta != Vector2.Zero)
                camera.Drag(controller.DragDelta);
        }
    }
}
