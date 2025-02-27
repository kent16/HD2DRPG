using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public abstract class AbstractSkillExecutor : MonoBehaviour
{
    // 設定
    [SerializeField] private SkillSetting setting;
    public SkillSetting Setting{get{return setting;}}

    // Start is called before the first frame update
    public void Start()
    {

    }

    // Update is called once per frame
    public void Update()
    {

    }

    /// <summary>
    /// スキルを呼び出す
    /// </summary>
    /// <param name="myself">スキルを使用する側のキャラクター</param>
    /// <param name="targets">スキルを使用される側のキャラクター</param>
    public async void Invoke(AbstractCharacterController myself, List<AbstractCharacterController> targets)
    {
        // MPを消費
        myself.UseMP(Setting.Mp);

        // スキル名を表示
        BattleUIManager.Instance.DisplayInfoUI(Setting.SkillName);
        // キャラクターアニメーションを再生
        PlayCharacterAnimation(myself);

        // スキル開始遅延を待機
        await UniTask.Delay(TimeSpan.FromSeconds(Setting.SkillStartDelay), cancellationToken: this.GetCancellationTokenOnDestroy());

        // スキルを実行
        Execute(myself, targets);

        // スキル完了を待機
        float completeDelay = Setting.SkillStartIterationDelay * (Setting.Count - 1) + 
                              Setting.SkillExecuteDelay * Setting.Count + 
                              Setting.SkillCompletedDelay; 
        await UniTask.Delay(TimeSpan.FromSeconds(completeDelay), cancellationToken: this.GetCancellationTokenOnDestroy());

        // 終了時処理
        BattleManager.Instance.EndAction();
    }

    /// <summary>
    /// スキルを実行する
    /// </summary>
    /// <param name="myself">スキルを使用する側のキャラクター</param>
    /// <param name="targets">スキルを使用される側のキャラクター</param>
    public abstract void Execute(AbstractCharacterController myself, List<AbstractCharacterController> targets);

    /// <summary>
    /// スキルを使用するときのキャラクターアニメーションを再生する
    /// </summary>
    /// <param name="myself">スキルを使用する側のキャラクター</param>
    public void PlayCharacterAnimation(AbstractCharacterController myself)
    {
        switch(Setting.SkillType)
        {
            case SkillType.Atack:
                myself.PlayAnimation(CharacterAnimationType.Atack);
                break;
            case SkillType.Heal:
            case SkillType.Buff:
            case SkillType.StatusCondition:
                myself.PlayAnimation(CharacterAnimationType.Action);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// スキルのエフェクトを再生する
    /// </summary>
    /// <param name="targets">スキルを使用される側のキャラクター</param>
    /// <param name="effect">スキルエフェクト</param>
    public void PlaySkillEffect(List<AbstractCharacterController> targets, SkillEffectController effect)
    {
        if(effect == null) return;
        if(targets.Count == 0) return;

        // スキルを再生する座標を取得
        Vector3 effectPosition = Vector3.zero;
        foreach(AbstractCharacterController target in targets)
        {
            if(target != null)
            {
                effectPosition += target.transform.position;
            }
        }
        effectPosition = effectPosition / targets.Count + effect.transform.position;

        // 再生
        Instantiate(effect, effectPosition, effect.transform.rotation);
    }

    /// <summary>
    /// ダメージを計算する
    /// </summary>
    /// <param name="atackType">攻撃タイプ</param>
    /// <param name="atk">スキルを使用する側の攻撃力</param>
    /// <param name="def">スキルを使用される側の防御力</param>
    /// <param name="ratio">スキル倍率</param>
    public int CalculateDamage(SkillAtackType atackType, int atk, int def, int ratio)
    {
        int damage = 0;
        switch(atackType)
        {
            case SkillAtackType.Physical:
                damage = (int)MathF.Abs(atk - def / 2) * ratio / 100;
                break;
            case SkillAtackType.Magical:
                damage = atk * ratio / 100;
                break;
            default:
                break;
        }
        return damage;
    }

    /// <summary>
    /// 回復値を計算する
    /// </summary>
    /// <param name="def">スキルを使用する側の防御力</param>
    /// <param name="ratio">スキル倍率</param>
    public int CalculateHealPoint(int def, int ratio)
    {
        return def * ratio / 100;
    }
}
