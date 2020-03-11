using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools.Utils;

namespace StackTicTacToe.Tests
{
    public class LightPillarFactoryTest
    {
        GameObject obj;
        LightPillarFactory lightPillarFactory;

        [SetUp]
        public void SetUp()
        {
            obj = new GameObject("LightPillarCreatorTest", typeof(LightPillarFactory));
            lightPillarFactory = obj.GetComponent<LightPillarFactory>();
        }

        [Test]
        public void Create_Num([Random(1, 5, 3)]int w, [Random(1, 5, 3)]int h)
        {
            lightPillarFactory.Create(w, h);
            Assert.That(lightPillarFactory.transform.childCount, Is.EqualTo(w * h));
        }

        [Test]
        public void Create_Pos([Random(1, 5, 3)]int w, [Random(1, 5, 3)]int h)
        {
            lightPillarFactory.Create(w, h);

            for(int i = 0; i < w; ++i)
            {
                for(int j = 0; j < h; ++j)
                {
                    var pos = lightPillarFactory.transform.GetChild(j + i * h).position;
                    Assert.IsTrue(FloatEqualityComparer.Instance.Equals(0.5f * -(w - 1) + i, pos.x));
                    Assert.IsTrue(FloatEqualityComparer.Instance.Equals(0.5f * -(h - 1) + j, pos.z));
                }
            }
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(obj);
        }
    }
}
