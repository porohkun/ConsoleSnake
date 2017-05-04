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
        static int _xPos = 10;
        static int _yPos = 10;
        static int _xPrev = 10;
        static int _yPrev = 10;
        static int _xSpd = 1;
        static int _ySpd = 0;

        static int _speed = 100 * 10000;
        static int _width = 78;
        static int _height = 22;
        static int _xOff = 1;
        static int _yOff = 2;
        static int[,] _field = new int[_width, _height];
        static int _score = 0;

        protected static void WriteAt(string s, int x, int y, bool withOffset = true)
        {
            try
            {
                if (withOffset)
                    Console.SetCursorPosition(x + _xOff, y + _yOff);
                else
                    Console.SetCursorPosition(x, y);
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
            WriteAt("╔",  - 1, - 1);
            WriteAt("╗", _width , - 1);
            WriteAt("╚",  -1, _height );
            WriteAt("╝", _width , _height );
            for (int x = 0; x < _width; x++)
            {
                WriteAt("═", x , -1);
                WriteAt("═", x , _height );
            }
            for (int y = 0; y < _height; y++)
            {
                WriteAt("║",  - 1, y );
                WriteAt("║", _width , y );
            }

            long lastTick = 0;

            while (true)
            {
                if (Console.KeyAvailable)
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.UpArrow: _xSpd = 0; _ySpd = -1; break;
                        case ConsoleKey.DownArrow: _xSpd = 0; _ySpd = 1; break;
                        case ConsoleKey.LeftArrow: _xSpd = -1; _ySpd = 0; break;
                        case ConsoleKey.RightArrow: _xSpd = 1; _ySpd = 0; break;
                        case ConsoleKey.Escape: break;
                    }
                if (DateTime.Now.Ticks - lastTick > _speed)
                {
                    _xPrev = _xPos;
                    _yPrev = _yPos;
                    _xPos += _xSpd;
                    _yPos += _ySpd;
                    if (_xPos >= _width) _xPos = _width-1;
                    if (_xPos <= 0) _xPos = 0;
                    if (_yPos >= _height) _yPos = _height-1;
                    if (_yPos <= 0) _yPos = 0;
                    WriteAt(" ", _xPrev , _yPrev );
                    WriteAt("@", _xPos , _yPos );
                    WriteAt(_score.ToString(), 0, 0, false);
                    lastTick = DateTime.Now.Ticks;
                }

            }

        }
    }
}

