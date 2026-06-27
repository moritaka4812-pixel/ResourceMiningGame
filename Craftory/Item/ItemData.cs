
namespace Craftory.Item
{
    public class ItemData
    {
        public ItemType Type { get; set; }
        public ItemCategory Category { get; set; }
        public string TexturePath { get; set; }

        public Texture2D Texture { get; set; }
    }
}
