using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera MainCamera;
    private Camera LockOnCamera;

    private RockonCamera rockonCamera;

    private void Start()
    {
        MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        LockOnCamera = GameObject.Find("Rockon Camera").GetComponent<Camera>();
        rockonCamera = GameObject.Find("Rockon Camera").GetComponent<RockonCamera>();
    }
    void LateUpdate()
    {
        if(rockonCamera.Getrock())
        {
            // ��]���J�����Ɠ���������
            transform.rotation = LockOnCamera.transform.rotation;
        }
        else
        {
            // ��]���J�����Ɠ���������
            transform.rotation = MainCamera.transform.rotation;
        }
        
    }
}
