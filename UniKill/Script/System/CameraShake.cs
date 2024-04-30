using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    //�@Time.timeScale�ɐݒ肷��l
    [SerializeField]
    private float timeScale = 0.1f;

    //�@Time.timeScale�ɐݒ肷��l
    [SerializeField]
    private float speed = 0.001f;

    //�@���Ԃ�x�����Ă��鎞��
    [SerializeField]
    private float slowTime = 0.5f;
    //�@�o�ߎ���
    private float elapsedTime = 0f;
    //�@���Ԃ�x�����Ă��邩�ǂ���
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
        //�@�X���[�_�E���t���O��true�̎��͎��Ԍv��
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
                // target�̈ړ��ʕ��A�����i�J�����j���ړ�����
               // transform.position += targetObj.transform.position - targetPos;
                
                SetNormalTime();
            }
        }
    }
    //�@���Ԃ�x�点�鏈��
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
    //�@���Ԃ����ɖ߂�����
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
