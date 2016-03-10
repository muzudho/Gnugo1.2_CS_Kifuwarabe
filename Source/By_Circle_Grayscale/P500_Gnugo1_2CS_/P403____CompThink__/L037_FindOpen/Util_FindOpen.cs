/**
 * C# Arrange of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-28
 *  
 * findopen.c -> Util_Findopen.cs
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
using System.Collections.Generic;

namespace Grayscale.GPL.P403____CompThink__.L037_FindOpen
{
    /// <summary>
    /// �J�����g�ʒu����\�Ȏw�����T���܂��B
    /// </summary>
    public abstract class Util_FindOpen
    {
        /// <summary>
        /// �w�肵���|�C���g�ɂ��āA�S�Ẵ��o�e�B�[���J���Ă���X�y�[�X��T���܂��B
        /// 
        /// �ċA���܂��B
        /// 
        /// Gnugo1.2 �ł� findopen�֐��B
        /// </summary>
        /// <param name="trayLocations_mutable">�\�ȓ����̂��߂̈ʒu�̔z��BGnugo1.2 �ł́Ai�z��Aj�z��B</param>
        /// <param name="location">Gnugo1.2�ł́A �J�����g �s�ԍ� m = 0�`18�A��ԍ� n = 0�`18�B</param>
        /// <param name="color">�� or ��</param>
        /// <param name="liberty123OfPiece">Gnugo1.2 �ł� minlib �����B3�ȉ��̃��o�e�B�[</param>
        /// <param name="taikyoku"></param>
        /// <returns></returns>
        public static bool FindOpenLocations
        (
            List<GobanPoint> trayLocations_mutable,
            GobanPoint location,
            StoneColor color,
            int liberty123OfPiece,
            Taikyoku taikyoku
        )
        {
            // ���̈ʒu�͂������ׂ��A�Ƃ����t���O�𗧂Ă܂��B
            taikyoku.MarkingBoard.Done_Current(location);

            // �k�l�C�o�[
            if (!location.IsNorthEnd())//�k�[�łȂ����
            {
                if
                (
                    // �k�ׂ�����ۂŁB
                    taikyoku.Goban.LookColor_NorthOf(location) == StoneColor.Empty
                    &&
                    // �k�ׂ��A�΂�������ꏊ�i�R�E�ɂȂ邩������Ȃ��j�łȂ���΁B
                    !taikyoku.MyKo.Is_NorthOf(location)
                )
                {
                    // �k�ׂ��A�����w����Ƃ��Ēǉ��B
                    trayLocations_mutable.Add( location.ToNorth());
                    if (trayLocations_mutable.Count == liberty123OfPiece)
                    {
                        // ���o�e�B�[�̐����ǉ������Ȃ琳��I���B
                        return true;
                    }
                }
                else if
                (
                    // �w�肵���|�C���g�̖k�ׂ��w��̐F�ŁB
                    taikyoku.Goban.LookColor_NorthOf(location) == color
                    &&
                    taikyoku.MarkingBoard.CanDo_North(location) // �k�����܂����ׂĂ��Ȃ��Ȃ�
                )
                {
                    if
                    (
                        Util_FindOpen.FindOpenLocations(trayLocations_mutable, location.ToNorth(), color, liberty123OfPiece, taikyoku)
                        &&
                        trayLocations_mutable.Count == liberty123OfPiece
                    )
                    {
                        return true;
                    }
                }
            }

            // ��l�C�o�[�𒲂ׂ܂��B
            if (!location.IsSouthEnd(taikyoku.GobanBounds))
            {
                if
                (
                    taikyoku.Goban.LookColor_SouthOf(location) == StoneColor.Empty
                    &&
                    // ��ׂ��A������΁i�R�E��������Ȃ��j�łȂ���΁B
                    !taikyoku.MyKo.Is_SouthOf(location)
                )
                {
                    trayLocations_mutable.Add( location.ToSouth());
                    if (trayLocations_mutable.Count == liberty123OfPiece)
                    {
                        return true;
                    }
                }
                else
                {
                    if
                    (
                        taikyoku.Goban.LookColor_SouthOf(location) == color
                        &&
                        taikyoku.MarkingBoard.CanDo_South(location)
                    )
                    {
                        if
                        (
                            Util_FindOpen.FindOpenLocations(trayLocations_mutable, location.ToSouth(), color, liberty123OfPiece, taikyoku)
                            &&
                            trayLocations_mutable.Count == liberty123OfPiece
                        )
                        {
                            return true;
                        }
                    }
                }
            }

            // ���l�C�o�[�𒲂ׂ܂��B
            if (location.J != 0)
            {
                if
                (
                    taikyoku.Goban.LookColor_WestOf(location) == StoneColor.Empty
                    &&
                    // ���ׂ��A������΁i�R�E��������Ȃ��j�łȂ���΁B
                    taikyoku.MyKo.Is_WestOf(location)
                )
                {
                    trayLocations_mutable.Add( location.ToWest());
                    if (trayLocations_mutable.Count == liberty123OfPiece)
                    {
                        return true;
                    }
                }
                else if
                (
                    taikyoku.Goban.LookColor_WestOf(location) == color
                    &&
                    taikyoku.MarkingBoard.CanDo_West(location)
                )
                {
                    if
                    (
                        Util_FindOpen.FindOpenLocations(trayLocations_mutable, location.ToWest(), color, liberty123OfPiece, taikyoku)
                        &&
                        trayLocations_mutable.Count == liberty123OfPiece
                    )
                    {
                        return true;
                    }
                }
            }

            // ���l�C�o�[�𒲂ׂ܂��B
            if (!location.IsEastEnd(taikyoku.GobanBounds))
            {
                if
                (
                    taikyoku.Goban.LookColor_EastOf(location) == StoneColor.Empty
                    &&
                    // ���ׂ��A������΁i�R�E��������Ȃ��j�łȂ���΁B
                    taikyoku.MyKo.Is_EastOf(location)
                )
                {
                    trayLocations_mutable.Add( location.ToEast());
                    if (trayLocations_mutable.Count == liberty123OfPiece)
                    {
                        return true;
                    }
                }
                else if
                (
                    taikyoku.Goban.LookColor_EastOf(location) == color
                    &&
                    taikyoku.MarkingBoard.CanDo_East(location)
                )
                {
                    if
                    (
                        Util_FindOpen.FindOpenLocations(trayLocations_mutable, location.ToEast(), color, liberty123OfPiece, taikyoku)
                        &&
                        trayLocations_mutable.Count == liberty123OfPiece
                    )
                    {
                        return true;
                    }
                }
            }

            // �J���Ă���|�C���g��������̂Ɏ��s������
            return false;
        }
    }
}