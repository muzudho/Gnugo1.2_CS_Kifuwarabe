/**
 * C# Arrange of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-28
 *  
 * patterns.h -> Util_LocalZyoseki.cs
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
using Grayscale.GPL.P___405_CompZyoseki.L125_LocalZyoseki;
using Grayscale.GPL.P160____Collection_.L500_Collection;

namespace Grayscale.GPL.P405____CompZyoseki.L125_LocalZyoseki
{
    /// <summary>
    /// 方向（orientation）をくるくる回すなどの、行列計算（transformation）を何度も試すための x,y 座標と、属性。
    /// 
    /// Gnugo1.2 では、 patval 構造体。
    /// </summary>
    public class ZyosekiStoneImpl
    {
        /// <summary>
        /// 行列計算用の座標
        /// 
        /// Gnugo1.2 では、単に構造体の x,y というメンバー・プロパティー。
        /// </summary>
        public GyoretuPoint P{get;set;}

        /// <summary>
        /// 属性。
        /// att = 0 - empty,
        ///       1 - your piece,
        ///       2 - my piece,
        ///       3 - my next move
        ///       4 - empty on edge,
        ///       5 - your piece on edge,
        ///       6 - my piece on edge
        /// </summary>
        public LocalZyoseki_StoneAttribute Att { get; set; }

        public ZyosekiStoneImpl(
            GyoretuPoint p,
            LocalZyoseki_StoneAttribute att
            )
        {
            this.P = p;
            this.Att = att;
        }
    }

    /// <summary>
    /// 局地的な定石。
    /// 
    /// Gnugo1.2 では、 pattern 静的構造体。
    /// </summary>
    public class LocalZyosekiImpl
    {
        /// <summary>
        /// 定石の中の石。
        /// 
        /// Gnugo1.2 では、patn という変数名で、配列のサイズは MAXPC定数で 16 と指定されていた。
        /// </summary>
        public ZyosekiStoneImpl[] Stones{get;set;}
        
        /// <summary>
        /// 使用する行列変換の最後の番号
        /// 
        /// 左右対象図形になっていて、左右反転しなくていい定石は 4 を、
        /// それ以外は 8 を指定してください。
        /// 
        /// Gnugo1.2 では、trfno という変数名。
        /// </summary>
        public int LastTrfNo{get;set;}

        /// <summary>
        /// パターンの評価値
        /// 
        /// Gnugo1.2 では、patwt という変数名。
        /// </summary>
        public int Score { get; set; }

        public LocalZyosekiImpl(
            ZyosekiStoneImpl[] stones,
            int lastTrfNo,
            int score
            )
        {
            this.Stones = stones;
            this.LastTrfNo = lastTrfNo;
            this.Score = score;
        }
    }

    public class Util_LocalZyoseki
    {
        /// <summary>
        /// 0 ～ 24 のパターンがあるが、22 は欠番。
        /// [PATNO]
        /// 
        /// Gnugo1.2 では pat という名前の静的構造体配列。pattern構造体を要素として持っていた。
        /// また、 PATNO = 24 という要素数サイズを表す定数を用意していたが、C# では配列の要素数は 配列が持っているので、不要になった。
        /// また、pattern 24 の要素数は 10と指定されていたが、間違いと思われたので 8 に修正した。
        /// </summary>
        public static LocalZyosekiImpl[] Patterns = new LocalZyosekiImpl[]
        {
            //   pattern 0: 232   connect if invaded
            //      010
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 0), LocalZyoseki_StoneAttribute._2_MyPiece),// 最初の要素は必ず、起点となる石（0,0）。端にあるかどうかの違いはある。
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 0), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 1), LocalZyoseki_StoneAttribute._1_YourPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 1), LocalZyoseki_StoneAttribute._0_Empty)
                },
                4,// 左右対称の定石は4、それ以外は 8。
                82// この定石の評価値。
            ),
            // pattern 1: 230   connect if invaded
            //    012
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 0), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 1), LocalZyoseki_StoneAttribute._1_YourPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 1), LocalZyoseki_StoneAttribute._2_MyPiece)
                },
                8,
                82
            ),
            // pattern 2: 212   try to attack
            //    030
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 0), LocalZyoseki_StoneAttribute._1_YourPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 1), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 1), LocalZyoseki_StoneAttribute._0_Empty)
                },
                4,
                82
            ),
            // pattern 3: 2302   connect if invaded
            //    0100
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 0), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 1), LocalZyoseki_StoneAttribute._1_YourPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(3, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(3, 1), LocalZyoseki_StoneAttribute._0_Empty)
                },
                8,
                83
            ),
            // pattern 4: 20302   connect if invaded
            // 00100
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 0), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 1), LocalZyoseki_StoneAttribute._1_YourPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(3, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(3, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(4, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(4, 1), LocalZyoseki_StoneAttribute._0_Empty)
                },
                4,
                84
            ),
            // pattern 5: 203   form eye to protect
            //    021
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 1), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 0), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 1), LocalZyoseki_StoneAttribute._1_YourPiece)
                },
                8,
                82
            ),
            // pattern 6: 202    form eye to protect
            // 031
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 1), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 1), LocalZyoseki_StoneAttribute._1_YourPiece)
                },
                8,
                82
            ),
            // pattern 7: 230   connect if invaded
            //    102
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 1), LocalZyoseki_StoneAttribute._1_YourPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 0), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 1), LocalZyoseki_StoneAttribute._2_MyPiece)
                },
                8,
                82
            ),
            // pattern 8: 200000
            //  00030  extend
            //  00000
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(3, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(4, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(5, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(3, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(4, 1), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(5, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 2), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 2), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(3, 2), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(4, 2), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(5, 2), LocalZyoseki_StoneAttribute._0_Empty)
                },
                8,
                80
            ),
            //pattern 9:  2
            //000
            //000  extend
            //000
            //030
            //000
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(-1, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 1, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(-1, 2), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 2), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 1, 2), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(-1, 3), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 3), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 1, 3), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(-1, 4), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 4), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 1, 4), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(-1, 5), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 5), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 1, 5), LocalZyoseki_StoneAttribute._0_Empty)
                },
                4,
                80
            ),
            //pattern 10: 20000
            //0030  extend
            //0000
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(3, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(4, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(3, 1), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(4, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 2), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 2), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(3, 2), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(4, 2), LocalZyoseki_StoneAttribute._0_Empty)
                },
                8,
                79
            ),
            //pattern 11:    2
            //        000
            //        000  extend
            //        030
            //        000
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(-1, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 1, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(-1, 2), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 2), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 1, 2), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(-1, 3), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 3), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 1, 3), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(-1, 4), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 4), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 1, 4), LocalZyoseki_StoneAttribute._0_Empty)
                },
                4,
                79
            ),
            //pattern 12: 2000
            //     030  extend
            //     000
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(3, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 1), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(3, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 2), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 2), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(3, 2), LocalZyoseki_StoneAttribute._0_Empty)
                },
                8,
                76
            ),
            //pattern 13:    2
            //        000  extend
            //        030
            //        000
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(-1, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 1, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(-1, 2), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 2), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 1, 2), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(-1, 3), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 3), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 1, 3), LocalZyoseki_StoneAttribute._0_Empty)
                },
                 4,
                76
            ),
            //pattern 14: 643   form eye on the edge
            //     20
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 0), LocalZyoseki_StoneAttribute._6_MyPieceOnEdge),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 0), LocalZyoseki_StoneAttribute._4_EmptyOnEdge),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 1), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 0), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 1), LocalZyoseki_StoneAttribute._0_Empty)
                },
                 8,
                80
            ),
            //pattern 15: 646    solidify eye on the edge
            //        231
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 0), LocalZyoseki_StoneAttribute._6_MyPieceOnEdge),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 0), LocalZyoseki_StoneAttribute._4_EmptyOnEdge),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 1), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 0), LocalZyoseki_StoneAttribute._6_MyPieceOnEdge),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 1), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(3, 1), LocalZyoseki_StoneAttribute._1_YourPiece)
                },
                 8,
                75
            ),
            //pattern 16: 646    solidify eye on the edge
            //     230
            //      1
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 0), LocalZyoseki_StoneAttribute._6_MyPieceOnEdge),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 0), LocalZyoseki_StoneAttribute._4_EmptyOnEdge),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 1), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 0), LocalZyoseki_StoneAttribute._6_MyPieceOnEdge),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 1), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 2), LocalZyoseki_StoneAttribute._1_YourPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(3, 1), LocalZyoseki_StoneAttribute._0_Empty)
                },
                 8,
                75
            ),
            //pattern 17: 646    solidify eye on the edge
            //     230
            //      0
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 0), LocalZyoseki_StoneAttribute._6_MyPieceOnEdge),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 0), LocalZyoseki_StoneAttribute._4_EmptyOnEdge),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 1), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 0), LocalZyoseki_StoneAttribute._6_MyPieceOnEdge),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 1), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 2), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(3, 1), LocalZyoseki_StoneAttribute._0_Empty)
                },
                 8,
                75
            ),
            //pattern 18:    2       form eye on center
            //        202
            //     3
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(-1, 1), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 1, 1), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 2), LocalZyoseki_StoneAttribute._3_MyNextMove)
                },
                 4,
                80
            ),
            //pattern 19:    2       solidify eye on center
            //        202
            //     231
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(-1, 1), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 1, 1), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 2), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 1, 2), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 2, 2), LocalZyoseki_StoneAttribute._1_YourPiece)
                },
                 8,
                75
            ),
            //pattern 20:    2       solidify eye on center
            //        202
            //     230
            //      0
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(-1, 1), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 1, 1), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 2), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 1, 2), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 2, 2), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 1, 3), LocalZyoseki_StoneAttribute._0_Empty)
                },
                 8,
                75
            ),
            //pattern 21:    2      solidify eye on center
            //        202
            //     230
            //      1
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(-1, 1), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 1, 1), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 2), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 1, 2), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 2, 2), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 1, 3), LocalZyoseki_StoneAttribute._1_YourPiece)
                },
                 8,
                75
            ),
            //pattern 23: 20100     connect if invaded
            //        00302
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 0), LocalZyoseki_StoneAttribute._1_YourPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 1), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(3, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(3, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(4, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(4, 1), LocalZyoseki_StoneAttribute._2_MyPiece)
                },
                 8,
                84
            ),
            //pattern 24: 2100    connect if invaded
            //        0302
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 0), LocalZyoseki_StoneAttribute._1_YourPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 1), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(3, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(3, 1), LocalZyoseki_StoneAttribute._2_MyPiece)
                },
                 8,
                83
            )
        };
    }
}