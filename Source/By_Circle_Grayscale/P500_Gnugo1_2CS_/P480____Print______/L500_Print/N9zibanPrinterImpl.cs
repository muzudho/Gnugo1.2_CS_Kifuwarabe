/**
 * C# Arrange of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-28
 *  
 * showbord.c -> BoardPrinterImpl.cs
 */
/*
              GNUGO - the game of Go (Wei-Chi)
             Version 1.2   last revised 10-31-95
        Copyright (C) Free Software Foundation, Inc.
                   written by Man L. Li
                   modified by Wayne Iba
     modified by Frank Pursel <fpp%minor.UUCP@dragon.com>
                 documented by Bob Webber
*/
/*
This program is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation - version 2.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License in file COPYING for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.

Please report any bug/fix, modification, suggestion to

           manli@cs.uh.edu
*/
using Grayscale.GPL.P___160_Collection_.L500_Collection;
using Grayscale.GPL.P___190_Board______.L063_Word;
using Grayscale.GPL.P___300_Taikyoku___.L500_Taikyoku;
using Grayscale.GPL.P___480_Print______.L500_Print;
using Grayscale.GPL.P160____Collection_.L500_Collection;
using System;
using System.Text;

namespace Grayscale.GPL.P480____Print______.L500_Print
{
    /// <summary>
    /// ���݂̌�ՂƁA�v���C���[����\�����܂��B
    /// </summary>
    public class N9zibanPrinterImpl : BoardPrinterB
    {
        /// <summary>
        /// ��Ղ������܂��B9�H�Ղł��B
        /// 
        /// �ՊO�ɁA19�H�Ճf�[�^���\�������@�\���c���Ă���܂��B�i�G���[���F�p�j
        /// 
        /// Gnugo1.2 �ł� showboard �Ƃ����֐��ł��B
        /// </summary>
        /// <param name="taikyoku"></param>
        public void ShowBoard(Taikyoku taikyoku)
        {
            // 3�s ��s�i19�`17�j
            for (int row = 0; row < 3; row++)
            {
                Console.WriteLine();
            }

            //----------------------------------------
            // �s 16
            //----------------------------------------
            //
            // 16�s�ڂɂ́A�u���Ȃ��̐F�́`�v�̕\�L������܂��B
            //
            Console.Write("                                        ");

            if (taikyoku.YourColor == StoneColor.White)
            {
                Console.WriteLine("     Your color: White O");
            }
            else if (taikyoku.YourColor == StoneColor.Black)
            {
                Console.WriteLine("     Your color: Black X");
            }
            else
            {
                Console.WriteLine();
            }

            //----------------------------------------
            // �s 15�`11
            //----------------------------------------
            //
            // 15�s�ڂɂ́A�u�킽���̐F�́`�v�̕\�L������܂��B
            //
            for (int row = 4; row < 9; row++)
            {
                Console.Write("                                        ");

                if (row == 4)
                {
                    if (taikyoku.MyColor == StoneColor.White)
                    {
                        Console.WriteLine("     My color:   White O");
                    }
                    else if (taikyoku.MyColor == StoneColor.Black)
                    {
                        Console.WriteLine("     My color:   Black X");
                    }
                    else
                    {
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine();
                }
            }

            //----------------------------------------
            // �s 10
            //----------------------------------------
            //
            // 10�s�ڂɂ́A�u�킽���̎������́`�v�̕\�L������܂��B
            //
            Console.Write("                                          ");
            Console.WriteLine("     You have captured {0} pieces", taikyoku.Count_MyCaptured);

            Console.Write("   A B C D E F G H J                      ");
            Console.Write("  ");
            //Console.WriteLine("   A B C D E F G H J K L M N O P Q R S T");
            Console.WriteLine("     I have captured {0} pieces", taikyoku.Count_YourCaptured);

            //----------------------------------------
            // �s 9�`8
            //----------------------------------------
            for (int i = 0; i < 2; i++)
            {
                int ii = taikyoku.GobanBounds.BoardSize - i;
                Console.Write("{0,2}", ii);

                Console.WriteLine(this.CreateBoardLine_Normal(i, taikyoku));
            }

            // �s 7
            Console.Write(" 7");
            Console.WriteLine(this.CreateBoardLine_Star(taikyoku.GobanBounds.BoardSize - 7, taikyoku));

            // �s 6�`4
            for (int i = 3; i < 6; i++)
            {
                int ii = taikyoku.GobanBounds.BoardSize - i;
                Console.Write("{0,2}", ii);

                Console.WriteLine(this.CreateBoardLine_Normal(i, taikyoku));
            }

            // �s 3
            Console.Write(" 3");
            Console.WriteLine(this.CreateBoardLine_Star(taikyoku.GobanBounds.BoardSize - 3, taikyoku));

            // �s 2�`1
            for (int i = 7; i < taikyoku.GobanBounds.BoardSize; i++)
            {
                int ii = taikyoku.GobanBounds.BoardSize - i;
                Console.Write("{0,2}", ii);

                Console.WriteLine(this.CreateBoardLine_Normal(i, taikyoku));
            }
            Console.WriteLine("   A B C D E F G H J                    ");
            //Console.WriteLine("   A B C D E F G H J K L M N O P Q R S T");
            Console.WriteLine();
        }

        /// <summary>
        /// ���̂��鐅�����ł��B
        /// </summary>
        /// <param name="i"></param>
        /// <param name="taikyoku"></param>
        /// <returns></returns>
        private string CreateBoardLine_Star(int i, Taikyoku taikyoku)
        {
            StringBuilder sb = new StringBuilder();

            // 0�`1
            for (int j = 0; j < 2; j++)
            {
                GobanPoint location = new GobanPointImpl(i, j);
                if (taikyoku.Goban.At(location) == StoneColor.Empty)
                {
                    sb.Append(" -");
                }
                else if (taikyoku.Goban.At(location) == StoneColor.White)
                {
                    sb.Append(" O");
                }
                else
                {
                    sb.Append(" X");
                }
            }

            // 2
            {
                int j = 2;
                GobanPoint location = new GobanPointImpl(i, j);
                if (taikyoku.Goban.At(location) == StoneColor.Empty)
                {
                    sb.Append(" +");
                }
                else if (taikyoku.Goban.At(location) == StoneColor.White)
                {
                    sb.Append(" O");
                }
                else
                {
                    sb.Append(" X");
                }
            }

            // 3�`5
            for (int j = 3; j < 6; j++)
            {
                GobanPoint location = new GobanPointImpl(i, j);
                if (taikyoku.Goban.At(location) == StoneColor.Empty)
                {
                    sb.Append(" -");
                }
                else if (taikyoku.Goban.At(location) == StoneColor.White)
                {
                    sb.Append(" O");
                }
                else
                {
                    sb.Append(" X");
                }
            }

            // 6
            {
                int j = 6;
                GobanPoint location = new GobanPointImpl(i, j);
                if (taikyoku.Goban.At(location) == StoneColor.Empty)
                {
                    sb.Append(" +");
                }
                else if (taikyoku.Goban.At(location) == StoneColor.White)
                {
                    sb.Append(" O");
                }
                else
                {
                    sb.Append(" X");
                }
            }

            // 7�`9
            for (int j = 7; j < 9; j++)
            {
                GobanPoint location = new GobanPointImpl(i, j);
                if (taikyoku.Goban.At(location) == StoneColor.Empty)
                {
                    sb.Append(" -");
                }
                else if (taikyoku.Goban.At(location) == StoneColor.White)
                {
                    sb.Append(" O");
                }
                else
                {
                    sb.Append(" X");
                }
            }

            int ii = taikyoku.GobanBounds.BoardSize - i;
            sb.Append(String.Format("{0,2}", ii));

            return sb.ToString();
        }

        /// <summary>
        /// ���̂Ȃ��������ł��B
        /// </summary>
        /// <param name="i"></param>
        /// <param name="taikyoku"></param>
        /// <returns></returns>
        private string CreateBoardLine_Normal(int i, Taikyoku taikyoku)
        {
            StringBuilder sb = new StringBuilder();

            for (int j = 0; j < 9; j++)
            {
                GobanPoint location = new GobanPointImpl(i, j);
                if (taikyoku.Goban.At(location) == StoneColor.Empty)
                {
                    sb.Append(" -");
                }
                else if (taikyoku.Goban.At(location) == StoneColor.White)
                {
                    sb.Append(" O");
                }
                else
                {
                    sb.Append(" X");
                }
            }

            int ii = taikyoku.GobanBounds.BoardSize - i;
            sb.Append(String.Format( "{0,2}",ii));

            return sb.ToString();
        }

    }
}