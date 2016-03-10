/**
 * C# Arrange of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-29
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
    /// ゲームの終了時に使用するフラグ。
    /// 
    /// Gnugo1.2 の endgame.c ファイルでは、石の色をフラグ代わりに使っていて紛らわしかったので、
    /// C#化にあたって、列挙型を 別途用意することにした。
    /// </summary>
    public enum CheckFlag_Ending
    {
        /// <summary>
        /// 処理を行うのが可能な状態。
        /// </summary>
        Yet = 1,

        /// <summary>
        /// 処理は済んだ状態。
        /// </summary>
        Finished = 2,

        /// <summary>
        /// これから処理を行う状態。
        /// 
        /// Gnugo1.2 では、endgame.c ファイルで #define を使って GREY定数 3 と定義されている。
        /// </summary>
        Working = 3
    }
}
