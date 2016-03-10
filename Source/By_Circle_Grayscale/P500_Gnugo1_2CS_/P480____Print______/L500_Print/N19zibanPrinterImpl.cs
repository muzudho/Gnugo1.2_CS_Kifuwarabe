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
    public class N19zibanPrinterImpl : BoardPrinterB
    {
        /// <summary>
        /// ��Ղ������܂��B19�H�Ղł��B
        /// 
        /// Gnugo1.2 �ł� showboard �Ƃ����֐��ł��B
        /// </summary>
        /// <param name="taikyoku"></param>
        public void ShowBoard(Taikyoku taikyoku)
        {
            // p = 0 for empty ,
            // p = 1 for white piece,
            // p = 2 for black piece
            
            Console.WriteLine("   A B C D E F G H J K L M N O P Q R S T");

            // �s 19�`17
            for (int i = 0; i < 3; i++)
            {
                int ii = taikyoku.GobanBounds.BoardSize - i;
                Console.Write("{0,2}", ii);

                Console.Write(this.CreateBoardLine_Normal(i,taikyoku));

                Console.WriteLine("{0,2}", ii);
            }

            //----------------------------------------
            // �s 16
            //----------------------------------------
            //
            // 16�s�ڂɂ́A��������̂ł��������؂�܂��B�܂��A�u���Ȃ��̐F�́`�v�̕\�L������܂��B
            //
            Console.Write("16");

            Console.Write(this.CreateBoardLine_Star(taikyoku.GobanBounds.BoardSize - 16, taikyoku));

            Console.Write("16");

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
            for (int i = 4; i < 9; i++)
            {
                int ii = taikyoku.GobanBounds.BoardSize - i;
                Console.Write("{0,2}", ii);

                Console.Write( this.CreateBoardLine_Normal(i,taikyoku) );

                Console.Write("{0,2}", ii);
                if (i == 4)
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
                else if (i == 8)
                {
                    Console.WriteLine("     You have captured {0} pieces", taikyoku.Count_MyCaptured);
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
            // 10�s�ڂɂ́A��������̂ł��������؂�܂��B�܂��A�u�킽���̎������́`�v�̕\�L������܂��B
            //
            Console.Write("10");

            Console.Write(this.CreateBoardLine_Star(taikyoku.GobanBounds.BoardSize - 10, taikyoku));

            Console.Write("10");
            Console.WriteLine("     I have captured {0} pieces", taikyoku.Count_YourCaptured);

            //----------------------------------------
            // �s 9�`5
            //----------------------------------------
            for (int i = 10; i < 15; i++)
            {
                int ii = taikyoku.GobanBounds.BoardSize - i;
                Console.Write("{0,2}", ii);

                Console.Write(this.CreateBoardLine_Normal(i,taikyoku));

                Console.WriteLine("{0,2}", ii);
            }
    
            // �s 4
            Console.Write(" 4");

            Console.Write(this.CreateBoardLine_Star(taikyoku.GobanBounds.BoardSize - 4, taikyoku));

            Console.Write(" 4");
            Console.WriteLine();

            // �s 3�`1
            for (int i = 16; i < taikyoku.GobanBounds.BoardSize; i++)
            {
                int ii = taikyoku.GobanBounds.BoardSize - i;
                Console.Write("{0,2}", ii);

                Console.Write(this.CreateBoardLine_Normal(i,taikyoku));

                Console.Write("{0,2}", ii);
                Console.WriteLine();
            }
            Console.WriteLine("   A B C D E F G H J K L M N O P Q R S T");
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

            for (int j = 0; j < 3; j++)
            {
                GobanPoint location = new GobanPointImpl(i, j);
                if (taikyoku.Goban.LookColor(location) == StoneColor.Empty)
                {
                    sb.Append(" -");
                }
                else if (taikyoku.Goban.LookColor(location) == StoneColor.White)
                {
                    sb.Append(" O");
                }
                else
                {
                    sb.Append(" X");
                }
            }

            {
                int j = 3;
                GobanPoint location = new GobanPointImpl(i, j);
                if (taikyoku.Goban.LookColor(location) == StoneColor.Empty)
                {
                    sb.Append(" +");
                }
                else if (taikyoku.Goban.LookColor(location) == StoneColor.White)
                {
                    sb.Append(" O");
                }
                else
                {
                    sb.Append(" X");
                }
            }

            for (int j = 4; j < 9; j++)
            {
                GobanPoint location = new GobanPointImpl(i, j);
                if (taikyoku.Goban.LookColor(location) == StoneColor.Empty)
                {
                    sb.Append(" -");
                }
                else if (taikyoku.Goban.LookColor(location) == StoneColor.White)
                {
                    sb.Append(" O");
                }
                else
                {
                    sb.Append(" X");
                }
            }

            {
                int j = 9;
                GobanPoint location = new GobanPointImpl(i, j);
                if (taikyoku.Goban.LookColor(location) == StoneColor.Empty)
                {
                    sb.Append(" +");
                }
                else if (taikyoku.Goban.LookColor(location) == StoneColor.White)
                {
                    sb.Append(" O");
                }
                else
                {
                    sb.Append(" X");
                }
            }

            for (int j = 10; j < 15; j++)
            {
                GobanPoint location = new GobanPointImpl(i, j);
                if (taikyoku.Goban.LookColor(location) == StoneColor.Empty)
                {
                    sb.Append(" -");
                }
                else if (taikyoku.Goban.LookColor(location) == StoneColor.White)
                {
                    sb.Append(" O");
                }
                else
                {
                    sb.Append(" X");
                }
            }

            {
                int j = 15;
                GobanPoint location = new GobanPointImpl(i, j);
                if (taikyoku.Goban.LookColor(location) == StoneColor.Empty)
                {
                    sb.Append(" +");
                }
                else if (taikyoku.Goban.LookColor(location) == StoneColor.White)
                {
                    sb.Append(" O");
                }
                else
                {
                    sb.Append(" X");
                }
            }

            for (int j = 16; j < taikyoku.GobanBounds.BoardSize; j++)
            {
                GobanPoint location = new GobanPointImpl(i, j);
                if (taikyoku.Goban.LookColor(location) == StoneColor.Empty)
                {
                    sb.Append(" -");
                }
                else if (taikyoku.Goban.LookColor(location) == StoneColor.White)
                {
                    sb.Append(" O");
                }
                else
                {
                    sb.Append(" X");
                }
            }

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

            for (int j = 0; j < taikyoku.GobanBounds.BoardSize; j++)
            {
                GobanPoint stoneLocation = new GobanPointImpl(i, j);
                if (taikyoku.Goban.LookColor(stoneLocation) == StoneColor.Empty)
                {
                    sb.Append(" -");
                }
                else if (taikyoku.Goban.LookColor(stoneLocation) == StoneColor.White)
                {
                    sb.Append(" O");
                }
                else
                {
                    sb.Append(" X");
                }
            }

            return sb.ToString();
        }

    }
}