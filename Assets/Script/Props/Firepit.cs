using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Firepit : MonoBehaviour
{
    //当前火台的下标
    public int curIndex;
    //当前火焰台是否一点燃
    private bool isFire;

    [SerializeField]
    private Image firepit;
    [SerializeField]
    private Sprite fire, nofire;
    [SerializeField]
    private Button switchBtn;
    [SerializeField]
    private CabinetPanel cabinetPanel;
    [SerializeField]
    private AudioSource fireAudio;

    private void Start()
    {
        switchBtn.onClick.AddListener(OnSwitchClick);
    }

    //点击切换当前火焰台状态
    public void OnSwitchClick()
    {
        if (this.isFire == true)
            return;
        isFire = true;
        fireAudio.Play();
        cabinetPanel.ChangeFirepitState(curIndex, true);
    }

    public void SwitchStatue(bool isFire)
    {
        this.isFire = isFire;
        if (isFire)
            firepit.sprite = fire;
        else
            firepit.sprite = nofire;
    }

    public void BanInteractive()
    {
        switchBtn.enabled = false;
    }
}
