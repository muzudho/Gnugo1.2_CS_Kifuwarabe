/**
 * C# Arrange of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-28
 *  
 * findnext.c -> Util_SasiteNext.cs
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
using Grayscale.GPL.P403____CompThink__.L075_OwnEye;

namespace Grayscale.GPL.P403____CompThink__.L125_SasiteNext
{
    /// <summary>
    /// �R���s���[�^�[�̎w�����T���܂��B
    /// 
    /// �ċA���܂��B�ׁA�ׂƌ��čs���܂��B
    /// 
    /// findsavr �Ŏg���܂��B
    /// </summary>
    public abstract class Util_SasiteNext
    {
        /// <summary>
        /// Liberty�����Ȃ��΁i�܂��͘A�j�A�܂�ア�΁i�܂��͘A�j�قǍ����]������֐��ł��B
        /// 
        /// Gnugo1.2 �ł� fval �֐��B
        /// </summary>
        /// <param name="newLiberty"></param>
        /// <param name="expectedLiberty"></param>
        /// <returns></returns>
        private static int Evaluate_LibertyWeak
        (
            int newLiberty, // Gnugo1.2 �ł� newlib �����B�V�������o�e�B�[
            int expectedLiberty  // Gnugo1.2 �ł� minlib �����B���o�e�B�[�̏��Ȃ�������񂵂Ă��郋�[�v�J�E���^�[�B
        )
        {
            int result_score; // Gnugo1.2 �ł� val �ϐ��B�]���l

            if (newLiberty <= expectedLiberty)
            {
                result_score = -1;   // �]���l�� -1
            }
            else
            {
                int upLiberty = newLiberty - expectedLiberty;    // ���������o�e�B�[

                result_score = 40  // 40����{�ɁB
                        +
                        (upLiberty - 1) * 50    // ���o�e�B�[�� 2 �ȏ㑝����Ȃ�A1 �����邽���o�e�B�[�� 50 �_�̃{�[�i�X�B
                        /
                        (expectedLiberty * expectedLiberty * expectedLiberty)  // �w�肵�����o�e�B�[���傫���قǃX�R�A������d�|���B
                                                                // 1 : 1 �_
                                                                // 2 : 8 �_
                                                                // 3 : 27 �_
                                                                // 4 : 64 �_
                        ;
            }

            return result_score;
        }

        /// <summary>
        /// �ʒum,n���܂ރO���[�v����A
        /// �㉺���E�ɗאڂ���΁i�܂��͘A�j�̌ċz�_�𒲂ׁA
        /// �ł��X�R�A�̍����΁i�܂��͘A�j�̏ꏊ i,j �ƁA�]���l��T���܂��B�i�A�̏ꍇ�A�ǂ����P�̐΂̏ꏊ�j
        ///
        /// Gnugo1.2 �ł� findnextmove �֐��B
        /// </summary>
        /// <param name="out_bestLocation">���̓�����   �s�A��ԍ�</param>
        /// <param name="out_bestScore">Gnugo1.2�ł� val �Ƃ������O�̃|�C���^�[�ϐ��B���̎w����̕]���l</param>
        /// <param name="curStone_location">�J�����g�E�X�g�[���� �s�ԍ� m�A��ԍ� n</param>
        /// <param name="expectedLiberty">�J�����g�E�X�g�[���̃��o�e�B�[�BGnugo1.2 �ł� minlib �ϐ��B</param>
        /// <param name="taikyoku"></param>
        /// <returns></returns>
        public static bool FindStone_LibertyWeak
        (
            out GobanPoint out_bestLocation,
            out int out_bestScore,
            GobanPoint curStone_location,
            int expectedLiberty,
            Taikyoku taikyoku
        )
        {
            GobanPoint tryLocation; // Gnugo1.2 �ł� ti,tj �Ƃ����ϐ����B
            tryLocation = new GobanPointImpl(0, 0);// 2015-11-26 �ǉ�
            int tryScore = 0;    // Gnugo1.2 �ł� tval �ϐ��B �׈ʒu�̕]���l�B    // 2015-11-26 �ǉ� 0 �ŏ������B
            bool found = false;

            out_bestLocation = new GobanPointImpl(-1,-1);
            out_bestScore = -1;

            // �J�����g�ʒu���}�[�N���܂��B
            taikyoku.MarkingBoard.Done_Current(curStone_location);

            //--------------------------------------------------------------------------
            // �k�l�C�o�[�𒲂ׂ܂��B
            if (!curStone_location.IsNorthEnd()) // �k�[�łȂ���΁B
            {
                if (taikyoku.Goban.NorthOf(curStone_location) == StoneColor.Empty)
                {
                    // �k�ׂ̈ʒui,j���Z�b�g
                    tryLocation.SetLocation(curStone_location.ToNorth());
            
                    // �ׂ̐΁i�܂��͘A�j�̃��o�e�B�[�𐔂��܂��B
                    int newLiberty; // Gnugo1.2 �ł́A�O���[�o���ϐ� lib = 0 �ł����B
                    Util_CountLiberty.Count(out newLiberty, tryLocation, taikyoku.MyColor, taikyoku);
                    tryScore = Util_SasiteNext.Evaluate_LibertyWeak(newLiberty, expectedLiberty);   // �]���l�v�Z
            
                    found = true;
                }
                else if
                (
                    taikyoku.Goban.NorthOf(curStone_location) == taikyoku.MyColor // �k�ׂ��R���s���[�^�[�̐΂ŁA
                    &&
                    taikyoku.MarkingBoard.CanDo_North(curStone_location) // �k�ׂ̃}�[�L���O�� 0 �Ȃ�
                )
                {
                    if (Util_SasiteNext.FindStone_LibertyWeak(out tryLocation, out tryScore, curStone_location.ToNorth(), expectedLiberty, taikyoku))    // �ċA�I�Ɍ���
                    {
                        found = true;
                    }
                }
            }

            if (found)  // ����������1
            {
                found = false;
                if (out_bestScore < tryScore && !Util_OwnEye.IsThis(tryLocation, taikyoku))
                {
                    out_bestScore = tryScore;    // �������̕]���l���c���Ă���H
                    out_bestLocation.SetLocation(tryLocation);// �������̌�_i,j���c���Ă���H
                }
            }

            //--------------------------------------------------------------------------
            // ��l�C�o�[�𒲂ׂ܂��B
            if (!curStone_location.IsSouthEnd(taikyoku.GobanBounds))    // ��[�łȂ���΁B
            {
                if (taikyoku.Goban.SouthOf(curStone_location) == StoneColor.Empty)
                {
                    // ��ׂ̐΁i�܂��͘A�j�̌ċz�_�̐��𒲂ׁA
                    // ���҂���ċz�_�̐����@�傫���قǍ����]���l��t���܂��B
                    tryLocation.SetLocation(curStone_location.ToSouth());

                    int adjLiberty; // Gnugo1.2 �ł́A�O���[�o���ϐ� lib = 0 �ł����B
                    Util_CountLiberty.Count(out adjLiberty, tryLocation, taikyoku.MyColor, taikyoku);
                    tryScore = Util_SasiteNext.Evaluate_LibertyWeak(adjLiberty, expectedLiberty);
                    found = true;
                }
                else if
                (
                    taikyoku.Goban.SouthOf(curStone_location) == taikyoku.MyColor
                    &&
                    taikyoku.MarkingBoard.CanDo_South(curStone_location) // �쑤
                )
                {
                    if (Util_SasiteNext.FindStone_LibertyWeak(out tryLocation, out tryScore, curStone_location.ToSouth(), expectedLiberty, taikyoku))
                    {
                        found = true;
                    }
                }
            }

            if (found)  // ����������1
            {
                found = false;
                if (out_bestScore < tryScore && !Util_OwnEye.IsThis(tryLocation, taikyoku))
                {
                    out_bestScore = tryScore;
                    out_bestLocation.SetLocation(tryLocation);
                }
            }

            //--------------------------------------------------------------------------
            // ���l�C�o�[�𒲂ׂ܂��B
            if (!curStone_location.IsWestEnd()) // ���[�łȂ���΁B
            {
                if (taikyoku.Goban.WestOf(curStone_location) == StoneColor.Empty)
                {
                    tryLocation.SetLocation(curStone_location.ToWest());

                    int adjLiberty; // Gnugo1.2 �ł́A�O���[�o���ϐ� lib = 0 �ł����B
                    Util_CountLiberty.Count(out adjLiberty, tryLocation, taikyoku.MyColor, taikyoku);
                    tryScore = Util_SasiteNext.Evaluate_LibertyWeak(adjLiberty, expectedLiberty);
                    found = true;
                }
                else
                {
                    if (
                        taikyoku.Goban.WestOf(curStone_location) == taikyoku.MyColor
                        &&
                        taikyoku.MarkingBoard.CanDo_West(curStone_location) // ����
                        )
                    {
                        if (Util_SasiteNext.FindStone_LibertyWeak(out tryLocation, out tryScore, curStone_location.ToWest(), expectedLiberty, taikyoku))
                        {
                            found = true;
                        }
                    }
                }
            }

            if (found)  // �������Ă���� 1
            {
                found = false;
                if (tryScore > out_bestScore && !Util_OwnEye.IsThis(tryLocation, taikyoku))
                {
                    out_bestScore = tryScore;
                    out_bestLocation.SetLocation(tryLocation);
                }
            }

            //--------------------------------------------------------------------------
            // ���l�C�o�[�𒲂ׂ܂��B
            if (!curStone_location.IsEastEnd(taikyoku.GobanBounds))    // ���[�łȂ���΁B
            {
                if (taikyoku.Goban.EastOf(curStone_location) == StoneColor.Empty)
                {
                    tryLocation.SetLocation(curStone_location.ToEast());

                    int adjLiberty; // Gnugo1.2 �ł́A�O���[�o���ϐ� lib = 0 �ł����B
                    Util_CountLiberty.Count(out adjLiberty, tryLocation, taikyoku.MyColor, taikyoku);
                    tryScore = Util_SasiteNext.Evaluate_LibertyWeak(adjLiberty, expectedLiberty);
                    found = true;
                }
                else
                {
                    if
                    (
                        taikyoku.Goban.EastOf(curStone_location) == taikyoku.MyColor
                        &&
                        taikyoku.MarkingBoard.CanDo_East(curStone_location) // ����
                    )
                    {
                        if (Util_SasiteNext.FindStone_LibertyWeak(out tryLocation, out tryScore, curStone_location.ToEast(), expectedLiberty, taikyoku))
                        {
                            found = true;
                        }
                    }
                }
            }

            if (found)  // Gnugo1.2�ł́A�������Ă���� 1 �ł����B
            {
                found = false;
                if (out_bestScore < tryScore && !Util_OwnEye.IsThis(tryLocation, taikyoku))
                {
                    out_bestScore = tryScore;
                    out_bestLocation.SetLocation(tryLocation);
                }
            }

            //--------------------------------------------------------------------------
            if (0 < out_bestScore)   // ���̓������������B
            {
                return true;
            }
            else    // ���̓����͎��s�B
            {
                return false;
            }
        }
    }
}