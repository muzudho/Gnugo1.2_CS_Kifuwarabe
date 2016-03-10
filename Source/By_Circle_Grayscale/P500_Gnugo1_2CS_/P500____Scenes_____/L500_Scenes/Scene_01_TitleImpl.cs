/**
 * C# Arrange 2.0 of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-29
 *  
 * showinst.c -> Scene_01_TitleImpl.cs
 *               Scene_02_InstructionImpl.cs
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
using System;

namespace Grayscale.GPL.P500____Scenes_____.L500_Scenes
{
    /// <summary>
    /// ゲーム開始の説明を表示します。
    /// </summary>
    public class Scene_01_TitleImpl
    {
        /// <summary>
        /// プログラムのタイトル表示
        /// 
        /// Gnugo1.2 では、showinst関数。
        /// </summary>
        public void DoScene()
        {
            Console.WriteLine("XOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOX");
            Console.WriteLine("O                                                                             O");
            Console.WriteLine("X                           GNUGO (Previously Hugo)                           X");
            Console.WriteLine("O                           the game of Go (Wei-Chi)                          O");
            Console.WriteLine("X                                                                             X");
            Console.WriteLine("O                            version 1.2   10-31-95                           O");
            Console.WriteLine("X           Copyright (C) 1989, 1995 Free Software Foundation, Inc.           X");
            Console.WriteLine("O                              Author: Man L. Li                              O");
            Console.WriteLine("X           GNUGO comes with ABSOLUTELY NO WARRANTY; see COPYING for          X");
            Console.WriteLine("O           detail.   This is free software, and you are welcome to           O");
            Console.WriteLine("X           redistribute it; see COPYING for copying conditions.              X");
            Console.WriteLine("O                                                                             O");
            Console.WriteLine("X              Please report all bugs, modifications, suggestions             X");
            Console.WriteLine("O                             to manli@cs.uh.edu                              O");
            Console.WriteLine("X                                                                             X");
            Console.WriteLine("OXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXO");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("    C# Arrange");
            Console.WriteLine("     version 2.0   2015-11-30");
            Console.WriteLine("    (^o^) modified by Muzudho");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.Write("Press return to continue");
            Console.ReadLine();
        }
    }
}
