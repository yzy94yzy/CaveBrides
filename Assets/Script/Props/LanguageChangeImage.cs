using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//需要在中英切换的时候切换图片的
public class LanguageChangeImage : MonoBehaviour
{
    [SerializeField]
    private Image cur_img;
    [SerializeField]
    private Sprite chinese_img, english_img;
    // Start is called before the first frame update
    void OnEnable()
    {
        ChangeLanguage(LanguageManager.Instance.IsChinese);
        LanguageManager.Instance.LanguageChange += ChangeLanguage;
    }

    private void OnDisable()
    {
        LanguageManager.Instance.LanguageChange -= ChangeLanguage;
    }

    public void ChangeLanguage(bool isChinese)
    {
        if (isChinese)
            cur_img.sprite = chinese_img;
        else
            cur_img.sprite = english_img;
    }
}
