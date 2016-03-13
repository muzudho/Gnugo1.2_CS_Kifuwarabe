/**
 * C# Arrange of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-28
 *  
 * fioe.c -> Util_OwnEye.cs
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
using Grayscale.GPL.P___190_Board______.L250_Board;
using Grayscale.GPL.P___300_Taikyoku___.L500_Taikyoku;
using Grayscale.GPL.P160____Collection_.L500_Collection;


namespace Grayscale.GPL.P403____CompThink__.L075_OwnEye
{
    /// <summary>
    /// 石を打ち込んだ場所が、自分の目でないかどうか調べます。（非合法手）
    /// コンピューターの思考中、または findnext で使われます。
    /// 
    /// Gnugo1.2 では fioe関数。
    /// </summary>
    public abstract class Util_OwnEye
    {
        /// <summary>
        /// 自分（コンピューター）の目へ打ち込んだとき、真。（非合法手）
        /// </summary>
        /// <param name="location">Gnugo1.2 では、石の 行番号 i = 0～18、列番号 j = 0～18。</param>
        /// <param name="taikyoku"></param>
        /// <returns></returns>
        public static bool IsThis
        (
            GobanPoint location,
            Taikyoku taikyoku
        )
        {
            Board ban = taikyoku.Goban; // 碁盤

            // 上辺を調べます。
            if (location.IsNorthEnd())
            {
                if
                (
                    location.IsWestEnd()    // 北西の角
                    &&
                    // 北西の隅を囲む２つの石が自分（コンピューター）の色なら。
                    ban.At(new GobanPointImpl(1, 0)) == taikyoku.MyColor
                    &&
                    ban.At(new GobanPointImpl(0, 1)) == taikyoku.MyColor
                )
                {
                    return true;
                }
                if
                (
                    location.IsEastEnd(taikyoku.GobanBounds)   // 北東角
                    &&
                    // 北東の隅を囲む２つの石が自分（コンピューター）の色なら。
                    ban.At(new GobanPointImpl(1, taikyoku.GobanBounds.BoardEnd)) == taikyoku.MyColor
                    &&
                    ban.At(new GobanPointImpl(0, taikyoku.GobanBounds.BoardEnd-1)) == taikyoku.MyColor
                )
                {
                    return true;
                }

                if
                (
                    // 上辺で、３方向がコンピューターの石なら
                    ban.At(new GobanPointImpl(1, location.J)) == taikyoku.MyColor // コンピューターの石
                    &&
                    // 左右ともコンピューターの石
                    ban.At(new GobanPointImpl(0, location.J - 1)) == taikyoku.MyColor
                    &&
                    ban.At(new GobanPointImpl(0, location.J + 1)) == taikyoku.MyColor
                )
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            // 下辺を調べます。
            if (location.IsSouthEnd(taikyoku.GobanBounds))
            {
                if
                (
                    location.IsWestEnd()    // 南西角
                    &&
                    // 南西の隅を囲む２つの石がコンピューターの色なら。
                    ban.At(new GobanPointImpl(taikyoku.GobanBounds.BoardEnd-1, 0)) == taikyoku.MyColor
                    &&
                    ban.At(new GobanPointImpl(taikyoku.GobanBounds.BoardEnd, 1)) == taikyoku.MyColor
                )
                {
                    return true;
                }

                if
                (
                    location.IsEastEnd(taikyoku.GobanBounds)   // 南東
                    &&
                    // 南東の隅を囲む２つの石がコンピューターの色なら。
                    ban.At(new GobanPointImpl(taikyoku.GobanBounds.BoardEnd - 1, taikyoku.GobanBounds.BoardEnd)) == taikyoku.MyColor
                    &&
                    ban.At(new GobanPointImpl(taikyoku.GobanBounds.BoardEnd, taikyoku.GobanBounds.BoardEnd-1)) == taikyoku.MyColor
                )
                {
                    return true;
                }

                if
                (
                    // 下辺で、３方向がコンピューターの石なら
                    ban.At(new GobanPointImpl(taikyoku.GobanBounds.BoardEnd-1, location.J)) == taikyoku.MyColor
                    &&
                    ban.At(new GobanPointImpl(taikyoku.GobanBounds.BoardEnd, location.J - 1)) == taikyoku.MyColor
                    &&
                    ban.At(new GobanPointImpl(taikyoku.GobanBounds.BoardEnd, location.J + 1)) == taikyoku.MyColor
                )
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            // 左辺を調べます。
            if (location.IsWestEnd())
            {
                if
                (
                    // 左辺で、３方向がコンピューターの石なら
                    ban.At(new GobanPointImpl(location.I, 1)) == taikyoku.MyColor
                    &&
                    ban.At(new GobanPointImpl(location.I - 1, 0)) == taikyoku.MyColor
                    &&
                    ban.At(new GobanPointImpl(location.I + 1, 0)) == taikyoku.MyColor
                )
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            // 右辺を調べます。
            if (location.IsEastEnd(taikyoku.GobanBounds))
            {
                if
                (
                    // 右辺で、３方向がコンピューターの石なら
                    ban.At(new GobanPointImpl(location.I, taikyoku.GobanBounds.BoardEnd-1)) == taikyoku.MyColor
                    &&
                    ban.At(new GobanPointImpl(location.I - 1, taikyoku.GobanBounds.BoardEnd)) == taikyoku.MyColor
                    &&
                    ban.At(new GobanPointImpl(location.I + 1, taikyoku.GobanBounds.BoardEnd)) == taikyoku.MyColor
                )
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            // 中央のピースを調べます。
            if
            (
                // ４方向がコンピューターの石なら
                ban.NorthOf(location) == taikyoku.MyColor
                &&
                ban.EastOf(location) == taikyoku.MyColor
                &&
                ban.SouthOf(location) == taikyoku.MyColor
                &&
                ban.WestOf(location) == taikyoku.MyColor
            )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}