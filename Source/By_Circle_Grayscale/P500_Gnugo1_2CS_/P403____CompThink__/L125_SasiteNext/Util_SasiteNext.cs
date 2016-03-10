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
        /// �V����������]������֐��ł��B
        /// 
        /// Gnugo1.2 �ł� fval �֐��B
        /// </summary>
        /// <param name="newLiberty"></param>
        /// <param name="iLiberty"></param>
        /// <returns></returns>
        private static int Evaluate
        (
            int newLiberty, // Gnugo1.2 �ł� newlib �����B�V�������o�e�B�[
            int iLiberty  // Gnugo1.2 �ł� minlib �����B���o�e�B�[�̏��Ȃ�������񂵂Ă��郋�[�v�J�E���^�[�B
        )
        {
            int result_score; // Gnugo1.2 �ł� val �ϐ��B�]���l

            if (newLiberty <= iLiberty)
            {
                result_score = -1;   // �]���l�� -1
            }
            else
            {
                int k = newLiberty - iLiberty;    // ���o�e�B�[�̍�

                result_score = 40  // 40����{�ɁB
                        +
                        (k - 1) * 50    // ���o�e�B�[�̍��̂��悻 50�{
                        /
                        (iLiberty * iLiberty * iLiberty)  // ���o�e�B�[�͂R��̉��l
                        ;
            }

            return result_score;
        }

        /// <summary>
        /// �ʒum,n���܂ރO���[�v����A�V�����w���� i,j ��T���܂��B
        ///
        /// Gnugo1.2 �ł� findnextmove �֐��B
        /// </summary>
        /// <param name="out_location">���̓�����   �s�A��ԍ�</param>
        /// <param name="out_score">Gnugo1.2�ł� val �Ƃ������O�̃|�C���^�[�ϐ��B���̎w����̕]���l</param>
        /// <param name="curStone_location">�J�����g�E�X�g�[���� �s�ԍ� m�A��ԍ� n</param>
        /// <param name="iLiberty">�J�����g�E�X�g�[���̃��o�e�B�[�BGnugo1.2 �ł� minlib �ϐ��B</param>
        /// <param name="taikyoku"></param>
        /// <returns></returns>
        public static bool FindSasite
        (
            out GobanPoint out_location,
            out int out_score,
            GobanPoint curStone_location,
            int iLiberty,
            Taikyoku taikyoku
        )
        {
            GobanPoint tryLocation; // Gnugo1.2 �ł� ti,tj �Ƃ����ϐ����B
            tryLocation = new GobanPointImpl(0, 0);// 2015-11-26 �ǉ�
            int tryScore = 0;    // Gnugo1.2 �ł� tval �ϐ��B �׈ʒu�̕]���l�B    // 2015-11-26 �ǉ� 0 �ŏ������B
            bool found = false;

            out_location = new GobanPointImpl(-1,-1);
            out_score = -1;

            // �J�����g�ʒu���}�[�N���܂��B
            taikyoku.MarkingBoard.Done_Current(curStone_location);

            //--------------------------------------------------------------------------
            // �k�l�C�o�[�𒲂ׂ܂��B
            if (!curStone_location.IsNorthEnd()) // �k�[�łȂ���΁B
            {
                if (taikyoku.Goban.LookColor_NorthOf(curStone_location) == StoneColor.Empty)
                {
                    // �k�ׂ̈ʒui,j���Z�b�g
                    tryLocation.SetLocation(curStone_location.ToNorth());
            
                    // ���o�e�B�[�𐔂��܂��B
                    int libertyOfPiece; // Gnugo1.2 �ł́A�O���[�o���ϐ� lib = 0 �ł����B
                    Util_CountLiberty.Count_LibertyOfPiece(out libertyOfPiece, tryLocation, taikyoku.MyColor, taikyoku);
                    tryScore = Util_SasiteNext.Evaluate(libertyOfPiece, iLiberty);   // �]���l�v�Z
            
                    found = true;
                }
                else if
                (
                    taikyoku.Goban.LookColor_NorthOf(curStone_location) == taikyoku.MyColor // �k�ׂ��R���s���[�^�[�̐΂ŁA
                    &&
                    taikyoku.MarkingBoard.CanDo_North(curStone_location) // �k�ׂ̃}�[�L���O�� 0 �Ȃ�
                )
                {
                    if (Util_SasiteNext.FindSasite(out tryLocation, out tryScore, curStone_location.ToNorth(), iLiberty, taikyoku))    // �ċA�I�Ɍ���
                    {
                        found = true;
                    }
                }
            }

            if (found)  // ����������1
            {
                found = false;
                if (out_score < tryScore && !Util_OwnEye.IsOwnEye(tryLocation, taikyoku))
                {
                    out_score = tryScore;    // �������̕]���l���c���Ă���H
                    out_location.SetLocation(tryLocation);// �������̌�_i,j���c���Ă���H
                }
            }

            //--------------------------------------------------------------------------
            // ��l�C�o�[�𒲂ׂ܂��B
            if (!curStone_location.IsSouthEnd(taikyoku.GobanBounds))    // ��[�łȂ���΁B
            {
                if (taikyoku.Goban.LookColor_SouthOf(curStone_location) == StoneColor.Empty)
                {
                    // ��ׂ̈ʒui,j���Z�b�g
                    tryLocation.SetLocation(curStone_location.ToSouth());

                    int libertyOfPiece; // Gnugo1.2 �ł́A�O���[�o���ϐ� lib = 0 �ł����B
                    Util_CountLiberty.Count_LibertyOfPiece(out libertyOfPiece, tryLocation, taikyoku.MyColor, taikyoku);
                    tryScore = Util_SasiteNext.Evaluate(libertyOfPiece, iLiberty);
                    found = true;
                }
                else if
                (
                    taikyoku.Goban.LookColor_SouthOf(curStone_location) == taikyoku.MyColor
                    &&
                    taikyoku.MarkingBoard.CanDo_South(curStone_location) // �쑤
                )
                {
                    if (Util_SasiteNext.FindSasite(out tryLocation, out tryScore, curStone_location.ToSouth(), iLiberty, taikyoku))
                    {
                        found = true;
                    }
                }
            }

            if (found)  // ����������1
            {
                found = false;
                if (out_score < tryScore && !Util_OwnEye.IsOwnEye(tryLocation, taikyoku))
                {
                    out_score = tryScore;
                    out_location.SetLocation(tryLocation);
                }
            }

            //--------------------------------------------------------------------------
            // ���l�C�o�[�𒲂ׂ܂��B
            if (!curStone_location.IsWestEnd()) // ���[�łȂ���΁B
            {
                if (taikyoku.Goban.LookColor_WestOf(curStone_location) == StoneColor.Empty)
                {
                    // ���ׂ̈ʒui,j���Z�b�g
                    tryLocation.SetLocation(curStone_location.ToWest());

                    int libertyOfPiece; // Gnugo1.2 �ł́A�O���[�o���ϐ� lib = 0 �ł����B
                    Util_CountLiberty.Count_LibertyOfPiece(out libertyOfPiece, tryLocation, taikyoku.MyColor, taikyoku);
                    tryScore = Util_SasiteNext.Evaluate(libertyOfPiece, iLiberty);
                    found = true;
                }
                else
                {
                    if (
                        taikyoku.Goban.LookColor_WestOf(curStone_location) == taikyoku.MyColor
                        &&
                        taikyoku.MarkingBoard.CanDo_West(curStone_location) // ����
                        )
                    {
                        if (Util_SasiteNext.FindSasite(out tryLocation, out tryScore, curStone_location.ToWest(), iLiberty, taikyoku))
                        {
                            found = true;
                        }
                    }
                }
            }

            if (found)  // �������Ă���� 1
            {
                found = false;
                if (tryScore > out_score && !Util_OwnEye.IsOwnEye(tryLocation, taikyoku))
                {
                    out_score = tryScore;
                    out_location.SetLocation(tryLocation);
                }
            }

            //--------------------------------------------------------------------------
            // ���l�C�o�[�𒲂ׂ܂��B
            if (!curStone_location.IsEastEnd(taikyoku.GobanBounds))    // ���[�łȂ���΁B
            {
                // p[m][n + 1] �͓��ׁB
                if (taikyoku.Goban.LookColor_EastOf(curStone_location) == StoneColor.Empty)
                {
                    // ���ׂ̈ʒui,j���Z�b�g
                    tryLocation.SetLocation(curStone_location.ToEast());

                    int libertyOfPiece; // Gnugo1.2 �ł́A�O���[�o���ϐ� lib = 0 �ł����B
                    Util_CountLiberty.Count_LibertyOfPiece(out libertyOfPiece, tryLocation, taikyoku.MyColor, taikyoku);
                    tryScore = Util_SasiteNext.Evaluate(libertyOfPiece, iLiberty);
                    found = true;
                }
                else
                {
                    if
                    (
                        taikyoku.Goban.LookColor_EastOf(curStone_location) == taikyoku.MyColor
                        &&
                        taikyoku.MarkingBoard.CanDo_East(curStone_location) // ����
                    )
                    {
                        if (Util_SasiteNext.FindSasite(out tryLocation, out tryScore, curStone_location.ToEast(), iLiberty, taikyoku))
                        {
                            found = true;
                        }
                    }
                }
            }

            if (found)  // Gnugo1.2�ł́A�������Ă���� 1 �ł����B
            {
                found = false;
                if (out_score < tryScore && !Util_OwnEye.IsOwnEye(tryLocation, taikyoku))
                {
                    out_score = tryScore;
                    out_location.SetLocation(tryLocation);
                }
            }

            //--------------------------------------------------------------------------
            if (0 < out_score)   // ���̓������������B
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