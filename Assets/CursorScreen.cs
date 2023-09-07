using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CursorScreen : MonoBehaviour
{
    private float cursorSpeed = 700;
    //public Button btn;
    //public GameObject cursor;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        gameObject.GetComponent<CanvasGroup>().alpha = 0;
    }

    private void Awake()
    {
        //Cursor.visible = false;    
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal3");
        float v = Input.GetAxisRaw("Vertical3");
        if (h + v != 0)
        {
            //Debug.Log("yes");
            gameObject.GetComponent<CanvasGroup>().alpha = 1;
            transform.Translate(Vector3.right * h * cursorSpeed * Time.deltaTime);
            transform.Translate(Vector3.up * v * cursorSpeed * Time.deltaTime);
        }
        gameObject.transform.position = new Vector3(Mathf.Clamp(gameObject.transform.position.x, 40, Screen.width - 40),
                                           Mathf.Clamp(gameObject.transform.position.y, 40, Screen.height - 40),
                                           gameObject.transform.position.z);

        //transform.position = Input.mousePosition;
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            gameObject.GetComponent<CanvasGroup>().alpha = 0;
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
            return uiRaycastResultCache[0].gameObject;
        return null;
    }
}
