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
    // stateを切り替える間隔.これが短いほど素早く, 長いほど緩やかに切り替わる.
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
            // State更新のためUpdate分だけ待つ
            await UniTask.Yield();
            if (token.IsCancellationRequested)
            {
                break;
            }

            var hashExpect = Animator.StringToHash(State);
            var currentState = _animator.GetCurrentAnimatorStateInfo(0);
            if (currentState.shortNameHash != hashExpect)
            {
                // DurationTimeSecondの間隔を挟んでAnimatorのStateを切り替える
                _animator.CrossFadeInFixedTime(hashExpect, DurationTimeSecond);
                // 切り替えている間のcurrentStateは切り替える前のStateが出てくる.
                // そのためDurationTimeSecondが過ぎるまで待つ
                await UniTask.Delay(TimeSpan.FromSeconds(DurationTimeSecond), cancellationToken: token);
                continue;
            }

            // stateが終了していた場合はdefaultに戻す
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
            // 存在するStateだけ受け入れる
            State = nextState;
        }
    }
}

