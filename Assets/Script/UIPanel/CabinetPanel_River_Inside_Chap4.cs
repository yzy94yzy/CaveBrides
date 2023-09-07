using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CabinetPanel_River_Inside_Chap4 : MonoBehaviour
{
    public float show_time;
    [SerializeField]
    private GameObject unlockTrigger, exitBtn;
    [SerializeField]
    private Button plate_left, plate_right;
    [SerializeField]
    private Image stick_desk, stick_in, hint, hint_english;
    [SerializeField]
    private AudioSource disappearAudio, chaxiangAudio;

    // Start is called before the first frame update
    void Start()
    {
        plate_left.onClick.AddListener(ShowTributeBamboo);
        plate_right.onClick.AddListener(ShowTributeDuckHeart);
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

    //����ë��ķ���
    public void ShowTributeBamboo()
    {
        BanAllButton();
        PropManager.Instance.UseProp(PropType.Bamboo);
        plate_left.enabled = false;
        plate_left.transform.GetChild(0).gameObject.SetActive(true);
        plate_left.transform.GetChild(0).gameObject.GetComponent<Image>().DOFade(1, show_time);
        CheckIsTributeAllHave();
    }

    //����Ѽ�ĵķ���
    public void ShowTributeDuckHeart()
    {
        BanAllButton();
        PropManager.Instance.UseProp(PropType.DuckHeart);
        plate_right.enabled = false;
        plate_right.transform.GetChild(0).gameObject.SetActive(true);
        plate_right.transform.GetChild(0).gameObject.GetComponent<Image>().DOFade(1, show_time);
        CheckIsTributeAllHave();
    }

    //�ڲ��Ŷ�����ʱ����ð�ť֮�󼤻�
    public void BanAllButton()
    {
        plate_left.enabled = false;
        plate_right.enabled = false;
    }

    public void ActiveButton()
    {
        if (plate_left.transform.GetChild(0).gameObject.activeInHierarchy)
            plate_right.enabled = true;
        else
            plate_left.enabled = true;
    }

    //����Ƿ�������Ʒ���Ѿ��׼�
    public void CheckIsTributeAllHave()
    {
        if (plate_left.transform.GetChild(0).gameObject.activeInHierarchy && plate_right.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            Invoke("HideTwoTribute", show_time);
        }
        else
            ActiveButton();
    }

    //�׼���������
    private void HideTwoTribute()
    {
        disappearAudio.Play();
        plate_left.transform.GetChild(0).gameObject.GetComponent<Image>().DOFade(0, show_time * 2);
        plate_right.transform.GetChild(0).gameObject.GetComponent<Image>().DOFade(0, show_time * 2);
        Invoke("UnlockHintTrigger", show_time * 2);
    }
    //�׼��궫�����ֶԻ�
    private void UnlockHintTrigger()
    {
        unlockTrigger.SetActive(false);
    }

    //�������ʾ��һϵ�ж���
    public void StickAndHint()
    {
        stick_desk.DOFade(0, show_time);
        Invoke("StickInShow", show_time);
    }
    private void StickInShow()
    {
        chaxiangAudio.Play();
        stick_desk.gameObject.SetActive(false);
        stick_in.gameObject.SetActive(true);
        stick_in.DOFade(1, show_time);
        Invoke("HintShow", show_time);
    }
    private void HintShow()
    {
        if (LanguageManager.Instance.IsChinese)
            hint.DOFade(1, show_time * 2);
        else
            hint_english.DOFade(1, show_time * 2);
        Invoke("ActiveExitBtn", show_time * 2);
    }

    private void ChangeLanguage(bool isChinese)
    {
        if (isChinese)
        {
            hint_english.DOFade(0, 0);
            hint.DOFade(1, 0);
        }
        else
        {
            hint_english.DOFade(1, 0);
            hint.DOFade(0, 0);
        }    
    }


    private void ActiveExitBtn()
    {
        exitBtn.SetActive(true);
    }
}
