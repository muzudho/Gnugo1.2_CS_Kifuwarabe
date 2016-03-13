/**
 * C# Arrange 2.0 of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-29
 *  
 * -> new file!
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
using System;

namespace Grayscale.GPL.P900____Scenes_____.L500_Scenes
{
    public class Scene_03_BoardSizeImpl
    {
        /// <summary>
        /// ボードサイズ。
        /// 
        /// 9路盤 or 19路盤。
        /// </summary>
        public int BoardSize { get; set; }

        public Scene_03_BoardSizeImpl()
        {
            // デフォルトでは 19路盤。
            this.BoardSize = 19;
        }


        /// <summary>
        /// ボードサイズを尋ねます。
        /// </summary>
        public void DoScene()//Taikyoku taikyoku
        {
            // ボードサイズを尋ねます。数字を入れてください。
            string message = "Number of board size. default is 19. (9 or 19)?";
            Console.Write(message);

            int boardSize_temp;
            for (; ; )
            {
                bool valid;

                string command_str = Console.ReadLine();
                if(""==command_str.Trim())
                {
                    boardSize_temp = 19;
                    valid = true;
                }
                else if (int.TryParse(command_str, out boardSize_temp))
                {
                    switch (boardSize_temp)
                    {
                        case 9: boardSize_temp = 9; valid = true; break;
                        case 19: boardSize_temp = 19; valid = true; break;
                        default: valid = false; break;
                    }
                }
                else
                {
                    valid = false;
                }

                if(valid)
                {
                    // 入力ok。
                    break;
                }
                else
                {
                    // もう１回
                    Console.Write(message);
                }
            }

            this.BoardSize = boardSize_temp;
        }
    }
}
