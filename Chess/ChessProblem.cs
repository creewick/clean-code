using System.Linq;

namespace Chess
{
	public class ChessProblem
	{
		private static Board board;
	    public static ChessStatus ChessStatus;

	    public static void LoadFrom(string[] lines)
		{
			board = new BoardParser().ParseBoard(lines);
		}

		// Определяет мат, шах или пат белым.
		public static ChessStatus CalculateChessStatus(PieceColor color)
		{
			var isCheck = IsCheck(color);
			var hasMoves = DoesKingHasMoves(color);
			foreach (var locFrom in board.GetPieces(PieceColor.White))
			{
				foreach (var locTo in board.GetPiece(locFrom).GetMoves(locFrom, board))
				{
					var old = board.GetPiece(locTo);
					board.Set(locTo, board.GetPiece(locFrom));
					board.Set(locFrom, null);
					if (!IsCheck(color))
						hasMoves = true;
					board.Set(locFrom, board.GetPiece(locTo));
					board.Set(locTo, old);
				}
			}

			if (isCheck)
				if (hasMoves)
					ChessStatus = ChessStatus.Check;
				else ChessStatus = ChessStatus.Mate;
			else if (hasMoves) ChessStatus = ChessStatus.Ok;
			else ChessStatus = ChessStatus.Stalemate;

		    return ChessStatus;
		}

	    private static bool DoesKingHasMoves(PieceColor color)
	    {
	        foreach (var locFrom in board.GetPieces(PieceColor.White))
	        {
	            foreach (var locTo in board.GetPiece(locFrom).GetMoves(locFrom, board))
	            {
	                var old = board.GetPiece(locTo);
	                board.Set(locTo, board.GetPiece(locFrom));
	                board.Set(locFrom, null);
	                if (!IsCheck(color))
	                    return true;
	                board.Set(locFrom, board.GetPiece(locTo));
	                board.Set(locTo, old);
	            }
	        }
	        return false;
	    }
        // check — это шах
        private static bool IsCheck(PieceColor color)
		{
		    var enemyColor = color == PieceColor.White 
                ? PieceColor.Black 
                : PieceColor.White;
			foreach (var location in board.GetPieces(enemyColor))
			{
				var piece = board.GetPiece(location);
				var moves = piece.GetMoves(location, board);
				if (moves.Any(destination => board.GetPiece(destination).Is(color, PieceType.King)))
				    return true;
			}
			return false;
		}
	}
}