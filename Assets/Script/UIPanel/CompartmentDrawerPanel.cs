using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CompartmentDrawerPanel : MonoBehaviour
{
    public float drawerChange_time;
   [SerializeField]
    private CanvasGroup drawers_open;
    [SerializeField]
    private Image drawers_close;
    //��֤�ƶ������в��ᷢ����ť����
    [SerializeField]
    private List<Button> buttonList;
    public void ChangeAllButtonState(bool state)
    {

    }

    //��������
    public void OpenDrawers()
    {
        ChangeAllButtonState(false);
        drawers_open.gameObject.SetActive(true);
        drawers_close.DOFade(0, drawerChange_time);
        drawers_open.DOFade(1, drawerChange_time);
        Invoke("OpenDrawersEnd", drawerChange_time);
    }

    private void OpenDrawersEnd()
    {
        drawers_close.gameObject.SetActive(false);
        ChangeAllButtonState(true);
    }
}
