using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsSet : MonoBehaviour
{
    [SerializeField] int Fps = 60;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = Fps;
    }
}
