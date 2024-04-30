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

    private float _repeatSpan;    //繰り返す間隔
    private float _timeElapsed;   //経過時間
    private CameraShake timeManager;
    private RockonCamera rockonCamera;
    private CameraShake subtimeManager;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GameObject.Find("Enemy").GetComponent<Enemy>();
        _repeatSpan = 0.1f;    //実行間隔を５に設定
        _timeElapsed = 0;   //経過時間をリセット
        timeManager = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        rockonCamera = GameObject.Find("Rockon Camera").GetComponent<RockonCamera>();
        subtimeManager = GameObject.Find("Rockon Camera").GetComponent<CameraShake>();
        // Instantiate(Effect, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        //_timeElapsed += Time.deltaTime;     //時間をカウントする
        ////経過時間が繰り返す間隔を経過したら
        //if (_timeElapsed >= _repeatSpan)
        //{
        //    //ここで処理を実行
        //    Instantiate(Effect, transform.position, Quaternion.identity);
        //    _timeElapsed = 0;   //経過時間をリセットする
        //}
        if (DebugColloder)
        {
            GetComponent<MeshRenderer>().enabled = true;
            DebugColloder = false;
        }
        ////消え方の処理
        //if (Hit)
        //{

        //}
    }

   

    private void OnTriggerStay(Collider other)
    {
        //当たったゲームオブジェクトがEnemyタグだったとき
        if (!Hit && other.gameObject.CompareTag("Enemy"))
        {

            //Instantiate(prefab ,gameObject.transform.position, Quaternion.identity);

            Debug.Log("プレイヤーに攻撃ヒット");
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
