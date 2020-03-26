using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace StackTicTacToe.Tests
{
    public class BoardSystemTest
    {
        BoardSystem boardSystem;

        [SetUp]
        public void SetUp()
        {
            boardSystem = new BoardSystem();
        }

        [TestCase(false, -1, 0, 1, 1)]
        [TestCase(false, 3, 2, 0, 1)]
        [TestCase(false, 3, 2, 4, 0)]
        [TestCase(false, 3, 2, 4, 5)]
        [TestCase(true, 3, 4, 5, 3)]
        [TestCase(true, 3, 3, 3, 3)]
        [TestCase(true, 1, 2, 3, 3)]
        [TestCase(true, 3, 1, 1, 3)]
        public void IsBoardSizeValidTest(bool expected, int boardSizeX, int boardSizeY, int boardSizeZ, int goalNum)
        {
            Vector3Int boardSize = new Vector3Int(boardSizeX, boardSizeY, boardSizeZ);
            Assert.AreEqual(expected, BoardSystem.IsBoardSizeValid(boardSize, goalNum));
        }

        [Test]
        public void GetTurnTest()
        {
            boardSystem.Initialize(new Vector3Int(3, 5, 3), 1);

            Assert.That(boardSystem.GetTurn(), Is.EqualTo(BoardSystem.CellColor.Blue));
            boardSystem.Place(0, 0);
            Assert.That(boardSystem.GetTurn(), Is.EqualTo(BoardSystem.CellColor.Red));
        }

        [Test]
        public void IsPlacableTest([NUnit.Framework.Range(1, 5)]int height)
        {
            boardSystem.Initialize(new Vector3Int(1, height, 1), height);

            for (int i = 0; i < height; i++)
            {
                Assert.IsTrue(boardSystem.IsPlacable(0, 0));
                boardSystem.Place(0, 0);
            }

            Assert.IsFalse(boardSystem.IsPlacable(0, 0));
        }

        [Test]
        public void GetWinnerTest_1Cell()
        {
            boardSystem.Initialize(new Vector3Int(3, 5, 3), 1);

            boardSystem.Place(0, 0);
            Assert.IsTrue(boardSystem.GetWinner() == BoardSystem.CellColor.Blue);
        }

        [TestCase(0, 0, 0, 1, 1, 0, Description = "X")]
        [TestCase(0, 0, 0, 1, 0, 0, Description = "Y")]
        [TestCase(0, 0, 1, 0, 0, 1, Description = "Z")]
        [TestCase(0, 0, 1, 0, 1, 0, Description = "Diagonal(x+/y+/z0)")]
        [TestCase(0, 1, 0, 0, 0, 0, Description = "Diagonal(x+/y-/z0)")]
        [TestCase(0, 0, 0, 1, 0, 1, Description = "Diagonal(x0/y+/z+)")]
        [TestCase(0, 1, 0, 0, 0, 0, Description = "Diagonal(x0/y-/z+)")]
        [TestCase(0, 0, 0, 1, 1, 1, Description = "Diagonal(x+/y0/z+)")]
        [TestCase(0, 1, 0, 0, 1, 0, Description = "Diagonal(x-/y0/z+)")]
        [TestCase(0, 0, 1, 1, 1, 1, Description = "Diagonal(x+/y+/z+)")]
        [TestCase(1, 1, 0, 0, 0, 0, Description = "Diagonal(x+/y-/z+)")]
        [TestCase(0, 1, 1, 0, 1, 0, Description = "Diagonal(x-/y+/z+)")]
        [TestCase(1, 0, 0, 1, 0, 1, Description = "Diagonal(x-/y-/z+)")]
        public void GetWinnerTest_2Cells_BlueWin(int x1, int y1, int x2, int y2, int x3, int y3)
        {
            boardSystem.Initialize(new Vector3Int(4, 5, 4), 2);

            boardSystem.Place(x1, y1);
            Assert.IsTrue(boardSystem.GetWinner() == BoardSystem.CellColor.None);
            boardSystem.Place(x2, y2);
            Assert.IsTrue(boardSystem.GetWinner() == BoardSystem.CellColor.None);
            boardSystem.Place(x3, y3);
            Assert.IsTrue(boardSystem.GetWinner() == BoardSystem.CellColor.Blue);
        }

        [TestCase(0, 0, 0, 1, 1, 0, Description = "X")]
        [TestCase(0, 0, 0, 1, 0, 0, Description = "Y")]
        [TestCase(0, 0, 1, 0, 0, 1, Description = "Z")]
        [TestCase(0, 0, 1, 0, 1, 0, Description = "Diagonal(x+/y+/z0)")]
        [TestCase(0, 1, 0, 0, 0, 0, Description = "Diagonal(x+/y-/z0)")]
        [TestCase(0, 0, 0, 1, 0, 1, Description = "Diagonal(x0/y+/z+)")]
        [TestCase(0, 1, 0, 0, 0, 0, Description = "Diagonal(x0/y-/z+)")]
        [TestCase(0, 0, 0, 1, 1, 1, Description = "Diagonal(x+/y0/z+)")]
        [TestCase(0, 1, 0, 0, 1, 0, Description = "Diagonal(x-/y0/z+)")]
        [TestCase(0, 0, 1, 1, 1, 1, Description = "Diagonal(x+/y+/z+)")]
        [TestCase(1, 1, 0, 0, 0, 0, Description = "Diagonal(x+/y-/z+)")]
        [TestCase(0, 1, 1, 0, 1, 0, Description = "Diagonal(x-/y+/z+)")]
        [TestCase(1, 0, 0, 1, 0, 1, Description = "Diagonal(x-/y-/z+)")]
        public void GetWinnerTest_2Cells_RedWin(int x2, int y2, int x3, int y3, int x4, int y4)
        {
            boardSystem.Initialize(new Vector3Int(4, 5, 4), 2);

            boardSystem.Place(3, 3);
            Assert.IsTrue(boardSystem.GetWinner() == BoardSystem.CellColor.None);
            boardSystem.Place(x2, y2);
            Assert.IsTrue(boardSystem.GetWinner() == BoardSystem.CellColor.None);
            boardSystem.Place(x3, y3);
            Assert.IsTrue(boardSystem.GetWinner() == BoardSystem.CellColor.None);
            boardSystem.Place(x4, y4);
            Assert.IsTrue(boardSystem.GetWinner() == BoardSystem.CellColor.Red);
        }

        [Test]
        public void IsFilledTest()
        {
            boardSystem.Initialize(new Vector3Int(1, 2, 1), 2);

            boardSystem.Place(0, 0);
            boardSystem.Place(0, 0);
            Assert.IsTrue(boardSystem.IsFilled());
        }
    }
}
