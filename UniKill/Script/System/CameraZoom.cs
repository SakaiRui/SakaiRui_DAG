using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class CameraZoom : MonoBehaviour
{
    private Vector3 targetposition;
    private Camera camerazoom;

    private bool IsDo = false;
    // Start is called before the first frame update
    void Start()
    {
        camerazoom = gameObject.GetComponent<Camera>();
        Debug.Log(camerazoom);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void Camerazoom(Vector3 target)
    {
        if(!IsDo)
        {
            DOTween.To(
       () => camerazoom.orthographicSize,          // 何を対象にするのか
       num => camerazoom.orthographicSize = num,   // 値の更新
       3.0f,                  // 最終的な値
       1.0f                  // アニメーション時間
        );
            //Vector3 position = transform.position; // ローカル変数に格納
            //target.z = transform.position.z; // ローカル変数に格納した値を上書き
            targetposition = target; // ローカル変数を代入
            gameObject.transform.DOMove(targetposition, 5.0f).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                IsDo = true;
                SceneManager.LoadScene("Title");
            });
            
        }
       
        
       
    }
}
