using Tiles = ResourceMiningGame.Maps.Tiles;
using Color = Microsoft.Xna.Framework.Color;

namespace ResourceMiningGame.Maps
{
    public class Map1 : MapBase
    {

        ContentManager contentManager;

        public Map1()
        {
            MapTiles = new Tiles.Tile[10, 10]; //簡単なタイルを生成
            MapSizeX = 10;
            MapSizeY = 10;
            
            for (int y = 0; y < MapSizeY; y++)
            {
                for(int x = 0; x < MapSizeX; x++)
                {
                    MapTiles[x, y] = new Tiles.Tile
                    (
                        (x + y) % 3 == 0 ? Tiles.TileType.Copper : Tiles.TileType.Ground, //Type
                        new Vector2(x * 32, y * 32) //Position
                    );
                }
            }
        }

        public override void LoadContent(ContentManager contentManager)
        {
            this.contentManager = contentManager;

            for(int y = 0; y < MapSizeY; y++) //マップ全タイルのスプライトシートとアニメーション情報をロード
            {
                for(int x = 0; x < MapSizeX; x++)
                {
                    var tile = MapTiles[x, y]; //オブジェクト参照でタイルを取得
                    var info = Tiles.TileRegistry.Data[tile.Type]; //タイルの情報を取得

                    tile.SpriteSheet = contentManager.Load<Texture2D>(info.TexturePath); //各タイルの種類ごとの情報をロード
                    tile.FrameCount = info.FrameCount;
                    tile.FrameWidth = info.FrameWidth;
                    tile.FrameHeight = info.FrameHeight;
                    tile.FrameTime = info.FrameTime;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(SpriteBatch sb , VisibleTileRange range) //画面の範囲のみDraw
        {
            for (int y = range.StartY; y < range.EndY; y++)
            {
                for(int x = range.StartX; x < range.EndX; x++)
                {
                    MapTiles[x,y].Draw(sb);
                }
            }
        }
    }
}
