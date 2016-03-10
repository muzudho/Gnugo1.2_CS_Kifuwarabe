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
    /// <summary>
    /// マークのための作業用行列。
    /// 
    /// Gnugo1.2 では、C言語のブーリアン型の書き方で、
    /// 1はマークされていない（真）、
    /// 0はマークされている（偽）、としていました。
    /// countlib.c で 全要素を1で埋めていました。
    /// count で要素を false に変更されていきました。
    ///
    /// マークしたということは、
    /// もう数えたので次は加算しないということです。
    /// </summary>
    public interface CountedBoard
    {
        /// <summary>
        /// 碁番の全域を、調査可能としてフラグを設定します。
        /// Gnugo1.2 の countlib で使われていたコードです。配列の内容を 1 （if文で真の扱い）で埋めていました。
        /// </summary>
        void FillAll_WeCan(int boardSize);

        /// <summary>
        /// 指定した位置を使用したなら偽にします。
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
        /// 北側を使用したなら真。
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        void Done_North(GobanPoint location);

        /// <summary>
        /// 東側をチェック可能なら真。
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        bool CanDo_East(GobanPoint location);

        /// <summary>
        /// 東側を使用したなら真。
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        void Done_East(GobanPoint location);

        /// <summary>
        /// 南側をチェック可能なら真。
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        bool CanDo_South(GobanPoint location);

        /// <summary>
        /// 南側を使用したなら真。
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        void Done_South(GobanPoint location);

        /// <summary>
        /// 西側をチェック可能なら真。
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        bool CanDo_West(GobanPoint location);

        /// <summary>
        /// 西側を使用したなら真。
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        void Done_West(GobanPoint location);

    }
}
