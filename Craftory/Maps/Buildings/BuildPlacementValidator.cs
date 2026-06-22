using Point = Microsoft.Xna.Framework.Point;

namespace Craftory.Maps.Buildings
{
    public class BuildPlacementValidator
    {
        private IMap map;
        private bool[,] previewOccupied;

        public BuildPlacementValidator(IMap map, bool[,] previewOccupied)
        {
            this.map = map;
            this.previewOccupied = previewOccupied;
        }

        public bool CanPlace(BuildingInfo info, Point origin)
        {
            foreach(var pos in info.GetArea(origin)) //建物の各タイル位置に対して
            {
                var t = map.GetTile(pos.X, pos.Y);
                if (t == null || t.IsOccupied || !t.IsBuildable) //マップが配置不可
                    return false;

                if (previewOccupied[pos.X, pos.Y]) //プレビューが配置不可
                    return false;
            }
            return true;
        }
    }
}
