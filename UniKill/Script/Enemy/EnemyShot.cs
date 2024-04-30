using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShot : MonoBehaviour
{
    [SerializeField]
    private int AttackPower = 10;
    [SerializeField]
    private GameObject Effect;
    private Player player;

    private bool DebugColloder = false;

    private bool Hit = false;

    private float _repeatSpan;    //�J��Ԃ��Ԋu
    private float _timeElapsed;   //�o�ߎ���
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        _repeatSpan = 0.1f;    //���s�Ԋu���T�ɐݒ�
        _timeElapsed = 0;   //�o�ߎ��Ԃ����Z�b�g
    }

    // Update is called once per frame
    void Update()
    {
        _timeElapsed += Time.deltaTime;     //���Ԃ��J�E���g����
        //�o�ߎ��Ԃ��J��Ԃ��Ԋu���o�߂�����
        if (_timeElapsed >= _repeatSpan)
        {
            //�����ŏ��������s
            Instantiate(Effect, transform.position, Quaternion.identity);
            _timeElapsed = 0;   //�o�ߎ��Ԃ����Z�b�g����
        }
        if (DebugColloder)
        {
            GetComponent<MeshRenderer>().enabled = true;
            DebugColloder = false;
        }
        //�������̏���
        if (Hit)
        {

        }
    }

   

    private void OnTriggerStay(Collider other)
    {
        //���������Q�[���I�u�W�F�N�g��Enemy�^�O�������Ƃ�
        if (!Hit && other.gameObject.CompareTag("Player") && !player.isInvincibility())
        {

            //Instantiate(prefab ,gameObject.transform.position, Quaternion.identity);

            Debug.Log("�v���C���[�ɍU���q�b�g");

            GetComponent<Collider>().enabled = false;
            //forceField.enabled = false;
            player.MinusHp(AttackPower);
            Hit = true;
        }
    }

    public void SetPower(int num)
    {
        AttackPower = num;
    }

    public void SetDebugColloder(bool num)
    {
        DebugColloder = num;
    }
}
