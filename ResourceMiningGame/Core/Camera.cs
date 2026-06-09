using Point = Microsoft.Xna.Framework.Point;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;

namespace ResourceMiningGame.Core
{
    public class Camera
    {
        public Vector2 Position; //カメラ位置
        public float Zoom; //初期ズーム
        private float Zoom_min; //最小ズーム
        private float Zoom_max; //最大ズーム
        private int prevScrollValue; //マウスホイールスクロールの以前の値
        private Point prevMousePos;
        private bool isDragging = false;


        public Camera(Vector2 position , float zoom = 1f, float zoom_min = 0.5f, float zoom_max = 3.0f)
        {
            Zoom_min = zoom_min;
            Zoom_max = zoom_max;
            Zoom = zoom;
            Position = position;
        }

        public void Update(GameTime gameTime)
        {
            var mouse = Mouse.GetState();
            var keyboard = Keyboard.GetState();
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //マウスホイールズーム
            int scrollDelta = mouse.ScrollWheelValue - prevScrollValue; //スクロールした差分
            prevScrollValue = mouse.ScrollWheelValue; //以前の値の更新

            if(scrollDelta != 0) //スクロール変化があった場合
            {
                Zoom += scrollDelta > 0 ? 0.1f : -0.1f; //ズームの変化しすぎを防ぐ
                Zoom = Math.Clamp(Zoom, 0.5f, 3f);  //ズーム範囲
            }

            //WASD移動
            float moveSpeed = 500f * dt / Zoom; // Zoomが小さい(縮小状態)なら相対的に低速、Zoomが大きいなら逆
            //WASDに対応した移動を行う
            if (keyboard.IsKeyDown(Keys.W)) Position.Y -= moveSpeed;
            if (keyboard.IsKeyDown(Keys.S)) Position.Y += moveSpeed;
            if (keyboard.IsKeyDown(Keys.A)) Position.X -= moveSpeed;
            if (keyboard.IsKeyDown(Keys.D)) Position.X += moveSpeed;

            //ミドルボタンドラッグ移動
            if (mouse.MiddleButton == ButtonState.Pressed)
            {
                if (!isDragging) //押されていてドラッグ判定がまだなら
                {
                    isDragging = true; //ドラッグ判定
                    prevMousePos = mouse.Position; //マウスポジション取得
                }
                else
                {
                    var delta = mouse.Position - prevMousePos;　//マウス移動量計算
                    prevMousePos = mouse.Position; //マウス位置更新

                    Position -= delta.ToVector2() / Zoom; //マウス移動量に応じて位置更新
                }
            }
            else //ミドルボタンが押されていない
            {
                isDragging = false;
            }
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
