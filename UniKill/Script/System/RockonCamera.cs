using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockonCamera : MonoBehaviour
{
    GameObject targetObj;
    Vector3 targetPos;
    private bool rock = false;
    private GameObject RockonTarget;
    private GameObject SearchCircle;
    private GameObject MainCamera;
    private GameObject RockCameraPos;
    private Vector3 MainCameraPos;
    private Vector3 center;
    private Vector3 Origin; // cameraの座標
    private float camDistance, camHeight;
    public CameraSwitcher cameraSwitcher;
    private CameraShake cameraShake;

    [SerializeField] GameObject lockonCursor;

    [SerializeField] Camera This;

    public float speed = 10.0F;

    //二点間の距離を入れる
    private float distance_two;
    // Start is called before the first frame update
    void Start()
    {
        targetObj = GameObject.Find("Player");
        targetPos = targetObj.transform.position;
        Origin = transform.position - targetPos;
        camDistance = transform.position.z - targetPos.z;
        camHeight = transform.position.y - targetPos.y;
        This = GameObject.Find("Rockon Camera").GetComponent<Camera>();
        cameraShake = GameObject.Find("Rockon Camera").GetComponent<CameraShake>();
        MainCamera = GameObject.Find("Main Camera");
        MainCameraPos = MainCamera.transform.position;
        RockCameraPos = transform.parent.gameObject;
        //transform.parent = MainCamera.transform;
        //transform.position = MainCamera.transform.position;
        
    }

    // Update is called once per frame
    void Update()

    {
       

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (rock)
            {
                rock = false;
                lockonCursor.SetActive(false);
              
            }
            else
            {
                rock = true;
               
                lockonCursor.SetActive(true);
            }
        }
        if(rock)
        {
            if (RockonTarget == null)
            {
                cameraSwitcher.Switchcamera();
                rock = false;
                lockonCursor.SetActive(false);
                
            }
            else
            {
                //objb,objaの位置関係、角度
                var vec = (targetObj.transform.position - RockonTarget.transform.position);
                // targetの移動量分、自分（カメラ）も移動する
                //if (!cameraShake.isSlow())
                //{ this.transform.position = RockonTarget.transform.position + (vec * 1f) + transform.forward * -8.0f + targetObj.transform.up * 3.0f; }
               
                //transform.position += targetObj.transform.position - targetPos;
                //transform.position = targetObj.transform.position - RockonTarget.transform.position;

                //targetPos = targetObj.transform.position;
                center = (RockonTarget.transform.position + targetPos) * 0.5f;
                //Debug.Log(vec);

                Vector3 EnemyPosition = RockonTarget.transform.position;
                EnemyPosition.y += 2.0f;

                // 対象物と自分自身の座標からベクトルを算出
                Vector3 vector3 = EnemyPosition - this.transform.position;
                // もし上下方向の回転はしない(Baseオブジェクトが床から離れないようにする)ようにしたければ以下のようにする。
                // vector3.y = 0f;

                // Quaternion(回転値)を取得
                Quaternion quaternion = Quaternion.LookRotation(vector3);
                // 算出した回転値をこのゲームオブジェクトのrotationに代入
                this.transform.rotation = quaternion;

                // マウスの移動量
                //float mouseInputX = Input.GetAxis("Mouse X");
                //float mouseInputY = Input.GetAxis("Mouse Y");
                // targetの位置のY軸を中心に、回転（公転）する
                //transform.RotateAround(targetPos, Vector3.up, mouseInputX * Time.deltaTime * 200f);

                
                lockonCursor.transform.Rotate(0, 0, 1f);
                lockonCursor.transform.position = This.WorldToScreenPoint(EnemyPosition);
            }
        }

    }

    public bool Getrock()
    {
        return rock;
    }

    public GameObject GetRockonTarget()
    {
        return RockonTarget;
    }

    public void SetRockonTarget(GameObject gameObject)
    {
        RockonTarget = gameObject;
    }
}
