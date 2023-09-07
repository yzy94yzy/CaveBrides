using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{


    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError(typeof(T).ToString() + " is NULL.");
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError(typeof(T).ToString() + " aready have one on" + _instance.gameObject.name);
        }
        _instance = this as T;
        Init();
    }
    /// <summary>
    /// Ҫ��awake��ʼ��������Init
    /// </summary>
    protected virtual void Init()
    {
        //optional to override
    }
}