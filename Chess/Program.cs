using Chess;
using System.Text.Json;
using System.Text.RegularExpressions;
using static Chess.API;

Board board = new();

//example: http://tablebase.lichess.ovh/standard?fen=4k3/6KP/8/8/8/8/7p/8_w_-_-_0_1




while (true)
{
    board.PrintBoard();
    using (var client = new HttpClient())
    {
        var response = client.GetAsync($"http://tablebase.lichess.ovh/standard?fen={board._fen.Replace(" ", "_")}").Result;
        var json = response.Content.ReadAsStringAsync().Result;
        var chessResult = JsonSerializer.Deserialize<API.ChessResult>(json);
        for (int i = 0; i < chessResult.getSanMoves().Split(' ').Length - 1; i++)
        {
            Console.WriteLine($"{i + 1}. {chessResult.getSanMoves().Split(' ')[i]}");
        }
        Console.WriteLine("choose move:");
        string answer = Console.ReadLine();
        if (int.TryParse(answer, out int num)) board.MakeMove(chessResult.getUciMoves().Split(' ')[num - 1]);
        else if (chessResult.getSanMoves().Split(' ').Contains(answer))
        {
            board.MakeMove(chessResult.getUciMoves().Split(' ')[Array.IndexOf(chessResult.getSanMoves().Split(' '), answer)]);
        }
        else Console.WriteLine("invalid input");
    }
}
