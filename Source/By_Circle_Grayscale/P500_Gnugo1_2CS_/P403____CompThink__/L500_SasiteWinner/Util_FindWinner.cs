/**
 * C# Arrange of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-28
 *  
 * findwinr.c -> Util_FindWinner.cs
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
using Grayscale.GPL.P403____CompThink__.L037_FindOpen;
using System.Collections.Generic;

namespace Grayscale.GPL.P403____CompThink__.L500_SasiteWinner
{

    /// <summary>
    /// ����̐΂������U������R���s���[�^�[�̎��̓�����T���܂��B
    /// 
    /// �R���s���[�^�[�̎v�l���Ɏg���܂��B
    /// </summary>
    public abstract class Util_FindWinner
    {
        /// <summary>
        /// ��邩�A�U�����鑊��̃s�[�X��T���܂��B
        /// 
        /// Gnugo1.2 �ł� findwinner �֐��B
        /// </summary>
        /// <param name="out_location">���̓�����   �s�A��ԍ�</param>
        /// <param name="out_score">Gnugo1.2 �ł� *val �����B�]���l</param>
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
            // �v�f�� 3 �ȉ��̃��X�g�B
            List<GobanPoint> trayLocations = new List<GobanPoint>(3);// Gnugo1.2 �ł́A���ꂼ��v�f��[3]�� ti�z��Atj�z��B

            int tryScore;// Gnugo1.2 �ł� tval �ϐ��B

            out_location = new GobanPointImpl(-1, -1);// �ʒui,j
            out_score = -1;  // �]���l

            //
            // ���o�e�B�[�i�l���̒u����Ƃ���j���R�ȉ��̑���i�l�ԁj�̐΂�T���܂��B
            // �܂�A�Ȃ����Ă���΁i�F���قȂ�΂ƂȂ����Ă���ꍇ������j���A�[�ɂ���΂Ƃ������Ƃł��B
            //
            for (int m = 0; m < taikyoku.GobanBounds.BoardSize; m++)
            {
                for (int n = 0; n < taikyoku.GobanBounds.BoardSize; n++)
                {
                    GobanPoint location = new GobanPointImpl(m, n);
                    if (
                        // ����i�l�ԁj�̐΂��u���Ă���B
                        taikyoku.Goban.At(location) == taikyoku.YourColor
                        &&
                        // ���o�e�B�[�� 3�ȉ��B
                        libertyOfPiece_eachPoint[m, n] < 4
                    )
                    {
                        // ���ꂩ��t���O�𗧂ĂĂ������߂ɃN���A�[���܂��B
                        taikyoku.MarkingBoard.Initmark(taikyoku.GobanBounds.BoardSize);

                        int ct = 0; // �ċA����Ƃ��Ɏg���܂��B�����l 0 ��^���Ă����΍\���܂���B
                        if
                        (
                            Util_FindOpen.FindOpenLocations(trayLocations, location, taikyoku.YourColor, libertyOfPiece_eachPoint[m, n], taikyoku)
                        )
                        {
                            if (libertyOfPiece_eachPoint[m,n] == 1)
                            {
                                // ���o�e�B�[�� 1 �̑���i�l�ԁj�̐΂������܂����B
                                if (out_score < 120) // �]���l�� 120 �����Ȃ�
                                {
                                    // �]���l120�̎w����Ƃ��āA�x�X�g���[�u�̈ʒu���X�V���܂��B
                                    out_score = 120;
                                    out_location.SetLocation(trayLocations[0]);
                                }
                            }
                            else
                            {
                                // ���o�e�B�[�̐��̂Q�d���[�v�ŁA
                                // 0,1 ��A0,2 ��A 1,2 �ȂǁA�قȂ�y�A u,v �����܂��B
                                for (int u = 0; u < libertyOfPiece_eachPoint[m,n]; u++)//�����i�R���s���[�^�[�j�Ƃ����z��
                                {
                                    for (int v = 0; v < libertyOfPiece_eachPoint[m,n]; v++)//����i�l�ԁj�Ƃ����z��
                                    {
                                        if (u != v)
                                        {
                                            int libertyOfPiece_a; // Gnugo1.2 �ł́A�O���[�o���ϐ� lib = 0 �ł����B
                                            Util_CountLiberty.Count(out libertyOfPiece_a, trayLocations[u], taikyoku.MyColor, taikyoku);
                                            if (0 < libertyOfPiece_a)    // �Ó��ȓ���
                                            {
                                                // �R���s���[�^�[�̐F�̐΂��i�����Ɂj�u���܂��B
                                                taikyoku.Goban.Put(trayLocations[u], taikyoku.MyColor);
                                        
                                                // look ahead opponent move
                                                int libertyOfPiece_b; // Gnugo1.2 �ł́A�O���[�o���ϐ� lib = 0 �ł����B
                                                Util_CountLiberty.Count(out libertyOfPiece_b, trayLocations[v], taikyoku.YourColor, taikyoku);
                                                if
                                                (
                                                    1 == libertyOfPiece_a
                                                    &&
                                                    0 < libertyOfPiece_b
                                                )
                                                {
                                                    tryScore = 0;
                                                }
                                                else
                                                {
                                                    tryScore = 120 - 20 * libertyOfPiece_b;
                                                }
                                            
                                                if (out_score < tryScore)
                                                {
                                                    // ���]���l�̍����w����������܂����B�x�X�g���[�u�̈ʒu���X�V���܂��B
                                                    out_score = tryScore;
                                                    out_location.SetLocation(trayLocations[u]);
                                                }

                                                // �i�����ɒu�����j�΂���菜���܂��B�Տ�����ɖ߂������ł��B
                                                taikyoku.Goban.Put(trayLocations[u], StoneColor.Empty);
                                            }
                                        }
                                    }//v
                                }//u
                            }
                        }
                    }
                }//n
            }//m

            if (out_score > 0)   // �w������������B
            {
                return true;
            }
            else    // ���҂̎w�����������̂ɂ͎��s�����B
            {
                return false;
            }
        }
    }
}