using Craftory.Core;
using Point = Microsoft.Xna.Framework.Point;
using Color = Microsoft.Xna.Framework.Color;

namespace Craftory.Maps.Buildings.Conveyors
{
    public class Conveyor : BuildingInstance
    {
        public ConveyorTile TileLogic { get; private set; }
        public BuildingDirection Direction { get; private set; }

        public Conveyor(Point pos, BuildingDirection dir) : 
            base(BuildType.Conveyor, pos, dir)
        {
            Direction = dir;

            var info = BuildingRegistry.Data[BuildType.Conveyor];
            Anim = info.CreateTileAnimation(dir);

            TileLogic = new ConveyorTile(WorkSpeed);

            SetNextTile();
        }

        public override void UpdateLogic(GameTime gameTime)
        {
            TileLogic.Update(gameTime);
        }

        public override void Draw(SpriteBatch sb, Camera camera)
        {
            //建物描画
            DrawRotated(sb);
            //アイテム描画
            TileLogic.Draw(sb, new Vector2(TilePosition.X * 32, TilePosition.Y * 32), Direction);
        }

        private void SetNextTile()
        {
            var nextPos = Direction switch
            {
                BuildingDirection.Right => new Point(TilePosition.X + 1, TilePosition.Y),
                BuildingDirection.Left => new Point(TilePosition.X - 1, TilePosition.Y),
                BuildingDirection.Up => new Point(TilePosition.X, TilePosition.Y - 1),
                BuildingDirection.Down => new Point(TilePosition.X, TilePosition.Y + 1),
                _ => TilePosition
            };

            var tile = GameCore.Instance.MapManager.Map.GetTile(nextPos.X, nextPos.Y);
            if (tile?.Occupant is Conveyor nextConveyor)
            {
                TileLogic.SetNextTile(nextConveyor.TileLogic);
            }
        }

        private void DrawRotated(SpriteBatch sb)
        {
            var tex = Anim.Texture;
            var frame = Anim.GetCurrentFrameRect();

            float rotation = Direction switch
            {
                BuildingDirection.Right => 0f,
                BuildingDirection.Down => MathF.PI / 2,
                BuildingDirection.Left => MathF.PI,
                BuildingDirection.Up => -MathF.PI / 2,
            };

            Vector2 origin = new(tex.Width / Anim.FrameCount / 2f, tex.Height / 2f);
            Vector2 pos = TilePosition.ToVector2() * 32 + origin;

            sb.Draw(
                tex,
                pos,
                frame,
                Color.White,
                rotation,
                origin,
                1f,
                SpriteEffects.None,
                0f
            );
        }

        public void RefreshNextTile()
        {
            SetNextTile();
        }

        public Point GetNextPosition()
        {
            return Direction switch
            {
                BuildingDirection.Right => new Point(TilePosition.X + 1, TilePosition.Y),
                BuildingDirection.Left => new Point(TilePosition.X - 1, TilePosition.Y),
                BuildingDirection.Up => new Point(TilePosition.X, TilePosition.Y - 1),
                BuildingDirection.Down => new Point(TilePosition.X, TilePosition.Y + 1),
                _ => TilePosition
            };
        }

        public Point GetBackPosition()
        {
            return Direction switch
            {
                BuildingDirection.Right => new Point(TilePosition.X - 1, TilePosition.Y),
                BuildingDirection.Left => new Point(TilePosition.X + 1, TilePosition.Y),
                BuildingDirection.Up => new Point(TilePosition.X, TilePosition.Y + 1),
                BuildingDirection.Down => new Point(TilePosition.X, TilePosition.Y - 1),
                _ => TilePosition
            };
        }
    }
}
