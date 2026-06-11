using ResourceMiningGame.Core;
using ResourceMiningGame.Maps.Tiles;
using ResourceMiningGame.Input;
using ResourceMiningGame.Maps;

namespace ResourceMiningGame.Controller
{
    public class TileSelectionController
    {
        private readonly IMap map;

        public TileSelectionController(IMap map)
        {
            this.map = map;
        }

        public Tile SelectTile(MouseInput mouse, Camera camera) //選択されたタイルを返す
        {
            if(!mouse.LeftClicked()) return null; //マウスが押されていない

            Vector2 screenPos = mouse.Current.Position.ToVector2(); //マウスの位置を取得
            Matrix inverse = Matrix.Invert(camera.GetViewMatrix()); //カメラの逆行列を取得
            Vector2 worldPos = Vector2.Transform(screenPos, inverse); //スクリーン座標からワールド座標に変換

            int tileX = (int)(worldPos.X / 16); //タイル座標に変換
            int tileY = (int)(worldPos.Y / 16);

            if (tileX < 0 || tileX >= map.MapSizeX || tileY < 0 || tileY >= map.MapSizeY) //マップの範囲外
                return null;

            return map.GetTile(tileX, tileY);
        }
    }
}
