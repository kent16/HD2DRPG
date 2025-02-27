using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class SkillHealExecutor : AbstractSkillExecutor
{
    /// <summary>
    /// スキルを実行する
    /// </summary>
    /// <param name="myself">スキルを使用する側のキャラクター</param>
    /// <param name="targets">スキルを使用される側のキャラクター</param>
    public override async void Execute(AbstractCharacterController myself, List<AbstractCharacterController> targets)
    {
        // 回復値計算
        int healPoint = CalculateHealPoint(myself.Context.Def, Setting.Ratio);

        for(int i = 0; i < Setting.Count; i++)
        {
            // スキルエフェクトの再生
            PlaySkillEffect(targets, Setting.Effect);
            // スキル実行遅延
            await UniTask.Delay(TimeSpan.FromSeconds(Setting.SkillExecuteDelay), cancellationToken: this.GetCancellationTokenOnDestroy());
            
            // HPを回復する
            targets.ForEach(target => target.Heal(Setting.HealType, healPoint));
        }
    }
}
