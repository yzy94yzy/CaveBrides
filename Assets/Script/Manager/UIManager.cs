using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    public bool propBarIsOpen
    {
        get;
        set;
    }

    //�����ر�UI�����ʱ��
    public float close_time;
    //���������½�
    public int CurChapter
    {
        get;
        set;
    }

    private Dictionary<PanelType, string> panelPath = new();
    private Dictionary<PanelType, BasePanel> panelDict = new Dictionary<PanelType, BasePanel>();
    private Stack<BasePanel> panelStack = new Stack<BasePanel>();

    //�������ö������ֵ�
    public float hintShowTime;

    //�Ƚ�����Ŀ���UI
    [SerializeField]
    private GameObject startRhymePanel;
    [SerializeField]
    private PropBarPanel propBarPanel;
    [SerializeField]
    private GameObject hintGroup, settingPanel;


    //�������ú�Ļ����
    [SerializeField]
    private Image blackCrossPanel;


    public void Start()
    {
        propBarIsOpen = false;
        CurChapter = 0;
        InitPanel();

    }

    /// <summary>
    /// ��ui��ʾ�ڽ�����
    /// </summary>
    /// <param name="panelType"></param>
    public BasePanel PushPanel(PanelType panelType)
    {
        //Debug.Log("push" + panelType.ToString());
        if (panelStack.Count > 1)
        {
            BasePanel topPanel = panelStack.Peek();
            topPanel.OnPause();
        }

        if (panelDict.TryGetValue(panelType, out BasePanel panel))
        {
            panelStack.Push(panel);
            panel.OnEnter();
            //Debug.Log("����");
            return panel;
        }
        else
        {
            BasePanel panel1 = SpawnPanel(panelType);
            panelStack.Push(panel1);
            panel1.OnEnter();
            //Debug.Log("����");
            return panel1;
        }
    }


    /// <summary>
    /// ��ʾ����UI��������ǰһ����ʾ�������ء�
    /// </summary>
    /// <param name="panelType"></param>
    /// <returns></returns>
    public BasePanel AddPanel(PanelType panelType)
    {
        //Debug.Log("����ui add : " + panelType.ToString());
        if (panelDict.TryGetValue(panelType, out BasePanel panel))
        {
            panel.OnEnter();
            return panel;
        }
        else
        {
            BasePanel panel1 = SpawnPanel(panelType);
            panel1.OnEnter();
            //Debug.Log("����");
            return panel1;
        }
    }

    /// <summary>
    /// �رյ�ǰui
    /// </summary>
    public void PopPanel()
    {
        if (panelStack.Count == 0) return;

        BasePanel topPanel = panelStack.Pop();
        topPanel.OnExit();

        if (panelStack.Count > 0)
        {
            BasePanel panel = panelStack.Peek();
            panel.OnRecovery();
        }
    }

    /// <summary>
    /// ʵ������Ӧ��ui
    /// </summary>
    /// <param name="panelType"></param>
    private BasePanel SpawnPanel(PanelType panelType)
    {
        if (panelPath.TryGetValue(panelType, out string path))
        {
            GameObject g = GameObject.Instantiate(Resources.Load(path)) as GameObject;
            /*if (panelType == PanelType.Message)
                g.transform.SetParent(hintCanvasTransform, false);
            else*/
            g.transform.SetParent(GameObject.Find("Canvas").transform, false);
            BasePanel panel = g.GetComponent<BasePanel>();
            panel.SetUIMag = this;
            panelDict.Add(panelType, panel);
            return panel;
        }
        else
        {
            Debug.Log("��");
            return null;
        }
    }

    /// <summary>
    /// ��ʼ��ui·��
    /// </summary>
    private void InitPanel()
    {
        string originPath = "Panel/";
        string[] path = new string[] { "SettingPanel"};
        if (panelPath != null)
        {
            panelPath.Add(PanelType.Setting, originPath + path[0]);
        }
        else
            Debug.LogError("panelPath is null");
        if (panelStack == null)
            Debug.LogError("panelStack is null");

    }


    public BasePanel GetPanel(PanelType type)
    {
        return panelDict[type];
    }

    public void OpenSettingPanel()
    {
        settingPanel.SetActive(true);
    }

    public void OpenAndClosePropBar()
    {
        if (propBarIsOpen)
        {
            //Debug.Log("close");
            propBarIsOpen = false;
            propBarPanel.ShowAndHidePropBar(false);
        }
        else
        {
            //Debug.Log("open");
            propBarIsOpen = true;
            propBarPanel.ShowAndHidePropBar(true);
        }
    }


    //��ʾͼƬ����ʾ
    private int hintIndex = -1;
    private bool isShow = false;
    public void ShowHintImage(int hintIndex)
    {
        if (isShow && hintIndex == 0)
            return;
        isShow = true;
        this.hintIndex = hintIndex;
        hintGroup.SetActive(true);
        hintGroup.transform.GetChild(hintIndex).GetComponent<Image>().DOFade(1, hintShowTime);
        if (hintIndex != 0)
            Invoke("HideHintImage", hintShowTime);
    }

    //��������Ҫ�������ֵ���ʾͼƬ����ʧ
    public void ResetHintGroupFalse()
    {
        if (!isShow)
            return;
        isShow = false;
        //��ֹ��������û�����DOTWEEN����BUG
        DOTween.KillAll();
        if (hintIndex != -1)
        {
            Color col = hintGroup.transform.GetChild(hintIndex).GetComponent<Image>().color;
            col.a = 0;
            hintGroup.transform.GetChild(hintIndex).GetComponent<Image>().color = col;
            hintIndex = -1;
        }
        hintGroup.SetActive(false);
    }

    //��������ֻ����һ�ε���ʾͼƬ��С��
    public void HideHintImage()
    { 
        isShow = false;
        if (hintIndex != -1 && hintGroup.transform.GetChild(hintIndex) != null)
        {
            hintGroup.transform.GetChild(hintIndex).GetComponent<Image>().DOFade(0, hintShowTime);
        }
        Invoke("SetHintGroupFalse", hintShowTime);
    }

    //��ʾͼƬ
    public void SetHintGroupFalse()
    {
        //��ֹ����û��ִ�У�ȫ����Ϊ0͸����
        for (int i = 0; i < hintGroup.transform.childCount; i++)
            hintGroup.transform.GetChild(i).GetComponent<Image>().DOFade(0, 0);
        DOTween.Kill(hintGroup);
        hintIndex = -1;
        hintGroup.SetActive(false);
    }


    private float blackCrossTime = 0;
    //�������ú�Ļ����
    public void BlackCross(float blackCrossTime)
    {
        if (GameObject.FindWithTag("Player").GetComponent<PlayerManager>() != null)
            GameObject.FindWithTag("Player").GetComponent<PlayerManager>().enabled = false;
        else
            GameObject.FindWithTag("Player").GetComponent<BirdManager>().enabled = false;
        this.blackCrossTime = blackCrossTime;
        DOTween.Init();
        blackCrossPanel.DOFade(1, blackCrossTime);
        //�ȴ�0.5s����л�����
        Invoke("BlackFade", blackCrossTime + 1f);
    }

    //�ӳٹرպ�Ļ����������ж�
    private void BlackFade()
    {
        blackCrossPanel.DOFade(0, blackCrossTime);
        Invoke("ActivePlayer", blackCrossTime);
    }


    //�浵��ȡ��ʱ��һ��ʼ����Ҫ��Ļ�ڵ� ��Ҫ��һ��ʱ������ʾֱ���Ǻ�ɫ
    public void BlackMaskCross(float blackCrossTime)
    {
        //�ȴ��л����������ʾ 
        this.blackCrossTime = blackCrossTime;
        Invoke("BlackFade", blackCrossTime);
    }
    
    public void CloseBlackImmediately()
    {
        blackCrossPanel.DOFade(0, 0);
    }

    public void ActivePlayer()
    {
        //��һ��ֻҪ�ص����� �п������� ��Ҫ�������
        if (ChapManager.Instance.CurChap == 1)
        {
            return;
        }
        if (GameObject.FindWithTag("Player").GetComponent<PlayerManager>() != null)
            GameObject.FindWithTag("Player").GetComponent<PlayerManager>().enabled = true;
        else
            GameObject.FindWithTag("Player").GetComponent<BirdManager>().enabled = true;
    }


    CanvasGroup closePanel = null;
    public void ClosePanelSlowly(CanvasGroup panel)
    {
        closePanel = panel;
        panel.DOFade(0, close_time);
        Invoke("SetPanelFalse", close_time);
    }
    
    public void SetPanelFalse()
    {
        closePanel.gameObject.SetActive(false);
    }

}
