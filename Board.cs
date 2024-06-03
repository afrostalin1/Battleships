using System;
using System.Text.RegularExpressions;
using static Battleship.EmojiCodes;
namespace Battleship
{
    /// <summary>
    /// Represents the Board for Battleships, including the enemy AI and game logic
    /// </summary>
    class Board
    {
        /// <summary>
        /// Represents the players own ships
        /// </summary>
        public List<List<Tile>> PlayerGrid { get; private set; } = new List<List<Tile>>();

        /// <summary>
        /// Represents the enemies ships
        /// </summary>
        public List<List<Tile>> EnemyGrid { get; private set; } = new List<List<Tile>>();

        /// <summary>
        /// Internal counter of how many of the players ships have been destroyed
        /// </summary>       
        public int PlayerShipsDestroyed { get; private set; } = 0;

        /// <summary>
        /// Internal counter of how many of the enemy ships have been destroyed
        /// </summary> 
        public int EnemyShipsDestroyed { get; private set; } = 0;

        /// <summary>
        /// Constructor for the board, uses FillBoard to populate the list with tiles and then AddShips to 
        /// add ships to the grids.
        /// </summary> 
        public Board()
        {
            try
            {
                FillBoard(PlayerGrid);
                AddShips(PlayerGrid);

                FillBoard(EnemyGrid);
                AddShips(EnemyGrid);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while initializing the board: {ex.Message}");
            }


        }

        /// <summary>
        /// Public method allowing the program to display the tile or ships using emojis, depending on 
        /// if its admin or not, you can see where the ships are.
        /// </summary>
        /// <param name="list">The grid where</param>
        /// <param name="isAdmin">True if you want to see exactly where the ships are</param>
        public void ShowBoard(List<List<Tile>> list, bool isAdmin)
        {
            try
            {
                Console.WriteLine("    A    B    C    D    E    F    G    H    I    J");
                for (int i = 0; i < list.Count; i++)
                {
                    Console.Write((i + 1)); // Row number
                    foreach (var tile in list[i])
                    {
                        string emoji = GetEmoji(tile, isAdmin, i);
                        Console.Write(emoji);
                    }
                    Console.WriteLine(); // Move to the next row
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while displaying the board: {ex.Message}");
            }

        }


        /// <summary>
        /// Private method returning emojis to display over the tiles or ship, with some conditional
        /// padding.
        /// </summary>
        /// <param name="tile">The selected tile to represent</param>
        /// <param name="isAdmin">Whether to show all ships or not</param>
        /// <param name="rowIndex">What number row</param>
        /// <returns></returns>
        private string GetEmoji(Tile tile, bool isAdmin, int rowIndex)
        {
            try
            {
                // Admin board logic
                if (isAdmin)
                {
                    if (tile is Ship)
                    {
                        return (rowIndex < 9) ? "  " + CarrierEmoji + " " : " " + CarrierEmoji + " ";
                    }
                    return (rowIndex < 9) ? "  " + WaveEmoji + " " : " " + WaveEmoji + "  ";
                }

                // Player board logic
                if (tile is Ship && tile.IsHit)
                {
                    return (rowIndex < 9) ? "  " + ExplosionEmoji + " " : " " + ExplosionEmoji + " ";
                }
                if (tile.IsHit)
                {
                    return (rowIndex < 9) ? "  " + MissEmoji + " " : " " + MissEmoji + "  ";
                }
                return (rowIndex < 9) ? "  " + WaveEmoji + " " : " " + WaveEmoji + "  ";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while getting emoji for tile: {ex.Message}");
                return ""; // Return empty string if an error occurs
            }

        }

        /// <summary>
        /// Allows the user to take their turn. They can see their own grid, see the rules of the game
        /// and type in the coordinates they want to attack
        /// </summary>
        /// <param name="choice">The user input</param>
        /// <returns></returns>

        public bool PlayerMove(string choice)
        {
            try
            {
                var isMove = false;
                var isSelection = false;
                char first, second, third;
                do
                {
                    switch (choice.Length)
                    {
                        case 2:
                            first = choice[0];
                            second = choice[1];

                            // Validation check to see if input follows the format of A1 or B5 etc.
                            if (char.IsLetter(first) && char.IsDigit(second))
                            {
                                // Capitalizes the letter and then checks to see if the 1st char is A-J and the Second is a number between 1-10
                                first = char.ToUpper(first);

                                if (first < 'K' && int.TryParse(second.ToString(), out _))
                                {
                                    string position = first.ToString() + second.ToString();
                                    // Check to see if tile has already been hit
                                    isSelection = false;
                                    isMove = Attack(position, true);
                                }
                            }
                            break;
                        case 3:
                            first = choice[0];
                            second = choice[1];
                            third = choice[2];

                            // Validation check to see if input follows the format of A1 or B5 etc.
                            if (char.IsLetter(first) && char.IsDigit(second) && char.IsDigit(third))
                            {
                                // Capitalizes the letter and then checks to see if the 1st char is A-J and the Second is a number between 1-10
                                first = char.ToUpper(first);
                                if (first < 'K' && int.TryParse(second.ToString(), out _) && int.TryParse(third.ToString(), out _))
                                {
                                    string position = first.ToString() + second.ToString() + third.ToString();
                                    // Check to see if tile has already been hit
                                    isSelection = false;
                                    isMove = Attack(position, true);
                                }
                            }
                            break;
                        case 1:
                            int selected;
                            if (int.TryParse(choice, out selected))
                            {
                                if (selected < 3)
                                {
                                    if (selected == 1)
                                    {
                                        // Change
                                        ShowBoard(PlayerGrid, true);
                                        isSelection = true;
                                        isMove = false;
                                    }
                                    else if (selected == 2)
                                    {
                                        Console.WriteLine("Welcome to Battleship!\n\nInstructions:\n- Gameplay:\n  - Players take turns calling out coordinates to target enemy ships.\n  - Use letters (A-J) for rows and numbers (1-10) for columns.\n  - If your shot lands on an enemy ship, it's a hit; otherwise, it's a miss.\n  - Sunk ships are announced by their type.\n\n- Winning:\n  - Sink all of your opponent's ships to win the game.\n\n- Strategy Tips:\n  - Deduce the locations of enemy ships based on hits and misses.\n  - Spread out shots to cover the entire grid efficiently.\n\nHave fun playing Battleship!\n");
                                        Console.ReadLine();
                                        isSelection = true;
                                        isMove = false;
                                    }
                                }
                            }
                            break;

                    }
                    if (isSelection)
                    {
                        Console.WriteLine("Please select a coordinate or an option");
                        choice = Console.ReadLine();
                    }
                    else if (!isMove)
                    {
                        Console.WriteLine("Incorrect selection, please select a coordinate or an option");
                        choice = Console.ReadLine();
                    }
                } while (!isMove);



                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while processing player move: {ex.Message}");
                return false; // Return false if an error occurs
            }
        }
        /// <summary>
        /// Lets the player or AI attack
        /// </summary>
        /// <param name="position">The position of the tile to be attacked</param>
        /// <param name="isPlayer">Whether the player or AI is attacking</param>
        /// <returns>True if the attack worked</returns>
        public bool Attack(string position, bool isPlayer)
        {
            try
            {
                //   var ownGrid = isPlayer ?   PlayerGrid : EnemyGrid;
                var targetGrid = isPlayer ? EnemyGrid : PlayerGrid;


                Tile attackedTile = targetGrid.SelectMany(list => list).FirstOrDefault(tile => tile.Position == position);

                if (attackedTile != null)
                {
                    if (attackedTile.IsHit)
                    {
                        Console.WriteLine(isPlayer ? "Tile has already been hit, turn skipped" : "Enemy targeted tile that was already hit, turn skipped");
                        return true;
                    }

                    attackedTile.IsHit = true;

                    if (attackedTile is Ship ship)
                    {
                        Ship? selectedShip = attackedTile as Ship;
                        Console.WriteLine(isPlayer ? $"Enemy ship at {position} was hit" : $"Your ship at {position} was hit");

                        var list = targetGrid.SelectMany(list => list).OfType<Ship>().Where(ship => ship.ShipName == selectedShip.ShipName).ToList();
                        int counter = 0;
                        foreach (Ship ship1 in list)
                        {
                            if (ship1.IsHit == true)
                            {
                                counter++;
                            }
                        }
                        if (counter == list.Count)
                        {
                            if (isPlayer)
                            {
                                EnemyShipsDestroyed++;
                                //Resets position for coordinated attack after a ship is detroyed
                                Console.WriteLine($"Enemy {ship.ShipName} was destroyed");
                            }
                            else
                            {
                                PlayerShipsDestroyed++;
                                //Resets position for coordinated attack after a ship is detroyed
                                Console.WriteLine($"Your {ship.ShipName} was destroyed");
                            }

                        }
                    }
                    else
                    {
                        Console.WriteLine(isPlayer ? "Missed!" : $"Enemy missed at {attackedTile.Position}");
                    }
                    return true;
                }

                Console.WriteLine(isPlayer ? "Error: Tile doesn't exist or has already been hit" : "Error: Enemy targeted non-existing tile");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while processing attack: {ex.Message}");
                return false; // Return false if an error occurs
            }
        }


        /// <summary>
        /// Generates a random column and row for the enemy ai to use.
        /// </summary>
        /// <returns>Int representations of a column, row and bool</returns>
        public static (int, int, int) Pseudorandom()
        {
            try
            {
                //Pseudorandom setup
                var rand = new Random();
                var col = rand.Next('A', 'J' + 1);
                var row = rand.Next(1, 10 + 1);
                var randomBoolNumber = rand.Next(2);

                return (col, row, randomBoolNumber);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while generating pseudorandom numbers: {ex.Message}");
                return (0, 0, 0); // Return default values if an error occurs
            }

        }


        /// <summary>
        /// Internal method to add ships to a grid.
        /// </summary>
        /// <param name="grid">Grid for the ships to be added to</param>
        private static void AddShips(List<List<Tile>> grid)
        {
            try
            {
                AddShip(grid, "Carrier", 5);
                AddShip(grid, "Battleship", 4);
                AddShip(grid, "Cruiser", 3);
                AddShip(grid, "Submarine", 3);
                AddShip(grid, "Destroyer", 2);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding ships: {ex.Message}");
            }
        }

        /// <summary>
        /// Logic to add a ship to a grid. A ship is a tile so multiple tiles need to be changed to make one ship
        /// </summary>
        /// <param name="grid">The grid it'll be added to</param>
        /// <param name="shipType">The name of the ship</param>
        /// <param name="shipLength">How long the ship need to be</param>
        private static void AddShip(List<List<Tile>> grid, string shipType, int shipLength)
        {
            try
            {
                bool repeat = true;
                // // Carrier setup
                do
                {
                    //Pseudorandom setup
                    var random = Pseudorandom();
                    var col = random.Item1;
                    var row = random.Item2;

                    // Out of bounds conditional check
                    bool ColCondition = col + shipLength <= 'J';
                    bool RowCondition = row + shipLength <= 10;
                    if (ColCondition)
                    {
                        //Checks to see if the method placed a ship successfully by seeing if it returned true.
                        if (AddShipsToGrid(col, row, shipLength, shipType, grid, true) == true)
                        {
                            repeat = false;
                        }

                    }
                    else if (RowCondition)
                    {
                        if (AddShipsToGrid(col, row, shipLength, shipType, grid, false) == true)
                        {
                            repeat = false;
                        }

                    }
                } while (repeat);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while replacing tile with ship: {ex.Message}");
            }
        }

        /// <summary>
        /// Adds multiple ship tiles of the same name to the grid
        /// </summary>
        /// <param name="col">Column</param>
        /// <param name="row">Row</param>
        /// <param name="length">Length of the ship</param>
        /// <param name="shipName">Name of the ship</param>
        /// <param name="grid">Grid to add the ship to</param>
        /// <param name="colOrRow">Bool to choose whether the ship should be vertical or horizontal on the grid</param>
        /// <returns>True if successful in adding a ship</returns>
        private static bool AddShipsToGrid(int col, int row, int length, string shipName, List<List<Tile>> grid, bool colOrRow)
        {
            try
            {
                var ShipList = new List<Tile>();
                if (colOrRow)
                {
                    int limit = col + length;
                    //Selects the tiles to put a ship on randomly

                    // For loop checks to see if the selected position is valid e.g. if it isnt null or has a ship on it already
                    //If it does it will return false
                    for (; col < limit; col++)
                    {
                        string tempPosition = Convert.ToChar(col).ToString() + row;
                        Tile? tileToAdd = grid.SelectMany(list => list).FirstOrDefault(tile => tile.Position == tempPosition);
                        if (tileToAdd is Ship || tileToAdd == null)
                        {
                            return false;
                        }
                        else
                        {
                            ShipList.Add(tileToAdd);
                        }
                    }
                    foreach (Tile tile in ShipList)
                    {
                        var position = tile.Position;
                        var ship = new Ship { Position = position, ShipName = shipName };
                        ReplaceTileWithShip(grid, tile, ship);
                    }
                }
                else
                {
                    int limit = row + length;
                    for (; row < limit; row++)
                    {
                        string tempPosition = Convert.ToChar(col).ToString() + row;
                        Tile? tileToAdd = grid.SelectMany(list => list).FirstOrDefault(tile => tile.Position == tempPosition);
                        if (tileToAdd is Ship || tileToAdd == null)
                        {
                            return false;
                        }
                        else
                        {
                            ShipList.Add(tileToAdd);
                        }
                    }
                    foreach (Tile tile in ShipList)
                    {
                        var position = tile.Position;
                        var ship = new Ship { Position = position, ShipName = shipName };
                        ReplaceTileWithShip(grid, tile, ship);
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while filling the board: {ex.Message}");
                return false;
                
            }
        }

        /// <summary>
        /// Replaces a tile with a ship
        /// </summary>
        /// <param name="grid">Grid to use</param>
        /// <param name="selectedTile">Selected tile</param>
        /// <param name="newShip">Ship to be added</param>
        private static void ReplaceTileWithShip(List<List<Tile>> grid, Tile selectedTile, Ship newShip)
        {
            try
            {


                // Find the index of the selected tile in the flattened list
                int selectedIndex = grid.SelectMany(list => list).ToList().IndexOf(selectedTile);

                // Replace the selected tile with the new ship
                int rowIndex = selectedIndex / grid[0].Count;
                int columnIndex = selectedIndex % grid[0].Count;
                grid[rowIndex][columnIndex] = newShip;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while replacing tile with ship: {ex.Message}");
            }
        }


        /// <summary>
        /// Fills the board with tiles, used in the constructor
        /// </summary>
        /// <param name="list">Grid to add tiles to</param>
        /// <returns>Returns the grid with the tiles populated</returns>
        private static List<List<Tile>> FillBoard(List<List<Tile>> list)
        {
            try
            {
                for (int col = 1; col < 11; col++)

                {
                    List<Tile> rowList = new List<Tile>();
                    string rowPosition;



                    for (int row = 0; row < 10; row++)
                    {
                        if (row >= 0 && row <= 10)
                        {
                            //This adds '1' to the char number to turn an A into a B and then a B into a C etc. up until J depending on the iteration.
                            rowPosition = ((char)('A' + row)).ToString();
                        }
                        else
                        {
                            // Handle invalid column values
                            rowPosition = "Invalid";
                        }
                        rowList.Add(new Tile { Position = rowPosition + col });
                    }
                    list.Add(rowList);
                }
                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while filling the board: {ex.Message}");
                return new List<List<Tile>>(); // Return empty list if an error occurs
            }
        }


    }
}