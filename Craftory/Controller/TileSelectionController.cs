using Craftory.Core;
using Craftory.Maps.Tiles;
using Craftory.Input;
using Craftory.Maps;

namespace Craftory.Controller
{
    public class TileSelectionController
    {
        private readonly IMap map;

        public TileSelectionController(IMap map)
        {
            this.map = map;
        }

        public TileSelectionResult SelectTile(MouseInput mouse, Camera camera) //選択されたタイルを返す
        {
            if(!mouse.LeftClicked()) return new TileSelectionResult(TileSelectionResultType.None, null); //マウスが押されていない

            Vector2 screenPos = mouse.Current.Position.ToVector2(); //マウスの位置を取得
            Matrix inverse = Matrix.Invert(camera.GetViewMatrix()); //カメラの逆行列を取得
            Vector2 worldPos = Vector2.Transform(screenPos, inverse); //スクリーン座標からワールド座標に変換

            var tilePos = map.WorldToTile(worldPos);

            if (tilePos == null) //マップの範囲外
                return new TileSelectionResult(TileSelectionResultType.Outside, null);

            return new TileSelectionResult(TileSelectionResultType.Selected, map.GetTile(tilePos.Value.X, tilePos.Value.Y));
        }
    }
}
