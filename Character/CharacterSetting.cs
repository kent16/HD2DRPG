using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "CharacterSetting", menuName="CreateCharacterSetting")]
public class CharacterSetting : ScriptableObject
{
    [SerializeField] private string characterName;
    [SerializeField] private CharacterType type;
    [SerializeField] private Sprite icon;
    [SerializeField] private int hp;
    [SerializeField] private int mp;
    [SerializeField] private int atk;
    [SerializeField] private int def;
    [SerializeField] private int spd;
    [SerializeField] private int exp;
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
