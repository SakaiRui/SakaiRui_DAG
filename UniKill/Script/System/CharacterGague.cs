using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class CharacterGague : MonoBehaviour
{
    [SerializeField]
    private Image GreenGauge;
    [SerializeField]
    private Image RedGauge;

    private float life;
    private float maxlife;
    private Tween redGaugeTween;

    public void GaugeReduction(float reducationValue, float nowlife, float time = 1.5f)
    {
        life = nowlife;
        var valueFrom = life / maxlife;
        var valueTo = (life - reducationValue) /maxlife;

        // �΃Q�[�W����
        GreenGauge.fillAmount = valueTo;

        if (redGaugeTween != null)
        {
            redGaugeTween.Kill();
        }

        // �ԃQ�[�W����
        redGaugeTween = DOTween.To(
            () => valueFrom,
            x => {
                RedGauge.fillAmount = x;
            },
            valueTo,
            time
        );
    }

    public void SetMaxLife(float MaxLife)
    {
        maxlife = MaxLife;
        life = MaxLife;
    }
}
