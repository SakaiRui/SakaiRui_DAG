using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameramove : MonoBehaviour
{
    //�v���C���[��ϐ��Ɋi�[
    public GameObject Player;
   
    private Vector3 offset;      //���΋����擾�p

    //�J������xz�ǂ���̎�����Ɍ��Ă��邩
    public float xz;

    // Use this for initialization
    void Start()
    {
        xz = transform.localEulerAngles.y;
       
       
    }

    // Update is called once per frame
    void Update()
    {
        
        //�v���C���[�ʒu���
        Vector3 playerPos = Player.transform.position;
        
        xz = transform.localEulerAngles.y;

        //��]������p�x
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
           
            transform.RotateAround(playerPos, Vector3.up, 90f);
           
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
          
            transform.RotateAround(playerPos, Vector3.up, -90f);
            
        }
        
    }
}
