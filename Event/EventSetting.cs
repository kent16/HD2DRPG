using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "EventSetting", menuName="CreateEventSetting")]
public class EventSetting : ScriptableObject
{
    [Header("Common")]
    // イベント名
    [SerializeField] private string eventName;
    // イベント説明
    [SerializeField] private string eventDiscription;

    [Header("Background")]
    // 背景
    [SerializeField] private GameObject background;

    [Header("Event")]
    // イベントノード
    [SerializeField] private List<GameObject> eventNodes;

    [Header("Character")]
    // キャラクター
    [SerializeField] private List<GameObject> characters;
    // キャラクター初期位置
    [SerializeField] private List<Vector3> initCharacterPositions;
    // キャラクターレンダリング設定
    [SerializeField] private CharacterRenderingSetting characterRenderingSetting;
    
    public string EventName{get{return eventName;}}
    public string EventDiscription{get{return eventDiscription;}}
    public GameObject Background{get{return background;}}
    public List<GameObject> EventNodes{get{return eventNodes;}}
    public List<GameObject> Characters{get{return characters;}}
    public List<Vector3> InitCharacterPositions{get{return initCharacterPositions;}}
    public CharacterRenderingSetting CharacterRenderingSetting{get{return characterRenderingSetting;}}
}
