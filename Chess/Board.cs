using System;
using System.Dynamic;

namespace Chess
{
    public class Board
    {
        const string STARTING_FEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        public Square[] _chessboard { get; set; }
        public string _fen { get; set; }
        public int _halfMove { get; set; }
        public int _fullMove { get; set; }
        public int? _enPassant { get; set; }
        public bool _isWhitesMove { get; set; }
        public bool _possibleCastleK { get; set; }
        public bool _possibleCastleQ { get; set; }
        public bool _possibleCastlek { get; set; }
        public bool _possibleCastleq { get; set; }

        public Board()
        {
            _fen = STARTING_FEN;
            _halfMove = 0;
            _fullMove = 1;
            _enPassant = null;
            _isWhitesMove = true;
            _possibleCastleK = true;
            _possibleCastleQ = true;
            _possibleCastlek = true;
            _possibleCastleq = true;
            string fen = GetPiecesFromFen(STARTING_FEN);
            _chessboard = new Square[64];
            for (sbyte i = 0; i < 64; i++)
            {
                _chessboard[i] = new Square(i, fen[i]);
            }

        }
        public Board(string fen)
        {
            _fen = fen;
            _halfMove = int.Parse(fen.Split(' ')[4]);
            _fullMove = int.Parse(fen.Split(' ')[5]);
            _enPassant = fen.Split(' ')[3].Contains('-') ? null : ConvertSquareStringToInt(fen.Split(' ')[3]);
            _isWhitesMove = fen.Split(' ')[1] == "w";
            _possibleCastleK = fen.Split(' ')[2].Contains('K');
            _possibleCastleQ = fen.Split(' ')[2].Contains('Q');
            _possibleCastlek = fen.Split(' ')[2].Contains('k');
            _possibleCastleq = fen.Split(' ')[2].Contains('q');
            string pieces = GetPiecesFromFen(fen);
            _chessboard = new Square[64];
            for (sbyte i = 0; i < 64; i++)
            {
                _chessboard[i] = new Square(i, pieces[i]);
            }
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
        
        private string GetPiecesFromFen(string fen)
        {
            
            string pieces = fen.Split(' ')[0].Replace("8", "        ").Replace("7", "       ").Replace("6", "      ").Replace("5", "     ").Replace("4", "    ").Replace("3", "   ").Replace("2", "  ").Replace("1", " ").Replace("/", "");
            return pieces;
        }

        public string GenerateFEN()
        {
            string pieces = "";
            for (int i = 0; i < 64; i++)
            {
                pieces += _chessboard[i]._piece._symbol;
            }            
            string fen = "";
            int emptySquares = 0;
            for (int i = 0; i < 64; i++)
            {
                if (pieces[i] == ' ')
                {
                    emptySquares++;
                }
                else
                {
                    if (emptySquares != 0)
                    {
                        fen += emptySquares.ToString();
                        emptySquares = 0;
                    }
                    fen += pieces[i];
                }
                if (i % 8 == 7)
                {
                    if (emptySquares != 0)
                    {
                        fen += emptySquares.ToString();
                        emptySquares = 0;
                    }
                    if (i != 63) fen += "/";
                }
            }
            
            fen += " " + (_isWhitesMove ? "w" : "b") + " " +
                (_possibleCastleK ? "K" : "") + (_possibleCastleQ ? "Q" : "") +
                (_possibleCastlek ? "k" : "") + (_possibleCastleq ? "q" : "") +
                (!_possibleCastleK && !_possibleCastleQ && !_possibleCastlek && !_possibleCastleq ? "-" : "") +
                " " + (_enPassant == null ? "-" : ConvertIntToSquareString((int)_enPassant)) +
                " " + _halfMove.ToString() + " " + _fullMove.ToString();
            return fen;
        }

        public void MakeMove(string uciMove)
        {
            int fromSquare = ConvertSquareStringToInt(uciMove.Substring(0, 2));
            int toSquare = ConvertSquareStringToInt(uciMove.Substring(2, 2));
            int? promotionPiece = uciMove.Length == 5 ? uciMove[4] : null;

            // fen update
            if (Char.ToLower(_chessboard[fromSquare]._piece._symbol) == 'p' || _chessboard[toSquare]._piece._symbol != ' ') _halfMove = 0;
            else _halfMove++;

            if (!_isWhitesMove) _fullMove++;
            
            if (Char.ToLower(_chessboard[fromSquare]._piece._symbol) == 'p' && Math.Abs(fromSquare - toSquare) == 16) _enPassant = (fromSquare + toSquare) / 2;
            else _enPassant = null;

            _isWhitesMove = !_isWhitesMove;
            
            if (_chessboard[fromSquare]._piece._symbol == 'K' || _chessboard[toSquare]._piece._symbol == 'K')
            { _possibleCastleK = false; _possibleCastleQ = false; }
            if (_chessboard[fromSquare]._piece._symbol == 'k' || _chessboard[toSquare]._piece._symbol == 'k')
            { _possibleCastlek = false; _possibleCastleq = false; }
            if (_chessboard[fromSquare]._piece._symbol == 'R' || _chessboard[toSquare]._piece._symbol == 'R')
            {
                if (fromSquare == 56) _possibleCastleQ = false;
                if (fromSquare == 63) _possibleCastleK = false;
            }
            if (_chessboard[fromSquare]._piece._symbol == 'r' || _chessboard[toSquare]._piece._symbol == 'r')
            {
                if (fromSquare == 0) _possibleCastleq = false;
                if (fromSquare == 7) _possibleCastlek = false;
            }




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

            _fen = GenerateFEN();

        }

        private int ConvertSquareStringToInt(string square)
        {
            return (8 - (square[1] - '0')) * 8 + (square[0] - 'a');
        }
        private string ConvertIntToSquareString(int num)
        {
            return ((char)('a' + num % 8)).ToString() + (8 - (num / 8)).ToString();
        }

    }
}
