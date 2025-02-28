using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "CharacterSetting", menuName="CreateCharacterSetting")]
public class CharacterSetting : ScriptableObject
{
    // キャラクター名
    [SerializeField] private string characterName;
    // キャラクタータイプ
    [SerializeField] private CharacterType type;
    // キャラクターアイコン
    [SerializeField] private Sprite icon;
    // HP
    [SerializeField] private int hp;
    // MP
    [SerializeField] private int mp;
    // 攻撃力
    [SerializeField] private int atk;
    // 防御力
    [SerializeField] private int def;
    // 行動速度
    [SerializeField] private int spd;
    // 獲得経験値
    [SerializeField] private int exp;
    // 使用スキル
    [SerializeField] private SkillDB skillDB;
    
    public string CharacterName{get{return characterName;}}
    public CharacterType Type{get{return type;}}
    public Sprite Icon{get{return icon;}}
    public int Hp{get{return hp;}}
    public int Mp{get{return mp;}}
    public int Atk{get{return atk;}}
    public int Def{get{return def;}}
    public int Spd{get{return spd;}}
    public int Exp{get{return exp;}}
    public SkillDB SkillDB{get{return skillDB;}}
}
