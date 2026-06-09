
using ResourceMiningGame.Maps.Tiles;

namespace ResourceMiningGame.Maps.Tiles
{
    public static class TileRegistry
    {
        public static Dictionary<TileType, TileAnimationInfo> Data = //タイル種類ごとのアニメーション情報を保持するDictionary変数を定義
            new Dictionary<TileType, TileAnimationInfo>()
            {
                {
                    TileType.Copper, //各タイルアニメーションデータなどについて格納
                    new TileAnimationInfo
                    {
                        TexturePath = "TileUI/copper",
                        FrameCount = 8,
                        FrameTime = 0.25f
                    }
                },
                {
                    TileType.Ground,
                    new TileAnimationInfo
                    {
                        TexturePath = "TileUI/stone",
                        FrameCount = 1,
                        FrameTime = 0.0f
                    }

                }
            };
    }
}
