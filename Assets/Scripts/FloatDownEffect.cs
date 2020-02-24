using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class FloatDownEffect : MonoBehaviour, IEffect
{
    [SerializeField] private float floatDownSec = 2.0f;
    [SerializeField] private float floatingHeight = 1.0f;

    public void MakeEffect()
    {
        var aimingPosition = transform.position;
        var initialPosition = aimingPosition + Vector3.up * floatingHeight;
        transform.position = initialPosition;

        var currentTime = 0f;
        SingleAssignmentDisposable disposable = new SingleAssignmentDisposable();
        disposable.Disposable = Observable.EveryFixedUpdate()
            .Subscribe(_ =>
            {
                currentTime += Time.fixedDeltaTime;
                var progress = currentTime / floatDownSec;
                transform.position = Vector3.Lerp(initialPosition, aimingPosition, progress);
                if(progress >= 1.0f)
                {
                    transform.position = aimingPosition;
                    disposable.Dispose();
                }
            });
    }

    void Start()
    {
        MakeEffect();
    }
}
