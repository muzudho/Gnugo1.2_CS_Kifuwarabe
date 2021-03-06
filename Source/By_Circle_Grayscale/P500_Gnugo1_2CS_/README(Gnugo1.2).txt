                GNUGO - the game of Go (Wei-Chi)
               Version 1.2   last revised 10-31-95
         Copyright (C) Free Software Foundation, Inc.
                    written by Man L. Li
                    modified by Wayne Iba
        modified by Frank Pursel <fpp%minor.UUCP@dragon.com>
                   documented by Bob Webber

                        Introduction

This program is a minor modification of the GNUGO (Version 1.1).  Main
change in this version is the removing of any system dependent functions
to conform to ISO Standard C (ANSI C).  This software is an attempt to
distribute a free program to play Go.  The idea of this program is based
on the article "Programming the Game of Go. Byte, Vol.6 No.4" by
J. K. Millen.

Currently, this program only understands basic Go rules and skills.
It counts the number of liberty for each board piece.  Computer move is
generated by choosing among several possible moves to attack the opponent,
defense own pieces and match playing patterns.  If no good move is found
then random move will be generated.  It doesn't have the concept of eye
although it will try to form one.

The program is written in C running on UNIX and DOS in TTY mode.  A
variation of this program NeXTGo running on NeXTStep has been developed
by John Neil (neil@math.mth.pdx.edu).  Future versions of this program
will include graphical user interfaces on X-Window and MS-Windows
systems.

You are encouraged to send in enhencement, suggestion, bug/fix for this
program.  Future release can be obtained from Free Software Foundation.

                        Installation

This package contains the following files:

README - this document
COPYING - GNU general public license
Documentation - description of each function
ChangeLog - modification records
Makefile - file to compile GNUGO program on UNIX
gnugo.mak - file to compile GNUGO program on DOS with Microsoft
            C/C++ compiler
objs - linking list used by gnugo.mak
count.c - count liberty of one piece
countlib.c - count liberty of pieces
endgame.c - bookkeeping at end of game
eval.c - evaluate liberty
exambord.c - update game board
findcolr.c - find connected pieces of the same color
findnext.c - find move to defense against attack and function to
             evaluate move
findopen.c - find opponent liberty and choose move to attack
findpatn.c - match play patterns for next move
findsavr.c - check own weakness to defense
findwinr.c - find opponent weakness to attack
fioe.c - check if fill in its own eye
genmove.c - main function to generate computer move
getij.c - convert move string to board position
getmove.c - read move or command from human player
initmark.c - initialize marking array
main.c - GNUGO main program
matchpat.c - match play pattern
opening.c - generate game opening moves
openregn.c - check open region
sethand.c - setup handicap pieces
showbord.c - show GO board and stone positions
showinst.c - show instruction on game playing
suicide.c - check illegal move if suicide
gnugo.h - general definitions and function prototypes
patterns.h - playing patterns

To install GNUGO on UNIX system, use the command

   make

To install GNUGO on DOS with Microsoft C/C++ compiler, use the command

   nmake -f gnugo.mak

                          Copyright

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

