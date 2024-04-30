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

    [Header("最大HP")]
    [SerializeField] private int maxHp = 150;

    [Header("HP")]
    [SerializeField] private int HP = 150;

    [Header("攻撃力")]
    [SerializeField] private int Attack = 40;

    [Header("怯み値")]
    [SerializeField] private int StaggerThresHold = 3;
    private int StaggerThresHoldCount = 0;

    [Header("攻撃判定")]
    [SerializeField] private EnemyAttack enemyAttack;

    [Header("範囲攻撃判定")]
    [SerializeField] private GameObject enemyskillAttack;

    [Header("AIのオンオフ")]
    [SerializeField] private bool enemyAI = false;

    [SerializeField]
    [Tooltip("対象物(向く方向)")]
    private GameObject target;

    // 視野角（度数法）
    [Header("視野角（度数法）")]
    [SerializeField] private float _sightAngle;

    // 視界の最大距離
    [Header("視界の最大距離")]
    [SerializeField] private float _maxDistance = float.PositiveInfinity;

    [SerializeField]
    private GameObject ShotCollider;
    [SerializeField]
    private int ShotPower;
    [SerializeField]
    private int ShotSpeed;

    [SerializeField] private CharacterGague characterGage;

    [Header("攻撃エフェクト")]
    [SerializeField] private GameObject ATTACK;
    [Header("範囲攻撃エフェクト")]
    [SerializeField] private GameObject SkillAttack;
    [Header("スキルエフェクト")]
    [SerializeField] private GameObject SkillEffect;
    private GameObject obj;
    [SerializeField] private GameObject sword;

    private bool death = false;
    private LeftHandMatcher IK;

    private enum EnemyState
    {
        IDLE,  // 待機
        MOVE,  // 移動
        ATTACK,// 近接攻撃
        SHOT,  // 遠距離攻撃
        SKILLSHOT,  // 遠距離攻撃
        HIT,
    }

    [Header("状態")]
    [SerializeField] private EnemyState enemystate = EnemyState.IDLE;
    private bool DebugColloder = false;
    private Animator _animator;
    private const string StateDefault = "Idol";
    private string State = StateDefault;
    private int Randomnumber;
    private bool IsMove = false;
    private bool MoveEnd = false;
    // stateを切り替える間隔.これが短いほど素早く, 長いほど緩やかに切り替わる.
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
        // 敵のAI：ON
        if (enemyAI)
        {
            if (!death)
            {
                switch (enemystate)
                {
                    case EnemyState.IDLE:
                        // 敵に次の行動を決める乱数
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
                            // 近接攻撃
                            
                                // 敵に次の行動を決める乱数
                                Randomnumber = UnityEngine.Random.Range(1, 100);
                                IsMove = false;
                                //プレイヤーとの距離判定

                                enemystate = EnemyState.MOVE;
                                
                           

                                    // 近接攻撃
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
                                    // 近接スキル
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
                                // 敵に次の行動を決める乱数
                                Randomnumber = UnityEngine.Random.Range(1, 100);
                                IsMove = false;
                                //プレイヤーとの距離判定

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
                        // 遠距離攻撃
                        

                        break;
                    case EnemyState.MOVE:
                        if (!MoveEnd)
                        {
                            
                            //プレイヤーとの距離判定
                            distanceOfPlayer =
                            Vector3.Distance(target.transform.position, this.transform.position);


                            //Quaternion lookRotation = Quaternion.LookRotation(target.transform.position - transform.position, Vector3.up);

                            //lookRotation.z = 0;
                            //lookRotation.x = 0;

                            //transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.7f);

                            
                          
                            if (IsMove && State != "Turn" && State != "TurnLeft")
                            {
                                // 対象物と自分自身の座標からベクトルを算出
                                Vector3 vector3 = target.transform.position - this.transform.position;
                                // もし上下方向の回転はしない(Baseオブジェクトが床から離れないようにする)ようにしたければ以下のようにする。
                                vector3.y = 0f;

                                // Quaternion(回転値)を取得
                                Quaternion quaternion = Quaternion.LookRotation(vector3);
                                // 算出した回転値をこのゲームオブジェクトのrotationに代入
                                //this.transform.rotation = Quaternion.Slerp(transform.rotation, quaternion, 0.5f);
                                this.transform.rotation = quaternion;
                            }

                            if (State == "Turn"|| State == "TurnLeft")
                            {
                                // 対象物と自分自身の座標からベクトルを算出
                                Vector3 vector3 = target.transform.position - this.transform.position;
                                // もし上下方向の回転はしない(Baseオブジェクトが床から離れないようにする)ようにしたければ以下のようにする。
                                vector3.y = 0f;

                                // Quaternion(回転値)を取得
                                Quaternion quaternion = Quaternion.LookRotation(vector3);
                                // 算出した回転値をこのゲームオブジェクトのrotationに代入
                                this.transform.rotation = Quaternion.Slerp(transform.rotation, quaternion, 0.05f);
                            }

                            // プレイヤーの方に移動する処理（予定）
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
                            // 対象物と自分自身の座標からベクトルを算出
                            Vector3 vector3 = target.transform.position - this.transform.position;
                            // もし上下方向の回転はしない(Baseオブジェクトが床から離れないようにする)ようにしたければ以下のようにする。
                            vector3.y = 0f;

                            // Quaternion(回転値)を取得
                            Quaternion quaternion = Quaternion.LookRotation(vector3);
                            // 算出した回転値をこのゲームオブジェクトのrotationに代入
                            this.transform.rotation = Quaternion.Slerp(transform.rotation, quaternion, 0.05f);
                        }
                        break;


                    case EnemyState.HIT:

                        break;
                }

            }
        }
        // 敵のAI：OFF
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

    // 敵のHP減少
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
            // State更新のためUpdate分だけ待つ
            await UniTask.Yield();
            if (token.IsCancellationRequested)
            {
                break;
            }

            var hashExpect = Animator.StringToHash(State);
            var currentState = _animator.GetCurrentAnimatorStateInfo(0);
            if (currentState.shortNameHash != hashExpect)
            {
                // DurationTimeSecondの間隔を挟んでAnimatorのStateを切り替える
                _animator.CrossFadeInFixedTime(hashExpect, DurationTimeSecond);
                // 切り替えている間のcurrentStateは切り替える前のStateが出てくる.
                // そのためDurationTimeSecondが過ぎるまで待つ
                await UniTask.Delay(TimeSpan.FromSeconds(DurationTimeSecond), cancellationToken: token);
                continue;
            }

            // stateが終了していた場合はdefaultに戻す&&死んだときは戻さない
            if (currentState.shortNameHash != hashDefault && currentState.normalizedTime >= 1f)
            {
                if (death)
                {
                    SceneManager.LoadScene("Title");
                }
                if (enemyAI)
                {
                   
                    
                    {
                        // 現在のStateに合わせて終了後のアニメーションを変える
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
            // 存在するStateだけ受け入れる
            State = nextState;
        }
    }

    private bool IsVisible()
    {
        // 自身の位置
        var selfPos = transform.position;
        // ターゲットの位置
        var targetPos = target.transform.position;

        // 自身の向き（正規化されたベクトル）
        var selfDir = transform.forward;

        // ターゲットまでの向きと距離計算
        var targetDir = targetPos - selfPos;
        var targetDistance = targetDir.magnitude;

        // cos(θ/2)を計算
        var cosHalf = Mathf.Cos(_sightAngle / 2 * Mathf.Deg2Rad);

        // 自身とターゲットへの向きの内積計算
        // ターゲットへの向きベクトルを正規化する必要があることに注意
        var innerProduct = Vector3.Dot(selfDir, targetDir.normalized);

        // 視界判定
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
                Debug.Log("遠距離攻撃");
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
    //弱一段
    void Attack1_Start()
    {
        
    }
    void Attack1_End()
    {
        
    }

    //弱二段
    void Attack2_Start()
    {

      
    }
    void Attack2_End()
    {
       
    }

    //弱三段
    void Attack3_Start()
    {

       
    }
    void Attack3_End()
    {
        
    }

    //強攻撃
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
