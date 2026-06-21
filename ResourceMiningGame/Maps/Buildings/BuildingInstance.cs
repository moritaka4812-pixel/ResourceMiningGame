using ResourceMiningGame.Core;
using ResourceMiningGame.Maps.Tiles;
using Point = Microsoft.Xna.Framework.Point;

namespace ResourceMiningGame.Maps.Buildings
{
    public class BuildingInstance : ITileOccupant
    {
        public BuildType Type { get; private set; }     // 建物の種類
        public Point TilePosition { get; private set; } //タイル座標
        public Point SizeInTiles { get; private set; }  //タイル単位の大きさ
        public bool IsActive { get; private set; }      //稼働状況
        public float WorkSpeed { get; private set; }    //採掘速度など、タイプ依存の性能値
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
