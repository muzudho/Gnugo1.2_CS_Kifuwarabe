/**
 * C# Arrange of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-28
 *  
 * getmove.c -> Util_CommandDriven.cs
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
using Grayscale.GPL.P___170_GameState__.L500_Struct;
using Grayscale.GPL.P___190_Board______.L063_Word;
using Grayscale.GPL.P___300_Taikyoku___.L500_Taikyoku;
using Grayscale.GPL.P160____Collection_.L500_Collection;
using Grayscale.GPL.P190____Board______.L260_PointFugo;
using Grayscale.GPL.P450____KeyValid___.L500_Suicide;
using Grayscale.GPL.P460____SaveLoad___.L500_SaveLoad;
using System;

namespace Grayscale.GPL.P470____KeyInput___.L500_CommandDriven
{
    /// <summary>
    /// 人間の入力コマンドを取得します。
    /// </summary>
    public abstract class Util_CommandDriven
    {
        /// <summary>
        /// 人間の入力したコマンドに対応した処理を行います。
        /// 間違った入力があった場合、再帰的に呼び出されます。
        /// 
        /// Gnugo1.2 では、getmove関数。
        /// </summary>
        /// <param name="move_charArray">入力した文字列。a1やT19などの指し手。</param>
        /// <param name="out_sasite">指し手。石を置く位置</param>
        /// <param name="taikyoku"></param>
        public static void DoCommand
        (
            string command_str,
            out GobanPoint out_sasite,
            Taikyoku taikyoku
        )
        {
            if (command_str == "stop")  // ゲームを終了します。
            {
                taikyoku.PlayState = GameState.Stop;
                out_sasite = new GobanPointImpl(-1, -1);// 2015-11-26 追加
            }
            else
            {
                if (command_str == "save")  // データを保存して、ゲームを終了します。
                {
                    // 局面を、テキストファイルに書き出します。
                    Util_Save.Save(taikyoku);

                    taikyoku.PlayState = GameState.Saved;
                    // i が -1 のときは、パスのシグナルです。
                    out_sasite = new GobanPointImpl(-1, -1);// 2015-11-26 追加
                }
                else if (command_str == "pass")  // 人間のパス
                {
                    taikyoku.Pass++;
                    // i が -1 のときは、パスのシグナルです。
                    out_sasite = new GobanPointImpl(-1, -1);// 2015-11-26 追加
                }
                else
                {
                    taikyoku.Pass = 0;
                    if (
                        // 例えば、 a1 や、 T19 といった入力文字を解析し、盤上の位置に変換します。
                        !PointFugoImpl.TryParse(command_str, out out_sasite,taikyoku)
                        ||
                        (taikyoku.Goban.LookColor(out_sasite) != StoneColor.Empty)
                        ||
                        Util_Suicide.Aa_Suicide(out_sasite, taikyoku)
                    )
                    {
                        //
                        // 非合法手だった場合、再入力を促します。
                        //
                        Console.WriteLine("illegal move !"); // [" + command_str + "] 2015-11-26 入力されたコマンドを表示するように拡張
                        Console.Write("your move? ");

                        // 続けて、再帰的に処理実行。
                        string command_str2 = Console.ReadLine();
                        //scanf("%s", move);

                        Util_CommandDriven.DoCommand(command_str2, out out_sasite, taikyoku);
                    }
                }
            }
        }
    }
}