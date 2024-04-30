using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Œ©‚½–Ú‚¾‚¯’×‚ê‚é
public class bound : MonoBehaviour
{
    
    private float boundtimer;
    public GameObject Cube;
    public float timebound = 0.35f;
    bool bound1;
    CubeJump cubejump;

    // Start is called before the first frame update
    void Start()
    {

       
    }

    // Update is called once per frame
    void Update()
    {
       
    }
  

    private IEnumerator Henkei()
    {
        //Debug.Log(Cube.GetComponent<CubeJump>().bound);
        this.transform.localScale = new Vector3(1.3f, 0.7f, 1.3f);

        yield return new WaitForSeconds(0.05f);
        this.transform.localScale = new Vector3(1, 1, 1);
       
    }
    public void dohankei()
    {
        StartCoroutine("Henkei");
    }
}
