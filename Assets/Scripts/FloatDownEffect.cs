using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class FloatDownEffect : AbstractEffect
{
    [SerializeField] private float floatingHeight = 1.0f;

    private Vector3 initialPosition, aimingPosition;

    protected override void SetUp()
    {
        aimingPosition = transform.position;
        initialPosition = aimingPosition + Vector3.up * floatingHeight;
        transform.position = initialPosition;
    }

    protected override void Effect(float timeFromStart)
    {
        var progress = Mathf.Sin(Mathf.PI * timeFromStart / effectSeconds / 2);
        transform.position = Vector3.Lerp(initialPosition, aimingPosition, progress);
    }

    protected override void TearDown()
    {
        transform.position = aimingPosition;
    }
}
