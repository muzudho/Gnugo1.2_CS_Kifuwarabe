/**
 * C# Arrange of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-29
 *  
 * main.c -> Util_Load.cs
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
using Grayscale.GPL.P160____Collection_.L500_Collection;
using Grayscale.GPL.P190____Board______.L125_WorkingBoard;
using Grayscale.GPL.P190____Board______.L250_Board;
using Grayscale.GPL.P300____Taikyoku___.L500_Taikyoku;
using Grayscale.GPL.P310____ConvStone__.L500_Conv;
using Grayscale.GPL.P409____ComputerB__.L500_Computer;
using Grayscale.GPL.P480____Print______.L500_Print;
using System.IO;

namespace Grayscale.GPL.P460____SaveLoad___.L500_SaveLoad
{
    /// <summary>
    /// セーブファイルを読み込み、局面を復元します。
    /// </summary>
    public class Util_Load
    {
        /// <summary>
        /// セーブファイルを読み込み、局面を復元します。
        /// </summary>
        /// <param name="taikyoku"></param>
        /// <param name="boardSize">19路盤なら19、9路盤なら9と入れてください。</param>
        public static void Load(out Taikyoku taikyoku, int boardSize)
        {
            BoardPrinterB boardPrinterB;
            switch (boardSize)
            {
                case 9: boardPrinterB = new N9zibanPrinterImpl(); break;
                default: boardPrinterB = new N19zibanPrinterImpl(); break;
            }

            taikyoku = new TaikyokuImpl(
                boardSize,
                new ComputerPlayerBImpl(),
                new BoardImpl(boardSize),
                new MarkingBoardImpl(boardSize),
                new CountedBoardImpl(boardSize),
                boardPrinterB
                );


            string gnugoDatText = File.ReadAllText(taikyoku.SaveFileName);


            // 盤設定を読み込みます。
            for (int i = 0; i < taikyoku.GobanBounds.BoardSize; i++)
            {
                for (int j = 0; j < taikyoku.GobanBounds.BoardSize; j++)
                {
                    taikyoku.Goban.Put(new GobanPointImpl(i, j), Conv_StoneColor.FromNumber(int.Parse(gnugoDatText.Substring(0, 1))));
                    gnugoDatText = gnugoDatText.Substring(1);
                    //fscanf(fp, "%c", ref ;
                }
            }

            // コンピューターの色、取ったピースズを読み込みます。
            int ix;
            ix = gnugoDatText.IndexOf(" ");
            taikyoku.MyColor = Conv_StoneColor.FromNumber(int.Parse(gnugoDatText.Substring(0, ix)));
            gnugoDatText = gnugoDatText.Substring(ix
                + 1//空白の次へ
                );

            ix = gnugoDatText.IndexOf(" ");
            taikyoku.Count_MyCaptured = int.Parse(gnugoDatText.Substring(0, ix));
            gnugoDatText = gnugoDatText.Substring(ix + 1);

            ix = gnugoDatText.IndexOf(" ");
            taikyoku.Count_YourCaptured = int.Parse(gnugoDatText.Substring(0, ix));
            gnugoDatText = gnugoDatText.Substring(ix + 1);

            //fscanf(fp, "%d %d %d ", ref Util_GlobalVar.Mymove,
            //    ref Util_GlobalVar.Mk, ref Util_GlobalVar.Uk);

            // 序盤定跡フラグを読み込みます。
            for (int index = 0; index < 9; index++)
            {
                ix = gnugoDatText.IndexOf(" ");
                taikyoku.OpeningZyosekiFlag[index] = int.Parse(gnugoDatText.Substring(0, ix)) != 0;
                gnugoDatText = gnugoDatText.Substring(ix + 1);
                //fscanf(fp, "%d ", ref Util_GlobalVar.Opn[i]);
            }

            //fclose(fp);
            taikyoku.YourColor = Conv_StoneColor.FromNumber(3 - (int)taikyoku.MyColor);
        }
    }
}
