using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragScript : MonoBehaviour
{
    public List<GameObject> dragObj;
    public List<GameObject> btnObj;
    public GameObject cursor;
    private RectTransform rtCursor;
    // Start is called before the first frame update
    void Start()
    {
        rtCursor = cursor.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            GameObject obj = GetFirstPickGameObject(cursor.transform.position);
            //Debug.Log(GetFirstPickGameObject(cursor.transform.position).name);
            if (obj.name == "Dolls4")
            {
                obj.transform.Translate(0, -100, 0);
            }
            if (obj.name == "Dolls1")
            {
                obj.transform.Translate(0, 100, 0);
            }
            if (obj.name == "Dolls2")
            {
                obj.transform.Translate(-100, 0, 0);
            }
            if (obj.name == "Dolls3")
            {
                obj.transform.Translate(100, 0, 0);
            }
            if (obj.name == "Stick")
            {
                obj.GetComponent<Button>().onClick.Invoke();
            }
            if (obj.name == "ExitAndActiveStoneBtn")
            {
                obj.GetComponent<Button>().onClick.Invoke();
            }
        }

        /*foreach(GameObject obj in dragObj)
        {
            RectTransform rtDragObj = obj.GetComponent<RectTransform>();
            if (RectTransToScreenPos(rtCursor, null).Overlaps(RectTransToScreenPos(rtDragObj, null))) 
            {
                if (Input.GetKeyDown(KeyCode.JoystickButton0))
                {
                    obj.transform.Translate(-100, 0, 0);
                }
            }
        }

        foreach (GameObject obj in btnObj)
        {
            RectTransform rtBtnObj = obj.GetComponent<RectTransform>();
            if (RectTransToScreenPos(rtCursor, null).Overlaps(RectTransToScreenPos(rtBtnObj, null)))
            {
                if (Input.GetKeyDown(KeyCode.JoystickButton0) && obj.GetComponent<Button>().enabled)
                {
                    obj.GetComponent<Button>().onClick.Invoke();
                    obj.GetComponent<Button>().enabled = false;
                }
            }
        }*/
    }

    public static Rect RectTransToScreenPos(RectTransform rt, Camera cam)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        Vector2 v0 = RectTransformUtility.WorldToScreenPoint(cam, corners[0]);
        Vector2 v1 = RectTransformUtility.WorldToScreenPoint(cam, corners[2]);
        Rect rect = new Rect(v0, v1 - v0);
        //Debug.Log(rect);
        return rect;
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
