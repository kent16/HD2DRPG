using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using EasyTransition;

public class BattleUIManager : MonoBehaviour
{
    [Header("Hierarchy")]
    // インフォUIヒエラルキー
    [SerializeField] private GameObject infoHierarchy;
    // スキルメニューUIヒエラルキー
    [SerializeField] private GameObject skillMenuHierarchy;
    // 味方ステータスUIヒエラルキー
    [SerializeField] private GameObject allySettingHierarchy;
    // 敵ステータスUIヒエラルキー
    [SerializeField] private GameObject enemySettingHierarchy;
    // バトル進行UIヒエラルキー
    [SerializeField] private GameObject battleProgressHierarchy;
    // ダメージUIヒエラルキー
    [SerializeField] private GameObject damageHierarchy;
    // 勝利UIヒエラルキー
    [SerializeField] private GameObject victoryHierarchy;
    // 敗北UIヒエラルキー
    [SerializeField] private GameObject defeatHierarchy;

    [Header("UI")]
    // インフォUI
    [SerializeField] private InfoUIController info;
    // 味方ステータスUI
    [SerializeField] private List<StatusUIController> allyStatuses;
    // 敵ステータスUI
    [SerializeField] private List<StatusUIController> enemyStatuses;
    // スキルUI
    [SerializeField] private List<SkillUIController> skills;
    // ターンキューUI
    [SerializeField] private List<QueueUIController> turnQueues;
    // ダメージUI
    [SerializeField] private DamageUIController damage;
    // 勝利UI
    [SerializeField] private VictoryUIController victoryUi;
    // 敗北UI
    [SerializeField] private DefeatUIController defeatUi;

    [Header("Transition")]
    // トランジション設定
    [SerializeField] private TransitionSettings transitionSettings;
    // トランジション遅延
    [SerializeField] private float transitionDelay;

    // BattleUIManagerインスタンス
    public static BattleUIManager Instance{get; set;}

    void Awake()
    {
        Instance = this;
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
    /// 勝利時のUIを制御する
    /// </summary>
    /// <param name="exp">獲得経験値</param>
    /// <param name="allyParty">味方パーティ</param>
    public void ShowVictoryUI(int exp, List<AbstractCharacterController> allyParty)
    {
        // ステータスUIを非表示
        HideDisplayAllStatusUI();
        // ターンキューUIを非表示
        HideTurnQueueUI();
        // 勝利UIを表示
        Instantiate(victoryUi, victoryHierarchy.transform).SetVictoryInfo(exp, allyParty);
    }

    /// <summary>
    /// 敗北時のUIを制御する
    /// </summary>
    public void ShowDefeatUI()
    {
        // 敗北UIを表示
        Instantiate(defeatUi, defeatHierarchy.transform);
    }

    /// <summary>
    /// ウェーブ開始時のUIを制御する
    /// </summary>
    /// <param name="wave">ウェーブ数</param>
    /// <param name="allyParty">味方パーティ</param>
    /// <param name="enemyParty">敵パーティ</param>
    public async void ShowWaveStartingUI(int wave, List<AbstractCharacterController> allyParty, List<AbstractCharacterController> enemyParty)
    {
        // 味方の移動が完了するまで待機
        await UniTask.Delay(TimeSpan.FromSeconds(Constants.Duration.WAVE_START_ALLY_MOVING), cancellationToken: this.GetCancellationTokenOnDestroy());

        // ステータスUIを表示
        DisplayAllStatusUI(allyParty, enemyParty);
        // ウェーブ数を表示
        DisplayInfoUI("WAVE" + wave);

    }

    /// <summary>
    /// ウェーブ終了時のUIを制御する
    /// </summary>
    public void ShowWaveEndingUI()
    {
        // ステータスUIを非表示
        HideDisplayAllStatusUI();
        // ターンキューUIを非表示
        HideTurnQueueUI();
        // トランジション
        GameManager.Instance.Transit();
    }

    /// <summary>
    /// ターン開始時のUIを制御する
    /// </summary>
    /// <param name="turn">ターン数</param>
    public void ShowTurnStartingUI(int turn)
    {
        // ターン数を表示
        DisplayInfoUI("TURN" + turn);
        // ターンキューUIを表示
        DisplayTurnQueueUI();
    }

    /// <summary>
    /// ターン終了時のUIを制御する
    /// </summary>
    public void ShowTurnEndingUI()
    {
        // ターンキューUIを非表示
        HideTurnQueueUI();
    }

    /// <summary>
    /// 行動開始時のUIを制御する
    /// </summary>
    /// <param name="turnQueue">行動順キュー</param>
    public void ShowActionStartingUI(List<AbstractCharacterController> turnQueue)
    {
        // ターンキューUIを設定
        SetTurnQueueUI(turnQueue);
    }

    /// <summary>
    /// 行動終了時のUIを制御する
    /// </summary>
    public void ShowActionEndingUI()
    {
        // // ターンキューUIを設定
        // SetTurnQueueUI(new List<GameObject>());
    }

    /// <summary>
    /// インフォUIを表示する
    /// </summary>
    /// <param name="text">表示するテキスト</param>
    public void DisplayInfoUI(string text)
    {
        Instantiate(info, infoHierarchy.transform).SetText(text);
    }

    /// <summary>
    /// すべてのステータスUIを表示する
    /// </summary>
    /// <param name="allyParty">味方パーティ</param>
    /// <param name="enemyParty">敵パーティ</param>
    public void DisplayAllStatusUI(List<AbstractCharacterController> allyParty, List<AbstractCharacterController> enemyParty)
    {
        // 味方ステータスUIを表示
        for(int formationNo = 0; formationNo < 4; formationNo++)
        {
            if(allyParty[formationNo] != null)
                allyStatuses[formationNo].Display();
            if(enemyParty[formationNo] != null)
                enemyStatuses[formationNo].Display();
        }
    }

    /// <summary>
    /// すべてのステータスUIを非表示にする
    /// </summary>
    public void HideDisplayAllStatusUI()
    {
        allyStatuses.ForEach(status => status.Hide());
        enemyStatuses.ForEach(status => status.Hide());
    }

    /// <summary>
    /// 特定のステータスUIを非表示にする
    /// </summary>
    /// <param name="type">非表示にする対象のキャラクタータイプ</param>
    /// <param name="formationNo">非表示にする対象の陣形番号</param>
    public void HideStatusUI(CharacterType type, int formationNo)
    {
        switch(type)
        {
            case CharacterType.Ally:
                allyStatuses[formationNo].Hide();
                break;
            case CharacterType.Enemy:
                enemyStatuses[formationNo].Hide();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// スキルUIを表示する
    /// </summary>
    /// <param name="formationNo">陣形番号</param>
    public void DisplaySkillUI(int formationNo)
    {
        skills[formationNo].Display();
    }

    /// <summary>
    /// スキルUIを非表示にする
    /// </summary>
    /// <param name="formationNo">陣形番号</param>
    public void HideSkillUI(int formationNo)
    {
        skills[formationNo].Hide();
    }

    /// <summary>
    /// ステータスUIに値を設定する
    /// </summary>
    /// <param name="setting">キャラクター設定</param>
    /// <param name="context">キャラクターコンテクスト</param>
    public void SetStatusUI(CharacterSetting setting, CharacterContext context)
    {
        switch(setting.Type)
        {
            case CharacterType.Ally:
                allyStatuses[context.FormationNo].SetAllyStatus(setting, context);
                break;
            case CharacterType.Enemy:
                enemyStatuses[context.FormationNo].SetEnemyStatus(setting, context);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 行動順キューUIを表示する
    /// </summary>
    public void DisplayTurnQueueUI()
    {
        InitTurnQueueUI();
        for(int queueNo = 0; queueNo < turnQueues.Count; queueNo++)
        {
            turnQueues[queueNo].Display();
        }
    }

    /// <summary>
    /// 行動順キューUIを非表示にする
    /// </summary>
    public void HideTurnQueueUI()
    {
        InitTurnQueueUI();
        for(int queueNo = 0; queueNo < turnQueues.Count; queueNo++)
        {
            turnQueues[queueNo].Hide();
        }
    }

    /// <summary>
    /// 行動順キューUIに値を設定する
    /// </summary>
    /// <param name="turnQueue">行動順キュー</param>
    public void SetTurnQueueUI(List<AbstractCharacterController> turnQueue)
    {
        for(int queueNo = 0; queueNo < turnQueues.Count; queueNo++)
        {
            Sprite characterIcon = null;
            if(turnQueue.Count > queueNo)
            {
                characterIcon = turnQueue[queueNo].Setting.Icon;
            }
            turnQueues[queueNo].SetIconUI(characterIcon);
        }
    }

    /// <summary>
    /// 行動順キューUIを初期化する
    /// </summary>
    public void InitTurnQueueUI()
    {
        for(int queueNo = 0; queueNo < turnQueues.Count; queueNo++)
        {
            turnQueues[queueNo].InitIconUI();
        }
    }

    /// <summary>
    /// ダメージUIを表示する
    /// </summary>
    /// <param name="type">キャラクタータイプ</param>
    /// <param name="formationNo">陣形番号</param>
    /// <param name="dmg">ダメージ値</param>
    public void DisplayDamageUI(CharacterType type, int formationNo, int dmg)
    {
        Instantiate(damage, damageHierarchy.transform).SetDamage(type, formationNo, dmg);
    }
}
