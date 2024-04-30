using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{

    [Header("�ő�HP")]
    [SerializeField] private int maxHp = 150;

    [Header("HP")]
    [SerializeField] private int HP = 150;

    [Header("�U����")]
    [SerializeField] private int Attack = 40;

    [Header("���ݒl")]
    [SerializeField] private int StaggerThresHold = 3;
    private int StaggerThresHoldCount = 0;

    [Header("�U������")]
    [SerializeField] private EnemyAttack enemyAttack;

    [Header("�͈͍U������")]
    [SerializeField] private GameObject enemyskillAttack;

    [Header("AI�̃I���I�t")]
    [SerializeField] private bool enemyAI = false;

    [SerializeField]
    [Tooltip("�Ώە�(��������)")]
    private GameObject target;

    // ����p�i�x���@�j
    [Header("����p�i�x���@�j")]
    [SerializeField] private float _sightAngle;

    // ���E�̍ő勗��
    [Header("���E�̍ő勗��")]
    [SerializeField] private float _maxDistance = float.PositiveInfinity;

    [SerializeField]
    private GameObject ShotCollider;
    [SerializeField]
    private int ShotPower;
    [SerializeField]
    private int ShotSpeed;

    [SerializeField] private CharacterGague characterGage;

    [Header("�U���G�t�F�N�g")]
    [SerializeField] private GameObject ATTACK;
    [Header("�͈͍U���G�t�F�N�g")]
    [SerializeField] private GameObject SkillAttack;
    [Header("�X�L���G�t�F�N�g")]
    [SerializeField] private GameObject SkillEffect;
    private GameObject obj;
    [SerializeField] private GameObject sword;

    private bool death = false;
    private LeftHandMatcher IK;

    private enum EnemyState
    {
        IDLE,  // �ҋ@
        MOVE,  // �ړ�
        ATTACK,// �ߐڍU��
        SHOT,  // �������U��
        SKILLSHOT,  // �������U��
        HIT,
    }

    [Header("���")]
    [SerializeField] private EnemyState enemystate = EnemyState.IDLE;
    private bool DebugColloder = false;
    private Animator _animator;
    private const string StateDefault = "Idol";
    private string State = StateDefault;
    private int Randomnumber;
    private bool IsMove = false;
    private bool MoveEnd = false;
    // state��؂�ւ���Ԋu.���ꂪ�Z���قǑf����, �����قǊɂ₩�ɐ؂�ւ��.
    public  float DurationTimeSecond = 0.4f;
  
    // Start is called before the first frame update
    void Start()
    {
        if (HP < maxHp)
        {
            HP = maxHp;
        }
        _animator = GetComponent<Animator>();
        AnimationStateLoop().Forget();
        enemyAttack.SetPower(Attack);

        characterGage.SetMaxLife(maxHp);
        IK = GetComponent<LeftHandMatcher>();
    }

    // Update is called once per frame
    void Update()
    {
       
        if(Input.GetKeyDown(KeyCode.P))
        {
            enemyAI = !enemyAI;
            SetState(StateDefault);
            enemystate = EnemyState.IDLE;
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            DebugColloder = !DebugColloder;
        }
        // �G��AI�FON
        if (enemyAI)
        {
            if (!death)
            {
                switch (enemystate)
                {
                    case EnemyState.IDLE:
                        // �G�Ɏ��̍s�������߂闐��
                        Randomnumber = UnityEngine.Random.Range(1, 100);
                        MoveEnd = false;
                        var diff = target.transform.position - transform.position;

                        var axis = Vector3.Cross(transform.forward, diff);

                        var angle = Vector3.Angle(transform.forward, diff) * (axis.y < 0 ? -1 : 1);
                        float distanceOfPlayer =
                           Vector3.Distance(target.transform.position, this.transform.position);
                       

                        if (angle > 0)
                        {
                            State = "Turn";
                        }
                        else
                        {
                            State = "TurnLeft";
                        }
                        if(distanceOfPlayer <= 5.0f)
                        {
                            // �ߐڍU��
                            
                                // �G�Ɏ��̍s�������߂闐��
                                Randomnumber = UnityEngine.Random.Range(1, 100);
                                IsMove = false;
                                //�v���C���[�Ƃ̋�������

                                enemystate = EnemyState.MOVE;
                                
                           

                                    // �ߐڍU��
                                    if (Randomnumber <= 50)
                                    {
                                         Debug.Log(Randomnumber);
                                        if (IsVisible() && distanceOfPlayer <= 3.0f)
                                        {
                                            State = "Attack_1";

                                            enemystate = EnemyState.ATTACK;
                                            MoveEnd = true;
                                        }
                                         Debug.Log(Randomnumber);
                                    }
                                    // �ߐڃX�L��
                                    else
                                    {
                                        State = "Shot_1";
                                        enemystate = EnemyState.SKILLSHOT;
                                    }
                                
                              

                            
                        }
                        else
                        {
                            if (Randomnumber <= 40)
                            {
                                // �G�Ɏ��̍s�������߂闐��
                                Randomnumber = UnityEngine.Random.Range(1, 100);
                                IsMove = false;
                                //�v���C���[�Ƃ̋�������

                                enemystate = EnemyState.MOVE;
                                if (distanceOfPlayer <= 3.0f && IsVisible())
                                {

                                    MoveEnd = true;
                                    State = "Attack_1";
                                    enemystate = EnemyState.ATTACK;
                                   
                                }
                               

                            }
                            else
                            {

                               
                               
                                enemystate = EnemyState.SHOT;
                            }

                        }
                        // �������U��
                        

                        break;
                    case EnemyState.MOVE:
                        if (!MoveEnd)
                        {
                            
                            //�v���C���[�Ƃ̋�������
                            distanceOfPlayer =
                            Vector3.Distance(target.transform.position, this.transform.position);


                            //Quaternion lookRotation = Quaternion.LookRotation(target.transform.position - transform.position, Vector3.up);

                            //lookRotation.z = 0;
                            //lookRotation.x = 0;

                            //transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.7f);

                            
                          
                            if (IsMove && State != "Turn" && State != "TurnLeft")
                            {
                                // �Ώە��Ǝ������g�̍��W����x�N�g�����Z�o
                                Vector3 vector3 = target.transform.position - this.transform.position;
                                // �����㉺�����̉�]�͂��Ȃ�(Base�I�u�W�F�N�g�������痣��Ȃ��悤�ɂ���)�悤�ɂ�������Έȉ��̂悤�ɂ���B
                                vector3.y = 0f;

                                // Quaternion(��]�l)���擾
                                Quaternion quaternion = Quaternion.LookRotation(vector3);
                                // �Z�o������]�l�����̃Q�[���I�u�W�F�N�g��rotation�ɑ��
                                //this.transform.rotation = Quaternion.Slerp(transform.rotation, quaternion, 0.5f);
                                this.transform.rotation = quaternion;
                            }

                            if (State == "Turn"|| State == "TurnLeft")
                            {
                                // �Ώە��Ǝ������g�̍��W����x�N�g�����Z�o
                                Vector3 vector3 = target.transform.position - this.transform.position;
                                // �����㉺�����̉�]�͂��Ȃ�(Base�I�u�W�F�N�g�������痣��Ȃ��悤�ɂ���)�悤�ɂ�������Έȉ��̂悤�ɂ���B
                                vector3.y = 0f;

                                // Quaternion(��]�l)���擾
                                Quaternion quaternion = Quaternion.LookRotation(vector3);
                                // �Z�o������]�l�����̃Q�[���I�u�W�F�N�g��rotation�ɑ��
                                this.transform.rotation = Quaternion.Slerp(transform.rotation, quaternion, 0.05f);
                            }

                            // �v���C���[�̕��Ɉړ����鏈���i�\��j
                            if (distanceOfPlayer > 3.0f)
                            {
                                IsMove = true;
                               
                               
                            }
                            else
                            {
                                IsMove = false;
                            }



                            if(IsMove && State != "Turn" && State != "TurnLeft")
                            {
                                Vector3 p = new Vector3(0f, 0f, 0.1f);

                                transform.Translate(p);
                            }

                            if(IsVisible() && !IsMove && State != "Turn" && State != "TurnLeft")
                            {
                                
                                MoveEnd = true;
                                State = "Attack_1";
                                enemystate = EnemyState.ATTACK;
                            }
                        }


                        break;
                    case EnemyState.ATTACK:

                        break;
                    case EnemyState.SHOT:
                        if (State == "Turn" || State == "TurnLeft")
                        {
                            // �Ώە��Ǝ������g�̍��W����x�N�g�����Z�o
                            Vector3 vector3 = target.transform.position - this.transform.position;
                            // �����㉺�����̉�]�͂��Ȃ�(Base�I�u�W�F�N�g�������痣��Ȃ��悤�ɂ���)�悤�ɂ�������Έȉ��̂悤�ɂ���B
                            vector3.y = 0f;

                            // Quaternion(��]�l)���擾
                            Quaternion quaternion = Quaternion.LookRotation(vector3);
                            // �Z�o������]�l�����̃Q�[���I�u�W�F�N�g��rotation�ɑ��
                            this.transform.rotation = Quaternion.Slerp(transform.rotation, quaternion, 0.05f);
                        }
                        break;


                    case EnemyState.HIT:

                        break;
                }

            }
        }
        // �G��AI�FOFF
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                State = "Attack_1";
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                State = "Attack_2";
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                State = "Shot_1";
                enemystate = EnemyState.SHOT;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                State = "Shot_1";
                enemystate = EnemyState.SKILLSHOT;
            }
        }
        
    }

    // �G��HP����
    public void MinusEnemyHp (int Damage)
    {
        IK.SwitchIK(false);
        characterGage.GaugeReduction(Damage, HP);
        HP -= Damage;
        if(StaggerThresHoldCount >= StaggerThresHold)
        {
            State = "Damage";
            DurationTimeSecond = 0.1f;
            enemystate = EnemyState.HIT;
            StaggerThresHoldCount = 0;
            enemyAttack.OffAttackCollider();
        }
        if (StaggerThresHoldCount < StaggerThresHold)
        {
            StaggerThresHoldCount++;
            
        }
       
        if(HP <= 0)
        {
            DurationTimeSecond = 0.1f;
            State = "Harddamage";
            death = true;
        }
        
    }
    public void HitAction()
    {
        StaggerThresHoldCount = 10;
    }


    private async UniTaskVoid AnimationStateLoop()
    {
        var token = this.GetCancellationTokenOnDestroy();
        var hashDefault = Animator.StringToHash(StateDefault);

        while (true)
        {
            // State�X�V�̂���Update�������҂�
            await UniTask.Yield();
            if (token.IsCancellationRequested)
            {
                break;
            }

            var hashExpect = Animator.StringToHash(State);
            var currentState = _animator.GetCurrentAnimatorStateInfo(0);
            if (currentState.shortNameHash != hashExpect)
            {
                // DurationTimeSecond�̊Ԋu�������Animator��State��؂�ւ���
                _animator.CrossFadeInFixedTime(hashExpect, DurationTimeSecond);
                // �؂�ւ��Ă���Ԃ�currentState�͐؂�ւ���O��State���o�Ă���.
                // ���̂���DurationTimeSecond���߂���܂ő҂�
                await UniTask.Delay(TimeSpan.FromSeconds(DurationTimeSecond), cancellationToken: token);
                continue;
            }

            // state���I�����Ă����ꍇ��default�ɖ߂�&&���񂾂Ƃ��͖߂��Ȃ�
            if (currentState.shortNameHash != hashDefault && currentState.normalizedTime >= 1f)
            {
                if (death)
                {
                    SceneManager.LoadScene("Title");
                }
                if (enemyAI)
                {
                   
                    
                    {
                        // ���݂�State�ɍ��킹�ďI����̃A�j���[�V������ς���
                        switch (enemystate)
                        {
                            case EnemyState.IDLE:
                                SetState(StateDefault);
                                enemystate = EnemyState.IDLE;
                                break;
                            case EnemyState.MOVE:
                                if (State == "Turn" || State == "TurnLeft")
                                {
                                    float distanceOfPlayer =
                                    Vector3.Distance(target.transform.position, this.transform.position);
                                    if (distanceOfPlayer > 3.0f)
                                    {
                                        IsMove = true;
                                    }
                                    if (!IsMove)
                                    {
                                        MoveEnd = true;
                                        State = "Attack_1";
                                        enemystate = EnemyState.ATTACK;

                                    }
                                    else
                                    {
                                        State = "Move";
                                       
                                    }

                                }

                                break;
                            case EnemyState.ATTACK:
                                SetState(StateDefault);
                                enemystate = EnemyState.IDLE;
                                break;
                            case EnemyState.SHOT:
                                //SetState(StateDefault);
                                //enemystate = EnemyState.IDLE;
                                if (State == "Turn" || State == "TurnLeft")
                                {
                                    State = "Shot_1";
                                }
                                else
                                {
                                    SetState(StateDefault);
                                    enemystate = EnemyState.IDLE;
                                }
                                break;
                            case EnemyState.SKILLSHOT:
                                SetState(StateDefault);
                                enemystate = EnemyState.IDLE;
                                break;
                            case EnemyState.HIT:
                                SetState(StateDefault);
                                IK.SwitchIK(true);
                                DurationTimeSecond = 0.4f;
                                enemystate = EnemyState.IDLE;
                                break;
                        }

                    }
                }
                else
                {
                    SetState(StateDefault);
                    enemystate = EnemyState.IDLE;
                }
               
            }
        }
    }

    public void SetState(string nextState)
    {
        if (_animator.HasState(0, Animator.StringToHash(nextState)))
        {
            // ���݂���State�����󂯓����
            State = nextState;
        }
    }

    private bool IsVisible()
    {
        // ���g�̈ʒu
        var selfPos = transform.position;
        // �^�[�Q�b�g�̈ʒu
        var targetPos = target.transform.position;

        // ���g�̌����i���K�����ꂽ�x�N�g���j
        var selfDir = transform.forward;

        // �^�[�Q�b�g�܂ł̌����Ƌ����v�Z
        var targetDir = targetPos - selfPos;
        var targetDistance = targetDir.magnitude;

        // cos(��/2)���v�Z
        var cosHalf = Mathf.Cos(_sightAngle / 2 * Mathf.Deg2Rad);

        // ���g�ƃ^�[�Q�b�g�ւ̌����̓��όv�Z
        // �^�[�Q�b�g�ւ̌����x�N�g���𐳋K������K�v�����邱�Ƃɒ���
        var innerProduct = Vector3.Dot(selfDir, targetDir.normalized);

        // ���E����
        return innerProduct > cosHalf && targetDistance < _maxDistance;
    }

    void OnCollider()
    {
        
        enemyAttack.OnAttackCollider();

    }
    void OffCollider()
    {

        enemyAttack.OffAttackCollider();

    }
    void Shot()
    {
       
        switch (enemystate)
        {
            case EnemyState.SHOT:
                Debug.Log("�������U��");
                GameObject ball = (GameObject)Instantiate(ShotCollider, transform.position, Quaternion.identity);
                Vector3 pos = ball.transform.position;
                pos.y += 1.0f;
                ball.transform.position = pos;
                Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
                ballRigidbody.AddForce(transform.forward * ShotSpeed);
                ball.GetComponent<EnemyShot>().SetPower(ShotPower);
                if(DebugColloder)
                {
                    ball.GetComponent<EnemyShot>().SetDebugColloder(DebugColloder);
                }
                break;
            case EnemyState.SKILLSHOT:
                GameObject skillAttack = Instantiate(enemyskillAttack, transform.position, Quaternion.identity);
                if (DebugColloder)
                {
                    
                    skillAttack.GetComponent<EnemySkillAttack>().SetDebugColloder(DebugColloder);
                }
                break;
        }

    }
    
    //void OnSkillCollider()
    //{
    //    enemyAttack.OnAttackCollider();

    //}
    //void OffSkillCollider()
    //{
    //    enemyAttack.OffAttackCollider();
    //}
    //���i
    void Attack1_Start()
    {
        
    }
    void Attack1_End()
    {
        
    }

    //���i
    void Attack2_Start()
    {

      
    }
    void Attack2_End()
    {
       
    }

    //��O�i
    void Attack3_Start()
    {

       
    }
    void Attack3_End()
    {
        
    }

    //���U��
    void Skill_Start()
    {
       
    }
    void Skill_End()
    {
        
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
        obj.transform.Rotate(new Vector3(-80.0f, 30.0f, 0.0f));
        obj.transform.parent = null;
    }
    void SkillEffectOn()
    {

        switch (enemystate)
        {
            case EnemyState.SHOT:
                Instantiate(SkillEffect, this.transform.position, Quaternion.identity);
                break;
            case EnemyState.SKILLSHOT:
                Instantiate(SkillAttack, this.transform.position, Quaternion.identity);
                break;
        }
       
        
    }
}
