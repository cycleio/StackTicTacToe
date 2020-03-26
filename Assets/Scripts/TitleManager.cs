using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;

namespace StackTicTacToe
{
    public class TitleManager : MonoBehaviour
    {
        [SerializeField] private TMP_InputField boardSizeXInput;
        [SerializeField] private TMP_InputField boardSizeYInput;
        [SerializeField] private TMP_InputField boardSizeZInput;
        [SerializeField] private TMP_InputField numToWin;
        [SerializeField] private TextMeshProUGUI errorText;


        private SceneMover sceneMover;

        private void Start()
        {
            sceneMover = SceneMover.Instance;

            if (!Object.ReferenceEquals(sceneMover.TransportData, null))
            {
                var boardSettings = (BoardSettings)sceneMover.TransportData;
                boardSizeXInput.text = boardSettings.boardSize.x.ToString();
                boardSizeYInput.text = boardSettings.boardSize.y.ToString();
                boardSizeZInput.text = boardSettings.boardSize.z.ToString();
                numToWin.text = boardSettings.goalNum.ToString();
            }
        }

        public void OnStartButtonPressed()
        {
            BoardSettings boardSettings = new BoardSettings();
            boardSettings.boardSize.x = int.Parse(boardSizeXInput.text);
            boardSettings.boardSize.y = int.Parse(boardSizeYInput.text);
            boardSettings.boardSize.z = int.Parse(boardSizeZInput.text);
            boardSettings.goalNum = int.Parse(numToWin.text);

            if(!BoardSystem.IsBoardSizeValid(boardSettings.boardSize, boardSettings.goalNum)) {
                // 入力値が不正 -> 再設定要求
                ShowError("Error: Input value(s) are invalid");
                return;
            }

            //入力値が適正 -> シーン遷移
            sceneMover.TransportData = boardSettings;
            sceneMover.ChangeScene("MainScene");

        }

        /// <summary>
        /// エラー表示を行う
        /// </summary>
        /// <param name="msg">表示メッセージ</param>
        private void ShowError(string msg)
        {
            errorText.text = msg;
            errorText.gameObject.SetActive(true);
            Observable.Timer(System.TimeSpan.FromSeconds(5))
                .First()
                .Subscribe(_ =>
                {
                    errorText.gameObject.SetActive(false);
                });
        }
    }
}
