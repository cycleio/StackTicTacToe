using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UniRx;

namespace StackTicTacToe
{
    /// <summary>
    /// シーン遷移機構
    /// シーン遷移(非同期読み込み/フェードエフェクト)及び
    /// シーン間のデータ(Struct)受け渡しが可能
    /// </summary>
    public class SceneMover : SingletonMonoBehaviour<SceneMover>
    {
        [SerializeField] private float fadingSeconds = 1.0f;
        [SerializeField] private Image blackSquare;

        public bool IsSceneMoving { get; private set; } = false;

        private dynamic transportData;
        public dynamic TransportData
        {
            get
            {
                // 取ったら消去(Dequeue/Popのような動作)
                dynamic data = transportData;
                transportData = null;
                return data;
            }
            set { transportData = value; }
        }

        private new void Awake()
        {
            DontDestroyOnLoad(gameObject);
            base.Awake();
        }

        /// <summary>
        /// シーンを遷移する
        /// 
        /// 時系列： -> t
        /// |次シーンロード|前シーンアンロード|フェードイン|
        /// |フェードアウト|
        /// </summary>
        /// <param name="sceneName">移動先シーン名</param>
        public void ChangeScene(string sceneName)
        {
            if (IsSceneMoving) return; // 既にシーン移動中なら遷移処理をキャンセル
            IsSceneMoving = true; // シーン移動中フラグを立てる

            var currentScene = SceneManager.GetActiveScene();

            // 次シーンロード開始
            AsyncOperation loading = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            loading.allowSceneActivation = false;

            // フェードアウト開始
            bool isFadeOutDone = false;
            StartFade(isFadeOut: true, onFinished: () => {
                loading.allowSceneActivation = true;
                isFadeOutDone = true;
            });

            // 次シーンロード終了&フェードアウト完了したら前シーンアンロード
            loading.AsAsyncOperationObservable()
                .Where(obj => obj.isDone && isFadeOutDone)
                .First()
                .Subscribe(_ =>
                {
                    // 前シーンアンロード開始
                    var unloading = SceneManager.UnloadSceneAsync(currentScene);

                    // アンロード完了したらフェードイン開始
                    unloading.AsAsyncOperationObservable()
                        .Where(val => val.isDone)
                        .First()
                        .Subscribe(__ =>
                        {
                            StartFade(isFadeOut: false, onFinished: () =>
                            {
                                // フェード完了時シーン移動中フラグを折る
                                IsSceneMoving = false;
                            });
                        });
                });
        }

        private void StartFade(bool isFadeOut, System.Action onFinished)
        {
            float currentTime = 0f;
            Color blackSquareColor = blackSquare.color;

            // フェードアウト開始時：画面の表示&初期化
            if (isFadeOut)
            {
                blackSquareColor.a = 0f;
                blackSquare.color = blackSquareColor;
                blackSquare.gameObject.SetActive(true);
            }

            Observable.EveryFixedUpdate()
                .TakeUntil(Observable.Timer(System.TimeSpan.FromSeconds(fadingSeconds)))
                .Subscribe(_ => // フェード処理
                {
                    currentTime += Time.fixedDeltaTime;
                    blackSquareColor.a = isFadeOut ? currentTime / fadingSeconds : 1 - currentTime / fadingSeconds;
                    blackSquare.color = blackSquareColor;
                },
                () => // フェード後処理
                {
                    if (isFadeOut)
                    {
                        // フェードアウト終了時：黒画面の不透明化
                        blackSquareColor.a = 1f;
                        blackSquare.color = blackSquareColor;
                    }
                    else
                    {
                        // フェードイン終了時：黒画面消去
                        blackSquare.gameObject.SetActive(false);
                    }

                    onFinished();
                });
        }
    }
}
