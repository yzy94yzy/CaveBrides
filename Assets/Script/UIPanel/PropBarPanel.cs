using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PropBarPanel : BasePanel
{
    //玩家目前有的道具数量
    private int propNum;
    //玩家目前的娃娃数量
    private int dollsNum = 0;
    //存储道具对应的缩略图所在路径
    private Dictionary<PropType, string> propPath = new Dictionary<PropType, string>();
    //存储玩家目前有的道具
    private List<PropType> propList = new List<PropType>();

    [SerializeField]
    private RectTransform startPoint, showPoint;
    [SerializeField]
    private Image propBar;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetPropNum(int num)
    {
        propNum = num;
    }
    public void InitAllProps()
    {
        //Debug.Log("Init all props");
        string originPath = "Prop/";
        string[] path = new string[] { "Stick", "Dolls_1", "Dolls_2", "Dolls_3", "Dolls_4", "Earing", "Letter", "Windowpaper", "Picture", "SisterNote"
        , "Bamboo", "DuckHeart", "Knife", "GreenRice", "Book", "EyeDoll"};

        if (propPath != null)
        {
            propPath.Add(PropType.Stick, originPath + path[0]);
            propPath.Add(PropType.Dolls_1, originPath + path[1]);
            propPath.Add(PropType.Dolls_2, originPath + path[2]);
            propPath.Add(PropType.Dolls_3, originPath + path[3]);
            propPath.Add(PropType.Dolls_4, originPath + path[4]);
            propPath.Add(PropType.Earing, originPath + path[5]);
            propPath.Add(PropType.Letter, originPath + path[6]);
            propPath.Add(PropType.Windowpaper, originPath + path[7]);
            propPath.Add(PropType.Picture, originPath + path[8]);
            propPath.Add(PropType.SisterNote, originPath + path[9]);
            propPath.Add(PropType.Bamboo, originPath + path[10]);
            propPath.Add(PropType.DuckHeart, originPath + path[11]);
            propPath.Add(PropType.Knife, originPath + path[12]);
            propPath.Add(PropType.GreenRice, originPath + path[13]);
            propPath.Add(PropType.Book, originPath + path[14]);
            propPath.Add(PropType.EyeDoll, originPath + path[15]);
        }
        else
            Debug.LogError("propPath is null");

    }

    public void ShowAndHidePropBar(bool isShow)
    {
        if (isShow)
        {
            gameObject.SetActive(true);
            //防止一下bug
            if (PlayerManager.Instance != null)
            {
                PlayerManager.Instance.StopPlayerAnim();
                PlayerManager.Instance.enabled = false;
            }
            else
                Debug.LogError("控制道具栏的玩家为空!!!");
            propBar.rectTransform.DOMove(showPoint.position, 1f);
            CancelInvoke("ClosePropBar");
        }
        else
        {
            if (PlayerManager.Instance != null)
                PlayerManager.Instance.enabled = true;
            else
                BirdManager.Instance.enabled = true;
            propBar.rectTransform.DOMove(startPoint.position, 1f);
            Invoke("ClosePropBar", 1f);
        }
    }

    public void ClosePropBar()
    {
        gameObject.SetActive(false);
    }

    //娃娃逻辑单独处理
    public void GetDolls()
    {
        AddNewProp(PropType.Dolls_4);
        dollsNum = 4;
    }
    
    public void UseDolls()
    {
        UseProp(PropManager.Instance.IntToPropType(dollsNum));
        dollsNum--;
        //Debug.Log("use dolls remain num " + dollsNum);
        if (dollsNum > 0)
            AddNewProp(PropManager.Instance.IntToPropType(dollsNum));
    }

    //拾取到新道具
    public void AddNewProp(PropType propType)
    {
        propNum++;
        if (propPath.TryGetValue(propType, out string path))
        {
            //Debug.Log("add new prop  " + propType);
            GameObject g = GameObject.Instantiate(Resources.Load(path)) as GameObject;
            g.transform.SetParent(this.transform.GetChild(propNum - 1).transform, false);
            propList.Add(propType);
        }
        else
        {
            Debug.LogError(" add new prop path is null " + propType);
        }
    }

    //玩家使用自己已有的道具
    public void UseProp(PropType propType)
    {
        //Debug.Log("use prop " + propType);
        //先清除目前的道具栏 为更新作准备
        ClearPropBar();
        propNum--;

        //减掉要用掉的道具
        foreach (PropType pt in propList)
        {
            if (pt == propType)
            {
                propList.Remove(pt);
                break;
            }
        }
        //重新加载道具栏
        UpdatePropBarList();
    }

    //清空道具栏
    public void ClearPropBar()
    {
        for (int i = 0; i < propNum; i++)
        {
            if (this.transform.GetChild(i).childCount > 0)
            {
                DestroyImmediate(this.transform.GetChild(i).GetChild(0).gameObject);
            }
        }
    }

    //列表出现了删除更新 重新加载一下道具
    public void UpdatePropBarList()
    {
        for(int i = 0; i < propNum; i++)
        {
            if (propPath.TryGetValue(propList[i], out string path))
            {
                //Debug.Log("update " + i + "  prop path " + path);
                GameObject g = GameObject.Instantiate(Resources.Load(path)) as GameObject;
                g.transform.SetParent(this.transform.GetChild(i).transform, false);
            }
        }
    }

    //检查玩家当前是否已拥有某个道具
    public bool CheckPropIsOwn(PropType type)
    {
        foreach(PropType p in propList)
        {
            if (p == type)
                return true; 
        }
        return false;
    }
}
