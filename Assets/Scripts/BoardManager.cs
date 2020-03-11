using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Zenject;
using TMPro;

namespace StackTicTacToe
{
    /// <summary>
    /// ボード関連のオブジェクト管理を行う
    /// ボード/光の柱生成、セル生成
    /// </summary>
    public class BoardManager : MonoBehaviour
    {
        [SerializeField] private Vector3Int boardSize; // ボードの大きさ
        [SerializeField][Min(1)] private int goalNum; // N目並べのNの値
        [SerializeField] private float boardObjectHeight; //ボードobjectの高さ
        [SerializeField] private GameObject blueCell; // 青セルPrefab
        [SerializeField] private GameObject redCell; // 赤セルPrefab
        [SerializeField] private TextMeshProUGUI winningText; // 勝利テキスト

        private IInputUniRx inputUniRx; // インプット

        [Inject]
        private void Initialize(IInputUniRx _inputUniRx)
        {
            inputUniRx = _inputUniRx;
        }

        private void OnDestroy()
        {
            inputUniRx.Dispose();
        }

        private void Start()
        {
            LightPillar currentPillar = null; // 選択中表示用の光の柱(null -> 非選択状態)
            Vector2Int currentPos = Vector2Int.zero; // 選択中の位置(マス)

            CompositeDisposable disposables = new CompositeDisposable();

            // ボードObject作成
            var boardFactory = GetComponentInChildren<BoardFactory>();
            boardFactory.Create(boardSize.x, boardSize.z);

            // 光の柱Object作成
            var lightPillarFactory = GetComponentInChildren<LightPillarFactory>();
            lightPillarFactory.Create(boardSize.x, boardSize.z);

            // システム初期化
            var boardSystem = new BoardSystem();
            boardSystem.Initialize(boardSize, goalNum);

            // シーン遷移が終わったら入力可能にする
            Observable.EveryUpdate()
                .SkipWhile(_ => SceneMover.Instance.IsSceneMoving)
                .First()
                .Subscribe(_ =>
                {
                    inputUniRx.Initialize();
                });

            // カーソル位置に光の柱を表示(そのマスを選択中にする)
            inputUniRx.Position
                .Subscribe(pos =>
                {
                    var ray = Camera.main.ScreenPointToRay(new Vector3(pos.x, pos.y));
                    Debug.DrawRay(ray.origin, ray.direction * 10);
                    RaycastHit raycastHit = new RaycastHit();

                    if (Physics.Raycast(ray, out raycastHit))
                    {
                        var hitLightPillar = raycastHit.transform.GetComponent<LightPillar>();
                        if (!ReferenceEquals(currentPillar, hitLightPillar))
                        {
                            // 指しているマスが違う
                            if (currentPillar != null) currentPillar.Disable();
                            // セル設置可能(高さに余裕がある)なら
                            var nextPos = hitLightPillar.Position;
                            if (boardSystem.IsPlacable(nextPos.x, nextPos.y)) {
                                // 選択(光の柱表示)
                                currentPillar = hitLightPillar;
                                currentPos = nextPos;
                                currentPillar.Enable();
                            }
                        } // else: 変更不要(同じマスを指し続けている)
                    }
                    else
                    {
                        // どこも指していない
                        if (currentPillar != null)
                        {
                            currentPillar.Disable();
                            currentPillar = null;
                        }
                    }
                })
                .AddTo(disposables);

            // 選択中にTap -> セルを置く
            inputUniRx.Tap
                .Where(_ => currentPillar != null)
                .Subscribe(_ =>
                {
                    // セルObjectの生成&配置
                    var prefab = boardSystem.GetTurn() == BoardSystem.CellColor.Blue ? blueCell : redCell;
                    var position = currentPillar.transform.position;
                    position.y = boardObjectHeight + // boardの高さ
                        boardSystem.GetHeight(currentPos.x, currentPos.y) // cellを置くべき高さ
                        * (blueCell.transform.localScale.y + 0.05f) // (blueCube + 隙間)の高さ
                        + blueCell.transform.localScale.y * 0.5f; // blueCubeの中心が中央であることへの対応
                    Instantiate(prefab, position, Quaternion.identity, transform);

                    // システム上のセルの配置
                    boardSystem.Place(currentPos.x, currentPos.y);

                    // 勝利判定
                    if(boardSystem.GetWinner() != BoardSystem.CellColor.None)
                    {
                        disposables.Dispose();
                        ShowWinner(boardSystem.GetWinner());
                    }

                    // これ以上載せられないなら選択解除
                    if (!boardSystem.IsPlacable(currentPos.x, currentPos.y))
                    {
                        currentPillar.Disable();
                        currentPillar = null;
                    }
                })
                .AddTo(disposables);
        }

        private void ShowWinner(BoardSystem.CellColor winner)
        {
            string color = (winner == BoardSystem.CellColor.Blue ? "6060FF" : "FF6060");
            string winnerName = (winner == BoardSystem.CellColor.Blue ? "Blue" : "Red");

            winningText.text = $"<color=#{color}>{winnerName} Win!</color>";
            winningText.gameObject.SetActive(true);

            Observable.Timer(System.TimeSpan.FromSeconds(3))
                .Subscribe(_ =>
                {
                    SceneMover.Instance.ChangeScene("TitleScene");
                })
                .AddTo(this);
        }
    }
}
