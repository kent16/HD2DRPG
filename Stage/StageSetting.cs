using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "StageSetting", menuName="CreateStageSetting")]
public class StageSetting : ScriptableObject
{
    [Header("Stage")]
    // ステージ名
    [SerializeField] private string stageName;
    // ステージ説明
    [SerializeField] private string stageDiscription;
    // フィールド上のステージの座標
    [SerializeField] private Vector3 stagePosition;
    // 遭遇する敵のアイコン
    [SerializeField] private List<Sprite> stageEnemyIcons;

    [Header("BattleSetting")]
    // 通常戦闘設定
    [SerializeField] private BattleSetting battleSetting;
    // ボス戦設定
    [SerializeField] private BattleSetting bossBattleSetting;

    [Header("EventSetting")]
    // 戦闘前イベント設定
    [SerializeField] private EventSetting preEventSetting;
    // ボス戦前イベント設定
    [SerializeField] private EventSetting bossEventSetting;
    // 戦闘後イベント設定
    [SerializeField] private EventSetting postEventSetting;
    
    public string StageName{get{return stageName;}}
    public string StageDiscription{get{return stageDiscription;}}
    public Vector3 StagePosition{get{return stagePosition;}}
    public List<Sprite> StageEnemyIcons{get{return stageEnemyIcons;}}
    public BattleSetting BattleSetting{get{return battleSetting;}}
    public BattleSetting BossBattleSetting{get{return bossBattleSetting;}}
    public EventSetting PreEventSetting{get{return preEventSetting;}}
    public EventSetting BossEventSetting{get{return bossEventSetting;}}
    public EventSetting PostEventSetting{get{return postEventSetting;}}
}
