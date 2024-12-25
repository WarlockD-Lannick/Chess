namespace Library
{
    public class Game
    {
        private ApplicationSettings settings;


        public Board Board { get; set; }

        public bool ShowPossibleMoves { get; set; }

        public Game(ApplicationSettings settings)//создание доски
        {
            this.settings = settings;

            Board = new Board();
            Reset(true);
        }

        public void Reset(bool init = false)//сброс
        {
            Board.Squares.Clear();
            if (settings.Priority != string.Empty)
            {
                GenerateBoard(settings.BoardXmlPathPriority);
                return;
            }

            if (Board.TopColor == Color.White || init)
                GenerateBoard(settings.BoardXmlPathSwitch1);
            else
                GenerateBoard(settings.BoardXmlPathSwitch2);
        }

        public void Reset(Board board)//сброс на онове второй доски 
        {
            Board.Squares.Clear();
            FillBoard(board);
        }

        private void FillBoard(Board board)//запоолнить с другой
        {
            foreach (var square in board.Squares)
                Board.Squares.Add(square);
        }

        private void GenerateBoard(string path)//генерит доску с файла
        {
            var board = Serializer.ImportFromTxt(path) as Board;
            FillBoard(board);
        }
    }
}
