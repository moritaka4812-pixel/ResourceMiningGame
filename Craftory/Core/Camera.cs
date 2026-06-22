using Point = Microsoft.Xna.Framework.Point;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;

namespace Craftory.Core
{
    public class Camera
    {
        public Vector2 Position; //カメラ位置
        public float Zoom; //現在のズーム
        private float Zoom_min; //最小ズーム
        private float Zoom_max; //最大ズーム
        private Game1 game;

        public Camera(Vector2 position , Game1 game, float zoom = 1f, float zoom_min = 0.5f, float zoom_max = 3.0f)
        {
            Zoom_min = zoom_min;
            Zoom_max = zoom_max;
            Zoom = zoom;
            Position = position;
            this.game = game;
        }

        public void ZoomBy(float amount) //amountだけzoom値を増減させる
        {
            Zoom = Math.Clamp(Zoom + amount, Zoom_min, Zoom_max);
        }

        public void ZoomAt(float zoomDelta, Vector2 mouseScreenPos, int viewportWidth, int viewportHeight)
        {
            int w = game.GraphicsDevice.Viewport.Width;
            int h = game.GraphicsDevice.Viewport.Height;

            Vector2 before = ScreenToWorld(mouseScreenPos);

            Zoom += zoomDelta;
            Zoom = Math.Clamp(Zoom, Zoom_min, Zoom_max);

            Vector2 after = ScreenToWorld(mouseScreenPos);

            Position += before - after;
        }

        public void Move(Vector2 delta) //ワールド座標上での移動
        {
            Position += delta;
        }

        public void Drag(Vector2 delta) //スクリーン座標の移動量をワールド座標に変換して移動
        {
            Position -= delta / Zoom;
        }

        public Matrix GetViewMatrix() //描画に必要なMatrixを返す(ワールド座標から画面上の座標に変換)
        {
            int w = game.GraphicsDevice.Viewport.Width;
            int h = game.GraphicsDevice.Viewport.Height;
            //カメラの位置に対して、その位置と逆方向に平行移動する。(逆方向の移動はMonoGameのtransformMatrixによるもの)
            //その後Zoom倍率を適用する。
            return Matrix.CreateTranslation(new Vector3(-Position, 0))　*
                   Matrix.CreateScale(Zoom) *
                   Matrix.CreateTranslation(new Vector3(w / 2, h / 2, 0)); 
        }

        public Vector2 ScreenToWorld(Vector2 screenPos) //逆行列を使って画面座標をワールド座標へ
        {
            Matrix inverse = Matrix.Invert(GetViewMatrix());
            return Vector2.Transform(screenPos, inverse);
        }

        public Vector2 WorldToScreen(Vector2 worldPos) //行列をそのまま使ってワールド座標を画面座標に
        {
            return Vector2.Transform(worldPos, GetViewMatrix());
        }
    }
}
