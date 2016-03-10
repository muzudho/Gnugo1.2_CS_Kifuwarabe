/**
 * C# Arrange 2.0 of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-28
 *  
 * -> new file.
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
using Grayscale.GPL.P___180_ComputerA__.L500_Computer;
using Grayscale.GPL.P___181_BoardPrinterA.L500_BoardPrinterA;
using Grayscale.GPL.P___190_Board______.L063_Word;
using Grayscale.GPL.P___190_Board______.L125_WorkingBoard;
using Grayscale.GPL.P___190_Board______.L250_Board;
using Grayscale.GPL.P___300_Taikyoku___.L250_Goban;
using System;

namespace Grayscale.GPL.P___300_Taikyoku___.L500_Taikyoku
{
    public interface Taikyoku
    {

        /// <summary>
        /// 碁盤の矩形。
        /// </summary>
        GobanRectangleB GobanBounds { get; set; }

        /// <summary>
        /// 盤を表示するものです。
        /// </summary>
        BoardPrinterA BoardPrinter { get; set; }


        /// <summary>
        /// ランダム値生成用。
        /// </summary>
        Random Random { get; set; }

        /// <summary>
        /// 碁盤
        /// </summary>
        Board Goban { get; set; }

        /// <summary>
        /// マーキングのための作業用行列
        /// 
        /// findopen で使用。
        /// findnextmove で使用。
        /// </summary>
        MarkingBoard MarkingBoard { get; set; }

        /// <summary>
        /// マークのための作業用行列。
        /// 
        /// Gnugo1.2 では、C言語のブーリアン型の書き方で、
        /// 1はマークされていない（真）、
        /// 0はマークされている（偽）、としていました。
        /// countlib.c で 全要素を1で埋めていました。
        /// count で要素を false に変更されていきました。
        ///
        /// マークしたということは、
        /// もう数えたので次は加算しないということです。
        /// </summary>
        CountedBoard CountedBoard { get; set; }

        /// <summary>
        /// 自分（コンピューター）が取った石の位置。
        /// 石を取ったものの、コウだったので石を戻さないといけない、というときに利用？
        /// 
        /// Gnugo1.2 では、mik,mjk という名前の静的グローバル変数。meのi,jのk（コウ）という意味だろうか？
        /// </summary>
        GobanPoint MyKo { get; set; }

        /// <summary>
        /// 相手（人間）が取った石の位置。
        /// 石を取ったものの、コウだったので石を戻さないといけない、というときに利用？
        /// 
        /// Gnugo1.2 では、uik,ujk という名前の静的グローバル変数。youのi,jのk（コウ）という意味だろうか？
        /// </summary>
        GobanPoint YourKo { get; set; }

        /// <summary>
        /// 自分（コンピューター）の色（指し手）
        /// 
        /// Gnugo1.2 では、mymove という名前の静的グローバル変数。
        /// </summary>
        StoneColor MyColor { get; set; }

        /// <summary>
        /// 相手（人間）の色（指し手）
        /// 
        /// Gnugo1.2 では、umove という名前の静的グローバル変数。your move の略だろうか？
        /// </summary>
        StoneColor YourColor { get; set; }

        /// <summary>
        /// ゲームの状態
        /// 
        /// Gnugo1.2 では、play という名前の静的グローバル変数。
        /// </summary>
        GameState PlayState { get; set; }

        /// <summary>
        /// パスを示します。0,1,2が入ります。
        /// </summary>
        int Pass { get; set; }

        /// <summary>
        /// 取られたピース
        /// 
        /// Gnugo1.2 では、mk,uk という名前の静的グローバル変数。meとyouのk（キャプチャー）という意味だろうか？
        /// </summary>
        int Count_MyCaptured { get; set; }
        int Count_YourCaptured { get; set; }

        /// <summary>
        /// 序盤定石のON/OFFフラグ。ブーリアン型として利用するが、画面には ONなら 1、OFFなら 0 と表示されるのかと思う。
        /// 
        /// Gnugo1.2 では、opn という名前の静的グローバル変数。opening の略だろうか？
        /// </summary>
        bool[] OpeningZyosekiFlag { get; set; }

        /// <summary>
        /// コンピューター・プレイヤー。
        /// </summary>
        ComputerPlayerA ComputerPlayer { get; }

        /// <summary>
        /// セーブファイル名。
        /// </summary>
        string SaveFileName { get; }

    }
}
