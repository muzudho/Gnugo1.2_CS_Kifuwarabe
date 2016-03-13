/**
 * C# Arrange of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-28
 *  
 * findcolr.c -> Util_SeitiZi.cs
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

namespace Grayscale.GPL.P490____EndOfGame__.L250_SeitiZi
{
    /// <summary>
    /// ���n�̒n�B
    /// </summary>
    public abstract class Util_SeitiZi
    {

        /// <summary>
        /// ���n���ꂽ�n���ǂ������A�m�F���܂��B
        /// 
        /// �ꏊ���P�w�肵�A
        /// ���̂S������ �΂��Ȃ����A�����F�̐΂����ō\������Ă���A
        /// ���������ꏊ�� ���n����Ă��鎩���̐w�n�Ƃ��Đ������B
        /// 
        /// Gnugo1.2 �ł� findcolr �֐��B
        /// </summary>
        /// <param name="location">Gnugo1.2 �ł́A �s�ԍ� i = 0�`18�A��ԍ� j = 0�`18�B</param>
        /// <param name="taikyoku"></param>
        /// <returns></returns>
        public static StoneColor Test_SeitiZi
        (
            GobanPoint location,
            Taikyoku taikyoku
        )
        {
            //----------------------------------------
            // �����̐���
            //----------------------------------------
            //
            // �Տ�̌�_���P�w�肵�܂��B
            //
            // �����ɐ΂�����΁A���̐F��Ԃ��܂��B
            //
            // �΂��Ȃ���΁A�S�����̒�����ōŏ��ɓ˂�������΂̐F�𒲂ׂ܂��B
            // �i�܂�ŁA�����̔�Ԃ̓����̂悤�Ɂj
            //

            StoneColor result = StoneColor.Empty;     // �F // 2015-11-26 �ǉ� Empty�ŏ������B

            /// [0]�k�A[1]��A[2]���A[3]��
            StoneColor[] color = new StoneColor[4];
                

            //
            // �w�肵���Տ�̈ʒu�ɐ΂�����΁A���̐F��Ԃ��܂��B
            //
            if (taikyoku.Goban.At(location) != StoneColor.Empty)
            {
                return taikyoku.Goban.At(location);
            }

            // �k�l�C�o�[���A�΂ɓ˂�������܂Œ��ׂ܂��B
            if (!location.IsNorthEnd())//�k�[�łȂ����
            {
                // �w��̍s�ԍ� �` 0 �܂ŁB�˂�������Ȃ���� k �� �k�[ �܂ŁB
                int k = location.I; 
                do
                {
                    --k;
                }
                while (
                    taikyoku.Goban.At(new GobanPointImpl(k, location.J)) == StoneColor.Empty
                    &&
                    0 < k
                );
                color[0] = taikyoku.Goban.At(new GobanPointImpl(k, location.J));
            }
            else
            {
                // ����ہB
                color[0] = StoneColor.Empty;
            }

            // ��l�C�o�[���A�΂ɓ˂�������܂Œ��ׂ܂��B
            if (!location.IsSouthEnd(taikyoku.GobanBounds))
            {
                // �i18��菬�����A�w��̍s�ԍ��j�` ��[ �܂ŁB
                int k = location.I;
                do
                {
                    ++k;
                }
                while
                (
                    taikyoku.Goban.At(new GobanPointImpl(k, location.J)) == StoneColor.Empty
                    &&
                    k < taikyoku.GobanBounds.BoardEnd
                );
                color[1] = taikyoku.Goban.At(new GobanPointImpl(k, location.J));
            }
            else
            {
                // ����ہB
                color[1] = StoneColor.Empty;
            }

            // ���l�C�o�[���A�΂ɓ˂�������܂Œ��ׂ܂��B
            if (!location.IsWestEnd())
            {
                // �i0���傫���A�w��̗�ԍ��j�` 1 �܂ŁB
                int k = location.J;
                do
                {
                    --k;
                }
                while
                (
                    taikyoku.Goban.At(new GobanPointImpl(location.I, k)) == StoneColor.Empty
                    &&
                    k > 0
                );
                color[2] = taikyoku.Goban.At(new GobanPointImpl(location.I, k));
            }
            else
            {
                // ����ہB
                color[2] = StoneColor.Empty;
            }

            // ���l�C�o�[���A�΂ɓ˂�������܂Œ��ׂ܂��B
            if (!location.IsEastEnd(taikyoku.GobanBounds))
            {
                // �i18��菬�����A�w��̗�ԍ��j�` 17 �܂ŁB
                int k = location.J;
                do
                {
                    ++k;
                }
                while
                (
                    taikyoku.Goban.At(new GobanPointImpl(location.I, k)) == StoneColor.Empty
                    &&
                    k < taikyoku.GobanBounds.BoardEnd
                );
                color[3] = taikyoku.Goban.At(new GobanPointImpl(location.I, k));
            }
            else
            {
                // ����ہB
                color[3] = StoneColor.Empty;
            }

            // �S�����̂����A����ۂłȂ���_�̐F���P�����m�肽���B
            for (int k=0;k<4;k++)
            {
                if (color[k] == StoneColor.Empty)
                {
                    continue;
                }
                else
                {
                    result = color[k];
                    break;
                }
            }

            // �N���X�`�F�b�N���܂��B
            // ����  �킽���������G���[���������Ȃ�A�S�Ă̎��s�[�X�͔Ղ�����Ȃ��B
            // �킽�������͂�����C������悤�v���C���[�����ɑ����B
            for (int k=0;k<4;k++)
            {
                // �F���ݒ肳��Ă���̂ɁA�Ԃ����ʂ̐F������Ă�����G���[�ł��B���ł��B
                if
                (
                    color[k] != StoneColor.Empty
                    &&
                    color[k] != result
                )
                {
                    return StoneColor.Empty;// 0;
                }
            }

            // �����A�킽���������S�Ẵ`�F�b�N��OK����΁A���ʂ����|�[�g���܂��B
            return result;
        }
    }
}