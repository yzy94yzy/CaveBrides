using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //�����ý���
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            UIManager.Instance.OpenSettingPanel();
        }
    }
}
