using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEditor.Animations;

/*
 ================
 �T�v�F�v���C���[�̓����̏���
 ================
 */
public class Player : MonoBehaviour
{

    private Animator animator;



    private enum JumpState
    {
        IDLE,    // ���͑҂����
        WAITING,    // �W�����v���ߏ��
        RISING,    // �㏸�����
        FALLING,    // ���~�����
        DOUBLE,
        LANDING,    // ���n���
    }

    //�̗�
    [Header("�ő�HP")]
    [SerializeField] private float MaxHp = 100;

    [Header("���݂�HP\n(���݂�HP�̓f�o�b�N�p��'E'�Ō���'R'�ő����A\n'L'�ŃR���\�[���Ƀf�o�b�N�\��)")]
    [SerializeField] private float Hp = 100;

   
    [Header("��e���̖��G����(�d������)")]
    [SerializeField] private float InvincibilityTime = 1.0f;
    private float InvincibilityTimeCount = 0.0f;

    private float plusMpCount = 1;
    private float MaxplusMpCount = 0;

    //�Z�̃N�[���_�E��
    [Header("�Z�̃N�[���_�E��\n('K�ŃN�[���_�E�������A�N�[���_�E���������̓R���\�[���Ƀf�o�b�N�\��')")]
    [SerializeField] private float SkillCoolTime = 3.0f;

    //���E�̈ړ��X�s�[�h
    [Header("����X�s�[�h")]
    [SerializeField] private float AvoidanceSpeed = 3.0f;

    //���E�̈ړ��X�s�[�h
    [Header("���E�̃_�b�V���ړ��X�s�[�h")]
    [SerializeField] private float sprintSpeed = 5.0f;
    private bool Issprint = false;

    

  
    [Header("��𖳓G����")]
    [SerializeField] private int AvoidanceInvincibilityTime = 10;

    [Header("�W���X�g��𖳓G����")]
    [SerializeField] private int SuperAvoidanceInvincibilityTime = 10;

    [Header("�W���X�g��𔻒莞��")]
    [SerializeField] private int SuperAvoidancejudgeTime = 10;

    private int AvoidanceInvincibilityTimeCount = 0;

    private int SuperAvoidancejudgeCount = 0;
    private int SuperAvoidanceInvincibilityTimeCount = 0;

    private bool IsSuperAvoidancejudge = false;

    private bool IsSuperAvoidance = false;

    private bool IsSuperAvoidanceAttack = false;


    [Header("�W���X�g����㑖�鎞��")]
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

    //�W�����v�̋���
    [Header("�W�����v�̋���")]
    [SerializeField] private float jumpPower = 3.0f;

    
  

    //�U����
    [Header("�U����")]
    [SerializeField] private int AttackPower = 10;

  

    //���U����
    [Header("���U����")]
    [SerializeField] private int SkillAttackPower = 10;

    //���U���̍ő嗭�ߎ���
    [Header("���U���̍ő嗭�ߎ���")]
    [SerializeField] private float MaxSkillAttackTime = 3.0f;
    private float SkillAttackTimeCount = 0.0f;
    private bool NowCharge = false;

    //���U����
    [Header("�󒆍U����")]
    [SerializeField] private int AirAttackPower = 10;

    //�����蔻��I�u�W�F�N�g
    [Header("��U�������蔻��I�u�W�F�N�g(������)")]
    [SerializeField] private GameObject attackObject;
    private Collider attackCollider;
    private Attack attack;

    


    //�����蔻��I�u�W�F�N�g
    [Header("�󒆍U�������蔻��I�u�W�F�N�g(������)")]
    [SerializeField] private GameObject airattackObject;
    //private Collider attackCollider;
    //�󒆍U���̍d������
    [Header("�󒆍U���̍d������")]
    [SerializeField] private float airattackstun = 1.0f;

    //�󒆍U���̍U�������^�C�~���O
    [Header("�󒆍U���̍U�������^�C�~���O")]
    [SerializeField] private float airStartAttckTime = 1.0f;
    private Attack airattack;

    //�����蔻��I�u�W�F�N�g
    [Header("���U�������蔻��I�u�W�F�N�g(������)")]
    [SerializeField] private GameObject SkillattackObject;
    private Collider SkillattackCollider;
    private Attack Skillattack;

    //���U���̍d������
    [Header("���U���̍d������")]
    [SerializeField] private float Skillattackstun = 1.0f;

    //���U���̍U�������^�C�~���O
    [Header("���U���̍U�������^�C�~���O")]
    [SerializeField] private float SkillStartAttckTime = 1.0f;
    private Rigidbody rb;



   

    [SerializeField] private TrailRenderer trail1;

    [Header("���n�G�t�F�N�g")]
    [SerializeField] GameObject prefab;

    [Header("���")]
    [SerializeField] GameObject dashprefab;

    [Header("�W���X�g���")]
    [SerializeField] GameObject justprefab;

    [Header("HP��")]
    [SerializeField] GameObject HPprefab;

 
    [Header("Charge��")]
    [SerializeField] GameObject Chargeprefab;

    [Header("HPlow")]
    [SerializeField] GameObject LowHpprefab;

    [Header("��_���G�t�F�N�g")]
    [SerializeField] GameObject Damage;

    [Header("����U���G�t�F�N�g")]
    [SerializeField] GameObject special;


    [Header("�U���G�t�F�N�g")]
    [SerializeField] private GameObject ATTACK;
    private GameObject obj;
    [SerializeField] private GameObject sword;
    private GameObject LowHppEffect;

    [SerializeField] private float turnSpeed = 10.0f;   // ��]���x

    private GameObject Charge;

    private LeftHandMatcher IK;

    private Effekseer.EffekseerEmitter effekseerEmitter;

    // �e�I�u�W�F�N�g
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

    [SerializeField] private Vector3 velocity;              // �ړ�����
    [SerializeField] private float moveSpeed = 10.0f;        // �ړ����x
    [SerializeField] private float applySpeed = 0.2f;       // �U������̓K�p���x
    [SerializeField] private PlayerFollowCamera refCamera;  // �J�����̐�����]���Q�Ƃ���p

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
       

        

        //�v���C���[�̒�~����
        if (!IsAvoidance && !NowAttack && !NowSkillAttack && JumpCount == 0
               && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.J) && !IsHit && !IsSuperAvoidancedash && !IsSuperAvoidance && !IsSuperAvoidanceAttack)
        {
          
            animator.SetBool("walk", false);
            animator.SetBool("run", false);
            animator.SetBool("avoidance", false);
           // Debug.Log("��~��");
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.W) && !IsAvoidance && jumpTimer == 0 && jumpState != JumpState.WAITING && !airAttck)
        {
            animator.SetBool("walk", false);
            animator.SetBool("run", false);
            animator.SetBool("avoidance", false);
            
           

        }

       
       

        //��e�A�j���[�V�������̖��G���_��
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

        //�ړ�
        {
            //�U�����͓����Ȃ�
            if (!NowSkillAttack &&!NowAttack && !NowCharge && !IsHit)
            {
                
               // Debug.Log(animator.GetBool("walk"));
                //��𒆂͓����Ȃ�
                if (IsAvoidance && !IsSuperAvoidance)
                {

                    if(IsSuperAvoidancejudge)
                    {
                        SuperAvoidancejudgeCount++;
                        Debug.Log("�W���X�g��𔻒蒆");
                        if (SuperAvoidancejudgeCount >= SuperAvoidancejudgeTime)
                        {
                            IsSuperAvoidancejudge = false;
                            SuperAvoidancejudgeCount = 0;
                        }
                        else if(Input.GetKeyDown(KeyCode.LeftShift))
                        {
                            Debug.Log("�W���X�g���");
                            //�W���X�g����̏���
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
                    //���G���Ԓ��Ȃ�
                    if (AvoidanceInvincibilityTimeCount <= AvoidanceInvincibilityTime && IsInvincibility == true)
                    {

                        AvoidanceInvincibilityTimeCount += 1;
                       // Debug.Log(AvoidanceInvincibilityTimeCount);
                    }
                    //���G���ԏI��
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
                    //���G���Ԓ��Ȃ�
                    if (SuperAvoidanceInvincibilityTimeCount <= SuperAvoidanceInvincibilityTime && IsInvincibility == true)
                    {

                        SuperAvoidanceInvincibilityTimeCount += 1;
                        // Debug.Log(AvoidanceInvincibilityTimeCount);
                    }
                    //���G���ԏI��
                    else
                    {
                        gameObject.layer = LayerMask.NameToLayer("Default");
                        IsInvincibility = false;
                        SuperAvoidanceInvincibilityTimeCount = 0;
                    }


                    transform.position += speed * transform.forward * Time.deltaTime;

                   
                }

                //���
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


                // W�L�[�i�O�ړ��j
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

                // S�L�[�i���ړ��j
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

                // D�L�[�i�E�ړ��j
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

                // A�L�[�i���ړ��j
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
                    Debug.Log("���dash");
                   
                    if (SuperAvoidancedashTimeCount >= SuperAvoidancedashTime)
                    {
                        IsSuperAvoidancedash = false;
                        SuperAvoidancedashTimeCount = 0;
                        animator.SetBool("SuperAvoidanceDash", false);
                        IK.SwitchIK(true);
                        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W))
                        {
                            animator.SetBool("walk", true);
                            Debug.Log("����������");
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


               
                //�W�����v
                if (Input.GetKeyDown(KeyCode.Space) && JumpCount == 0 && jumpTimer == 0 && jumpState != JumpState.WAITING && jumpState != JumpState.LANDING && !IsAvoidance && jumpState != JumpState.DOUBLE && !IsSuperAvoidancedash && !IsSuperAvoidancedash && !IsSuperAvoidanceAttack)
                {
                    rb.velocity = Vector3.up * jumpPower + new Vector3(rb.velocity.x, 0, rb.velocity.z);
                    JumpCount++;
                    animator.SetBool("Jump", true);
                    animator.SetBool("walk", false);
                    animator.SetBool("doublerising", false);
                    animator.SetBool("Airattack", false);
                }
                
                //�W�����v���̃A�j���[�V�����̐���
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
                            // Debug.Log("�n�ʂɂ���");
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
        //HP��MP�̏���
        {
            //'E'��HP����(�f�o�b�N�p)
            if (Input.GetKeyDown(KeyCode.E))
            {
                Hp -= 10;
                characterGage.GaugeReduction(10, Hp);
            }

            //'R'��HP����(�f�o�b�N�p)
            if (Input.GetKeyDown(KeyCode.R))
            {
                Hp++;
            }



            //���݂�HP��MP�̃f�o�b�N�\��
            if (Input.GetKeyDown(KeyCode.L))
            {
                Debug.Log("HP�F" + Hp);
             
            }

           
        }

        //�U��
        {
            
            //��U��
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

            //�󒆍U���̏����̂ݎ���
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
            //    //    SoundManager.Instance.GetSEVolume()); // �ʏ�U�����Đ�
            //    if (Issprint)
            //    {
            //        Issprint = false;
            //        speed = normalSpeed;
            //        //----------------------
            //        //�ύX
            //        //----------------------
            //        animator.SetBool("run", false);
            //    }
            //    if (lowHp)
            //    {
            //        Debug.Log("ko");

            //    }
            //    //----------------------
            //    //�ύX
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

        //���U��
        {

          
            //�Z���g�p����ĂȂ��āA'K'�������ꂽ��N�[���_�E���������g�p�t���O��true��
            if (Input.GetKeyDown(KeyCode.Q) && !UseSkill && !NowSkillAttack && JumpCount == 0 && !NowAttack && !IsAvoidance && !IsHit && !IsSuperAvoidance && !IsSuperAvoidancedash && !IsSuperAvoidanceAttack)
            {
               
                if (Issprint)
                {
                    Issprint = false;
                    speed = AvoidanceSpeed;
                    //----------------------
                    //�ύX
                    //----------------------
                    animator.SetBool("run", false);
                }
                animator.SetBool("walk", false);

                Debug.Log("�X�L���J�n");
                animator.SetBool("skill", true);

            }

            if (NowCharge)
            {
                Debug.Log("Charge��");
                SkillAttackTimeCount += Time.deltaTime;

                //animator.speed *= -1f;

                //���ߒi�K�ŃG�t�F�N�g�ύX
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
                    //�L�[�𗣂������̗��ߒi�K�ōU���͕ύX
                    if (SkillAttackTimeCount < MaxSkillAttackTime / 2.0f)
                    {
                        Debug.Log("���ߖ�");
                        Skillattack.SetPower(SkillAttackPower);
                        Skillattack.SetSENumber(1);
                      
                    }
                    if (SkillAttackTimeCount >= MaxSkillAttackTime / 2.0f && SkillAttackTimeCount < MaxSkillAttackTime)
                    {
                        Debug.Log("����1");
                        Skillattack.SetPower(SkillAttackPower + 10);
                        Skillattack.SetSENumber(1);
                     
                    }
                    if (SkillAttackTimeCount >= MaxSkillAttackTime)
                    {
                        Debug.Log("����2");
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

            //�Z�̎g�p�t���O��true�Ȃ�N�[���_�E������
            if (SkilTime > 0.0f && UseSkill)
            {
                SkilTime -= Time.deltaTime;
                //�Z�̃N�[���_�E����0�ɂȂ�����g�p�t���O��false��
                if (SkilTime <= 0.0f)
                {
                    SkilTime = 0.0f;
                    UseSkill = false;
                }
                //�Z�̃N�[���_�E���̃f�o�b�N�\��
                // Debug.Log(SkilTime);
            }


        }

        //HP��MP������l�ȏ�ɂȂ�Ȃ��悤��
        if (Hp >= MaxHp)
        {
            Hp = MaxHp;
        }
       
        //HP��0�����ɂȂ�Ȃ��悤��
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

            // �J�����̕�������AX-Z���ʂ̒P�ʃx�N�g�����擾
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

            // �����L�[�̓��͒l�ƃJ�����̌�������A�ړ�����������
            Vector3 moveForward = cameraForward * inputVertical + Camera.main.transform.right * inputHorizontal;

           // Debug.Log(rockonCamera.Getrock());

            if (!rockonCamera.Getrock())
            {
               

               // Debug.Log("�t���[");
                // �L�����N�^�[�̌�����i�s������
                if (moveForward != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveForward), applySpeed);
                }
                // �ړ������ɃX�s�[�h���|����B�W�����v�◎��������ꍇ�́A�ʓrY�������̑��x�x�N�g���𑫂��B
                if (!IsSuperAvoidancedash)
                rb.velocity = moveForward.normalized * moveSpeed + new Vector3(0, rb.velocity.y, 0);
            }
            else
            {
               
                cameraForward = Vector3.Scale(Rockcamera.transform.forward, new Vector3(1, 0, 1)).normalized;
                
                //�J��������]������
      
                Vector3 vector3 = rockonCamera.GetRockonTarget().transform.position - this.transform.position;
                vector3.y = 0.0f;
                //��]������p�x
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
                    //Debug.Log("�ʂ���");
                    transform.RotateAround(rockonCamera.GetRockonTarget().transform.position, Vector3.up, angle);
                }
                
                // �����L�[�̓��͒l�ƃJ�����̌�������A�ړ�����������
                moveForward = vector3.normalized * inputVertical;
                // �ړ������ɃX�s�[�h���|����B�W�����v�◎��������ꍇ�́A�ʓrY�������̑��x�x�N�g���𑫂��B
                if (!IsSuperAvoidancedash)
                    rb.velocity = moveForward.normalized * moveSpeed + new Vector3(0, rb.velocity.y, 0);

                moveForward = cameraForward.normalized * inputVertical + Rockcamera.transform.right * inputHorizontal;
                // �L�����N�^�[�̌�����i�s������
                
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


    //���݂�HP�̎擾
    public float GetHp()
    {
        return Hp;
    }


    public float GetMaxHp()
    {
        return MaxHp;
    }

    //�O����Ăяo���ăv���C���[��HP����
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

            //�@�q�b�g�X�g�b�v
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
           

            
            
          
            //����̍d�����͔�_����傫��
            if (IsAvoidance)
            {
                damage += 20;
            }
            Debug.Log(damage);

            characterGage.GaugeReduction(damage, Hp);

            Hp -= damage; // �_���[�W���A�v���C���[�̗̑͂����炷
            
            if (Hp <= 0) // �̗͂�0�ȉ��ɂȂ��������
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
            //�W���X�g����̏���
            RocktimeManager.SlowDown(0);
            Debug.Log("�W���X�g���");
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

   

    //�Z�̃N�[���_�E��
    public float GetCoolTime()
    {
        return SkilTime;
    }

    //�Z�̎g�p�t���O
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
        // �e�I�u�W�F�N�g��Transform���擾
        var parentTransform = parent.transform;

        // �q�I�u�W�F�N�g���i�[����z��쐬
        var children = new GameObject[parentTransform.childCount];

        // 0�`��-1�܂ł̎q�����Ԃɔz��Ɋi�[
        for (var i = 0; i < children.Length; ++i)
        {
            // Transform����Q�[���I�u�W�F�N�g���擾���Ċi�[
            children[i] = parentTransform.GetChild(i).gameObject;
        }

        // �q�I�u�W�F�N�g���i�[���ꂽ�z��
        return children;
    }
    //---------------------------------
    //�A�j���[�^�[�C�x���g�ŌĂяo��
    //---------------------------------

    //���i
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

    //���i
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

    //��O�i
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

    //���U��
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

    //�U���̓����蔻�蔭��
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

    //���U���̗��ߊJ�n
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
