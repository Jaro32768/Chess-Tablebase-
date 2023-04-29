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
            string[] fenParts = fen.Split(' ');
            string pieces = fenParts[0].Replace("8", "        ").Replace("7", "       ").Replace("6", "      ").Replace("5", "     ").Replace("4", "    ").Replace("3", "   ").Replace("2", "  ").Replace("1", " ").Replace("/", "");
            return pieces;
        }

        private string GetFENFromPieces(string pieces)
        {
            throw new NotImplementedException();
        }

        public void MakeMove(string uciMove)
        {
            int fromSquare = ConvertSquareStringToInt(uciMove.Substring(0, 2));
            int toSquare = ConvertSquareStringToInt(uciMove.Substring(2, 2));
            int? promotionPiece = uciMove.Length == 5 ? uciMove[4] : null;

            // en passant
            if (Char.ToLower(_chessboard[fromSquare]._piece._symbol) == 'p' &&
                _chessboard[toSquare]._piece._symbol == ' ' &&
                fromSquare % 8 != toSquare % 8)
            {
                switch (fromSquare - toSquare)
                {
                    case 7:
                        {
                            _chessboard[fromSquare + 1]._piece = new Piece(' ');
                            break;
                        }
                    case 9:
                        {
                            _chessboard[fromSquare - 1]._piece = new Piece(' ');
                            break;
                        }
                    case -7:
                        {
                            _chessboard[fromSquare - 1]._piece = new Piece(' ');
                            break;
                        }
                    case -9:
                        {
                            _chessboard[fromSquare + 1]._piece = new Piece(' ');
                            break;
                        }
                }
            }

            // simple move
            _chessboard[toSquare]._piece = _chessboard[fromSquare]._piece;
            _chessboard[fromSquare]._piece = new Piece(' ');
            
            // promotion
            if (promotionPiece != null) _chessboard[toSquare]._piece = new Piece((char)promotionPiece);

            // castling short
            if (fromSquare - toSquare == -2 && Char.ToLower(_chessboard[toSquare]._piece._symbol) == 'k')
            {
                _chessboard[toSquare - 1]._piece = _chessboard[toSquare + 1]._piece;
                _chessboard[toSquare + 1]._piece = new Piece(' ');
            }
            // castling long
            if (fromSquare - toSquare == 2 && Char.ToLower(_chessboard[toSquare]._piece._symbol) == 'k')
            {
                _chessboard[toSquare + 1]._piece = _chessboard[toSquare - 2]._piece;
                _chessboard[toSquare - 2]._piece = new Piece(' ');
            }

        }

        private int ConvertSquareStringToInt(string square)
        {
            return (8 - (square[1] - '0')) * 8 + (square[0] - 'a');
        }
    }
}
