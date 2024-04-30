using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//カーソルの位置を取得
public class Pointer : MonoBehaviour
{
    private Vector3 mouse;
    public GameObject MainCamera;
    
    void Start()
    {
        
    }

    void Update()
    {
        
        mouse = Input.mousePosition;
        mouse.z = 1.0f;
        mouse = Camera.main.ScreenToWorldPoint(mouse);
        
        this.transform.position = new Vector3(mouse.x, mouse.y, mouse.z);
        
    }
}
