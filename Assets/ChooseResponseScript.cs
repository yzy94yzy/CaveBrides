using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
public class ChooseResponseScript : MonoBehaviour
{
    [DllImport("user32.dll", EntryPoint = "keybd_event")]
    public static extern void keybd_event(
            byte bVk,    //�����ֵ ��Ӧ������ascll��ʮ����ֵ
            byte bScan,// 0
            int dwFlags,  //0 Ϊ���£�1��ס��2Ϊ�ͷ�
            int dwExtraInfo  // 0
        );

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    { 
        if (Input.GetKeyDown(KeyCode.JoystickButton4))
        {
            //Debug.Log("up");
            keybd_event(38, 0, 0, 0);
            keybd_event(38, 0, 2, 0);
        } 
        else if (Input.GetKeyDown(KeyCode.JoystickButton5))
        {
            //Debug.Log("down");
            keybd_event(40, 0, 0, 0);
            keybd_event(40, 0, 2, 0);
        }  
    }
}
