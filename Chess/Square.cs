using System;

namespace Chess
{
    public class Square
    {
        public sbyte _id { get; set; }
        public Piece _piece { get; set; }

        public Square(sbyte id, char piece)
        {
            _id = id;
            _piece = new(piece);
        }

    }
}
