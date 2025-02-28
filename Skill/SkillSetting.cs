using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillSetting : MonoBehaviour
{
    [Header("Common")]
    // スキル名
    [SerializeField] private string skillName;
    // スキル説明
    [SerializeField] private string discription;

    [Header("Type")]
    // スキルタイプ
    [SerializeField] private SkillType skillType;
    // スキル回復タイプ
    [SerializeField] private SkillHealType healType;
    // スキル攻撃タイプ
    [SerializeField] private SkillAtackType atackType;
    // スキルバフタイプ
    [SerializeField] private SkillBuffType buffType;
    // スキル状態異常タイプ
    [SerializeField] private SkillStatusConditionType statusConditionType;
    // スキル有効射程タイプ
    [SerializeField] private SkillRangeType targetRangeType;
    // スキル使用対象キャラクタータイプ
    [SerializeField] private CharacterType targetCharacterType;

    [Header("Effect")]
    // エフェクト
    [SerializeField] private SkillEffectController effect;

    [Header("Delay")]
    // スキルを開始するまでの遅延時間
    // この遅延後にキャラクターのアニメーション再生を開始する
    [SerializeField] private float skillStartDelay;
    // スキルを開始するまでの遅延時間（2回目以降）
    // この遅延は連続攻撃スキルの2回目以降でのみ使用される
    [SerializeField] private float skillStartIterationDelay;
    // スキル開始してから実行するまでの遅延時間
    // この遅延後にダメージ等の処理やエフェクト再生をする
    [SerializeField] private float skillExecuteDelay;
    // スキル開始してから実行するまでの遅延時間
    // この遅延後にキャラクターの行動を終了する
    [SerializeField] private float skillCompletedDelay;

    [Header("Detail")]
    // スキル効果倍率（%）
    [SerializeField] private int ratio;
    // スキル連続使用回数
    [SerializeField] private int count;
    // スキル効果継続ターン数
    [SerializeField] private int continueTurn;
    // ランダムな対象にスキルを使用するか
    [SerializeField] private bool isRandom;

    [Header("DetailForAlly")]
    // 消費MP
    [SerializeField] private int mp;

    [Header("DetailForEnemy")]
    // スキル抽選時の重みづけ（大きいほど抽選される確率が高くなる）
    [SerializeField] private int weight;
    // 固定ターン（指定したターンごとに必ずスキルを使用する）
    [SerializeField] private int fixedTurn;
    
    public string SkillName{get{return skillName;}}
    public string Discription{get{return discription;}}

    public SkillType SkillType{get{return skillType;}}
    public SkillHealType HealType{get{return healType;}}
    public SkillAtackType AtackType{get{return atackType;}}
    public SkillBuffType BuffType{get{return buffType;}}
    public SkillStatusConditionType StatusConditionType{get{return statusConditionType;}}
    public SkillRangeType TargetRangeType{get{return targetRangeType;}}
    public CharacterType TargetCharacterType{get{return targetCharacterType;}}

    public SkillEffectController Effect{get{return effect;}}

    public float SkillStartDelay{get{return skillStartDelay;}}
    public float SkillStartIterationDelay{get{return skillStartIterationDelay;}}
    public float SkillExecuteDelay{get{return skillExecuteDelay;}}
    public float SkillCompletedDelay{get{return skillCompletedDelay;}}

    public int Ratio{get{return ratio;}}
    public int Count{get{return count;}}
    public int ContinueTurn{get{return continueTurn;}}
    public bool IsRandom{get{return isRandom;}}

    public int Mp{get{return mp;}}
    
    public int Weight{get{return weight;}}
    public int FixedTurn{get{return fixedTurn;}}
}
