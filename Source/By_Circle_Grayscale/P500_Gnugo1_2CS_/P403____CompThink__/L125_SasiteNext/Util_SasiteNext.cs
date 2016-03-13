/**
 * C# Arrange of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-28
 *  
 * findnext.c -> Util_SasiteNext.cs
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
using Grayscale.GPL.P___190_Board______.L063_Word;
using Grayscale.GPL.P___300_Taikyoku___.L500_Taikyoku;
using Grayscale.GPL.P160____Collection_.L500_Collection;
using Grayscale.GPL.P340____Liberty____.L250_Liberty;
using Grayscale.GPL.P403____CompThink__.L075_OwnEye;

namespace Grayscale.GPL.P403____CompThink__.L125_SasiteNext
{
    /// <summary>
    /// コンピューターの指し手を探します。
    /// 
    /// 再帰します。隣、隣と見て行きます。
    /// 
    /// findsavr で使われます。
    /// </summary>
    public abstract class Util_SasiteNext
    {
        /// <summary>
        /// Libertyが少ない石（または連）、つまり弱い石（または連）ほど高く評価する関数です。
        /// 
        /// Gnugo1.2 では fval 関数。
        /// </summary>
        /// <param name="newLiberty"></param>
        /// <param name="expectedLiberty"></param>
        /// <returns></returns>
        private static int Evaluate_LibertyWeak
        (
            int newLiberty, // Gnugo1.2 では newlib 引数。新しいリバティー
            int expectedLiberty  // Gnugo1.2 では minlib 引数。リバティーの少ない方から回しているループカウンター。
        )
        {
            int result_score; // Gnugo1.2 では val 変数。評価値

            if (newLiberty <= expectedLiberty)
            {
                result_score = -1;   // 評価値は -1
            }
            else
            {
                int upLiberty = newLiberty - expectedLiberty;    // 増えたリバティー

                result_score = 40  // 40を基本に。
                        +
                        (upLiberty - 1) * 50    // リバティーが 2 以上増えるなら、1 増えるたリバティーに 50 点のボーナス。
                        /
                        (expectedLiberty * expectedLiberty * expectedLiberty)  // 指定したリバティーが大きいほどスコアが減る仕掛け。
                                                                // 1 : 1 点
                                                                // 2 : 8 点
                                                                // 3 : 27 点
                                                                // 4 : 64 点
                        ;
            }

            return result_score;
        }

        /// <summary>
        /// 位置m,nを含むグループから、
        /// 上下左右に隣接する石（または連）の呼吸点を調べ、
        /// 最もスコアの高い石（または連）の場所 i,j と、評価値を探します。（連の場合、どこか１つの石の場所）
        ///
        /// Gnugo1.2 では findnextmove 関数。
        /// </summary>
        /// <param name="out_bestLocation">次の動きの   行、列番号</param>
        /// <param name="out_bestScore">Gnugo1.2では val という名前のポインター変数。次の指し手の評価値</param>
        /// <param name="curStone_location">カレント・ストーンの 行番号 m、列番号 n</param>
        /// <param name="expectedLiberty">カレント・ストーンのリバティー。Gnugo1.2 では minlib 変数。</param>
        /// <param name="taikyoku"></param>
        /// <returns></returns>
        public static bool FindStone_LibertyWeak
        (
            out GobanPoint out_bestLocation,
            out int out_bestScore,
            GobanPoint curStone_location,
            int expectedLiberty,
            Taikyoku taikyoku
        )
        {
            GobanPoint tryLocation; // Gnugo1.2 では ti,tj という変数名。
            tryLocation = new GobanPointImpl(0, 0);// 2015-11-26 追加
            int tryScore = 0;    // Gnugo1.2 では tval 変数。 隣位置の評価値。    // 2015-11-26 追加 0 で初期化。
            bool found = false;

            out_bestLocation = new GobanPointImpl(-1,-1);
            out_bestScore = -1;

            // カレント位置をマークします。
            taikyoku.MarkingBoard.Done_Current(curStone_location);

            //--------------------------------------------------------------------------
            // 北ネイバーを調べます。
            if (!curStone_location.IsNorthEnd()) // 北端でなければ。
            {
                if (taikyoku.Goban.NorthOf(curStone_location) == StoneColor.Empty)
                {
                    // 北隣の位置i,jをセット
                    tryLocation.SetLocation(curStone_location.ToNorth());
            
                    // 隣の石（または連）のリバティーを数えます。
                    int newLiberty; // Gnugo1.2 では、グローバル変数 lib = 0 でした。
                    Util_CountLiberty.Count(out newLiberty, tryLocation, taikyoku.MyColor, taikyoku);
                    tryScore = Util_SasiteNext.Evaluate_LibertyWeak(newLiberty, expectedLiberty);   // 評価値計算
            
                    found = true;
                }
                else if
                (
                    taikyoku.Goban.NorthOf(curStone_location) == taikyoku.MyColor // 北隣がコンピューターの石で、
                    &&
                    taikyoku.MarkingBoard.CanDo_North(curStone_location) // 北隣のマーキングが 0 なら
                )
                {
                    if (Util_SasiteNext.FindStone_LibertyWeak(out tryLocation, out tryScore, curStone_location.ToNorth(), expectedLiberty, taikyoku))    // 再帰的に検索
                    {
                        found = true;
                    }
                }
            }

            if (found)  // 見つかったら1
            {
                found = false;
                if (out_bestScore < tryScore && !Util_OwnEye.IsThis(tryLocation, taikyoku))
                {
                    out_bestScore = tryScore;    // 高い方の評価値を残している？
                    out_bestLocation.SetLocation(tryLocation);// 高い方の交点i,jを残している？
                }
            }

            //--------------------------------------------------------------------------
            // 南ネイバーを調べます。
            if (!curStone_location.IsSouthEnd(taikyoku.GobanBounds))    // 南端でなければ。
            {
                if (taikyoku.Goban.SouthOf(curStone_location) == StoneColor.Empty)
                {
                    // 南隣の石（または連）の呼吸点の数を調べ、
                    // 期待する呼吸点の数より　大きいほど高い評価値を付けます。
                    tryLocation.SetLocation(curStone_location.ToSouth());

                    int adjLiberty; // Gnugo1.2 では、グローバル変数 lib = 0 でした。
                    Util_CountLiberty.Count(out adjLiberty, tryLocation, taikyoku.MyColor, taikyoku);
                    tryScore = Util_SasiteNext.Evaluate_LibertyWeak(adjLiberty, expectedLiberty);
                    found = true;
                }
                else if
                (
                    taikyoku.Goban.SouthOf(curStone_location) == taikyoku.MyColor
                    &&
                    taikyoku.MarkingBoard.CanDo_South(curStone_location) // 南側
                )
                {
                    if (Util_SasiteNext.FindStone_LibertyWeak(out tryLocation, out tryScore, curStone_location.ToSouth(), expectedLiberty, taikyoku))
                    {
                        found = true;
                    }
                }
            }

            if (found)  // 見つかったら1
            {
                found = false;
                if (out_bestScore < tryScore && !Util_OwnEye.IsThis(tryLocation, taikyoku))
                {
                    out_bestScore = tryScore;
                    out_bestLocation.SetLocation(tryLocation);
                }
            }

            //--------------------------------------------------------------------------
            // 西ネイバーを調べます。
            if (!curStone_location.IsWestEnd()) // 西端でなければ。
            {
                if (taikyoku.Goban.WestOf(curStone_location) == StoneColor.Empty)
                {
                    tryLocation.SetLocation(curStone_location.ToWest());

                    int adjLiberty; // Gnugo1.2 では、グローバル変数 lib = 0 でした。
                    Util_CountLiberty.Count(out adjLiberty, tryLocation, taikyoku.MyColor, taikyoku);
                    tryScore = Util_SasiteNext.Evaluate_LibertyWeak(adjLiberty, expectedLiberty);
                    found = true;
                }
                else
                {
                    if (
                        taikyoku.Goban.WestOf(curStone_location) == taikyoku.MyColor
                        &&
                        taikyoku.MarkingBoard.CanDo_West(curStone_location) // 西側
                        )
                    {
                        if (Util_SasiteNext.FindStone_LibertyWeak(out tryLocation, out tryScore, curStone_location.ToWest(), expectedLiberty, taikyoku))
                        {
                            found = true;
                        }
                    }
                }
            }

            if (found)  // 見つかっていれば 1
            {
                found = false;
                if (tryScore > out_bestScore && !Util_OwnEye.IsThis(tryLocation, taikyoku))
                {
                    out_bestScore = tryScore;
                    out_bestLocation.SetLocation(tryLocation);
                }
            }

            //--------------------------------------------------------------------------
            // 東ネイバーを調べます。
            if (!curStone_location.IsEastEnd(taikyoku.GobanBounds))    // 東端でなければ。
            {
                if (taikyoku.Goban.EastOf(curStone_location) == StoneColor.Empty)
                {
                    tryLocation.SetLocation(curStone_location.ToEast());

                    int adjLiberty; // Gnugo1.2 では、グローバル変数 lib = 0 でした。
                    Util_CountLiberty.Count(out adjLiberty, tryLocation, taikyoku.MyColor, taikyoku);
                    tryScore = Util_SasiteNext.Evaluate_LibertyWeak(adjLiberty, expectedLiberty);
                    found = true;
                }
                else
                {
                    if
                    (
                        taikyoku.Goban.EastOf(curStone_location) == taikyoku.MyColor
                        &&
                        taikyoku.MarkingBoard.CanDo_East(curStone_location) // 東側
                    )
                    {
                        if (Util_SasiteNext.FindStone_LibertyWeak(out tryLocation, out tryScore, curStone_location.ToEast(), expectedLiberty, taikyoku))
                        {
                            found = true;
                        }
                    }
                }
            }

            if (found)  // Gnugo1.2では、見つかっていれば 1 でした。
            {
                found = false;
                if (out_bestScore < tryScore && !Util_OwnEye.IsThis(tryLocation, taikyoku))
                {
                    out_bestScore = tryScore;
                    out_bestLocation.SetLocation(tryLocation);
                }
            }

            //--------------------------------------------------------------------------
            if (0 < out_bestScore)   // 次の動きを見つけた。
            {
                return true;
            }
            else    // 次の動きは失敗。
            {
                return false;
            }
        }
    }
}