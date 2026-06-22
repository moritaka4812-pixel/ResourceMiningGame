using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Microsoft.Xna.Framework.Graphics;

namespace Craftory.Core
{
    public class ScrollView
    {
        GraphicsDevice device;
        public Rectangle ViewRect; //スクロール表示領域（画面に見える範囲）
        public int ContentHeight;　//スクロール出来る範囲の高さ
        public int ScrollY;  //現在のスクロール位置(上座標)

        public ScrollView(GraphicsDevice device, Rectangle viewRect, int contentHeight)
        {
            this.device = device;
            ViewRect = viewRect;
            ContentHeight = contentHeight;
        }

        public void Update()
        {
            var wheel = Mouse.GetState().ScrollWheelValue; //マウスホイールの回転量

            // マウスホイールの回転に応じてスクロール位置を更新(回転と反対に増減)
            ScrollY -= (int)(Mouse.GetState().ScrollWheelValue * 0.1f);

            // 範囲外にスクロールしないように制限
            ScrollY = Math.Clamp(ScrollY, 0, ContentHeight - ViewRect.Height);

        }

        public void ApplyScissor(SpriteBatch sb, Matrix matrix)
        {
            var raster = new RasterizerState() { ScissorTestEnable = true }; //GPUのラスタライズ設定のうちのScissorを使って描画範囲を切り抜くように指示する設定

            sb.End();//一度spriteBatchを終了して設定を行う
            sb.Begin(rasterizerState: raster, transformMatrix: matrix);

            device.ScissorRectangle = ViewRect;
        }

        public Matrix GetMatrix()//描画時にどれだけ内部座標を動かすかの行列を返す
        {
            return Matrix.CreateTranslation(0, -ScrollY, 0);//描画座標の原点を(0, -ScrollY)に移動させて見かけ上スクロールしているように見せる
        }
    }
}
