namespace Battleship
{
    /// <summary>
    /// Represents a tile on the Battleship game board.
    /// </summary>
    class Tile
    {
        /// <summary>
        /// Initializes a new instance of the Tile class.
        /// </summary>
        public Tile()
        {
            // Constructor logic can be added here if needed in the future.
        }

        /// <summary>
        /// Gets or sets the position of the tile on the board.
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the tile has been hit.
        /// </summary>
        public bool IsHit { get; set; } = false;
    }
}
