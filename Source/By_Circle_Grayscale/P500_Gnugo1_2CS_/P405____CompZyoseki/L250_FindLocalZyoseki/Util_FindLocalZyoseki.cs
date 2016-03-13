/**
 * C# Arrange of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-28
 *  
 * matchpat.c -> Util_FindLocalZyoseki.cs
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
using Grayscale.GPL.P___190_Board______.L063_Word;
using Grayscale.GPL.P___300_Taikyoku___.L500_Taikyoku;
using Grayscale.GPL.P___405_CompZyoseki.L125_LocalZyoseki;
using Grayscale.GPL.P160____Collection_.L500_Collection;
using Grayscale.GPL.P190____Board______.L500_Util;
using Grayscale.GPL.P340____Liberty____.L250_Liberty;
using Grayscale.GPL.P405____CompZyoseki.L125_LocalZyoseki;

namespace Grayscale.GPL.P405____CompZyoseki.L250_FindLocalZyoseki
{
    /// <summary>
    /// マッチパターンの動き。
    /// 
    /// Gnugo1.2 では matchpat関数。
    /// </summary>
    public abstract class Util_FindLocalZyoseki
    {
        /// <summary>
        /// 絶対値。
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int Abs(int x)
        {
            return x < 0 ? -x : x;
        }

        /// <summary>
        /// 中央からどれだけ離れているか。
        /// 
        /// Gnugo1.2 では line 関数。
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int DistanceFromCenter(int x, GobanRectangleA gobanBounds )
        {
            return Util_FindLocalZyoseki.Abs(x - gobanBounds.BoardCenter);
        }


        /// <summary>
        /// 反転や回転など、盤上を見る向きを変える操作のため（transfomation）の行列たち。[8][2][2]
        /// 
        /// 1、-1、0 だけなので等倍。
        /// 
        /// 行列式は
        /// ┌     ┐
        /// │ 0 -1│
        /// │ 1  0│
        /// └     ┘
        /// のような書き方をするが、プログラム上では見にくい。左上、左下、右上、右下の順に並んでいるものとする。
        /// 
        /// </summary>
        public static readonly int[,,] Trf =
        {
            // x         y     入力側
            // x,  y     x   y 定石側
            {{ 1,  0}, { 0,  1}},   // [0]入力された座標、そのまんまです。  （linear transfomation matrix）
            // ┌     ┐
            // │ 1  0│
            // │ 0  1│
            // └     ┘

            {{ 1,  0}, { 0, -1}},   // [1]上下反転                      （invert）
            // ┌     ┐
            // │ 1  0│
            // │ 0 -1│
            // └     ┘

            {{ 0,  1}, {-1,  0}},   // [2]反時計回りに90度回転            （rotate 90）
            // ┌     ┐
            // │ 0 -1│
            // │ 1  0│
            // └     ┘

            {{ 0, -1}, {-1,  0}},   // [3]反時計回りに90度回転して上下反転  （rotate 90 and invert）
            // ┌     ┐
            // │ 0 -1│
            // │-1  0│
            // └     ┘

            {{-1,  0}, { 0,  1}},   // [4]左右反転                     （flip left）
            // ┌     ┐
            // │-1  0│
            // │ 0  1│
            // └     ┘

            {{-1,  0}, { 0, -1}},   // [5]左右反転して上下反転            （flip left and invert）
            // ┌     ┐
            // │-1  0│
            // │ 0 -1│
            // └     ┘

            {{ 0,  1}, { 1,  0}},   // [6]反時計回りに90度回転して左右反転  （rotate 90 and flip left）
            // ┌     ┐
            // │ 0  1│
            // │ 1  0│
            // └     ┘

            {{ 0, -1}, { 1,  0}}    // [7]反時計回りに90度回転して左右反転して上下反転  （rotate 90, flip left and invert）
            //                          （つまり時計回りに90度回転）
            // ┌     ┐
            // │ 0  1│
            // │-1  0│
            // └     ┘
        };

        /// <summary>
        /// 序盤定石以外の、局地的な定石にあてはまるかをしらべ、定石があれば置く場所を取得します。
        /// </summary>
        /// <param name="out_location">次の手の 行、列番号</param>
        /// <param name="out_score">Gnugo1.2 では val 引数。評価値</param>
        /// <param name="origin_location">Gnugo1.2 では、 原点の   行 m、列 n 引数。</param>
        /// <param name="taikyoku"></param>
        /// <returns></returns>
        public static bool FindPattern
        (
            out GobanPoint out_location,
            out int out_score,
            GobanPoint origin_location,
            Taikyoku taikyoku
        )
        {
            GobanPoint tryLocation = new GobanPointImpl(0, 0); // Gnugo1.2 では、ti,tj という変数名。// 2015-11-26 追加 0,0 で初期化。

            out_location = new GobanPointImpl(-1,-1);
            out_score = -1;

            for (int ptnNo = 0; ptnNo < Util_LocalZyoseki.Patterns.Length; ptnNo++) // それぞれのパターンを試します。
            {
                for (int ll = 0; ll < Util_LocalZyoseki.Patterns[ptnNo].LastTrfNo; ll++)/* try each orientation transformation */
                {
                    int k = 0;
                    bool isContinueLoop = true; // Gnugo1.2 では cont変数。continueの略か？

                    //
                    // 条件に一致していないものを、ふるい落としていきます。
                    // 条件に一致しているものは、どんどん次のループに進みます。
                    //
                    while (
                        k != Util_LocalZyoseki.Patterns[ptnNo].Stones.Length
                        &&
                        isContinueLoop
                        )/* いくつかのポイントに一致 */
                    {
                        //
                        // 変形（transform）して、盤上の座標に変換します。
                        //
                        int nx = 
                            origin_location.J
                            +
                            Util_FindLocalZyoseki.Trf[ll, 0, 0] * Util_LocalZyoseki.Patterns[ptnNo].Stones[k].P.X
                            +
                            Util_FindLocalZyoseki.Trf[ll, 0, 1] * Util_LocalZyoseki.Patterns[ptnNo].Stones[k].P.Y
                            ;

                        int my = 
                            origin_location.I
                            +
                            Util_FindLocalZyoseki.Trf[ll, 1, 0] * Util_LocalZyoseki.Patterns[ptnNo].Stones[k].P.X
                            +
                            Util_FindLocalZyoseki.Trf[ll, 1, 1] * Util_LocalZyoseki.Patterns[ptnNo].Stones[k].P.Y
                            ;

                        /* outside the board */
                        if (
                            !Util_AboutBoard.In_Board(my,taikyoku.GobanBounds)
                            ||
                            !Util_AboutBoard.In_Board(nx, taikyoku.GobanBounds)
                        )
                        {
                            isContinueLoop = false;
                            break;
                        }

                        GobanPoint mnLocation = new GobanPointImpl(my, nx);
                        switch (Util_LocalZyoseki.Patterns[ptnNo].Stones[k].Att)
                        {
                            case LocalZyoseki_StoneAttribute._0_Empty :
                                if (taikyoku.Goban.At(mnLocation) == StoneColor.Empty)    /* open */
                                {
                                    break;
                                }
                                else
                                {
                                    isContinueLoop = false;
                                    break;
                                }
                            case LocalZyoseki_StoneAttribute._1_YourPiece:
                                if (taikyoku.Goban.At(mnLocation) == taikyoku.YourColor)  /* your piece */
                                {
                                    break;
                                }
                                else
                                {
                                    isContinueLoop = false;
                                    break;
                                }
                            case LocalZyoseki_StoneAttribute._2_MyPiece:
                                if (taikyoku.Goban.At(mnLocation) == taikyoku.MyColor)  /* my piece */
                                {
                                    break;
                                }
                                else
                                {
                                    isContinueLoop = false;
                                    break;
                                }
                            case LocalZyoseki_StoneAttribute._3_MyNextMove:
                                if (taikyoku.Goban.At(mnLocation) == StoneColor.Empty)    /* open for new move */
                                {
                                    int libertyOfPiece; // Gnugo1.2 では、グローバル変数 lib = 0 でした。
                                    Util_CountLiberty.Count(out libertyOfPiece, mnLocation, taikyoku.MyColor, taikyoku);    /* check liberty */
                                    if (1 < libertyOfPiece)  /* move o.k. */
                                    {
                                        tryLocation.SetLocation(my,nx);
                                        break;
                                    }
                                    else
                                    {
                                        isContinueLoop = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    isContinueLoop = false;
                                    break;
                                }
                            case LocalZyoseki_StoneAttribute._4_EmptyOnEdge:
                                if
                                (
                                    taikyoku.Goban.At(mnLocation) == StoneColor.Empty  /* open on edge */
                                    &&
                                    (
                                        Util_AboutBoard.On_Edge(my, taikyoku.GobanBounds) || Util_AboutBoard.On_Edge(nx, taikyoku.GobanBounds)
                                    )
                                )
                                {
                                    break;
                                }
                                else
                                {
                                    isContinueLoop = false;
                                    break;
                                }
                            case LocalZyoseki_StoneAttribute._5_YourPieceOnEdge:
                                if
                                (
                                    taikyoku.Goban.At(mnLocation) == taikyoku.YourColor  /* your piece on edge */
                                    &&
                                    (
                                        Util_AboutBoard.On_Edge(my, taikyoku.GobanBounds) || Util_AboutBoard.On_Edge(nx, taikyoku.GobanBounds)
                                    )
                                )
                                {
                                    break;
                                }
                                else
                                {
                                    isContinueLoop = false;
                                    break;
                                }
                            case LocalZyoseki_StoneAttribute._6_MyPieceOnEdge:
                                if
                                (
                                    taikyoku.Goban.At(mnLocation) == taikyoku.MyColor  /* my piece on edge */
                                    &&
                                    (
                                        Util_AboutBoard.On_Edge(my, taikyoku.GobanBounds) || Util_AboutBoard.On_Edge(nx, taikyoku.GobanBounds)
                                    )
                                )
                                {
                                    break;
                                }
                                else
                                {
                                    isContinueLoop = false;
                                    break;
                                }
                        }
                        ++k;
                    }
            
                    if (isContinueLoop) // パターンに一致しています。
                    {
                        int tryScore = Util_LocalZyoseki.Patterns[ptnNo].Score; // Gnugo1.2 では、tval 変数。


                        if (
                            // パターン番号 8～13 には、報酬とペナルティーを付けます。
                            8 <= ptnNo && ptnNo <= 13    /* patterns for expand region */
                           )
                        {
                            if (7 < Util_FindLocalZyoseki.DistanceFromCenter(tryLocation.I, taikyoku.GobanBounds))
                            {
                                // 中心から 8以上離れている（端と、その隣ぐらいの位置）ならば、ペナルティーを付けます。
                                tryScore--;
                            }
                            else if ((Util_FindLocalZyoseki.DistanceFromCenter(tryLocation.I, taikyoku.GobanBounds) == 6) || (Util_FindLocalZyoseki.DistanceFromCenter(tryLocation.I, taikyoku.GobanBounds) == 7))
                            {
                                // 中心から 6～7 離れている位置（端から3,4番目ぐらい）には、報酬を与えます。
                                tryScore++;
                            }

                            if (7 < Util_FindLocalZyoseki.DistanceFromCenter(tryLocation.J, taikyoku.GobanBounds))  /* penalty on line 1, 2 */
                            {
                                tryScore--;
                            }
                            else if ((Util_FindLocalZyoseki.DistanceFromCenter(tryLocation.J, taikyoku.GobanBounds) == 6) || (Util_FindLocalZyoseki.DistanceFromCenter(tryLocation.J, taikyoku.GobanBounds) == 7))
                            {
                                tryScore++;    /* reward on line 3, 4 */
                            }
                        }
                
                        if (tryScore > out_score)
                        {
                            out_score = tryScore;
                            out_location.SetLocation(tryLocation);
                        }
                    }
                }//for
            }

            if (0 < out_score)   // パターンにマッチしました。
            {
                return true;
            }
            else    // マッチに失敗しました。
            {
                return false;
            }
        }
    }
}