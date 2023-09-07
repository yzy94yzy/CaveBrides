using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoystickEyeScript : MonoBehaviour
{
    public GameObject cursor;
    //private float cursorSpeed;
    private bool isDrag;
    private GameObject curObj;

    [SerializeField]
    private Transform judgePos;
    [SerializeField]
    private GameObject completeLeftPos;
    [SerializeField]
    private GameObject completeRightPos;
    //×óÓÒÑÛµÄÅÐ¶¨
    [SerializeField]
    private GameObject left_eye, right_eye;
    [SerializeField]
    private PaperPeoplePanel paperPeoplePanel;
    [SerializeField]
    private AudioSource addEyeAudio;




    // Start is called before the first frame update
    void Start()
    {
        //cursorSpeed = 600;
        isDrag = false;
        curObj = null;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0) && cursor.GetComponent<CanvasGroup>().alpha == 1)
        {
            GameObject obj = GetFirstPickGameObject(cursor.transform.position);

            if (obj != null)
            {
                if (obj.GetComponent<Button>() != null)
                {
                    Button btn = obj.GetComponent<Button>();
                    if (btn.enabled)
                    {
                        btn.onClick.Invoke();
                    }
                }
                else if (obj.CompareTag("DragProp"))
                {
                    curObj = obj;
                    //Debug.Log(curObj.name);
                    isDrag = !isDrag;
                    if (!isDrag)
                    {
                        if (completeLeftPos != null && Mathf.Sqrt((completeLeftPos.transform.position - judgePos.position).magnitude) < 7)
                        {
                            if (!left_eye.activeInHierarchy)
                            {
                                addEyeAudio.Play();
                                left_eye.SetActive(true);
                            }
                            if (left_eye.activeInHierarchy && right_eye.activeInHierarchy)
                            {
                                paperPeoplePanel.DollsComplete();
                                //PenMove();
                            }
                        }
                        else if (completeRightPos != null && Mathf.Sqrt((completeRightPos.transform.position - judgePos.position).magnitude) < 7)
                        {
                            if (!right_eye.activeInHierarchy)
                            {
                                addEyeAudio.Play();
                                right_eye.SetActive(true);
                            }
                            if (left_eye.activeInHierarchy && right_eye.activeInHierarchy)
                            {
                                paperPeoplePanel.DollsComplete();
                                //PenMove();
                            }
                        }
                    }
                }
            }
        }

        if (isDrag)
        {
            float h = Input.GetAxisRaw("Horizontal3");
            float v = Input.GetAxisRaw("Vertical3");
            curObj.transform.Translate(Vector3.right * h * 700 * Time.deltaTime);
            curObj.transform.Translate(Vector3.up * v * 700 * Time.deltaTime);
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                isDrag = false;
            }
        }
    }

    public GameObject GetFirstPickGameObject(Vector3 position)
    {
        EventSystem eventSystem = EventSystem.current;
        PointerEventData pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = position;
        //ÉäÏß¼ì²âui
        List<RaycastResult> uiRaycastResultCache = new List<RaycastResult>();
        eventSystem.RaycastAll(pointerEventData, uiRaycastResultCache);
        if (uiRaycastResultCache.Count > 1)
            return uiRaycastResultCache[1].gameObject;
        return null;
    }
}
