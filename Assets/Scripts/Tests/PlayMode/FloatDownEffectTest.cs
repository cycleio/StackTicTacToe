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
            //initialPosition = new Vector3(2, 5, 3);
            obj = new GameObject("Tester", typeof(FloatDownEffect));
            obj.transform.position = initialPosition;
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
                Debug.Log(posY);
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
