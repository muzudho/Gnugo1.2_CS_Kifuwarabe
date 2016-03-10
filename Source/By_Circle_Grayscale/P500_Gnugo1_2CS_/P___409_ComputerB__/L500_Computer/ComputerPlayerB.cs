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
using Grayscale.GPL.P___180_ComputerA__.L500_Computer;
using Grayscale.GPL.P___405_CompZyoseki.L500_FindPattern;

namespace Grayscale.GPL.P___409_ComputerB__.L500_Computer
{
    public interface ComputerPlayerB : ComputerPlayerA
    {
        /// <summary>
        /// Gnugo1.2 では 関数内での静的cnd変数。  定石グラフ図の、ノード番号
        /// </summary>
        int NodeNo { get; set; }

        /// <summary>
        /// Gnugo1.2 では 関数内での静的mtype変数。　ムーブ・タイプ
        /// </summary>
        MoveType Movetype { get; set; }
    }
}
