/**
 * C# Arrange 2.0 of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-29
 *  
 * main.c -> Scene_04_HandicapImpl.cs
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
using Grayscale.GPL.P___300_Taikyoku___.L500_Taikyoku;
using Grayscale.GPL.P___480_Print______.L500_Print;
using Grayscale.GPL.P320____Handicap___.L500_Handicap;
using System;

namespace Grayscale.GPL.P900____Scenes_____.L500_Scenes
{
    public class Scene_04_HandicapImpl
    {
        /// <summary>
        /// ハンディーキャップ。
        /// 
        /// 最初に置く黒石の数。
        /// </summary>
        public int Handicap { get; set; }

        public Scene_04_HandicapImpl()
        {
            this.Handicap = 0;
        }


        /// <summary>
        /// ハンディーキャップを尋ねます。
        /// 
        /// Gnugo1.2 では、main.c の main関数。
        /// </summary>
        public void DoScene(Taikyoku taikyoku)
        {
            // ハンディーキャップを尋ねます。
            Console.Write("Number of handicap for black (0 to 17)? ");

            // 数字を入れてください。
            int handicap_temp;
            while (!int.TryParse(Console.ReadLine(), out handicap_temp))
            {
                // もう１回
                Console.Write("Number of handicap for black (0 to 17)? ");
            }
            this.Handicap = handicap_temp;
            //scanf("%d", ref i);
            //getchar();

            switch(taikyoku.GobanBounds.BoardSize)
            {
                case 9: Util_Handicap_9ziban.DoHandicap(this.Handicap, taikyoku); break;
                default: Util_Handicap_19ziban.DoHandicap(this.Handicap, taikyoku); break;
            }
            

            // ハンディーキャップを選択したところで、ゲーム盤を表示します。
            ((BoardPrinterB)taikyoku.BoardPrinter).ShowBoard(taikyoku);
        }
    }
}
