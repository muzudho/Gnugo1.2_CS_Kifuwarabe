/**
 * C# Arrange of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-28
 *  
 * genmove.c -> Util_BestMove.cs
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
using Grayscale.GPL.P403____CompThink__.L075_OwnEye;
using Grayscale.GPL.P403____CompThink__.L250_SasiteSaver;
using Grayscale.GPL.P403____CompThink__.L500_SasiteWinner;
using Grayscale.GPL.P405____CompZyoseki.L500_FindOpeningZyoseki;
using System;

namespace Grayscale.GPL.P407____CompBestMov.L500_BestMove
{
    /// <summary>
    /// �R���s���[�^�[�̎��̓��������܂��B
    /// </summary>
    public abstract class Util_BestMove
    {
        /// <summary>
        /// �őP���T�����s�ő�񐔁B
        /// </summary>
        public const int MAXTRY = 400;

        /// <summary>
        /// �R���s���[�^�[�̓��������܂��B
        /// 
        /// Gnugo1.2 �ł� genmove�֐��B
        /// </summary>
        /// <param name="out_bestMove">�R���s���[�^�[���I�񂾁A��Ԃ����΂̒u���ꏊ</param>
        /// <param name="taikyoku"></param>
        public static void Generate_BestMove
        (
            out GobanPoint out_bestMove,
            Taikyoku taikyoku
        )
        {
            GobanPoint tryLocation; // Gnugo1.2 �ł́A ti, tj �Ƃ����ϐ����B
            int bestScore;// Gnugo1.2 �ł� val�ϐ��B�]���l�B
            int tryScore; // Gnugo1.2 �ł� tval�ϐ��B�����ĎZ�o�����]���l�B
            int try_ = 0;   // �g���C�̐�

            // ���[�u�ƕ]���l�����������܂��B
            out_bestMove = new GobanPointImpl(-1, -1);
            bestScore = -1;

            // �J�����g�F�̐΂̃��o�e�B�[�i�l���̐΂�u����Ƃ���j
            // 
            // Gnugo1.2 �ł́Al �Ƃ������O�̃O���[�o���ϐ��Bliberty �̗����낤���H
            // eval �œ��e���ݒ肳��A���̓��e�� exambord�Afindsavr�Afindwinr�Asuicide�Ŏg�p����܂��B
            int[,] libertyOfPiece_eachPoint;

            // ����i�l�ԁj�̂��ꂼ��̃s�[�X�̃��o�e�B�[���Ăѐ����܂��B
            Util_CountLibertyAll.Count_LibertyOfPiece_EachPoint(out libertyOfPiece_eachPoint, taikyoku.YourColor, taikyoku);

            // ����̃s�[�X���������A�U�߂��肷����T���܂��B
            if (Util_FindWinner.FindSasite(out tryLocation, out tryScore, libertyOfPiece_eachPoint, taikyoku))
            {
                if (bestScore < tryScore) // �V�����őP����������Ȃ�
                {
                    bestScore = tryScore; // �őP�̕]���l�ƒu���ꏊ���A�X�V���܂��B
                    out_bestMove.SetLocation(tryLocation);
                }
            }

            // ������������Ă���΁A����̃s�[�X�����܂��B
            if (Util_SasiteSaver.FindSasite(out tryLocation, out tryScore, libertyOfPiece_eachPoint, taikyoku))
            {
                if (bestScore < tryScore) // �V�����őP����������Ȃ�
                {
                    bestScore = tryScore; // �őP�̕]���l�ƒu���ꏊ���A�X�V���܂��B
                    out_bestMove.SetLocation(tryLocation);
                }
            }

            // �V���������̂��߂̃��[�J���E�v���[�E�p�^�[���Ɉ�v���邩�����܂��B
            if (Util_FindOpeningZyoseki.FindPattern(out tryLocation, out tryScore, taikyoku))
            {
                if (bestScore < tryScore) // �V�����őP����������Ȃ�
                {
                    bestScore = tryScore; // �őP�̕]���l�ƒu���ꏊ���A�X�V���܂��B
                    out_bestMove.SetLocation(tryLocation);
                }
            }

            // �����肪������΁A�����_���ɑł��܂��B
            if (bestScore < 0)
            {
                int count_libertyOfPiece;// Gnugo1.2 �ł́A�ÓI�O���[�o���ϐ� lib �ł����B

                // ����Ԃ��B
                do
                {
                    // �s�� �Օ��Ń����_���B
                    out_bestMove.I = taikyoku.Random.Next() % taikyoku.GobanBounds.BoardSize;

                    // �Ⴂ���C���������A�����̗̈�
                    if
                    (
                        out_bestMove.OutOfI(2, 16)//0�`1�s�A�܂��� 17�`18�s���B
                        ||
                        out_bestMove.ContainsI(6, 12)//6�`12�s�̂ǂ����ɂ���B
                    )
                    {
                        // �U��Ȃ���
                        out_bestMove.I = taikyoku.Random.Next() % taikyoku.GobanBounds.BoardSize;
                        if (out_bestMove.OutOfI(2, 16))//0�`1�s�A�܂��� 17�`18�s���B
                        {
                            // �U��Ȃ���
                            out_bestMove.I = taikyoku.Random.Next() % taikyoku.GobanBounds.BoardSize;
                        }
                    }

                    // ��� �Օ� �Ń����_���B
                    out_bestMove.J = taikyoku.Random.Next() % taikyoku.GobanBounds.BoardSize;

                    // �Ⴂ���C���������A�����̗̈�
                    if
                    (
                        out_bestMove.OutOfJ(2,16)//0�`1��A�܂��� 17�`18�񂾁B
                        || // �܂��́A
                        out_bestMove.ContainsJ(6,12)//6�`12��̂ǂ����ɂ���B
                    )
                    {
                        // �U��Ȃ���
                        out_bestMove.J = taikyoku.Random.Next() % taikyoku.GobanBounds.BoardSize;

                        if (out_bestMove.OutOfJ(2, 16))//0�`1��A�܂��� 17�`18�񂾁B
                        {
                            // �U��Ȃ���
                            out_bestMove.J = taikyoku.Random.Next() % taikyoku.GobanBounds.BoardSize;
                        }
                    }

                    // ���o�e�B�[�𐔂��Ȃ����B
                    Util_CountLiberty.Count_LibertyOfPiece(out count_libertyOfPiece, out_bestMove, taikyoku.MyColor, taikyoku);
                    ++try_;
                }
                while
                (
                    try_ < MAXTRY // �܂������g���C�ł��܂��B
                    &&
                    (
                        // ���̂R�̏����̂ǂꂩ�𖞂����悤�Ȃ�A�ăg���C���܂��B
                        // �i�P�j�x�X�g���[�u������ۂ��B�񍇖@�肩���B
                        // �i�Q�j�s�[�X�̃��o�e�B�[�� 0�`1 �����Ȃ��B
                        // �i�R�j�����̖ڂ𖄂߂��Ȃ�B
                        taikyoku.Goban.LookColor(out_bestMove) != StoneColor.Empty
                        ||
                        count_libertyOfPiece < 2
                        ||
                        Util_OwnEye.IsOwnEye(out_bestMove, taikyoku)
                    )
                );
            }

            if (MAXTRY <= try_)  // �ő厎�s�񐔂𒴂��Ă�����A�R���s���[�^�[�̓p�X�B
            {
                taikyoku.Pass++;

                //----------------------------------------
                // �R���s���[�^�[�̎w�����\�����܂��B
                //----------------------------------------
                Console.WriteLine("I pass.");
                out_bestMove.SetPass();
            }
            else   // �Ó��Ȏw������w���܂��B
            {
                taikyoku.Pass = 0;

                //----------------------------------------
                // �R���s���[�^�[�̎w�����\�����܂��B
                //----------------------------------------
                Console.Write("my move: ");
                // ���W�̗�ԍ����A�A�X�L�[�����R�[�h�ɕϊ����܂��B
                char a;
                if (out_bestMove.J < 8)
                {
                    a = (char)(out_bestMove.J + 65);
                }
                else
                {
                    a = (char)(out_bestMove.J + 66);
                }            
                Console.Write(a);

                int ii; // Gnugo1.2 �ł́A�si �𔽓]�������ϐ����� ii�B
                ii = taikyoku.GobanBounds.BoardSize - out_bestMove.I;
                if (ii < 10)
                {
                    Console.WriteLine("{0,1}", ii);
                }
                else
                {
                    Console.WriteLine("{0,2}", ii);
                }
            }
        }
    }
}