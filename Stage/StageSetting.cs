using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "StageSetting", menuName="CreateStageSetting")]
public class StageSetting : ScriptableObject
{
    [Header("Stage")]
    [SerializeField] private string stageName;
    [SerializeField] private string stageDiscription;
    [SerializeField] private Vector3 stagePosition;
    [SerializeField] private List<Sprite> stageEnemyIcons;

    [Header("BattleSetting")]
    [SerializeField] private BattleSetting battleSetting;
    [SerializeField] private BattleSetting bossBattleSetting;

    [Header("EventSetting")]
    [SerializeField] private EventSetting preEventSetting;
    [SerializeField] private EventSetting bossEventSetting;
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
