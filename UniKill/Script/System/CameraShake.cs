using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    //　Time.timeScaleに設定する値
    [SerializeField]
    private float timeScale = 0.1f;

    //　Time.timeScaleに設定する値
    [SerializeField]
    private float speed = 0.001f;

    //　時間を遅くしている時間
    [SerializeField]
    private float slowTime = 0.5f;
    //　経過時間
    private float elapsedTime = 0f;
    //　時間を遅くしているかどうか
    private bool isSlowDown = false;

    private Vector3 mytransform;

    GameObject targetObj;
    Vector3 targetPos;

    private void Start()
    {
        mytransform = transform.position;
        targetObj = GameObject.Find("Player");
        targetPos = targetObj.transform.position;
    }

    void Update()
    {
        //　スローダウンフラグがtrueの時は時間計測
        if (isSlowDown)
        {

            elapsedTime += Time.unscaledDeltaTime;
            if (elapsedTime >= slowTime / 2)
            {
                if(elapsedTime >= slowTime * (3 / 4))
                {
                    transform.position -= Vector3.right * speed;
                    transform.position -= Vector3.up * speed;
                }
                else
                {
                    transform.position += Vector3.right * speed;
                    transform.position += Vector3.up * speed;
                }
                //transform.position += Vector3.right * speed;
                //transform.position += Vector3.up * speed;
            }
            else
            {
                if (elapsedTime <= slowTime / 4)
                {
                    transform.position -= Vector3.right * speed;
                    transform.position -= Vector3.up * speed;
                }
                else
                {
                    transform.position += Vector3.right * speed;
                    transform.position += Vector3.up * speed;
                }
                //transform.position -= Vector3.right * speed;
                //transform.position -= Vector3.up * speed;
            }
            if (elapsedTime >= slowTime)
            {
                // targetの移動量分、自分（カメラ）も移動する
               // transform.position += targetObj.transform.position - targetPos;
                
                SetNormalTime();
            }
        }
    }
    //　時間を遅らせる処理
    public void SlowDown(int num)
    {
        if(!isSlowDown)
        {
            if(num == 0)
            {
                timeScale = 0.15f;
                speed = 0.1f;
                slowTime = 0.2f;
            }
            if (num == 1)
            {
                timeScale = 0.1f;
                speed = 0.1f;
                slowTime = 0.4f;
            }
            if (num == 2)
            {
                timeScale = 0.00f;
                speed = 0.1f;
                slowTime = 0.4f;
            }
            elapsedTime = 0f;
            
            Time.timeScale = timeScale;
            isSlowDown = true;
        }
        
    }
    //　時間を元に戻す処理
    public void SetNormalTime()
    {
        Time.timeScale = 1f;
        isSlowDown = false;
        transform.localPosition = Vector3.zero;
    }
    public bool isSlow()
    {
        return isSlowDown;
    }
}
