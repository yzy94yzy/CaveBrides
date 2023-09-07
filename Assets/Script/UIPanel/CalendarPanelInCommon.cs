using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalendarPanelInCommon : MonoBehaviour
{
    private bool isChinese = true;
    [SerializeField]
    private Image img;
    [SerializeField]
    private List<Sprite> imgList;
    [SerializeField]
    private List<Sprite> imgList_English;
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
        ChangeLanguage(LanguageManager.Instance.IsChinese);
    }

    private void OnDisable()
    {
        LanguageManager.Instance.LanguageChange -= ChangeLanguage;
    }

    public void ChangeLanguage(bool isChinese)
    {
        this.isChinese = isChinese;
        if (ChapManager.Instance.CurChap < 3)
            return;
        if (isChinese)
            img.sprite = imgList[ChapManager.Instance.CurChap - 2];
        else
            img.sprite = imgList_English[ChapManager.Instance.CurChap - 2];
    }
}
