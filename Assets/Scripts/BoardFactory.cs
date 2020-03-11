using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace StackTicTacToe
{
    /// <summary>
    /// ボードを作成する
    /// 1マスの大きさは1[m]で固定
    /// ボードの高さはPrefab依存
    /// </summary>
    public class BoardFactory : MonoBehaviour
    {
        private readonly float boardBaseMargin = 0.5f; // マスの外側の領域の幅
        private readonly float lineFloatingHeight = 0.02f; // ボードから線が浮かんでいる距離

        [SerializeField] private GameObject boardBasePrefab;
        [SerializeField] private GameObject linePrefab;

        /// <summary>
        /// ボードを作成する
        /// </summary>
        /// <param name="width">横方向のマス数</param>
        /// <param name="height">縦方向のマス数</param>
        public void Create(int width, int height)
        {
            Assert.IsTrue(width > 0);
            Assert.IsTrue(height > 0);

            CreateBoardBase(width, height);
            CreateLines(width, height);
        }

        private void CreateBoardBase(int width, int height)
        {
            var boardBase = Instantiate(boardBasePrefab, transform);
            var scale = boardBase.transform.localScale;
            scale.x = width + boardBaseMargin;
            scale.z = height + boardBaseMargin;
            boardBase.transform.localScale = scale;
        }

        private void CreateLines(int width, int height)
        {
            // memo: line(prefab)のwidth/heightは1マスの1/10
            var edgePosX = width / 2.0f;
            var edgePosZ = height / 2.0f;
            var linePosY = boardBasePrefab.transform.localScale.y + lineFloatingHeight;

            for (int i = 0; i <= width; ++i)
            {
                var obj = Instantiate(linePrefab,
                    transform.position + new Vector3(i - edgePosX, linePosY, 0),
                    Quaternion.identity, transform);
                var scale = obj.transform.localScale;
                scale.z *= height * 10 + 1; // +1: ふちに届かせるための補正
                obj.transform.localScale = scale;
            }
            for (int i = 0; i <= height; ++i)
            {
                var obj = Instantiate(linePrefab,
                    transform.position + new Vector3(0, linePosY, i - edgePosZ),
                    Quaternion.identity, transform);
                var scale = obj.transform.localScale;
                scale.x *= width * 10 + 1; // +1: ふちに届かせるための補正
                obj.transform.localScale = scale;
            }
        }
    }
}
