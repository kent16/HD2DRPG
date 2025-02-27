using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using EasyTransition;

public class FieldManager : MonoBehaviour
{
    [Header("Hierarchy")]
    // 味方ヒエラルキー
    [SerializeField] private Transform alliesHierarchy;

    [Header("Ally")]
    // 味方キャラクター
    [SerializeField] private GameObject ally;

    [Header("Stage")]
    // ステージ設定DB
    [SerializeField] private StageSettingDB stageSettingDB;

    [Header("Duration")]
    // 移動所要時間
    [SerializeField] private float movingDuration;

    [Header("Transition")]
    // トランジション設定
    [SerializeField] private TransitionSettings transitionSettings;
    // トランジション遅延
    [SerializeField] private float transitionDelay;

    // FieldManagerインスタンス
    public static FieldManager Instance{get; set;}
    // 移動中か
    public bool IsMoving{get; set;}
    // モーダルUIを表示中か
    public bool IsDisplayingModal{get; set;}

    // ワールド番号
    private int worldNo = 1;
    // ステージ番号
    private int stageNo = 1;
    // 生成した味方キャラクター
    private AbstractCharacterController instantiatedAlly;

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // セーブデータ取得

        // 味方を初期化する
        InitAlly();
        // ステージUIを表示する
        FieldUIManager.Instance.DisplayStageUI(stageSettingDB.GetStageSetting(stageNo));
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsMoving && !IsDisplayingModal)
        {
            if(Input.GetKeyDown(KeyCode.Return))
                GameManager.Instance.StartStage(stageSettingDB.GetStageSetting(stageNo));
            if(Input.GetKeyDown(KeyCode.LeftArrow))
                MovePrevStage();
            if(Input.GetKeyDown(KeyCode.RightArrow))
                MoveNextStage();
            if(Input.GetKeyDown(KeyCode.P))
                FieldUIManager.Instance.DisplayPartyUI();
        }
    }

    /// <summary>
    /// 味方を初期化する
    /// </summary>
    private void InitAlly()
    {
        // リーダーの味方キャラクターを取得
        instantiatedAlly = Instantiate(ally, stageSettingDB.GetStageSetting(stageNo).StagePosition, Constants.Rotation.FIELD_INIT, alliesHierarchy).GetComponent<AbstractCharacterController>();
    }

    /// <summary>
    /// 前のステージに移動
    /// </summary>
    private void MovePrevStage()
    {
        if(stageNo > 1)
        {
            stageNo--;
            MoveAlly(true);
        }
    }

    /// <summary>
    /// 次のステージに移動
    /// </summary>
    private void MoveNextStage()
    {
        if(stageSettingDB.GetStageSettings().Count > stageNo)
        {
            stageNo++;
            MoveAlly(false);
        }
    }

    /// <summary>
    /// 味方キャラクターを移動させる
    /// </summary>
    /// <param name="isMovingToNextStage">次のステージに移動中か</param>
    private async void MoveAlly(bool isMovingToNextStage)
    {
        StageSetting stageSetting = stageSettingDB.GetStageSetting(stageNo);
        // キャラクターを移動させる
        instantiatedAlly.Move(stageSetting.StagePosition, movingDuration);
        instantiatedAlly.Flip(isMovingToNextStage);
        IsMoving = true;
        // ステージUIを非表示にする
        FieldUIManager.Instance.HideStageUI();

        // 移動完了まで待機する
        await UniTask.Delay(TimeSpan.FromSeconds(movingDuration), cancellationToken: this.GetCancellationTokenOnDestroy());
        IsMoving = false;
        // ステージUIを表示する
        FieldUIManager.Instance.DisplayStageUI(stageSetting);
    }
}
