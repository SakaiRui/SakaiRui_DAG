using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEditor.Animations;

/*
 ================
 概要：プレイヤーの動きの処理
 ================
 */
public class Player : MonoBehaviour
{

    private Animator animator;



    private enum JumpState
    {
        IDLE,    // 入力待ち状態
        WAITING,    // ジャンプ溜め状態
        RISING,    // 上昇中状態
        FALLING,    // 下降中状態
        DOUBLE,
        LANDING,    // 着地状態
    }

    //体力
    [Header("最大HP")]
    [SerializeField] private float MaxHp = 100;

    [Header("現在のHP\n(現在のHPはデバック用に'E'で減少'R'で増加、\n'L'でコンソールにデバック表示)")]
    [SerializeField] private float Hp = 100;

   
    [Header("被弾時の無敵時間(硬直時間)")]
    [SerializeField] private float InvincibilityTime = 1.0f;
    private float InvincibilityTimeCount = 0.0f;

    private float plusMpCount = 1;
    private float MaxplusMpCount = 0;

    //技のクールダウン
    [Header("技のクールダウン\n('Kでクールダウン発生、クールダウン発生中はコンソールにデバック表示')")]
    [SerializeField] private float SkillCoolTime = 3.0f;

    //左右の移動スピード
    [Header("回避スピード")]
    [SerializeField] private float AvoidanceSpeed = 3.0f;

    //左右の移動スピード
    [Header("左右のダッシュ移動スピード")]
    [SerializeField] private float sprintSpeed = 5.0f;
    private bool Issprint = false;

    

  
    [Header("回避無敵時間")]
    [SerializeField] private int AvoidanceInvincibilityTime = 10;

    [Header("ジャスト回避無敵時間")]
    [SerializeField] private int SuperAvoidanceInvincibilityTime = 10;

    [Header("ジャスト回避判定時間")]
    [SerializeField] private int SuperAvoidancejudgeTime = 10;

    private int AvoidanceInvincibilityTimeCount = 0;

    private int SuperAvoidancejudgeCount = 0;
    private int SuperAvoidanceInvincibilityTimeCount = 0;

    private bool IsSuperAvoidancejudge = false;

    private bool IsSuperAvoidance = false;

    private bool IsSuperAvoidanceAttack = false;


    [Header("ジャスト回避後走る時間")]
    [SerializeField] private int SuperAvoidancedashTime = 10;

    private int SuperAvoidancedashTimeCount = 0;
    private bool IsSuperAvoidancedash = false;

    private float AvoidanceTimer = 0.0f;
    private bool IsAvoidance = false;
    private bool endAvoidance = false;
    private float AvoidanceCoolDown = 0.5f;
    private float AvoidanceCoolDownCount;


    private bool AirAvoidance = false;

    private float speed;

    //ジャンプの強さ
    [Header("ジャンプの強さ")]
    [SerializeField] private float jumpPower = 3.0f;

    
  

    //攻撃力
    [Header("攻撃力")]
    [SerializeField] private int AttackPower = 10;

  

    //強攻撃力
    [Header("強攻撃力")]
    [SerializeField] private int SkillAttackPower = 10;

    //強攻撃の最大溜め時間
    [Header("強攻撃の最大溜め時間")]
    [SerializeField] private float MaxSkillAttackTime = 3.0f;
    private float SkillAttackTimeCount = 0.0f;
    private bool NowCharge = false;

    //強攻撃力
    [Header("空中攻撃力")]
    [SerializeField] private int AirAttackPower = 10;

    //当たり判定オブジェクト
    [Header("弱攻撃当たり判定オブジェクト(仮実装)")]
    [SerializeField] private GameObject attackObject;
    private Collider attackCollider;
    private Attack attack;

    


    //当たり判定オブジェクト
    [Header("空中攻撃当たり判定オブジェクト(仮実装)")]
    [SerializeField] private GameObject airattackObject;
    //private Collider attackCollider;
    //空中攻撃の硬直時間
    [Header("空中攻撃の硬直時間")]
    [SerializeField] private float airattackstun = 1.0f;

    //空中攻撃の攻撃発生タイミング
    [Header("空中攻撃の攻撃発生タイミング")]
    [SerializeField] private float airStartAttckTime = 1.0f;
    private Attack airattack;

    //当たり判定オブジェクト
    [Header("強攻撃当たり判定オブジェクト(仮実装)")]
    [SerializeField] private GameObject SkillattackObject;
    private Collider SkillattackCollider;
    private Attack Skillattack;

    //強攻撃の硬直時間
    [Header("強攻撃の硬直時間")]
    [SerializeField] private float Skillattackstun = 1.0f;

    //強攻撃の攻撃発生タイミング
    [Header("強攻撃の攻撃発生タイミング")]
    [SerializeField] private float SkillStartAttckTime = 1.0f;
    private Rigidbody rb;



   

    [SerializeField] private TrailRenderer trail1;

    [Header("着地エフェクト")]
    [SerializeField] GameObject prefab;

    [Header("回避")]
    [SerializeField] GameObject dashprefab;

    [Header("ジャスト回避")]
    [SerializeField] GameObject justprefab;

    [Header("HP回復")]
    [SerializeField] GameObject HPprefab;

 
    [Header("Charge回復")]
    [SerializeField] GameObject Chargeprefab;

    [Header("HPlow")]
    [SerializeField] GameObject LowHpprefab;

    [Header("被ダメエフェクト")]
    [SerializeField] GameObject Damage;

    [Header("特殊攻撃エフェクト")]
    [SerializeField] GameObject special;


    [Header("攻撃エフェクト")]
    [SerializeField] private GameObject ATTACK;
    private GameObject obj;
    [SerializeField] private GameObject sword;
    private GameObject LowHppEffect;

    [SerializeField] private float turnSpeed = 10.0f;   // 回転速度

    private GameObject Charge;

    private LeftHandMatcher IK;

    private Effekseer.EffekseerEmitter effekseerEmitter;

    // 親オブジェクト
    private GameObject _parent;
    private GameObject[] children;
    private GameObject[] facechildren;
    private GameObject[] facechildrenSkinMesh;
    private GameObject[] facechildrenMesh;

    private float currentAnimationTime = 0;

    private bool isOneJumping = false;
   
    private int JumpCount = 0;
    private int oldJumpCount = 0;

    private float SkilTime;
    private bool UseSkill;


    private bool IsInvincibility = false;
    private bool IsHitInvincibility = false;
    private bool IsHit = false;

    

    private int Maxrotateframe;

    private Vector3 oldtransform;

    private bool JumpStart = false;

    private int jumpTimer = 0;

    private int JumpendTimer = 0;

    private JumpState jumpState = JumpState.IDLE;

    private float AirPotisione;
    private Vector3 vector;

    private bool airAttck = false;

    private int KeyLook = 0;

    private bool gameover = false;
    private bool lowHp = false;

   
    private bool NowAttack = false;

    private bool NowSkillAttack = false;


    private CameraShake timeManager;
    private CameraShake RocktimeManager;

    //private AnimatorState state;
    private CameraZoom zoom;
    public GameObject childgameObject;
    public GameObject facechildgameObject;


    [SerializeField] private CharacterGague characterGage;

    [SerializeField] private Vector3 velocity;              // 移動方向
    [SerializeField] private float moveSpeed = 10.0f;        // 移動速度
    [SerializeField] private float applySpeed = 0.2f;       // 振り向きの適用速度
    [SerializeField] private PlayerFollowCamera refCamera;  // カメラの水平回転を参照する用

    [SerializeField]
    private GameObject ShotCollider;
    [SerializeField]
    private int ShotPower;
    [SerializeField]
    private int ShotSpeed;

    float inputHorizontal;
    float inputVertical;

    private PlayerFollowCamera followCamera;
    private RockonCamera rockonCamera;

    private GameObject Rockcamera;

    private int TwoCount = 0;

    private bool DebugColloder = false;


    void Start()
    {
        if(Hp < MaxHp)
        {
            Hp = MaxHp;
        }

        gameover = false;
        SkilTime = 0.0f;
        UseSkill = false;
        
        
        rb = GetComponent<Rigidbody>();

        oldtransform = transform.position;

        MaxplusMpCount = plusMpCount;
        //animator.speed = 1.0f;
        attack = attackObject.GetComponent<Attack>();
        attackCollider = attackObject.GetComponent<Collider>();
        attack.SetPower(AttackPower);

        airattack = airattackObject.GetComponent<Attack>();
        airattack.SetPower(AirAttackPower);

        Skillattack = SkillattackObject.GetComponent<Attack>();
        SkillattackCollider = SkillattackObject.GetComponent<Collider>();
        Skillattack.SetPower(SkillAttackPower);

        speed = AvoidanceSpeed;
        animator = GetComponent<Animator>();
     
        timeManager = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        zoom = GameObject.Find("Main Camera").GetComponent<CameraZoom>();
        RocktimeManager = GameObject.Find("Rockon Camera").GetComponent<CameraShake>();
        _parent = transform.Find("Character1_Reference").gameObject;
        children = GetChildren(childgameObject);
        facechildren = GetChildren(facechildgameObject);
        followCamera = GameObject.Find("Main Camera").GetComponent<PlayerFollowCamera>();
        Debug.Log(followCamera);
        rockonCamera = GameObject.Find("Rockon Camera").GetComponent<RockonCamera>();
        Rockcamera = GameObject.Find("Rockon Camera");
        IK = GetComponent<LeftHandMatcher>();

        characterGage.SetMaxLife(MaxHp);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.M))
        {
            DebugColloder = !DebugColloder;
        }

        if (gameover)
        {
            animator.speed = 0.2f;
            return;
        }
       

        

        //プレイヤーの停止判定
        if (!IsAvoidance && !NowAttack && !NowSkillAttack && JumpCount == 0
               && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.J) && !IsHit && !IsSuperAvoidancedash && !IsSuperAvoidance && !IsSuperAvoidanceAttack)
        {
          
            animator.SetBool("walk", false);
            animator.SetBool("run", false);
            animator.SetBool("avoidance", false);
           // Debug.Log("停止中");
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.W) && !IsAvoidance && jumpTimer == 0 && jumpState != JumpState.WAITING && !airAttck)
        {
            animator.SetBool("walk", false);
            animator.SetBool("run", false);
            animator.SetBool("avoidance", false);
            
           

        }

       
       

        //被弾アニメーション中の無敵＆点滅
        if (IsHitInvincibility)
        {
            
            
            if (!IsHit)
            {
                InvincibilityTimeCount -= Time.deltaTime;
                TwoCount++;
                if(TwoCount >= 10)
                {
                    for (var i = 0; i < children.Length; i++)
                    {
                        children[i].GetComponent<SkinnedMeshRenderer>().enabled = !children[i].GetComponent<SkinnedMeshRenderer>().enabled;
                    }
                    for (var i = 0; i < facechildren.Length; i++)
                    {
                        if (facechildren[i].GetComponent<SkinnedMeshRenderer>())
                        {
                            facechildren[i].GetComponent<SkinnedMeshRenderer>().enabled = !facechildren[i].GetComponent<SkinnedMeshRenderer>().enabled;
                        }
                        else if (facechildren[i].GetComponent<MeshRenderer>())
                        {
                            facechildren[i].GetComponent<MeshRenderer>().enabled = !facechildren[i].GetComponent<MeshRenderer>().enabled;
                        }

                    }
                    TwoCount = 0;
                }
               
            }
            if (InvincibilityTimeCount <= 0.0f && !IsHit)
            {
                InvincibilityTimeCount = 0.0f;
                IsHitInvincibility = false;
                for (var i = 0; i < children.Length; i++)
                {
                    children[i].GetComponent<SkinnedMeshRenderer>().enabled = true;
                }
                for (var i = 0; i < facechildren.Length; i++)
                {
                    if (facechildren[i].GetComponent<SkinnedMeshRenderer>())
                    {
                        facechildren[i].GetComponent<SkinnedMeshRenderer>().enabled = true;
                    }
                    else if (facechildren[i].GetComponent<MeshRenderer>())
                    {
                        facechildren[i].GetComponent<MeshRenderer>().enabled = true;
                    }

                }
                TwoCount = 0;
            }
        }

        if(AvoidanceCoolDownCount >= 0.0f)
        {
            AvoidanceCoolDownCount -= Time.deltaTime;
        }

        //移動
        {
            //攻撃中は動けない
            if (!NowSkillAttack &&!NowAttack && !NowCharge && !IsHit)
            {
                
               // Debug.Log(animator.GetBool("walk"));
                //回避中は動けない
                if (IsAvoidance && !IsSuperAvoidance)
                {

                    if(IsSuperAvoidancejudge)
                    {
                        SuperAvoidancejudgeCount++;
                        Debug.Log("ジャスト回避判定中");
                        if (SuperAvoidancejudgeCount >= SuperAvoidancejudgeTime)
                        {
                            IsSuperAvoidancejudge = false;
                            SuperAvoidancejudgeCount = 0;
                        }
                        else if(Input.GetKeyDown(KeyCode.LeftShift))
                        {
                            Debug.Log("ジャスト回避");
                            //ジャスト回避の処理
                            IsInvincibility = true;
                            AvoidanceInvincibilityTimeCount = 0;
                            IsSuperAvoidance = true;
                            IsSuperAvoidancejudge = false;
                            SuperAvoidancejudgeCount = 0;
                            animator.SetTrigger("SuperAvoidance");
                            //animator.SetBool("avoidance", false);
                            IK.SwitchIK(false);
                            Instantiate(justprefab, gameObject.transform.position + transform.forward * 5.0f + transform.up * 3.0f, Quaternion.LookRotation(-transform.forward,new Vector3(0,1,0)),transform);
                        }
                    }
                    //Debug.Log(speed);
                    //無敵時間中なら
                    if (AvoidanceInvincibilityTimeCount <= AvoidanceInvincibilityTime && IsInvincibility == true)
                    {

                        AvoidanceInvincibilityTimeCount += 1;
                       // Debug.Log(AvoidanceInvincibilityTimeCount);
                    }
                    //無敵時間終了
                    else
                    {
                        gameObject.layer = LayerMask.NameToLayer("Default");
                        IsInvincibility = false;
                        AvoidanceInvincibilityTimeCount = 0;
                    }

                    
                    transform.position += speed * 0.5f * transform.forward * Time.deltaTime;
                 
                }

                if (IsSuperAvoidance)
                {

                    
                    //Debug.Log(speed);
                    //無敵時間中なら
                    if (SuperAvoidanceInvincibilityTimeCount <= SuperAvoidanceInvincibilityTime && IsInvincibility == true)
                    {

                        SuperAvoidanceInvincibilityTimeCount += 1;
                        // Debug.Log(AvoidanceInvincibilityTimeCount);
                    }
                    //無敵時間終了
                    else
                    {
                        gameObject.layer = LayerMask.NameToLayer("Default");
                        IsInvincibility = false;
                        SuperAvoidanceInvincibilityTimeCount = 0;
                    }


                    transform.position += speed * transform.forward * Time.deltaTime;

                   
                }

                //回避
                if (Input.GetKeyDown(KeyCode.LeftShift) && !IsAvoidance &&  jumpTimer == 0 && 
                    !AirAvoidance && jumpState != JumpState.WAITING && !airAttck && JumpCount == 0 && AvoidanceCoolDownCount <= 0.0f && !IsSuperAvoidance && !IsSuperAvoidancedash && !IsSuperAvoidanceAttack)
                {
                    IsAvoidance = true;
                    IsInvincibility = true;
                    //gameObject.layer = LayerMask.NameToLayer("HitPlayer");
                    speed = AvoidanceSpeed;
                    IsSuperAvoidancejudge = true;
                    trail1.enabled = true;
                    trail1.time = 0.2f;
                    IK.SwitchIK(false);
                    AvoidanceCoolDownCount = AvoidanceCoolDown;

                    Debug.Log(speed);

                    animator.SetBool("avoidance", true);
                   

                    Instantiate(dashprefab, gameObject.transform.position, Quaternion.Euler(-transform.forward));

                  
                    transform.position += speed * transform.forward * Time.deltaTime;
                }


                // Wキー（前移動）
                if (Input.GetKey(KeyCode.W) && !IsAvoidance && jumpTimer == 0 && jumpState != JumpState.WAITING && !airAttck && !IsSuperAvoidance && !IsSuperAvoidancedash && !IsSuperAvoidanceAttack)
                {
                    if (JumpCount == 0 && !Input.GetKey(KeyCode.S))
                    {
                        animator.SetBool("walk", true);

                    }
                    if(Input.GetKey(KeyCode.S))
                    {
                        animator.SetBool("walk", false);
                    }


                   // velocity.z += 1;

                    //if (!Input.GetKey(KeyCode.A))
                    //    isLeft = false;
                }

                // Sキー（後ろ移動）
                if (Input.GetKey(KeyCode.S) && !IsAvoidance && jumpTimer == 0 && jumpState != JumpState.WAITING && !airAttck && !IsSuperAvoidance && !IsSuperAvoidancedash && !IsSuperAvoidanceAttack)
                {
                    if (JumpCount == 0 && !Input.GetKey(KeyCode.W))
                    {
                        animator.SetBool("walk", true);

                    }
                    if (Input.GetKey(KeyCode.W))
                    {
                        animator.SetBool("walk", false);
                    }
                    //velocity.z -= 1;

                    //if (!Input.GetKey(KeyCode.D))
                    //    isLeft = true;
                }

                // Dキー（右移動）
                if (Input.GetKey(KeyCode.D) && !IsAvoidance && jumpTimer == 0 && jumpState != JumpState.WAITING && !airAttck && !IsSuperAvoidance && !IsSuperAvoidancedash && !IsSuperAvoidanceAttack)
                {
                    if (JumpCount == 0 && !Input.GetKey(KeyCode.A))
                    {
                        animator.SetBool("walk", true);
                       
                    }
                    if (Input.GetKey(KeyCode.A))
                    {
                        animator.SetBool("walk", false);
                    }


                    //transform.position += speed * Vector3.right * Time.deltaTime;
                    // velocity.x += 1;

                    //if (!Input.GetKey(KeyCode.A))
                    //    isLeft = false;
                }

                // Aキー（左移動）
                if (Input.GetKey(KeyCode.A) && !IsAvoidance && jumpTimer == 0 && jumpState != JumpState.WAITING && !airAttck && !IsSuperAvoidance && !IsSuperAvoidancedash && !IsSuperAvoidanceAttack)
                {
                    if (JumpCount == 0 && !Input.GetKey(KeyCode.D))
                    {
                        animator.SetBool("walk", true);
                       
                    }
                    if (Input.GetKey(KeyCode.D))
                    {
                        animator.SetBool("walk", false);
                    }
                    //transform.position -= speed * Vector3.right * Time.deltaTime;
                    // velocity.x -= 1;

                    //if (!Input.GetKey(KeyCode.D))
                    //    isLeft = true;
                }

                if (!IsAvoidance && !IsSuperAvoidance)
                {
                    inputHorizontal = Input.GetAxisRaw("Horizontal");
                    //Debug.Log(inputHorizontal);
                    inputVertical = Input.GetAxisRaw("Vertical");
                }
                    

                if(IsSuperAvoidancedash)
                {
                    transform.position += speed * 2.0f * transform.forward * Time.deltaTime;
                    SuperAvoidancedashTimeCount++;
                    Debug.Log("回避dash");
                   
                    if (SuperAvoidancedashTimeCount >= SuperAvoidancedashTime)
                    {
                        IsSuperAvoidancedash = false;
                        SuperAvoidancedashTimeCount = 0;
                        animator.SetBool("SuperAvoidanceDash", false);
                        IK.SwitchIK(true);
                        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W))
                        {
                            animator.SetBool("walk", true);
                            Debug.Log("回避から歩き");
                        }
                           
                        else
                            animator.SetBool("walk", false);
                    }
                    else if (Input.GetMouseButtonDown(1))
                    {
                        Debug.Log("special");
                        IsSuperAvoidanceAttack = true;
                        IsSuperAvoidancedash = false;
                        SuperAvoidancedashTimeCount = 0;
                        animator.SetBool("SuperAvoidanceDash", false);
                        animator.SetBool("SuperAvoidanceAttack", true);
                        
                    }
                }


               
                //ジャンプ
                if (Input.GetKeyDown(KeyCode.Space) && JumpCount == 0 && jumpTimer == 0 && jumpState != JumpState.WAITING && jumpState != JumpState.LANDING && !IsAvoidance && jumpState != JumpState.DOUBLE && !IsSuperAvoidancedash && !IsSuperAvoidancedash && !IsSuperAvoidanceAttack)
                {
                    rb.velocity = Vector3.up * jumpPower + new Vector3(rb.velocity.x, 0, rb.velocity.z);
                    JumpCount++;
                    animator.SetBool("Jump", true);
                    animator.SetBool("walk", false);
                    animator.SetBool("doublerising", false);
                    animator.SetBool("Airattack", false);
                }
                
                //ジャンプ中のアニメーションの推移
                switch (jumpState)
                {
                    case JumpState.IDLE:

                        if (JumpCount > 0)
                        {
                            jumpState = JumpState.WAITING;
                        }


                        break;
                    case JumpState.WAITING:
                        
                        jumpTimer++;
                        if (jumpTimer == 1)
                        {
                            animator.SetBool("jumpstart", true);
                            animator.SetBool("walk", false);
                        }
                        if (jumpTimer >= 3)
                        {

                            animator.SetBool("jumpstart", false);
                            animator.SetBool("rising", true);
                           
                            animator.SetBool("walk", false);
                            jumpState = JumpState.RISING;
                            jumpTimer = 0;
                        }

                        break;
                    case JumpState.RISING:
                       
                        //rb.velocity -= Vector3.up * 2.5f;
                        if (rb.velocity.y < 0)
                        {
                            animator.SetBool("rising", false);
                            animator.SetBool("falling", true);
                            jumpState = JumpState.FALLING;
                        }
                        if (JumpCount == 0)
                        {
                            animator.SetBool("rising", false);
                            animator.SetBool("landing", true);
                            jumpState = JumpState.LANDING;
                        }
                        
                        break;
                    case JumpState.FALLING:
                         
                        animator.SetBool("endlanding", false);
                        if (JumpCount == 0)
                        {
                            // Debug.Log("地面についた");
                            animator.SetBool("falling", false);
                            animator.SetBool("landing", true);
                            animator.SetBool("walk", false);
                            jumpState = JumpState.LANDING;
                        }
                       

                        break;
                   
                    case JumpState.LANDING:
                        
                        jumpTimer++;
                        if (jumpTimer >= 2)
                        {
                            animator.SetBool("landing", false);
                            animator.SetBool("endlanding", true);
                            jumpState = JumpState.IDLE;
                            jumpTimer = 0;
                        }
                        break;
                }


              

            }
        }
        //HPとMPの処理
        {
            //'E'でHP減少(デバック用)
            if (Input.GetKeyDown(KeyCode.E))
            {
                Hp -= 10;
                characterGage.GaugeReduction(10, Hp);
            }

            //'R'でHP増加(デバック用)
            if (Input.GetKeyDown(KeyCode.R))
            {
                Hp++;
            }



            //現在のHPとMPのデバック表示
            if (Input.GetKeyDown(KeyCode.L))
            {
                Debug.Log("HP：" + Hp);
             
            }

           
        }

        //攻撃
        {
            
            //弱攻撃
            if (Input.GetMouseButtonDown(1) && /*!attack.NowStunTime() &&*/ JumpCount == 0 && !NowSkillAttack && !IsAvoidance && !IsSuperAvoidance && !IsSuperAvoidancedash && !IsSuperAvoidanceAttack)
            {
               
                if (Issprint)
                {
                    Issprint = false;
                    speed = AvoidanceSpeed;
                
                    animator.SetBool("run", false);
                }

              
                animator.SetTrigger("NormalAttck");
               
            }

            //空中攻撃の処理のみ実装
            //if (Input.GetKeyDown(KeyCode.J) && !NowAttack && JumpCount >= 1 && !NowSkillAttack && !IsAvoidance && !IsHit && !airAttck && 
            //    (jumpState == JumpState.RISING || jumpState == JumpState.DOUBLE || jumpState == JumpState.FALLING) )
            //{

            //    airAttck = true;
            //    animator.SetBool("Airattack", true);
            //    //rb.velocity = Vector3.zero;
            //    airattack.SetPower(AirAttackPower);
            //    if (lowHp)
            //    {
            //        airattack.SetPower(AirAttackPower + 20);
            //    }
            //    airattack.StartAttack(airattackstun, airStartAttckTime, isLeft, airAttck,0);
            //    //SoundManager.Instance.SetPlaySE(SoundManager.E_SE.SE_PLAYER_NORMALATK,
            //    //    SoundManager.Instance.GetSEVolume()); // 通常攻撃音再生
            //    if (Issprint)
            //    {
            //        Issprint = false;
            //        speed = normalSpeed;
            //        //----------------------
            //        //変更
            //        //----------------------
            //        animator.SetBool("run", false);
            //    }
            //    if (lowHp)
            //    {
            //        Debug.Log("ko");

            //    }
            //    //----------------------
            //    //変更
            //    //----------------------
            //    // animator.SetBool("normal", true);

            //    Mp -= AirMinusAttackMp;
            //    //Debug.Log(Mp);
            //    if (Mp < 0)
            //    {
            //        Mp = 0;

            //    }
            //}

            //if (airAttck)
            //{
            //    //rb.velocity = Vector3.zero;
            //    switch (isLeft)
            //    {
            //        case true:
            //            transform.position -= speed * 1.5f * Vector3.right * Time.deltaTime;
            //            transform.position -= speed * 2.5f * Vector3.up * Time.deltaTime;
            //            break;

            //        case false:
            //            transform.position += speed * 1.5f * Vector3.right * Time.deltaTime;
            //            transform.position -= speed * 2.5f * Vector3.up * Time.deltaTime;

            //            break;
            //    }

            //}
        }

        //強攻撃
        {

          
            //技が使用されてなくて、'K'が押されたらクールダウン発生＆使用フラグをtrueに
            if (Input.GetKeyDown(KeyCode.Q) && !UseSkill && !NowSkillAttack && JumpCount == 0 && !NowAttack && !IsAvoidance && !IsHit && !IsSuperAvoidance && !IsSuperAvoidancedash && !IsSuperAvoidanceAttack)
            {
               
                if (Issprint)
                {
                    Issprint = false;
                    speed = AvoidanceSpeed;
                    //----------------------
                    //変更
                    //----------------------
                    animator.SetBool("run", false);
                }
                animator.SetBool("walk", false);

                Debug.Log("スキル開始");
                animator.SetBool("skill", true);

            }

            if (NowCharge)
            {
                Debug.Log("Charge中");
                SkillAttackTimeCount += Time.deltaTime;

                //animator.speed *= -1f;

                //溜め段階でエフェクト変更
                if (SkillAttackTimeCount < MaxSkillAttackTime / 2.0f)
                {
                    
                }
                if (SkillAttackTimeCount >= MaxSkillAttackTime / 2.0f && SkillAttackTimeCount < MaxSkillAttackTime)
                {
                    effekseerEmitter.SendTrigger(1);
                    
                }
                if (SkillAttackTimeCount >= MaxSkillAttackTime)
                {
                    effekseerEmitter.SendTrigger(2);
                    
                }
                if (!Input.GetKey(KeyCode.Q))
                {
                    Destroy(Charge);
                    animator.SetFloat("Speed", 1f);
                    animator.speed = 1.0f;
                    //キーを離した時の溜め段階で攻撃力変更
                    if (SkillAttackTimeCount < MaxSkillAttackTime / 2.0f)
                    {
                        Debug.Log("溜め無");
                        Skillattack.SetPower(SkillAttackPower);
                        Skillattack.SetSENumber(1);
                      
                    }
                    if (SkillAttackTimeCount >= MaxSkillAttackTime / 2.0f && SkillAttackTimeCount < MaxSkillAttackTime)
                    {
                        Debug.Log("溜め1");
                        Skillattack.SetPower(SkillAttackPower + 10);
                        Skillattack.SetSENumber(1);
                     
                    }
                    if (SkillAttackTimeCount >= MaxSkillAttackTime)
                    {
                        Debug.Log("溜め2");
                        Skillattack.SetPower(SkillAttackPower + 20);
                        Skillattack.SetSENumber(2);
                      
                    }
                   
                    NowCharge = false;
                    SkilTime = SkillCoolTime;
                    //OnColliderSkillAttack();
                    //Skillattack.StartAttack(Skillattackstun, SkillStartAttckTime,isLeft, airAttck,2);
                    UseSkill = true;
                    
                    SkillAttackTimeCount = 0.0f;

                   
                }
            }

            //技の使用フラグがtrueならクールダウン発生
            if (SkilTime > 0.0f && UseSkill)
            {
                SkilTime -= Time.deltaTime;
                //技のクールダウンが0になったら使用フラグをfalseに
                if (SkilTime <= 0.0f)
                {
                    SkilTime = 0.0f;
                    UseSkill = false;
                }
                //技のクールダウンのデバック表示
                // Debug.Log(SkilTime);
            }


        }

        //HPとMPが上限値以上にならないように
        if (Hp >= MaxHp)
        {
            Hp = MaxHp;
        }
       
        //HPが0未満にならないように
        if (Hp < 0)
        {
            //Destroy(gameObject);
            Hp = 0;
        }
       
     

        oldtransform = transform.position;


    }

    void FixedUpdate()
    {

        if (!NowSkillAttack && !NowAttack && !NowCharge && !IsHit && !IsAvoidance && !IsSuperAvoidance && !IsSuperAvoidanceAttack)
        {

            // カメラの方向から、X-Z平面の単位ベクトルを取得
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

            // 方向キーの入力値とカメラの向きから、移動方向を決定
            Vector3 moveForward = cameraForward * inputVertical + Camera.main.transform.right * inputHorizontal;

           // Debug.Log(rockonCamera.Getrock());

            if (!rockonCamera.Getrock())
            {
               

               // Debug.Log("フリー");
                // キャラクターの向きを進行方向に
                if (moveForward != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveForward), applySpeed);
                }
                // 移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す。
                if (!IsSuperAvoidancedash)
                rb.velocity = moveForward.normalized * moveSpeed + new Vector3(0, rb.velocity.y, 0);
            }
            else
            {
               
                cameraForward = Vector3.Scale(Rockcamera.transform.forward, new Vector3(1, 0, 1)).normalized;
                
                //カメラを回転させる
      
                Vector3 vector3 = rockonCamera.GetRockonTarget().transform.position - this.transform.position;
                vector3.y = 0.0f;
                //回転させる角度
                float angle = inputHorizontal * moveSpeed * 0.04f * (-0.4f * (22f - Vector3.Distance(rockonCamera.GetRockonTarget().transform.position, this.transform.position)));
                //Debug.Log(angle);
                if (angle > 0)
                {
                    angle = Mathf.Clamp(angle, 1.0f, 3.5f);
                }
                if (angle < 0)
                {
                    angle = Mathf.Clamp(angle, -3.5f, -1.0f);
                }
                
                {
                    //Debug.Log("通った");
                    transform.RotateAround(rockonCamera.GetRockonTarget().transform.position, Vector3.up, angle);
                }
                
                // 方向キーの入力値とカメラの向きから、移動方向を決定
                moveForward = vector3.normalized * inputVertical;
                // 移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す。
                if (!IsSuperAvoidancedash)
                    rb.velocity = moveForward.normalized * moveSpeed + new Vector3(0, rb.velocity.y, 0);

                moveForward = cameraForward.normalized * inputVertical + Rockcamera.transform.right * inputHorizontal;
                // キャラクターの向きを進行方向に
                
                {
                    if (moveForward == Vector3.zero)
                    {
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(vector3), applySpeed);
                    }
                    else 
                    {

                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveForward), applySpeed);
                    }
                }



            }


        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Instantiate(prefab, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);
            AirAvoidance = false;
            JumpCount = 0;
            animator.SetBool("Jump", false);
            animator.SetBool("falling", false);
            animator.SetBool("landing", true);
            animator.SetBool("walk", false);
            animator.SetBool("doublerising", false);
            jumpState = JumpState.LANDING;
            if (airAttck)
            {
                animator.SetBool("Airattack", false);
                airAttck = false;
                //airattack.EndAttckNow();
            }

          
        }

        
    }

    private void OffColliderAttack()
    {
        attackObject.GetComponent<MeshRenderer>().enabled = false;
        attackCollider.enabled = false;
    }
    private void OnColliderAttack()
    {
        //attackObject.GetComponent<MeshRenderer>().enabled = true;
        attackCollider.enabled = true;
    }

    private void OffColliderSkillAttack()
    {
        SkillattackObject.GetComponent<MeshRenderer>().enabled = false;
        SkillattackCollider.enabled = false;
    }
    private void OnColliderSkillAttack()
    {
        //SkillattackObject.GetComponent<MeshRenderer>().enabled = true;
        SkillattackCollider.enabled = true;
    }


    //現在のHPの取得
    public float GetHp()
    {
        return Hp;
    }


    public float GetMaxHp()
    {
        return MaxHp;
    }

    //外から呼び出してプレイヤーのHP減少
    public void MinusHp(int damage)
    {
        if (!IsInvincibility && !IsHitInvincibility && !IsSuperAvoidancejudge)
        {
            IK.SwitchIK(false);
            animator.speed = 1.0f;
            Instantiate(Damage, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1.0f, gameObject.transform.position.z), Quaternion.identity);
            OffColliderAttack();
            OffColliderSkillAttack();
            NowCharge = false;
            NowAttack = false;
            Destroy(Charge);
            SkillAttackTimeCount = 0.0f;
            animator.SetBool("skill", false);
            NowAttack = false;
            NowSkillAttack = false;
            IsSuperAvoidancedash = false;
            animator.SetBool("SuperAvoidanceDash", false);
            IsSuperAvoidanceAttack = false;
            animator.SetBool("SuperAvoidanceAttack", false);
            SuperAvoidancedashTimeCount = 0;
            IsSuperAvoidance = false;

            //　ヒットストップ
            if (rockonCamera.Getrock())
            {
                if (damage >= 30)
                {
                    animator.SetBool("HardHit", true);
                    InvincibilityTimeCount = 2.0f;
                    RocktimeManager.SlowDown(2);
                }
                else if (damage >= 20)
                {
                    RocktimeManager.SlowDown(1);
                    animator.SetBool("hit", true);
                    InvincibilityTimeCount = 1.7f;
                }
                else
                {
                    RocktimeManager.SlowDown(0);
                    animator.SetBool("hit", true);
                    InvincibilityTimeCount = 1.7f;
                }
            }
            else
            {
                if (damage >= 30)
                {
                    animator.SetBool("HardHit", true);
                    InvincibilityTimeCount = 2.0f;
                    timeManager.SlowDown(2);
                }
                else if (damage >= 20)
                {
                    timeManager.SlowDown(1);
                    animator.SetBool("hit", true);
                    InvincibilityTimeCount = 1.7f;
                }
                else
                {
                    timeManager.SlowDown(0);
                    animator.SetBool("hit", true);
                    InvincibilityTimeCount = 1.7f;
                }
            }


           
            switch (jumpState)
            {
                case JumpState.IDLE:

                    break;
                case JumpState.WAITING:
                    animator.SetBool("jumpstart", false);

                    jumpState = JumpState.IDLE;

                    break;
                case JumpState.RISING:

                    animator.SetBool("rising", false);
                    rb.velocity = Vector3.zero;
                    jumpState = JumpState.IDLE;

                    break;
                case JumpState.FALLING:

                    animator.SetBool("falling", false);

                    jumpState = JumpState.IDLE;

                    break;
                case JumpState.DOUBLE:

                    animator.SetBool("doublerising", false);

                    jumpState = JumpState.IDLE;

                    break;
                case JumpState.LANDING:

                    animator.SetBool("landing", false);

                    jumpState = JumpState.IDLE;

                    break;
            }
            animator.SetBool("Jump", false);
            jumpTimer = 0;
           

            
            
          
            //回避の硬直中は被ダメを大きく
            if (IsAvoidance)
            {
                damage += 20;
            }
            Debug.Log(damage);

            characterGage.GaugeReduction(damage, Hp);

            Hp -= damage; // ダメージ分、プレイヤーの体力を減らす
            
            if (Hp <= 0) // 体力が0以下になったら消す
            {
                gameover = true;
               
                zoom.Camerazoom(transform.position);
                animator.SetBool("hit", false);
                animator.SetBool("HardHit", true);
            }
            else
            {
                // IsInvincibility = true;
                IsHit = true;
                IsHitInvincibility = true;
                //gameObject.layer = LayerMask.NameToLayer("HitPlayer");
            }
        }

        if(IsSuperAvoidancejudge)
        {
            //ジャスト回避の処理
            RocktimeManager.SlowDown(0);
            Debug.Log("ジャスト回避");
            IsInvincibility = true;
            AvoidanceInvincibilityTimeCount = 0;
            SuperAvoidancejudgeCount = 0;
            IsSuperAvoidance = true;
            IsSuperAvoidancejudge = false;
            //animator.SetBool("avoidance", false);
            animator.SetTrigger("SuperAvoidance");
            IK.SwitchIK(false);
            //Quaternion rot = justprefab.transform.rotation * transform.rotation * Quaternion.Inverse(Camera.main.transform.localRotation);
            Instantiate(justprefab, gameObject.transform.position + transform.forward * 5.0f + transform.up * 3.0f, Quaternion.LookRotation(-transform.forward, new Vector3(0, 1, 0)), transform);

        }

    }

    void HitEnd()
    {
        if (IsHit)
        {
            IK.SwitchIK(true);
            
            animator.SetBool("hit", false);
            animator.SetBool("HardHit", false);
            animator.SetBool("walk", false);
            animator.SetBool("run", false);
            animator.SetBool("avoidance", false);



            animator.SetBool("normal", false);
            NowCharge = false;

            OffColliderAttack();
            OffColliderSkillAttack();


            animator.SetBool("skill", false);
            //IsHit = false;
            IsSuperAvoidancejudge = false;
            IsSuperAvoidance = false;
            IsInvincibility = false;
            AvoidanceInvincibilityTimeCount = 0;
            SuperAvoidanceInvincibilityTimeCount = 0;
            gameObject.layer = LayerMask.NameToLayer("Default");
            if (IsAvoidance)
            {
                IsAvoidance = false;
                AvoidanceTimer = 0.0f;
                speed = AvoidanceSpeed;
                trail1.time = 0.0f;

                trail1.enabled = false;

            }
            for (var i = 0; i < children.Length; i++)
            {
                children[i].GetComponent<SkinnedMeshRenderer>().enabled = false;
            }
            for (var i = 0; i < facechildren.Length; i++)
            {
                if (facechildren[i].GetComponent<SkinnedMeshRenderer>())
                {
                    facechildren[i].GetComponent<SkinnedMeshRenderer>().enabled = false;
                }
                else if (facechildren[i].GetComponent<MeshRenderer>())
                {
                    facechildren[i].GetComponent<MeshRenderer>().enabled = false;
                }

            }
        }
        IsHit = false;


    }

   

    //技のクールダウン
    public float GetCoolTime()
    {
        return SkilTime;
    }

    //技の使用フラグ
    public bool GetUseSkill()
    {
        return UseSkill;
    }

    public bool isInvincibility()
    {
        if(IsSuperAvoidancejudge)
        {
            return false;
        }
        if (!IsInvincibility && !IsHitInvincibility)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
  
   
  
    private static GameObject[] GetChildren(GameObject parent)
    {
        // 親オブジェクトのTransformを取得
        var parentTransform = parent.transform;

        // 子オブジェクトを格納する配列作成
        var children = new GameObject[parentTransform.childCount];

        // 0～個数-1までの子を順番に配列に格納
        for (var i = 0; i < children.Length; ++i)
        {
            // Transformからゲームオブジェクトを取得して格納
            children[i] = parentTransform.GetChild(i).gameObject;
        }

        // 子オブジェクトが格納された配列
        return children;
    }
    //---------------------------------
    //アニメーターイベントで呼び出す
    //---------------------------------

    //弱一段
    void Attack1_Start()
    {
        animator.SetBool("walk", false);
        NowAttack = true;
        attack.SetSENumber(0);
       
    }

    void EndAvoidance()
    {
     
        if (IsAvoidance && !IsSuperAvoidance)
        {
            
            endAvoidance = true;
            
            if (IsSuperAvoidancejudge)
            {
                IsSuperAvoidancejudge = false;
            }
            if (IsSuperAvoidance)
            {
                IsSuperAvoidance = false;
            }
            {
                animator.SetBool("avoidance", false);
            }

            trail1.time = 0.0f;

            trail1.enabled = false;
            IK.SwitchIK(true);
            animator.SetBool("airavoidance", false);
            animator.SetBool("avoidance", false);
            IsAvoidance = false;
            endAvoidance = false;
            speed = AvoidanceSpeed;
            IsInvincibility = false;
            AvoidanceInvincibilityTimeCount = 0;
            SuperAvoidanceInvincibilityTimeCount = 0;

           
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W))
                animator.SetBool("walk", true);
            else
                animator.SetBool("walk", false);
        }

        if(IsAvoidance && IsSuperAvoidance)
        {
            endAvoidance = true;
            IK.SwitchIK(false);
            if (IsSuperAvoidancejudge)
            {
                IsSuperAvoidancejudge = false;
            }
            if (IsSuperAvoidance)
            {
                IsSuperAvoidance = false;
            }
            {
                animator.SetBool("avoidance", false);
            }

            trail1.time = 0.0f;

            trail1.enabled = false;
            
            animator.SetBool("airavoidance", false);
            animator.SetBool("avoidance", false);
            IsAvoidance = false;
            endAvoidance = false;
            speed = AvoidanceSpeed;
            IsInvincibility = false;
            AvoidanceInvincibilityTimeCount = 0;
            SuperAvoidanceInvincibilityTimeCount = 0;

            IsSuperAvoidancedash = true;
            animator.SetBool("SuperAvoidanceDash", true);
            
        }
    }

    void Attack1_End()
    {
        NowAttack = false;
        if (obj)
        {
            Destroy(obj);
        }
    }

    //弱二段
    void Attack2_Start()
    {
       
        animator.SetBool("walk", false);
        NowAttack = true;
        attack.SetSENumber(0);
     
        obj = (GameObject)Instantiate(ATTACK, sword.transform.position, Quaternion.identity);
        obj.transform.parent = sword.transform;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = new Vector3(3.0f, 3.0f, 3.0f);
        obj.transform.localEulerAngles = Vector3.zero;
        obj.transform.Rotate(new Vector3(-80.0f, 0.0f, 0.0f));
        obj.transform.parent = null;
       
    }
    void Attack2_End()
    {
        NowAttack = false;
        if (obj)
        {
            Destroy(obj);
        }
    }

    //弱三段
    void Attack3_Start()
    {
       
       
        attack.SetSENumber(1);
        Debug.Log(obj);
        
    }
    void Attack3_End()
    {
        NowAttack = false;
        if (obj)
        {
            Destroy(obj);
        }
    }

    //強攻撃
    void Skill_Start()
    {
        NowAttack = true;
        NowSkillAttack = true;
    }
    void Skill_End()
    {
        animator.SetBool("skill", false);
        NowAttack = false;
        NowSkillAttack = false;
    }

    //攻撃の当たり判定発生
    void OnCollider()
    {
        animator.SetBool("walk", false);
        NowAttack = true;
        attack.SetPower(AttackPower);
        attack.OnAttackCollider();
       
    }
    void OnEffect()
    {
        
        if (obj)
        {
            Destroy(obj);
        }
        obj = (GameObject)Instantiate(ATTACK, sword.transform.position, Quaternion.identity);
        obj.transform.parent = sword.transform;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
        obj.transform.localEulerAngles = Vector3.zero;
        obj.transform.Rotate(new Vector3(-80.0f, 10.0f, 0.0f));
        obj.transform.parent = null;
    }
    void OffCollider()
    {
        attack.OffAttackCollider();

    }
    void OnSkillCollider()
    {
        Skillattack.OnAttackCollider();

    }
    void OffSkillCollider()
    {
        Skillattack.OffAttackCollider();
        Skillattack.OffAttackCollider();
    }

    //強攻撃の溜め開始
    void SkillChargeStart()
    {
        //animator.speed = -1.0f;
        animator.SetFloat("Speed", 0f);
        NowCharge = true;
        Charge =
       Instantiate(Chargeprefab, gameObject.transform.position, Quaternion.identity);
        effekseerEmitter = Charge.transform.GetChild(0).gameObject.GetComponent<Effekseer.EffekseerEmitter>();

    }

    void EndSpecialAttack()
    {
        IsSuperAvoidanceAttack = false;
        animator.SetBool("SuperAvoidanceAttack", false);
        IK.SwitchIK(true);
    }
    void OnSpecialAttack()
    {
        //Instantiate(special, gameObject.transform.position, Quaternion.LookRotation(-transform.forward, new Vector3(0, 1, 0)), transform);
        Instantiate(special, gameObject.transform.position, Quaternion.LookRotation(-transform.forward, new Vector3(0, 1, 0)), transform);

        GameObject ball = (GameObject)Instantiate(ShotCollider, gameObject.transform.position, Quaternion.LookRotation(-transform.forward, new Vector3(0, 1, 0)), transform);
        Vector3 pos = ball.transform.position;
        //pos.y += 1.0f;
        ball.transform.position = pos;
        Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
        //ballRigidbody.AddForce(transform.forward * ShotSpeed);
        ball.GetComponent<PlayerShot>().SetPower(ShotPower);
        ball.transform.parent = null;
        if (DebugColloder)
        {
            ball.GetComponent<PlayerShot>().SetDebugColloder(DebugColloder);
        }
    }

    void specialEffect()
    {
        //Instantiate(special, gameObject.transform.position, Quaternion.LookRotation(-transform.forward, new Vector3(0, 1, 0)), transform);
        Instantiate(special, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 1.5f, gameObject.transform.position.z), Quaternion.LookRotation(-transform.forward, new Vector3(0, 1, 0)), transform);

       
    }

    
}
