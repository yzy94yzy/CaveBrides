/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowFirepitPanel : MonoBehaviour
{
    //现在操作的firepit下标
    private int curIndex;

    //需要配置的图片
    [SerializeField]
    private Image firepit;
    [SerializeField]
    private Button fireBtn, nofireBtn;

    //所有火焰台图片
    [SerializeField]
    private List<Sprite> allFirepitList;


    // Start is called before the first frame update
    void Start()
    {
        fireBtn.onClick.AddListener(OnFireBtnClick);
        nofireBtn.onClick.AddListener(OnNoFireBtnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowFirepitDetailPanel(int index)
    {
        firepit.sprite = allFirepitList[index];
        curIndex = index;
    }

    public void OnFireBtnClick()
    {
        gameObject.SetActive(false);
        GameAudioManager.Instance.PlayFireAudio();
        //PropManager.Instance.ChangeFirepitState(curIndex, true);
    }

    public void OnNoFireBtnClick()
    {
        gameObject.SetActive(false);
        GameAudioManager.Instance.PlayNoFireAudio();
        //PropManager.Instance.ChangeFirepitState(curIndex, false);
    }
}
*/