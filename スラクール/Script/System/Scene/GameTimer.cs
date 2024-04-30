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
        /*Time.~~�Ŏ��Ԃ𐔂���
         * .time�Ńf���V�[���J�n������̌o�ߎ���
         * .deltaTime�Ŏ���Update�����s���邽�߂̊Ԋu
         * ��1�t���[����0.1�ړ����邾�ƁA���������ō������܂�Ă��܂��B
         * ���̍������܂�Ȃ��悤��deltaTime�̌v�Z�ł́A
         * �u1�b�Ԃɂǂꂾ���ω����邩�v�ōl����B
         *  �@ 1FPS�̏ꍇ�@deltaTime = 1.0 (1�b��1�񏈗�)
         *  �A50FPS�̏ꍇ�@deltaTime = 0.02(1�b��50�񏈗�)
         *  �ړ��� = 1�b�ňړ������ * 1��̏������� * ������
         *  ���̌v�Z���Ōv�Z����ƁA�@���A���ړ��ʂ������ɂȂ�
         */

        countTime += Time.deltaTime;
        timerText.text = "TIME:" + countTime.ToString("n1");
    

        time = countTime;
    }
}
