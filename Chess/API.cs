using System.Text.Json;
using System.Text.Json.Serialization;

namespace Chess
{
    public class API
    {
        public class ChessResult
        {
            [JsonPropertyName("checkmate")]
            public bool Checkmate { get; set; }

            [JsonPropertyName("stalemate")]
            public bool Stalemate { get; set; }

            [JsonPropertyName("variant_win")]
            public bool VariantWin { get; set; }

            [JsonPropertyName("variant_loss")]
            public bool VariantLoss { get; set; }

            [JsonPropertyName("insufficient_material")]
            public bool InsufficientMaterial { get; set; }

            [JsonPropertyName("dtz")]
            public int? Dtz { get; set; }

            [JsonPropertyName("precise_dtz")]
            public int? PreciseDtz { get; set; }

            [JsonPropertyName("dtm")]
            public int? Dtm { get; set; }

            [JsonPropertyName("category")]
            public string Category { get; set; }

            [JsonPropertyName("moves")]
            public List<ChessMove> Moves { get; set; }
            
            public override string ToString()
            {
                return JsonSerializer.Serialize(this);
            }
            
            public string getMoves()
            {
                string output = string.Empty;
                foreach (var move in Moves)
                {
                    output += move.San + " ";
                }
                return output;
            }

            public string getWinningMoves()
            {
                throw new NotImplementedException();
            }
            public string getDrawingMoves()
            {
                throw new NotImplementedException();
            }

            public string getLosingMoves()
            {
                throw new NotImplementedException();
            }
        }

        public class ChessMove
        {
            [JsonPropertyName("uci")]
            public string Uci { get; set; }

            [JsonPropertyName("san")]
            public string San { get; set; }

            [JsonPropertyName("zeroing")]
            public bool Zeroing { get; set; }

            [JsonPropertyName("checkmate")]
            public bool Checkmate { get; set; }

            [JsonPropertyName("stalemate")]
            public bool Stalemate { get; set; }

            [JsonPropertyName("variant_win")]
            public bool VariantWin { get; set; }

            [JsonPropertyName("variant_loss")]
            public bool VariantLoss { get; set; }

            [JsonPropertyName("insufficient_material")]
            public bool InsufficientMaterial { get; set; }

            [JsonPropertyName("dtz")]
            public int? Dtz { get; set; }

            [JsonPropertyName("precise_dtz")]
            public int? PreciseDtz { get; set; }

            [JsonPropertyName("dtm")]
            public int? Dtm { get; set; }

            [JsonPropertyName("category")]
            public string Category { get; set; }
        }
    }
}
