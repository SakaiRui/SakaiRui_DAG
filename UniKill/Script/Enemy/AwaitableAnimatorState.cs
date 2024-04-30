using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AwaitableAnimatorState : MonoBehaviour
{
    private void Start()
    {
        _animator = GetComponent<Animator>();
        AnimationStateLoop().Forget();
    }

    private Animator _animator;
    private const string StateDefault = "default";
    public string State = StateDefault;
    // state‚ğØ‚è‘Ö‚¦‚éŠÔŠu.‚±‚ê‚ª’Z‚¢‚Ù‚Ç‘f‘‚­, ’·‚¢‚Ù‚ÇŠÉ‚â‚©‚ÉØ‚è‘Ö‚í‚é.
    public const float DurationTimeSecond = 0.4f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            State = "0";
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            State = "1";
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            State = "2";
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            State = "3";
        }
    }
    private async UniTaskVoid AnimationStateLoop()
    {
        var token = this.GetCancellationTokenOnDestroy();
        var hashDefault = Animator.StringToHash(StateDefault);

        while (true)
        {
            // StateXV‚Ì‚½‚ßUpdate•ª‚¾‚¯‘Ò‚Â
            await UniTask.Yield();
            if (token.IsCancellationRequested)
            {
                break;
            }

            var hashExpect = Animator.StringToHash(State);
            var currentState = _animator.GetCurrentAnimatorStateInfo(0);
            if (currentState.shortNameHash != hashExpect)
            {
                // DurationTimeSecond‚ÌŠÔŠu‚ğ‹²‚ñ‚ÅAnimator‚ÌState‚ğØ‚è‘Ö‚¦‚é
                _animator.CrossFadeInFixedTime(hashExpect, DurationTimeSecond);
                // Ø‚è‘Ö‚¦‚Ä‚¢‚éŠÔ‚ÌcurrentState‚ÍØ‚è‘Ö‚¦‚é‘O‚ÌState‚ªo‚Ä‚­‚é.
                // ‚»‚Ì‚½‚ßDurationTimeSecond‚ª‰ß‚¬‚é‚Ü‚Å‘Ò‚Â
                await UniTask.Delay(TimeSpan.FromSeconds(DurationTimeSecond), cancellationToken: token);
                continue;
            }

            // state‚ªI—¹‚µ‚Ä‚¢‚½ê‡‚Ídefault‚É–ß‚·
            if (currentState.shortNameHash != hashDefault && currentState.normalizedTime >= 1f)
            {
                SetState(StateDefault);
            }
        }
    }

    public void SetState(string nextState)
    {
        if (_animator.HasState(0, Animator.StringToHash(nextState)))
        {
            // ‘¶İ‚·‚éState‚¾‚¯ó‚¯“ü‚ê‚é
            State = nextState;
        }
    }
}

