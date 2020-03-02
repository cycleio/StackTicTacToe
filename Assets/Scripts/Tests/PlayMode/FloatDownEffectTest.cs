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
        Vector3 initialPosition;

        [SetUp]
        public void SetUp()
        {
            initialPosition = new Vector3(2, 5, 3);
            obj = new GameObject("FloatDownEffectTest", typeof(FloatDownEffect));
            obj.transform.position = initialPosition;
        }

        [Test]
        public void SetYPosition()
        {
            Assert.That(obj.transform.position.y, Is.GreaterThan(4));
        }

        [UnityTest]
        [Timeout(2500)]
        public IEnumerator FloatDown()
        {
            var transform = obj.transform;
            float posY = transform.position.y;

            // wait for effect ends
            while (posY - initialPosition.y > float.Epsilon)
            {
                posY = transform.position.y;
                yield return new WaitForFixedUpdate();
            }

            Assert.That(posY - initialPosition.y, Is.LessThanOrEqualTo(float.Epsilon));
        }

        [UnityTearDown]
        public void TearDown()
        {
            Object.Destroy(obj);
        }
    }
}
