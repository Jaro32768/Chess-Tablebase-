using System;

namespace Chess
{
    public class Board
    {
        const string STARTING_FEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        public Square[] _chessboard { get; set; }
        public string _fen { get; set; }

        public Board()
        {
            string fen = GetPiecesFromFEN(STARTING_FEN);
            _chessboard = new Square[64];
            for (sbyte i = 0; i < 64; i++)
            {
                _chessboard[i] = new Square(i, fen[i]);
            }
            _fen = STARTING_FEN;
        }
        public Board(string _fen)
        {
            string fen = GetPiecesFromFEN(_fen);
            _chessboard = new Square[64];
            for (sbyte i = 0; i < 64; i++)
            {
                _chessboard[i] = new Square(i, fen[i]);
            }
            _fen = _fen;
        }

        public void MovePiece(Square from, Square to)
        {
            to._piece = from._piece;
            from._piece = null;
        }

        public void PrintBoard()
        {
            Console.Write("┌───┬───┬───┬───┬───┬───┬───┬───┐\n|");
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Console.Write($" {_chessboard[i * 8 + j]._piece._symbol} |");
                }
                Console.Write("\n├───┼───┼───┼───┼───┼───┼───┼───┤\n|");
            }
            for (int i = 0; i < 8; i++)
            {
                Console.Write($" {_chessboard[56 + i]._piece._symbol} |");
            }
            Console.WriteLine("\n└───┴───┴───┴───┴───┴───┴───┴───┘");

        }
        
        private string GetPiecesFromFEN(string fen)
        {
            //"♔♕♖♗♘♙♚♛♜♝♞♟
            string[] fenParts = fen.Split(' ');
            string pieces = fenParts[0].Replace("8", "        ").Replace("7", "       ").Replace("6", "      ").Replace("5", "     ").Replace("4", "    ").Replace("3", "   ").Replace("2", "  ").Replace("1", " ").Replace("/", "");
            return pieces;
        }
    }
}
