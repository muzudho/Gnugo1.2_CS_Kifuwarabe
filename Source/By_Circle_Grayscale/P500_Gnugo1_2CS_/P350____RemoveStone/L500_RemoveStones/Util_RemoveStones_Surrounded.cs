/**
 * C# Arrange of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-28
 *  
 * exambord.c -> new file.
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
using Grayscale.GPL.P340____Liberty____.L500_LibertyAll;

namespace Grayscale.GPL.P350____RemoveStone.L500_RemoveStones
{
    /// <summary>
    /// �͂�ꂽ�΂��A�Տォ�珜�O���܂��B�R�E�ɂȂ�Ȃ����ǂ����A���ӂ��܂��B
    /// 
    /// Gnugo1.2 �ł́Aexambord�֐��B
    /// </summary>
    public abstract class Util_RemoveStones_Surrounded
    {
        /// <summary>
        /// �͂�ꂽ�΂��A�Տォ�珜�O���܂��B�R�E�ɂȂ�Ȃ����ǂ����A���ӂ��܂��B
        /// 
        /// Gnugo1.2 �ł́Aexambord�֐��B
        /// </summary>
        /// <param name="colorKo">���ꂽ���́A�� or ��</param>
        /// <param name="taikyoku"></param>
        public static void RemoveStones_Surrounded
        (
            StoneColor colorKo,
            Taikyoku taikyoku
        )
        {
            // �Տ�̑S�Ẵ|�C���g�ɁA���̐������L�������܂��B
            //
            // ���̒n�_���A�l���łȂ����Ă���΂̂����܂�ipiece�j�̂Ƃ��A
            // ���̃s�[�X�S�̂ł̃��o�e�B�[�𐔂��A�|�C���g�̂P�P�ɂ��̐������o�������܂��B
            int[,] libertyOfPiece_eachPoint;           
            Util_CountLibertyAll.Count_LibertyOfPiece_EachPoint(out libertyOfPiece_eachPoint, colorKo, taikyoku);

            // ������΂̈ʒu�����������܂��B�i�R�E�Ŗ߂��Ȃ��Ă͂Ȃ�Ȃ��΂̈ʒu���o���Ă������́j
            if (colorKo == taikyoku.MyColor)
            {
                taikyoku.MyKo.MoveToVanish();
            }
            else
            {
                taikyoku.YourKo.MoveToVanish();
            }


            // �R�E�̓����Ŏ���΂̐��B
            int countOfDelete = 0;

            // ���o�e�B�[�̂Ȃ��΂͑S�Č�Ԃ���폜���܂��B
            // �܂�l����S�Ĉ͂܂ꂽ�s�[�X�i�΂̂��܂�j�ł��B
            for (int i = 0; i < taikyoku.GobanBounds.BoardSize; i++)
            {
                for (int j = 0; j < taikyoku.GobanBounds.BoardSize; j++)
                {
                    GobanPoint location = new GobanPointImpl(i, j);

                    if
                    (
                        // ����鑤�̐΂ł���A
                        taikyoku.Goban.LookColor(location) == colorKo
                        &&
                        // �s�[�X�̃��o�e�B�[���L�^����Ă��Ȃ��Ȃ�
                        libertyOfPiece_eachPoint[i,j] == 0
                    )
                    {
                        // �΂����O���܂��B
                        taikyoku.Goban.Put(location, StoneColor.Empty);

                        // �i��Ō��ɖ߂����Ƃ�����̂Łj������΂̈ʒu���L�����A���ꂽ�΂̐����J�E���g�A�b�v���܂��B
                        if (colorKo == taikyoku.MyColor)
                        {
                            // �R���s���[�^�[��
                            taikyoku.MyKo.SetLocation(location);
                            ++taikyoku.Count_MyCaptured;
                        }
                        else
                        {
                            // �l�ԑ�
                            taikyoku.YourKo.SetLocation(location);
                            ++taikyoku.Count_YourCaptured;
                        }
                        ++countOfDelete;    // ���̎w����Ŏ������̐��H
                    }

                    // ������΂��Q�ȏ�Ȃ�A�R�E�iKo�j�̉\�����Ȃ��Ȃ�܂��B�΂̈ʒu�̋L�������Z�b�g���܂��B
                    if (colorKo == taikyoku.MyColor && 1 < countOfDelete)
                    {
                        // �R���s���[�^�[��������΂̈ʒu�i�R�E�ɂȂ邩������Ȃ��j���N���A�[�B
                        taikyoku.MyKo.MoveToVanish();
                    }
                    else if (1 < countOfDelete) // TODO: �������� 1 < countOfDelete ���Ȃ��Ă����̂��H
                    {
                        // �l�Ԃ�������΂̈ʒu�i�R�E�ɂȂ邩������Ȃ��j���N���A�[�B
                        taikyoku.YourKo.MoveToVanish();   
                    }

                }//j
            }//i
        }

    }
}