/**
 * C# Arrange of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-28
 *  
 * findpatn.c -> Util_FindOpeningZyoseki.cs
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
using Grayscale.GPL.P___405_CompZyoseki.L500_FindPattern;
using Grayscale.GPL.P___409_ComputerB__.L500_Computer;
using Grayscale.GPL.P160____Collection_.L500_Collection;
using Grayscale.GPL.P330____OpenZyoseki.L500_Opening;
using Grayscale.GPL.P405____CompZyoseki.L075_EmptyRectangle;
using Grayscale.GPL.P405____CompZyoseki.L250_FindLocalZyoseki;

namespace Grayscale.GPL.P405____CompZyoseki.L500_FindOpeningZyoseki
{
    /// <summary>
    /// ���Ղ̓��������ƁA�p�^�[����������́A�R���s���[�^�[�̓�����T���܂��B
    /// 
    /// Gnugo1.2 �ł� findpatn�֐��B
    /// </summary>
    public class Util_FindOpeningZyoseki
    {
        /// <summary>
        /// ��΂ɓ����Ă���Ƃ��̕]���l�B
        /// </summary>
        private const int ZYOSEKI_SCORE = 80;

        /// <summary>
        /// ���̓����̂��߂Ƀ}�b�`�����΁i�p�^�[���j��T���܂��B
        /// 
        /// Gnugo1.2 �ł� findpatn�֐��B
        /// </summary>
        /// <param name="out_location">���̓�����   �s�A��ԍ�</param>
        /// <param name="out_score">Gnugo1.2 �ł� val �����B���̎w����̕]���l</param>
        /// <param name="taikyoku"></param>
        /// <returns></returns>
        public static bool FindPattern
        (
            out GobanPoint out_location,
            out int out_score,
            Taikyoku taikyoku
        )
        {
            GobanPoint tryLocation;// Gnugo1.2 �ł́Ati,tj �Ƃ����ϐ���
            int tryScore;// Gnugo1.2 �ł́Atval �Ƃ����ϐ����B

            //
            // �I�[�v�j���O�̒�΂́A�ՖʑS�̂̂��̂ł��B
            //
            // �܂��A���ՂŐ�́i�I�L���p�C�Goccupy�j�ł���p�icorners�j��_���A
            // ���ɁA�J���Ă���l�ӂ�_���Ă����܂��B
            //

            //----------------------------------------
            // ���[4] �Ō�̓����̑���
            //----------------------------------------
            //
            // �����OFF�ł��B���̒�΂������ɐi��ł���Ƃ��A���̃t���O�������Ă��܂��B
            //
            if (taikyoku.OpeningZyosekiFlag[4])
            {
                taikyoku.OpeningZyosekiFlag[4] = false; // ���̒�΂�OFF�ɂ��܂��B

                // �O��̃f�[�^�����o���܂��B
                int cnd = ((ComputerPlayerB)taikyoku.ComputerPlayer).NodeNo;
                MoveType movetype = ((ComputerPlayerB)taikyoku.ComputerPlayer).Movetype;
                if (OpeningZyosekiImpl.Opening(out out_location, ref cnd, movetype, taikyoku))
                {
                    taikyoku.OpeningZyosekiFlag[4] = true; // �����Ɠ����Ȃ�A���̒�΂�ON�Ƀ��Z�b�g���܂��B
                }
                ((ComputerPlayerB)taikyoku.ComputerPlayer).NodeNo = cnd;

                if (taikyoku.Goban.At(out_location) == StoneColor.Empty)  // �u����Ȃ�
                {
                    out_score = Util_FindOpeningZyoseki.ZYOSEKI_SCORE;  // ��΂̕]���l
                    return true;
                }
                else
                {
                    // �΂�u���Ȃ�������A���̒�΂��I���܂��B
                    taikyoku.OpeningZyosekiFlag[4] = false;
                }
            }

            //----------------------------------------
            // ���[0] ���k�̊p
            //----------------------------------------
            if (taikyoku.OpeningZyosekiFlag[0])
            {
                taikyoku.OpeningZyosekiFlag[0] = false; // �t���O���N���A�[�B
                if (Util_EmptyRectangle.IsEmptyRectangle(new GobanPointImpl( 0, 0),new GobanPointImpl( 5, 5),taikyoku))
                {

                    int cnd = 0;
                    MoveType movetype = MoveType.Basic;
                    OpeningZyosekiImpl.Opening(out out_location, ref cnd, movetype, taikyoku);  // ���̎�̂��߂̐V�����m�[�h���擾���܂��B
                    if (OpeningZyosekiImpl.Opening(out out_location, ref cnd, movetype, taikyoku))
                    {
                        taikyoku.OpeningZyosekiFlag[4] = true;
                    }
                    ((ComputerPlayerB)taikyoku.ComputerPlayer).NodeNo = cnd;
                    ((ComputerPlayerB)taikyoku.ComputerPlayer).Movetype = movetype;

                    out_score = Util_FindOpeningZyoseki.ZYOSEKI_SCORE;  // �]���l
                    return true;
                }
            }

            //----------------------------------------
            // ���[1] �쐼�̊p
            //----------------------------------------
            if (taikyoku.OpeningZyosekiFlag[1])
            {
                taikyoku.OpeningZyosekiFlag[1] = false;
                if (Util_EmptyRectangle.IsEmptyRectangle(new GobanPointImpl(13, 0), new GobanPointImpl(18, 5),taikyoku))
                {
                    int cnd = 0;
                    MoveType movetype = MoveType.Inverted;
                    OpeningZyosekiImpl.Opening(out out_location, ref cnd, movetype, taikyoku); // ���̎�̂��߂̐V�����m�[�h���擾���܂��B
                    if (OpeningZyosekiImpl.Opening(out out_location, ref cnd, movetype, taikyoku))
                    {
                        taikyoku.OpeningZyosekiFlag[4] = true;
                    }
                    ((ComputerPlayerB)taikyoku.ComputerPlayer).NodeNo = cnd;
                    ((ComputerPlayerB)taikyoku.ComputerPlayer).Movetype = movetype;

                    out_score = Util_FindOpeningZyoseki.ZYOSEKI_SCORE;  // �]���l
                    return true;
                }
            }

            //----------------------------------------
            // ���[2] �k���̊p
            //----------------------------------------
            if (taikyoku.OpeningZyosekiFlag[2])
            {
                taikyoku.OpeningZyosekiFlag[2] = false;
                if (Util_EmptyRectangle.IsEmptyRectangle(new GobanPointImpl(0, 13), new GobanPointImpl(5, 18),taikyoku))
                {
                    int cnd = 0;
                    MoveType movetype = MoveType.Reflected;
                    OpeningZyosekiImpl.Opening(out out_location, ref cnd, movetype, taikyoku);   // ���̎�̂��߂̐V�����m�[�h���擾���܂��B
                    if (OpeningZyosekiImpl.Opening(out out_location, ref cnd, movetype, taikyoku))
                    {
                        taikyoku.OpeningZyosekiFlag[4] = true;
                    }
                    ((ComputerPlayerB)taikyoku.ComputerPlayer).NodeNo = cnd;
                    ((ComputerPlayerB)taikyoku.ComputerPlayer).Movetype = movetype;

                    out_score = Util_FindOpeningZyoseki.ZYOSEKI_SCORE;  // �]���l
                    return true;
                }
            }

            //----------------------------------------
            // ���[3] �쓌�̊p
            //----------------------------------------
            if (taikyoku.OpeningZyosekiFlag[3])
            {
                taikyoku.OpeningZyosekiFlag[3] = false;
                if (Util_EmptyRectangle.IsEmptyRectangle(new GobanPointImpl(13, 13), new GobanPointImpl(18, 18),taikyoku))
                {
                    int cnd = 0;
                    MoveType movetype = MoveType.Inverted_And_Reflected;
                    OpeningZyosekiImpl.Opening(out out_location, ref cnd, movetype, taikyoku);   // ���̎�̂��߂̐V�����m�[�h���擾���܂��B
                    if (OpeningZyosekiImpl.Opening(out out_location, ref cnd, movetype, taikyoku))
                    {
                        taikyoku.OpeningZyosekiFlag[4] = true;
                    }
                    ((ComputerPlayerB)taikyoku.ComputerPlayer).NodeNo = cnd;
                    ((ComputerPlayerB)taikyoku.ComputerPlayer).Movetype = movetype;

                    out_score = Util_FindOpeningZyoseki.ZYOSEKI_SCORE;  // �]���l
                    return true;
                }
            }

            //
            // �Ӂiedges�j�̃I�L���p�C�ioccupy�j
            //

            //----------------------------------------
            // ���[5] �k��
            //----------------------------------------
            //
            // ��Ԃ̖k���̋�`�̈悪����ۂȂ�A�ł����ޏꏊ�̒�΂��P����܂��B���̒�΂͂��̂P�ӏ������ł��B
            //
            if (taikyoku.OpeningZyosekiFlag[5])
            {
                taikyoku.OpeningZyosekiFlag[5] = false;
                if (Util_EmptyRectangle.IsEmptyRectangle(new GobanPointImpl(0, 6), new GobanPointImpl(4, 11),taikyoku))
                {
                    out_location = new GobanPointImpl(3,9);// ���̎w����̈ʒui,j
                    out_score = Util_FindOpeningZyoseki.ZYOSEKI_SCORE;  // �]���l
                    return true;
                }
            }

            //----------------------------------------
            // ���[6] ���
            //----------------------------------------
            //
            // ��Ԃ̓쑤�̋�`�̈悪����ۂȂ�A�ł����ޏꏊ�̒�΂��P����܂��B���̒�΂͂��̂P�ӏ������ł��B
            //
            if (taikyoku.OpeningZyosekiFlag[6])
            {
                taikyoku.OpeningZyosekiFlag[6] = false;
                if (Util_EmptyRectangle.IsEmptyRectangle(new GobanPointImpl(18, 6), new GobanPointImpl(14, 11),taikyoku))
                {
                    out_location = new GobanPointImpl(15, 9);// ���̎w����̈ʒui,j
                    out_score = Util_FindOpeningZyoseki.ZYOSEKI_SCORE;  // �]���l
                    return true;
                }
            }

            //----------------------------------------
            // ���[7] ����
            //----------------------------------------
            //
            // ��Ԃ̐����̋�`�̈悪����ۂȂ�A�ł����ޏꏊ�̒�΂��P����܂��B���̒�΂͂��̂P�ӏ������ł��B
            //
            if (taikyoku.OpeningZyosekiFlag[7])
            {
                taikyoku.OpeningZyosekiFlag[7] = false;
                if (Util_EmptyRectangle.IsEmptyRectangle(new GobanPointImpl(6, 0), new GobanPointImpl(11, 4), taikyoku))
                {
                    out_location = new GobanPointImpl(9, 3);// ���̎w����̈ʒui,j
                    out_score = Util_FindOpeningZyoseki.ZYOSEKI_SCORE;  // �]���l
                    return true;
                }
            }

            //----------------------------------------
            // ���[8] ����
            //----------------------------------------
            //
            // ��Ԃ̓����̋�`�̈悪����ۂȂ�A�ł����ޏꏊ�̒�΂��P����܂��B���̒�΂͂��̂P�ӏ������ł��B
            //
            if (taikyoku.OpeningZyosekiFlag[8])
            {
                taikyoku.OpeningZyosekiFlag[8] = false;
                if (Util_EmptyRectangle.IsEmptyRectangle(new GobanPointImpl(6, 18), new GobanPointImpl(11, 14), taikyoku))
                {
                    out_location = new GobanPointImpl(9, 15);// ���̎w����̈ʒui,j
                    out_score = Util_FindOpeningZyoseki.ZYOSEKI_SCORE;  // �]���l
                    return true;
                }
            }

            //
            // ���Ւ�΂̂ǂ�ɂ����Ă͂܂�Ȃ���΁B
            //

            out_location = new GobanPointImpl(-1, -1);
            out_score = -1;

            //
            // �ǒn�I�Ȓ�΂�T���܂��B
            //
            for (int m = 0; m < taikyoku.GobanBounds.BoardSize; m++)
            {
                for (int n = 0; n < taikyoku.GobanBounds.BoardSize; n++)
                {
                    GobanPoint mnLocation = new GobanPointImpl(m, n);
                    if
                    (
                        taikyoku.Goban.At(mnLocation) == taikyoku.MyColor // �R���s���[�^�[�̐�
                        &&
                        Util_FindLocalZyoseki.FindPattern(out tryLocation, out tryScore, mnLocation, taikyoku)
                        &&
                        out_score < tryScore   // �]���l���������
                    )
                    {
                        out_score = tryScore;  // �]���l
                        out_location.SetLocation(tryLocation);// ���̎w����̈ʒui,j
                    }
                }
            }

            if (out_score > 0)   // ��΁i�p�^�[���j�������܂����B
            {
                return true;
            }
            else    // ��΂͌�����܂���ł����B
            {
                return false;
            }
        }
    }
}