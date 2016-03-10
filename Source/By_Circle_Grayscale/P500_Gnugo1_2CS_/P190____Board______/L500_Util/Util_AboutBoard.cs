/**
 * C# Arrange of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-29
 *
 * matchpat.c
 * enggame.c  -> Util_AboutBoard.cs
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
using Grayscale.GPL.P___160_Collection_.L250_Rectangle;
using Grayscale.GPL.P___160_Collection_.L500_Collection;
using Grayscale.GPL.P160____Collection_.L500_Collection;

namespace Grayscale.GPL.P190____Board______.L500_Util
{
    /// <summary>
    /// 碁盤に関する数字。
    /// </summary>
    public class Util_AboutBoard
    {
        /// <summary>
        /// 盤上にあるか。
        /// 碁番の行列が同じ長さという前提で、行でも列でも両用。
        /// </summary>
        /// <param name="ij"></param>
        /// <returns></returns>
        public static bool In_Board(
            int ij,
            GobanRectangleA gobanBounds
        )
        {
            return 0 <= ij && ij <= gobanBounds.BoardEnd;
        }

        /// <summary>
        /// 盤上の端っこ（両端のどちらでも）にあるか。
        /// 碁番の行列が同じ長さという前提で、行でも列でも両用。
        /// </summary>
        /// <param name="ij"></param>
        /// <returns></returns>
        public static bool On_Edge
        (
            int ij,
            GobanRectangleA gobanBounds
        )
        {
            return ij == 0 || ij == gobanBounds.BoardSize;
        }

        /// <summary>
        /// 交点の番号を、碁盤の座標 i,j に戻します。
        /// 
        /// Gnugo1.2 では、endgame.c の node2ij関数。
        /// </summary>
        /// <param name="out_location"></param>
        /// <param name="node"></param>
        public static void GetLocationByNode(
            out GobanPoint out_location,
            int node,
            GobanRectangleA gobanBounds
        )
        {
            out_location = new GobanPointImpl(
                node / gobanBounds.BoardSize,
                node % gobanBounds.BoardSize
                );
        }

        /// <summary>
        /// 交点の番号。
        /// 
        /// 例えば19路盤であれば、左上から右へ、右上から次の行の左端へと、
        /// 0,1,2…,18、
        /// 19,20,21...37、
        /// といったように番号が振られます。
        /// 
        /// Gnugo1.2 では、endgame.c の node関数。
        /// </summary>
        /// <param name="location">Gnugo1.2 では、i,j という変数名。</param>
        /// <returns></returns>
        public static int GetNodeByLocation
        (
            GobanPoint location,
            GobanRectangleA gobanBounds
        )
        {
            return location.I * gobanBounds.BoardSize + location.J;
        }

    }
}
