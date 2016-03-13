/**
 * C# Arrange of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-28
 *  
 * findpatn.c -> Util_FindOpeningZyoseki.cs
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
using Grayscale.GPL.P___405_CompZyoseki.L500_FindPattern;
using Grayscale.GPL.P___409_ComputerB__.L500_Computer;
using Grayscale.GPL.P160____Collection_.L500_Collection;
using Grayscale.GPL.P330____OpenZyoseki.L500_Opening;
using Grayscale.GPL.P405____CompZyoseki.L075_EmptyRectangle;
using Grayscale.GPL.P405____CompZyoseki.L250_FindLocalZyoseki;

namespace Grayscale.GPL.P405____CompZyoseki.L500_FindOpeningZyoseki
{
    /// <summary>
    /// 序盤の動きたちと、パターンたちからの、コンピューターの動きを探します。
    /// 
    /// Gnugo1.2 では findpatn関数。
    /// </summary>
    public class Util_FindOpeningZyoseki
    {
        /// <summary>
        /// 定石に入っているときの評価値。
        /// </summary>
        private const int ZYOSEKI_SCORE = 80;

        /// <summary>
        /// 次の動きのためにマッチする定石（パターン）を探します。
        /// 
        /// Gnugo1.2 では findpatn関数。
        /// </summary>
        /// <param name="out_location">次の動きの   行、列番号</param>
        /// <param name="out_score">Gnugo1.2 では val 引数。次の指し手の評価値</param>
        /// <param name="taikyoku"></param>
        /// <returns></returns>
        public static bool FindPattern
        (
            out GobanPoint out_location,
            out int out_score,
            Taikyoku taikyoku
        )
        {
            GobanPoint tryLocation;// Gnugo1.2 では、ti,tj という変数名
            int tryScore;// Gnugo1.2 では、tval という変数名。

            //
            // オープニングの定石は、盤面全体のものです。
            //
            // まず、序盤で占領（オキュパイ；occupy）できる角（corners）を狙い、
            // 次に、開いている四辺を狙っていきます。
            //

            //----------------------------------------
            // 定石[4] 最後の動きの続き
            //----------------------------------------
            //
            // 初回はOFFです。他の定石が順調に進んでいるとき、このフラグが立っています。
            //
            if (taikyoku.OpeningZyosekiFlag[4])
            {
                taikyoku.OpeningZyosekiFlag[4] = false; // この定石をOFFにします。

                // 前回のデータを取り出します。
                int cnd = ((ComputerPlayerB)taikyoku.ComputerPlayer).NodeNo;
                MoveType movetype = ((ComputerPlayerB)taikyoku.ComputerPlayer).Movetype;
                if (OpeningZyosekiImpl.Opening(out out_location, ref cnd, movetype, taikyoku))
                {
                    taikyoku.OpeningZyosekiFlag[4] = true; // もっと動くなら、この定石をONにリセットします。
                }
                ((ComputerPlayerB)taikyoku.ComputerPlayer).NodeNo = cnd;

                if (taikyoku.Goban.At(out_location) == StoneColor.Empty)  // 置けるなら
                {
                    out_score = Util_FindOpeningZyoseki.ZYOSEKI_SCORE;  // 定石の評価値
                    return true;
                }
                else
                {
                    // 石を置けなかったら、この定石も終わります。
                    taikyoku.OpeningZyosekiFlag[4] = false;
                }
            }

            //----------------------------------------
            // 定石[0] 西北の角
            //----------------------------------------
            if (taikyoku.OpeningZyosekiFlag[0])
            {
                taikyoku.OpeningZyosekiFlag[0] = false; // フラグをクリアー。
                if (Util_EmptyRectangle.IsEmptyRectangle(new GobanPointImpl( 0, 0),new GobanPointImpl( 5, 5),taikyoku))
                {

                    int cnd = 0;
                    MoveType movetype = MoveType.Basic;
                    OpeningZyosekiImpl.Opening(out out_location, ref cnd, movetype, taikyoku);  // 次の手のための新しいノードを取得します。
                    if (OpeningZyosekiImpl.Opening(out out_location, ref cnd, movetype, taikyoku))
                    {
                        taikyoku.OpeningZyosekiFlag[4] = true;
                    }
                    ((ComputerPlayerB)taikyoku.ComputerPlayer).NodeNo = cnd;
                    ((ComputerPlayerB)taikyoku.ComputerPlayer).Movetype = movetype;

                    out_score = Util_FindOpeningZyoseki.ZYOSEKI_SCORE;  // 評価値
                    return true;
                }
            }

            //----------------------------------------
            // 定石[1] 南西の角
            //----------------------------------------
            if (taikyoku.OpeningZyosekiFlag[1])
            {
                taikyoku.OpeningZyosekiFlag[1] = false;
                if (Util_EmptyRectangle.IsEmptyRectangle(new GobanPointImpl(13, 0), new GobanPointImpl(18, 5),taikyoku))
                {
                    int cnd = 0;
                    MoveType movetype = MoveType.Inverted;
                    OpeningZyosekiImpl.Opening(out out_location, ref cnd, movetype, taikyoku); // 次の手のための新しいノードを取得します。
                    if (OpeningZyosekiImpl.Opening(out out_location, ref cnd, movetype, taikyoku))
                    {
                        taikyoku.OpeningZyosekiFlag[4] = true;
                    }
                    ((ComputerPlayerB)taikyoku.ComputerPlayer).NodeNo = cnd;
                    ((ComputerPlayerB)taikyoku.ComputerPlayer).Movetype = movetype;

                    out_score = Util_FindOpeningZyoseki.ZYOSEKI_SCORE;  // 評価値
                    return true;
                }
            }

            //----------------------------------------
            // 定石[2] 北東の角
            //----------------------------------------
            if (taikyoku.OpeningZyosekiFlag[2])
            {
                taikyoku.OpeningZyosekiFlag[2] = false;
                if (Util_EmptyRectangle.IsEmptyRectangle(new GobanPointImpl(0, 13), new GobanPointImpl(5, 18),taikyoku))
                {
                    int cnd = 0;
                    MoveType movetype = MoveType.Reflected;
                    OpeningZyosekiImpl.Opening(out out_location, ref cnd, movetype, taikyoku);   // 次の手のための新しいノードを取得します。
                    if (OpeningZyosekiImpl.Opening(out out_location, ref cnd, movetype, taikyoku))
                    {
                        taikyoku.OpeningZyosekiFlag[4] = true;
                    }
                    ((ComputerPlayerB)taikyoku.ComputerPlayer).NodeNo = cnd;
                    ((ComputerPlayerB)taikyoku.ComputerPlayer).Movetype = movetype;

                    out_score = Util_FindOpeningZyoseki.ZYOSEKI_SCORE;  // 評価値
                    return true;
                }
            }

            //----------------------------------------
            // 定石[3] 南東の角
            //----------------------------------------
            if (taikyoku.OpeningZyosekiFlag[3])
            {
                taikyoku.OpeningZyosekiFlag[3] = false;
                if (Util_EmptyRectangle.IsEmptyRectangle(new GobanPointImpl(13, 13), new GobanPointImpl(18, 18),taikyoku))
                {
                    int cnd = 0;
                    MoveType movetype = MoveType.Inverted_And_Reflected;
                    OpeningZyosekiImpl.Opening(out out_location, ref cnd, movetype, taikyoku);   // 次の手のための新しいノードを取得します。
                    if (OpeningZyosekiImpl.Opening(out out_location, ref cnd, movetype, taikyoku))
                    {
                        taikyoku.OpeningZyosekiFlag[4] = true;
                    }
                    ((ComputerPlayerB)taikyoku.ComputerPlayer).NodeNo = cnd;
                    ((ComputerPlayerB)taikyoku.ComputerPlayer).Movetype = movetype;

                    out_score = Util_FindOpeningZyoseki.ZYOSEKI_SCORE;  // 評価値
                    return true;
                }
            }

            //
            // 辺（edges）のオキュパイ（occupy）
            //

            //----------------------------------------
            // 定石[5] 北辺
            //----------------------------------------
            //
            // 碁番の北側の矩形領域が空っぽなら、打ち込む場所の定石が１つあります。この定石はこの１箇所だけです。
            //
            if (taikyoku.OpeningZyosekiFlag[5])
            {
                taikyoku.OpeningZyosekiFlag[5] = false;
                if (Util_EmptyRectangle.IsEmptyRectangle(new GobanPointImpl(0, 6), new GobanPointImpl(4, 11),taikyoku))
                {
                    out_location = new GobanPointImpl(3,9);// 次の指し手の位置i,j
                    out_score = Util_FindOpeningZyoseki.ZYOSEKI_SCORE;  // 評価値
                    return true;
                }
            }

            //----------------------------------------
            // 定石[6] 南辺
            //----------------------------------------
            //
            // 碁番の南側の矩形領域が空っぽなら、打ち込む場所の定石が１つあります。この定石はこの１箇所だけです。
            //
            if (taikyoku.OpeningZyosekiFlag[6])
            {
                taikyoku.OpeningZyosekiFlag[6] = false;
                if (Util_EmptyRectangle.IsEmptyRectangle(new GobanPointImpl(18, 6), new GobanPointImpl(14, 11),taikyoku))
                {
                    out_location = new GobanPointImpl(15, 9);// 次の指し手の位置i,j
                    out_score = Util_FindOpeningZyoseki.ZYOSEKI_SCORE;  // 評価値
                    return true;
                }
            }

            //----------------------------------------
            // 定石[7] 西辺
            //----------------------------------------
            //
            // 碁番の西側の矩形領域が空っぽなら、打ち込む場所の定石が１つあります。この定石はこの１箇所だけです。
            //
            if (taikyoku.OpeningZyosekiFlag[7])
            {
                taikyoku.OpeningZyosekiFlag[7] = false;
                if (Util_EmptyRectangle.IsEmptyRectangle(new GobanPointImpl(6, 0), new GobanPointImpl(11, 4), taikyoku))
                {
                    out_location = new GobanPointImpl(9, 3);// 次の指し手の位置i,j
                    out_score = Util_FindOpeningZyoseki.ZYOSEKI_SCORE;  // 評価値
                    return true;
                }
            }

            //----------------------------------------
            // 定石[8] 東辺
            //----------------------------------------
            //
            // 碁番の東側の矩形領域が空っぽなら、打ち込む場所の定石が１つあります。この定石はこの１箇所だけです。
            //
            if (taikyoku.OpeningZyosekiFlag[8])
            {
                taikyoku.OpeningZyosekiFlag[8] = false;
                if (Util_EmptyRectangle.IsEmptyRectangle(new GobanPointImpl(6, 18), new GobanPointImpl(11, 14), taikyoku))
                {
                    out_location = new GobanPointImpl(9, 15);// 次の指し手の位置i,j
                    out_score = Util_FindOpeningZyoseki.ZYOSEKI_SCORE;  // 評価値
                    return true;
                }
            }

            //
            // 序盤定石のどれにも当てはまらなければ。
            //

            out_location = new GobanPointImpl(-1, -1);
            out_score = -1;

            //
            // 局地的な定石を探します。
            //
            for (int m = 0; m < taikyoku.GobanBounds.BoardSize; m++)
            {
                for (int n = 0; n < taikyoku.GobanBounds.BoardSize; n++)
                {
                    GobanPoint mnLocation = new GobanPointImpl(m, n);
                    if
                    (
                        taikyoku.Goban.At(mnLocation) == taikyoku.MyColor // コンピューターの石
                        &&
                        Util_FindLocalZyoseki.FindPattern(out tryLocation, out tryScore, mnLocation, taikyoku)
                        &&
                        out_score < tryScore   // 評価値が高ければ
                    )
                    {
                        out_score = tryScore;  // 評価値
                        out_location.SetLocation(tryLocation);// 次の指し手の位置i,j
                    }
                }
            }

            if (out_score > 0)   // 定石（パターン）を見つけました。
            {
                return true;
            }
            else    // 定石は見つかりませんでした。
            {
                return false;
            }
        }
    }
}