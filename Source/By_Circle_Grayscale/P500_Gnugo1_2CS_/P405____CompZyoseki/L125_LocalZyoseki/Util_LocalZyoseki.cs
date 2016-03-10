/**
 * C# Arrange of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-28
 *  
 * patterns.h -> Util_LocalZyoseki.cs
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
using Grayscale.GPL.P___405_CompZyoseki.L125_LocalZyoseki;
using Grayscale.GPL.P160____Collection_.L500_Collection;

namespace Grayscale.GPL.P405____CompZyoseki.L125_LocalZyoseki
{
    /// <summary>
    /// �����iorientation�j�����邭��񂷂Ȃǂ́A�s��v�Z�itransformation�j�����x���������߂� x,y ���W�ƁA�����B
    /// 
    /// Gnugo1.2 �ł́A patval �\���́B
    /// </summary>
    public class ZyosekiStoneImpl
    {
        /// <summary>
        /// �s��v�Z�p�̍��W
        /// 
        /// Gnugo1.2 �ł́A�P�ɍ\���̂� x,y �Ƃ��������o�[�E�v���p�e�B�[�B
        /// </summary>
        public GyoretuPoint P{get;set;}

        /// <summary>
        /// �����B
        /// att = 0 - empty,
        ///       1 - your piece,
        ///       2 - my piece,
        ///       3 - my next move
        ///       4 - empty on edge,
        ///       5 - your piece on edge,
        ///       6 - my piece on edge
        /// </summary>
        public LocalZyoseki_StoneAttribute Att { get; set; }

        public ZyosekiStoneImpl(
            GyoretuPoint p,
            LocalZyoseki_StoneAttribute att
            )
        {
            this.P = p;
            this.Att = att;
        }
    }

    /// <summary>
    /// �ǒn�I�Ȓ�΁B
    /// 
    /// Gnugo1.2 �ł́A pattern �ÓI�\���́B
    /// </summary>
    public class LocalZyosekiImpl
    {
        /// <summary>
        /// ��΂̒��̐΁B
        /// 
        /// Gnugo1.2 �ł́Apatn �Ƃ����ϐ����ŁA�z��̃T�C�Y�� MAXPC�萔�� 16 �Ǝw�肳��Ă����B
        /// </summary>
        public ZyosekiStoneImpl[] Stones{get;set;}
        
        /// <summary>
        /// �g�p����s��ϊ��̍Ō�̔ԍ�
        /// 
        /// ���E�Ώې}�`�ɂȂ��Ă��āA���E���]���Ȃ��Ă�����΂� 4 ���A
        /// ����ȊO�� 8 ���w�肵�Ă��������B
        /// 
        /// Gnugo1.2 �ł́Atrfno �Ƃ����ϐ����B
        /// </summary>
        public int LastTrfNo{get;set;}

        /// <summary>
        /// �p�^�[���̕]���l
        /// 
        /// Gnugo1.2 �ł́Apatwt �Ƃ����ϐ����B
        /// </summary>
        public int Score { get; set; }

        public LocalZyosekiImpl(
            ZyosekiStoneImpl[] stones,
            int lastTrfNo,
            int score
            )
        {
            this.Stones = stones;
            this.LastTrfNo = lastTrfNo;
            this.Score = score;
        }
    }

    public class Util_LocalZyoseki
    {
        /// <summary>
        /// 0 �` 24 �̃p�^�[�������邪�A22 �͌��ԁB
        /// [PATNO]
        /// 
        /// Gnugo1.2 �ł� pat �Ƃ������O�̐ÓI�\���̔z��Bpattern�\���̂�v�f�Ƃ��Ď����Ă����B
        /// �܂��A PATNO = 24 �Ƃ����v�f���T�C�Y��\���萔��p�ӂ��Ă������AC# �ł͔z��̗v�f���� �z�񂪎����Ă���̂ŁA�s�v�ɂȂ����B
        /// �܂��Apattern 24 �̗v�f���� 10�Ǝw�肳��Ă������A�ԈႢ�Ǝv��ꂽ�̂� 8 �ɏC�������B
        /// </summary>
        public static LocalZyosekiImpl[] Patterns = new LocalZyosekiImpl[]
        {
            //   pattern 0: 232   connect if invaded
            //      010
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 0), LocalZyoseki_StoneAttribute._2_MyPiece),// �ŏ��̗v�f�͕K���A�N�_�ƂȂ�΁i0,0�j�B�[�ɂ��邩�ǂ����̈Ⴂ�͂���B
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 0), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 1), LocalZyoseki_StoneAttribute._1_YourPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 1), LocalZyoseki_StoneAttribute._0_Empty)
                },
                4,// ���E�Ώ̂̒�΂�4�A����ȊO�� 8�B
                82// ���̒�΂̕]���l�B
            ),
            // pattern 1: 230   connect if invaded
            //    012
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 0), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 1), LocalZyoseki_StoneAttribute._1_YourPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 1), LocalZyoseki_StoneAttribute._2_MyPiece)
                },
                8,
                82
            ),
            // pattern 2: 212   try to attack
            //    030
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 0), LocalZyoseki_StoneAttribute._1_YourPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 1), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 1), LocalZyoseki_StoneAttribute._0_Empty)
                },
                4,
                82
            ),
            // pattern 3: 2302   connect if invaded
            //    0100
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 0), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 1), LocalZyoseki_StoneAttribute._1_YourPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(3, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(3, 1), LocalZyoseki_StoneAttribute._0_Empty)
                },
                8,
                83
            ),
            // pattern 4: 20302   connect if invaded
            // 00100
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 0), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 1), LocalZyoseki_StoneAttribute._1_YourPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(3, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(3, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(4, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(4, 1), LocalZyoseki_StoneAttribute._0_Empty)
                },
                4,
                84
            ),
            // pattern 5: 203   form eye to protect
            //    021
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 1), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 0), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 1), LocalZyoseki_StoneAttribute._1_YourPiece)
                },
                8,
                82
            ),
            // pattern 6: 202    form eye to protect
            // 031
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 1), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 1), LocalZyoseki_StoneAttribute._1_YourPiece)
                },
                8,
                82
            ),
            // pattern 7: 230   connect if invaded
            //    102
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 1), LocalZyoseki_StoneAttribute._1_YourPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 0), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 1), LocalZyoseki_StoneAttribute._2_MyPiece)
                },
                8,
                82
            ),
            // pattern 8: 200000
            //  00030  extend
            //  00000
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(3, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(4, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(5, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(3, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(4, 1), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(5, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 2), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 2), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(3, 2), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(4, 2), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(5, 2), LocalZyoseki_StoneAttribute._0_Empty)
                },
                8,
                80
            ),
            //pattern 9:  2
            //000
            //000  extend
            //000
            //030
            //000
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(-1, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 1, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(-1, 2), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 2), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 1, 2), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(-1, 3), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 3), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 1, 3), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(-1, 4), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 4), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 1, 4), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(-1, 5), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 5), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 1, 5), LocalZyoseki_StoneAttribute._0_Empty)
                },
                4,
                80
            ),
            //pattern 10: 20000
            //0030  extend
            //0000
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(3, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(4, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(3, 1), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(4, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 2), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 2), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(3, 2), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(4, 2), LocalZyoseki_StoneAttribute._0_Empty)
                },
                8,
                79
            ),
            //pattern 11:    2
            //        000
            //        000  extend
            //        030
            //        000
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(-1, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 1, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(-1, 2), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 2), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 1, 2), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(-1, 3), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 3), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 1, 3), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(-1, 4), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 4), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 1, 4), LocalZyoseki_StoneAttribute._0_Empty)
                },
                4,
                79
            ),
            //pattern 12: 2000
            //     030  extend
            //     000
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(3, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 1), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(3, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 2), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 2), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(3, 2), LocalZyoseki_StoneAttribute._0_Empty)
                },
                8,
                76
            ),
            //pattern 13:    2
            //        000  extend
            //        030
            //        000
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(-1, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 1, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(-1, 2), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 2), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 1, 2), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(-1, 3), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 3), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 1, 3), LocalZyoseki_StoneAttribute._0_Empty)
                },
                 4,
                76
            ),
            //pattern 14: 643   form eye on the edge
            //     20
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 0), LocalZyoseki_StoneAttribute._6_MyPieceOnEdge),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 0), LocalZyoseki_StoneAttribute._4_EmptyOnEdge),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 1), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 0), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 1), LocalZyoseki_StoneAttribute._0_Empty)
                },
                 8,
                80
            ),
            //pattern 15: 646    solidify eye on the edge
            //        231
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 0), LocalZyoseki_StoneAttribute._6_MyPieceOnEdge),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 0), LocalZyoseki_StoneAttribute._4_EmptyOnEdge),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 1), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 0), LocalZyoseki_StoneAttribute._6_MyPieceOnEdge),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 1), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(3, 1), LocalZyoseki_StoneAttribute._1_YourPiece)
                },
                 8,
                75
            ),
            //pattern 16: 646    solidify eye on the edge
            //     230
            //      1
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 0), LocalZyoseki_StoneAttribute._6_MyPieceOnEdge),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 0), LocalZyoseki_StoneAttribute._4_EmptyOnEdge),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 1), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 0), LocalZyoseki_StoneAttribute._6_MyPieceOnEdge),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 1), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 2), LocalZyoseki_StoneAttribute._1_YourPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(3, 1), LocalZyoseki_StoneAttribute._0_Empty)
                },
                 8,
                75
            ),
            //pattern 17: 646    solidify eye on the edge
            //     230
            //      0
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 0), LocalZyoseki_StoneAttribute._6_MyPieceOnEdge),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 0), LocalZyoseki_StoneAttribute._4_EmptyOnEdge),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 1), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 0), LocalZyoseki_StoneAttribute._6_MyPieceOnEdge),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 1), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 2), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(3, 1), LocalZyoseki_StoneAttribute._0_Empty)
                },
                 8,
                75
            ),
            //pattern 18:    2       form eye on center
            //        202
            //     3
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(-1, 1), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 1, 1), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 2), LocalZyoseki_StoneAttribute._3_MyNextMove)
                },
                 4,
                80
            ),
            //pattern 19:    2       solidify eye on center
            //        202
            //     231
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(-1, 1), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 1, 1), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 2), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 1, 2), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 2, 2), LocalZyoseki_StoneAttribute._1_YourPiece)
                },
                 8,
                75
            ),
            //pattern 20:    2       solidify eye on center
            //        202
            //     230
            //      0
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(-1, 1), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 1, 1), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 2), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 1, 2), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 2, 2), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 1, 3), LocalZyoseki_StoneAttribute._0_Empty)
                },
                 8,
                75
            ),
            //pattern 21:    2      solidify eye on center
            //        202
            //     230
            //      1
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(-1, 1), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 1, 1), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 0, 2), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 1, 2), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 2, 2), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl( 1, 3), LocalZyoseki_StoneAttribute._1_YourPiece)
                },
                 8,
                75
            ),
            //pattern 23: 20100     connect if invaded
            //        00302
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 0), LocalZyoseki_StoneAttribute._1_YourPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 1), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(3, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(3, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(4, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(4, 1), LocalZyoseki_StoneAttribute._2_MyPiece)
                },
                 8,
                84
            ),
            //pattern 24: 2100    connect if invaded
            //        0302
            new LocalZyosekiImpl(
                new ZyosekiStoneImpl[]{
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 0), LocalZyoseki_StoneAttribute._2_MyPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(0, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 0), LocalZyoseki_StoneAttribute._1_YourPiece),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(1, 1), LocalZyoseki_StoneAttribute._3_MyNextMove),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(2, 1), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(3, 0), LocalZyoseki_StoneAttribute._0_Empty),
                    new ZyosekiStoneImpl(new GyoretuPointImpl(3, 1), LocalZyoseki_StoneAttribute._2_MyPiece)
                },
                 8,
                83
            )
        };
    }
}