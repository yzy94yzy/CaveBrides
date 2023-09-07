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

    //缓慢关闭UI界面的时间
    public float close_time;
    //用来控制章节
    public int CurChapter
    {
        get;
        set;
    }

    private Dictionary<PanelType, string> panelPath = new();
    private Dictionary<PanelType, BasePanel> panelDict = new Dictionary<PanelType, BasePanel>();
    private Stack<BasePanel> panelStack = new Stack<BasePanel>();

    //用来配置动画表现的
    public float hintShowTime;

    //比较特殊的控制UI
    [SerializeField]
    private GameObject startRhymePanel;
    [SerializeField]
    private PropBarPanel propBarPanel;
    [SerializeField]
    private GameObject hintGroup, settingPanel;


    //用来设置黑幕过渡
    [SerializeField]
    private Image blackCrossPanel;


    public void Start()
    {
        propBarIsOpen = false;
        CurChapter = 0;
        InitPanel();

    }

    /// <summary>
    /// 把ui显示在界面上
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
            //Debug.Log("调用");
            return panel;
        }
        else
        {
            BasePanel panel1 = SpawnPanel(panelType);
            panelStack.Push(panel1);
            panel1.OnEnter();
            //Debug.Log("生成");
            return panel1;
        }
    }


    /// <summary>
    /// 显示弹窗UI，不会让前一个显示界面隐藏。
    /// </summary>
    /// <param name="panelType"></param>
    /// <returns></returns>
    public BasePanel AddPanel(PanelType panelType)
    {
        //Debug.Log("弹窗ui add : " + panelType.ToString());
        if (panelDict.TryGetValue(panelType, out BasePanel panel))
        {
            panel.OnEnter();
            return panel;
        }
        else
        {
            BasePanel panel1 = SpawnPanel(panelType);
            panel1.OnEnter();
            //Debug.Log("生成");
            return panel1;
        }
    }

    /// <summary>
    /// 关闭当前ui
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
    /// 实例化对应的ui
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
            Debug.Log("空");
            return null;
        }
    }

    /// <summary>
    /// 初始化ui路径
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


    //提示图片的显示
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

    //用来处理要反复出现的提示图片的消失
    public void ResetHintGroupFalse()
    {
        if (!isShow)
            return;
        isShow = false;
        //防止继续播放没放完的DOTWEEN出现BUG
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

    //用来处理只出现一次的提示图片的小事
    public void HideHintImage()
    { 
        isShow = false;
        if (hintIndex != -1 && hintGroup.transform.GetChild(hintIndex) != null)
        {
            hintGroup.transform.GetChild(hintIndex).GetComponent<Image>().DOFade(0, hintShowTime);
        }
        Invoke("SetHintGroupFalse", hintShowTime);
    }

    //提示图片
    public void SetHintGroupFalse()
    {
        //防止动画没有执行，全都置为0透明度
        for (int i = 0; i < hintGroup.transform.childCount; i++)
            hintGroup.transform.GetChild(i).GetComponent<Image>().DOFade(0, 0);
        DOTween.Kill(hintGroup);
        hintIndex = -1;
        hintGroup.SetActive(false);
    }


    private float blackCrossTime = 0;
    //用来设置黑幕过渡
    public void BlackCross(float blackCrossTime)
    {
        if (GameObject.FindWithTag("Player").GetComponent<PlayerManager>() != null)
            GameObject.FindWithTag("Player").GetComponent<PlayerManager>().enabled = false;
        else
            GameObject.FindWithTag("Player").GetComponent<BirdManager>().enabled = false;
        this.blackCrossTime = blackCrossTime;
        DOTween.Init();
        blackCrossPanel.DOFade(1, blackCrossTime);
        //等待0.5s完成切换操作
        Invoke("BlackFade", blackCrossTime + 1f);
    }

    //延迟关闭黑幕并激活玩家行动
    private void BlackFade()
    {
        blackCrossPanel.DOFade(0, blackCrossTime);
        Invoke("ActivePlayer", blackCrossTime);
    }


    //存档读取的时候一开始就需要黑幕遮挡 需要久一点时间再显示直接是黑色
    public void BlackMaskCross(float blackCrossTime)
    {
        //等待切换完了完成显示 
        this.blackCrossTime = blackCrossTime;
        Invoke("BlackFade", blackCrossTime);
    }
    
    public void CloseBlackImmediately()
    {
        blackCrossPanel.DOFade(0, 0);
    }

    public void ActivePlayer()
    {
        //第一章只要关掉界面 有开场动画 不要激活玩家
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
