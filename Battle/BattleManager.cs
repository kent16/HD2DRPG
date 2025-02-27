using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using EasyTransition;

public class BattleManager : MonoBehaviour
{
    [Header("Hierarchy")]
    // 背景ヒエラルキー
    [SerializeField] private Transform backgroundHierarchy;
    // 味方ヒエラルキー
    [SerializeField] private Transform alliesHierarchy;
    // 敵ヒエラルキー
    [SerializeField] private Transform enemiesHierarchy;

    [Header("Transition")]
    // トランジション設定
    [SerializeField] private TransitionSettings transitionSettings;
    // トランジション遅延
    [SerializeField] private float transitionDelay;

    // BattleManagerインスタンス
    public static BattleManager Instance{get; set;}

    // バトル設定
    private BattleSetting setting;
    // ウェーブ数
    private int wave = 0;
    // ターン数
    private int turn = 0;
    // 獲得経験値
    private int exp = 0;
    // 行動中のキャラクター
    private AbstractCharacterController actingCharacter;
    // 現在の味方パーティ
    private List<AbstractCharacterController> currentAllyParty;
    // 現在の敵パーティ
    private List<AbstractCharacterController> currentEnemyParty;
    // このウェーブで死亡した味方
    private List<AbstractCharacterController> deadAllies = new List<AbstractCharacterController>();
    
    public BattleSetting Setting{get{return setting;}}
    public int Wave{get{return wave;}}
    public int Turn{get{return turn;}}
    public List<AbstractCharacterController> CurrentAllyParty{get{return currentAllyParty;}}
    public List<AbstractCharacterController> CurrentEnemyParty{get{return currentEnemyParty;}}

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // イベント設定取得
        setting = GameManager.Instance.GetBattleSetting();
        // 背景初期化
        InitBackground();
        // 味方初期化
        InitAllies();
        // ウェーブ開始
        StartWave();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 勝利時の処理
    /// </summary>
    public void Victory()
    {
        // レベルアップに必要な経験値を更新し、セーブデータに保存
        foreach(AbstractCharacterController ally in currentAllyParty)
        {
            if(ally != null)
            {
                ally.GetComponent<IAllyExpUpdatable>().UpdateNextLevelExp(exp);
            }
        }
        // セーブを実行する
        SaveManager.Instance.Save();
        // 勝利演出
        BattleUIManager.Instance.ShowVictoryUI(exp, currentAllyParty);
    }

    /// <summary>
    /// 敗北時の処理
    /// </summary>
    public void Defeat()
    {
        // 敗北演出
        BattleUIManager.Instance.ShowDefeatUI();
    }

    /// <summary>
    /// ウェーブ開始処理
    /// </summary>
    private async void StartWave()
    {
        // ウェーブ数更新
        wave++;

        // 味方を移動させる
        MoveAlliesStarting();
        // 敵初期化
        InitEnemies();

        // ウェーブ開始演出
        BattleUIManager.Instance.ShowWaveStartingUI(wave, currentAllyParty, currentEnemyParty);
        await UniTask.Delay(TimeSpan.FromSeconds(setting.WaveStartingDuration), cancellationToken: this.GetCancellationTokenOnDestroy());

        // ターン開始
        StartTurn();
    }

    /// <summary>
    /// ウェーブ終了処理
    /// </summary>
    public async void EndWave()
    {
        // 初期化
        turn = 0;
        // ステージクリア判定
        if(wave == setting.EnemyDBList.Count)
        {
            Victory();
            return;
        }

        // バフを解除
        foreach(AbstractCharacterController character in GetAllCharacters())
        {
            if(character != null)
            {
                character.ClearBuff();
            }
        }

        // 味方を移動させる
        MoveAlliesEnding();

        // ウェーブ終了時演出
        BattleUIManager.Instance.ShowWaveEndingUI();
        await UniTask.Delay(TimeSpan.FromSeconds(setting.WaveEndingDuration), cancellationToken: this.GetCancellationTokenOnDestroy());

        // 死亡した味方を削除する
        DestroyDeadAlly();

        // ウェーブ開始
        StartWave();
    }

    /// <summary>
    /// ターン開始処理
    /// </summary>
    private async void StartTurn()
    {
        // ターン数更新
        turn++;

        // 全キャラクターを行動可能にする
        foreach(AbstractCharacterController character in GetAllCharacters())
        {
            if(character != null)
            {
                character.Context.IsActionable = true;
            }
        }

        // ターン開始演出
        BattleUIManager.Instance.ShowTurnStartingUI(turn);
        await UniTask.Delay(TimeSpan.FromSeconds(setting.TurnStartingDuration), cancellationToken: this.GetCancellationTokenOnDestroy());

        // アクション開始
        StartAction();
    }

    /// <summary>
    /// ターン終了処理
    /// </summary>
    private void EndTurn()
    {
        // バフと状態異常を更新
        foreach(AbstractCharacterController character in GetAllCharacters())
        {
            if(character != null)
            {
                character.ExecuteStatusCondition();
                character.UpdateStatusCondition();
                character.UpdateBuff();
            }
        }

        // ターン終了演出
        BattleUIManager.Instance.ShowTurnEndingUI();
        // await UniTask.Delay(TimeSpan.FromSeconds(waveStartingDuration), cancellationToken: this.GetCancellationTokenOnDestroy());

        StartTurn();
    }

    /// <summary>
    /// 行動開始処理
    /// </summary>
    private void StartAction()
    {
        // 味方が全滅した場合は敗北
        if(IsWipedOutAlly())
        {
            Defeat();
            return;
        }
        // 敵が全滅した場合はウェーブ終了
        if(IsWipedOutEnemy())
        {
            EndWave();
            return;
        }

        // ターンキュー（行動順）を計算
        List<AbstractCharacterController> turnQueue = CalculateTurnQueue();
        // ターンキューが空の場合はターンを終了
        if(turnQueue.Count == 0)
        {
            EndTurn();
            return;
        }

        // アクション開始演出
        BattleUIManager.Instance.ShowActionStartingUI(turnQueue);
        // await UniTask.Delay(TimeSpan.FromSeconds(waveStartingDuration), cancellationToken: this.GetCancellationTokenOnDestroy());

        // アクションを実行
        actingCharacter = turnQueue.First();
        actingCharacter.GetComponent<AbstractCharacterController>().Action();
    }

    /// <summary>
    /// 行動終了処理
    /// </summary>
    public void EndAction()
    {
        // 行動完了にする
        actingCharacter.GetComponent<AbstractCharacterController>().Context.IsActionable = false;

        // アクション終了演出
        // BattleUIManager.Instance.ShowActionEndingUI();
        // await UniTask.Delay(TimeSpan.FromSeconds(waveStartingDuration), cancellationToken: this.GetCancellationTokenOnDestroy());

        // アクション開始
        StartAction();
    }

    /// <summary>
    /// 背景を初期化する
    /// </summary>
    private void InitBackground()
    {
        Instantiate(setting.Background, backgroundHierarchy);
    }

    /// <summary>
    /// 味方を初期化する
    /// </summary>
    private void InitAllies()
    {
        // セーブデータから味方パーティを取得する
        List<AllyType> savedAllyParty = SaveManager.Instance.GetSaveData().Party;
        // 味方パーティを取得
        List<GameObject> allyParty = new List<GameObject>();
        savedAllyParty.ForEach(allyType => allyParty.Add(setting.AllyDB.GetAllyCharacter(allyType)));
        // 味方を配置
        currentAllyParty = new List<AbstractCharacterController>();
        for(int formationNo = 0; formationNo < 4; formationNo++)
        {
            AbstractCharacterController instantiatedAlly = null;
            if(allyParty.Count > formationNo && allyParty[formationNo] != null)
            {
                instantiatedAlly = Instantiate(allyParty[formationNo], GetCharacterPosition(CharacterType.Ally, formationNo) + setting.AllyInitPositionOffset, Constants.Rotation.BATTLE_INIT, alliesHierarchy).GetComponent<AbstractCharacterController>();
                instantiatedAlly.Init(formationNo);
            }
            currentAllyParty.Add(instantiatedAlly);
        }
    }

    /// <summary>
    /// 敵を初期化する
    /// </summary>
    private void InitEnemies()
    {
        // 敵パーティを取得
        List<GameObject> enemyParty = setting.EnemyDBList[wave-1].GetCharacters();
        // 敵を配置
        currentEnemyParty = new List<AbstractCharacterController>();
        for(int formationNo = 0; formationNo < 4; formationNo++)
        {
            AbstractCharacterController instantiatedEnemy = null;
            if(enemyParty.Count > formationNo && enemyParty[formationNo] != null)
            {
                instantiatedEnemy = Instantiate(enemyParty[formationNo], GetCharacterPosition(CharacterType.Enemy, formationNo), Constants.Rotation.BATTLE_INIT, enemiesHierarchy).GetComponent<AbstractCharacterController>();
                instantiatedEnemy.Init(formationNo);
            }
            currentEnemyParty.Add(instantiatedEnemy);
        }
    }

    /// <summary>
    /// 味方を所定位置まで移動させる（開始時）
    /// </summary>
    private void MoveAlliesStarting()
    {
        for(int formationNo = 0; formationNo < 4; formationNo++)
        {
            if(currentAllyParty[formationNo] != null)
            {
                currentAllyParty[formationNo].transform.position = GetCharacterPosition(CharacterType.Ally, formationNo) + setting.AllyInitPositionOffset;
                currentAllyParty[formationNo].Move(GetCharacterPosition(CharacterType.Ally, formationNo), Constants.Duration.WAVE_START_ALLY_MOVING);
            }
        }
    }

    /// <summary>
    /// 味方を所定位置まで移動させる（終了時）
    /// </summary>
    private void MoveAlliesEnding()
    {
        for(int formationNo = 0; formationNo < 4; formationNo++)
        {
            if(currentAllyParty[formationNo] != null)
            {
                currentAllyParty[formationNo].Move(GetCharacterPosition(CharacterType.Ally, formationNo) + setting.AllyFinalPositionOffset, Constants.Duration.WAVE_END_ALLY_MOVING);
            }
        }
    }

    /// <summary>
    /// ターン行動順キューの計算する
    /// </summary>
    /// <returns>ターン行動順キュー</returns>
    private List<AbstractCharacterController> CalculateTurnQueue()
    {
        // キャラクターの行動速度を取得
        // TODO Keyを陣形番号にする（並び替えで行動速度が同値のときの対処）
        Dictionary<AbstractCharacterController, int> characterSpds = new Dictionary<AbstractCharacterController, int>();
        foreach(AbstractCharacterController character in GetAllCharacters())
        {
            // キャラクターがいないときはスキップ
            if(character == null)
            {
                continue;
            }

            // 現在のターンの行動順を計算する場合は、行動可能なキャラクターのみを対象にする
            CharacterContext context = character.Context;
            if(context.IsActionable)
            {
                characterSpds.Add(character, context.Spd);
            }
        }
        // 行動速度順に並び変える
        List<AbstractCharacterController> turnQueue = new List<AbstractCharacterController>();
        foreach(KeyValuePair<AbstractCharacterController, int> character in characterSpds.OrderByDescending(x => x.Value))
        {
            turnQueue.Add(character.Key);
        }
        return turnQueue;
    }

    /// <summary>
    /// 生存している全キャラクターを取得する
    /// </summary>
    /// <returns>生存しているすべてのキャラクターコントローラ</returns>
    public List<AbstractCharacterController> GetAllCharacters()
    {
        return currentAllyParty.Concat(currentEnemyParty).ToList();
    }

    /// <summary>
    /// 指定したキャラクターを取得する
    /// </summary>
    /// <param name="targetType">取得対象のキャラクタータイプ</param>
    /// <param name="targetFormationNo">取得対象の陣形番号</param>
    /// <returns>キャラクターコントローラ</returns>
    public AbstractCharacterController GetTargetCharacter(CharacterType targetType, int targetFormationNo)
    {
        switch(targetType)
        {
            case CharacterType.Ally:
                return currentAllyParty[targetFormationNo];
            case CharacterType.Enemy:
                return currentEnemyParty[targetFormationNo];
            default:
                return null;
        }
    }

    /// <summary>
    /// 指定したキャラクターの座標を取得する
    /// </summary>
    /// <param name="targetType">取得対象のキャラクタータイプ</param>
    /// <param name="targetFormationNo">取得対象の陣形番号</param>
    /// <returns>キャラクター座標</returns>
    public Vector3 GetCharacterPosition(CharacterType targetType, int targetFormationNo)
    {
        switch(targetType)
        {
            case CharacterType.Ally:
                return setting.AllyPositions[targetFormationNo];
            case CharacterType.Enemy:
                return setting.EnemyPositions[targetFormationNo];
            default:
                return Vector3.zero;
        }
    }

    /// <summary>
    /// 指定したキャラクターを削除する
    /// </summary>
    /// <param name="targetType">削除対象のキャラクタータイプ</param>
    /// <param name="target">削除対象のキャラクターコントローラ</param>
    public void RemoveCharacter(CharacterType targetType, AbstractCharacterController target)
    {
        switch(targetType)
        {
            case CharacterType.Ally:
                for(int formationNo = 0; formationNo < currentAllyParty.Count; formationNo++)
                {
                    if(target.Equals(currentAllyParty[formationNo]))
                    {
                        deadAllies.Add(target);
                        currentAllyParty[formationNo] = null;
                    }
                }
                break;
            case CharacterType.Enemy:
                for(int formationNo = 0; formationNo < currentEnemyParty.Count; formationNo++)
                {
                    if(target.Equals(currentEnemyParty[formationNo]))
                    {
                        currentEnemyParty[formationNo] = null;
                    }
                }
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 本ステージクリアで獲得できる経験値を加算する
    /// </summary>
    /// <param name="exp">加算分の経験値</param>
    public void AddExp(int exp)
    {
        this.exp += exp;
    }

    /// <summary>
    /// 味方が全滅したか
    /// </summary>
    /// <returns>味方が全滅したか</returns>
    private bool IsWipedOutAlly()
    {
        foreach(AbstractCharacterController ally in currentAllyParty)
        {
            if(ally != null)
                return false;
        }
        return true;
    }

    /// <summary>
    /// 敵が全滅したか
    /// </summary>
    /// <returns>敵が全滅したか</returns>
    private bool IsWipedOutEnemy()
    {
        foreach(AbstractCharacterController enemy in currentEnemyParty)
        {
            if(enemy != null)
                return false;
        }
        return true;
    }

    /// <summary>
    /// 死亡した味方をシーン上から削除する
    /// </summary>
    private void DestroyDeadAlly()
    {
        deadAllies.ForEach(deadAlly => Destroy(deadAlly.gameObject));
        deadAllies = new List<AbstractCharacterController>();
    }
}
