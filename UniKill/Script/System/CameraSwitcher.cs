using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    //���C���J�����ƃT�u�J�����̏������ꂼ��ϐ��ɓ����
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera subCamera;
    private float _repeatSpan;    //�J��Ԃ��Ԋu
    private float _timeElapsed;   //�o�ߎ���
    private bool SwitchOn = false;
    private void Start()
    {
        _repeatSpan = 0f;    //���s�Ԋu���T�ɐݒ�
        _timeElapsed = 0;   //�o�ߎ��Ԃ����Z�b�g
    }

    void Update()
    {
        //Enter�L�[���������Ƃ�
        if (Input.GetKeyDown(KeyCode.R))
        {
            SwitchOn = true;
           
        }
        if(SwitchOn)
        {
            _timeElapsed += Time.deltaTime;     //���Ԃ��J�E���g����
                                                //�o�ߎ��Ԃ��J��Ԃ��Ԋu���o�߂�����
            if (_timeElapsed >= _repeatSpan)
            {
                //�����ŏ��������s
                SwitchOn = false;
                _timeElapsed = 0;   //�o�ߎ��Ԃ����Z�b�g����
                //���C���J�����ƃT�u�J������؂�ւ���
                //Depth�̍���ϐ�depthDiff�ɓ����
                float depthDiff = mainCamera.depth - subCamera.depth;

                //���C���J������Depth�̕����傫���Ƃ�
                if (depthDiff > 0)
                {
                    //Depth�̒l�����ւ���
                    mainCamera.depth = -1;
                    subCamera.depth = 0;
                }
                //�T�u�J������Depth�̕����傫���Ƃ�
                else
                {
                    //Depth�̒l�����ւ���
                    mainCamera.depth = 0;
                    subCamera.depth = -1;
                }
            }
        }

    }

    public void Switchcamera()
    {
        //���C���J�����ƃT�u�J������؂�ւ���
        //Depth�̍���ϐ�depthDiff�ɓ����
        float depthDiff = mainCamera.depth - subCamera.depth;

        //���C���J������Depth�̕����傫���Ƃ�
        if (depthDiff > 0)
        {
            //Depth�̒l�����ւ���
            mainCamera.depth = -1;
            subCamera.depth = 0;
        }
        //�T�u�J������Depth�̕����傫���Ƃ�
        else
        {
            //Depth�̒l�����ւ���
            mainCamera.depth = 0;
            subCamera.depth = -1;
        }
    }
}
