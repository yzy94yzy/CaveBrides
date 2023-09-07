using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VolumeDragScript : MonoBehaviour
{
    [SerializeField]
    private GameObject cursor;
    private bool isDrag;
    private GameObject curObj;
    // Start is called before the first frame update
    void Start()
    {
        isDrag = false;
        curObj = null;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            //isDrag = false;
            if (isDrag)
                {
                isDrag = false;
            }
            else
            {
                GameObject obj = GetFirstPickGameObject(cursor.transform.position);
                if (obj.CompareTag("Handle"))
                {
                    isDrag = true;
                    curObj = obj;
                }
            }
            
        }*/
        if (Input.GetKeyDown(KeyCode.JoystickButton0) && !isDrag)
        {
            GameObject obj = GetFirstPickGameObject(cursor.transform.position);
            Button btn = obj.GetComponent<Button>();
            if (btn != null && btn.enabled)
            {
                btn.onClick.Invoke();
            }
            else if (obj.CompareTag("Handle") && !isDrag)
            {
                curObj = obj;
                //Debug.Log(curObj.name);
                isDrag = !isDrag;
                /*if (!isDrag)
                {
                    
                }*/
            }

        }

        if (isDrag)
        {
            float x = curObj.transform.position.x, y = curObj.transform.position.y, z = curObj.transform.position.z;
            curObj.transform.position = new Vector3(cursor.transform.position.x, y, z);
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0 || Input.GetKeyDown(KeyCode.JoystickButton1))
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
        //…‰œﬂºÏ≤‚ui
        List<RaycastResult> uiRaycastResultCache = new List<RaycastResult>();
        eventSystem.RaycastAll(pointerEventData, uiRaycastResultCache);
        if (uiRaycastResultCache.Count > 0)
            return uiRaycastResultCache[1].gameObject;
        return null;
    }
}
