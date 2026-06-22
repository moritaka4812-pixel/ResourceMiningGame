using Craftory.Core;
using Craftory.Maps.Tiles;
using Point = Microsoft.Xna.Framework.Point;

namespace Craftory.Maps.Buildings
{
    public class BuildingInstance : ITileOccupant
    {
        public BuildType Type { get; private set; }     // 建物の種類
        public Point TilePosition { get; private set; } //タイル座標
        public Point SizeInTiles { get; private set; }  //タイル単位の大きさ
        public bool IsActive { get; private set; }      //稼働状況
        public float WorkSpeed { get; private set; }    //採掘速度など、タイプ依存の性能値
        public List<Point> OccupiedTiles { get; private set; }
        public TileAnimation Anim;

        public BuildingInstance(BuildType type, Point tilePosition)
        {
            Type = type;
            TilePosition = tilePosition;

            var info = BuildingRegistry.Data[type];

            Anim = info.CreateTileAnimation();

            SizeInTiles = info.SizeInTiles;
            WorkSpeed = info.WorkSpeed;
            IsActive = true;

            OccupiedTiles = new List<Point>();
            for(int x = 0; x < info.Width; x++)
            {
                for(int y = 0; y < info.Height; y++)
                {
                    OccupiedTiles.Add(new Point(tilePosition.X + x, tilePosition.Y + y));
                }
            }
        }

        public virtual void UpdateLogic(GameTime gameTime)
        {
            if(!IsActive) return;
            
        }

        public virtual void UpdateVisual(GameTime gameTime)
        {
            if(!IsActive) return;
            Anim.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch sb, Camera camera)
        {
            var worldPos = TilePosition.ToVector2() * 32;
            // 建物のスプライト描画
            Anim.Draw(sb, worldPos);
        }
    }
}
