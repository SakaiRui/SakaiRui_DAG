using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisLocker : MonoBehaviour
{
    [SerializeField] private Vector3 vector;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 eulerAngles = transform.eulerAngles; // ローカル変数に格納
        //eulerAngles.z = 0; // ローカル変数に格納した値を上書き
        transform.eulerAngles = vector; // ローカル変数を代入
    }
}
