using System;
using System.Text;
namespace Battleship
{
    /// <summary>
    /// Entry point for the Battleship game.
    /// </summary>
    class Program
    {
        private static int numberOfShipsToSink = 1;
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Board board = new Board();
            Console.WriteLine("Welcome to BattleShips! As this is an early version the ships for both sides have been placed randomly");
            Console.WriteLine("When you attack, please type A3 or B4 to select a tile");
            Console.WriteLine("Or type in 1 to see your grid, or 2 to see the rules");
            Console.WriteLine("Press Enter to continue");
            Console.ReadLine();

            do
            {
                Console.WriteLine("Please type a tile (format A3) to attack or select 1 to see your grid or 2 for the rules");
                board.ShowBoard(board.EnemyGrid, false);
                string option = Console.ReadLine();
                board.PlayerMove(option);
                if(board.EnemyShipsDestroyed == numberOfShipsToSink || board.PlayerShipsDestroyed == numberOfShipsToSink)
                {
                    break;
                }
                //Enemy attack
                var random = Board.Pseudorandom();
                var col = random.Item1;
                var row = random.Item2;
                char cha = Convert.ToChar(col);
                string position = cha.ToString() + row;
                board.Attack(position, false);
            } while (board.EnemyShipsDestroyed < numberOfShipsToSink && board.PlayerShipsDestroyed < numberOfShipsToSink);
            if (board.EnemyShipsDestroyed == numberOfShipsToSink)
            {
                Console.WriteLine("Congratulations you won!");
            }
            else
            {
                Console.WriteLine("Defeated");
            }


        }
    }
}