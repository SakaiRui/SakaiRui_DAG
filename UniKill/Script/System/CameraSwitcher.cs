using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    //メインカメラとサブカメラの情報をそれぞれ変数に入れる
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera subCamera;
    private float _repeatSpan;    //繰り返す間隔
    private float _timeElapsed;   //経過時間
    private bool SwitchOn = false;
    private void Start()
    {
        _repeatSpan = 0f;    //実行間隔を５に設定
        _timeElapsed = 0;   //経過時間をリセット
    }

    void Update()
    {
        //Enterキーを押したとき
        if (Input.GetKeyDown(KeyCode.R))
        {
            SwitchOn = true;
           
        }
        if(SwitchOn)
        {
            _timeElapsed += Time.deltaTime;     //時間をカウントする
                                                //経過時間が繰り返す間隔を経過したら
            if (_timeElapsed >= _repeatSpan)
            {
                //ここで処理を実行
                SwitchOn = false;
                _timeElapsed = 0;   //経過時間をリセットする
                //メインカメラとサブカメラを切り替える
                //Depthの差を変数depthDiffに入れる
                float depthDiff = mainCamera.depth - subCamera.depth;

                //メインカメラのDepthの方が大きいとき
                if (depthDiff > 0)
                {
                    //Depthの値を入れ替える
                    mainCamera.depth = -1;
                    subCamera.depth = 0;
                }
                //サブカメラのDepthの方が大きいとき
                else
                {
                    //Depthの値を入れ替える
                    mainCamera.depth = 0;
                    subCamera.depth = -1;
                }
            }
        }

    }

    public void Switchcamera()
    {
        //メインカメラとサブカメラを切り替える
        //Depthの差を変数depthDiffに入れる
        float depthDiff = mainCamera.depth - subCamera.depth;

        //メインカメラのDepthの方が大きいとき
        if (depthDiff > 0)
        {
            //Depthの値を入れ替える
            mainCamera.depth = -1;
            subCamera.depth = 0;
        }
        //サブカメラのDepthの方が大きいとき
        else
        {
            //Depthの値を入れ替える
            mainCamera.depth = 0;
            subCamera.depth = -1;
        }
    }
}
