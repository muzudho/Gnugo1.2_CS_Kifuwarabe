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
        /// <param name="futureLiberty">Gnugo1.2 では newlib 引数。ツケた後の呼吸点の数。</param>
        /// <param name="currentLiberty">Gnugo1.2 では minlib 引数。ツケる前の呼吸点の数。</param>
        /// <returns></returns>
        private static int Evaluate_LibertyWeak
        (
            int futureLiberty,
            int currentLiberty
        )
        {
            int result_score; // Gnugo1.2 では val 変数。評価値

            if (futureLiberty <= currentLiberty)
            {
                // ツケて　呼吸点が減っているようでは話しになりません。
                result_score = -1;
            }
            else
            {
                // ツケて　呼吸点が増えているので、どれだけ増えたかを数えます。
                int upLiberty = futureLiberty - currentLiberty;

                result_score = 40  // 40を基本に。
                        +
                        (upLiberty - 1) * 50    // 呼吸点が２以上増えるなら、呼吸点が１増えるたびに 50 点のボーナス。
                        /
                        (currentLiberty * currentLiberty * currentLiberty)  // ツケる前の呼吸点の数が大きいほど、スコアが減る（緊急の関心を薄れさせる）仕掛け。
                                                                // 1 : 1 点
                                                                // 2 : 8 点
                                                                // 3 : 27 点
                                                                // 4 : 64 点
                        ;
            }

            return result_score;
        }

        /// <summary>
        /// 弱っている石を助ける手を探します。
        /// 助けるというのは、（襲われそうな）自分の石の　横にツケることです。
        /// 
        /// 位置m,nを含むグループから、
        /// 上下左右に隣接する石（または連）の呼吸点を調べ、
        /// 最もスコアの高い石（または連）の場所 i,j と、評価値を探します。（連の場合、どこか１つの石の場所）
        ///
        /// Gnugo1.2 では findnextmove 関数。
        /// </summary>
        /// <param name="out_bestLocation">次の動きの   行、列番号</param>
        /// <param name="out_bestScore">Gnugo1.2では val という名前のポインター変数。次の指し手の評価値</param>
        /// <param name="myStone_location">（脅かされているかもしれない）コンピューターの石の 行番号 m、列番号 n</param>
        /// <param name="currentLiberty">現在のリバティーの数。1～3を、脅かされていると考えます。Gnugo1.2 では minlib 変数。</param>
        /// <param name="taikyoku"></param>
        /// <returns></returns>
        public static bool FindStone_LibertyWeak
        (
            out GobanPoint  out_bestLocation,
            out int         out_bestScore,
            GobanPoint      myStone_location,
            int             currentLiberty,
            Taikyoku        taikyoku
        )
        {

            out_bestLocation = new GobanPointImpl(-1,-1);
            out_bestScore = -1;

            // カレント位置をマークします。
            taikyoku.MarkingBoard.Done_Current(myStone_location);

            //
            // 東西南北のどこかに　空きスペース　（がないと助けられません）があるはずです。
            //

            //--------------------------------------------------------------------------
            // 北隣を調べます。
            {
                GobanPoint tryLocation = new GobanPointImpl(0, 0); // Gnugo1.2 では ti,tj という変数名。// 初期値は 2015-11-26 追加
                int tryScore = 0;    // Gnugo1.2 では tval 変数。 隣位置の評価値。    // 2015-11-26 追加 0 で初期化。
                bool found = false;

                if (!myStone_location.IsNorthEnd()) // 北端でない石のみ。
                {
                    if (taikyoku.Goban.NorthOf(myStone_location) == StoneColor.Empty)
                    {
                        // わたしの石の北隣にある空きスペースの位置。
                        tryLocation.SetLocation(myStone_location.ToNorth());

                        // 空きスペースに石を置いたと考えて、石を置いた局面のその自分の石（または連）の呼吸点を数えます。
                        int futureLiberty; // Gnugo1.2 では、グローバル変数 lib = 0 でした。
                        Util_CountLiberty.Count(out futureLiberty, tryLocation, taikyoku.MyColor, taikyoku);

                        // 評価値計算
                        tryScore = Util_SasiteNext.Evaluate_LibertyWeak(futureLiberty, currentLiberty);

                        found = true;
                    }
                    else if
                    (
                        taikyoku.Goban.NorthOf(myStone_location) == taikyoku.MyColor // 北隣がコンピューターの石で、
                        &&
                        taikyoku.MarkingBoard.CanDo_North(myStone_location) // 北隣のマーキングが 0 なら
                    )
                    {
                        if (Util_SasiteNext.FindStone_LibertyWeak(out tryLocation, out tryScore, myStone_location.ToNorth(), currentLiberty, taikyoku))    // 再帰的に検索
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
            }

            //--------------------------------------------------------------------------
            // 南ネイバーを調べます。
            {
                GobanPoint tryLocation = new GobanPointImpl(0, 0); // Gnugo1.2 では ti,tj という変数名。// 初期値は 2015-11-26 追加
                int tryScore = 0;    // Gnugo1.2 では tval 変数。 隣位置の評価値。    // 2015-11-26 追加 0 で初期化。
                bool found = false;

                if (!myStone_location.IsSouthEnd(taikyoku.GobanBounds))    // 南端でなければ。
                {
                    if (taikyoku.Goban.SouthOf(myStone_location) == StoneColor.Empty)
                    {
                        // 南隣の石（または連）の呼吸点の数を調べ、
                        // 期待する呼吸点の数より　大きいほど高い評価値を付けます。
                        tryLocation.SetLocation(myStone_location.ToSouth());

                        int futureLiberty; // Gnugo1.2 では、グローバル変数 lib = 0 でした。
                        Util_CountLiberty.Count(out futureLiberty, tryLocation, taikyoku.MyColor, taikyoku);
                        tryScore = Util_SasiteNext.Evaluate_LibertyWeak(futureLiberty, currentLiberty);
                        found = true;
                    }
                    else if
                    (
                        taikyoku.Goban.SouthOf(myStone_location) == taikyoku.MyColor
                        &&
                        taikyoku.MarkingBoard.CanDo_South(myStone_location) // 南側
                    )
                    {
                        if (Util_SasiteNext.FindStone_LibertyWeak(out tryLocation, out tryScore, myStone_location.ToSouth(), currentLiberty, taikyoku))
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
            }

            //--------------------------------------------------------------------------
            // 西ネイバーを調べます。
            {
                GobanPoint tryLocation = new GobanPointImpl(0, 0); // Gnugo1.2 では ti,tj という変数名。// 初期値は 2015-11-26 追加
                int tryScore = 0;    // Gnugo1.2 では tval 変数。 隣位置の評価値。    // 2015-11-26 追加 0 で初期化。
                bool found = false;

                if (!myStone_location.IsWestEnd()) // 西端でなければ。
                {
                    if (taikyoku.Goban.WestOf(myStone_location) == StoneColor.Empty)
                    {
                        tryLocation.SetLocation(myStone_location.ToWest());

                        int futureLiberty; // Gnugo1.2 では、グローバル変数 lib = 0 でした。
                        Util_CountLiberty.Count(out futureLiberty, tryLocation, taikyoku.MyColor, taikyoku);
                        tryScore = Util_SasiteNext.Evaluate_LibertyWeak(futureLiberty, currentLiberty);
                        found = true;
                    }
                    else
                    {
                        if (
                            taikyoku.Goban.WestOf(myStone_location) == taikyoku.MyColor
                            &&
                            taikyoku.MarkingBoard.CanDo_West(myStone_location) // 西側
                            )
                        {
                            if (Util_SasiteNext.FindStone_LibertyWeak(out tryLocation, out tryScore, myStone_location.ToWest(), currentLiberty, taikyoku))
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
            }

            //--------------------------------------------------------------------------
            // 東ネイバーを調べます。
            {
                GobanPoint tryLocation = new GobanPointImpl(0, 0); // Gnugo1.2 では ti,tj という変数名。// 初期値は 2015-11-26 追加
                int tryScore = 0;    // Gnugo1.2 では tval 変数。 隣位置の評価値。    // 2015-11-26 追加 0 で初期化。
                bool found = false;

                if (!myStone_location.IsEastEnd(taikyoku.GobanBounds))    // 東端でなければ。
                {
                    if (taikyoku.Goban.EastOf(myStone_location) == StoneColor.Empty)
                    {
                        tryLocation.SetLocation(myStone_location.ToEast());

                        int futureLiberty; // Gnugo1.2 では、グローバル変数 lib = 0 でした。
                        Util_CountLiberty.Count(out futureLiberty, tryLocation, taikyoku.MyColor, taikyoku);
                        tryScore = Util_SasiteNext.Evaluate_LibertyWeak(futureLiberty, currentLiberty);
                        found = true;
                    }
                    else
                    {
                        if
                        (
                            taikyoku.Goban.EastOf(myStone_location) == taikyoku.MyColor
                            &&
                            taikyoku.MarkingBoard.CanDo_East(myStone_location) // 東側
                        )
                        {
                            if (Util_SasiteNext.FindStone_LibertyWeak(out tryLocation, out tryScore, myStone_location.ToEast(), currentLiberty, taikyoku))
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