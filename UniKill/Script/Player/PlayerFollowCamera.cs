using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowCamera : MonoBehaviour
{
    GameObject targetObj;
    Vector3 targetPos;
    private bool rock = false;
    private GameObject RockonTarget;
    private GameObject SearchCircle;
    private Vector3 center;
    private Vector3 Origin; // cameraの座標
    private float camDistance, camHeight;
    void Start()
    {
        targetObj = GameObject.Find("Player");
        targetPos = targetObj.transform.position;
        Origin = transform.position - targetPos;
        camDistance = transform.position.z - targetPos.z;
        camHeight = transform.position.y - targetPos.y;
    }

    void Update()
    {
        

        // マウスの左クリックを押している間
        
            // targetの移動量分、自分（カメラ）も移動する
            //transform.position += targetObj.transform.position - targetPos;
            //targetPos = targetObj.transform.position;
            //// マウスの移動量
            //float mouseInputX = Input.GetAxis("Mouse X");
            //float mouseInputY = Input.GetAxis("Mouse Y");
            //// targetの位置のY軸を中心に、回転（公転）する
            //transform.RotateAround(targetPos, Vector3.up, mouseInputX * Time.deltaTime * 200f);
            //// カメラの垂直移動（※角度制限なし、必要が無ければコメントアウト）
            ////transform.RotateAround(targetPos, transform.right, mouseInputY * Time.deltaTime * 200f);
       
    }

  
}
