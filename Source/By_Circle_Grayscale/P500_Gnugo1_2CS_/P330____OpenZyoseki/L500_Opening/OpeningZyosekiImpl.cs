/**
 * C# Arrange of GNUGO 1.2
 * 
 * modified by Muzudho
 * last modified 2015-11-28
 *  
 * opening.c -> OpeningZyosekiImpl.cs
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
using Grayscale.GPL.P___405_CompZyoseki.L500_FindPattern;
using Grayscale.GPL.P160____Collection_.L500_Collection;

namespace Grayscale.GPL.P330____OpenZyoseki.L500_Opening
{
    /// <summary>
    /// �O���t�\���ɂȂ��Ă���m�[�h�B
    /// ���Ւ�΂��������߂�̂Ɏg���B
    /// 
    /// Gnugo1.2 �ł� opening.c �� tnode �\���́B
    /// </summary>
    class ZyosekiNode
    {
        /// <summary>
        /// Gnugo1.2 �ł́Ai,j �v���p�e�B�[�B
        /// </summary>
        public GobanPoint P{get;set;}

        /// <summary>
        /// �v�f����[8]�B���̒�Δԍ��B
        /// </summary>
        public int[] Next{get;set;}

        public ZyosekiNode(
            GobanPoint p,
            int[] next
            )
        {
            this.P = p;
            this.Next = next;
        }
    }

    /// <summary>
    /// ���Ւ�Ղ�I�����܂��B
    /// </summary>
    public class OpeningZyosekiImpl
    {
        /// <summary>
        /// ���Ղ̒�΁B�c���[��ɂȂ��Ă��邪�A�\���͔z��A�g�����̓����N�E���X�g�B
        /// </summary>
        private static ZyosekiNode[] ZyosekiTree = new ZyosekiNode[]
        {
            new ZyosekiNode(new GobanPointImpl(-1,-1), new int[]{ 1, 2, 3, 4, 5, 6, 7, 20}),   // 0 ����B���� 1�`7�A20�ԂցB
            new ZyosekiNode(new GobanPointImpl( 2, 3), new int[]{ 8, 9}                   ),   // 1
            new ZyosekiNode(new GobanPointImpl( 2, 4), new int[]{10}                      ),
            new ZyosekiNode(new GobanPointImpl( 3, 2), new int[]{11, 12}                  ),
            new ZyosekiNode(new GobanPointImpl( 3, 3), new int[]{14, 15, 16, 17, 18, 19}  ),
            new ZyosekiNode(new GobanPointImpl( 3, 4), new int[]{10}                      ),    // 5
            new ZyosekiNode(new GobanPointImpl( 4, 2), new int[]{13}                      ),
            new ZyosekiNode(new GobanPointImpl( 4, 3), new int[]{13}                      ),
            new ZyosekiNode(new GobanPointImpl( 4, 2), new int[]{}                        ),
            new ZyosekiNode(new GobanPointImpl( 4, 3), new int[]{}                        ),
            new ZyosekiNode(new GobanPointImpl( 3, 2), new int[]{}                        ),    // 10
            new ZyosekiNode(new GobanPointImpl( 2, 4), new int[]{}                        ),
            new ZyosekiNode(new GobanPointImpl( 3, 4), new int[]{}                        ),
            new ZyosekiNode(new GobanPointImpl( 2, 3), new int[]{}                        ),   // 13�B��΂͂����ŏI���B
            new ZyosekiNode(new GobanPointImpl( 2, 5), new int[]{10}                      ),
            new ZyosekiNode(new GobanPointImpl( 2, 6), new int[]{10}                      ),    // 15
            new ZyosekiNode(new GobanPointImpl( 3, 5), new int[]{10}                      ),
            new ZyosekiNode(new GobanPointImpl( 5, 2), new int[]{13}                      ),
            new ZyosekiNode(new GobanPointImpl( 5, 3), new int[]{13}                      ),
            new ZyosekiNode(new GobanPointImpl( 6, 2), new int[]{13}                      ),
            new ZyosekiNode(new GobanPointImpl( 2, 2), new int[]{}                        ) // 20
        };

        /// <summary>
        /// �Q�[���c���[���珘�Ղ̂��߂̓������擾���܂��B
        /// </summary>
        /// <param name="p"></param>
        /// <param name="ref_nodeNo">Gnugo1.2 �ł� ����cnd�B</param>
        /// <param name="moveType"></param>
        /// <param name="taikyoku"></param>
        /// <returns></returns>
        public static bool Opening
        (
            out GobanPoint p,
            ref int ref_nodeNo,
            MoveType moveType,
            Taikyoku taikyoku
        )
        {
            int m;
            p = new GobanPointImpl();

            /* get i, j */
            if (moveType == MoveType.Inverted || moveType == MoveType.Inverted_And_Reflected)
            {
                p.I = taikyoku.GobanBounds.BoardEnd - OpeningZyosekiImpl.ZyosekiTree[ref_nodeNo].P.I;   /* inverted */
            }
            else
            {
                p.I = OpeningZyosekiImpl.ZyosekiTree[ref_nodeNo].P.I;
            }

            if (moveType == MoveType.Reflected || moveType == MoveType.Inverted_And_Reflected)
            {
                p.J = taikyoku.GobanBounds.BoardEnd - OpeningZyosekiImpl.ZyosekiTree[ref_nodeNo].P.J;   /* reflected */
            }
            else
            {
                p.J = OpeningZyosekiImpl.ZyosekiTree[ref_nodeNo].P.J;
            }

            if (OpeningZyosekiImpl.ZyosekiTree[ref_nodeNo].Next.Length != 0) // ��΂ɁA���̎w���肪�܂�����Ȃ�B
            {
                // ���ɂǂ̒�΂�i�ނ��̓����_���i���m���j�B
                m = taikyoku.Random.Next() % OpeningZyosekiImpl.ZyosekiTree[ref_nodeNo].Next.Length;
                ref_nodeNo = OpeningZyosekiImpl.ZyosekiTree[ref_nodeNo].Next[m]; // ��΃O���t�}�̐V�����m�[�h�ԍ��ցB
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
