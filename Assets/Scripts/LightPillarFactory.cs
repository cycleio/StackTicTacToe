using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackTicTacToe
{
    /// <summary>
    /// 光の柱(カーソル相当のエフェクト用GameObject)を作成する
    /// 1マスの大きさを1[m]と仮定
    /// ボードの高さをboardBasePrefabから読む
    /// </summary>
    public class LightPillarFactory : MonoBehaviour
    {
        private readonly float lightPillarHeight = 1.5f; // lightPillarPrefabの高さ[m]

        [SerializeField] LightPillar lightPillarPrefab;
        [SerializeField] GameObject boardBasePrefab;

        /// <summary>
        /// 光の柱を作成する
        /// </summary>
        /// <param name="width">横方向のマス数</param>
        /// <param name="height">縦方向のマス数</param>
        public void Create(int width, int height)
        {
            var posY = lightPillarHeight / 2f + boardBasePrefab.transform.localScale.y;
            for (int i = 0; i < width; ++i)
            {
                for (int j = 0; j < height; ++j)
                {
                    var lightPillar = Instantiate(lightPillarPrefab,
                        new Vector3(0.5f * -(width - 1) + i, posY, 0.5f * -(height - 1) + j),
                        Quaternion.identity, transform);
                    lightPillar.Initialize(new Vector2Int(i, j));
                }
            }
        }
    }
}
