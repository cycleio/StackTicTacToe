using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools.Utils;

namespace StackTicTacToe.Tests
{
    public class LookAtTest
    {
        GameObject obj;

        [SetUp]
        public void SetUp()
        {
            obj = new GameObject("LookAtTest", typeof(LookAt));
            obj.transform.position = Vector3.zero;
        }

        [Test]
        public void LookAt([Random(3)]float x, [Random(3)]float y, [Random(3)]float z)
        {
            Vector3 toLook = new Vector3(x, y, z);
            obj.GetComponent<LookAt>().LookAtPosition(toLook);
            Assert.IsTrue(Vector3EqualityComparer.Instance.Equals(Vector3.zero, obj.transform.position));
            Assert.IsTrue(QuaternionEqualityComparer.Instance.Equals(Quaternion.LookRotation(toLook), obj.transform.rotation));
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(obj);
        }
    }
}
