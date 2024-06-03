using System;
namespace Battleship
{
    /// <summary>
    /// A class containing the Emojis used on the board but saving them as constants 
    /// </summary>
    public static class EmojiCodes
    {
        public static string WaveEmoji { get; } = "\uD83C\uDF0A"; // Wave emoji
        public static string ExplosionEmoji { get; } = "\uD83D\uDCA5"; // Explosion emoji
        public static string CarrierEmoji { get; } = "\uD83D\uDEA2"; // Carrier emoji
        public static string MissEmoji {get; } = "\uD83D\uDEAB"; // Miss

    }
}
