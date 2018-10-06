using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace CodinGameSolutions
{
    namespace UltimateTicTacToe
    {
        //2D array (T [][] or List<List<T>>)
        //[0][1][2]
        //[3][4][5]
        //[6][7][8]
        //Flattened: [0][1][2][3][4][5][6][7][8]
        /**
  * Auto-generated code below aims at helping you parse
  * the standard input according to the problem statement.
  **/
        //Board values are either X - O - "" (empty)
        class Board
        {
            const int MAX_SEARCH_DEPTH = 5;
            int m_indexOfLastMove = -1;
            public Board()
            {
                m_boardValues = new List<string>();
                for (int i = 0; i < 9; ++i) { m_boardValues.Add("");}
            }


            public Board(Board _board, int _indexOfMove, bool _isMax)
            {
                m_boardValues = new List<string>(_board.m_boardValues);
                m_boardValues[_indexOfMove] = _isMax ? "X" : "O";
                m_indexOfLastMove = _indexOfMove;
            }

            List<string> m_boardValues; 
            List<Board> m_possibleBoardList;

            int m_rating = 0;

            public void EvaluateBoard(bool _isMax, int _currentDepth)
            {
                string winner = GetGameWinner();
                if (winner != "")
                {
                    if (winner == "X") { m_rating += 10; }
                    else if (winner == "O") { m_rating -= 10; }
                }
                
                if (_currentDepth >= MAX_SEARCH_DEPTH) { return; }
                m_possibleBoardList = new List<Board>();
                List<int> emptySquareIndices = new List<int>();

                for (int i = 0; i < m_boardValues.Count; ++i)
                {
                    bool isEmpty = m_boardValues[i] == "";

                    if (isEmpty)
                    {
                        emptySquareIndices.Add(i);
                    }
                }

                foreach (int _index in emptySquareIndices)
                {
                    m_possibleBoardList.Add(new Board(this, _index, _isMax));
                }

                if (_currentDepth <= MAX_SEARCH_DEPTH)
                {
                    foreach (Board _board in m_possibleBoardList)
                    {
                        _board.EvaluateBoard(!_isMax, _currentDepth + 1);
                    }
                }

            }

            private string GetGameWinner()
            {
                // ALL HORIZONTAL TIC TAC TOE
                for (int i = 0; i < m_boardValues.Count; i+=3)
                {
                    if (m_boardValues[i] == "") { continue; }
                    if (Extensions.AllEquals(m_boardValues[i], m_boardValues[i+1], m_boardValues[i+2]))
                    {
                        return m_boardValues[i];
                    }
                }

                // ALL VERTICAL TIC TAC TOE
                for (int i = 0; i < 3; ++i)
                {
                    if (m_boardValues[i] == "") { continue; }
                    if (Extensions.AllEquals(m_boardValues[i], m_boardValues[i + 3], m_boardValues[i + 6]))
                    {
                        return m_boardValues[i];
                    }
                }

                //DIAGONAL TIC TAC TOE

                //TOP LEFT TO BOTTOM RIGHT
                if (m_boardValues[0] != "")
                {
                    if (Extensions.AllEquals(m_boardValues[0], m_boardValues[4], m_boardValues[8]))
                    {
                        return m_boardValues[0];
                    }
                }

                //TOP RIGHT TO BOTTOM LEFT
                if (m_boardValues[2] != "")
                {
                    if (Extensions.AllEquals(m_boardValues[2], m_boardValues[4], m_boardValues[6]))
                    {
                        return m_boardValues[0];
                    }
                }

                return "";
            }

            public static int PositionToIndex(int _x, int _y)
            {
                return _y * 3 + _x;
            }

            public static void IndexToPosition(int _index, out int _x, out int _y)
            {
                _y = (int)Math.Floor((_index / 3.0f));
                _x = _index % 3;
            }



            internal void GetBestMove(bool _isMax,out int _x, out int _y)
            {
                if (_isMax)
                {
                    m_possibleBoardList = m_possibleBoardList.OrderByDescending(x => x.m_rating).ToList();
                }
                else
                {
                    m_possibleBoardList = m_possibleBoardList.OrderBy(x => x.m_rating).ToList();
                }

                Board bestBoard = m_possibleBoardList[0];
                IndexToPosition(bestBoard.m_indexOfLastMove,out _x, out _y);
            }

            public string Print()
            {
                string value = "";
                int count = 0;
                for (int i = 0; i < m_boardValues.Count; ++i)
                {
                    ++count;
                    value += "[" +m_boardValues[i] + "]";
                    if (count == 3) { value += Environment.NewLine; count = 0; } 
                }

                value += "\n \n Rating: " + m_rating;
                return value;
            }
        }

        class Player
        {
            const string X = "X";
            const string O = "O";

            /// <summary>
            /// Defines the entry point of the application.
            /// </summary>
            /// <param name="args">The arguments.</param>
            static void Main(string[] args)
            {
                string[] inputs;
                Board currentBoard = new Board();

                // game loop
                while (true)
                {
                    inputs = Console.ReadLine().Split(' ');
                    int opponentRow = int.Parse(inputs[0]);
                    int opponentCol = int.Parse(inputs[1]);


                    int validActionCount = int.Parse(Console.ReadLine());


                    for (int i = 0; i < validActionCount; i++)
                    {
                        inputs = Console.ReadLine().Split(' ');
                        int row = int.Parse(inputs[0]);
                        int col = int.Parse(inputs[1]);
                    }

                    int bestMoveX = 0;
                    int bestMoveY = 0;

                    if (opponentRow != -1)
                    {
                        currentBoard = new Board(currentBoard, Board.PositionToIndex(opponentCol, opponentRow), false);
                    }
                    
                    
                    currentBoard.EvaluateBoard(true, 0);
                    currentBoard.GetBestMove(true, out bestMoveX, out bestMoveY);
                    currentBoard = new Board(currentBoard, Board.PositionToIndex(bestMoveX, bestMoveY), true);
                    currentBoard.EvaluateBoard(true, 0);
                    Console.Error.WriteLine(currentBoard.Print());
                    // Write an action using Console.WriteLine()
                    // To debug: Console.Error.WriteLine("Debug messages...");


                    Console.WriteLine(bestMoveY + " " + bestMoveX);
                }
            }
        }

        public static class Extensions
        {
            static public bool AllEquals(string _firstValue, params string[] _elements)
            {
                return _elements.All(x => x == _firstValue);
            }
        }
    }
}
