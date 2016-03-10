/**
 * C# Arrange 2.0 of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-29
 *  
 * endgame.c -> Util_EndOfGame.cs
 *           -> Scene_99_EndOfGameImpl.cs
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
using Grayscale.GPL.P___300_Taikyoku___.L500_Taikyoku;
using Grayscale.GPL.P___480_Print______.L500_Print;
using Grayscale.GPL.P490____EndOfGame__.L500_EndOfGame;
using System;

namespace Grayscale.GPL.P500____Scenes_____.L500_Scenes
{
    /// <summary>
    /// ゲーム終了後のシーン進行です。
    /// </summary>
    public class Scene_99_EndOfGameImpl
    {

        /// <summary>
        /// シーンを進行。
        /// 
        /// つながったデッド・ピースを、コンソールから１つ１つ手入力し、
        /// 整地をしたのち、勝者をアナウンスします。
        /// 
        /// Gnugo1.2 では、endgame関数。
        /// </summary>
        /// <param name="taikyoku"></param>
        public void DoScene(Taikyoku taikyoku)
        {

            //----------------------------------------
            // 石の数え方を、３ステップに分けて進めます。
            //----------------------------------------
            //
            // （１）死んでいる石を盤上から全て取り除く。
            // （２）どちらの陣地でもないところを埋める。
            // （３）石を数えて、勝者をアナウンスする。
            //
            // まず、全体の説明と、１ステップ目の説明を表示します。
            //
            Console.WriteLine();
            Console.WriteLine("To count score, we need the following steps:");
            Console.WriteLine("First, I need you to remove all dead pieces on the board.");
            Console.WriteLine("Second, I need you to fill in neutral territories with pieces.");
            Console.WriteLine("Last, I will fill in all pieces and announce the winner.");
            Console.WriteLine();
            Console.WriteLine("First, you should enter the dead pieces (black and white) to be removed.  Enter");
            Console.WriteLine(" 'stop' when you have finished.");

            //----------------------------------------
            // 死んでいる石のつながりを、盤上から削除します。
            //----------------------------------------
            Util_EndOfGame.Remove_DeadPieces(taikyoku);

            //----------------------------------------
            // どちらの地（陣地）でもない交点を、黒か白の石で交互に埋めます。
            //----------------------------------------
            Console.WriteLine("Next, you need to fill in pieces (black and white) in all neutral territories.");
            Console.WriteLine("Enter your and my pieces alternately and enter 'stop' when finish");
            Util_EndOfGame.FillNeutralTerritories(taikyoku);

            // この時点で、整地を済ませたとします。

            // 空きスペースを、どちらの陣地か判定し、その陣地の石に変換します。
            Util_EndOfGame.FillStones(taikyoku);

            // 盤上の石を数えます。
            int myTotal; // Gnugo1.2 では mtot という変数名。my total の略だろうか？
            int yourTotal; // Gnugo1.2 では utot という変数名。your total の略だろうか？
            Util_EndOfGame.CountPieces(out myTotal, out yourTotal, taikyoku);

            //----------------------------------------
            // 整地後の碁盤を表示し、勝者をアナウンスします。
            //----------------------------------------
            ((BoardPrinterB)taikyoku.BoardPrinter).ShowBoard(taikyoku);
            Console.WriteLine("Your total number of pieces {0}", yourTotal);
            Console.WriteLine("My total number of pieces {0}", myTotal);
            Console.ReadLine();
        }

    }
}
