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

namespace Grayscale.GPL.P___190_Board______.L125_WorkingBoard
{
    public interface MarkingBoard
    {
        /// <summary>
        /// 全てのマークを false で初期化します。
        /// 
        /// Gnugo1.2 では initmark.c ファイルの initmark 関数です。配列の全要素を 0 で初期化していました。
        /// 
        /// findopen, findnextmove の使用前に使用。
        /// </summary>
        void Initmark(int boardSize);

        /// <summary>
        /// 指定した位置を使用したならチェックします。
        /// 
        /// Gnugo1.2 では 1 でマークしていました。
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        void Done_Current(GobanPoint location);

        /// <summary>
        /// 北側をチェック可能なら真。
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        bool CanDo_North(GobanPoint location);

        /// <summary>
        /// 東側をチェック可能なら真。
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        bool CanDo_East(GobanPoint location);

        /// <summary>
        /// 南側をチェック可能なら真。
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        bool CanDo_South(GobanPoint location);

        /// <summary>
        /// 西側をチェック可能なら真。
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        bool CanDo_West(GobanPoint location);
    }
}
