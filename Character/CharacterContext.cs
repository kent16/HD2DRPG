using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterContext
{
    public int Level{get; set;}
    public int NextLevelExp{get; set;}
    public int Hp{get; set;}
    public int Mp{get; set;}
    public int Atk{get; set;}
    public int Def{get; set;}
    public int Spd{get; set;}
    public SkillBuffType BuffType{get; set;}
    public int BuffTurn{get; set;}
    public SkillStatusConditionType StatusConditionType{get; set;}
    public int StatusConditionTurn{get; set;}
    public int FormationNo{get; set;}
    public bool IsActionable{get; set;}
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
