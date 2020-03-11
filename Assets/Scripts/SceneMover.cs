using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UniRx;

namespace StackTicTacToe
{
    public class SceneMover : SingletonMonoBehaviour<SceneMover>
    {
        [SerializeField] private float fadingSeconds = 1.0f;
        [SerializeField] private Image blackSquare;

        public bool IsSceneMoving { get; private set; } = false;

        private new void Awake()
        {
            DontDestroyOnLoad(gameObject);
            base.Awake();
        }

        /// <summary>
        /// シーンを遷移する
        /// </summary>
        /// <param name="sceneName">移動先シーン名</param>
        public void ChangeScene(string sceneName)
        {
            var loading = SceneManager.LoadSceneAsync(sceneName);
            loading.allowSceneActivation = false;

            // シーン移動中フラグを立てる
            IsSceneMoving = true;

            // blackSquare初期化
            var blackSquareColor = blackSquare.color;
            blackSquareColor.a = 0f;
            blackSquare.color = blackSquareColor;
            blackSquare.gameObject.SetActive(true);

            // フェードアウト処理
            float currentTime = 0f;
            Observable.EveryFixedUpdate()
                .TakeUntil(Observable.Timer(System.TimeSpan.FromSeconds(fadingSeconds)))
                .Subscribe(_ =>
                {
                    currentTime += Time.fixedDeltaTime;
                    blackSquareColor.a = currentTime / fadingSeconds;
                    blackSquare.color = blackSquareColor;
                },
                () =>
                { // フェードアウト終了時、シーン遷移可能に
                blackSquareColor.a = 1f;
                    blackSquare.color = blackSquareColor;
                    loading.allowSceneActivation = true;
                });

            // ロード完了を検知したらフェードイン処理
            loading.AsAsyncOperationObservable()
                .Where(operation => operation.isDone)
                .First()
                .Subscribe(_ =>
                {
                    currentTime = 0f;
                    Observable.EveryFixedUpdate()
                        .TakeUntil(Observable.Timer(System.TimeSpan.FromSeconds(fadingSeconds)))
                        .Subscribe(__ =>
                        {
                            currentTime += Time.fixedDeltaTime;
                            blackSquareColor.a = 1 - currentTime / fadingSeconds;
                            blackSquare.color = blackSquareColor;
                        },
                        () => // フェードイン終了時処理
                        {
                            // 黒画面消去
                            blackSquare.gameObject.SetActive(false);
                            // シーン移動中フラグを折る
                            IsSceneMoving = false;
                        });
                });
        }
    }
}
