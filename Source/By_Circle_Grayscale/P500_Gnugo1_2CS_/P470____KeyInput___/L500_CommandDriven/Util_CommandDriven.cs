/**
 * C# Arrange of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-28
 *  
 * getmove.c -> Util_CommandDriven.cs
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
using Grayscale.GPL.P___170_GameState__.L500_Struct;
using Grayscale.GPL.P___190_Board______.L063_Word;
using Grayscale.GPL.P___300_Taikyoku___.L500_Taikyoku;
using Grayscale.GPL.P160____Collection_.L500_Collection;
using Grayscale.GPL.P190____Board______.L260_PointFugo;
using Grayscale.GPL.P450____KeyValid___.L500_Suicide;
using Grayscale.GPL.P460____SaveLoad___.L500_SaveLoad;
using System;

namespace Grayscale.GPL.P470____KeyInput___.L500_CommandDriven
{
    /// <summary>
    /// �l�Ԃ̓��̓R�}���h���擾���܂��B
    /// </summary>
    public abstract class Util_CommandDriven
    {
        /// <summary>
        /// �l�Ԃ̓��͂����R�}���h�ɑΉ������������s���܂��B
        /// �Ԉ�������͂��������ꍇ�A�ċA�I�ɌĂяo����܂��B
        /// 
        /// Gnugo1.2 �ł́Agetmove�֐��B
        /// </summary>
        /// <param name="move_charArray">���͂���������Ba1��T19�Ȃǂ̎w����B</param>
        /// <param name="out_sasite">�w����B�΂�u���ʒu</param>
        /// <param name="taikyoku"></param>
        public static void DoCommand
        (
            string command_str,
            out GobanPoint out_sasite,
            Taikyoku taikyoku
        )
        {
            if (command_str == "stop")  // �Q�[�����I�����܂��B
            {
                taikyoku.PlayState = GameState.Stop;
                out_sasite = new GobanPointImpl(-1, -1);// 2015-11-26 �ǉ�
            }
            else
            {
                if (command_str == "save")  // �f�[�^��ۑ����āA�Q�[�����I�����܂��B
                {
                    // �ǖʂ��A�e�L�X�g�t�@�C���ɏ����o���܂��B
                    Util_Save.Save(taikyoku);

                    taikyoku.PlayState = GameState.Saved;
                    // i �� -1 �̂Ƃ��́A�p�X�̃V�O�i���ł��B
                    out_sasite = new GobanPointImpl(-1, -1);// 2015-11-26 �ǉ�
                }
                else if (command_str == "pass")  // �l�Ԃ̃p�X
                {
                    taikyoku.Pass++;
                    // i �� -1 �̂Ƃ��́A�p�X�̃V�O�i���ł��B
                    out_sasite = new GobanPointImpl(-1, -1);// 2015-11-26 �ǉ�
                }
                else
                {
                    taikyoku.Pass = 0;
                    if (
                        // �Ⴆ�΁A a1 ��A T19 �Ƃ��������͕�������͂��A�Տ�̈ʒu�ɕϊ����܂��B
                        !PointFugoImpl.TryParse(command_str, out out_sasite,taikyoku)
                        ||
                        (taikyoku.Goban.LookColor(out_sasite) != StoneColor.Empty)
                        ||
                        Util_Suicide.Aa_Suicide(out_sasite, taikyoku)
                    )
                    {
                        //
                        // �񍇖@�肾�����ꍇ�A�ē��͂𑣂��܂��B
                        //
                        Console.WriteLine("illegal move !"); // [" + command_str + "] 2015-11-26 ���͂��ꂽ�R�}���h��\������悤�Ɋg��
                        Console.Write("your move? ");

                        // �����āA�ċA�I�ɏ������s�B
                        string command_str2 = Console.ReadLine();
                        //scanf("%s", move);

                        Util_CommandDriven.DoCommand(command_str2, out out_sasite, taikyoku);
                    }
                }
            }
        }
    }
}