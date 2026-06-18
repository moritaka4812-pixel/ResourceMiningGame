

using ResourceMiningGame.Core;

namespace ResourceMiningGame.Maps
{
    public abstract class MapBase : IMap
    {
        public Tiles.Tile[,] MapTiles { get; set; }
        public int MapSizeX { get; set; }
        public int MapSizeY { get; set; }
        public int TileSize = 32;

        public VisibleTileRange GetVisibleRange(Camera camera, GraphicsDevice graphics) //マップの必要なワールド座標での描画範囲（タイル範囲）を取得
        {
            int viewportWidth = graphics.Viewport.Width; //ウィンドウ画面の横幅
            int viewportHeight = graphics.Viewport.Height; //ウィンドウ画面の高さ

            // ViewMatrix の逆行列を使ってスクリーン座標から画面のワールド座標を求める
            Matrix inverse = Matrix.Invert(camera.GetViewMatrix());
            //Vector2.Transform(Vector2.Zero, inverse)でウィンドウの左上をワールド座標に
            Vector2 topLeft = Vector2.Transform(Vector2.Zero, inverse); //ワールド座標上でのウィンドウ画面の左上
            Vector2 bottomRight = Vector2.Transform( //ワールド座標上でのウィンドウ画面の右下
                new Vector2(viewportWidth, viewportHeight),
                inverse
            );

            return new VisibleTileRange //画面に移るタイルの開始と終了Indexを返す
            {
                StartX = Math.Max((int)(topLeft.X / TileSize), 0),
                EndX = Math.Min((int)(bottomRight.X / TileSize) + 1, MapSizeX),
                StartY = Math.Max((int)(topLeft.Y / TileSize), 0),
                EndY = Math.Min((int)(bottomRight.Y / TileSize) + 1, MapSizeY)
            };
        }


        public Tiles.Tile GetTile(int x, int y) //指定されたタイルを返す
        {
            return MapTiles[x, y];
        }

        public abstract void LoadContent(ContentManager content);
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch sb, VisibleTileRange range);
    }
}
