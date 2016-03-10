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
using Grayscale.GPL.P___160_Collection_.L500_Collection;

namespace Grayscale.GPL.P160____Collection_.L500_Collection
{
    /// <summary>
    /// 碁盤上の位置i,j。
    /// </summary>
    public class GobanPointImpl : GobanPoint
    {
        /// <summary>
        /// i は縦。一番上を0、一番下を18。
        /// </summary>
        public int I { get; set; }

        /// <summary>
        /// j は横。一番左を0、一番右を18。
        /// </summary>
        public int J { get; set; }
        

        public GobanPointImpl()
        {
            this.I = 0;
            this.J = 0;
        }

        public GobanPointImpl(int i, int j)
        {
            this.I = i;
            this.J = j;
        }

        public void SetLocation(int i, int j)
        {
            this.I = i;
            this.J = j;
        }

        public void SetLocation(GobanPoint p)
        {
            this.I = p.I;
            this.J = p.J;
        }

        /// <summary>
        /// パスをします。
        /// 
        /// Gnugo1.2 では、i が -1 のときは パスのシグナルとします。
        /// </summary>
        /// <returns></returns>
        public void SetPass()
        {
            this.I = -1;
        }

        /// <summary>
        /// パスをするなら真。
        /// 
        /// Gnugo1.2 では、i が -1 のときは パスのシグナルとします。
        /// </summary>
        /// <returns></returns>
        public bool IsPass()
        {
            return this.I == -1;
        }

        /// <summary>
        /// I が範囲外なら真。（境界線上を含まない）
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public bool OutOfI(int min, int max)
        {
            return this.I < min || max < this.I;
        }

        /// <summary>
        /// J が範囲外なら真。（境界線上を含まない）
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public bool OutOfJ(int min, int max)
        {
            return this.J < min || max < this.J;
        }

        /// <summary>
        /// I が範囲内なら真。（境界線上を含む）
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public bool ContainsI(int min, int max)
        {
            return min <= this.I && this.I <= max;
        }

        /// <summary>
        /// J が範囲内なら真。（境界線上を含む）
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public bool ContainsJ(int min, int max)
        {
            return min <= this.J && this.J <= max;
        }

        /// <summary>
        /// 北側の位置を示すオブジェクトを作成します。
        /// </summary>
        /// <returns></returns>
        public GobanPoint ToNorth()
        {
            return new GobanPointImpl(this.I - 1, this.J);
        }

        /// <summary>
        /// 東側の位置を示すオブジェクトを作成します。
        /// </summary>
        /// <returns></returns>
        public GobanPoint ToEast()
        {
            return new GobanPointImpl(this.I, this.J+1);
        }

        /// <summary>
        /// 南側の位置を示すオブジェクトを作成します。
        /// </summary>
        /// <returns></returns>
        public GobanPoint ToSouth()
        {
            return new GobanPointImpl(this.I + 1, this.J);
        }

        /// <summary>
        /// 西側の位置を示すオブジェクトを作成します。
        /// </summary>
        /// <returns></returns>
        public GobanPoint ToWest()
        {
            return new GobanPointImpl(this.I, this.J-1);
        }

        /// <summary>
        /// 碁盤上にない場所に設定にします。
        /// </summary>
        public void MoveToVanish()
        {
            this.I = -1;
            this.J = -1;
        }

        /// <summary>
        /// 北端なら真。
        /// </summary>
        /// <returns></returns>
        public bool IsNorthEnd()
        {
            return this.I == 0;
        }

        /// <summary>
        /// 東端なら真。
        /// </summary>
        /// <returns></returns>
        public bool IsEastEnd(GobanRectangleA gobanBound)
        {
            return this.J == gobanBound.BoardEnd;
        }

        /// <summary>
        /// 南端なら真。
        /// </summary>
        /// <returns></returns>
        public bool IsSouthEnd(GobanRectangleA gobanBound)
        {
            return this.I == gobanBound.BoardEnd;
        }

        /// <summary>
        /// 西端なら真。
        /// </summary>
        /// <returns></returns>
        public bool IsWestEnd()
        {
            return this.J == 0;
        }

        /// <summary>
        /// 引数で指定したポイントの北隣が、このポイントなら真。
        /// </summary>
        /// <returns></returns>
        public bool Is_NorthOf(GobanPoint location)
        {
            return this.I == location.I - 1 && this.J == location.J;
        }

        /// <summary>
        /// 引数で指定したポイントの東隣が、このポイントなら真。
        /// </summary>
        /// <returns></returns>
        public bool Is_EastOf(GobanPoint location)
        {
            return this.I == location.I && this.J == location.J + 1;
        }

        /// <summary>
        /// 引数で指定したポイントの南隣が、このポイントなら真。
        /// </summary>
        /// <returns></returns>
        public bool Is_SouthOf(GobanPoint location)
        {
            return this.I == location.I + 1 && this.J == location.J;
        }

        /// <summary>
        /// 引数で指定したポイントの西隣が、このポイントなら真。
        /// </summary>
        /// <returns></returns>
        public bool Is_WestOf(GobanPoint location)
        {
            return this.I == location.I && this.J == location.J - 1;
        }
    }
}
