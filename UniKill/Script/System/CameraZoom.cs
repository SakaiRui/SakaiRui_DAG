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
       () => camerazoom.orthographicSize,          // ����Ώۂɂ���̂�
       num => camerazoom.orthographicSize = num,   // �l�̍X�V
       3.0f,                  // �ŏI�I�Ȓl
       1.0f                  // �A�j���[�V��������
        );
            //Vector3 position = transform.position; // ���[�J���ϐ��Ɋi�[
            //target.z = transform.position.z; // ���[�J���ϐ��Ɋi�[�����l���㏑��
            targetposition = target; // ���[�J���ϐ�����
            gameObject.transform.DOMove(targetposition, 5.0f).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                IsDo = true;
                SceneManager.LoadScene("Title");
            });
            
        }
       
        
       
    }
}
