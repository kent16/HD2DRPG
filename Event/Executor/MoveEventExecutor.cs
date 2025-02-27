using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class MoveEventExecutor : AbstractEventExecutor
{
    // イベント対象キャラクター番号
    [SerializeField] private int targetCharacterNo;
    // 移動先の座標
    [SerializeField] private Vector3 targetPosition;
    // 移動時間
    [SerializeField] private float movingDuration;

    /// <summary>
    /// 実行する
    /// </summary>
    public override async void Execute()
    {
        AbstractCharacterController target = EventManager.Instance.GetCharacter(targetCharacterNo);

        target.Move(targetPosition, movingDuration);

        await UniTask.Delay(TimeSpan.FromSeconds(movingDuration), cancellationToken: cts.Token);
        
        Complete();
    }
}
