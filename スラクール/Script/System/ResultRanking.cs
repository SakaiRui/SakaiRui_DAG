using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�N���A�^�C���ۑ�
public class ResultRanking : MonoBehaviour
{

    Text rankingText;
    int i;
    // Start is called before the first frame update
    void Start()
    {
        rankingText = GetComponent<Text>();

        float[] Rank = new float[5];

        //�����Q�[�����n�߂�O�Ƀf�[�^�����������ꍇ���L�̊֐����g�p�Ńf�[�^��������
        //PlayerPrefs.DeleteAll();

        
        for(int i = 0;i < 5;i++)
        {
            Rank[i] = PlayerPrefs.GetFloat
                ("rank" + (i + 1),(i + 1) * 30);
        }

        

        int newRecord = -1;
        for (i = 0; i < 5; i++)
        {
            if (GameTimer.time < Rank[i])
            {
                newRecord = i;
                break;
            }
        }

        if (newRecord != -1)
        {
            for (int i = 4; i > newRecord; i--)
            {
                Rank[i] = Rank[i - 1];
            }

            Rank[newRecord] = GameTimer.time;
        }


        rankingText.text = "����\n";
        for (i = 0; i < 5; i++)
        {
            rankingText.text += (i + 1) + ":";
            rankingText.text += Rank[i].ToString("n1") + "\n";
        }

        //�X�V���������L���O��ۑ�
       
        for (int i = 0; i < 5; i++)
        {
            PlayerPrefs.SetFloat("rank" + (i + 1), Rank[i]);
        }
        PlayerPrefs.Save();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
