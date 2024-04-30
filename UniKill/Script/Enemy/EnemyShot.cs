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

    private float _repeatSpan;    //繰り返す間隔
    private float _timeElapsed;   //経過時間
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        _repeatSpan = 0.1f;    //実行間隔を５に設定
        _timeElapsed = 0;   //経過時間をリセット
    }

    // Update is called once per frame
    void Update()
    {
        _timeElapsed += Time.deltaTime;     //時間をカウントする
        //経過時間が繰り返す間隔を経過したら
        if (_timeElapsed >= _repeatSpan)
        {
            //ここで処理を実行
            Instantiate(Effect, transform.position, Quaternion.identity);
            _timeElapsed = 0;   //経過時間をリセットする
        }
        if (DebugColloder)
        {
            GetComponent<MeshRenderer>().enabled = true;
            DebugColloder = false;
        }
        //消え方の処理
        if (Hit)
        {

        }
    }

   

    private void OnTriggerStay(Collider other)
    {
        //当たったゲームオブジェクトがEnemyタグだったとき
        if (!Hit && other.gameObject.CompareTag("Player") && !player.isInvincibility())
        {

            //Instantiate(prefab ,gameObject.transform.position, Quaternion.identity);

            Debug.Log("プレイヤーに攻撃ヒット");

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
