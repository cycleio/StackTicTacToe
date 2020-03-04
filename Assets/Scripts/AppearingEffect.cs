using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// 出現エフェクト
/// MeshRendererのMaterialのcolor、alphaを0から元の値へ
/// 要：Materialを透明化可能にすること
/// </summary>
public class AppearingEffect : AbstractEffect
{
    private Color initialColor, aimingColor;
    private Material material;

    protected override void SetUp()
    {
        material = GetComponent<MeshRenderer>().material;
        aimingColor = material.color;
        initialColor = new Color(aimingColor.r, aimingColor.g, aimingColor.b, 0f);
        material.color = initialColor;
    }

    protected override void Effect(float timeFromStart)
    {
        var progress = timeFromStart / effectSeconds;
        material.color = Color.Lerp(initialColor, aimingColor, progress);
    }

    protected override void TearDown()
    {
        material.color = aimingColor;
    }
}
