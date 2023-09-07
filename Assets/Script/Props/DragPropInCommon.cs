using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragPropInCommon : DragProps
{
    //ͨ�����͵��ߴ���
    [SerializeField]
    private DragPropInteractPanel dragPropInteractPanel;
    // Update is called once per frame
    void Update()
    {
        
    }

    public override void CheckPosition()
    {
        if (Mathf.Sqrt((completePos.transform.position - transform.position).magnitude) < 10)
        {
            transform.position = completePos.position;
            dragPropInteractPanel.CompleteDrag();
        }
        else
        {
            transform.position = startPos.position;
        }
    }

}
