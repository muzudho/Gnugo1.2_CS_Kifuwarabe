/**
 * C# Arrange of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-28
 *  
 * exambord.c -> new file.
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
using Grayscale.GPL.P340____Liberty____.L500_LibertyAll;

namespace Grayscale.GPL.P350____RemoveStone.L500_RemoveStones
{
    /// <summary>
    /// 囲われた石を、盤上から除外します。コウにならないかどうか、注意します。
    /// 
    /// Gnugo1.2 では、exambord関数。
    /// </summary>
    public abstract class Util_RemoveStones_Surrounded
    {
        /// <summary>
        /// 囲われた石を、盤上から除外します。コウにならないかどうか、注意します。
        /// 
        /// Gnugo1.2 では、exambord関数。
        /// </summary>
        /// <param name="colorKo">取られた方の、黒 or 白</param>
        /// <param name="taikyoku"></param>
        public static void RemoveStones_Surrounded
        (
            StoneColor colorKo,
            Taikyoku taikyoku
        )
        {
            // 盤上の全てのポイントに、次の数字を記憶させます。
            //
            // その地点が、四方でつながっている石のかたまり（piece）のとき、
            // そのピース全体でのリバティーを数え、ポイントの１つ１つにその数字を覚えさせます。
            int[,] libertyOfPiece_eachPoint;           
            Util_CountLibertyAll.Count_LibertyOfPiece_EachPoint(out libertyOfPiece_eachPoint, colorKo, taikyoku);

            // 取った石の位置を初期化します。（コウで戻さなくてはならない石の位置を覚えていたもの）
            if (colorKo == taikyoku.MyColor)
            {
                taikyoku.MyKo.MoveToVanish();
            }
            else
            {
                taikyoku.YourKo.MoveToVanish();
            }


            // コウの動きで取れる石の数。
            int countOfDelete = 0;

            // リバティーのない石は全て碁番から削除します。
            // つまり四方を全て囲まれたピース（石のあつまり）です。
            for (int i = 0; i < taikyoku.GobanBounds.BoardSize; i++)
            {
                for (int j = 0; j < taikyoku.GobanBounds.BoardSize; j++)
                {
                    GobanPoint location = new GobanPointImpl(i, j);

                    if
                    (
                        // 取られる側の石であり、
                        taikyoku.Goban.LookColor(location) == colorKo
                        &&
                        // ピースのリバティーが記録されていないなら
                        libertyOfPiece_eachPoint[i,j] == 0
                    )
                    {
                        // 石を除外します。
                        taikyoku.Goban.Put(location, StoneColor.Empty);

                        // （後で元に戻すこともあるので）取った石の位置を記憶し、取られた石の数をカウントアップします。
                        if (colorKo == taikyoku.MyColor)
                        {
                            // コンピューター側
                            taikyoku.MyKo.SetLocation(location);
                            ++taikyoku.Count_MyCaptured;
                        }
                        else
                        {
                            // 人間側
                            taikyoku.YourKo.SetLocation(location);
                            ++taikyoku.Count_YourCaptured;
                        }
                        ++countOfDelete;    // この指し手で取った駒の数？
                    }

                    // 取った石が２つ以上なら、コウ（Ko）の可能性がなくなります。石の位置の記憶をリセットします。
                    if (colorKo == taikyoku.MyColor && 1 < countOfDelete)
                    {
                        // コンピューターが取った石の位置（コウになるかもしれない）をクリアー。
                        taikyoku.MyKo.MoveToVanish();
                    }
                    else if (1 < countOfDelete) // TODO: こっちは 1 < countOfDelete しなくていいのか？
                    {
                        // 人間が取った石の位置（コウになるかもしれない）をクリアー。
                        taikyoku.YourKo.MoveToVanish();   
                    }

                }//j
            }//i
        }

    }
}