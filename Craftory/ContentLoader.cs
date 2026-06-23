namespace Craftory
{
    public static class ContentLoader
    {
        private static ContentManager content;

        public static void Initialize(ContentManager cm)
        {
            content = cm;
        }

        public static Texture2D LoadTexture(string path)
        {
            return content.Load<Texture2D>(path);
        }

        public static SpriteFont LoadFont(string path)
        {
            return content.Load<SpriteFont>(path);
        }
    }
}
