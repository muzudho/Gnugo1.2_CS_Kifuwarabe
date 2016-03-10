/**
 * C# Arrange of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-26
 *  
 * eval.c    -> Util_CountLibertyAll.cs
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

namespace Grayscale.GPL.P340____Liberty____.L500_LibertyAll
{
    public abstract class Util_CountLibertyAll
    {
        /// <summary>
        /// �]���֐��Ƃ������́A�Տ�̂Ȃ����������F�̐΂̃��o�e�B�[�𐔂��Ă��܂��B
        ///
        /// Gnugo1.2 �ł́Aeval �֐��B
        /// </summary>
        /// <param name="libertyOfPiece_eachPoint">
        /// �����F�̐΂̂Ȃ����Ă��镔���ipiece�j�̃��o�e�B�[�i�l���̐΂�u����Ƃ���j�𐔂��܂��B
        /// �s�[�X�̂����A�ŏ��Ɏw�肵���P�� ���̐����͋L�^����܂��B
        /// 
        /// Gnugo1.2 �ł́Al �Ƃ������O�̃O���[�o���ϐ��Bliberty �̗����낤���H
        /// eval �œ��e���ݒ肳��A���̓��e�� exambord�Afindsavr�Afindwinr�Asuicide�Ŏg�p����܂��B
        /// </param>
        /// <param name="color">�� or ��</param>
        public static void Count_LibertyOfPiece_EachPoint
        (
            out int[,] libertyOfPiece_eachPoint,
            StoneColor color,
            Taikyoku taikyoku
        )
        {
            libertyOfPiece_eachPoint = new int[taikyoku.GobanBounds.BoardSize, taikyoku.GobanBounds.BoardSize];

            // ���ꂼ��̃s�[�X�̃��o�e�B�[�𐔂��܂��B
            for (int i = 0; i < taikyoku.GobanBounds.BoardSize; i++)
            {
                for (int j = 0; j < taikyoku.GobanBounds.BoardSize; j++)
                {
                    GobanPoint location = new GobanPointImpl(i, j);
                    if (taikyoku.Goban.LookColor(location) == color)// �w��̐F�̂�
                    {
                        int libertyOfPiece; // Gnugo1.2 �ł́A�O���[�o���ϐ� lib = 0 �ł����B
                        Util_CountLiberty.Count_LibertyOfPiece(out libertyOfPiece, location, color, taikyoku);
                        libertyOfPiece_eachPoint[i, j] = libertyOfPiece;
                    }
                }
            }
        }
    }
}