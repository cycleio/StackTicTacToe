using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace StackTicTacToe
{
    /// <summary>
    /// ボード上のセルを管理し、配置の可否や勝利等を判定するクラス
    /// </summary>
    public class BoardSystem
    {
        public enum CellColor
        {
            None = 0,
            Blue,
            Red
        }

        private Vector3Int boardSize; // ボードの大きさ
        private CellColor[,,] boardCells; // ボード上のセル(コマ)の状況
        private int[,] cellsHeight; // ボード上のセルの高さ
        private CellColor turnCellColor; // ターン実行中のプレイヤーのセルの色
        private CellColor winningCellColor; // 勝利条件を満たしたプレイヤーの色
        private int goalNum; // セルを並べる目標数

        
        public CellColor GetTurn() => turnCellColor;
        public CellColor GetWinner() => winningCellColor;
        public int GetHeight(int x, int y) => cellsHeight[x, y];
        public bool IsPlacable(int x, int y) => cellsHeight[x, y] < boardSize.y;
        public bool IsFilled() => cellsHeight.Cast<int>().All(val => val == boardSize.y);
        
        static public bool IsBoardSizeValid(Vector3Int boardSize, int goalNum)
        {
            // boardSizeの各要素が1以上か?
            if (Vector3Int.Max(boardSize, Vector3Int.one) != boardSize) return false;
            // goalNumは1以上か?
            if (goalNum < 1) return false;
            // boardSizeの各要素のうち、少なくとも1つはgoalNum以上か?
            // boardSizeの各要素がgoalNum未満でないか?
            if (Mathf.Max(boardSize.x, boardSize.y,boardSize.z) < goalNum) return false;

            return true;
        }

        public void Initialize(Vector3Int boardSize, int goalNum)
        {
            Assert.IsTrue(IsBoardSizeValid(boardSize, goalNum));
            boardCells = new CellColor[boardSize.x, boardSize.y, boardSize.z];
            cellsHeight = new int[boardSize.x, boardSize.z];
            turnCellColor = CellColor.Blue;
            winningCellColor = CellColor.None;
            this.boardSize = boardSize;
            this.goalNum = goalNum;
        }

        public void Place(int x, int y)
        {
            if (!IsPlacable(x, y)) return;
            boardCells[x, cellsHeight[x, y], y] = turnCellColor;

            winningCellColor = UpdateWinningCellColor(x, y);

            ++cellsHeight[x, y];
            turnCellColor = (turnCellColor == CellColor.Blue ? CellColor.Red : CellColor.Blue);
        }

        // 揃いうるラインの方向
        private readonly Vector3Int[] searchDirections = new Vector3Int[]
        {
            new Vector3Int(1, 0, 0),    // X
            new Vector3Int(0, 1, 0),    // Y
            new Vector3Int(0, 0, 1),    // Z
            new Vector3Int(1, 1, 0),    // Diagonal(x+/y+/z0)
            new Vector3Int(1, -1, 0),   // Diagonal(x+/y-/z0)
            new Vector3Int(0, 1, 1),    // Diagonal(x0/y+/z+)
            new Vector3Int(0, -1, 1),   // Diagonal(x0/y-/z+)
            new Vector3Int(1, 0, 1),    // Diagonal(x+/y0/z+)
            new Vector3Int(-1, 0, 1),   // Diagonal(x-/y0/z+)
            new Vector3Int(1, 1, 1),    // Diagonal(x+/y+/z+)
            new Vector3Int(1, -1, 1),   // Diagonal(x+/y-/z+)
            new Vector3Int(-1, 1, 1),   // Diagonal(x-/y+/z+)
            new Vector3Int(-1, -1, 1)   // Diagonal(x-/y-/z+)
        };

        /// <summary>
        /// 直前に置かれたセルに関連する位置を検索し、勝利条件を満たしたかどうかをチェック
        /// </summary>
        /// <returns>勝利したセルの色。両者とも満たしていなければNone</returns>
        private CellColor UpdateWinningCellColor(int x, int y)
        {
            // 実装方針：置かれたセルを含むsearchDirections方向の直線を見る
            //           goalNum個の組について判定する
            // 例：3*3の平面で2目並べする例
            // 7 8 9
            // 4 5 6
            // 1 2 3
            // 7の位置に置かれたら、直線789、741、753に対し
            // 78, 89, 74, …のように2個の単位ごとに双方のセルが同じ色かチェックする
            // なぜ？：3次元(X*Y*Z)空間でN(<=Max(X,Y,Z))目並べ、の自由度に対応
            //         また、全パターン走査は計算回数が多すぎると判断

            var cellPosition = new Vector3Int(x, cellsHeight[x, y], y);

            Queue<CellColor> cells = new Queue<CellColor>();
            var winner = CellColor.None;

            //Debug.Log($"CellPos:{cellPosition}");
            for (int i = 0; i < searchDirections.Length; i++)
            {
                // 置かれたセルからx平面/y平面/z平面までの距離のうち最短のもの
                var lengthToOrigin = new int[] {
                    searchDirections[i].x > 0 ? cellPosition.x
                        : searchDirections[i].x < 0 ? boardSize.x - cellPosition.x - 1
                        : int.MaxValue,
                    searchDirections[i].y > 0 ? cellPosition.y
                        : searchDirections[i].y < 0 ? boardSize.y - cellPosition.y - 1
                        : int.MaxValue,
                    searchDirections[i].z > 0 ? cellPosition.z
                        : searchDirections[i].z < 0 ? boardSize.z - cellPosition.z - 1
                        : int.MaxValue,
                }.Min();
                // 置かれたセルからx=(ボードx)/y=(ボードy)/z=(ボードz)の各平面までの距離のうち最短のもの
                var lengthToFarPoint = new int[] {
                    searchDirections[i].x < 0 ? cellPosition.x
                        : searchDirections[i].x > 0 ? boardSize.x - cellPosition.x - 1
                        : int.MaxValue,
                    searchDirections[i].y < 0 ? cellPosition.y
                        : searchDirections[i].y > 0 ? boardSize.y - cellPosition.y - 1
                        : int.MaxValue,
                    searchDirections[i].z < 0 ? cellPosition.z
                        : searchDirections[i].z > 0 ? boardSize.z - cellPosition.z - 1
                        : int.MaxValue,
                }.Min();

                var currentPos = cellPosition - searchDirections[i] * lengthToOrigin;
                cells.Clear();

                // currentPosから currentPos + (searchDirections[i] * (lengthToOrigin + lengthToFarPoint))
                // までの直線を確認する

                //Debug.Log($"Direction: {searchDirections[i]}");
                for (int j = 0; j <= lengthToOrigin + lengthToFarPoint; j++)
                {
                    //Debug.Log($"pos:{currentPos}, col:{boardCells[currentPos.x, currentPos.y, currentPos.z]}");
                    cells.Enqueue(boardCells[currentPos.x, currentPos.y, currentPos.z]);

                    if(cells.Count == goalNum)
                    {
                        if (cells.Distinct().Count() == 1 && cells.Peek() != CellColor.None)
                        {
                            winner = cells.Peek();
                            break;
                        }
                        cells.Dequeue();
                    }

                    currentPos += searchDirections[i];
                }

                if (winner != CellColor.None) break;
            }

            return winner;
        }
    }
}
