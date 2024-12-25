﻿using System;
using System.Collections.Generic;

namespace Library
{
    public class Point //вообщем позиции предоставляет
    {
        private string[] letters = new string[] { "A", "B", "C", "D", "E", "F", "G", "H" };

        public int PosX { get; set; }

        public int PosY { get; set; }

        public Point(int posX, int posY)
        {
            PosX = posX;
            PosY = posY;
        }

        public Point()
        {

        }

        public Point GoToDirection(Point point)//принимает текущюю и перемещяет в нужную
        {
            if (point == null)
                return null;

            var res = this + point;
            if (res.IsInsideTheBoard())//в нутри ли доски
                return res;
            else
                return null;
        }

        public bool IsInsideTheBoard()
        {
            return PosX >= 0 && PosX < 8 && PosY >= 0 && PosY < 8;
        }

        public List<Point> AllMovesWithinDirection(Point end, Point dir)
        {
            var list = new List<Point>();

            if (dir == null)
                return list;

            var start = new Point(PosX, PosY);

            while (!start.Equals(end))
            {
                start = start.GoToDirection(dir);
                list.Add(start);
            }

            return list;
        }

        public List<Point> AllMovesWithinDirection(Point dir)
        {
            var list = new List<Point>();
            var start = new Point(PosX, PosY);

            while (start != null && start.IsInsideTheBoard())
            {
                start = start.GoToDirection(dir);

                if (start != null && start.IsInsideTheBoard())
                    list.Add(start);
            }

            return list;
        }

        public bool IsInDirection(Point end, Point dir)
        {
            var start = new Point(PosX, PosY);
            while (!start.Equals(end) && start.IsInsideTheBoard())
            {
                if (start.Equals(end))
                    return true;

                start = start.GoToDirection(dir);

                if (start == null)
                    return false;

                if (start.Equals(end))
                    return true;
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            var point = obj as Point;
            return PosX == point.PosX && PosY == point.PosY;
        }

        public override string ToString()
        {
            return letters[PosX] + (Board.BoardSize - PosY);
        }

        public static Point operator +(Point b, Point c)
        {
            return new Point(b.PosX + c.PosX, b.PosY + c.PosY);
        }


        public object Clone()
        {
            return new Point()
            {
                PosX = PosX,
                PosY = PosY
            };
        }
    }
}
