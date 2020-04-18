using System;
using System.Threading;

namespace Tic_Tac_Toe
{
	public class TicTacToe
	{
		private enum Tile { Empty, Player1, Player2 };
		private Tile[,] board = new Tile[3, 3];
		private Tile currentTurn = Tile.Player1;

		private readonly ConsoleColor pipeColor, player1Color, player2Color, digitColor;
		public const char player1Symbol = 'X', player2Symbol = 'O';

		public TicTacToe()
		{
			pipeColor = ConsoleColor.White;
			player1Color = ConsoleColor.Green;
			player2Color = ConsoleColor.Blue;
			digitColor = ConsoleColor.DarkGray;
		}

		public TicTacToe(ConsoleColor pipeColor, ConsoleColor player1Color,
						 ConsoleColor player2Color, ConsoleColor digitColor)
		{
			this.pipeColor = pipeColor;
			this.player1Color = player1Color;
			this.player2Color = player2Color;
			this.digitColor = digitColor;
		}

		public void Reset()
		{
			board = new Tile[3, 3];
			currentTurn = Tile.Player1;
		}

		private void PlayerTurn()
		{
			byte i;

			do
			{
				Console.Write("Enter the board number you'd like to act on: ");
			} while (!byte.TryParse(Console.ReadLine(), out i) || i > 8 || board[i / 3, i % 3] != Tile.Empty);

			board[i / 3, i % 3] = currentTurn;
			currentTurn = (currentTurn == Tile.Player1) ? Tile.Player2 : Tile.Player1;
		}

		private Tile CheckWinner()
		{
			// Check diagonally
			if (board[0, 0] == board[1, 1] && board[0, 0] == board[2, 2] ||
				board[0, 2] == board[1, 1] && board[0, 2] == board[2, 0])
				return board[1, 1];

			for (int i = 0; i < 3; ++i)
			{
				// Check horizontally
				if (board[i, 0] == board[i, 1] && board[i, 0] == board[i, 2])
					return board[i, 0];

				// Check vertically
				if (board[0, i] == board[1, i] && board[0, i] == board[2, i])
					return board[0, i];
			}

			return Tile.Empty;
		}

		private void ShowBoard()
		{
			Console.ForegroundColor = pipeColor;

			Console.WriteLine("Current turn: Player {0}, or '{1}'", currentTurn == Tile.Player1 ? '1' : '2',
				currentTurn == Tile.Player1 ? player1Symbol : player2Symbol);
			
			Console.WriteLine("          ┌───┬───┬───┐");

			for (int y = 0; y < 3; ++y)
			{
				Console.Write("          ");
				for (int x = 0; x < 3; ++x)
				{
					Console.ForegroundColor = pipeColor;
					Console.Write("│ ");

					switch (board[y, x])
					{
						case Tile.Empty:
							Console.ForegroundColor = digitColor;
							Console.Write($"{y * 3 + x} ");
							break;
						case Tile.Player1:
							Console.ForegroundColor = player1Color;
							Console.Write("{0} ", player1Symbol);
							break;
						case Tile.Player2:
							Console.ForegroundColor = player2Color;
							Console.Write("{0} ", player2Symbol);
							break;
					}
				}
				Console.ForegroundColor = pipeColor;
				Console.WriteLine("│");

				if (y < 2)
					Console.WriteLine("          ├───┼───┼───┤");
			}
			Console.WriteLine("          └───┴───┴───┘");
			Console.ResetColor();
		}

		public void Play()
		{
			Console.Clear();
			Console.WriteLine(@"
████████╗██╗ ██████╗    ████████╗ █████╗  ██████╗    ████████╗ ██████╗ ███████╗
╚══██╔══╝██║██╔════╝    ╚══██╔══╝██╔══██╗██╔════╝    ╚══██╔══╝██╔═══██╗██╔════╝
   ██║   ██║██║            ██║   ███████║██║            ██║   ██║   ██║█████╗  
   ██║   ██║██║            ██║   ██╔══██║██║            ██║   ██║   ██║██╔══╝  
   ██║   ██║╚██████╗       ██║   ██║  ██║╚██████╗       ██║   ╚██████╔╝███████╗
   ╚═╝   ╚═╝ ╚═════╝       ╚═╝   ╚═╝  ╚═╝ ╚═════╝       ╚═╝    ╚═════╝ ╚══════╝

                              by Ben Neilsen


                ┌───────────────────────────────────────┐
                │ Players will take turns vying to      │
                │ align three of their pieces in a row, │
                │ column, or diagonal.                  │
                └───────────────────────────────────────┘


                  Press any key to continue . . .");

			Console.ReadKey();
			
			Tile winner = Tile.Empty;
			for (byte i = 0; i < 9; ++i)
			{
				Console.Clear();
				ShowBoard();
				PlayerTurn();

				// There can only be a winner on the 5th move
				if (i >= 4 && (winner = CheckWinner()) != Tile.Empty)
					break;
			}

			Console.Clear();
			ShowBoard();

			if (winner != Tile.Empty)
				Console.WriteLine(@"
   ┌───────────────────────────────────────┐
   │                                       │
   │              Player {0} won!            │
   │                                       │
   └───────────────────────────────────────┘",
				winner == Tile.Player1 ? '1' : '2');
			else
				Console.WriteLine(@"
   ┌───────────────────────────────────────┐
   │                                       │
   │           There was a draw.           │
   │                                       │
   └───────────────────────────────────────┘");
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			var tictactoe = new TicTacToe();

			while (true)
			{
				tictactoe.Play();
				tictactoe.Reset();
				Console.ReadKey();
			}
		}
	}
}
