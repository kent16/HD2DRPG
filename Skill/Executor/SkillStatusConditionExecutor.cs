using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class SkillStatusConditionExecutor : AbstractSkillExecutor
{
    /// <summary>
    /// スキルを実行する
    /// </summary>
    /// <param name="myself">スキルを使用する側のキャラクター</param>
    /// <param name="targets">スキルを使用される側のキャラクター</param>
    public override async void Execute(AbstractCharacterController myself, List<AbstractCharacterController> targets)
    {
        for(int i = 0; i < Setting.Count; i++)
        {
            // スキルエフェクトの再生
            PlaySkillEffect(targets, Setting.Effect);
            // スキル実行遅延
            await UniTask.Delay(TimeSpan.FromSeconds(Setting.SkillExecuteDelay), cancellationToken: this.GetCancellationTokenOnDestroy());
            
            // 状態異常を付与する
            targets.ForEach(target => target.ApplyStatusCondition(Setting.StatusConditionType, Setting.ContinueTurn));
        }
    }
}
