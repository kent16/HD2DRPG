using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class AnimationEventExecutor : AbstractEventExecutor
{
    // イベント対象キャラクター番号
    [SerializeField] private int targetCharacterNo;
    // 再生するアニメーションタイプ
    [SerializeField] private CharacterAnimationType targetAnimationType;
    // 再生時間
    [SerializeField] private float playingDuration;

    /// <summary>
    /// 実行する
    /// </summary>
    public override async void Execute()
    {
        AbstractCharacterController target = EventManager.Instance.GetCharacter(targetCharacterNo);

        target.PlayAnimation(targetAnimationType);

        await UniTask.Delay(TimeSpan.FromSeconds(playingDuration), cancellationToken: cts.Token);
        
        Complete();
    }
}
