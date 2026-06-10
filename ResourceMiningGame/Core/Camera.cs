using Point = Microsoft.Xna.Framework.Point;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;

namespace ResourceMiningGame.Core
{
    public class Camera
    {
        public Vector2 Position; //カメラ位置
        public float Zoom; //現在のズーム
        private float Zoom_min; //最小ズーム
        private float Zoom_max; //最大ズーム

        public Camera(Vector2 position , float zoom = 1f, float zoom_min = 0.5f, float zoom_max = 3.0f)
        {
            Zoom_min = zoom_min;
            Zoom_max = zoom_max;
            Zoom = zoom;
            Position = position;
        }

        public void ZoomBy(float amount) //amountだけzoom値を増減させる
        {
            Zoom = Math.Clamp(Zoom + amount, Zoom_min, Zoom_max);
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
            //カメラの位置に対して、その位置と逆方向に平行移動する。(逆方向の移動はMonoGameのtransformMatrixによるもの)
            //その後Zoom倍率を適用する。
            return Matrix.CreateTranslation(new Vector3(-Position, 0))　*
                   Matrix.CreateScale(Zoom); 
        }
    }
}
