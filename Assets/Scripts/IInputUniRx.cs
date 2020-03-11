using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace StackTicTacToe
{
    /// <summary>
    /// UniRxを用いた入力用インターフェース
    /// </summary>
    public interface IInputUniRx : System.IDisposable
    {
        System.IObservable<Vector2> Position { get; }
        System.IObservable<Unit> Tap { get; }

        void Initialize();
    }
}
