using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Color = Microsoft.Xna.Framework.Color;
using Craftory.Maps.Shadow;
using Craftory.Maps.Resource;
using Craftory.Core;

namespace Craftory.Maps.Tiles
{
    public class Tile
    {
        public TileType Type;
        public TileResourceType Resource;
        public bool IsBuildable; //建設可能かどうか
        public Vector2 Position; //タイルの位置（ワールド座標）
        public ITileOccupant? Occupant; //建物やユニットなど

        public List<ShadowSource> ShadowSources;

        public bool IsOccupied => Occupant != null;


        public TileAnimation TerrainAnim;
        public TileAnimation ResourceAnim;

        public static Texture2D BlockedTex;
        protected static Texture2D whiteTex;
        protected static Texture2D blackTex;


        public Tile(TileType Type,
                    TileResourceType resource,
                    Vector2 Position)
        {
            this.Type = Type;
            this.Resource = resource;
            this.Position = Position;
            this.IsBuildable = TileRules.Buildable[Type];

            ShadowSources = new List<ShadowSource>();
            TerrainAnim = TileRegistry.Terrain[Type].CreateTileAnimation();
            ResourceAnim = TileResourceRegistry.Resources[resource]?.CreateTileAnimation();
        }

        public static void Initialize(GraphicsDevice device)
        {
            whiteTex = new Texture2D(device, 1, 1); //白テクスチャをセット
            whiteTex.SetData(new[] { Color.White });

            blackTex = new Texture2D(device, 1, 1);
            blackTex.SetData(new[] { Color.Black });

            BlockedTex = ContentLoader.LoadTexture("TileUI/blocked");
        }

        public void Update(GameTime gameTime)
        {
            TerrainAnim?.Update(gameTime);
            ResourceAnim?.Update(gameTime);
        }

        public void Draw(SpriteBatch sb)
        {
            int tileY = (int)(Position.Y / 32);
            TerrainAnim?.Draw(sb, Position);
            ResourceAnim?.Draw(sb, Position);

            if (!IsBuildable)
            {
                sb.Draw(BlockedTex, Position, Color.White);
                sb.Draw(blackTex,
                    new Rectangle((int)Position.X, (int)Position.Y + 28, 32, 4),
                    Color.Black * 0.4f);
            }
        }

    }
}
