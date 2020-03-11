using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;

namespace StackTicTacToe
{
    public class TitleManager : MonoBehaviour
    {
        private IInputUniRx inputUniRx;

        [Inject]
        private void Initialize(IInputUniRx _inputUniRx)
        {
            inputUniRx = _inputUniRx;
        }

        void Start()
        {
            inputUniRx.Initialize();
            inputUniRx.Tap
                .Where(_ => !SceneMover.Instance.IsSceneMoving)
                .Subscribe(_ =>
                {
                    inputUniRx.Dispose();
                    SceneMover.Instance.ChangeScene("MainScene");
                })
                .AddTo(gameObject);
        }
    }
}
