using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    // パーティ
    public List<AllyType> Party{get; set;}
    // レベル
    public int LevelWarrior{get; set;}
    public int LevelArcher{get; set;}
    public int LevelSister{get; set;}
    public int LevelKnight{get; set;}
    public int LevelMagician{get; set;}
    public int LevelThief{get; set;}
    // レベルアップに必要な経験値
    public int NextExpWarrior{get; set;}
    public int NextExpArcher{get; set;}
    public int NextExpSister{get; set;}
    public int NextExpKnight{get; set;}
    public int NextExpMagician{get; set;}
    public int NextExpThief{get; set;}

    public SaveData(bool isDefault = false)
    {
        if(isDefault)
        {
            // パーティ
            Party = new List<AllyType>(){AllyType.Warrior, AllyType.None, AllyType.Archer,  AllyType.None};
            // レベル
            LevelWarrior = Constants.Default.LEVEL;
            LevelArcher = Constants.Default.LEVEL;
            LevelSister = Constants.Default.LEVEL;
            LevelKnight = Constants.Default.LEVEL;
            LevelMagician = Constants.Default.LEVEL;
            LevelThief = Constants.Default.LEVEL;
            // 累計経験値
            NextExpWarrior = Constants.Default.NEXT_EXP;
            NextExpArcher = Constants.Default.NEXT_EXP;
            NextExpSister = Constants.Default.NEXT_EXP;
            NextExpKnight = Constants.Default.NEXT_EXP;
            NextExpMagician = Constants.Default.NEXT_EXP;
            NextExpThief = Constants.Default.NEXT_EXP;
        }
        else
        {
            // パーティ
            Party = null;
            // レベル
            LevelWarrior = Constants.Default.INT_NULL;
            LevelArcher = Constants.Default.INT_NULL;
            LevelSister = Constants.Default.INT_NULL;
            LevelKnight = Constants.Default.INT_NULL;
            LevelMagician = Constants.Default.INT_NULL;
            LevelThief = Constants.Default.INT_NULL;
            // 累計経験値
            NextExpWarrior = Constants.Default.INT_NULL;
            NextExpArcher = Constants.Default.INT_NULL;
            NextExpSister = Constants.Default.INT_NULL;
            NextExpKnight = Constants.Default.INT_NULL;
            NextExpMagician = Constants.Default.INT_NULL;
            NextExpThief = Constants.Default.INT_NULL;
        }
    }
}
