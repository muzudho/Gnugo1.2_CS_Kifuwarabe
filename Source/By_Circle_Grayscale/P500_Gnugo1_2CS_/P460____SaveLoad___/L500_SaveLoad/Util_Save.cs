/**
 * C# Arrange of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-29
 *  
 * getmove.c -> Util_Save.cs
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
using Grayscale.GPL.P160____Collection_.L500_Collection;
using Grayscale.GPL.P310____ConvStone__.L500_Conv;
using System.IO;
using System.Text;

namespace Grayscale.GPL.P460____SaveLoad___.L500_SaveLoad
{
    /// <summary>
    /// 局面を、テキストファイルに書き出します。
    /// </summary>
    public class Util_Save
    {
        /// <summary>
        /// 局面を、テキストファイルに書き出します。
        /// 
        /// 19路盤、9路盤　両用です。
        /// </summary>
        /// <param name="taikyoku"></param>
        public static void Save(Taikyoku taikyoku)
        {
            StringBuilder sb = new StringBuilder();

            //fp = fopen("gnugo.dat", "w");   // 盤設定を保存します。

            for (int m = 0; m < taikyoku.GobanBounds.BoardSize; m++)
            {
                for (int n = 0; n < taikyoku.GobanBounds.BoardSize; n++)
                {
                    sb.Append(Conv_StoneColor.ToNumber(taikyoku.Goban.LookColor(new GobanPointImpl(m, n))));
                    //fprintf(fp, "%c", Util_GlobalVar.P[m,n]);
                }
            }

            // コンピューターの色、お互いの取ったピース
            sb.Append(Conv_StoneColor.ToNumber(taikyoku.MyColor));
            sb.Append(" ");
            sb.Append(taikyoku.Count_MyCaptured);
            sb.Append(" ");
            sb.Append(taikyoku.Count_YourCaptured);
            sb.Append(" ");
            //fprintf(fp, "%d %d %d ", Util_GlobalVar.Mymove, Util_GlobalVar.Mk, Util_GlobalVar.Uk);

            // 序盤定跡フラグ
            for (int index = 0; index < 9; index++)
            {
                sb.Append(taikyoku.OpeningZyosekiFlag[index] ? 1 : 0);
                sb.Append(" ");
                //fprintf(fp, "%d ", Util_GlobalVar.Opn[m]);
            }

            //fclose(fp);
            File.WriteAllText(taikyoku.SaveFileName, sb.ToString());

        }

    }
}
