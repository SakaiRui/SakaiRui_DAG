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
        //Vector3 eulerAngles = transform.eulerAngles; // ���[�J���ϐ��Ɋi�[
        //eulerAngles.z = 0; // ���[�J���ϐ��Ɋi�[�����l���㏑��
        transform.eulerAngles = vector; // ���[�J���ϐ�����
    }
}
