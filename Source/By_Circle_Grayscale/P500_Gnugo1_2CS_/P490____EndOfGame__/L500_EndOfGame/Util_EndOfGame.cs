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
        /// 盤上の各場所について、隣四方に同じ色の石があるかを調べ、
        /// あれば その場所を記憶します。
        /// 
        /// Gnugo1.2 では createlist関数。二次元配列を使用していたが、Dictionary、List に変更した。
        /// </summary>
        /// <param name="color"></param>
        /// <param name="out_list_neswStones_asNode_eachPoint">Gnugo1.2 では、 movelist という名称。</param>
        /// <param name="taikyoku"></param>
        private static void GetNeswStones_AsNode_EachPoint(
            StoneColor color,
            out Dictionary<int,List<int>> out_list_neswStones_asNode_eachPoint,
            Taikyoku taikyoku
        )
        {
            //----------------------------------------
            // 実装の説明
            //----------------------------------------
            //
            // 各場所について、隣四方に同じ色の石があるかを調べ、
            // あれば その場所を記憶します。
            //

            out_list_neswStones_asNode_eachPoint = new Dictionary<int,List<int>>();

            for (int i = 0; i < taikyoku.GobanBounds.BoardSize; i++)
            {
                for (int j = 0; j < taikyoku.GobanBounds.BoardSize; j++)
                {
                    GobanPoint location = new GobanPointImpl(i, j);

                    if (taikyoku.Goban.At(location) == color)//指定した色の石について
                    {
                        int node = Util_AboutBoard.GetNodeByLocation(location, taikyoku.GobanBounds);//盤上の場所の通し番号

                        // 北東南西の場所。石がある場合のみ。
                        List<int> neswNodes = new List<int>();
                        // まず、その場所の四方の様子を記憶するためのリストを用意。
                        out_list_neswStones_asNode_eachPoint.Add(node, neswNodes);

                        // 北隣を調べる
                        if (!location.IsNorthEnd())//北端ではないなら。
                        {
                            if (taikyoku.Goban.NorthOf(location) == color)
                            {
                                neswNodes.Add(Util_AboutBoard.GetNodeByLocation(location.ToNorth(), taikyoku.GobanBounds));//北隣の場所を追加。
                            }
                        }
                        // 東隣を調べる
                        if (!location.IsEastEnd(taikyoku.GobanBounds))//東端ではないなら。
                        {
                            if (taikyoku.Goban.EastOf(location) == color)
                            {
                                neswNodes.Add(Util_AboutBoard.GetNodeByLocation(location.ToEast(), taikyoku.GobanBounds));
                            }
                        }
                        // 南隣を調べる
                        if (!location.IsSouthEnd(taikyoku.GobanBounds))//南端ではないなら
                        {
                            if (taikyoku.Goban.SouthOf(location) == color)
                            {
                                neswNodes.Add(Util_AboutBoard.GetNodeByLocation(location.ToSouth(), taikyoku.GobanBounds));
                            }
                        }
                        // 西隣を調べる
                        if (!location.IsWestEnd())//西端ではないなら。
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
        /// 指定した交点（ａ）から、四方に接続している石を全て数え上げて、返します。
        /// 
        /// ａの交点は、人間がコンソールから入力して指定します。
        /// </summary>
        /// <param name="out_connectedStones_asNode">Gnugo1.2 では、listpt という名前のグローバル配列。</param>
        /// <param name="humanInput_location">Gnugo1.2 では、i,j という変数名。人間がコンソールに入力した交点が入っている。</param>
        /// <param name="list_neswStones_asNode_eachPoint">Gnugo1.2 では、 movelist という名称。</param>
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
            // この交点は数えたかどうかのフラグです。
            //
            // Gnugo1.2 では color という変数名だったが、石の色と紛らわしいので名称を変更した。
            //
            CheckFlag_Ending[] counted_onBoard = new CheckFlag_Ending[taikyoku.GobanBounds.Nodes];

            //
            // 全ての交点を、まだ数えていないという設定にします。
            //
            for (int node = 0; node < taikyoku.GobanBounds.Nodes; node++)
            {
                counted_onBoard[node] = CheckFlag_Ending.Yet;
            }

            //
            // 人間がコンソール入力した交点にある石と、四方がつながっている同じ色の石が
            // このキューに入っては出て、を連鎖します。
            //
            // Gnugo1.2 では、キューも自作していたが、C#盤では 既存のものを使うように変更。
            //
            Queue queue_connectedNodes = new Queue(150);

            // 人間が指定した交点は チェック済みにし、キューに追加します。
            counted_onBoard[Util_AboutBoard.GetNodeByLocation(humanInput_location, taikyoku.GobanBounds)] = CheckFlag_Ending.Working;
            queue_connectedNodes.Enqueue(Util_AboutBoard.GetNodeByLocation(humanInput_location, taikyoku.GobanBounds));
#if DEBUG 
            Console.WriteLine("Survived first enqueue in bfslist.");
#endif

            //
            // キューが空になるまで、処理の連鎖を繰り返します。
            //
            while (queue_connectedNodes.Count != 0)
            {
                // まず、人間に指定された交点。（ループの２週目からは、その隣接する石）
                int node_a = (int)queue_connectedNodes.Dequeue();

                // 指定された交点の、存在する北東南西隣の石。（既に調べて配列に入れてあります）
                for (int nesw = 0; nesw < list_neswStones_asNode_eachPoint[node_a].Count; nesw++)//
                {
                    // その北東南西隣の石について
                    int node_b = list_neswStones_asNode_eachPoint[node_a][nesw];

                    // まだ数えていなければ
                    if (counted_onBoard[node_b] == CheckFlag_Ending.Yet)
                    {
                        // その交点には作業中フラグを立て、
                        // その交点（隣の石）を、次に判定する石としてキューに追加します。連鎖的な動きになります。
                        counted_onBoard[node_b] = CheckFlag_Ending.Working;
                        queue_connectedNodes.Enqueue(node_b); // 
                    }
                }
#if DEBUG 
                Console.WriteLine("Just prior to first dequeue!.");
#endif

                // 人間に指定された交点（ループの２週目からは、その隣接する石）は、
                // 数え上げる対象から外し、返却用のリストに追加します。
                counted_onBoard[node_a] = CheckFlag_Ending.Finished;
                out_connectedStones_asNode.Add( node_a); 
            }
        }

        /// <summary>
        /// 死んでいる石のつながりを、盤上から削除します。
        /// 
        /// Gnugo1.2 では、endgame関数。
        /// </summary>
        /// <param name="taikyoku"></param>
        public static void Remove_DeadPieces(Taikyoku taikyoku)
        {
            bool isContinuePhase; // Gnugo1.2 では cont という変数名。continueの略だろうか？

            //
            // 最初にコンピューター側を調べ、次に人間側を調べます。
            // 盤上の各点の石が、どの方向に隣接する石を持っているかを調べています。
            //

            // コンピューター側
            Dictionary<int, List<int>> myList_neswStones_asNode_eachPoint;// Gnugo1.2 では、mymovelist という変数名。int mymovelist[NODES][5]。
            Util_EndOfGame.GetNeswStones_AsNode_EachPoint(taikyoku.MyColor, out myList_neswStones_asNode_eachPoint, taikyoku);

            // 人間側
            Dictionary<int, List<int>> yourList_neswStones_asNode_eachPoint;// Gnugo1.2 では、umovelist という変数名。int umovelist[NODES][5];。
            Util_EndOfGame.GetNeswStones_AsNode_EachPoint(taikyoku.YourColor, out yourList_neswStones_asNode_eachPoint, taikyoku);

            isContinuePhase = true;
            GobanPoint location;// Gnugo1.2 では、 i, j という変数名。
            do
            {
                Console.Write("Dead piece? ");

                // 死んでいる石の場所を入力してください。
                string an_str = Console.ReadLine();
                //scanf("%s", an);

                //----------------------------------------
                // 妥当な入力があるまで、くるくる回します。
                //----------------------------------------
                {//2015-11-27 追加
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
                        // 入力が不妥当なので、ループし直し。
                        goto gt_DeadPieceContinue;
                    }
                }

                if (an_str == "stop")
                {
                    // ループから抜けます。
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
                        // 示された場所を空にしていき、コンピューターの取られた石カウントを増やします。
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

                        // 示された場所を空にしていき、人間の取られた石カウントを増やします。
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
        /// どちらの地（陣地）でもない交点を、黒か白の石で交互に埋めます。
        /// </summary>
        public static void FillNeutralTerritories
        (
            Taikyoku taikyoku
        )
        {
            bool isContinuePhase = true; // Gnugo1.2 では cont という変数名。continueの略だろうか？
            int youMeCounter = 0;//白黒を反転するためのカウンター

            GobanPoint location;// Gnugo1.2 では、 i, j という変数名。
            do
            {
                if (youMeCounter % 2 == 0)
                {
                    //
                    // 指定した場所を、コンピューター側の石に置き換える処理です。
                    //
                    Console.Write("Your piece? ");

                    string an_str = Console.ReadLine();
                    //scanf("%s", an);

                    //----------------------------------------
                    // 妥当な入力があるまで、くるくる回します。
                    //----------------------------------------
                    {//2015-11-27 追加
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
                            // 入力が不妥当なので、ループし直し。
                            goto gt_PlayerPieceContinue;
                        }
                    }
                    // ここで入力が妥当と分かった。このあと処理。

                    if (an_str == "stop")
                    {
                        // ループから抜けます。
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
                    // 指定した場所を、相手（人間）の石に置き換える処理です。
                    //
                    Console.Write("My piece? ");
                    string an_str = Console.ReadLine();
                    //scanf("%s", an);

                    //----------------------------------------
                    // 妥当な入力があるまで、くるくる回します。
                    //----------------------------------------
                    {//2015-11-27 追加
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
                            // 入力が不妥当なので、ループし直し。
                            goto gt_PlayerPieceContinue;
                        }
                    }
                    // ここで入力が妥当と分かった。このあと処理。

                    if (an_str == "stop")
                    {
                        // ループから抜けます。
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
        /// 空きスペースを、どちらの陣地か判定し、その陣地の石に変換します。
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
        /// 盤上の石を数えます。
        /// </summary>
        public static void CountPieces
        (
            out int out_myTotal, // Gnugo1.2 では mtot という変数名。my total の略だろうか？
            out int out_yourTotal, // Gnugo1.2 では utot という変数名。your total の略だろうか？
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