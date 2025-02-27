using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public abstract class AbstractCharacterController : MonoBehaviour
{
    // 設定
    public CharacterSetting Setting{get; set;}
    // コンテキスト
    public CharacterContext Context{get; set;}

    protected SpriteRenderer spriteRenderer;

    protected void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    protected void Start()
    {
        
    }

    // Update is called once per frame
    protected void Update()
    {
        
    }

    /// <summary>
    /// 初期化する
    /// </summary>
    /// <param name="formationNo">キャラクターの陣形番号</param>
    public abstract void Init(int formationNo);
    
    /// <summary>
    /// キャラクターのSpriteRendererを初期化する
    /// </summary>
    /// <param name="renderingSetting">レンダリング設定</param>
    public void InitSprite(CharacterRenderingSetting renderingSetting)
    {
        spriteRenderer.color = renderingSetting.Color;
        spriteRenderer.material = renderingSetting.OpaqueMaterial;
    }

    /// <summary>
    /// 行動する
    /// </summary>
    public abstract void Action();
    
    /// <summary>
    /// 移動する
    /// </summary>
    /// <param name="targetPosition">移動先の座標</param>
    /// <param name="duration">移動にかかる時間</param>
    public abstract void Move(Vector3 targetPosition, float duration);

    /// <summary>
    /// 反転する
    /// </summary>
    /// <param name="isPositive">正方向に反転するか</param>
    public void Flip(bool isPositive)
    {
        spriteRenderer.flipX = isPositive;
    }

    /// <summary>
    /// 死亡する
    /// </summary>
    protected virtual void Dead()
    {
        // パーティから敵を削除する
        BattleManager.Instance.RemoveCharacter(Setting.Type, this);
        // ステータスUIを非表示
        BattleUIManager.Instance.HideStatusUI(Setting.Type, Context.FormationNo);
    }

    /// <summary>
    /// ダメージを受ける
    /// </summary>
    /// <param name="dmg">被ダメージ値</param>
    public void Damage(int dmg)
    {
        if(Context.Hp <= 0)
        {
            return;
        }

        // HPを更新
        Context.Hp -= dmg;
        if(Context.Hp <= 0)
        {
            Context.Hp = 0;
            Dead();
        }
        // ステータスUIをセット
        BattleUIManager.Instance.SetStatusUI(Setting, Context);
        // ダメージUIを表示
        BattleUIManager.Instance.DisplayDamageUI(Setting.Type, Context.FormationNo, dmg);
    }

    /// <summary>
    /// 回復する
    /// </summary>
    /// <param name="healType">回復タイプ</param>
    /// <param name="point">回復値</param>
    public void Heal(SkillHealType healType, int point)
    {
        switch(healType)
        {
            case SkillHealType.HP:
                Context.Hp += point;
                if(Context.Hp > Setting.Hp)
                {
                    Context.Hp = Setting.Hp;
                }
                break;
            case SkillHealType.MP:
                Context.Mp += point;
                if(Context.Mp > Setting.Mp)
                {
                    Context.Mp = Setting.Mp;
                }
                break;
            default:
                break;
        }
        // ステータスUIをセット
        BattleUIManager.Instance.SetStatusUI(Setting, Context);
    }

    /// <summary>
    /// MPを消費する
    /// </summary>
    /// <param name="mp">消費するMP値</param>
    public void UseMP(int mp)
    {
        // MPを更新
        Context.Mp -= mp;
        if(Context.Mp <= 0)
        {
            Context.Mp = 0;
        }
        // ステータスUIをセット
        BattleUIManager.Instance.SetStatusUI(Setting, Context);
    }

    /// <summary>
    /// バフを適用する
    /// </summary>
    /// <param name="buffType">適用するバフタイプ</param>
    /// <param name="buffTurn">バフを適用するターン数</param>
    public void ApplyBuff(SkillBuffType buffType, int buffTurn)
    {
        // バフ設定済みの場合は解除
        if(Context.BuffType != SkillBuffType.None)
        {
            ClearBuff();
        }
        // バフを設定
        Context.BuffType = buffType;
        Context.BuffTurn = buffTurn;
        // バフ実行
        ExecuteBuff();
        // ステータスUIをセット
        BattleUIManager.Instance.SetStatusUI(Setting, Context);
    }

    /// <summary>
    /// バフを実行する
    /// </summary>
    public void ExecuteBuff()
    {
        switch(Context.BuffType)
        {
            case SkillBuffType.All:
                Context.Atk = (int)(Setting.Atk * 1.1f);
                Context.Def = (int)(Setting.Def * 1.1f);
                Context.Spd = (int)(Setting.Spd * 1.1f);
                break;
            case SkillBuffType.Atk:
                Context.Atk = (int)(Setting.Atk * 1.3f);
                break;
            case SkillBuffType.Def:
                Context.Def = (int)(Setting.Def * 1.5f);
                break;
            case SkillBuffType.Spd:
                Context.Spd = (int)(Setting.Spd * 1.5f);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// バフの残ターン数を更新する
    /// </summary>
    public void UpdateBuff()
    {
        if(Context.BuffType != SkillBuffType.None)
        {
            // バフターンを更新
            Context.BuffTurn--;
            // バフが終了した場合は解除
            if(Context.BuffTurn <= 0)
            {
                ClearBuff();
                return;
            }
            // ステータスUIをセット
            BattleUIManager.Instance.SetStatusUI(Setting, Context);
        }
    }

    /// <summary>
    /// バフを解除する
    /// </summary>
    public void ClearBuff()
    {
        // バフ初期化
        Context.Atk = Setting.Atk;
        Context.Def = Setting.Def;
        Context.Spd = Setting.Spd;
        Context.BuffType = SkillBuffType.None;
        Context.BuffTurn = 0;
        // ステータスUIをセット
        BattleUIManager.Instance.SetStatusUI(Setting, Context);
    }

    /// <summary>
    /// 状態異常を適用する
    /// </summary>
    /// <param name="statusConditionType">適用する状態異常タイプ</param>
    /// <param name="statusConditionTurn">状態異常を適用するターン数</param>
    public void ApplyStatusCondition(SkillStatusConditionType statusConditionType, int statusConditionTurn)
    {
        // 状態異常設定済みの場合は解除
        if(Context.StatusConditionType != SkillStatusConditionType.None)
        {
            ClearStatusCondition();
        }
        // 状態異常を設定
        Context.StatusConditionType = statusConditionType;
        Context.StatusConditionTurn = statusConditionTurn;
        // ステータスUIをセット
        BattleUIManager.Instance.SetStatusUI(Setting, Context);
    }

    /// <summary>
    /// 状態異常を実行する
    /// </summary>
    public void ExecuteStatusCondition()
    {
        switch(Context.StatusConditionType)
        {
            case SkillStatusConditionType.Poison:
                int dmg = Setting.Hp / 10;
                Damage(dmg);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 状態異常の残ターン数を更新する
    /// </summary>
    public void UpdateStatusCondition()
    {
        if(Context.StatusConditionType != SkillStatusConditionType.None)
        {
            // 状態異常実行
            ExecuteStatusCondition();
            // 状態異常ターンを更新
            Context.StatusConditionTurn--;
            // 状態異常が終了した場合は解除
            if(Context.StatusConditionTurn <= 0)
            {
                ClearStatusCondition();
                return;
            }
            // ステータスUIをセット
            BattleUIManager.Instance.SetStatusUI(Setting, Context);
        }
    }

    /// <summary>
    /// 状態異常を解除する
    /// </summary>
    public void ClearStatusCondition()
    {
        // 状態異常初期化
        Context.StatusConditionType = SkillStatusConditionType.None;
        Context.StatusConditionTurn = 0;
        // ステータスUIをセット
        BattleUIManager.Instance.SetStatusUI(Setting, Context);
    }
    
    /// <summary>
    /// 状態異常かチェックする
    /// 状態異常により行動不能の場合は行動終了する
    /// </summary>
    public async void CheckStatusCondition()
    {
        switch(Context.StatusConditionType)
        {
            case SkillStatusConditionType.Paralysis:
                BattleUIManager.Instance.DisplayInfoUI(Setting.CharacterName + "は麻痺で動けない！");
                await UniTask.Delay(TimeSpan.FromSeconds(Constants.Duration.STATUS_CONDITION), cancellationToken: this.GetCancellationTokenOnDestroy());
                BattleManager.Instance.EndAction();
                break;
            case SkillStatusConditionType.Sleep:
                BattleUIManager.Instance.DisplayInfoUI(Setting.CharacterName + "は眠っている！");
                await UniTask.Delay(TimeSpan.FromSeconds(Constants.Duration.STATUS_CONDITION), cancellationToken: this.GetCancellationTokenOnDestroy());
                BattleManager.Instance.EndAction();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// アニメーションを再生する
    /// </summary>
    /// <param name="animationType">再生するアニメーションタイプ</param>
    public abstract void PlayAnimation(CharacterAnimationType animationType);
    
    /// <summary>
    /// スキルの使用対象に選択しているキャラクターをハイライトする
    /// </summary>
    /// <param name="isEnable">ハイライトするか（falseの場合はハイライトを解除）</param>
    public void HighLight(bool isEnable)
    {
        if(isEnable)
            spriteRenderer.material.SetColor("_Emission", Constants.Emission.HIGHLIGHT);
        else
            spriteRenderer.material.SetColor("_Emission", Constants.Emission.NORMAL);
    }
}
