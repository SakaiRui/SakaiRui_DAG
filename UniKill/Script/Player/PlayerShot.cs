using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShot : MonoBehaviour
{
    [SerializeField]
    private int AttackPower = 10;
    [SerializeField]
    private GameObject Effect;
    private Enemy enemy;

    private bool DebugColloder = false;

    private bool Hit = false;

    private float _repeatSpan;    //�J��Ԃ��Ԋu
    private float _timeElapsed;   //�o�ߎ���
    private CameraShake timeManager;
    private RockonCamera rockonCamera;
    private CameraShake subtimeManager;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GameObject.Find("Enemy").GetComponent<Enemy>();
        _repeatSpan = 0.1f;    //���s�Ԋu���T�ɐݒ�
        _timeElapsed = 0;   //�o�ߎ��Ԃ����Z�b�g
        timeManager = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        rockonCamera = GameObject.Find("Rockon Camera").GetComponent<RockonCamera>();
        subtimeManager = GameObject.Find("Rockon Camera").GetComponent<CameraShake>();
        // Instantiate(Effect, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        //_timeElapsed += Time.deltaTime;     //���Ԃ��J�E���g����
        ////�o�ߎ��Ԃ��J��Ԃ��Ԋu���o�߂�����
        //if (_timeElapsed >= _repeatSpan)
        //{
        //    //�����ŏ��������s
        //    Instantiate(Effect, transform.position, Quaternion.identity);
        //    _timeElapsed = 0;   //�o�ߎ��Ԃ����Z�b�g����
        //}
        if (DebugColloder)
        {
            GetComponent<MeshRenderer>().enabled = true;
            DebugColloder = false;
        }
        ////�������̏���
        //if (Hit)
        //{

        //}
    }

   

    private void OnTriggerStay(Collider other)
    {
        //���������Q�[���I�u�W�F�N�g��Enemy�^�O�������Ƃ�
        if (!Hit && other.gameObject.CompareTag("Enemy"))
        {

            //Instantiate(prefab ,gameObject.transform.position, Quaternion.identity);

            Debug.Log("�v���C���[�ɍU���q�b�g");
            if (!rockonCamera.Getrock())
            {


                timeManager.SlowDown(2);

            }
            else
            {


                subtimeManager.SlowDown(2);

            }
            GetComponent<Collider>().enabled = false;
            //forceField.enabled = false;
            enemy.HitAction();
            enemy.MinusEnemyHp(AttackPower);
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
