using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropDetailPanel : MonoBehaviour
{
    private int curShowIndex;
    //��ǰϸ�������ʾ��
    [SerializeField]
    private Image curImg, bigImg, bigTitle;
    [SerializeField]
    private Text curDes, curName;

    //���е��ߵ�ϸ��
    [SerializeField]
    private List<Sprite> propTitle;
    [SerializeField]
    private List<string> propDescribe, propDescribe_English, propName, propName_English;
    //��Ҫ��Ӣ�л���ϸ��ͼ
    [SerializeField]
    private List<Sprite> propImg_chinese, propImg_english;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        LanguageManager.Instance.LanguageChange += ChangeLanguage;
    }
    private void OnDisable()
    {
        LanguageManager.Instance.LanguageChange -= ChangeLanguage;
    }

    //�л��ı�������Ӣ
    public void ChangeLanguage(bool isChinese)
    {
        if (isChinese)
        {
            curDes.text = propDescribe[curShowIndex];
            curName.text = propName[curShowIndex];
            if (curShowIndex == 8)
            {
                curImg.sprite = propImg_chinese[0];
                bigImg.sprite = propImg_chinese[0];
            }
            else if (curShowIndex == 9)
            {
                curImg.sprite = propImg_chinese[1];
                bigImg.sprite = propImg_chinese[1];
            }
            else if (curShowIndex == 14)
            {
                curImg.sprite = propImg_chinese[2];
                bigImg.sprite = propImg_chinese[2];
            }
        }
        else
        {
            curDes.text = propDescribe_English[curShowIndex];
            curName.text = propName_English[curShowIndex];
            if (curShowIndex == 8)
            {
                curImg.sprite = propImg_english[0];
                bigImg.sprite = propImg_english[0];
            }
            else if (curShowIndex == 9)
            {
                curImg.sprite = propImg_english[1];
                bigImg.sprite = propImg_english[1];
            }
            else if (curShowIndex == 14)
            {
                curImg.sprite = propImg_english[2];
                bigImg.sprite = propImg_english[2];
            }
        }
    }


    public void ShowPropDetail(int propIndex, Sprite propImg)
    {
        curShowIndex = propIndex;
        curImg.sprite = propImg;
        bigImg.sprite = propImg;
        bigTitle.sprite = propTitle[curShowIndex];
        ChangeLanguage(LanguageManager.Instance.IsChinese);
    }
}
