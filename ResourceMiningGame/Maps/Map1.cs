using Tiles = ResourceMiningGame.Maps.Tiles;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;
using ResourceMiningGame.Maps.Tiles;

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
                    TileType terrain = TileType.stone;

                    // 外周を Blocked にする
                    if (x == 0 || y == 0 || x == MapSizeX - 1 || y == MapSizeY - 1)
                        terrain = TileType.blockedStone;   // ← 新しい地形タイプ
                    else
                        terrain = TileType.stone;
                    //資源生成
                    ResourceType resource = 
                        (x + y) % 3 == 0 ? ResourceType.Copper : ResourceType.None;

                    MapTiles[x, y] = new Tile(
                        terrain,
                        resource,
                        new Vector2(x * TileSize, y * TileSize)
                        );
                }
            }
        }

        public override void LoadContent(ContentManager contentManager)
        {

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
