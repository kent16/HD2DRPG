using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class EffectEventExecutor : AbstractEventExecutor
{
    // 再生するエフェクト
    [SerializeField] private GameObject targetEffect;
    // 再生時間
    [SerializeField] private float playingDuration;

    /// <summary>
    /// 実行する
    /// </summary>
    public override async void Execute()
    {
        Instantiate(targetEffect, targetEffect.transform.position, targetEffect.transform.rotation);

        await UniTask.Delay(TimeSpan.FromSeconds(playingDuration), cancellationToken: cts.Token);
        
        Complete();
    }
}
