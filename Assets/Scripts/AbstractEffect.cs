using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// 時間を要するエフェクト用のabstract class
/// StartのタイミングでSetUp()を呼ぶ
/// 一定時間(effectSeconds[sec])Effect()を呼び続ける
/// 終了時にTearDown()を呼ぶ
/// </summary>
public abstract class AbstractEffect : MonoBehaviour
{
    [SerializeField][Min(0.001f)] protected float effectSeconds = 1.0f;

    abstract protected void Effect(float timeFromStart);
    abstract protected void SetUp();
    abstract protected void TearDown();

    private void Start()
    {
        SetUp();

        float time = 0.0f;
        SingleAssignmentDisposable disposable = new SingleAssignmentDisposable();
        disposable.Disposable = Observable.EveryFixedUpdate()
            .Subscribe(_ =>
            {
                time += Time.fixedDeltaTime;
                Effect(time);

                if (time >= effectSeconds)
                {
                    TearDown();
                    disposable.Dispose();
                }
            });
    }
}
