/**
 * C# Arrange 2.0 of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-30
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
namespace Grayscale.GPL.P___160_Collection_.L250_Rectangle
{
    /// <summary>
    /// 碁盤の矩形。
    /// 
    /// 使い方としては、このインターフェースは、(GobanRectangleB)に型キャストして使ってください。
    /// </summary>
    public interface GobanRectangleA
    {
        /// <summary>
        /// 9 or 19。9路盤か、19路盤か。デフォルトでは 19路盤。
        /// </summary>
        int BoardSize { get; set; }

        /// <summary>
        /// 盤の、数字の大きい方の端っこ。
        /// </summary>
        int BoardEnd { get; }

        /// <summary>
        /// 盤の真ん中。
        /// 
        /// 0から始まる数字として、
        /// 19路盤なら 9。（0～8、9、10～18）
        /// 9路盤なら 4。
        /// </summary>
        int BoardCenter { get; }

        /// <summary>
        /// 石を置ける場所の数。
        /// 
        /// 将棋と違って、囲碁では 升ではなく交点なので NODE なのだろうか。
        /// </summary>
        int Nodes { get; }

    }
}
