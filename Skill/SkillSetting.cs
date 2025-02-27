using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillSetting : MonoBehaviour
{
    [Header("Common")]
    [SerializeField] private string skillName;
    [SerializeField] private string discription;

    [Header("Type")]
    [SerializeField] private SkillType skillType;
    [SerializeField] private SkillHealType healType;
    [SerializeField] private SkillAtackType atackType;
    [SerializeField] private SkillBuffType buffType;
    [SerializeField] private SkillStatusConditionType statusConditionType;
    [SerializeField] private SkillRangeType targetRangeType;
    [SerializeField] private CharacterType targetCharacterType;

    [Header("Effect")]
    [SerializeField] private SkillEffectController effect;

    [Header("Delay")]
    [SerializeField] private float skillStartDelay;
    [SerializeField] private float skillStartIterationDelay;
    [SerializeField] private float skillExecuteDelay;
    [SerializeField] private float skillCompletedDelay;

    [Header("Detail")]
    [SerializeField] private int ratio;
    [SerializeField] private int count;
    [SerializeField] private int continueTurn;
    [SerializeField] private bool isRandom;

    [Header("DetailForAlly")]
    [SerializeField] private int mp;

    [Header("DetailForEnemy")]
    [SerializeField] private int weight;
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
