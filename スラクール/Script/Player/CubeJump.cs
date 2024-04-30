using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeJump : MonoBehaviour
{
    private Rigidbody rb;
    private int upForce;
    private Vector3 mousePosition;
    private Vector3 objPosition;
    private float rayDistance;
    public bool isGround;
    public GameObject MainCamera;
   
    private float speed = 4.5f;
    public float timeBetweenShot = 0.35f;
    public float timeBetweenjump = 0.35f;
    private float timer;
    private float time;
    private float boundtimer;
    
    public float timebound = 0.2f;
    private Vector3 pos;

    public bool bound1;
    bound script;

    public AudioClip sound1;
    AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        upForce = 3;
        rayDistance = 0.5f;
        rb.isKinematic = false;
        isGround = false;
        bound1 = false;
        //Vector3 pos = GameObject.Find("Cube").transform.position;
        audioSource = GetComponent<AudioSource>();
        script = GameObject.Find("1").GetComponent<bound>();
    }

    // Update is called once per frame
    void Update()
    {
        bound1 = false;
        // タイマーの時間を動かす
        timer += Time.deltaTime;
        time += Time.deltaTime;
        boundtimer += Time.deltaTime;
       
        Vector3 rayPosition = transform.position + new Vector3(0.0f, 0.0f, 0.0f);

        Ray ray1 = new Ray(rayPosition, Vector3.right);
        Ray ray2 = new Ray(rayPosition, Vector3.left);
        Ray ray3 = new Ray(rayPosition, Vector3.forward);
        Ray ray4 = new Ray(rayPosition, Vector3.back);

        RaycastHit hit;
        if (timer > timeBetweenShot)
        {
            //レイがヒットしたら壁に引っ付く
            if (Physics.Raycast(ray1, out hit, rayDistance))
            {
                if (hit.collider.name == "Wall")
                {
                    rb.isKinematic = true;
                    isGround = true;
                    // タイマーの時間を０に戻す。
                    timer = 0.0f;
                }
            }
            if (Physics.Raycast(ray2, out hit, rayDistance))
            {
                if (hit.collider.name == "Wall")
                {
                    rb.isKinematic = true;
                    isGround = true;
                    // タイマーの時間を０に戻す。
                    timer = 0.0f;
                }
            }
            if (Physics.Raycast(ray3, out hit, rayDistance))
            {
                if (hit.collider.name == "Wall")
                {
                    rb.isKinematic = true;
                    isGround = true;
                    // タイマーの時間を０に戻す。
                    timer = 0.0f;
                }
            }
            if (Physics.Raycast(ray4, out hit, rayDistance))
            {
                if (hit.collider.name == "Wall")
                {
                    rb.isKinematic = true;
                    isGround = true;
                    // タイマーの時間を０に戻す。
                    timer = 0.0f;
                }
            }
        }
        
    
        //クリック跳ねる
        if (Input.GetMouseButtonDown(0) && isGround && time > timeBetweenjump)
        {
            rb.isKinematic = false;
           
            //ifでカメラの向きを判定
            if (MainCamera.GetComponent<cameramove>().xz == 0f)
            {
                
                objPosition = Input.mousePosition;
                // タイマーの時間を０に戻す。
                time = 0.0f;
                rb.AddForce(new Vector3((objPosition.x - 959)/ speed, objPosition.y / upForce, 0));
            }
            if (MainCamera.GetComponent<cameramove>().xz == 90f)
            {
               
                objPosition = Input.mousePosition;
                // タイマーの時間を０に戻す。
                time = 0.0f;
                rb.AddForce(new Vector3(0, objPosition.y / upForce, -(objPosition.x - 959) / speed));
            }
            if (MainCamera.GetComponent<cameramove>().xz == 180f)
            {
               
                objPosition = Input.mousePosition;
                // タイマーの時間を０に戻す。
                time = 0.0f;
                rb.AddForce(new Vector3( -(objPosition.x - 959) / speed, objPosition.y / upForce, 0));
            }
            if (MainCamera.GetComponent<cameramove>().xz == 270f)
            {
              
                objPosition = Input.mousePosition;
                // タイマーの時間を０に戻す。
                time = 0.0f;
                rb.AddForce(new Vector3(0, objPosition.y / upForce, (objPosition.x - 959) / speed));
            }
         
        }
        
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Ground")
        {
            isGround = true;
           
            bound1 = true;
            script.dohankei();
            
            audioSource.PlayOneShot(sound1);
        }

       

    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Ground")
        {
            isGround = false;
        }
        if (collision.gameObject.name == "Wall")
        {
            isGround = false;
        }
    }

    private IEnumerator Henkei()
    {
        //Debug.Log(Cube.GetComponent<CubeJump>().bound);
        this.transform.localScale = new Vector3(1.3f, 0.7f, 1.3f);

        yield return new WaitForSeconds(0.05f);
        this.transform.localScale = new Vector3(1, 1, 1);

    }

   
}
