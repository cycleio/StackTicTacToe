using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackTicTacToe {
    /// <summary>
    /// タイトルとメインシーン間で受け渡される情報
    /// </summary>
    public struct BoardSettings
    {
        public Vector3Int boardSize;
        public int goalNum;
    }
}
