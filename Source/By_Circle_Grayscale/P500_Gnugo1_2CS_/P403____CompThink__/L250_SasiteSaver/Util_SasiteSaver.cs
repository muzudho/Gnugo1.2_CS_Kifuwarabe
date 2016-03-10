/**
 * C# Arrange of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-28
 *  
 * findsavr.c -> Util_SasiteSaver.cs
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
using Grayscale.GPL.P___300_Taikyoku___.L500_Taikyoku;
using Grayscale.GPL.P160____Collection_.L500_Collection;
using Grayscale.GPL.P403____CompThink__.L125_SasiteNext;

namespace Grayscale.GPL.P403____CompThink__.L250_SasiteSaver
{
    /// <summary>
    /// �΂����̍U������̖h��̂��߂̃R���s���[�^�[�̎��̓�����T���܂��B
    /// 
    /// �R���s���[�^�[�̎v�l���Ɏg���܂��B
    /// </summary>
    public abstract class Util_SasiteSaver
    {
        /// <summary>
        /// �����A����̃s�[�X�Y����������Ă���Ȃ�A������T���܂��B
        /// 
        /// Gnugo1.2 �ł� findsaver �֐��B
        /// </summary>
        /// <param name="out_location">���̎w�����   �s�A��ԍ�</param>
        /// <param name="out_score">Gnugo1.2 �ł́Aval �����B���̎w����̕]���l</param>
        /// <param name="libertyOfPiece_eachPoint">
        /// �J�����g�F�̐΂̃��o�e�B�[�i�l���̐΂�u����Ƃ���j
        /// 
        /// Gnugo1.2 �ł́Al �Ƃ������O�̃O���[�o���ϐ��Bliberty �̗����낤���H
        /// eval �œ��e���ݒ肳��A���̓��e�� exambord�Afindsavr�Afindwinr�Asuicide�Ŏg�p����܂��B
        /// </param>
        /// <param name="taikyoku"></param>
        /// <returns></returns>
        public static bool FindSasite
        (
            out GobanPoint out_location,
            out int out_score,
            int[,] libertyOfPiece_eachPoint,
            Taikyoku taikyoku
        )
        {
            GobanPoint tryLocation; // Gnugo1.2 �ł́Ati,tj �Ƃ����ϐ����B
            int tryScore;    // Gnugo1.2 �ł́Atval �ϐ��B�]���l

            out_location = new GobanPointImpl(-1,-1);// �ʒui,j
            out_score = -1;  // �]���l

            //
            // ���o�e�B�[�̏��Ȃ������珇�ɕ]����t���Ă����܂��B
            //
            for (int iLiberty = 1; iLiberty < 4; iLiberty++)// ���o�e�B�[ 1�`3 �̃��[�v�J�E���^�[�BGnugo1.2 �ł� minlib �ϐ��B
            {
                // �ŏ����o�e�B�[�Ƃ�������̃s�[�X�𐔂��܂��B
                for (int m = 0; m < taikyoku.GobanBounds.BoardSize; m++)
                {
                    for (int n = 0; n < taikyoku.GobanBounds.BoardSize; n++)
                    {
                        GobanPoint mnLocation = new GobanPointImpl(m, n);
                        if
                        (
                            taikyoku.Goban.LookColor(mnLocation) == taikyoku.MyColor // �R���s���[�^�[�̐F
                            &&
                            libertyOfPiece_eachPoint[m,n] == iLiberty // �l���̃��o�e�B�[�̐�
                        )
                        {
                            // �Z�[�u�E�s�[�X�Y�ւ̓�����T���܂��B
                            taikyoku.MarkingBoard.Initmark(taikyoku.GobanBounds.BoardSize);
                            if
                            (
                                Util_SasiteNext.FindSasite(out tryLocation, out tryScore, mnLocation, iLiberty, taikyoku)
                                &&
                                (tryScore > out_score)   // �]���l���������
                            )
                            {
                                out_score = tryScore;    // �]���l
                                out_location.SetLocation(tryLocation);// �ʒui,j
                            }
                        }
                    }
                }
            }

            if (out_score > 0)   // �������������
            {
                return true;
            }
            else    // ������������Ȃ�������
            {
                return false;
            }
        }
    }
}