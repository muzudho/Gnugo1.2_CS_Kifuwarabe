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
using Grayscale.GPL.P___190_Board______.L063_Word;
using System;

namespace Grayscale.GPL.P310____ConvStone__.L500_Conv
{
    public abstract class Conv_StoneColor
    {

        /// <summary>
        /// 数字を、列挙型に変換します。
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static StoneColor FromNumber(int color)
        {
            StoneColor result;

            switch (color)
            {
                case 0: result = StoneColor.Empty; break;
                case 1: result = StoneColor.White; break;
                case 2: result = StoneColor.Black; break;
                default: throw new Exception( "解析できない石の色番号=["+color+"]" );
            }

            return result;
        }

        /// <summary>
        /// 列挙型を、数字に変換します。
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static int ToNumber(StoneColor color)
        {
            int result;

            switch (color)
            {
                case StoneColor.Empty: result = 0; break;
                case StoneColor.White: result = 1; break;
                case StoneColor.Black: result = 2; break;
                default: throw new Exception("解析できない石の列挙型=[" + color + "]");
            }

            return result;
        }

    }
}
