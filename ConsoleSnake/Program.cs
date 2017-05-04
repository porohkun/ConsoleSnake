using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleSnake
{
    class Program
    {
        static int _speed = 200 * 10000;
        static Point _size = new Point(78, 22);
        static Point _offset = new Point(1, 2);

        static Random _rnd = new Random();
        static Direction[,] _field = new Direction[_size.X, _size.Y];
        static Point _pos = new Point(10, 10);
        static Point _prevPos = new Point(10, 10);
        static Direction _dir = Direction.East;
        static int _score = 5;
        static int _length = 1;
        static Point _fruit;
        static int _fruitSize = 0;

        protected static void WriteAt(string s, Point pos, bool withOffset = true)
        {
            try
            {
                Point res = pos;
                if (withOffset)
                    res = pos + _offset;
                Console.SetCursorPosition(res.X, res.Y);
                Console.Write(s);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }

        static void Main(string[] args)
        {
            Console.Clear();
            DrawRect(-Point.One, _size);

            long lastTick = 0;

            while (true)
            {
                if (Console.KeyAvailable)
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.UpArrow: _dir = Direction.North; break;
                        case ConsoleKey.DownArrow: _dir = Direction.South; break;
                        case ConsoleKey.LeftArrow: _dir = Direction.West; break;
                        case ConsoleKey.RightArrow: _dir = Direction.East; break;
                        case ConsoleKey.Escape: break;
                    }
                if (DateTime.Now.Ticks - lastTick > _speed)
                {
                    _field[_pos.X, _pos.Y] = _dir;
                    _pos += _dir;

                    if (!_pos.InRect(Point.Zero, _size))
                        break;
                    if (_field[_pos.X, _pos.Y] != Direction.None)
                        break;

                    if (_pos == _fruit)
                    {
                        _score += _fruitSize;
                        _fruitSize = 0;
                    }
                    while (_fruitSize == 0)
                    {
                        _fruit = new Point(_rnd.Next(_size.X), _rnd.Next(_size.Y));
                        if (_field[_fruit.X, _fruit.Y] == Direction.None)
                            _fruitSize = _rnd.Next(10);
                    }
                    WriteAt(_fruitSize.ToString(), _fruit);

                    if (_score == 0)
                    {
                        WriteAt(" ", _prevPos);
                        var pp = _prevPos;
                        _prevPos += _field[_prevPos.X, _prevPos.Y];
                        _field[pp.X, pp.Y] = Direction.None;
                    }
                    else
                    {
                        _score--;
                        _length++;
                    }
                    WriteAt("@", _pos);
                    WriteAt(_score.ToString(), Point.Zero, false);
                    lastTick = DateTime.Now.Ticks;
                }

            }

            DrawRect(new Point(20, 8), new Point(57, 13), true);
            WriteAt("GAME OVER!", new Point(34, 10));
            WriteAt("Length: " + _length, new Point(34, 11));
            Console.SetCursorPosition(0, 25);
            Console.ReadKey();
        }

        private static void DrawRect(Point min, Point max, bool fill = false)
        {
            WriteAt("╔", min);
            WriteAt("╗", new Point(max.X, min.Y));
            WriteAt("╚", new Point(min.X, max.Y));
            WriteAt("╝", max);
            for (int x = min.X + 1; x < max.X; x++)
            {
                WriteAt("═", new Point(x, min.Y));
                WriteAt("═", new Point(x, max.Y));
            }
            for (int y = min.Y + 1; y < max.Y; y++)
            {
                WriteAt("║", new Point(min.X, y));
                WriteAt("║", new Point(max.X, y));
            }
            if (fill)
                for (int x = min.X + 1; x < max.X; x++)
                    for (int y = min.Y + 1; y < max.Y; y++)
                        WriteAt("-", new Point(x, y));
        }
    }

    public enum Direction
    {
        None,
        North,
        South,
        East,
        West
    }

    public struct Point
    {
        public int X;
        public int Y;

        public static readonly Point Zero = new Point(0, 0);
        public static readonly Point One = new Point(1, 1);

        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        #region Методы

        public void Crop(Point min, Point max)
        {
            X = Math.Min(Math.Max(X, min.X), max.X);
            Y = Math.Min(Math.Max(Y, min.Y), max.Y);
        }

        public bool InRect(Point min, Point max)
        {
            if (X < min.X) return false;
            if (Y < min.Y) return false;
            if (X >= max.X) return false;
            if (Y >= max.Y) return false;
            return true;
        }

        public static double Distance(Point P1, Point P2)
        {
            return P1.DistanceTo(P2);
        }

        public double DistanceTo(Point P)
        {
            return Math.Sqrt((this.X - P.X) * (this.X - P.X) + (this.Y - P.Y) * (this.Y - P.Y));
        }

        #endregion

        #region Переназначение операторов

        public static bool operator ==(Point p1, Point p2)
        {
            return p1.X == p2.X && p1.Y == p2.Y;
        }

        public static bool operator !=(Point p1, Point p2)
        {
            return p1.X != p2.X || p1.Y != p2.Y;
        }

        public static Point operator +(Point p1, Point p2)
        {
            return new Point(p1.X + p2.X, p1.Y + p2.Y);
        }

        public static Point operator -(Point p1, Point p2)
        {
            return new Point(p1.X - p2.X, p1.Y - p2.Y);
        }

        public static Point operator -(Point p)
        {
            return new Point(-p.X, -p.Y);
        }

        public static Point operator +(Point p1, Direction dir)
        {
            int x = dir == Direction.East ? 1 : (dir == Direction.West ? -1 : 0);
            int y = dir == Direction.South ? 1 : (dir == Direction.North ? -1 : 0);
            return new Point(p1.X + x, p1.Y + y);
        }

        public static Point operator -(Point p1, Direction dir)
        {
            int x = dir == Direction.East ? -1 : (dir == Direction.West ? 1 : 0);
            int y = dir == Direction.South ? -1 : (dir == Direction.North ? 1 : 0);
            return new Point(p1.X + x, p1.Y + y);
        }

        #endregion

        #region Object

        public override bool Equals(object Point)
        {
            return this == (Point)Point;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode();
        }

        public override string ToString()
        {
            return String.Format("[{0}; {1}]", X, Y);
        }

        #endregion
    }
}

