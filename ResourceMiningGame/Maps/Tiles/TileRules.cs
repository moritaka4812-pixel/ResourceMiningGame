
namespace ResourceMiningGame.Maps.Tiles
{
    public static class TileRules
    {
        public static Dictionary<TileType, bool> Buildable = new()  //タイルの種類ごとの建設可否
        {
            {TileType.stone, true},
            {TileType.blockedStone, false }
        };
    }
}
