using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PixelCrushers.DialogueSystem;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField]
    private Texture2D cursor_texture;
    public int MaxChap
    {
        set;
        get;
    }
    protected override void Init()
    {
        MaxChap = 1;
        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_WEBGL
        Cursor.SetCursor(cursor_texture, Vector2.zero, CursorMode.ForceSoftware);
#else
        Cursor.SetCursor(cursor_texture, Vector2.zero, CursorMode.Auto);
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadSceneByIndex(int index)
    {
        DG.Tweening.DOTween.KillAll();
        DG.Tweening.DOTween.Clear();
        SceneManager.LoadSceneAsync(index);
        //LanguageManager.Instance.BroadcastLanguage();
        //开始阶段和游戏阶段的BGM切换
        if (index <= 1)
        {
            StartAudioManager.Instance.StartBGMPlay();
        }
        else
        {
            SceneManager.sceneLoaded += CompleteSceneLoadedDelayNext;
            //Invoke("ChangeLanguageSystem", 0.8f);
            StartAudioManager.Instance.StartBGMStop();
        }
    }

    public void CompleteSceneLoadedDelayNext(Scene scene, LoadSceneMode sceneType)
    {
        Invoke("AfterSceneLoad", 0.5f);
    }

    public void AfterSceneLoad()
    {
        SceneManager.sceneLoaded -= CompleteSceneLoadedDelayNext;
        LoadManager.Instance.ChangeScene();
        ChangeLanguageSystem();
    }

    //根据开始设置界面状态加载对话的中英文
    public void ChangeLanguageSystem()
    {
        if (LanguageManager.Instance.IsChinese)
            DialogueManager.SetLanguage("default");
        else
            DialogueManager.SetLanguage("EN");

    }

    //从loadscene到startscene的BGM要连贯
    public void LoadSceneToStartScene(int index)
    {
        SceneManager.LoadSceneAsync(index);
    }

    public void CloseBlackCrossDelay()
    {
        Invoke("CloseBlackCross", 0.8f);
    }

    private void CloseBlackCross()
    {
        UIManager.Instance.CloseBlackImmediately();
    }

}
