/**
 * C# Arrange of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-28
 *  
 * getij.c -> NodeFugoImpl.cs
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
using Grayscale.GPL.P___300_Taikyoku___.L500_Taikyoku;
using Grayscale.GPL.P160____Collection_.L500_Collection;

namespace Grayscale.GPL.P190____Board______.L260_PointFugo
{

    /// <summary>
    /// ��_�̕���
    /// 
    /// �͌�Տ�̐΂�u���Ƃ���B�����ł����Ƃ���̏��ɂ�����B
    /// 
    /// Gnugo1.2 �ł́Agetij �֐����������̂������B
    /// </summary>
    public abstract class PointFugoImpl
    {

        /// <summary>
        /// ���͕�������Ai,j ���W�֕ϊ����܂��B
        /// 
        /// Gnugo1.2 �ł́Agetij �֐����������̂������B
        /// </summary>
        /// <param name="move_str"></param>
        /// <param name="out_location"></param>
        /// <returns></returns>
        public static bool TryParse
        (
            string move_str,    // 2���A�܂���3���̎w���蕶����BGnugo1.2�ł� move �Ƃ����������B
            out GobanPoint out_location,    // �w����� �s�A��ԍ�
            Taikyoku taikyoku
        )
        {
            // move[0] from A�`T
            // move[1] move[2] from 1�`19
            // �w��������W�ɕϊ����܂��B
            char[] move = move_str.ToCharArray();
            int k;
            out_location = new GobanPointImpl(-1, -1);// 2015-11-27 �ǉ�

            if(move.Length<2)// 2015-11-26 �ǉ�
            {
                // 2���������Ȃ��Ó��B
                return false;
            }

            // 1������
            if
            (
                // A�`H �Ȃ�
                (move[0] >= 'A')
                &&
                (move[0] <= 'H')
            )
            {
                // 0�`7�� �ɕϊ�
                out_location.J = move[0] - 'A';
            }
            else
            {
                if
                (
                    // J�`T �Ȃ�
                    (move[0] >= 'J')
                    &&
                    (move[0] <= 'T')
                )
                {
                    // 8�`18�� �ɕϊ�
                    out_location.J = move[0] - 'B';
                }
                else
                {
                    // a�`h �Ȃ�
                    if ((move[0] >= 'a') && (move[0] <= 'h'))
                    {
                        // 0�`7�� �ɕϊ�
                        out_location.J = move[0] - 'a';
                    }
                    else
                    {
                        // j�`t �Ȃ�
                        if ((move[0] >= 'j') && (move[0] <= 't'))
                        {
                            // 8�`18��ɕϊ�
                            out_location.J = move[0] - 'b';
                        }
                        else
                        {
                            out_location.MoveToVanish();// 2015-11-26 �ǉ�
                            return false;
                        }
                    }
                }
            }

            // 2�����ڂ́A�i�Ɏg��
            k = move[1] - '0';
    
            // 3�����ڂ͒i
            if (2<move.Length)
            {
                k = k * 10 + move[2] - '0';
            }
            out_location.I = taikyoku.GobanBounds.BoardSize - k;

            if (out_location.ContainsI(0, taikyoku.GobanBounds.BoardEnd))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}