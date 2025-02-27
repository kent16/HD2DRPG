
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
    // public static class Position
    // {
    //     // 味方初期配置座標
    //     public static readonly List<Vector3> INIT_ALLIES = new List<Vector3>{new Vector3(-4.3f, 0.0f, -1.0f),
    //                                                                          new Vector3(-3.8f, 0.0f, -0.3f),
    //                                                                          new Vector3(-5.5f, 0.0f, -0.7f),
    //                                                                          new Vector3(-5.0f, 0.0f, -0.1f)};
    //     // 味方配置座標
    //     public static readonly List<Vector3> ALLIES = new List<Vector3>{new Vector3(-1.0f, 0.0f, -0.5f),
    //                                                                     new Vector3(-0.6f, 0.0f,  0.5f),
    //                                                                     new Vector3(-2.2f, 0.0f, -0.5f),
    //                                                                     new Vector3(-2.0f, 0.0f,  0.5f)};
    //     // 敵配置座標
    //     public static readonly List<Vector3> ENEMIES = new List<Vector3>{new Vector3(1.0f, 0.0f, -0.5f),
    //                                                                      new Vector3(0.6f, 0.0f,  0.5f),
    //                                                                      new Vector3(2.2f, 0.0f, -0.5f),
    //                                                                      new Vector3(2.0f, 0.0f,  0.5f)};
    // }
    public static class UIPosition
    {
        // // 味方ステータス表示座標
        // public static readonly List<Vector3> ALLY_STATUSES = new List<Vector3>{new Vector3(-290, 70, 0),
        //                                                                        new Vector3(-150, 150, 0),
        //                                                                        new Vector3(-570, 70, 0),
        //                                                                        new Vector3(-420, 150, 0)};
        // // 敵ステータス表示座標
        // public static readonly List<Vector3> ENEMY_STATUSES = new List<Vector3>{new Vector3(300, -190, 0),
        //                                                                         new Vector3(150, -55, 0),
        //                                                                         new Vector3(570, -160, 0),
        //                                                                         new Vector3(420, -45, 0)};
        // // スキルメニュー表示座標
        // public static readonly List<Vector3> SKILL_MENUS = new List<Vector3>{new Vector3(-150, -100, 0),
        //                                                                      new Vector3(-20, -20, 0),
        //                                                                      new Vector3(-430, -100, 0),
        //                                                                      new Vector3(-280, -20, 0)};
        // バトル進捗表示座標
        public static readonly Vector3 BATTLE_PROGRESS = new Vector3(0, 150, 0);
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
