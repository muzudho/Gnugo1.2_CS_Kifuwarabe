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
        /// <param name="out_bestLocation">���̓�����   �s�A��ԍ�</param>
        /// <param name="out_bestScore">Gnugo1.2 �ł� *val �����B�]���l</param>
        /// <param name="libertyOfNodes">
        /// �J�����g�F�̐΂̃��o�e�B�[�i�l���̐΂�u����Ƃ���j
        /// 
        /// Gnugo1.2 �ł́Al �Ƃ������O�̃O���[�o���ϐ��Bliberty �̗����낤���H
        /// eval �œ��e���ݒ肳��A���̓��e�� exambord�Afindsavr�Afindwinr�Asuicide�Ŏg�p����܂��B
        /// </param>
        /// <param name="taikyoku"></param>
        /// <returns></returns>
        public static bool FindBestLocation
        (
            out GobanPoint out_bestLocation,
            out int out_bestScore,
            int[,] libertyOfNodes,
            Taikyoku taikyoku
        )
        {
            int banSize = taikyoku.GobanBounds.BoardSize;

            // �v�f�� 3 �ȉ��̃��X�g�B
            List<GobanPoint> adj3Locations = new List<GobanPoint>(3);// Gnugo1.2 �ł́A���ꂼ��v�f��[3]�� ti�z��Atj�z��B

            int tryScore;// Gnugo1.2 �ł� tval �ϐ��B

            out_bestLocation = new GobanPointImpl(-1, -1);// �ʒui,j
            out_bestScore = -1;  // �]���l

            //
            // ���o�e�B�[�i�l���̒u����Ƃ���j���R�ȉ��̑���i�l�ԁj�̐΂�T���܂��B
            // �܂�A�Ȃ����Ă���΁i�F���قȂ�΂ƂȂ����Ă���ꍇ������j���A�[�ɂ���΂Ƃ������Ƃł��B
            //
            for (int m = 0; m < banSize; m++)
            {
                for (int n = 0; n < banSize; n++)
                {
                    GobanPoint iLocation = new GobanPointImpl(m, n);

                    if (
                        // ����i�l�ԁj�̐΂ŁA
                        taikyoku.Goban.At(iLocation) == taikyoku.YourColor
                        &&
                        // ���̐΁i���邢�͘A�S�́j�ŁA�ċz�_���R�ȉ��̂��̂�I�т܂��B
                        // ���Ȃ��Ƃ��@�΂��g�ɂȂ����Ă���΂ł��邱�Ƃ�z�肵�Ă��܂��B
                        libertyOfNodes[m, n] < 4
                    )
                    {
                        // ���ꂩ��t���O�𗧂ĂĂ������߂ɃN���A�[���܂��B
                        taikyoku.MarkingBoard.Initmark(taikyoku.GobanBounds.BoardSize);

                        if
                        (
                            // ���̐΁i�A�ł͂Ȃ��j�̊J���Ă�������i�P�����`�R��������͂��ł��j
                            Util_FindOpen.FindOpen3Locations(adj3Locations, iLocation, taikyoku.YourColor, libertyOfNodes[m, n], taikyoku)
                        )
                        {
                            if (libertyOfNodes[m,n] == 1)
                            {
                                // �A�^���̏�ԂȂ̂ŁA�ϋɓI�ɑ_���Ă����܂��B�i�ċz�_���A�ǂ����P���������Ȃ���ԁj

                                if (out_bestScore < 120) // �]���l�� 120 �����Ȃ�
                                {
                                    // �A�^���̕]���l�� 120 �_�͂���܂��B
                                    // ���̈ʒu�̕]�����グ�A�x�X�g���[�u�Ƃ��čX�V���܂��B
                                    out_bestScore = 120;
                                    out_bestLocation.SetLocation(adj3Locations[0]);
                                }
                            }
                            else
                            {
                                // �A�^���ł͂Ȃ��Ƃ��B

                                // �ċz�_�̐��i�Q�`��������j�ɉ����āB
                                int opens = libertyOfNodes[m, n];

                                // �킽���i�R���s���[�^�[�j���u�����Ƃ��ƁA����i�l�ԁj�ɒu���Ԃ��ꂽ�Ƃ���
                                // �S�p�^�[���ɂ���

                                // �z��̃C���f�b�N�X�� 0,1 ��A0,2 ��A 1,2 �ȂǁA�قȂ�y�A com,man �ɂȂ����
                                // �S�Ăɂ��āB
                                for (int iCom = 0; iCom < opens; iCom++)//�킽���i�R���s���[�^�[�j�Ƃ����z��
                                {
                                    for (int iMan = 0; iMan < opens; iMan++)//����i�l�ԁj�Ƃ����z��
                                    {
                                        if (iCom != iMan)
                                        {
                                            // �u���ʒu
                                            GobanPoint adjLocation_com = adj3Locations[iCom];
                                            // �u���Ԃ��ʒu
                                            GobanPoint adjLocation_man = adj3Locations[iMan];

                                            // �킽���́i�A�܂��́j�΂̃��o�e�B
                                            int liberty_com; // Gnugo1.2 �ł́A�O���[�o���ϐ� lib = 0 �ł����B
                                            Util_CountLiberty.Count(out liberty_com, adjLocation_com, taikyoku.MyColor, taikyoku);

                                            if (0 < liberty_com)    // �Ó����`�F�b�N
                                            {
                                                // �R���s���[�^�[�̐F�̐΂��@�ʒu a �Ɂi�����Ɂj�u���܂��B
                                                taikyoku.Goban.Put(adjLocation_com, taikyoku.MyColor);
                                        
                                                // look ahead opponent move
                                                int liberty_man; // Gnugo1.2 �ł́A�O���[�o���ϐ� lib = 0 �ł����B
                                                Util_CountLiberty.Count(out liberty_man, adjLocation_man, taikyoku.YourColor, taikyoku);
                                                if
                                                (
                                                    1 == liberty_com  // �אڂ��鎄�i�R���s���[�^�[�j���́i�A�܂��́j�΂̌ċz�_�͂P�B
                                                    &&
                                                    0 < liberty_man   // �אڂ��邠�Ȃ��i�l�ԁj���́i�A�܂��́j�΂̌ċz�_�͂P�ȏ�B
                                                )
                                                {
                                                    // �l�ԑ��̌ċz�_�̕����A�R���s���[�^�[���Ɠ����A���邢�͑����̂ŁA
                                                    // �ʒu a �ɒu�����l�Ȃ��B
                                                    tryScore = 0;
                                                }
                                                else
                                                {
                                                    // �R���s���[�^�[���u��������A
                                                    // �l�Ԃ��u����ɁA�ċz�_�������A�܂������肪�Ȃ��ꍇ�A�u�����l����B
                                                    tryScore = 120 - 20 * liberty_man;
                                                }
                                            
                                                if (out_bestScore < tryScore)
                                                {
                                                    // ���]���l�̍����w����������܂����B�x�X�g���[�u�̈ʒu���X�V���܂��B
                                                    out_bestScore = tryScore;
                                                    out_bestLocation.SetLocation(adjLocation_com);
                                                }

                                                // �i�����ɒu�����j�΂���菜���܂��B�Տ�����ɖ߂������ł��B
                                                taikyoku.Goban.Put(adjLocation_com, StoneColor.Empty);
                                            }
                                        }
                                    }//v
                                }//u
                            }
                        }
                    }
                }//n
            }//m

            if (0 < out_bestScore)   // �w������������B
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