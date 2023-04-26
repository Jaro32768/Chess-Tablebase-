using Chess;
using System.Text.Json;

Board board = new();
board.PrintBoard();

//example: http://tablebase.lichess.ovh/standard?fen=4k3/6KP/8/8/8/8/7p/8_w_-_-_0_1

using (var client = new HttpClient())
{
    var response = client.GetAsync($"http://tablebase.lichess.ovh/standard?fen={board._fen.Replace(" ", "_")}").Result;
    var json = response.Content.ReadAsStringAsync().Result;
    var chessResult = JsonSerializer.Deserialize<API.ChessResult>(json);
    for (int i = 0; i < chessResult.getMoves().Split(' ').Length - 1; i++)
    {
        Console.WriteLine($"{i + 1}. {chessResult.getMoves().Split(' ')[i]}");
    }
}
Console.WriteLine(JsonSerializer.Serialize(board._chessboard));