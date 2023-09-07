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
        //��ʼ�׶κ���Ϸ�׶ε�BGM�л�
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

    //���ݿ�ʼ���ý���״̬���ضԻ�����Ӣ��
    public void ChangeLanguageSystem()
    {
        if (LanguageManager.Instance.IsChinese)
            DialogueManager.SetLanguage("default");
        else
            DialogueManager.SetLanguage("EN");

    }

    //��loadscene��startscene��BGMҪ����
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
