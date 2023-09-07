using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PropManager : MonoSingleton<PropManager>
{
    //可配置的地方
    public float fall_time, altarMoveEnd, altarMoveTime;
    public Vector3 earingFallPos, YangzhuoPos;
    [SerializeField]
    private RectTransform sisterNoteFallPos;

    public Dictionary<int, PropType> propIntToTypeDict = new Dictionary<int, PropType>();


    //火焰台相关的物体
    [SerializeField]
    private GameObject altar;
    [SerializeField]
    private Transform backPack, earing;

    //闺房的物体
    [SerializeField]
    private Button windowpaperGet;

    //姐姐家的物体
    [SerializeField]
    private RectTransform sisterNote;

    //河边的物体
    [SerializeField]
    private GameObject stoneStatueEndTrigger, yangzhuo, yangUncle, grandma, uncles, uncles_inside, door_river_outside, door_river_inside;

    //第四章的道具管理
    [SerializeField]
    ChapScene_4 chapScene_4;
    [SerializeField]
    private GameObject cabinetpanel_sisterroom_inside_hint, papermoney, incenseTrigger;

    //和道具相关的UI
    [SerializeField]
    private PropBarPanel propBarPanel;
    [SerializeField]
    private PropDetailPanel propDetailPanel;
    [SerializeField]
    private CabinetPanel cabinetPanel;
    //需要放进道具栏的物品要destroy掉来触发对话
    [SerializeField]
    private List<GameObject> propsDestroyList;


    // Start is called before the first frame update
    void Start()
    {
        InitPropDictionary();
        propBarPanel.InitAllProps();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //初始化所有道具对应的int
    public void InitPropDictionary()
    {
        propIntToTypeDict.Add(0, PropType.Stick);
        propIntToTypeDict.Add(1, PropType.Dolls_1);
        propIntToTypeDict.Add(2, PropType.Dolls_2);
        propIntToTypeDict.Add(3, PropType.Dolls_3);
        propIntToTypeDict.Add(4, PropType.Dolls_4);
        propIntToTypeDict.Add(5, PropType.Earing);
        propIntToTypeDict.Add(6, PropType.Letter);
        propIntToTypeDict.Add(7, PropType.Windowpaper);
        propIntToTypeDict.Add(8, PropType.Picture);
        propIntToTypeDict.Add(9, PropType.SisterNote);
        propIntToTypeDict.Add(10, PropType.Bamboo);
        propIntToTypeDict.Add(11, PropType.DuckHeart);
        propIntToTypeDict.Add(12, PropType.Knife);
        propIntToTypeDict.Add(13, PropType.GreenRice);
        propIntToTypeDict.Add(14, PropType.Book);
        propIntToTypeDict.Add(15, PropType.EyeDoll);
    }


    //完成字符串到类型转换 为了适配UNITY里只能调用string类型的方法 但是接下来又需要propType
    public PropType IntToPropType(int type)
    {
        return propIntToTypeDict[type];
    }


    //道具栏的处理逻辑
    public void GetDolls()
    {
        propBarPanel.GetDolls();
    }

    public void UseDolls()
    {
        propBarPanel.UseDolls();
    }

    //重载方法一个用来在场景事件中调用；一个用来在代码中调用；增强可读性。
    public void AddNewProp(int type)
    {
        PropType propType = IntToPropType(type);
        propBarPanel.AddNewProp(propType);
    }
    public void AddNewProp(PropType type)
    {
        propBarPanel.AddNewProp(type);
    }
    //加载新章节的时候要设置
    public void SetPropNum(int num)
    {
        propBarPanel.SetPropNum(num);
    }

    //0 窗花 1 青团 2 鸭心 3 毛竹 4 小刀 5 眼睛娃娃
    public void ShowNewPropHint(int type)
    {
        Destroy(propsDestroyList[type]);
    }

    public void UseProp(int type)
    {
        PropType propType = IntToPropType(type);
        propBarPanel.UseProp(propType);
    }

    public void UseProp(PropType type)
    {
        propBarPanel.UseProp(type);
    }

    public void ClearPropBar()
    {
        propBarPanel.ClearPropBar();
    }

    public void UpdatePropList()
    {
        propBarPanel.UpdatePropBarList();
    }

    //检查道具是否已拥有 只允许代码调用返回bool值
    public bool CheckPropIsOwn(PropType type)
    {
        return propBarPanel.CheckPropIsOwn(type);
    }


    //处理背篓掉落
    public void BackPackFall()
    {
        float y = -1f; 
        Vector3 backNewPos = new Vector3(backPack.position.x, y, backPack.position.z);
        backPack.DOMove(backNewPos, 0.5f, true);
    }

    //处理石像吐出来的道具掉落
    public void StoneStatuePropFallDown()
    {
        //Debug.Log("stone statue prop fall");

        if (ChapManager.Instance.CurChap == 1)
        {
            earing.gameObject.SetActive(true);
            earing.DOMove(earingFallPos, fall_time);
        }
        else if (ChapManager.Instance.CurChap == 2)
        {
            sisterNote.gameObject.SetActive(true);
            sisterNote.DOMoveY(sisterNoteFallPos.position.y, fall_time);
        }
        else if (ChapManager.Instance.CurChap == 3)
        {
            stoneStatueEndTrigger.SetActive(true);
        }
        Invoke("ActiveStoneStatueProp", fall_time);
    }

    public void ActiveStoneStatueProp()
    {
        if (ChapManager.Instance.CurChap == 1)
            earing.GetComponent<BoxCollider2D>().enabled = true;
        else if (ChapManager.Instance.CurChap == 2)
            sisterNote.GetComponent<Button>().enabled = true;
    }


    //使用这个方法的时候要注意这是detail代码里的index 和 propbar里的不一样
    public void ShowPropDetailPanel(int propIndex, Sprite propImg)
    {
        propDetailPanel.gameObject.SetActive(true);
        propDetailPanel.ShowPropDetail(propIndex, propImg);
    }


    bool altarIsShow = false;
    public void ShowAltar()
    {
        if (altarIsShow)
            return;
        altarIsShow = true;
        GameAudioManager.Instance.PlayShowAltarAudio();
        altar.transform.DOMoveX(altarMoveEnd, altarMoveTime);
        Invoke("ActivePlayer", altarMoveTime);
    }

    public void ActivePlayer()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerManager>().enabled = true;
    }

    //处理门神完成激活窗花拾取
    public void ActiveWindowpaperCutGet()
    {
        windowpaperGet.enabled = true;
    }


    //第三章道具方法
    //在激活姐姐家里世界神龛后改变秧卓位置
    public void ChangeYangzhuoPosition()
    {
        yangzhuo.transform.position = YangzhuoPos;
    }
    
    public void Chap3RiverChangeNPCPosition()
    {
        //激活秧卓的结局对话
        yangzhuo.transform.GetChild(2).gameObject.SetActive(true);
        //移动NPC位置
        yangUncle.SetActive(false);
        grandma.SetActive(false);
        uncles.SetActive(true);
        uncles_inside.SetActive(true);
        //不允许进出门
        door_river_inside.GetComponent<Collider2D>().enabled = false;
        door_river_outside.GetComponent<Collider2D>().enabled = false;
    }

    //解锁河边里世界石像
    public void Chap4StoneStatueInsideUnlock()
    {
        chapScene_4.StoneStatueInsideUnlock();
    }


    //存档第三章进第四章要激活家神神龛
    public void SetCabinetSisterroomToChap4()
    {
        //随便激活一个就可以 中英会在打开界面的时候再切换
        cabinetpanel_sisterroom_inside_hint.SetActive(true);
        cabinetpanel_sisterroom_inside_hint.GetComponent<Image>().DOFade(1, 0);
        papermoney.SetActive(false);
        incenseTrigger.SetActive(false);
    }
}
