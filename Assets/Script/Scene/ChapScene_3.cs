using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapScene_3 : MonoBehaviour
{
    [SerializeField]
    private GameObject river_inside_scene, river_outside_scene, sisterroom_inside_scene, sisterroom_outside_scene;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SwitchInsideOrOutside()
    {
        if (river_inside_scene.activeInHierarchy || river_outside_scene.activeInHierarchy)
        {
            if (PlayerManager.Instance.IsInside)
            {
                river_outside_scene.SetActive(false);
                river_inside_scene.SetActive(true);
            }
            else
            {
                river_inside_scene.SetActive(false);
                river_outside_scene.SetActive(true);
            }
        }
        else if (sisterroom_inside_scene.activeInHierarchy || sisterroom_outside_scene.activeInHierarchy)
        {
            if (PlayerManager.Instance.IsInside)
            {
                sisterroom_outside_scene.SetActive(false);
                sisterroom_inside_scene.SetActive(true);
            }
            else
            {
                sisterroom_inside_scene.SetActive(false);
                sisterroom_outside_scene.SetActive(true);
            }
        }
        else
        {
            Debug.LogError("玩家不在第三章");
        }
    }
}
