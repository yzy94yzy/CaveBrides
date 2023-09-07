using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.transform.GetComponent<UnityEngine.UI.Image>().alphaHitTestMinimumThreshold = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
