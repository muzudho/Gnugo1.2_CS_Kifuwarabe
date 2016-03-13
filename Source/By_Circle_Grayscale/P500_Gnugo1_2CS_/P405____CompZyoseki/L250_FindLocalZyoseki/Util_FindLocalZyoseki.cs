/**
 * C# Arrange of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-28
 *  
 * matchpat.c -> Util_FindLocalZyoseki.cs
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
using Grayscale.GPL.P___160_Collection_.L250_Rectangle;
using Grayscale.GPL.P___160_Collection_.L500_Collection;
using Grayscale.GPL.P___190_Board______.L063_Word;
using Grayscale.GPL.P___300_Taikyoku___.L500_Taikyoku;
using Grayscale.GPL.P___405_CompZyoseki.L125_LocalZyoseki;
using Grayscale.GPL.P160____Collection_.L500_Collection;
using Grayscale.GPL.P190____Board______.L500_Util;
using Grayscale.GPL.P340____Liberty____.L250_Liberty;
using Grayscale.GPL.P405____CompZyoseki.L125_LocalZyoseki;

namespace Grayscale.GPL.P405____CompZyoseki.L250_FindLocalZyoseki
{
    /// <summary>
    /// �}�b�`�p�^�[���̓����B
    /// 
    /// Gnugo1.2 �ł� matchpat�֐��B
    /// </summary>
    public abstract class Util_FindLocalZyoseki
    {
        /// <summary>
        /// ��Βl�B
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int Abs(int x)
        {
            return x < 0 ? -x : x;
        }

        /// <summary>
        /// ��������ǂꂾ������Ă��邩�B
        /// 
        /// Gnugo1.2 �ł� line �֐��B
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int DistanceFromCenter(int x, GobanRectangleA gobanBounds )
        {
            return Util_FindLocalZyoseki.Abs(x - gobanBounds.BoardCenter);
        }


        /// <summary>
        /// ���]���]�ȂǁA�Տ�����������ς��鑀��̂��߁itransfomation�j�̍s�񂽂��B[8][2][2]
        /// 
        /// 1�A-1�A0 �����Ȃ̂œ��{�B
        /// 
        /// �s�񎮂�
        /// ��     ��
        /// �� 0 -1��
        /// �� 1  0��
        /// ��     ��
        /// �̂悤�ȏ����������邪�A�v���O������ł͌��ɂ����B����A�����A�E��A�E���̏��ɕ���ł�����̂Ƃ���B
        /// 
        /// </summary>
        public static readonly int[,,] Trf =
        {
            // x         y     ���͑�
            // x,  y     x   y ��Α�
            {{ 1,  0}, { 0,  1}},   // [0]���͂��ꂽ���W�A���̂܂�܂ł��B  �ilinear transfomation matrix�j
            // ��     ��
            // �� 1  0��
            // �� 0  1��
            // ��     ��

            {{ 1,  0}, { 0, -1}},   // [1]�㉺���]                      �iinvert�j
            // ��     ��
            // �� 1  0��
            // �� 0 -1��
            // ��     ��

            {{ 0,  1}, {-1,  0}},   // [2]�����v����90�x��]            �irotate 90�j
            // ��     ��
            // �� 0 -1��
            // �� 1  0��
            // ��     ��

            {{ 0, -1}, {-1,  0}},   // [3]�����v����90�x��]���ď㉺���]  �irotate 90 and invert�j
            // ��     ��
            // �� 0 -1��
            // ��-1  0��
            // ��     ��

            {{-1,  0}, { 0,  1}},   // [4]���E���]                     �iflip left�j
            // ��     ��
            // ��-1  0��
            // �� 0  1��
            // ��     ��

            {{-1,  0}, { 0, -1}},   // [5]���E���]���ď㉺���]            �iflip left and invert�j
            // ��     ��
            // ��-1  0��
            // �� 0 -1��
            // ��     ��

            {{ 0,  1}, { 1,  0}},   // [6]�����v����90�x��]���č��E���]  �irotate 90 and flip left�j
            // ��     ��
            // �� 0  1��
            // �� 1  0��
            // ��     ��

            {{ 0, -1}, { 1,  0}}    // [7]�����v����90�x��]���č��E���]���ď㉺���]  �irotate 90, flip left and invert�j
            //                          �i�܂莞�v����90�x��]�j
            // ��     ��
            // �� 0  1��
            // ��-1  0��
            // ��     ��
        };

        /// <summary>
        /// ���Ւ�ΈȊO�́A�ǒn�I�Ȓ�΂ɂ��Ă͂܂邩������ׁA��΂�����Βu���ꏊ���擾���܂��B
        /// </summary>
        /// <param name="out_location">���̎�� �s�A��ԍ�</param>
        /// <param name="out_score">Gnugo1.2 �ł� val �����B�]���l</param>
        /// <param name="origin_location">Gnugo1.2 �ł́A ���_��   �s m�A�� n �����B</param>
        /// <param name="taikyoku"></param>
        /// <returns></returns>
        public static bool FindPattern
        (
            out GobanPoint out_location,
            out int out_score,
            GobanPoint origin_location,
            Taikyoku taikyoku
        )
        {
            GobanPoint tryLocation = new GobanPointImpl(0, 0); // Gnugo1.2 �ł́Ati,tj �Ƃ����ϐ����B// 2015-11-26 �ǉ� 0,0 �ŏ������B

            out_location = new GobanPointImpl(-1,-1);
            out_score = -1;

            for (int ptnNo = 0; ptnNo < Util_LocalZyoseki.Patterns.Length; ptnNo++) // ���ꂼ��̃p�^�[���������܂��B
            {
                for (int ll = 0; ll < Util_LocalZyoseki.Patterns[ptnNo].LastTrfNo; ll++)/* try each orientation transformation */
                {
                    int k = 0;
                    bool isContinueLoop = true; // Gnugo1.2 �ł� cont�ϐ��Bcontinue�̗����H

                    //
                    // �����Ɉ�v���Ă��Ȃ����̂��A�ӂ邢���Ƃ��Ă����܂��B
                    // �����Ɉ�v���Ă�����̂́A�ǂ�ǂ񎟂̃��[�v�ɐi�݂܂��B
                    //
                    while (
                        k != Util_LocalZyoseki.Patterns[ptnNo].Stones.Length
                        &&
                        isContinueLoop
                        )/* �������̃|�C���g�Ɉ�v */
                    {
                        //
                        // �ό`�itransform�j���āA�Տ�̍��W�ɕϊ����܂��B
                        //
                        int nx = 
                            origin_location.J
                            +
                            Util_FindLocalZyoseki.Trf[ll, 0, 0] * Util_LocalZyoseki.Patterns[ptnNo].Stones[k].P.X
                            +
                            Util_FindLocalZyoseki.Trf[ll, 0, 1] * Util_LocalZyoseki.Patterns[ptnNo].Stones[k].P.Y
                            ;

                        int my = 
                            origin_location.I
                            +
                            Util_FindLocalZyoseki.Trf[ll, 1, 0] * Util_LocalZyoseki.Patterns[ptnNo].Stones[k].P.X
                            +
                            Util_FindLocalZyoseki.Trf[ll, 1, 1] * Util_LocalZyoseki.Patterns[ptnNo].Stones[k].P.Y
                            ;

                        /* outside the board */
                        if (
                            !Util_AboutBoard.In_Board(my,taikyoku.GobanBounds)
                            ||
                            !Util_AboutBoard.In_Board(nx, taikyoku.GobanBounds)
                        )
                        {
                            isContinueLoop = false;
                            break;
                        }

                        GobanPoint mnLocation = new GobanPointImpl(my, nx);
                        switch (Util_LocalZyoseki.Patterns[ptnNo].Stones[k].Att)
                        {
                            case LocalZyoseki_StoneAttribute._0_Empty :
                                if (taikyoku.Goban.At(mnLocation) == StoneColor.Empty)    /* open */
                                {
                                    break;
                                }
                                else
                                {
                                    isContinueLoop = false;
                                    break;
                                }
                            case LocalZyoseki_StoneAttribute._1_YourPiece:
                                if (taikyoku.Goban.At(mnLocation) == taikyoku.YourColor)  /* your piece */
                                {
                                    break;
                                }
                                else
                                {
                                    isContinueLoop = false;
                                    break;
                                }
                            case LocalZyoseki_StoneAttribute._2_MyPiece:
                                if (taikyoku.Goban.At(mnLocation) == taikyoku.MyColor)  /* my piece */
                                {
                                    break;
                                }
                                else
                                {
                                    isContinueLoop = false;
                                    break;
                                }
                            case LocalZyoseki_StoneAttribute._3_MyNextMove:
                                if (taikyoku.Goban.At(mnLocation) == StoneColor.Empty)    /* open for new move */
                                {
                                    int libertyOfPiece; // Gnugo1.2 �ł́A�O���[�o���ϐ� lib = 0 �ł����B
                                    Util_CountLiberty.Count(out libertyOfPiece, mnLocation, taikyoku.MyColor, taikyoku);    /* check liberty */
                                    if (1 < libertyOfPiece)  /* move o.k. */
                                    {
                                        tryLocation.SetLocation(my,nx);
                                        break;
                                    }
                                    else
                                    {
                                        isContinueLoop = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    isContinueLoop = false;
                                    break;
                                }
                            case LocalZyoseki_StoneAttribute._4_EmptyOnEdge:
                                if
                                (
                                    taikyoku.Goban.At(mnLocation) == StoneColor.Empty  /* open on edge */
                                    &&
                                    (
                                        Util_AboutBoard.On_Edge(my, taikyoku.GobanBounds) || Util_AboutBoard.On_Edge(nx, taikyoku.GobanBounds)
                                    )
                                )
                                {
                                    break;
                                }
                                else
                                {
                                    isContinueLoop = false;
                                    break;
                                }
                            case LocalZyoseki_StoneAttribute._5_YourPieceOnEdge:
                                if
                                (
                                    taikyoku.Goban.At(mnLocation) == taikyoku.YourColor  /* your piece on edge */
                                    &&
                                    (
                                        Util_AboutBoard.On_Edge(my, taikyoku.GobanBounds) || Util_AboutBoard.On_Edge(nx, taikyoku.GobanBounds)
                                    )
                                )
                                {
                                    break;
                                }
                                else
                                {
                                    isContinueLoop = false;
                                    break;
                                }
                            case LocalZyoseki_StoneAttribute._6_MyPieceOnEdge:
                                if
                                (
                                    taikyoku.Goban.At(mnLocation) == taikyoku.MyColor  /* my piece on edge */
                                    &&
                                    (
                                        Util_AboutBoard.On_Edge(my, taikyoku.GobanBounds) || Util_AboutBoard.On_Edge(nx, taikyoku.GobanBounds)
                                    )
                                )
                                {
                                    break;
                                }
                                else
                                {
                                    isContinueLoop = false;
                                    break;
                                }
                        }
                        ++k;
                    }
            
                    if (isContinueLoop) // �p�^�[���Ɉ�v���Ă��܂��B
                    {
                        int tryScore = Util_LocalZyoseki.Patterns[ptnNo].Score; // Gnugo1.2 �ł́Atval �ϐ��B


                        if (
                            // �p�^�[���ԍ� 8�`13 �ɂ́A��V�ƃy�i���e�B�[��t���܂��B
                            8 <= ptnNo && ptnNo <= 13    /* patterns for expand region */
                           )
                        {
                            if (7 < Util_FindLocalZyoseki.DistanceFromCenter(tryLocation.I, taikyoku.GobanBounds))
                            {
                                // ���S���� 8�ȏ㗣��Ă���i�[�ƁA���ׂ̗��炢�̈ʒu�j�Ȃ�΁A�y�i���e�B�[��t���܂��B
                                tryScore--;
                            }
                            else if ((Util_FindLocalZyoseki.DistanceFromCenter(tryLocation.I, taikyoku.GobanBounds) == 6) || (Util_FindLocalZyoseki.DistanceFromCenter(tryLocation.I, taikyoku.GobanBounds) == 7))
                            {
                                // ���S���� 6�`7 ����Ă���ʒu�i�[����3,4�Ԗڂ��炢�j�ɂ́A��V��^���܂��B
                                tryScore++;
                            }

                            if (7 < Util_FindLocalZyoseki.DistanceFromCenter(tryLocation.J, taikyoku.GobanBounds))  /* penalty on line 1, 2 */
                            {
                                tryScore--;
                            }
                            else if ((Util_FindLocalZyoseki.DistanceFromCenter(tryLocation.J, taikyoku.GobanBounds) == 6) || (Util_FindLocalZyoseki.DistanceFromCenter(tryLocation.J, taikyoku.GobanBounds) == 7))
                            {
                                tryScore++;    /* reward on line 3, 4 */
                            }
                        }
                
                        if (tryScore > out_score)
                        {
                            out_score = tryScore;
                            out_location.SetLocation(tryLocation);
                        }
                    }
                }//for
            }

            if (0 < out_score)   // �p�^�[���Ƀ}�b�`���܂����B
            {
                return true;
            }
            else    // �}�b�`�Ɏ��s���܂����B
            {
                return false;
            }
        }
    }
}