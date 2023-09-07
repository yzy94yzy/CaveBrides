using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class RayClickScript : MonoBehaviour
{
    public GameObject cursor;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //光标显形才可点击
        if (Input.GetKeyDown(KeyCode.JoystickButton0) && gameObject.GetComponent<CanvasGroup>().alpha == 1)
        {
            GameObject obj = GetFirstPickGameObject(cursor.transform.position);
            
            if (obj != null && obj.GetComponent<Button>() != null)
            {
                Button btn = obj.GetComponent<Button>();
                if (btn.enabled)
                {
                    btn.onClick.Invoke();
                }
            }
        }
    }

    public GameObject GetFirstPickGameObject(Vector3 position)
    {
        EventSystem eventSystem = EventSystem.current;
        PointerEventData pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = position;
        //射线检测ui
        List<RaycastResult> uiRaycastResultCache = new List<RaycastResult>();
        eventSystem.RaycastAll(pointerEventData, uiRaycastResultCache);
        if (uiRaycastResultCache.Count > 1)
            return uiRaycastResultCache[1].gameObject;
        return null;
    }
}
