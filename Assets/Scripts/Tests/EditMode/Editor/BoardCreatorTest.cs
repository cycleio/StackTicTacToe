using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Assert = UnityEngine.Assertions.Assert;

namespace Tests
{
    public class BoardCreatorTest
    {
        private GameObject board;
        private BoardCreator boardCreator;

        [SetUp]
        public void SetUp()
        {
            board = new GameObject("Board", typeof(BoardCreator));
            boardCreator = board.GetComponent<BoardCreator>();
        }

        [Test]
        public void Create([NUnit.Framework.Range(1, 5)] int w, [NUnit.Framework.Range(1, 5)] int h)
        {
            boardCreator.Create(w, h);
            Assert.AreEqual(w + h + 2, board.transform.childCount - 1);
            for (int i = 0; i < board.transform.childCount; ++i)
            {
                var child = board.transform.GetChild(i).gameObject;
                if (child.name == "BoardBase")
                {
                    Assert.AreApproximatelyEqual(w, child.transform.localScale.x);
                    Assert.AreApproximatelyEqual(h, child.transform.localScale.z);
                }
                else
                {
                    if (child.transform.localScale.x > 0.01)
                    {
                        Assert.AreApproximatelyEqual(0, child.transform.position.x);
                    }
                    else if (child.transform.localScale.z > 0.01)
                    {
                        Assert.AreApproximatelyEqual(0, child.transform.position.z);
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
            boardCreator = null;
            Object.DestroyImmediate(board);
        }
    }
}
