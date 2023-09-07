using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropDetailPanel : MonoBehaviour
{
    private int curShowIndex;
    //当前细节面板显示的
    [SerializeField]
    private Image curImg, bigImg, bigTitle;
    [SerializeField]
    private Text curDes, curName;

    //所有道具的细节
    [SerializeField]
    private List<Sprite> propTitle;
    [SerializeField]
    private List<string> propDescribe, propDescribe_English, propName, propName_English;
    //需要中英切换的细节图
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

    //切换文本内容中英
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
