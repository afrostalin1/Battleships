namespace Battleship
{
    /// <summary>
    /// Represents a ship in the Battleship game.
    /// </summary>
    class Ship : Tile
    {
        
        /// <summary>
        /// Initializes a new instance of the Ship class with the specified ship name.
        /// </summary>
        public Ship()
        {
            
        }

        /// <summary>
        /// Gets or sets the name of the ship.
        /// </summary>
        public string ShipName { get; set; }

    }
}
