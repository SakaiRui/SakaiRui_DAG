using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    Text timerText;
    float countTime;
    
    public static float time;
    // Start is called before the first frame update
    void Start()
    {
        timerText = GetComponent<Text>();
        timerText.text = "TIME:" + countTime.ToString("n1");


    }

    // Update is called once per frame
    void /*Fixed*/Update()
    {
        /*Time.~~で時間を数える
         * .timeでデモシーン開始時からの経過時間
         * .deltaTimeで次のUpdateを実行するための間隔
         * ※1フレームで0.1移動するだと、処理落ちで差が生まれてしまう。
         * その差が生まれないようにdeltaTimeの計算では、
         * 「1秒間にどれだけ変化するか」で考える。
         *  ① 1FPSの場合　deltaTime = 1.0 (1秒に1回処理)
         *  ②50FPSの場合　deltaTime = 0.02(1秒に50回処理)
         *  移動量 = 1秒で移動する量 * 1回の処理時間 * 処理回数
         *  ↑の計算式で計算すると、①も②も移動量が同じになる
         */

        countTime += Time.deltaTime;
        timerText.text = "TIME:" + countTime.ToString("n1");
    

        time = countTime;
    }
}
