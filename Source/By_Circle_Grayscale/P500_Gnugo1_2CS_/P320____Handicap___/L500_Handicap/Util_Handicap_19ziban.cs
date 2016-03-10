/**
 * C# Arrange of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-28
 *  
 * sethand.c -> Util_Handicap_19ziban.cs
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
using Grayscale.GPL.P___190_Board______.L063_Word;
using Grayscale.GPL.P___300_Taikyoku___.L500_Taikyoku;
using Grayscale.GPL.P160____Collection_.L500_Collection;

namespace Grayscale.GPL.P320____Handicap___.L500_Handicap
{
    /// <summary>
    /// �����̂��߂̃n���f�B�[�L���b�v�E�s�[�X�Y���Z�b�g�A�b�v���܂��B
    /// 19�H�՗p�B
    /// </summary>
    public abstract class Util_Handicap_19ziban
    {
        /// <summary>
        /// �n���f�B�[�L���b�v�E�s�[�X�Y���Z�b�g�A�b�v���܂��B
        /// 
        /// Gnugo1.2 �ł́A sethand �Ƃ����֐��ł��B
        /// </summary>
        /// <param name="handicap"></param>
        /// <param name="taikyoku"></param>
        public static void DoHandicap(int handicap, Taikyoku taikyoku)
        {
            if (0 < handicap) { taikyoku.Goban.Put(new GobanPointImpl(3, 3), StoneColor.Black); }
            else { goto gt_EndMethod; }

            if (1 < handicap) { taikyoku.Goban.Put(new GobanPointImpl(15, 15), StoneColor.Black); }
            else { goto gt_EndMethod; }

            if (2 < handicap) { taikyoku.Goban.Put(new GobanPointImpl(3, 15), StoneColor.Black); }
            else { goto gt_EndMethod; }

            if (3 < handicap) { taikyoku.Goban.Put(new GobanPointImpl(15, 3), StoneColor.Black); }
            else { goto gt_EndMethod; }

            if (handicap == 5)
            {
                taikyoku.Goban.Put(new GobanPointImpl(9, 9), StoneColor.Black);
                goto gt_EndMethod;
            }

            if (5 < handicap)
            {
                taikyoku.Goban.Put(new GobanPointImpl(9, 15), StoneColor.Black);
                taikyoku.Goban.Put(new GobanPointImpl(9, 3), StoneColor.Black);
            }
            else { goto gt_EndMethod; }

            if (7 == handicap)
            {
                // �n���f�B�L���b�v 7 �́A 9,9 �̈ʒu�B
                taikyoku.Goban.Put(new GobanPointImpl(9, 9), StoneColor.Black);
                goto gt_EndMethod;
            }

            if (7 < handicap)
            {
                // �n���f�B�L���b�v 8�ȏ� �́A 9,9 �ɒu���O�� 15,9�A3,9 �̈ʒu�B
                taikyoku.Goban.Put(new GobanPointImpl(15, 9), StoneColor.Black);
                taikyoku.Goban.Put(new GobanPointImpl(3, 9), StoneColor.Black);
            }
            else { goto gt_EndMethod; }

            if (8 < handicap) { taikyoku.Goban.Put(new GobanPointImpl(9, 9), StoneColor.Black); }
            else { goto gt_EndMethod; }

            if (9 < handicap) { taikyoku.Goban.Put(new GobanPointImpl(2, 2), StoneColor.Black); }
            else { goto gt_EndMethod; }

            if (10 < handicap) { taikyoku.Goban.Put(new GobanPointImpl(16, 16), StoneColor.Black); }
            else { goto gt_EndMethod; }

            if (11 < handicap) { taikyoku.Goban.Put(new GobanPointImpl(2, 16), StoneColor.Black); }
            else { goto gt_EndMethod; }

            if (12 < handicap) { taikyoku.Goban.Put(new GobanPointImpl(16, 2), StoneColor.Black); }
            else { goto gt_EndMethod; }

            if (13 < handicap) { taikyoku.Goban.Put(new GobanPointImpl(6, 6), StoneColor.Black); }
            else { goto gt_EndMethod; }

            if (14 < handicap) { taikyoku.Goban.Put(new GobanPointImpl(12, 12), StoneColor.Black); }
            else { goto gt_EndMethod; }

            if (15 < handicap) { taikyoku.Goban.Put(new GobanPointImpl(6, 12), StoneColor.Black); }
            else { goto gt_EndMethod; }

            if (16 < handicap) { taikyoku.Goban.Put(new GobanPointImpl(12, 6), StoneColor.Black); }

        gt_EndMethod:
            ;
        }
    }
}