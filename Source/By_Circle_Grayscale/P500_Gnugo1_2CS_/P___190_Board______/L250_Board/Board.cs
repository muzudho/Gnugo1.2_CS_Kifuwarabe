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
using Grayscale.GPL.P___160_Collection_.L500_Collection;
using Grayscale.GPL.P___190_Board______.L063_Word;

namespace Grayscale.GPL.P___190_Board______.L250_Board
{
    public interface Board
    {

        /// <summary>
        /// 碁盤（１９路盤）
        /// 
        /// Gnugo1.2 では、p という名前の静的グローバル変数。positionという意味だろうか？
        /// </summary>
        StoneColor[,] Position { get; set; }

        /// <summary>
        /// 碁石を交点に置きます。
        /// </summary>
        /// <param name="location"></param>
        /// <param name="color"></param>
        void Put(GobanPoint location, StoneColor color);

        /// <summary>
        /// 指定の交点の石の色を見ます。
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        StoneColor LookColor(GobanPoint location);

        /// <summary>
        /// 指定の交点の北側の石の色を見ます。
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        StoneColor LookColor_NorthOf(GobanPoint location);

        /// <summary>
        /// 指定の交点の東側の石の色を見ます。
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        StoneColor LookColor_EastOf(GobanPoint location);

        /// <summary>
        /// 指定の交点の南側の石の色を見ます。
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        StoneColor LookColor_SouthOf(GobanPoint location);

        /// <summary>
        /// 指定の交点の西側の石の色を見ます。
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        StoneColor LookColor_WestOf(GobanPoint location);
    }
}
