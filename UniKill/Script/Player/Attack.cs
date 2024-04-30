using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 ================
 概要：攻撃判定の処理
 ================
 */
public class Attack : MonoBehaviour
{

  private float StunTime = 0;

    private float StartAttckTime = 0;

    [Header("ヒットエフェクト")]
    [SerializeField] GameObject prefab;

    [Header("攻撃エフェクト")]
    [SerializeField] GameObject Attackprefab;

    //攻撃時の剣のエフェクト
    [Header("攻撃時の剣のエフェクト")]
    [SerializeField] private GameObject AttackLine;


    private CameraShake timeManager;

    private RockonCamera rockonCamera;
    private CameraShake subtimeManager;
    
    private bool Hit;
    private int AttackPower;
   

    private bool NowAttack = false;

    private ParticleSystemForceField forceField;

    private bool DebugColloder = false;

    private int SENumber;

    private int HITNORMALATK = 0;     // 通常攻撃音(1.2発目のヒット時)
    private int HITLASTNORMALATK = 1; // 通常攻撃音(3発目のヒット時)
    private int HITHARDATK = 2;       // 強攻撃音(ヒット時)
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
        

        forceField = gameObject.GetComponent<ParticleSystemForceField>();
        timeManager = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        rockonCamera = GameObject.Find("Rockon Camera").GetComponent<RockonCamera>();
        subtimeManager = GameObject.Find("Rockon Camera").GetComponent<CameraShake>();

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
        AttackLine.SetActive(true);
        forceField.enabled = true;
        Hit = false;
    }
    public void OffAttackCollider()
    {
        if(DebugColloder)
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
        
        GetComponent<Collider>().enabled = false;
        AttackLine.SetActive(false);
        forceField.enabled = false;
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
        //当たったゲームオブジェクトがEnemyタグだったとき
        if (other.gameObject.CompareTag("Enemy") && !Hit/* && DoInstantiate*/)
        {
            
            Instantiate(prefab ,gameObject.transform.position, Quaternion.identity);
            
            AttackLine.SetActive(false);
            Hit = true;
           
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            forceField.enabled = false;
            other.GetComponent<Enemy>().MinusEnemyHp(AttackPower);
            //再生する効果音の変更(予定)&ヒットストップ
            if(!rockonCamera.Getrock())
            {
                if (SENumber == HITNORMALATK)
                {

                    timeManager.SlowDown(0);

                }
                if (SENumber == HITLASTNORMALATK)
                {

                    timeManager.SlowDown(1);
                }
                if (SENumber == HITHARDATK)
                {

                    timeManager.SlowDown(2);
                }
            }
            else
            {
                if (SENumber == HITNORMALATK)
                {

                    subtimeManager.SlowDown(0);

                }
                if (SENumber == HITLASTNORMALATK)
                {

                    subtimeManager.SlowDown(1);
                }
                if (SENumber == HITHARDATK)
                {

                    subtimeManager.SlowDown(2);
                }
            }
           
        }
    }

    public void SetSENumber(int num)
    {
        SENumber = num;
    }
   
}