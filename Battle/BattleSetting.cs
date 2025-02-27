using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "BattleSetting", menuName="CreateBattleSetting")]
public class BattleSetting : ScriptableObject
{
    [Header("Common")]
    // バトル名
    [SerializeField] private string battleName;
    // バトル説明
    [SerializeField] private string battleDiscription;

    [Header("Background")]
    // 背景
    [SerializeField] private GameObject background;

    [Header("Character")]
    // 味方DB
    [SerializeField] private CharacterDB allyDB;
    // 敵DB
    [SerializeField] private List<CharacterDB> enemyDBList;
    // キャラクターレンダリング設定
    [SerializeField] private CharacterRenderingSetting characterRenderingSetting;

    [Header("Position")]
    // 味方座標
    [SerializeField] private List<Vector3> allyPositions;
    // 敵座標
    [SerializeField] private List<Vector3> enemyPositions;

    [Header("Offset")]
    // 味方初期位置オフセット
    [SerializeField] private Vector3 allyInitPositionOffset;
    // 味方最終位置オフセット
    [SerializeField] private Vector3 allyFinalPositionOffset;

    [Header("Duration")]
    // ウェーブ開始演出所要時間
    [SerializeField] private float waveStartingDuration;
    // ウェーブ終了演出所要時間
    [SerializeField] private float waveEndingDuration;
    // ターン開始演出所要時間
    [SerializeField] private float turnStartingDuration;
    
    public string BattleName{get{return BattleName;}}
    public string BattleDiscription{get{return BattleDiscription;}}
    public GameObject Background{get{return background;}}
    public CharacterDB AllyDB{get{return allyDB;}}
    public List<CharacterDB> EnemyDBList{get{return enemyDBList;}}
    public CharacterRenderingSetting CharacterRenderingSetting{get{return characterRenderingSetting;}}
    public List<Vector3> AllyPositions{get{return allyPositions;}}
    public List<Vector3> EnemyPositions{get{return enemyPositions;}}
    public Vector3 AllyInitPositionOffset{get{return allyInitPositionOffset;}}
    public Vector3 AllyFinalPositionOffset{get{return allyFinalPositionOffset;}}
    public float WaveStartingDuration{get{return waveStartingDuration;}}
    public float WaveEndingDuration{get{return waveEndingDuration;}}
    public float TurnStartingDuration{get{return turnStartingDuration;}}
}
