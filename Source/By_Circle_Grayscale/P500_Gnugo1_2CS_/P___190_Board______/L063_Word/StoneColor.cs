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
namespace Grayscale.GPL.P___190_Board______.L063_Word
{
    /// <summary>
    /// 石の色。
    /// </summary>
    public enum StoneColor
    {
        /// <summary>
        /// 空っぽの交点。
        /// 
        /// Gnugo1.2 では、#define を使って EMPTY定数 0 と定義されている。
        /// 盤上の空き場所でない場面でも 0 の代わりによくこの定数が使われていて 紛らわしかった。
        /// </summary>
        Empty = 0,

        /// <summary>
        /// 白石が置いてある。
        /// 
        /// Gnugo1.2 では、#define を使って WHITE定数 1 と定義されている。
        /// </summary>
        White = 1,

        /// <summary>
        /// 黒石が置いてある。
        /// 
        /// Gnugo1.2 では、#define を使って BLACK定数 2 と定義されている。
        /// </summary>
        Black = 2
    }
}
