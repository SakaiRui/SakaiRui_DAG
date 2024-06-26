using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDestroy : MonoBehaviour
{

    //消えるまでの時間
    [Header("消えるまでの時間")]
    [SerializeField] private float DestroyTime = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DestroyTime -= Time.deltaTime;
        if(DestroyTime <= 0.0f)
        {
            Destroy(gameObject);
        }
    }
}
