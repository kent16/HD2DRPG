using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterContext
{
    // レベル
    public int Level{get; set;}
    // 次のレベルまでの経験値
    public int NextLevelExp{get; set;}
    // HP
    public int Hp{get; set;}
    // MP
    public int Mp{get; set;}
    // 攻撃力
    public int Atk{get; set;}
    // 防御力
    public int Def{get; set;}
    // 行動速度
    public int Spd{get; set;}
    // 適用されているバフ
    public SkillBuffType BuffType{get; set;}
    // バフの残り継続ターン数
    public int BuffTurn{get; set;}
    // 適用されている状態異常
    public SkillStatusConditionType StatusConditionType{get; set;}
    // 状態異常の残り継続ターン数
    public int StatusConditionTurn{get; set;}
    // 陣形番号（0~3）
    public int FormationNo{get; set;}
    // 行動可能か
    public bool IsActionable{get; set;}
    // レベルアップしたか
    public bool IsLevelUpped{get; set;}

    public CharacterContext(CharacterSetting setting, int formationNo, int level = 0, int nextLevelExp = 0)
    {
        Level = level;
        NextLevelExp = nextLevelExp;
        Hp = setting.Hp;
        Mp = setting.Mp;
        Atk = setting.Atk;
        Def = setting.Def;
        Spd = setting.Spd;
        BuffType = SkillBuffType.None;
        BuffTurn = 0;
        StatusConditionType = SkillStatusConditionType.None;
        StatusConditionTurn = 0;
        FormationNo = formationNo;
        IsActionable = true;
        IsLevelUpped = false;
    }
}
