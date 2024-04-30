using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchCircle : MonoBehaviour
{
    private RockonCamera PlayerFollow;
    // Start is called before the first frame update
    void Start()
    {
        PlayerFollow = GameObject.Find("Rockon Camera").GetComponent<RockonCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            PlayerFollow.SetRockonTarget(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            PlayerFollow.SetRockonTarget(null);
        }
    }
}
