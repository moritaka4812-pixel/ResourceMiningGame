using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Craftory.Maps.Tiles;
using Craftory.Core;

namespace Craftory.Renderer
{
    public class TileSelectionRenderer
    {
        private Texture2D pixel;

        public TileSelectionRenderer(GraphicsDevice device)
        {
            pixel = new Texture2D(device, 1, 1);
            pixel.SetData(new[] { Color.White });
        }

        public void DrawScreenSpace(SpriteBatch sb, Tile tile, Camera camera)
        {
            if (tile == null) return;

            // ワールド → スクリーン座標
            Vector2 screenPos = Vector2.Transform(tile.Position, camera.GetViewMatrix());

            float zoom = camera.Zoom;
            int size = (int)(32 * zoom);   // ズームに合わせてタイルの大きさを補正
            int thickness = 1;             // 線の太さは固定（ズームの影響を受けない）

            // 上
            sb.Draw(pixel, new Rectangle(
                (int)screenPos.X, (int)screenPos.Y,
                size, thickness), Color.Yellow);

            // 下
            sb.Draw(pixel, new Rectangle(
                (int)screenPos.X, (int)screenPos.Y + size - thickness,
                size, thickness), Color.Yellow);

            // 左
            sb.Draw(pixel, new Rectangle(
                (int)screenPos.X, (int)screenPos.Y,
                thickness, size), Color.Yellow);

            // 右
            sb.Draw(pixel, new Rectangle(
                (int)screenPos.X + size - thickness, (int)screenPos.Y,
                thickness, size), Color.Yellow);
        }

    }
}
