/**
 * C# Arrange of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-28
 *  
 * findopen.c -> Util_Findopen.cs
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
using System.Collections.Generic;

namespace Grayscale.GPL.P403____CompThink__.L037_FindOpen
{
    /// <summary>
    /// カレント位置から可能な指し手を探します。
    /// </summary>
    public abstract class Util_FindOpen
    {
        /// <summary>
        /// 指定したポイントについて、全てのリバティーが開いているスペースを探します。
        /// 
        /// 再帰します。
        /// 
        /// Gnugo1.2 では findopen関数。
        /// </summary>
        /// <param name="trayLocations_mutable">可能な動きのための位置の配列。Gnugo1.2 では、i配列、j配列。</param>
        /// <param name="location">Gnugo1.2では、 カレント 行番号 m = 0～18、列番号 n = 0～18。</param>
        /// <param name="color">黒 or 白</param>
        /// <param name="liberty123OfPiece">Gnugo1.2 では minlib 引数。3以下のリバティー</param>
        /// <param name="taikyoku"></param>
        /// <returns></returns>
        public static bool FindOpenLocations
        (
            List<GobanPoint> trayLocations_mutable,
            GobanPoint location,
            StoneColor color,
            int liberty123OfPiece,
            Taikyoku taikyoku
        )
        {
            // この位置はもう調べた、というフラグを立てます。
            taikyoku.MarkingBoard.Done_Current(location);

            // 北ネイバー
            if (!location.IsNorthEnd())//北端でなければ
            {
                if
                (
                    // 北隣が空っぽで。
                    taikyoku.Goban.LookColor_NorthOf(location) == StoneColor.Empty
                    &&
                    // 北隣が、石を取った場所（コウになるかもしれない）でなければ。
                    !taikyoku.MyKo.Is_NorthOf(location)
                )
                {
                    // 北隣を、試す指し手として追加。
                    trayLocations_mutable.Add( location.ToNorth());
                    if (trayLocations_mutable.Count == liberty123OfPiece)
                    {
                        // リバティーの数分追加したなら正常終了。
                        return true;
                    }
                }
                else if
                (
                    // 指定したポイントの北隣が指定の色で。
                    taikyoku.Goban.LookColor_NorthOf(location) == color
                    &&
                    taikyoku.MarkingBoard.CanDo_North(location) // 北側をまだ調べていないなら
                )
                {
                    if
                    (
                        Util_FindOpen.FindOpenLocations(trayLocations_mutable, location.ToNorth(), color, liberty123OfPiece, taikyoku)
                        &&
                        trayLocations_mutable.Count == liberty123OfPiece
                    )
                    {
                        return true;
                    }
                }
            }

            // 南ネイバーを調べます。
            if (!location.IsSouthEnd(taikyoku.GobanBounds))
            {
                if
                (
                    taikyoku.Goban.LookColor_SouthOf(location) == StoneColor.Empty
                    &&
                    // 南隣が、取った石（コウかもしれない）でなければ。
                    !taikyoku.MyKo.Is_SouthOf(location)
                )
                {
                    trayLocations_mutable.Add( location.ToSouth());
                    if (trayLocations_mutable.Count == liberty123OfPiece)
                    {
                        return true;
                    }
                }
                else
                {
                    if
                    (
                        taikyoku.Goban.LookColor_SouthOf(location) == color
                        &&
                        taikyoku.MarkingBoard.CanDo_South(location)
                    )
                    {
                        if
                        (
                            Util_FindOpen.FindOpenLocations(trayLocations_mutable, location.ToSouth(), color, liberty123OfPiece, taikyoku)
                            &&
                            trayLocations_mutable.Count == liberty123OfPiece
                        )
                        {
                            return true;
                        }
                    }
                }
            }

            // 西ネイバーを調べます。
            if (location.J != 0)
            {
                if
                (
                    taikyoku.Goban.LookColor_WestOf(location) == StoneColor.Empty
                    &&
                    // 西隣が、取った石（コウかもしれない）でなければ。
                    taikyoku.MyKo.Is_WestOf(location)
                )
                {
                    trayLocations_mutable.Add( location.ToWest());
                    if (trayLocations_mutable.Count == liberty123OfPiece)
                    {
                        return true;
                    }
                }
                else if
                (
                    taikyoku.Goban.LookColor_WestOf(location) == color
                    &&
                    taikyoku.MarkingBoard.CanDo_West(location)
                )
                {
                    if
                    (
                        Util_FindOpen.FindOpenLocations(trayLocations_mutable, location.ToWest(), color, liberty123OfPiece, taikyoku)
                        &&
                        trayLocations_mutable.Count == liberty123OfPiece
                    )
                    {
                        return true;
                    }
                }
            }

            // 東ネイバーを調べます。
            if (!location.IsEastEnd(taikyoku.GobanBounds))
            {
                if
                (
                    taikyoku.Goban.LookColor_EastOf(location) == StoneColor.Empty
                    &&
                    // 東隣が、取った石（コウかもしれない）でなければ。
                    taikyoku.MyKo.Is_EastOf(location)
                )
                {
                    trayLocations_mutable.Add( location.ToEast());
                    if (trayLocations_mutable.Count == liberty123OfPiece)
                    {
                        return true;
                    }
                }
                else if
                (
                    taikyoku.Goban.LookColor_EastOf(location) == color
                    &&
                    taikyoku.MarkingBoard.CanDo_East(location)
                )
                {
                    if
                    (
                        Util_FindOpen.FindOpenLocations(trayLocations_mutable, location.ToEast(), color, liberty123OfPiece, taikyoku)
                        &&
                        trayLocations_mutable.Count == liberty123OfPiece
                    )
                    {
                        return true;
                    }
                }
            }

            // 開いているポイントを見つけるのに失敗したら
            return false;
        }
    }
}