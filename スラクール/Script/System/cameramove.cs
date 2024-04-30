using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameramove : MonoBehaviour
{
    //プレイヤーを変数に格納
    public GameObject Player;
   
    private Vector3 offset;      //相対距離取得用

    //カメラがxzどちらの軸を基準に見ているか
    public float xz;

    // Use this for initialization
    void Start()
    {
        xz = transform.localEulerAngles.y;
       
       
    }

    // Update is called once per frame
    void Update()
    {
        
        //プレイヤー位置情報
        Vector3 playerPos = Player.transform.position;
        
        xz = transform.localEulerAngles.y;

        //回転させる角度
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
           
            transform.RotateAround(playerPos, Vector3.up, 90f);
           
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
          
            transform.RotateAround(playerPos, Vector3.up, -90f);
            
        }
        
    }
}
