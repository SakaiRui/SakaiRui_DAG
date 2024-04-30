using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillAttack : MonoBehaviour
{
    [SerializeField]
    private int AttackPower = 10;
    [SerializeField]
    private GameObject Effect;
    private Player player;
    private bool Hit = false;
    private bool DebugColloder = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if(DebugColloder && !Hit)
        {
            GetComponent<MeshRenderer>().enabled = true;
            DebugColloder = false;
        }
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    //当たったゲームオブジェクトがEnemyタグだったとき
    //    if (other.gameObject.CompareTag("Player")&& !player.isInvincibility())
    //    {

    //        //Instantiate(prefab ,gameObject.transform.position, Quaternion.identity);



    //        GetComponent<Collider>().enabled = false;
    //        //forceField.enabled = false;
    //        player.MinusHp(AttackPower);
    //        Hit = true;
    //    }
    //}

    private void OnTriggerStay(Collider other)
    {
        //当たったゲームオブジェクトがEnemyタグだったとき
        if (!Hit && other.gameObject.CompareTag("Player") && !player.isInvincibility())
        {

            //Instantiate(prefab ,gameObject.transform.position, Quaternion.identity);



            GetComponent<Collider>().enabled = false;
            //forceField.enabled = false;
            player.MinusHp(AttackPower);
            Hit = true;
            GetComponent<MeshRenderer>().enabled = false;
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
