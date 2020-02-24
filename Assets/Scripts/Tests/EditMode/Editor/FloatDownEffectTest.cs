using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class FloatDownEffectTest
    {
        GameObject obj;
        Vector3 initialPosition = new Vector3(2, 5, 3);

        [SetUp]
        public void SetUp()
        {
            obj = new GameObject("Tester");
            obj.transform.position = initialPosition;
            obj.AddComponent<FloatDownEffect>();
        }

        [Test]
        public void SetYPosition()
        {
            Assert.That(obj.transform.position.y, Is.GreaterThan(5));
        }
    }
}
