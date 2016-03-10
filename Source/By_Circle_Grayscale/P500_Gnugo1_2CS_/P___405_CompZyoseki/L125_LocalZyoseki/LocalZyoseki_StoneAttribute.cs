/**
 * C# Arrange of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-28
 *  
 * -> new file.
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
namespace Grayscale.GPL.P___405_CompZyoseki.L125_LocalZyoseki
{
    /// <summary>
    /// 局地的な定石の、石の属性。
    /// 
    /// Gnugo1.2 では、構造体の att プロパティー。
    /// </summary>
    public enum LocalZyoseki_StoneAttribute
    {
        /// <summary>
        /// 空いている交点。
        /// </summary>
        _0_Empty = 0,

        /// <summary>
        /// 相手（人間）の石。
        /// </summary>
        _1_YourPiece = 1,

        /// <summary>
        /// 定石の図形の基点（0,0）となる、自分（コンピューター）の石。
        /// </summary>
        _2_MyPiece = 2,

        /// <summary>
        /// 自分（コンピューター）が次に置いていく石。
        /// </summary>
        _3_MyNextMove = 3,

        /// <summary>
        /// 開いている交点。盤の端っこにある。
        /// </summary>
        _4_EmptyOnEdge = 4,

        /// <summary>
        /// 相手（人間）が次に置いていく石。
        /// </summary>
        _5_YourPieceOnEdge = 5,

        /// <summary>
        /// 定石の図形の基点（0,0）となる、自分（コンピューター）の石。盤の端っこにある。
        /// </summary>
        _6_MyPieceOnEdge = 6

    }
}
