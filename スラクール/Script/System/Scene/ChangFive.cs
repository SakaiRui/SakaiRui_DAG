using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangFive : MonoBehaviour
{
    private float step_time;    // �o�ߎ��ԃJ�E���g�p
    public Text timerText;
    public float totalTime;
    int seconds;

    // Use this for initialization
    void Start()
    {
        step_time = 0.0f;       // �o�ߎ��ԏ�����
    }

    // Update is called once per frame
    void Update()
    {

        // �o�ߎ��Ԃ��J�E���g
        step_time += Time.deltaTime;

        // 3�b��ɉ�ʑJ�ځiscene2�ֈړ��j
        if (step_time >= 5.0f)
        {
            SceneManager.LoadScene("SampleScene");
        }
        totalTime -= Time.deltaTime;
        seconds = (int)totalTime;
        timerText.text = seconds.ToString();
    }
}
