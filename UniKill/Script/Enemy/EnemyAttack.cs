using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 ================
 �T�v�F�U������̏���
 ================
 */
public class EnemyAttack : MonoBehaviour
{

  private float StunTime = 0;

    private float StartAttckTime = 0;

    [Header("�q�b�g�G�t�F�N�g")]
    [SerializeField] GameObject prefab;

    [Header("�U���G�t�F�N�g")]
    [SerializeField] GameObject Attackprefab;

    //�U�����̌��̃G�t�F�N�g
    [Header("�U�����̌��̃G�t�F�N�g")]
    [SerializeField] private GameObject AttackLine;


    private CameraShake timeManager;

   

    private bool Hit;
    private int AttackPower;

    private GameObject Player;
    private Player player;

    private bool NowAttack = false;

    private ParticleSystemForceField forceField;

    private bool DebugColloder = false;

    private int SENumber;

    private int HITNORMALATK = 0;     // �ʏ�U����(1.2���ڂ̃q�b�g��)
    private int HITLASTNORMALATK = 1; // �ʏ�U����(3���ڂ̃q�b�g��)
    private int HITHARDATK = 2;       // ���U����(�q�b�g��)
    // Start is called before the first frame update
    void Start()
    {
        //stuntimer = 0;
        //MaxStunTime = StunTime;
        //Nowstuntime = false;
        //oldtransform = this.transform.position;
        AttackPower = 0;
        //EndAttack = false;
        Hit = false;

        SENumber = HITHARDATK;

        forceField = gameObject.GetComponent<ParticleSystemForceField>();
        timeManager = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        Player = GameObject.Find("Player");
        player = Player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            DebugColloder = !DebugColloder;
        }
    }

   
    public void StartAttack()
    {
        NowAttack = true;
    }
    public void EndAttack()
    {
        NowAttack = false;
    }
    public void OnAttackCollider()
    {
        if (DebugColloder)
        {
            GetComponent<MeshRenderer>().enabled = true;
        }
           
        GetComponent<Collider>().enabled = true;
        //AttackLine.SetActive(true);
        //forceField.enabled = true;
        Hit = false;
    }
    public void OffAttackCollider()
    {
        if(DebugColloder)
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
        
        GetComponent<Collider>().enabled = false;
       // AttackLine.SetActive(false);
        //forceField.enabled = false;
    }

    public bool NowColliderState()
    {
        return GetComponent<Collider>().enabled;
    }

    public void SetPower(int num)
    {
        AttackPower = num;
    }

    private void OnTriggerEnter(Collider other)
    {
        //���������Q�[���I�u�W�F�N�g��Player�^�O�������Ƃ�
        if (other.gameObject.CompareTag("Player") && !Hit/* && DoInstantiate*/)
        {
            
            //Instantiate(prefab ,gameObject.transform.position, Quaternion.identity);
            
           
            Hit = true;
           
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            //forceField.enabled = false;
            player.MinusHp(AttackPower);
            //�Đ�������ʉ��̕ύX(�\��)
            //if (SENumber == HITNORMALATK)
            //{
               
            //    timeManager.SlowDown(0);

            //}
            //if (SENumber == HITLASTNORMALATK)
            //{

            //    timeManager.SlowDown(1);
            //}
            //if (SENumber == HITHARDATK)
            //{

            //    timeManager.SlowDown(2);
            //}
        }
    }

    public void SetSENumber(int num)
    {
        SENumber = num;
    }
   
}