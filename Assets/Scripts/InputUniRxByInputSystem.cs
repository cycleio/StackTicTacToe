using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UniRx;

namespace StackTicTacToe
{
    /// <summary>
    /// InputSystemの入力をUniRxのObserverに変える薄いラッパー
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    public class InputUniRxByInputSystem : IInputUniRx, InputControl.IMainActions, System.IDisposable
    {
        public System.IObservable<Vector2> Position { get { return position.AsObservable(); } }
        public System.IObservable<Unit> Tap { get { return tap.AsObservable(); } }

        private Vector2ReactiveProperty position = new Vector2ReactiveProperty();
        private Subject<Unit> tap = new Subject<Unit>();
        private InputControl inputControl;

        public void Initialize()
        {
            inputControl = new InputControl();
            inputControl.Main.SetCallbacks(this);

            inputControl.Enable();
        }

        public void Dispose()
        {
            inputControl.Disable();
            inputControl.Dispose();
            inputControl = null;
        }

        /// <summary>
        /// Pointerが移動したときPositionにスクリーン上の位置を通知
        /// </summary>
        /// <param name="context">CallbackContext</param>
        public void OnPointerMove(InputAction.CallbackContext context)
        {
            position.SetValueAndForceNotify(context.ReadValue<Vector2>());
        }

        /// <summary>
        /// クリック/タップされたときTapに通知
        /// </summary>
        /// <param name="context">CallbackContext</param>
        public void OnTap(InputAction.CallbackContext context)
        {
            if(context.interaction is TapInteraction && context.performed)
            {
                tap.OnNext(Unit.Default);
            }
        }
    }
}
