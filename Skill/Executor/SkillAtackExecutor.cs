using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class SkillAtackExecutor : AbstractSkillExecutor
{
    /// <summary>
    /// スキルを実行する
    /// </summary>
    /// <param name="myself">スキルを使用する側のキャラクター</param>
    /// <param name="targets">スキルを使用される側のキャラクター</param>
    public override async void Execute(AbstractCharacterController myself, List<AbstractCharacterController> targets)
    {
        // 攻撃力取得
        int atk = myself.Context.Atk;

        if(Setting.IsRandom)
        {
            // ランダムな敵に攻撃
            for(int i = 0; i < Setting.Count; i++)
            {
                // スキル開始遅延（2回目以降）
                if(i > 0)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(Setting.SkillStartIterationDelay), cancellationToken: this.GetCancellationTokenOnDestroy());
                }

                // 攻撃対象取得
                AbstractCharacterController target = targets.OrderBy(a => Guid.NewGuid()).First();

                // スキルエフェクトの再生
                PlaySkillEffect(new List<AbstractCharacterController>(){target}, Setting.Effect);
                // スキル実行遅延
                await UniTask.Delay(TimeSpan.FromSeconds(Setting.SkillExecuteDelay), cancellationToken: this.GetCancellationTokenOnDestroy());

                // ダメージを与える
                int def = target.Context.Def;
                int damage = CalculateDamage(Setting.AtackType, atk, def, Setting.Ratio);
                target.Damage(damage);
            }
        }
        else
        {
            // 固定の敵に攻撃
            for(int i = 0; i < Setting.Count; i++)
            {
                // スキル開始遅延（2回目以降）
                if(i > 0)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(Setting.SkillStartIterationDelay), cancellationToken: this.GetCancellationTokenOnDestroy());
                }

                // スキルエフェクトの再生
                PlaySkillEffect(targets, Setting.Effect);
                // スキル実行遅延
                await UniTask.Delay(TimeSpan.FromSeconds(Setting.SkillExecuteDelay), cancellationToken: this.GetCancellationTokenOnDestroy());
                
                // ダメージを与える
                foreach(AbstractCharacterController target in targets)
                {
                    int def = target.Context.Def;
                    int damage = CalculateDamage(Setting.AtackType, atk, def, Setting.Ratio);
                    target.Damage(damage);
                }
            }
        }
    }
}
