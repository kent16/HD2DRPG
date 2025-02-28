
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Constants
{
    public static class Scene
    {
        public const string TITLE = "TitleScene";
        public const string FIELD = "FieldScene";
        public const string EVENT = "EventScene";
        public const string BATTLE = "BattleScene";
        
    }
    public static class Tag
    {
        
    }
    
    public static class Rotation
    {
        // バトル初期クォータニオン
        public static readonly Quaternion BATTLE_INIT = Quaternion.Euler(20.0f, 0.0f, 0.0f);
        // フィールド初期クォータニオン
        public static readonly Quaternion FIELD_INIT = Quaternion.Euler(30.0f, 0.0f, 0.0f);
    }

    public static class Duration
    {
        public const float WAVE_START_ALLY_MOVING = 1.0f;
        public const float WAVE_END_ALLY_MOVING = 1.5f;
        public const float BATTLE_START_UI_DISPLAYING = 0.5f;
        public const float BATTLE_START = 1.0f;
        public const float ACTION = 1.0f;
        public const float STATUS_CONDITION = 1.5f;
        public const float UI_DISPLAYING = 0.2f;
    }
    public static class Number
    {
        public const int PARTY_MEMBER_NUM = 4;
        public const int PARTY_COL_ROW_MEMBER_NUM = 2;
        public const int WAVE_NUM = 3;
    }
    public static class Default
    {
        public const int INT_NULL = -1;
        public const int LEVEL = 1;
        public const int NEXT_EXP = 100;
    }
    public static class Emission
    {
        public static readonly Color NORMAL = new Color32(0, 0, 0, 0);
        public static readonly Color HIGHLIGHT = new Color32(100, 100, 100, 255);
    }

}
