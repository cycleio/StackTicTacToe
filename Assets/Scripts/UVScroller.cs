using UnityEngine;
using System.Collections;

namespace StackTicTacToe
{
    [RequireComponent(typeof(MeshRenderer))]
    public class UVScroller : MonoBehaviour
    {
        [SerializeField] private float scrollSpeed = 1.0f;
        [SerializeField] private Vector2 mainOffset = Vector2.zero;

        private Material material;

        private void Start()
        {
            material = GetComponent<MeshRenderer>().material;
        }

        void Update()
        {
            material.SetTextureOffset("_MainTex", mainOffset * Time.time * scrollSpeed);
        }
    }
}
