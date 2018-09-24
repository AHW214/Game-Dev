namespace Game
{
    public static class Dimensions
    {
        public const int PPU = 16;
        public const float PPUScale = 4.0F;
        public const int screenHeightPixels = 1080;

        public static float UPP = 1.0F / PPU;
        public static float screenPPU = PPUScale * PPU;
        public static float screenUPP = 1.0F / screenPPU;
        public static float orthographicSize = screenHeightPixels / (2 * screenPPU);        
    }
}
