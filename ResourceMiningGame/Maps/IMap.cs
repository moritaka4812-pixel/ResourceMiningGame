using ResourceMiningGame.Core;
using Point = Microsoft.Xna.Framework.Point;
using Tiles = ResourceMiningGame.Maps.Tiles;

namespace ResourceMiningGame.Maps
{
    public interface IMap //各マップに派生するクラスが実装するインターフェース
    {
        Tiles.Tile[,] MapTiles { get; set; }
        public int MapSizeX { get; set; }
        public int MapSizeY { get; set; }

        void LoadContent(ContentManager contentManager);
        void Update(GameTime gameTime);
        void Draw(SpriteBatch sb, VisibleTileRange range);
        Tiles.Tile GetTile(int x, int y); //指定されたタイルを返すメソッド
        
        VisibleTileRange GetVisibleRange(Camera camera, GraphicsDevice graphics); //ワールド座標上のカメラ範囲を返す

        public Point WorldToTile(Vector2 worldPos);
    }
}
