using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools.Utils;

namespace StackTicTacToe.Tests
{
    public class BoardFactoryTest
    {
        private GameObject board;
        private BoardFactory boardFactory;

        [SetUp]
        public void SetUp()
        {
            board = new GameObject("BoardFactoryTest", typeof(BoardFactory));
            boardFactory = board.GetComponent<BoardFactory>();
        }

        [Test]
        public void Create([NUnit.Framework.Range(1, 5)] int w, [NUnit.Framework.Range(1, 5)] int h)
        {
            boardFactory.Create(w, h);
            Assert.AreEqual(w + h + 2, board.transform.childCount - 1);
            for (int i = 0; i < board.transform.childCount; ++i)
            {
                var child = board.transform.GetChild(i).gameObject;
                if (child.name == "BoardBase")
                {
                    Assert.IsTrue(FloatEqualityComparer.Instance.Equals(w, child.transform.localScale.x));
                    Assert.IsTrue(FloatEqualityComparer.Instance.Equals(h, child.transform.localScale.z));
                }
                else
                {
                    if (child.transform.localScale.x > 0.01)
                    {
                        Assert.IsTrue(FloatEqualityComparer.Instance.Equals(0, child.transform.position.x));
                    }
                    else if (child.transform.localScale.z > 0.01)
                    {
                        Assert.IsTrue(FloatEqualityComparer.Instance.Equals(0, child.transform.position.z));
                    }
                    else
                    {
                        Assert.IsTrue(false, $"Scale/Position of {i}-th child (Line) is not valid.");
                    }
                }
            }
        }

        [TearDown]
        public void TearDown()
        {
            boardFactory = null;
            Object.DestroyImmediate(board);
        }
    }
}
