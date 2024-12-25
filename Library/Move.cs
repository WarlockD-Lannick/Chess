using Library.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library
{
    public class Move//перемещение
    {
        private Piece start;
        private Square end;
        private Board board;

        public Move(Board board, Piece start, Square end)
        {
            this.start = start;
            this.end = end;
            this.board = board;
        }

        public Square End => end;

        public int Weight { get; set; }

        public virtual void Execute()
        {
            if (CanPieceBePromoted())
                OnInitiatePawnPromotion?.Invoke(start, new EventArgs());

            var lostPiece = board.ShiftPiece(start, end);
            start.IsFirstMove = false;
            if (lostPiece != null)
                OnPieceCaptured?.Invoke(lostPiece, new EventArgs());
        }
        
        public bool CanPieceBePromoted()//проверка пешки
        {
            if (!(start is Pawn))
                return false;

            return end.Point.PosY == 0 || end.Point.PosY == 7;
        }

        #region Events
        public event EventHandler OnCastlePossible;//при ракировке
        public event EventHandler OnInitiatePawnPromotion;//пешка дошла
        public event EventHandler OnPieceCaptured;//гибель фигуры в страшных муках
        #endregion

        public bool IsEndPosition(Square square)//совпадает ли квадрать с конечной точкой хода
        {
            return end.Point.Equals(square.Point);
        }

        public override string ToString()//Возвращает строковое представление хода
        {
            return start.ToString() + "->" + end.ToString();
        }
    }

    public class Castle : Move//ракировка
    {
        private Piece start;
        private Piece rook;//ладья в ракировке
        private Square end;
        private Board board;

        public Castle(Board board, Piece start, Square end, Square rook) : base(board, start, end)
        {
            this.board = board;
            this.end = end;
            this.start = start;
            this.rook = rook.Piece;
        }

        public override void Execute()
        {
            if (end.Point.PosX < 2)
                board.Squares.FirstOrDefault(x => x.Point.PosY == start.Point.PosY && x.Point.PosX == 2).Piece = rook.Clone() as Rook;
            //куда если с лева
            if (end.Point.PosX > 4)
                board.Squares.FirstOrDefault(x => x.Point.PosY == start.Point.PosY && x.Point.PosX == 4).Piece = rook.Clone() as Rook;
            //если с права

            board.Squares.FirstOrDefault(x => x.Point.Equals(rook.Point)).Piece = null;
            //обычное перемещение

            base.Execute();
        }
    }
}
