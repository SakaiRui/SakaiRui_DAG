using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHandMatcher : MonoBehaviour
{
    private AvatarIKGoal ikGoal = AvatarIKGoal.LeftHand; // IK������s���̂̕����i����͍���j
    [SerializeField] private Transform leftHandPoint;
    private Animator anim;
    public bool isEnableIK = true; // IK����L�����t���O
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnAnimatorIK()
    { // OnAnimatorIK��IK��������X�V����ۂɌĂ΂��R�[���o�b�N
        if (!isEnableIK)
            return;

        // ����ɍ쐬����LeftHandPoint�ɁA������ړ�������
        anim.SetIKPositionWeight(ikGoal, 1f);
        anim.SetIKRotationWeight(ikGoal, 1f);
        anim.SetIKPosition(ikGoal, leftHandPoint.position);
        anim.SetIKRotation(ikGoal, leftHandPoint.rotation);
    }

    public void SwitchIK(bool a)
    {
        isEnableIK = a;
    }
}
