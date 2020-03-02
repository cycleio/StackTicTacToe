using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class AppearingEffectTest
    {
        [SerializeField] private GameObject prefab;
        GameObject obj;
        MeshRenderer meshRenderer;

        [SetUp]
        public void SetUp()
        {
            obj = UnityEngine.TestTools.Utils.Utils.CreatePrimitive(PrimitiveType.Cube);
            obj.AddComponent<AppearingEffect>();
            meshRenderer = obj.GetComponent<MeshRenderer>();
            meshRenderer.material = new Material(meshRenderer.material);
            // materialがうまく設定できない
            // alphaが動かない -> test失敗する
        }

        [Test]
        public void SetInitialAlpha()
        {
            Debug.Log(meshRenderer.material);
            Assert.That(meshRenderer.material.color.a, Is.InRange(-float.Epsilon, float.Epsilon));
        }

        [UnityTest]
        [Timeout(2500)]
        public IEnumerator Appearing()
        {
            float colorA = meshRenderer.material.color.a;

            // wait for effect ends
            while (meshRenderer.material.color.a - colorA > float.Epsilon)
            {
                colorA = meshRenderer.material.color.a;
                yield return new WaitForFixedUpdate();
            }

            Assert.That(meshRenderer.material.color.a, Is.GreaterThanOrEqualTo(1.0f));
        }

        [TearDown]
        public void TearDown()
        {
            Object.Destroy(obj);
        }
    }
}
