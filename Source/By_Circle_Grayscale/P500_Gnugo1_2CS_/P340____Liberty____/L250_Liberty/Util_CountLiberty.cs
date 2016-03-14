/**
 * C# Arrange of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-26
 *  
 * countlib.c
 * count.c    -> Util_CountLiberty.cs
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

namespace Grayscale.GPL.P340____Liberty____.L250_Liberty
{
    public abstract class Util_CountLiberty
    {
        /// <summary>
        /// ��Ղ̎w��̌�_ i,j �ɂ���΂��N�_�ɁA�Ȃ����Ă��铯���F�̐΂́i�P�A�܂��͘A�́j�����o�[�e�B�𐔂��܂��B
        /// �ċA�I�ɌĂяo����܂��B
        /// Countlib�֐�����Ăяo���Ă��������B
        /// 
        /// Gnugo1.2 �ł́Acount�֐��ł��B
        /// </summary>
        /// <param name="count">Gnugo1.2 �ł́A�O���[�o���ϐ� lib �ł����B</param>
        /// <param name="location">Gnugo1.2�ł́A�s�ԍ� i = 0�`18�A��ԍ� j = 0�`18�B</param>
        /// <param name="color">�� or ��</param>
        /// <param name="taikyoku"></param>
        private static void Count_Recursive(
            ref int count,
            GobanPoint location,
            StoneColor color,
            Taikyoku taikyoku
        )
        {
            //----------------------------------------
            // �����̉��
            //----------------------------------------
            // �΂̎���S�����ɂ��āA
            // �����Ă��Ȃ��󂫌�_�ł���� ���o�e�B�[�� �P���Z���܂��B
            // �����Ă��Ȃ��āA�w�肵���F�i��ɓ����F�j�̐΂ł���΁A���̐΂��瓯���\�b�h���ċA�Ăяo�����܂��B
            //

            // �w�肵���ʒu�́A�����ς݂Ƃ��ă}�[�N���܂��B
            taikyoku.CountedBoard.Done_Current(location);

            // �k�ׂ̐΂𒲂ׂ܂��B
            if (!location.IsNorthEnd())//�k�[�łȂ����
            {
                if
                (
                    taikyoku.Goban.NorthOf(location) == StoneColor.Empty
                    &&
                    taikyoku.CountedBoard.CanDo_North(location)
                )
                {
                    // �k�ׂ��󂢂Ă���  �܂������Ă��Ȃ��Ȃ�A
                    // ���o�e�B�[���P�����グ�܂��B������͏d�����Đ����܂���B
                    ++count;
                    taikyoku.CountedBoard.Done_North(location);
                }
                else if
                (
                    taikyoku.Goban.NorthOf(location) == color
                    &&
                    taikyoku.CountedBoard.CanDo_North(location)
                )
                {
                    // �k�ׂ� �w��F�̐΂��u���Ă���A�܂������Ă��Ȃ��Ȃ�A
                    // ���̐΂��炳��ɃJ�E���g�𑱂��܂��B
                    Util_CountLiberty.Count_Recursive(ref count, location.ToNorth(), color, taikyoku);
                }
                // �w�肵���F�łȂ��΂��u���Ă���Ή������Ȃ��B
            }

            // ��ׂ𒲂ׂ܂��B
            if (!location.IsSouthEnd(taikyoku.GobanBounds))//��[�łȂ����
            {
                // �����A�������������邾�낤���i�O���O�j
                if
                (
                    taikyoku.Goban.SouthOf(location) == StoneColor.Empty
                    &&
                    taikyoku.CountedBoard.CanDo_South(location)
                )
                {
                    ++count;
                    taikyoku.CountedBoard.Done_South(location);
                }
                else if
                (
                    taikyoku.Goban.SouthOf(location) == color
                    &&
                    taikyoku.CountedBoard.CanDo_South(location)
                )
                {
                    Util_CountLiberty.Count_Recursive(ref count, location.ToSouth(), color, taikyoku);
                }
            }

            // ���ׂ𒲂ׂ܂��B
            if (!location.IsWestEnd())//���[�łȂ����
            {
                if
                (
                    taikyoku.Goban.WestOf(location) == StoneColor.Empty
                    &&
                    taikyoku.CountedBoard.CanDo_West(location)
                )
                {
                    ++count;
                    taikyoku.CountedBoard.Done_West(location);
                }
                else if
                (
                    taikyoku.Goban.WestOf(location) == color
                    &&
                    taikyoku.CountedBoard.CanDo_West(location)
                )
                {
                    Util_CountLiberty.Count_Recursive(ref count, location.ToWest(), color, taikyoku);
                }
            }

            // ���ׂ𒲂ׂ܂��B
            if (!location.IsEastEnd(taikyoku.GobanBounds))//���[�łȂ����
            {
                if
                (
                    (taikyoku.Goban.EastOf(location) == StoneColor.Empty)
                    &&
                    taikyoku.CountedBoard.CanDo_East(location)
                )
                {
                    ++count;
                    taikyoku.CountedBoard.Done_East(location);
                }
                else if
                (
                    taikyoku.Goban.EastOf(location) == color
                    &&
                    taikyoku.CountedBoard.CanDo_East(location)
                )
                {
                    Util_CountLiberty.Count_Recursive(ref count, location.ToEast(), color, taikyoku);
                }
            }
        }


        /// <summary>
        /// ��Ղ̎w��̌�_ i,j �ɂ���΂��N�_�ɁA�Ȃ����Ă��铯���F�́i�P�A�܂��͘A�́j�΁icolor piece�j�̑����o�[�e�B�𐔂��܂��B
        /// 
        /// Gnugo1.2 �ł́Acountlib�֐��ł��B
        /// </summary>
        /// <param name="out_count">Gnugo1.2 �ł́A�O���[�o���ϐ� lib �ł����B</param>
        /// <param name="startLocation_inPiece">Gnugo1.2�ł́A �s�ԍ� m = 0�`18�A��ԍ� n = 0�`18�B</param>
        /// <param name="color">�� or ��</param>
        public static void Count
        (
            out int out_count,
            GobanPoint startLocation_inPiece,
            StoneColor color,
            Taikyoku taikyoku
        )
        {
            out_count = 0;// Gnugo1.2 �ł́Acountlib�֐��̌Ăяo������ �O���[�o���ϐ��� lib = 0 ���Ă��܂����B


            // �S�Ẵs�[�X�𐔂��Ȃ�����悤�ɁA���Z�b�g���܂��B
            taikyoku.CountedBoard.FillAll_WeCan(taikyoku.GobanBounds.BoardSize);

            // �J�����g�E�s�[�X�̃��o�e�B�[�𐔂��܂��B
            Util_CountLiberty.Count_Recursive(ref out_count, startLocation_inPiece, color, taikyoku);
        }
    }
}