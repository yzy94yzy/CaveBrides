using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoystickDragScript : MonoBehaviour
{
    public GameObject cursor;
    //private float cursorSpeed;
    private bool isDrag;
    private GameObject curObj;

    /*[SerializeField]
    private GameObject completePos;*/
    [SerializeField]
    private DoorGodPanel doorGodPanel;

    //ȡѼ����Ϸ
    [SerializeField]
    private DuckPanel duckPanel;
    [SerializeField]
    private DragPropInteractPanel dragPropInteractPanel;
    [SerializeField]
    private AudioSource cutMeat;
    private int windowPaperUseTime;
    private int knifeUseTime;

    // Start is called before the first frame update
    void Start()
    {
        //cursorSpeed = 600;
        isDrag = false;
        curObj = null;
        windowPaperUseTime = 1;
        knifeUseTime = 1;
    }

    // Update is called once per frame
    void Update()
    {
        handleCursor();
    }

    public void handleCursor()
    {
        //��ǰ������ק
        if (isDrag)
        {
            //��קʱ�����ֱ�B��
            if (Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                isDrag = false;
                //���µ�ʱ����λ��
                if (curObj != null && curObj.GetComponent<JoystickDragProp>() != null && curObj.GetComponent<JoystickDragProp>().CheckPosition())
                {
                    CompleteDrag();
                }
                //Debug.Log(curObj.name + " " + !curObj.GetComponent<JoystickDragProp>().CheckPosition() + " 2 " + curObj.GetComponent<DragDoorGodPiece>().IsCorrect);
                //����ȷ������ƴͼ�ƿ���
                if (curObj.tag == "DragDoorgod" && !curObj.GetComponent<JoystickDragProp>().CheckPosition() && curObj.GetComponent<DragDoorGodPiece>().IsCorrect)
                {
                    doorGodPanel.RemoveOnePiece();
                    curObj.GetComponent<DragDoorGodPiece>().IsCorrect = false;
                }
                curObj = null;
            }
            //��קʱ�ƶ����
            else if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                isDrag = false;
                curObj = null;
            }
            //��קʱֻ�������
            else
            {
                curObj.transform.position = transform.position;
            }
        }
        //��ǰû����ק
        else
        {
            //�����ֱ�B���ҹ������ʱ�ſɲ���
            if (Input.GetKeyDown(KeyCode.JoystickButton0) && gameObject.GetComponent<CanvasGroup>().alpha == 1)
            {
                //���߻�ȡ��ǰĿ��
                GameObject obj = GetFirstPickGameObject(cursor.transform.position);

                if (obj != null)
                {
                    //��ǰĿ���ǰ�ťʱ�����°�ť
                    if (obj.GetComponent<Button>() != null)
                    {
                        Button btn = obj.GetComponent<Button>();
                        if (btn.enabled)
                        {
                            btn.onClick.Invoke();
                        }
                    }
                    //��ǰĿ��ʱ����ק��Ʒʱ
                    else if (obj.CompareTag("DragProp") || obj.CompareTag("DragDoorgod"))
                    {
                        curObj = obj;
                        isDrag = true;
                    }
                }
            }
        }
    }

    public GameObject GetFirstPickGameObject(Vector3 position)
    {
        EventSystem eventSystem = EventSystem.current;
        PointerEventData pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = position;
        //���߼��ui
        List<RaycastResult> uiRaycastResultCache = new List<RaycastResult>();
        eventSystem.RaycastAll(pointerEventData, uiRaycastResultCache);
        if (uiRaycastResultCache.Count > 1)
            return uiRaycastResultCache[1].gameObject;
        return null;
    }

    public void CompleteDrag()
    {
        //Debug.Log("complete drag");
        if (windowPaperUseTime > 0 && curObj.name == "Windowpaper")
        {
            windowPaperUseTime -= 1;
            curObj.transform.position = curObj.GetComponent<JoystickDragProp>().completePos.transform.position;
            doorGodPanel.CompleteDoorGod();
        }
        else if (knifeUseTime > 0 && curObj.name == "Knife")
        {
            knifeUseTime -= 1;
            cutMeat.Play();
            duckPanel.MoveKnife();
            //curObj.transform.position = completePos.transform.position;
        }
        else if (curObj.name == "Earing")
        {
            curObj.transform.position = curObj.GetComponent<JoystickDragProp>().completePos.transform.position;
            dragPropInteractPanel.CompleteDrag();
        }
        else if (curObj.tag == "DragDoorgod")
        {
            curObj.transform.position = curObj.GetComponent<JoystickDragProp>().completePos.transform.position;
            //�������ƴͼ�Ƶ���ȷ��λ�òż�
            if (!curObj.GetComponent<DragDoorGodPiece>().IsCorrect)
            {
                doorGodPanel.CompleteOnePiece();
                curObj.GetComponent<DragDoorGodPiece>().IsCorrect = true;
            }
        }
    }
}
