/**
 * C# Arrange of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-28
 *  
 * findcolr.c -> Util_SeitiZi.cs
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

namespace Grayscale.GPL.P490____EndOfGame__.L250_SeitiZi
{
    /// <summary>
    /// 整地の地。
    /// </summary>
    public abstract class Util_SeitiZi
    {

        /// <summary>
        /// 整地された地かどうかを、確認します。
        /// 
        /// 場所を１つ指定し、
        /// その４方向は 石がないか、同じ色の石だけで構成されている、
        /// そういう場所は 整地されている自分の陣地として正しい。
        /// 
        /// Gnugo1.2 では findcolr 関数。
        /// </summary>
        /// <param name="location">Gnugo1.2 では、 行番号 i = 0～18、列番号 j = 0～18。</param>
        /// <param name="taikyoku"></param>
        /// <returns></returns>
        public static StoneColor Test_SeitiZi
        (
            GobanPoint location,
            Taikyoku taikyoku
        )
        {
            //----------------------------------------
            // 実装の説明
            //----------------------------------------
            //
            // 盤上の交点を１つ指定します。
            //
            // そこに石があれば、その色を返します。
            //
            // 石がなければ、４方向の直線上で最初に突き当たる石の色を調べます。
            // （まるで、将棋の飛車の動きのように）
            //

            StoneColor result = StoneColor.Empty;     // 色 // 2015-11-26 追加 Emptyで初期化。

            /// [0]北、[1]南、[2]西、[3]東
            StoneColor[] color = new StoneColor[4];
                

            //
            // 指定した盤上の位置に石があれば、その色を返します。
            //
            if (taikyoku.Goban.At(location) != StoneColor.Empty)
            {
                return taikyoku.Goban.At(location);
            }

            // 北ネイバーを、石に突き当たるまで調べます。
            if (!location.IsNorthEnd())//北端でなければ
            {
                // 指定の行番号 ～ 0 まで。突き当たらなければ k は 北端 まで。
                int k = location.I; 
                do
                {
                    --k;
                }
                while (
                    taikyoku.Goban.At(new GobanPointImpl(k, location.J)) == StoneColor.Empty
                    &&
                    0 < k
                );
                color[0] = taikyoku.Goban.At(new GobanPointImpl(k, location.J));
            }
            else
            {
                // 空っぽ。
                color[0] = StoneColor.Empty;
            }

            // 南ネイバーを、石に突き当たるまで調べます。
            if (!location.IsSouthEnd(taikyoku.GobanBounds))
            {
                // （18より小さい、指定の行番号）～ 南端 まで。
                int k = location.I;
                do
                {
                    ++k;
                }
                while
                (
                    taikyoku.Goban.At(new GobanPointImpl(k, location.J)) == StoneColor.Empty
                    &&
                    k < taikyoku.GobanBounds.BoardEnd
                );
                color[1] = taikyoku.Goban.At(new GobanPointImpl(k, location.J));
            }
            else
            {
                // 空っぽ。
                color[1] = StoneColor.Empty;
            }

            // 西ネイバーを、石に突き当たるまで調べます。
            if (!location.IsWestEnd())
            {
                // （0より大きい、指定の列番号）～ 1 まで。
                int k = location.J;
                do
                {
                    --k;
                }
                while
                (
                    taikyoku.Goban.At(new GobanPointImpl(location.I, k)) == StoneColor.Empty
                    &&
                    k > 0
                );
                color[2] = taikyoku.Goban.At(new GobanPointImpl(location.I, k));
            }
            else
            {
                // 空っぽ。
                color[2] = StoneColor.Empty;
            }

            // 東ネイバーを、石に突き当たるまで調べます。
            if (!location.IsEastEnd(taikyoku.GobanBounds))
            {
                // （18より小さい、指定の列番号）～ 17 まで。
                int k = location.J;
                do
                {
                    ++k;
                }
                while
                (
                    taikyoku.Goban.At(new GobanPointImpl(location.I, k)) == StoneColor.Empty
                    &&
                    k < taikyoku.GobanBounds.BoardEnd
                );
                color[3] = taikyoku.Goban.At(new GobanPointImpl(location.I, k));
            }
            else
            {
                // 空っぽ。
                color[3] = StoneColor.Empty;
            }

            // ４方向のうち、空っぽでない交点の色を１つだけ知りたい。
            for (int k=0;k<4;k++)
            {
                if (color[k] == StoneColor.Empty)
                {
                    continue;
                }
                else
                {
                    result = color[k];
                    break;
                }
            }

            // クロスチェックします。
            // もし  わたしたちがエラーを見つけたなら、全ての死ピースは盤から取らない。
            // わたしたちはこれを修正するようプレイヤーたちに促す。
            for (int k=0;k<4;k++)
            {
                // 色が設定されているのに、返す結果の色が違っていたらエラーです。問題です。
                if
                (
                    color[k] != StoneColor.Empty
                    &&
                    color[k] != result
                )
                {
                    return StoneColor.Empty;// 0;
                }
            }

            // もし、わたしたちが全てのチェックにOKすれば、結果をレポートします。
            return result;
        }
    }
}