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
using Grayscale.GPL.P___190_Board______.L125_WorkingBoard;

namespace Grayscale.GPL.P190____Board______.L125_WorkingBoard
{
    /// <summary>
    /// マーキング用の作業用碁盤。
    /// 
    /// Gnugo1.2 では、ma という静的グローバル配列。
    /// </summary>
    public class MarkingBoardImpl : MarkingBoard
    {
        /// <summary>
        /// マーキングのための作業用行列
        /// 
        /// findopen で使用。
        /// findnextmove で使用。
        /// 
        /// Gnugo1.2 では、マークをしていないとき false、マークをしたとき true でしたが、
        /// C#版に改造するにあたり真偽値を逆にし、マークをしていないとき true、マークをしたとき false に変更しました。
        /// </summary>
        private bool[,] ma;

        public MarkingBoardImpl(int boardSize)
        {
            this.ma = new bool[boardSize, boardSize];
        }

        /// <summary>
        /// 全てのマークを false で初期化します。
        /// 
        /// Gnugo1.2 では initmark.c ファイルの initmark 関数です。配列の全要素を 0 （if文で偽になる）で初期化していました。
        /// C#版に改造するにあたり、真偽値を逆にしました。
        /// 
        /// findopen, findnextmove の使用前に使用。
        /// </summary>
        public void Initmark(int boardSize)
        {
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    this.ma[i, j] = true;// false;
                }
            }
        }

        /// <summary>
        /// 指定した位置を使用したならチェックします。
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public void Done_Current(GobanPoint location)
        {
            // Gnugo1.2 では 1 （if文で真になる）でマークしていました。
            /// C#版に改造するにあたり、真偽値を逆にしました。
            this.ma[location.I, location.J] = false;// true;
        }

        /// <summary>
        /// 北側をチェック可能なら真。
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public bool CanDo_North(GobanPoint location)
        {
            /// Gnugo1.2 では if文の中で !ma[m-1][n] 判定していました。
            /// C#版に改造するにあたり、真偽値を逆にしました。
            return this.ma[location.I - 1, location.J];//!
        }

        /// <summary>
        /// 東側をチェック可能なら真。
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public bool CanDo_East(GobanPoint location)
        {
            /// Gnugo1.2 では if文の中で !ma[m][n+1] 判定していました。
            /// C#版に改造するにあたり、真偽値を逆にしました。
            return this.ma[location.I, location.J + 1];//!
        }

        /// <summary>
        /// 南側をチェック可能なら真。
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public bool CanDo_South(GobanPoint location)
        {
            /// Gnugo1.2 では if文の中で !ma[m+1][n] 判定していました。
            /// C#版に改造するにあたり、真偽値を逆にしました。
            return this.ma[location.I + 1, location.J];//!
        }

        /// <summary>
        /// 西側をチェック可能なら真。
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public bool CanDo_West(GobanPoint location)
        {
            /// Gnugo1.2 では if文の中で !ma[m][n-1] 判定していました。
            /// C#版に改造するにあたり、真偽値を逆にしました。
            return this.ma[location.I, location.J - 1];//!
        }

    }
}
