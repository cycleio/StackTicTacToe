using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackTicTacToe
{
    public class LightPillar : MonoBehaviour
    {
        private GameObject child;

        public Vector2Int Position { get; private set; }

        public void Initialize(Vector2Int position)
        {
            child = transform.GetChild(0).gameObject;
            Position = position;
        }

        public void Enable()
        {
            child.SetActive(true);
        }

        public void Disable()
        {
            child.SetActive(false);
        }
    }
}
