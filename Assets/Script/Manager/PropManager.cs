using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PropManager : MonoSingleton<PropManager>
{
    //�����õĵط�
    public float fall_time, altarMoveEnd, altarMoveTime;
    public Vector3 earingFallPos, YangzhuoPos;
    [SerializeField]
    private RectTransform sisterNoteFallPos;

    public Dictionary<int, PropType> propIntToTypeDict = new Dictionary<int, PropType>();


    //����̨��ص�����
    [SerializeField]
    private GameObject altar;
    [SerializeField]
    private Transform backPack, earing;

    //�뷿������
    [SerializeField]
    private Button windowpaperGet;

    //���ҵ�����
    [SerializeField]
    private RectTransform sisterNote;

    //�ӱߵ�����
    [SerializeField]
    private GameObject stoneStatueEndTrigger, yangzhuo, yangUncle, grandma, uncles, uncles_inside, door_river_outside, door_river_inside;

    //�����µĵ��߹���
    [SerializeField]
    ChapScene_4 chapScene_4;
    [SerializeField]
    private GameObject cabinetpanel_sisterroom_inside_hint, papermoney, incenseTrigger;

    //�͵�����ص�UI
    [SerializeField]
    private PropBarPanel propBarPanel;
    [SerializeField]
    private PropDetailPanel propDetailPanel;
    [SerializeField]
    private CabinetPanel cabinetPanel;
    //��Ҫ�Ž�����������ƷҪdestroy���������Ի�
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

    //��ʼ�����е��߶�Ӧ��int
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


    //����ַ���������ת�� Ϊ������UNITY��ֻ�ܵ���string���͵ķ��� ���ǽ���������ҪpropType
    public PropType IntToPropType(int type)
    {
        return propIntToTypeDict[type];
    }


    //�������Ĵ����߼�
    public void GetDolls()
    {
        propBarPanel.GetDolls();
    }

    public void UseDolls()
    {
        propBarPanel.UseDolls();
    }

    //���ط���һ�������ڳ����¼��е��ã�һ�������ڴ����е��ã���ǿ�ɶ��ԡ�
    public void AddNewProp(int type)
    {
        PropType propType = IntToPropType(type);
        propBarPanel.AddNewProp(propType);
    }
    public void AddNewProp(PropType type)
    {
        propBarPanel.AddNewProp(type);
    }
    //�������½ڵ�ʱ��Ҫ����
    public void SetPropNum(int num)
    {
        propBarPanel.SetPropNum(num);
    }

    //0 ���� 1 ���� 2 Ѽ�� 3 ë�� 4 С�� 5 �۾�����
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

    //�������Ƿ���ӵ�� ֻ���������÷���boolֵ
    public bool CheckPropIsOwn(PropType type)
    {
        return propBarPanel.CheckPropIsOwn(type);
    }


    //����¨����
    public void BackPackFall()
    {
        float y = -1f; 
        Vector3 backNewPos = new Vector3(backPack.position.x, y, backPack.position.z);
        backPack.DOMove(backNewPos, 0.5f, true);
    }

    //����ʯ���³����ĵ��ߵ���
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


    //ʹ�����������ʱ��Ҫע������detail�������index �� propbar��Ĳ�һ��
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

    //����������ɼ����ʰȡ
    public void ActiveWindowpaperCutGet()
    {
        windowpaperGet.enabled = true;
    }


    //�����µ��߷���
    //�ڼ�����������������ı���׿λ��
    public void ChangeYangzhuoPosition()
    {
        yangzhuo.transform.position = YangzhuoPos;
    }
    
    public void Chap3RiverChangeNPCPosition()
    {
        //������׿�Ľ�ֶԻ�
        yangzhuo.transform.GetChild(2).gameObject.SetActive(true);
        //�ƶ�NPCλ��
        yangUncle.SetActive(false);
        grandma.SetActive(false);
        uncles.SetActive(true);
        uncles_inside.SetActive(true);
        //�����������
        door_river_inside.GetComponent<Collider2D>().enabled = false;
        door_river_outside.GetComponent<Collider2D>().enabled = false;
    }

    //�����ӱ�������ʯ��
    public void Chap4StoneStatueInsideUnlock()
    {
        chapScene_4.StoneStatueInsideUnlock();
    }


    //�浵�����½�������Ҫ�����������
    public void SetCabinetSisterroomToChap4()
    {
        //��㼤��һ���Ϳ��� ��Ӣ���ڴ򿪽����ʱ�����л�
        cabinetpanel_sisterroom_inside_hint.SetActive(true);
        cabinetpanel_sisterroom_inside_hint.GetComponent<Image>().DOFade(1, 0);
        papermoney.SetActive(false);
        incenseTrigger.SetActive(false);
    }
}
