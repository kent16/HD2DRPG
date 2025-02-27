using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class AwaitEventExecutor : AbstractEventExecutor
{
    // 待機時間
    [SerializeField] private float awaitDuration;

    /// <summary>
    /// 実行する
    /// </summary>
    public override async void Execute()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(awaitDuration), cancellationToken: cts.Token);
        
        Complete();
    }
}
