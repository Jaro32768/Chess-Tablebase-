using Chess;
using System.Text.Json;
using System.Text.RegularExpressions;
using static Chess.API;

Board board = new();

//example: http://tablebase.lichess.ovh/standard?fen=4k3/6KP/8/8/8/8/7p/8_w_-_-_0_1




while (true)
{
    Console.Clear();
    Console.WriteLine("FEN: " + board.GenerateFEN());
    board.PrintBoard();
    using (var client = new HttpClient())
    {
        var response = client.GetAsync($"http://tablebase.lichess.ovh/standard?fen={board._fen.Replace(" ", "_")}").Result;
        var json = response.Content.ReadAsStringAsync().Result;
        var chessResult = JsonSerializer.Deserialize<API.ChessResult>(json);

        string[] sanMoves = chessResult.getSanMoves().Split(' ');
        int numMoves = sanMoves.Length;

        int numColumns = (int)Math.Ceiling((double)numMoves / 10);
        int maxMoveNumberWidth = (numColumns * 10).ToString().Length;

        for (int i = 0; i < numMoves; i += numColumns)
        {
            for (int j = i; j < Math.Min(i + numColumns, numMoves); j++)
            {
                int moveNumber = j + 1;
                string moveNumberStr = moveNumber.ToString().PadLeft(maxMoveNumberWidth);
                
                if (sanMoves[j].Length < 2) { Console.WriteLine(); break; }

                Console.Write($"{moveNumberStr}. {sanMoves[j],-7}");

                if (moveNumber % numColumns == 0 || j == numMoves - 1)
                {
                    Console.WriteLine();
                }
            }
        }


        Console.WriteLine("choose move:");
        string answer = Console.ReadLine();
        if (int.TryParse(answer, out int num)) board.MakeMove(chessResult.getUciMoves().Split(' ')[num - 1]);
        else if (chessResult.getSanMoves().Split(' ').Contains(answer))
        {
            board.MakeMove(chessResult.getUciMoves().Split(' ')[Array.IndexOf(chessResult.getSanMoves().Split(' '), answer)]);
        }
    }
}
