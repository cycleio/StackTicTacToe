using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    //private static object syncObj = new object();
    private static bool applicationIsQuitting = false;
    
    public static T Instance
    {
        get
        {
            // 終了時にインスタンスの呼び出しがある場合のオブジェクト生成を防ぐ
            if (applicationIsQuitting)
            {
                return null;
            }

            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));

                if (instance == null)
                {
                    Debug.LogError(typeof(T) + "は存在しません");
                }

            }
            return instance;
        }
    }

    virtual protected void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);
            Debug.Log(
                typeof(T) +
                " は既に他のGameObjectにアタッチされているため破棄しました." +
                " アタッチされているGameObjectは " + Instance.gameObject.name + " です.");
            return;
        }
    }

    void OnApplicationQuit()
    {
        applicationIsQuitting = true;
    }

    void OnDestroy()
    {
        instance = null;
    }
    
    protected SingletonMonoBehaviour() { }
}
