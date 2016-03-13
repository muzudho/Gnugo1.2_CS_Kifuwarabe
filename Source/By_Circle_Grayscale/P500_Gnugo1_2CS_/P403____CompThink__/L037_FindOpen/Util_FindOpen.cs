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
using Grayscale.GPL.P___190_Board______.L250_Board;
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
        /// <param name="try3Locations">(mutable)隣接する東西南北の最大3方向がこの配列に入るはず。Gnugo1.2 では、i配列、j配列。</param>
        /// <param name="curLocation">Gnugo1.2では、 カレント 行番号 m = 0～18、列番号 n = 0～18。</param>
        /// <param name="color">黒 or 白</param>
        /// <param name="liberty123OfPiece">Gnugo1.2 では minlib 引数。3以下のリバティー</param>
        /// <param name="taikyoku"></param>
        /// <returns></returns>
        public static bool FindOpen3Locations
        (
            List<GobanPoint> try3Locations,
            GobanPoint curLocation,
            StoneColor color,
            int liberty123OfPiece,
            Taikyoku taikyoku
        )
        {
            bool result;
            Board ban = taikyoku.Goban; // 碁盤

            // この位置はもう調べた、というフラグを立てます。
            taikyoku.MarkingBoard.Done_Current(curLocation);

            // 北ネイバー
            if (!curLocation.IsNorthEnd())//北端でなければ
            {
                if
                (
                    // 北隣が空っぽで。
                    ban.NorthOf(curLocation) == StoneColor.Empty
                    &&
                    // 北隣が、石を取った場所（コウになるかもしれない）でなければ。
                    !taikyoku.MyKo.Is_NorthOf(curLocation)
                )
                {
                    // 北隣を候補として追加。
                    try3Locations.Add( curLocation.ToNorth());
                    if (try3Locations.Count == liberty123OfPiece)
                    {
                        // リバティーの数分追加したなら正常終了。
                        result = true;
                        goto gt_EndMethod;
                    }
                }
                else if
                (
                    // 指定したポイントの北隣が指定の色で。
                    ban.NorthOf(curLocation) == color
                    &&
                    taikyoku.MarkingBoard.CanDo_North(curLocation) // 北側をまだ調べていないなら
                )
                {
                    if
                    (
                        Util_FindOpen.FindOpen3Locations(try3Locations, curLocation.ToNorth(), color, liberty123OfPiece, taikyoku)
                        &&
                        try3Locations.Count == liberty123OfPiece
                    )
                    {
                        result = true;
                        goto gt_EndMethod;
                    }
                }
            }

            // 南ネイバーを調べます。
            if (!curLocation.IsSouthEnd(taikyoku.GobanBounds))
            {
                if
                (
                    ban.SouthOf(curLocation) == StoneColor.Empty
                    &&
                    // 南隣が、取った石（コウかもしれない）でなければ。
                    !taikyoku.MyKo.Is_SouthOf(curLocation)
                )
                {
                    try3Locations.Add( curLocation.ToSouth());
                    if (try3Locations.Count == liberty123OfPiece)
                    {
                        result = true;
                        goto gt_EndMethod;
                    }
                }
                else
                {
                    if
                    (
                        ban.SouthOf(curLocation) == color
                        &&
                        taikyoku.MarkingBoard.CanDo_South(curLocation)
                    )
                    {
                        if
                        (
                            Util_FindOpen.FindOpen3Locations(try3Locations, curLocation.ToSouth(), color, liberty123OfPiece, taikyoku)
                            &&
                            try3Locations.Count == liberty123OfPiece
                        )
                        {
                            result = true;
                            goto gt_EndMethod;
                        }
                    }
                }
            }

            // 西ネイバーを調べます。
            if (curLocation.J != 0)
            {
                if
                (
                    ban.WestOf(curLocation) == StoneColor.Empty
                    &&
                    // 西隣が、取った石（コウかもしれない）でなければ。
                    taikyoku.MyKo.Is_WestOf(curLocation)
                )
                {
                    try3Locations.Add( curLocation.ToWest());
                    if (try3Locations.Count == liberty123OfPiece)
                    {
                        result = true;
                        goto gt_EndMethod;
                    }
                }
                else if
                (
                    ban.WestOf(curLocation) == color
                    &&
                    taikyoku.MarkingBoard.CanDo_West(curLocation)
                )
                {
                    if
                    (
                        Util_FindOpen.FindOpen3Locations(try3Locations, curLocation.ToWest(), color, liberty123OfPiece, taikyoku)
                        &&
                        try3Locations.Count == liberty123OfPiece
                    )
                    {
                        result = true;
                        goto gt_EndMethod;
                    }
                }
            }

            // 東ネイバーを調べます。
            if (!curLocation.IsEastEnd(taikyoku.GobanBounds))
            {
                if
                (
                    ban.EastOf(curLocation) == StoneColor.Empty
                    &&
                    // 東隣が、取った石（コウかもしれない）でなければ。
                    taikyoku.MyKo.Is_EastOf(curLocation)
                )
                {
                    try3Locations.Add( curLocation.ToEast());
                    if (try3Locations.Count == liberty123OfPiece)
                    {
                        result = true;
                        goto gt_EndMethod;
                    }
                }
                else if
                (
                    ban.EastOf(curLocation) == color
                    &&
                    taikyoku.MarkingBoard.CanDo_East(curLocation)
                )
                {
                    if
                    (
                        Util_FindOpen.FindOpen3Locations(try3Locations, curLocation.ToEast(), color, liberty123OfPiece, taikyoku)
                        &&
                        try3Locations.Count == liberty123OfPiece
                    )
                    {
                        result = true;
                        goto gt_EndMethod;
                    }
                }
            }

            // 開いているポイントを見つけるのに失敗したら
            result = false;

        gt_EndMethod:
            return result;
        }
    }
}