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
using Grayscale.GPL.P___190_Board______.L250_Board;
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
        /// <param name="out_bestLocation">���̎w�����   �s�A��ԍ�</param>
        /// <param name="out_maxScore">Gnugo1.2 �ł́Aval �����B���̎w����̕]���l</param>
        /// <param name="liberty_eachPoint">
        /// �J�����g�F�́i�P�A�܂��͘A�́j�΂̃��o�e�B�[�i�l���̐΂�u����Ƃ���j
        /// 
        /// Gnugo1.2 �ł́Al �Ƃ������O�̃O���[�o���ϐ��Bliberty �̗����낤���H
        /// eval �œ��e���ݒ肳��A���̓��e�� exambord�Afindsavr�Afindwinr�Asuicide�Ŏg�p����܂��B
        /// </param>
        /// <param name="taikyoku"></param>
        /// <returns></returns>
        public static bool FindLocation_LibertyWeak
        (
            out GobanPoint out_bestLocation,
            out int out_maxScore,
            int[,] liberty_eachPoint,
            Taikyoku taikyoku
        )
        {
            Board ban = taikyoku.Goban; // ���
            int banSize = taikyoku.GobanBounds.BoardSize; //���H��

            out_bestLocation = new GobanPointImpl(-1,-1);// �ʒui,j
            out_maxScore = -1;  // �]���l

            //
            // �܂��@���o�e�B���P�A
            // ���Ɂ@���o�e�B���Q�A
            // �ƁA���o�e�B�̏��Ȃ������珇�ɕ]����t���Ă����܂��B
            // �Ֆʂ��S��T����邱�ƂɂȂ�܂��B
            //
            for (int iExpectedLiberty = 1; iExpectedLiberty < 4; iExpectedLiberty++)// ���o�e�B�[ 1�`3 �̃��[�v�J�E���^�[�BGnugo1.2 �ł� minlib �ϐ��B
            {
                // �ŏ����o�e�B�[�Ƃ�������̃s�[�X�𐔂��܂��B
                for (int m = 0; m < banSize; m++)
                {
                    for (int n = 0; n < banSize; n++)
                    {
                        GobanPoint iLocation = new GobanPointImpl(m, n);//�������̈ʒu�B

                        if
                        (
                            ban.At(iLocation) == taikyoku.MyColor // �R���s���[�^�[�̐F
                            &&
                            liberty_eachPoint[m,n] == iExpectedLiberty // �w�肵�����o�e�B�[�̐�
                        )
                        {
                            GobanPoint tryLocation; // Gnugo1.2 �ł́Ati,tj (try i,j)�Ƃ����ϐ����B
                            int tryScore;    // Gnugo1.2 �ł́Atval �ϐ��B�]���l

                            // �Z�[�u�E�s�[�X�Y�ւ̓�����T���܂��B
                            taikyoku.MarkingBoard.Initmark(taikyoku.GobanBounds.BoardSize);
                            if
                            (
                                Util_SasiteNext.FindStone_LibertyWeak(out tryLocation, out tryScore, iLocation, iExpectedLiberty, taikyoku)
                                &&
                                (out_maxScore < tryScore)   // �]���l�������i���o�e�B�[�����Ȃ��Ċ댯�j�Ȃ��
                            )
                            {
                                out_maxScore = tryScore;    // �]���l
                                out_bestLocation.SetLocation(tryLocation);// �ʒui,j
                            }
                        }
                    }
                }
            }

            if (0 < out_maxScore)   // �������������
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