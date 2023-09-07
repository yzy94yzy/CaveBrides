using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropBarControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //背包打开/关闭操作
        if ((Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.JoystickButton2)) && PlayerManager.Instance.PropBarIsUnlock)
        {
            UIManager.Instance.OpenAndClosePropBar();
        }
    }
}
