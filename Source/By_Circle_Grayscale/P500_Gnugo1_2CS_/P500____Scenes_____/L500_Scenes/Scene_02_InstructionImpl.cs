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
    public class Scene_02_InstructionImpl
    {
        /// <summary>
        /// プログラムの操作説明の表示
        /// 
        /// Gnugo1.2 では、showinst関数。
        /// </summary>
        public void DoScene()
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("To play this game first select number of handicap pieces (0 to 17) for the");
            Console.WriteLine("black side.  Next choose your color (black or white).  To place your piece,");
            Console.WriteLine("enter your move as coordinate on the board in column and row.  The column");
            Console.WriteLine("is from 'A' to 'T'(excluding 'I').  The row is from 1 to 19.");
            Console.WriteLine();
            Console.WriteLine("To pass your move enter 'pass' for your turn.  After both you and the computer");
            Console.WriteLine("passed the game will end.  To save the board and exit enter 'save'.  The game");
            Console.WriteLine("will continue the next time you start the program.  To stop the game in the");
            Console.WriteLine("middle of play enter 'stop' for your move.  You will be asked whether you want");
            Console.WriteLine("to count the result of the game.  If you answer 'y' then you need to remove the");
            Console.WriteLine("remaining dead pieces and fill up neutral turf on the board as instructed.");
            Console.WriteLine("Finally, the computer will count all pieces for both side and show the result.");
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
