/**
 * C# Arrange of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-26
 *  
 * countlib.c
 * count.c    -> Util_CountLiberty.cs
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

namespace Grayscale.GPL.P340____Liberty____.L250_Liberty
{
    public abstract class Util_CountLiberty
    {
        /// <summary>
        /// 碁盤の指定の交点 i,j にある石を起点に、つながっている同じ色の石の（１つ、または連の）総リバーティを数えます。
        /// 再帰的に呼び出されます。
        /// Countlib関数から呼び出してください。
        /// 
        /// Gnugo1.2 では、count関数です。
        /// </summary>
        /// <param name="count">Gnugo1.2 では、グローバル変数 lib でした。</param>
        /// <param name="location">Gnugo1.2では、行番号 i = 0～18、列番号 j = 0～18。</param>
        /// <param name="color">黒 or 白</param>
        /// <param name="taikyoku"></param>
        private static void Count_Recursive(
            ref int count,
            GobanPoint location,
            StoneColor color,
            Taikyoku taikyoku
        )
        {
            //----------------------------------------
            // 実装の解説
            //----------------------------------------
            // 石の周り４方向について、
            // 数えていない空き交点であれば リバティーを １加算します。
            // 数えていなくて、指定した色（主に同じ色）の石であれば、その石から同メソッドを再帰呼び出しします。
            //

            // 指定した位置は、調査済みとしてマークします。
            taikyoku.CountedBoard.Done_Current(location);

            // 北隣の石を調べます。
            if (!location.IsNorthEnd())//北端でなければ
            {
                if
                (
                    taikyoku.Goban.NorthOf(location) == StoneColor.Empty
                    &&
                    taikyoku.CountedBoard.CanDo_North(location)
                )
                {
                    // 北隣が空いていて  まだ数えていないなら、
                    // リバティーを１つ数え上げます。次からは重複して数えません。
                    ++count;
                    taikyoku.CountedBoard.Done_North(location);
                }
                else if
                (
                    taikyoku.Goban.NorthOf(location) == color
                    &&
                    taikyoku.CountedBoard.CanDo_North(location)
                )
                {
                    // 北隣に 指定色の石が置いてあり、まだ数えていないなら、
                    // その石からさらにカウントを続けます。
                    Util_CountLiberty.Count_Recursive(ref count, location.ToNorth(), color, taikyoku);
                }
                // 指定した色でない石が置いてあれば何もしない。
            }

            // 南隣を調べます。
            if (!location.IsSouthEnd(taikyoku.GobanBounds))//南端でなければ
            {
                // もう、だいたい分かるだろう☆（＾▽＾）
                if
                (
                    taikyoku.Goban.SouthOf(location) == StoneColor.Empty
                    &&
                    taikyoku.CountedBoard.CanDo_South(location)
                )
                {
                    ++count;
                    taikyoku.CountedBoard.Done_South(location);
                }
                else if
                (
                    taikyoku.Goban.SouthOf(location) == color
                    &&
                    taikyoku.CountedBoard.CanDo_South(location)
                )
                {
                    Util_CountLiberty.Count_Recursive(ref count, location.ToSouth(), color, taikyoku);
                }
            }

            // 西隣を調べます。
            if (!location.IsWestEnd())//西端でなければ
            {
                if
                (
                    taikyoku.Goban.WestOf(location) == StoneColor.Empty
                    &&
                    taikyoku.CountedBoard.CanDo_West(location)
                )
                {
                    ++count;
                    taikyoku.CountedBoard.Done_West(location);
                }
                else if
                (
                    taikyoku.Goban.WestOf(location) == color
                    &&
                    taikyoku.CountedBoard.CanDo_West(location)
                )
                {
                    Util_CountLiberty.Count_Recursive(ref count, location.ToWest(), color, taikyoku);
                }
            }

            // 東隣を調べます。
            if (!location.IsEastEnd(taikyoku.GobanBounds))//東端でなければ
            {
                if
                (
                    (taikyoku.Goban.EastOf(location) == StoneColor.Empty)
                    &&
                    taikyoku.CountedBoard.CanDo_East(location)
                )
                {
                    ++count;
                    taikyoku.CountedBoard.Done_East(location);
                }
                else if
                (
                    taikyoku.Goban.EastOf(location) == color
                    &&
                    taikyoku.CountedBoard.CanDo_East(location)
                )
                {
                    Util_CountLiberty.Count_Recursive(ref count, location.ToEast(), color, taikyoku);
                }
            }
        }


        /// <summary>
        /// 碁盤の指定の交点 i,j にある石を起点に、つながっている同じ色の（１つ、または連の）石（color piece）の総リバーティを数えます。
        /// 
        /// Gnugo1.2 では、countlib関数です。
        /// </summary>
        /// <param name="out_count">Gnugo1.2 では、グローバル変数 lib でした。</param>
        /// <param name="startLocation_inPiece">Gnugo1.2では、 行番号 m = 0～18、列番号 n = 0～18。</param>
        /// <param name="color">黒 or 白</param>
        public static void Count
        (
            out int out_count,
            GobanPoint startLocation_inPiece,
            StoneColor color,
            Taikyoku taikyoku
        )
        {
            out_count = 0;// Gnugo1.2 では、countlib関数の呼び出し元で グローバル変数を lib = 0 していました。


            // 全てのピースを数えなおせるように、リセットします。
            taikyoku.CountedBoard.FillAll_WeCan(taikyoku.GobanBounds.BoardSize);

            // カレント・ピースのリバティーを数えます。
            Util_CountLiberty.Count_Recursive(ref out_count, startLocation_inPiece, color, taikyoku);
        }
    }
}