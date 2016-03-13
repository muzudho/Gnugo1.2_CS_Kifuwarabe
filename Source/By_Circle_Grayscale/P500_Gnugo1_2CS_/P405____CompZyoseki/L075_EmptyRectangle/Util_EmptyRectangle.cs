/**
 * C# Arrange of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-28
 *  
 * openregn.c -> Util_EmptyRectangle.cs
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

namespace Grayscale.GPL.P405____CompZyoseki.L075_EmptyRectangle
{
    public abstract class Util_EmptyRectangle
    {
        /// <summary>
        /// もし四角形 (i1,j1)～(i2,j2)がオープン（全て空っぽ）なら、調べます。
        /// 
        /// Gnugo1.2 では、 openregion関数。
        /// </summary>
        /// <param name="corner1">Gnugo1.2 では、i1,j1 引数。</param>
        /// <param name="corner2">Gnugo1.2 では、i2,j2 引数。</param>
        /// <param name="taikyoku"></param>
        /// <returns></returns>
        public static bool IsEmptyRectangle
        (
            GobanPoint corner1,
            GobanPoint corner2,
            Taikyoku taikyoku
        )
        {
            int minx, maxx, miny, maxy, x, y;

            // 上下の限界を差替えます。
            if (corner1.I < corner2.I)
            {
                miny = corner1.I;
                maxy = corner2.I;
            }
            else
            {
                miny = corner2.I;
                maxy = corner1.I;
            }

            if (corner1.J < corner2.J)
            {
                minx = corner1.J;
                maxx = corner2.J;
            }
            else
            {
                minx = corner2.J;
                maxx = corner1.J;
            }

            // 空っぽ領域を調べます。
            for (y = miny; y <= maxy; y++)
            {
                for (x = minx; x <= maxx; x++)
                {
                    if (taikyoku.Goban.At(new GobanPointImpl(y, x)) != StoneColor.Empty)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}