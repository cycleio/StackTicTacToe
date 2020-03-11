using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackTicTacToe
{
    public class LookAt : MonoBehaviour
    {
        [SerializeField] private Vector3 lookAtPosition = Vector3.zero;

        public void LookAtPosition(Vector3 pos)
        {
            transform.rotation = Quaternion.LookRotation(pos - transform.position);
        }

        private void Start()
        {
            LookAtPosition(lookAtPosition);
        }
    }
}
