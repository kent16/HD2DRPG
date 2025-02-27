using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using EasyTransition;

public class GameManager : MonoBehaviour
{
    [Header("Transition")]
    // トランジション設定
    [SerializeField] private TransitionSettings transitionSettings;
    // トランジション遅延
    [SerializeField] private float transitionDelay;

    // GameManagerインスタンス
    public static GameManager Instance{get; set;}

    // ステージ設定
    private StageSetting stageSetting;
    // ステージフェーズ
    private StagePhase stagePhase;

    void Awake()
    {
        // シングルトンなインスタンスを作成する
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 遷移する
    /// </summary>
    public void Transit()
    {
        TransitionManager.Instance().Transition(transitionSettings, transitionDelay); 
    }

    /// <summary>
    /// シーンを遷移する
    /// </summary>
    /// <param name="sceneName">遷移先のシーン名</param>
    public void TransitScene(string sceneName)
    {
        TransitionManager.Instance().Transition(sceneName, transitionSettings, transitionDelay);
    }

    /// <summary>
    /// ステージを開始する
    /// </summary>
    /// <param name="stageSetting">開始するステージの設定</param>
    public void StartStage(StageSetting stageSetting)
    {
        this.stageSetting = stageSetting;
        if(stageSetting.PreEventSetting != null)
            StartPreEvent();
        else if(stageSetting.BattleSetting != null)
            StartBattle();
        else if(stageSetting.BossEventSetting != null)
            StartBossEvent();
        else if(stageSetting.BossBattleSetting != null)
            StartBossBattle();
        else if(stageSetting.PostEventSetting != null)
            StartPostEvent();
        else
            EndStage();
    }

    /// <summary>
    /// ステージを終了する
    /// </summary>
    public void EndStage()
    {
        stageSetting = null;
        stagePhase = StagePhase.None;
        TransitScene(Constants.Scene.FIELD);
    }

    /// <summary>
    /// 次のフェーズに移る
    /// </summary>
    public void NextStagePhase()
    {
        switch(stagePhase)
        {
            case StagePhase.PreEvent:
                if(stageSetting.BattleSetting != null)
                    StartBattle();
                else if(stageSetting.BossEventSetting != null)
                    StartBossEvent();
                else if(stageSetting.BossBattleSetting != null)
                    StartBossBattle();
                else if(stageSetting.PostEventSetting != null)
                    StartPostEvent();
                else
                    EndStage();
                break;
            case StagePhase.Battle:
                if(stageSetting.BossEventSetting != null)
                    StartBossEvent();
                else if(stageSetting.BossBattleSetting != null)
                    StartBossBattle();
                else if(stageSetting.PostEventSetting != null)
                    StartPostEvent();
                else
                    EndStage();
                break;
            case StagePhase.BossEvent:
                if(stageSetting.BossBattleSetting != null)
                    StartBossBattle();
                else if(stageSetting.PostEventSetting != null)
                    StartPostEvent();
                else
                    EndStage();
                break;
            case StagePhase.BossBattle:
                if(stageSetting.PostEventSetting != null)
                    StartPostEvent();
                else
                    EndStage();
                break;
            default:
                EndStage();
                break;
        }
    }

    /// <summary>
    /// イベント設定を取得する
    /// </summary>
    public EventSetting GetEventSetting()
    {
        switch(stagePhase)
        {
            case StagePhase.PreEvent:
                return stageSetting.PreEventSetting;
            case StagePhase.BossEvent:
                return stageSetting.BossEventSetting;
            case StagePhase.PostEvent:
                return stageSetting.PostEventSetting;
            default:
                return null;
        }
    }
    /// <summary>
    /// バトル設定を取得する
    /// </summary>
    public BattleSetting GetBattleSetting()
    {
        switch(stagePhase)
        {
            case StagePhase.Battle:
                return stageSetting.BattleSetting;
            case StagePhase.BossBattle:
                return stageSetting.BossBattleSetting;
            default:
                return null;
        }
    }

    /// <summary>
    /// 前段イベントを開始する
    /// </summary>
    private void StartPreEvent()
    {
        stagePhase = StagePhase.PreEvent;
        TransitScene(Constants.Scene.EVENT);
    }
    /// <summary>
    /// バトルを開始する
    /// </summary>
    private void StartBattle()
    {
        stagePhase = StagePhase.Battle;
        TransitScene(Constants.Scene.BATTLE);
    }
    /// <summary>
    /// ボスイベントを開始する
    /// </summary>
    private void StartBossEvent()
    {
        stagePhase = StagePhase.BossEvent;
        TransitScene(Constants.Scene.EVENT);
    }
    /// <summary>
    /// ボスバトルを開始する
    /// </summary>
    private void StartBossBattle()
    {
        stagePhase = StagePhase.BossBattle;
        TransitScene(Constants.Scene.BATTLE);
    }
    /// <summary>
    /// 後段イベントを開始する
    /// </summary>
    private void StartPostEvent()
    {
        stagePhase = StagePhase.PostEvent;
        TransitScene(Constants.Scene.EVENT);
    }
}

