/**
 * C# Arrange of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-28
 *  
 * suicide.c -> Util_Suicide.cs
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
using Grayscale.GPL.P340____Liberty____.L500_LibertyAll;

namespace Grayscale.GPL.P450____KeyValid___.L500_Suicide
{
    /// <summary>
    /// ����̔񍇖@��𒲂ׂ܂��B
    /// </summary>
    public abstract class Util_Suicide
    {
        /// <summary>
        /// ����̔Տ�i,j�̓��������E��łȂ������ׂ܂��B
        /// </summary>
        /// <param name="location">Gnugo1.2 �ł́Ai,j �Ƃ����������B</param>
        /// <param name="taikyoku"></param>
        /// <returns></returns>
        public static bool Aa_Suicide(
            GobanPoint location,
            Taikyoku taikyoku
            )
        {
            int banSize = taikyoku.GobanBounds.BoardSize;

            // �V���������̃��o�e�B�[�𐔂��Ȃ����܂��B
            int liberty; // Gnugo1.2 �ł́A�O���[�o���ϐ� lib = 0 �ł����B
            Util_CountLiberty.Count(out liberty, location, taikyoku.YourColor, taikyoku);
            if (liberty == 0)
            // ���ׂāA�����R���s���[�^�[�̃s�[�X�Y���E���āA�R�̉\��������΁A
            // �V���������͎��E��ł��B
            {
                // assume alive
                taikyoku.Goban.Put(location, taikyoku.YourColor);

                // �J�����g�F�̐΂̃��o�e�B�[�i�l���̐΂�u����Ƃ���j
                // 
                // Gnugo1.2 �ł́Al �Ƃ������O�̃O���[�o���ϐ��Bliberty �̗����낤���H
                // eval �œ��e���ݒ肳��A���̓��e�� exambord�Afindsavr�Afindwinr�Asuicide�Ŏg�p����܂��B
                int[,] libertyOfNodes;

                // �R���s���[�^�[�̌ċz�_�̐��𐔂��܂��B
                Util_LibertyAtNode.CountAll(out libertyOfNodes, taikyoku.MyColor, taikyoku);
                int k = 0;

                for (int m = 0; m < banSize; m++)
                {
                    for (int n = 0; n < banSize; n++)
                    {
                        // �E����邩������Ȃ��s�[�X�Y�𐔂��܂��B
                        if
                        (
                            (taikyoku.Goban.At(new GobanPointImpl(m, n)) == taikyoku.MyColor)
                            &&
                            libertyOfNodes[m,n] == 0
                        )
                        {
                            ++k;
                        }

                        if
                        (
                            (k == 0)
                            ||
                            (
                                k == 1
                                &&
                                (
                                    (location.I == taikyoku.YourKo.I)
                                    &&
                                    (location.J == taikyoku.YourKo.J)
                                )
                            )
                        )
                        // either no effect on my pieces or an illegal Ko take back
                        {
                            taikyoku.Goban.Put(location, StoneColor.Empty);   /* restore to open */
                            return true;
                        }
                        else
                        {
                            /* good move */
                            return false;
                        }
                    }
                }

                return false;// 2015-11-26 �ǉ�
            }
            else
            {
                /* valid move */
                return false;
            }
        }
    }
}
