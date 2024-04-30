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
        //objb,obja�̈ʒu�֌W�A�p�x
        var vec = (targetObj.transform.position - RockonTarget.transform.position);
        // target�̈ړ��ʕ��A�������ړ�����
        this.transform.position = RockonTarget.transform.position + (vec * 1f) + transform.forward * -8.0f + targetObj.transform.up * 3.0f;
        targetPos = targetObj.transform.position;

        center = (RockonTarget.transform.position + targetPos) * 0.5f;
        //Debug.Log(vec);

        Vector3 EnemyPosition = RockonTarget.transform.position;
        EnemyPosition.y += 2.0f;

        // �Ώە��Ǝ������g�̍��W����x�N�g�����Z�o
        Vector3 vector3 = EnemyPosition - this.transform.position;
        // �����㉺�����̉�]�͂��Ȃ�(Base�I�u�W�F�N�g�������痣��Ȃ��悤�ɂ���)�悤�ɂ�������Έȉ��̂悤�ɂ���B
        // vector3.y = 0f;

        // Quaternion(��]�l)���擾
        Quaternion quaternion = Quaternion.LookRotation(vector3);
        // �Z�o������]�l�����̃Q�[���I�u�W�F�N�g��rotation�ɑ��
        this.transform.rotation = quaternion;
    }
}
