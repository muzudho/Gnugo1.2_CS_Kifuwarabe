/**
 * C# Arrange of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-26
 *  
 * eval.c    -> Util_CountLibertyAll.cs
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
using Grayscale.GPL.P160____Collection_.L500_Collection;
using Grayscale.GPL.P340____Liberty____.L250_Liberty;

namespace Grayscale.GPL.P340____Liberty____.L500_LibertyAll
{
    public abstract class Util_CountLibertyAll
    {
        /// <summary>
        /// 評価関数というよりは、盤上のつながった同じ色の石のリバティーを数えています。
        ///
        /// Gnugo1.2 では、eval 関数。
        /// </summary>
        /// <param name="libertyOfPiece_eachPoint">
        /// 同じ色の石のつながっている部分（piece）のリバティー（四方の石を置けるところ）を数えます。
        /// ピースのうち、最初に指定した１つに この数字は記録されます。
        /// 
        /// Gnugo1.2 では、l という名前のグローバル変数。liberty の略だろうか？
        /// eval で内容が設定され、その内容は exambord、findsavr、findwinr、suicideで使用されます。
        /// </param>
        /// <param name="color">黒 or 白</param>
        public static void Count_LibertyOfPiece_EachPoint
        (
            out int[,] libertyOfPiece_eachPoint,
            StoneColor color,
            Taikyoku taikyoku
        )
        {
            libertyOfPiece_eachPoint = new int[taikyoku.GobanBounds.BoardSize, taikyoku.GobanBounds.BoardSize];

            // それぞれのピースのリバティーを数えます。
            for (int i = 0; i < taikyoku.GobanBounds.BoardSize; i++)
            {
                for (int j = 0; j < taikyoku.GobanBounds.BoardSize; j++)
                {
                    GobanPoint location = new GobanPointImpl(i, j);
                    if (taikyoku.Goban.LookColor(location) == color)// 指定の色のみ
                    {
                        int libertyOfPiece; // Gnugo1.2 では、グローバル変数 lib = 0 でした。
                        Util_CountLiberty.Count_LibertyOfPiece(out libertyOfPiece, location, color, taikyoku);
                        libertyOfPiece_eachPoint[i, j] = libertyOfPiece;
                    }
                }
            }
        }
    }
}