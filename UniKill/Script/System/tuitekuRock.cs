using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tuitekuRock : MonoBehaviour
{
    // Start is called before the first frame update

    GameObject targetObj;
    Vector3 targetPos;
    private GameObject RockonTarget;
    private Vector3 center;
    // Start is called before the first frame update
    void Start()
    {
        targetObj = GameObject.Find("Player");
        targetPos = targetObj.transform.position;
        RockonTarget = GameObject.Find("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        //objb,objaの位置関係、角度
        var vec = (targetObj.transform.position - RockonTarget.transform.position);
        // targetの移動量分、自分も移動する
        this.transform.position = RockonTarget.transform.position + (vec * 1f) + transform.forward * -8.0f + targetObj.transform.up * 3.0f;
        targetPos = targetObj.transform.position;

        center = (RockonTarget.transform.position + targetPos) * 0.5f;
        //Debug.Log(vec);

        Vector3 EnemyPosition = RockonTarget.transform.position;
        EnemyPosition.y += 2.0f;

        // 対象物と自分自身の座標からベクトルを算出
        Vector3 vector3 = EnemyPosition - this.transform.position;
        // もし上下方向の回転はしない(Baseオブジェクトが床から離れないようにする)ようにしたければ以下のようにする。
        // vector3.y = 0f;

        // Quaternion(回転値)を取得
        Quaternion quaternion = Quaternion.LookRotation(vector3);
        // 算出した回転値をこのゲームオブジェクトのrotationに代入
        this.transform.rotation = quaternion;
    }
}
