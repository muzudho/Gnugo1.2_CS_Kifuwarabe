/**
 * C# Arrange of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-28
 *  
 * fioe.c -> Util_OwnEye.cs
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
using Grayscale.GPL.P___190_Board______.L250_Board;
using Grayscale.GPL.P___300_Taikyoku___.L500_Taikyoku;
using Grayscale.GPL.P160____Collection_.L500_Collection;


namespace Grayscale.GPL.P403____CompThink__.L075_OwnEye
{
    /// <summary>
    /// �΂�ł����񂾏ꏊ���A�����̖ڂłȂ����ǂ������ׂ܂��B�i�񍇖@��j
    /// �R���s���[�^�[�̎v�l���A�܂��� findnext �Ŏg���܂��B
    /// 
    /// Gnugo1.2 �ł� fioe�֐��B
    /// </summary>
    public abstract class Util_OwnEye
    {
        /// <summary>
        /// �����i�R���s���[�^�[�j�̖ڂ֑ł����񂾂Ƃ��A�^�B�i�񍇖@��j
        /// </summary>
        /// <param name="location">Gnugo1.2 �ł́A�΂� �s�ԍ� i = 0�`18�A��ԍ� j = 0�`18�B</param>
        /// <param name="taikyoku"></param>
        /// <returns></returns>
        public static bool IsThis
        (
            GobanPoint location,
            Taikyoku taikyoku
        )
        {
            Board ban = taikyoku.Goban; // ���

            // ��ӂ𒲂ׂ܂��B
            if (location.IsNorthEnd())
            {
                if
                (
                    location.IsWestEnd()    // �k���̊p
                    &&
                    // �k���̋����͂ނQ�̐΂������i�R���s���[�^�[�j�̐F�Ȃ�B
                    ban.At(new GobanPointImpl(1, 0)) == taikyoku.MyColor
                    &&
                    ban.At(new GobanPointImpl(0, 1)) == taikyoku.MyColor
                )
                {
                    return true;
                }
                if
                (
                    location.IsEastEnd(taikyoku.GobanBounds)   // �k���p
                    &&
                    // �k���̋����͂ނQ�̐΂������i�R���s���[�^�[�j�̐F�Ȃ�B
                    ban.At(new GobanPointImpl(1, taikyoku.GobanBounds.BoardEnd)) == taikyoku.MyColor
                    &&
                    ban.At(new GobanPointImpl(0, taikyoku.GobanBounds.BoardEnd-1)) == taikyoku.MyColor
                )
                {
                    return true;
                }

                if
                (
                    // ��ӂŁA�R�������R���s���[�^�[�̐΂Ȃ�
                    ban.At(new GobanPointImpl(1, location.J)) == taikyoku.MyColor // �R���s���[�^�[�̐�
                    &&
                    // ���E�Ƃ��R���s���[�^�[�̐�
                    ban.At(new GobanPointImpl(0, location.J - 1)) == taikyoku.MyColor
                    &&
                    ban.At(new GobanPointImpl(0, location.J + 1)) == taikyoku.MyColor
                )
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            // ���ӂ𒲂ׂ܂��B
            if (location.IsSouthEnd(taikyoku.GobanBounds))
            {
                if
                (
                    location.IsWestEnd()    // �쐼�p
                    &&
                    // �쐼�̋����͂ނQ�̐΂��R���s���[�^�[�̐F�Ȃ�B
                    ban.At(new GobanPointImpl(taikyoku.GobanBounds.BoardEnd-1, 0)) == taikyoku.MyColor
                    &&
                    ban.At(new GobanPointImpl(taikyoku.GobanBounds.BoardEnd, 1)) == taikyoku.MyColor
                )
                {
                    return true;
                }

                if
                (
                    location.IsEastEnd(taikyoku.GobanBounds)   // �쓌
                    &&
                    // �쓌�̋����͂ނQ�̐΂��R���s���[�^�[�̐F�Ȃ�B
                    ban.At(new GobanPointImpl(taikyoku.GobanBounds.BoardEnd - 1, taikyoku.GobanBounds.BoardEnd)) == taikyoku.MyColor
                    &&
                    ban.At(new GobanPointImpl(taikyoku.GobanBounds.BoardEnd, taikyoku.GobanBounds.BoardEnd-1)) == taikyoku.MyColor
                )
                {
                    return true;
                }

                if
                (
                    // ���ӂŁA�R�������R���s���[�^�[�̐΂Ȃ�
                    ban.At(new GobanPointImpl(taikyoku.GobanBounds.BoardEnd-1, location.J)) == taikyoku.MyColor
                    &&
                    ban.At(new GobanPointImpl(taikyoku.GobanBounds.BoardEnd, location.J - 1)) == taikyoku.MyColor
                    &&
                    ban.At(new GobanPointImpl(taikyoku.GobanBounds.BoardEnd, location.J + 1)) == taikyoku.MyColor
                )
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            // ���ӂ𒲂ׂ܂��B
            if (location.IsWestEnd())
            {
                if
                (
                    // ���ӂŁA�R�������R���s���[�^�[�̐΂Ȃ�
                    ban.At(new GobanPointImpl(location.I, 1)) == taikyoku.MyColor
                    &&
                    ban.At(new GobanPointImpl(location.I - 1, 0)) == taikyoku.MyColor
                    &&
                    ban.At(new GobanPointImpl(location.I + 1, 0)) == taikyoku.MyColor
                )
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            // �E�ӂ𒲂ׂ܂��B
            if (location.IsEastEnd(taikyoku.GobanBounds))
            {
                if
                (
                    // �E�ӂŁA�R�������R���s���[�^�[�̐΂Ȃ�
                    ban.At(new GobanPointImpl(location.I, taikyoku.GobanBounds.BoardEnd-1)) == taikyoku.MyColor
                    &&
                    ban.At(new GobanPointImpl(location.I - 1, taikyoku.GobanBounds.BoardEnd)) == taikyoku.MyColor
                    &&
                    ban.At(new GobanPointImpl(location.I + 1, taikyoku.GobanBounds.BoardEnd)) == taikyoku.MyColor
                )
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            // �����̃s�[�X�𒲂ׂ܂��B
            if
            (
                // �S�������R���s���[�^�[�̐΂Ȃ�
                ban.NorthOf(location) == taikyoku.MyColor
                &&
                ban.EastOf(location) == taikyoku.MyColor
                &&
                ban.SouthOf(location) == taikyoku.MyColor
                &&
                ban.WestOf(location) == taikyoku.MyColor
            )
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