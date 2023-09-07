using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

//让提示图片渐显的方法
public class ShowHintAnim : MonoBehaviour
{
    public float change_time;
    [SerializeField]
    private Image hint, hint_english;
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
        if (hint.color.a == 1 || hint_english.color.a == 1)
            ChangeLanguage(LanguageManager.Instance.IsChinese);
    }

    private void OnDisable()
    {
        LanguageManager.Instance.LanguageChange -= ChangeLanguage;
    }

    private void ChangeLanguage(bool isChinese)
    {
        if (isChinese)
        {
            hint_english.gameObject.SetActive(false);
            hint.gameObject.SetActive(true);
            hint.DOFade(1, 0);
        }
        else
        {
            hint.gameObject.SetActive(false);
            hint_english.gameObject.SetActive(true);
            hint_english.DOFade(1, 0);
        }
    }

    public void ShowHintSlowly()
    {
        if (LanguageManager.Instance.IsChinese)
        {
            hint.gameObject.SetActive(true);
            hint.DOFade(1, change_time);
        }
        else
        {
            hint_english.gameObject.SetActive(true);
            hint_english.DOFade(1, change_time);
        }
    }
}
