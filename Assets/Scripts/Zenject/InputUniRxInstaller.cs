using UnityEngine;
using Zenject;

namespace StackTicTacToe
{
    public class InputUniRxInstaller : MonoInstaller<InputUniRxInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IInputUniRx>()
                .To<InputUniRxByInputSystem>()
                .FromNew()
                .AsCached();
        }
    }
}
