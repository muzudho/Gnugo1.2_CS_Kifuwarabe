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
using Grayscale.GPL.P___160_Collection_.L250_Rectangle;

namespace Grayscale.GPL.P___160_Collection_.L500_Collection
{
    /// <summary>
    /// 碁盤上の位置i,j。
    /// </summary>
    public interface GobanPoint
    {
        /// <summary>
        /// i は縦。一番上を0、一番下を18。
        /// </summary>
        int I { get; set; }

        /// <summary>
        /// j は横。一番左を0、一番右を18。
        /// </summary>
        int J { get; set; }

        void SetLocation(int i, int j);
        void SetLocation(GobanPoint p);

        /// <summary>
        /// パスをします。
        /// 
        /// Gnugo1.2 では、i が -1 のときは パスのシグナルとします。
        /// </summary>
        /// <returns></returns>
        void SetPass();

        /// <summary>
        /// パスをするなら真。
        /// 
        /// Gnugo1.2 では、i が -1 のときは パスのシグナルとします。
        /// </summary>
        /// <returns></returns>
        bool IsPass();

        /// <summary>
        /// I が範囲外なら真。（境界線上を含まない）
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        bool OutOfI(int min, int max);

        /// <summary>
        /// J が範囲外なら真。（境界線上を含まない）
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        bool OutOfJ(int min, int max);

        /// <summary>
        /// I が範囲内なら真。（境界線上を含む）
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        bool ContainsI(int min, int max);

        /// <summary>
        /// J が範囲内なら真。（境界線上を含む）
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        bool ContainsJ(int min, int max);

        /// <summary>
        /// 北側の位置を示すオブジェクトを作成します。
        /// </summary>
        /// <returns></returns>
        GobanPoint ToNorth();

        /// <summary>
        /// 東側の位置を示すオブジェクトを作成します。
        /// </summary>
        /// <returns></returns>
        GobanPoint ToEast();

        /// <summary>
        /// 南側の位置を示すオブジェクトを作成します。
        /// </summary>
        /// <returns></returns>
        GobanPoint ToSouth();

        /// <summary>
        /// 西側の位置を示すオブジェクトを作成します。
        /// </summary>
        /// <returns></returns>
        GobanPoint ToWest();

        /// <summary>
        /// 碁盤上にない場所に設定にします。
        /// </summary>
        void MoveToVanish();

        /// <summary>
        /// 北端なら真。
        /// </summary>
        /// <returns></returns>
        bool IsNorthEnd();

        /// <summary>
        /// 東端なら真。
        /// </summary>
        /// <returns></returns>
        bool IsEastEnd(GobanRectangleA gobanBound);

        /// <summary>
        /// 南端なら真。
        /// </summary>
        /// <returns></returns>
        bool IsSouthEnd(GobanRectangleA gobanBound);

        /// <summary>
        /// 西端なら真。
        /// </summary>
        /// <returns></returns>
        bool IsWestEnd();

        /// <summary>
        /// 引数で指定したポイントの北隣が、このポイントなら真。
        /// </summary>
        /// <returns></returns>
        bool Is_NorthOf(GobanPoint location);

        /// <summary>
        /// 引数で指定したポイントの東隣が、このポイントなら真。
        /// </summary>
        /// <returns></returns>
        bool Is_EastOf(GobanPoint location);

        /// <summary>
        /// 引数で指定したポイントの南隣が、このポイントなら真。
        /// </summary>
        /// <returns></returns>
        bool Is_SouthOf(GobanPoint location);

        /// <summary>
        /// 引数で指定したポイントの西隣が、このポイントなら真。
        /// </summary>
        /// <returns></returns>
        bool Is_WestOf(GobanPoint location);

    }
}
