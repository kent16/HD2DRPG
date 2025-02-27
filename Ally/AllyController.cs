using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class AllyController : AbstractCharacterController, 
                              IAllyExpUpdatable
{
    [Header("Common")]
    // 味方タイプ
    [SerializeField] private AllyType allyType;
    // 経験値DB
    [SerializeField] private AllyExpDB expDB;
    // ステータスDB
    [SerializeField] private CharacterSettingDB settingDB;

    [Header("Animation")]
    // アニメータ
    [SerializeField] private Animator animator;
    // アニメーション名-アイドル
    [SerializeField] private string animNameIdle;
    // アニメーション名-移動
    [SerializeField] private string animNameMove;
    // アニメーション名-攻撃
    [SerializeField] private string animNameAtack;
    // アニメーション名-ダメージ
    [SerializeField] private string animNameDamage;
    // アニメーション名-死亡
    [SerializeField] private string animNameDead;
    // アニメーション名-勝利
    [SerializeField] private string animNameVictory;

    // 味方タイプ
    public AllyType AllyType{get{return allyType;}}

    // 行動不能時間
    private const float NOT_ACTINABLE_DURATION = 1.5f;


    /// <summary>
    /// 初期化する
    /// </summary>
    /// <param name="formationNo">キャラクターの陣形番号</param>
    public override void Init(int formationNo)
    {
        // セーブデータ取得
        int level;
        int nextLevelExp;
        SaveData saveData = SaveManager.Instance.GetSaveData();
        switch(allyType)
        {
            case AllyType.Warrior:
                level = saveData.LevelWarrior;
                nextLevelExp = saveData.NextExpWarrior;
                break;
            case AllyType.Archer:
                level = saveData.LevelArcher;
                nextLevelExp = saveData.NextExpArcher;
                break;
            case AllyType.Sister:
                level = saveData.LevelSister;
                nextLevelExp = saveData.NextExpSister;
                break;
            case AllyType.Knight:
                level = saveData.LevelKnight;
                nextLevelExp = saveData.NextExpKnight;
                break;
            case AllyType.Magician:
                level = saveData.LevelMagician;
                nextLevelExp = saveData.NextExpMagician;
                break;
            case AllyType.Thief:
                level = saveData.LevelThief;
                nextLevelExp = saveData.NextExpThief;
                break;
            default:
                level = -1;
                nextLevelExp = -1;
                break;
        }
        // 初期化
        Setting = settingDB.GetCharacterSettinge(level);
        Context = new CharacterContext(Setting, formationNo, level, nextLevelExp);
        InitSprite(BattleManager.Instance.Setting.CharacterRenderingSetting);
        // ステータスUIをセット
        BattleUIManager.Instance.SetStatusUI(Setting, Context);
    }

    /// <summary>
    /// 行動する
    /// </summary>
    public override void Action()
    {
        // 状態異常により行動不能の場合は行動終了する
        CheckStatusCondition();
        // スキルメニューを表示する
        BattleUIManager.Instance.DisplaySkillUI(Context.FormationNo);
    }

    /// <summary>
    /// 移動する
    /// </summary>
    /// <param name="targetPosition">移動先の座標</param>
    /// <param name="duration">移動にかかる時間</param>
    public override void Move(Vector3 targetPosition, float duration)
    {
        // 向きを反転する
        if(transform.position.x < targetPosition.x)
            spriteRenderer.flipX = false;
        else if(targetPosition.x < transform.position.x)
            spriteRenderer.flipX = true;
        // 移動する
        transform.DOMove(targetPosition, duration)
                 .SetEase(Ease.InOutSine)
                 .SetLink(gameObject)
                 .OnPlay(() => PlayAnimation(CharacterAnimationType.Dash))
                 .OnComplete(() => PlayAnimation(CharacterAnimationType.Idle));
    }

    /// <summary>
    /// 死亡する
    /// </summary>
    protected override void Dead()
    {
        base.Dead();
        // 死亡アニメーション再生
        PlayAnimation(CharacterAnimationType.Dead);
    }

    /// <summary>
    /// レベルアップに必要な経験値を更新する
    /// </summary>
    /// <param name="exp">新たに獲得した経験値</param>
    public void UpdateNextLevelExp(int exp)
    {
        if(Context.NextLevelExp - exp > 0)
        {
            // 経験値更新
            Context.NextLevelExp -= exp;
        }
        else
        {
            // レベルアップ
            Context.Level++;
            Context.NextLevelExp = expDB.GetExp(Context.Level) + Context.NextLevelExp - exp;
            Context.IsLevelUpped = true;
        }
        // キャラクター情報保存
        SetCharacterInfoToSaveData();
    }

    /// <summary>
    /// キャラクターのレベルと経験値をセーブデータに設定する
    /// </summary>
    private void SetCharacterInfoToSaveData()
    {
        SaveData saveData = new SaveData();
        switch(allyType)
        {
            case AllyType.Warrior:
                saveData.LevelWarrior = Context.Level;
                saveData.NextExpWarrior = Context.NextLevelExp;
                break;
            case AllyType.Archer:
                saveData.LevelArcher = Context.Level;
                saveData.NextExpArcher = Context.NextLevelExp;
                break;
            case AllyType.Sister:
                saveData.LevelSister = Context.Level;
                saveData.NextExpSister = Context.NextLevelExp;
                break;
            case AllyType.Knight:
                saveData.LevelKnight = Context.Level;
                saveData.NextExpKnight = Context.NextLevelExp;
                break;
            case AllyType.Magician:
                saveData.LevelMagician = Context.Level;
                saveData.NextExpMagician = Context.NextLevelExp;
                break;
            case AllyType.Thief:
                saveData.LevelThief = Context.Level;
                saveData.NextExpThief = Context.NextLevelExp;
                break;
            default:
                return;
        }
        SaveManager.Instance.SetSaveData(saveData);
    }

    /// <summary>
    /// アニメーションを再生する
    /// </summary>
    /// <param name="animationType">再生するアニメーションタイプ</param>
    public override void PlayAnimation(CharacterAnimationType animationType)
    {
        switch(animationType)
        {
            case CharacterAnimationType.Relax:
                break;
            case CharacterAnimationType.Walk:
                break;
            case CharacterAnimationType.Dash:
                animator.Play(animNameMove, 0, UnityEngine.Random.Range(0.0f, 1.0f));
                break;
            case CharacterAnimationType.Idle:
                animator.Play(animNameIdle, 0, UnityEngine.Random.Range(0.0f, 1.0f));
                break;
            case CharacterAnimationType.Atack:
                animator.Play(animNameAtack);
                break;
            case CharacterAnimationType.Action:
                break;
            case CharacterAnimationType.Damage:
                break;
            case CharacterAnimationType.Dying:
                break;
            case CharacterAnimationType.Dead:
                animator.Play(animNameDead);
                break;
            case CharacterAnimationType.Victory:
                break;
            default:
                break;
        }
    }
}
