using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//需要在切换语言的时候切换整个物体的
public class LanguageChangeObject : MonoBehaviour
{
    [SerializeField]
    private GameObject chinese_obj, english_obj;
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
        {
            chinese_obj.SetActive(true);
            english_obj.SetActive(false);
        }
        else
        {
            chinese_obj.SetActive(false);
            english_obj.SetActive(true);
        }
    }
}
