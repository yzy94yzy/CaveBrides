using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SisterWallPanel : MonoBehaviour
{
    public float moveWall_time, close_time;
    [SerializeField]
    private Button stonestatue_desk;
    [SerializeField]
    private RectTransform wallBackground, moveWall_endPos;
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

    }

    public void MoveToDown()
    {
        wallBackground.DOMoveY(moveWall_endPos.position.y, moveWall_time);
        Invoke("ActiveStoneStatueInteract", moveWall_time);
        Invoke("ResetPositionToDown", moveWall_time);
    }

    //防止分辨率切换导致镜头下移不成功
    private void ResetPositionToDown()
    {
        wallBackground.transform.position = new Vector3(wallBackground.position.x, moveWall_endPos.position.y, wallBackground.position.z);
    }

    private void ActiveStoneStatueInteract()
    {
        stonestatue_desk.enabled = true;
    }

    public void HideSelf()
    {
        UIManager.Instance.ClosePanelSlowly(gameObject.GetComponent<CanvasGroup>());
    }
}
