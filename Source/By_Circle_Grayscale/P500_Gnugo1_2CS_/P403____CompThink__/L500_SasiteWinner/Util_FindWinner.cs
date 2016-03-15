/**
 * C# Arrange of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-28
 *  
 * findwinr.c -> Util_FindWinner.cs
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
using Grayscale.GPL.P403____CompThink__.L037_FindOpen;
using System.Collections.Generic;

namespace Grayscale.GPL.P403____CompThink__.L500_SasiteWinner
{

    /// <summary>
    /// 相手の石たちを攻撃するコンピューターの次の動きを探します。
    /// 
    /// コンピューターの思考中に使われます。
    /// </summary>
    public abstract class Util_FindWinner
    {
        /// <summary>
        /// 取るか、攻撃する相手のピースを探します。
        /// 
        /// Gnugo1.2 では findwinner 関数。
        /// </summary>
        /// <param name="out_bestLocation">次の動きの   行、列番号</param>
        /// <param name="out_bestScore">Gnugo1.2 では *val 引数。評価値</param>
        /// <param name="libertyOfNodes">
        /// カレント色の石のリバティー（四方の石を置けるところ）
        /// 
        /// Gnugo1.2 では、l という名前のグローバル変数。liberty の略だろうか？
        /// eval で内容が設定され、その内容は exambord、findsavr、findwinr、suicideで使用されます。
        /// </param>
        /// <param name="taikyoku"></param>
        /// <returns></returns>
        public static bool FindBestLocation
        (
            out GobanPoint out_bestLocation,
            out int out_bestScore,
            int[,] libertyOfNodes,
            Taikyoku taikyoku
        )
        {
            int banSize = taikyoku.GobanBounds.BoardSize;

            // 要素数 3 以下のリスト。
            List<GobanPoint> adj3Locations = new List<GobanPoint>(3);// Gnugo1.2 では、それぞれ要素数[3]の ti配列、tj配列。

            int tryScore;// Gnugo1.2 では tval 変数。

            out_bestLocation = new GobanPointImpl(-1, -1);// 位置i,j
            out_bestScore = -1;  // 評価値

            //
            // リバティー（四方の置けるところ）が３つ以下の相手（人間）の石を探します。
            // つまり、つながっている石（色が異なる石とつながっている場合もあり）か、端にある石ということです。
            //
            for (int m = 0; m < banSize; m++)
            {
                for (int n = 0; n < banSize; n++)
                {
                    GobanPoint iLocation = new GobanPointImpl(m, n);

                    if (
                        // 相手（人間）の石で、
                        taikyoku.Goban.At(iLocation) == taikyoku.YourColor
                        &&
                        // この石（あるいは連全体）で、呼吸点が３以下のものを選びます。
                        // 少なくとも　石か枠につながっている石であることを想定しています。
                        libertyOfNodes[m, n] < 4
                    )
                    {
                        // これからフラグを立てていくためにクリアーします。
                        taikyoku.MarkingBoard.Initmark(taikyoku.GobanBounds.BoardSize);

                        if
                        (
                            // この石（連ではなく）の開いている方向（１方向～３方向あるはずです）
                            Util_FindOpen.FindOpen3Locations(adj3Locations, iLocation, taikyoku.YourColor, libertyOfNodes[m, n], taikyoku)
                        )
                        {
                            if (libertyOfNodes[m,n] == 1)
                            {
                                // アタリの状態なので、積極的に狙っていきます。（呼吸点が、どこか１方向しかない状態）

                                if (out_bestScore < 120) // 評価値が 120 未満なら
                                {
                                    // アタリの評価値は 120 点はあります。
                                    // この位置の評価を上げ、ベストムーブとして更新します。
                                    out_bestScore = 120;
                                    out_bestLocation.SetLocation(adj3Locations[0]);
                                }
                            }
                            else
                            {
                                // アタリではなくとも。

                                // 呼吸点の数（２～たくさん）に応じて。
                                int opens = libertyOfNodes[m, n];

                                // わたし（コンピューター）が置いたときと、相手（人間）に置き返されたときの
                                // 全パターンについて

                                // 配列のインデックスが 0,1 や、0,2 や、 1,2 など、異なるペア com,man になるもの
                                // 全てについて。
                                for (int iCom = 0; iCom < opens; iCom++)//わたし（コンピューター）という想定
                                {
                                    for (int iMan = 0; iMan < opens; iMan++)//相手（人間）という想定
                                    {
                                        if (iCom != iMan)
                                        {
                                            // 置く位置
                                            GobanPoint adjLocation_com = adj3Locations[iCom];
                                            // 置き返す位置
                                            GobanPoint adjLocation_man = adj3Locations[iMan];

                                            // わたしの（連または）石のリバティ
                                            int liberty_com; // Gnugo1.2 では、グローバル変数 lib = 0 でした。
                                            Util_CountLiberty.Count(out liberty_com, adjLocation_com, taikyoku.MyColor, taikyoku);

                                            if (0 < liberty_com)    // 妥当性チェック
                                            {
                                                // コンピューターの色の石を　位置 a に（試しに）置きます。
                                                taikyoku.Goban.Put(adjLocation_com, taikyoku.MyColor);
                                        
                                                // look ahead opponent move
                                                int liberty_man; // Gnugo1.2 では、グローバル変数 lib = 0 でした。
                                                Util_CountLiberty.Count(out liberty_man, adjLocation_man, taikyoku.YourColor, taikyoku);
                                                if
                                                (
                                                    1 == liberty_com  // 隣接する私（コンピューター）側の（連または）石の呼吸点は１個。
                                                    &&
                                                    0 < liberty_man   // 隣接するあなた（人間）側の（連または）石の呼吸点は１個以上。
                                                )
                                                {
                                                    // 人間側の呼吸点の方が、コンピューター側と同じ、あるいは多いので、
                                                    // 位置 a に置く価値なし。
                                                    tryScore = 0;
                                                }
                                                else
                                                {
                                                    // コンピューターが置いた手より、
                                                    // 人間が置く手に、呼吸点が同じ、また多い手がない場合、置く価値あり。
                                                    tryScore = 120 - 20 * liberty_man;
                                                }
                                            
                                                if (out_bestScore < tryScore)
                                                {
                                                    // より評価値の高い指し手を見つけました。ベストムーブの位置を更新します。
                                                    out_bestScore = tryScore;
                                                    out_bestLocation.SetLocation(adjLocation_com);
                                                }

                                                // （試しに置いた）石を取り除きます。盤上を元に戻すだけです。
                                                taikyoku.Goban.Put(adjLocation_com, StoneColor.Empty);
                                            }
                                        }
                                    }//v
                                }//u
                            }
                        }
                    }
                }//n
            }//m

            if (0 < out_bestScore)   // 指し手を見つけた。
            {
                return true;
            }
            else    // 勝者の指し手を見つけるのには失敗した。
            {
                return false;
            }
        }
    }
}