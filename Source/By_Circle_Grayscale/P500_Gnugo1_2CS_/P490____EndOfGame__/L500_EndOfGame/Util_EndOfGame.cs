/**
 * C# Arrange of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-28
 *  
 * endgame.c -> Util_EndOfGame.cs
 *           -> Scene_EndOfGame.cs
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
using Grayscale.GPL.P___480_Print______.L500_Print;
using Grayscale.GPL.P160____Collection_.L500_Collection;
using Grayscale.GPL.P190____Board______.L260_PointFugo;
using Grayscale.GPL.P190____Board______.L500_Util;
using Grayscale.GPL.P490____EndOfGame__.L250_SeitiZi;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Grayscale.GPL.P490____EndOfGame__.L500_EndOfGame
{

    public class Util_EndOfGame
    {
        /// <summary>
        /// �Տ�̊e�ꏊ�ɂ��āA�׎l���ɓ����F�̐΂����邩�𒲂ׁA
        /// ����� ���̏ꏊ���L�����܂��B
        /// 
        /// Gnugo1.2 �ł� createlist�֐��B�񎟌��z����g�p���Ă������ADictionary�AList �ɕύX�����B
        /// </summary>
        /// <param name="color"></param>
        /// <param name="out_list_neswStones_asNode_eachPoint">Gnugo1.2 �ł́A movelist �Ƃ������́B</param>
        /// <param name="taikyoku"></param>
        private static void GetNeswStones_AsNode_EachPoint(
            StoneColor color,
            out Dictionary<int,List<int>> out_list_neswStones_asNode_eachPoint,
            Taikyoku taikyoku
        )
        {
            //----------------------------------------
            // �����̐���
            //----------------------------------------
            //
            // �e�ꏊ�ɂ��āA�׎l���ɓ����F�̐΂����邩�𒲂ׁA
            // ����� ���̏ꏊ���L�����܂��B
            //

            out_list_neswStones_asNode_eachPoint = new Dictionary<int,List<int>>();

            for (int i = 0; i < taikyoku.GobanBounds.BoardSize; i++)
            {
                for (int j = 0; j < taikyoku.GobanBounds.BoardSize; j++)
                {
                    GobanPoint location = new GobanPointImpl(i, j);

                    if (taikyoku.Goban.At(location) == color)//�w�肵���F�̐΂ɂ���
                    {
                        int node = Util_AboutBoard.GetNodeByLocation(location, taikyoku.GobanBounds);//�Տ�̏ꏊ�̒ʂ��ԍ�

                        // �k���쐼�̏ꏊ�B�΂�����ꍇ�̂݁B
                        List<int> neswNodes = new List<int>();
                        // �܂��A���̏ꏊ�̎l���̗l�q���L�����邽�߂̃��X�g��p�ӁB
                        out_list_neswStones_asNode_eachPoint.Add(node, neswNodes);

                        // �k�ׂ𒲂ׂ�
                        if (!location.IsNorthEnd())//�k�[�ł͂Ȃ��Ȃ�B
                        {
                            if (taikyoku.Goban.NorthOf(location) == color)
                            {
                                neswNodes.Add(Util_AboutBoard.GetNodeByLocation(location.ToNorth(), taikyoku.GobanBounds));//�k�ׂ̏ꏊ��ǉ��B
                            }
                        }
                        // ���ׂ𒲂ׂ�
                        if (!location.IsEastEnd(taikyoku.GobanBounds))//���[�ł͂Ȃ��Ȃ�B
                        {
                            if (taikyoku.Goban.EastOf(location) == color)
                            {
                                neswNodes.Add(Util_AboutBoard.GetNodeByLocation(location.ToEast(), taikyoku.GobanBounds));
                            }
                        }
                        // ��ׂ𒲂ׂ�
                        if (!location.IsSouthEnd(taikyoku.GobanBounds))//��[�ł͂Ȃ��Ȃ�
                        {
                            if (taikyoku.Goban.SouthOf(location) == color)
                            {
                                neswNodes.Add(Util_AboutBoard.GetNodeByLocation(location.ToSouth(), taikyoku.GobanBounds));
                            }
                        }
                        // ���ׂ𒲂ׂ�
                        if (!location.IsWestEnd())//���[�ł͂Ȃ��Ȃ�B
                        {
                            if (taikyoku.Goban.WestOf(location) == color)
                            {
                                neswNodes.Add(Util_AboutBoard.GetNodeByLocation(location.ToWest(), taikyoku.GobanBounds));
                            }
                        }
                    }
                    // end if for color     
                }
                // End j loop
            }
            // End i loop
        }

        /// <summary>
        /// �w�肵����_�i���j����A�l���ɐڑ����Ă���΂�S�Đ����グ�āA�Ԃ��܂��B
        /// 
        /// ���̌�_�́A�l�Ԃ��R���\�[��������͂��Ďw�肵�܂��B
        /// </summary>
        /// <param name="out_connectedStones_asNode">Gnugo1.2 �ł́Alistpt �Ƃ������O�̃O���[�o���z��B</param>
        /// <param name="humanInput_location">Gnugo1.2 �ł́Ai,j �Ƃ����ϐ����B�l�Ԃ��R���\�[���ɓ��͂�����_�������Ă���B</param>
        /// <param name="list_neswStones_asNode_eachPoint">Gnugo1.2 �ł́A movelist �Ƃ������́B</param>
        private static void GetConnectedStones_AsNode_FromPoint
        (
            out List<int> out_connectedStones_asNode,
            GobanPoint humanInput_location,
            Dictionary<int, List<int>> list_neswStones_asNode_eachPoint,
            Taikyoku taikyoku
        )
        {
            out_connectedStones_asNode = new List<int>();

            //
            // ���̌�_�͐��������ǂ����̃t���O�ł��B
            //
            // Gnugo1.2 �ł� color �Ƃ����ϐ������������A�΂̐F�ƕ���킵���̂Ŗ��̂�ύX�����B
            //
            CheckFlag_Ending[] counted_onBoard = new CheckFlag_Ending[taikyoku.GobanBounds.Nodes];

            //
            // �S�Ă̌�_���A�܂������Ă��Ȃ��Ƃ����ݒ�ɂ��܂��B
            //
            for (int node = 0; node < taikyoku.GobanBounds.Nodes; node++)
            {
                counted_onBoard[node] = CheckFlag_Ending.Yet;
            }

            //
            // �l�Ԃ��R���\�[�����͂�����_�ɂ���΂ƁA�l�����Ȃ����Ă��铯���F�̐΂�
            // ���̃L���[�ɓ����Ă͏o�āA��A�����܂��B
            //
            // Gnugo1.2 �ł́A�L���[�����삵�Ă������AC#�Ղł� �����̂��̂��g���悤�ɕύX�B
            //
            Queue queue_connectedNodes = new Queue(150);

            // �l�Ԃ��w�肵����_�� �`�F�b�N�ς݂ɂ��A�L���[�ɒǉ����܂��B
            counted_onBoard[Util_AboutBoard.GetNodeByLocation(humanInput_location, taikyoku.GobanBounds)] = CheckFlag_Ending.Working;
            queue_connectedNodes.Enqueue(Util_AboutBoard.GetNodeByLocation(humanInput_location, taikyoku.GobanBounds));
#if DEBUG 
            Console.WriteLine("Survived first enqueue in bfslist.");
#endif

            //
            // �L���[����ɂȂ�܂ŁA�����̘A�����J��Ԃ��܂��B
            //
            while (queue_connectedNodes.Count != 0)
            {
                // �܂��A�l�ԂɎw�肳�ꂽ��_�B�i���[�v�̂Q�T�ڂ���́A���̗אڂ���΁j
                int node_a = (int)queue_connectedNodes.Dequeue();

                // �w�肳�ꂽ��_�́A���݂���k���쐼�ׂ̐΁B�i���ɒ��ׂĔz��ɓ���Ă���܂��j
                for (int nesw = 0; nesw < list_neswStones_asNode_eachPoint[node_a].Count; nesw++)//
                {
                    // ���̖k���쐼�ׂ̐΂ɂ���
                    int node_b = list_neswStones_asNode_eachPoint[node_a][nesw];

                    // �܂������Ă��Ȃ����
                    if (counted_onBoard[node_b] == CheckFlag_Ending.Yet)
                    {
                        // ���̌�_�ɂ͍�ƒ��t���O�𗧂āA
                        // ���̌�_�i�ׂ̐΁j���A���ɔ��肷��΂Ƃ��ăL���[�ɒǉ����܂��B�A���I�ȓ����ɂȂ�܂��B
                        counted_onBoard[node_b] = CheckFlag_Ending.Working;
                        queue_connectedNodes.Enqueue(node_b); // 
                    }
                }
#if DEBUG 
                Console.WriteLine("Just prior to first dequeue!.");
#endif

                // �l�ԂɎw�肳�ꂽ��_�i���[�v�̂Q�T�ڂ���́A���̗אڂ���΁j�́A
                // �����グ��Ώۂ���O���A�ԋp�p�̃��X�g�ɒǉ����܂��B
                counted_onBoard[node_a] = CheckFlag_Ending.Finished;
                out_connectedStones_asNode.Add( node_a); 
            }
        }

        /// <summary>
        /// ����ł���΂̂Ȃ�����A�Տォ��폜���܂��B
        /// 
        /// Gnugo1.2 �ł́Aendgame�֐��B
        /// </summary>
        /// <param name="taikyoku"></param>
        public static void Remove_DeadPieces(Taikyoku taikyoku)
        {
            bool isContinuePhase; // Gnugo1.2 �ł� cont �Ƃ����ϐ����Bcontinue�̗����낤���H

            //
            // �ŏ��ɃR���s���[�^�[���𒲂ׁA���ɐl�ԑ��𒲂ׂ܂��B
            // �Տ�̊e�_�̐΂��A�ǂ̕����ɗאڂ���΂������Ă��邩�𒲂ׂĂ��܂��B
            //

            // �R���s���[�^�[��
            Dictionary<int, List<int>> myList_neswStones_asNode_eachPoint;// Gnugo1.2 �ł́Amymovelist �Ƃ����ϐ����Bint mymovelist[NODES][5]�B
            Util_EndOfGame.GetNeswStones_AsNode_EachPoint(taikyoku.MyColor, out myList_neswStones_asNode_eachPoint, taikyoku);

            // �l�ԑ�
            Dictionary<int, List<int>> yourList_neswStones_asNode_eachPoint;// Gnugo1.2 �ł́Aumovelist �Ƃ����ϐ����Bint umovelist[NODES][5];�B
            Util_EndOfGame.GetNeswStones_AsNode_EachPoint(taikyoku.YourColor, out yourList_neswStones_asNode_eachPoint, taikyoku);

            isContinuePhase = true;
            GobanPoint location;// Gnugo1.2 �ł́A i, j �Ƃ����ϐ����B
            do
            {
                Console.Write("Dead piece? ");

                // ����ł���΂̏ꏊ����͂��Ă��������B
                string an_str = Console.ReadLine();
                //scanf("%s", an);

                //----------------------------------------
                // �Ó��ȓ��͂�����܂ŁA���邭��񂵂܂��B
                //----------------------------------------
                {//2015-11-27 �ǉ�
                    bool valid = false;

                    if (an_str == "stop")
                    {
                        valid = true;
                        location = null;
                    }
                    else
                    {
                        valid = PointFugoImpl.TryParse(an_str, out location,taikyoku);
                    }

                    if (!valid)
                    {
                        // ���͂��s�Ó��Ȃ̂ŁA���[�v�������B
                        goto gt_DeadPieceContinue;
                    }
                }

                if (an_str == "stop")
                {
                    // ���[�v���甲���܂��B
                    isContinuePhase = false;
                    goto gt_DeadPieceContinue;
                }
                else
                {
                    if (taikyoku.Goban.At(location) == taikyoku.MyColor)
                    {
#if DEBUG
                        Console.WriteLine("Just before bfslist.");
#endif
                        List<int> nodeList;
                        Util_EndOfGame.GetConnectedStones_AsNode_FromPoint(out nodeList, location, myList_neswStones_asNode_eachPoint,taikyoku);
#if DEBUG
                        Console.WriteLine("Survived first bfslist.");
#endif
                        // �����ꂽ�ꏊ����ɂ��Ă����A�R���s���[�^�[�̎��ꂽ�΃J�E���g�𑝂₵�܂��B
                        foreach(int node in nodeList)
                        {
                            Util_AboutBoard.GetLocationByNode(out location, node, taikyoku.GobanBounds);
                            taikyoku.Goban.Put(location, StoneColor.Empty);
                            taikyoku.Count_MyCaptured++;
                        }
                    }
                    else if (taikyoku.Goban.At(location) == taikyoku.YourColor)
                    {
#if DEBUG
                        Console.WriteLine("Just before second bfslist.");
#endif
                        List<int> nodeList;
                        Util_EndOfGame.GetConnectedStones_AsNode_FromPoint(out nodeList, location, yourList_neswStones_asNode_eachPoint,taikyoku);

                        // �����ꂽ�ꏊ����ɂ��Ă����A�l�Ԃ̎��ꂽ�΃J�E���g�𑝂₵�܂��B
                        foreach (int node in nodeList)
                        {
                            Util_AboutBoard.GetLocationByNode(out location, node, taikyoku.GobanBounds);
                            taikyoku.Goban.Put(location, StoneColor.Empty);
                            taikyoku.Count_YourCaptured++;
                        }
                    }
                    ((BoardPrinterB)taikyoku.BoardPrinter).ShowBoard(taikyoku);
                }

            gt_DeadPieceContinue:
                ;
            }
            while (isContinuePhase);
        }

        /// <summary>
        /// �ǂ���̒n�i�w�n�j�ł��Ȃ���_���A�������̐΂Ō��݂ɖ��߂܂��B
        /// </summary>
        public static void FillNeutralTerritories
        (
            Taikyoku taikyoku
        )
        {
            bool isContinuePhase = true; // Gnugo1.2 �ł� cont �Ƃ����ϐ����Bcontinue�̗����낤���H
            int youMeCounter = 0;//�����𔽓]���邽�߂̃J�E���^�[

            GobanPoint location;// Gnugo1.2 �ł́A i, j �Ƃ����ϐ����B
            do
            {
                if (youMeCounter % 2 == 0)
                {
                    //
                    // �w�肵���ꏊ���A�R���s���[�^�[���̐΂ɒu�������鏈���ł��B
                    //
                    Console.Write("Your piece? ");

                    string an_str = Console.ReadLine();
                    //scanf("%s", an);

                    //----------------------------------------
                    // �Ó��ȓ��͂�����܂ŁA���邭��񂵂܂��B
                    //----------------------------------------
                    {//2015-11-27 �ǉ�
                        bool valid = false;

                        if (an_str == "stop")
                        {
                            valid = true;
                            location = null;
                        }
                        else
                        {
                            valid = PointFugoImpl.TryParse(an_str, out location,taikyoku);
                        }

                        if (!valid)
                        {
                            // ���͂��s�Ó��Ȃ̂ŁA���[�v�������B
                            goto gt_PlayerPieceContinue;
                        }
                    }
                    // �����œ��͂��Ó��ƕ��������B���̂��Ə����B

                    if (an_str == "stop")
                    {
                        // ���[�v���甲���܂��B
                        isContinuePhase = false;
                        goto gt_PlayerPieceContinue;
                    }
                    else
                    {
                        taikyoku.Goban.Put(location, taikyoku.YourColor);

                        ((BoardPrinterB)taikyoku.BoardPrinter).ShowBoard(taikyoku);
                    }
                }
                else
                {
                    //
                    // �w�肵���ꏊ���A����i�l�ԁj�̐΂ɒu�������鏈���ł��B
                    //
                    Console.Write("My piece? ");
                    string an_str = Console.ReadLine();
                    //scanf("%s", an);

                    //----------------------------------------
                    // �Ó��ȓ��͂�����܂ŁA���邭��񂵂܂��B
                    //----------------------------------------
                    {//2015-11-27 �ǉ�
                        bool valid = false;

                        if (an_str == "stop")
                        {
                            valid = true;
                            location = null;
                        }
                        else
                        {
                            valid = PointFugoImpl.TryParse(an_str, out location,taikyoku);
                        }

                        if (!valid)
                        {
                            // ���͂��s�Ó��Ȃ̂ŁA���[�v�������B
                            goto gt_PlayerPieceContinue;
                        }
                    }
                    // �����œ��͂��Ó��ƕ��������B���̂��Ə����B

                    if (an_str == "stop")
                    {
                        // ���[�v���甲���܂��B
                        isContinuePhase = false;
                        goto gt_PlayerPieceContinue;
                    }
                    else
                    {
                        taikyoku.Goban.Put(location, taikyoku.MyColor);
                        ((BoardPrinterB)taikyoku.BoardPrinter).ShowBoard(taikyoku);
                    }
                }
                youMeCounter++;

            gt_PlayerPieceContinue:
                ;
            }
            while (isContinuePhase);
        }

        /// <summary>
        /// �󂫃X�y�[�X���A�ǂ���̐w�n�����肵�A���̐w�n�̐΂ɕϊ����܂��B
        /// </summary>
        public static void FillStones
        (
            Taikyoku taikyoku
        )
        {
            for (int i = 0; i < taikyoku.GobanBounds.BoardSize; i++)
            {
                for (int j = 0; j < taikyoku.GobanBounds.BoardSize; j++)
                {
                    GobanPoint ijLocation = new GobanPointImpl(i, j);
                    if (taikyoku.Goban.At(ijLocation) == StoneColor.Empty)
                    {
                        taikyoku.Goban.Put(ijLocation, Util_SeitiZi.Test_SeitiZi(ijLocation, taikyoku));
                    }
                }
            }
        }

        /// <summary>
        /// �Տ�̐΂𐔂��܂��B
        /// </summary>
        public static void CountPieces
        (
            out int out_myTotal, // Gnugo1.2 �ł� mtot �Ƃ����ϐ����Bmy total �̗����낤���H
            out int out_yourTotal, // Gnugo1.2 �ł� utot �Ƃ����ϐ����Byour total �̗����낤���H
            Taikyoku taikyoku
        )
        {
            out_myTotal = 0;
            out_yourTotal = 0;

            for (int i = 0; i < taikyoku.GobanBounds.BoardSize; i++)
            {
                for (int j = 0; j < taikyoku.GobanBounds.BoardSize; j++)
                {
                    GobanPoint ijLocation = new GobanPointImpl(i, j);
                    if (taikyoku.Goban.At(ijLocation) == taikyoku.MyColor)
                    {
                        ++out_myTotal;
                    }
                    else if (taikyoku.Goban.At(ijLocation) == taikyoku.YourColor)
                    {
                        ++out_yourTotal;
                    }
                }
            }
        }
    }
}