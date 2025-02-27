using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class StatusUIController : AbstractPermanentUIController
{
    [Header("Common")]
    // タイプ
    [SerializeField] private CharacterType type;
    // 陣形番号
    [SerializeField] private int formationNo;

    [Header("Display")]
    // 表示オフセット
    [SerializeField] private Vector3 offset;

    [Header("Name")]
    // キャラクター名
    [SerializeField] private TextMeshProUGUI nameText;

    [Header("HP")]
    // HPスライダー
    [SerializeField] private Slider hpSlider;
    // HPテキスト
    [SerializeField] private TextMeshProUGUI hpText;

    [Header("MP")]
    // MPスライダー
    [SerializeField] private Slider mpSlider;
    // MPテキスト
    [SerializeField] private TextMeshProUGUI mpText;

    [Header("Buff")]
    // バフ背景
    [SerializeField] private Image buffBackground;
    // バフアイコン（ALL）
    [SerializeField] private Image buffIconAll;
    // バフアイコン（ATK）
    [SerializeField] private Image buffIconAtk;
    // バフアイコン（DEF）
    [SerializeField] private Image buffIconDef;
    // バフアイコン（SPD）
    [SerializeField] private Image buffIconSpd;
    // バフターン
    [SerializeField] private TextMeshProUGUI buffTurnText;

    [Header("StatusCondition")]
    // 状態異常背景
    [SerializeField] private Image statusConditionBackground;
    // 状態異常アイコン（毒）
    [SerializeField] private Image statusConditionIconPoison;
    // 状態異常アイコン（麻痺）
    [SerializeField] private Image statusConditionIconParalysis;
    // 状態異常アイコン（眠り）
    [SerializeField] private Image statusConditionIconSleep;
    // 状態異常ターン
    [SerializeField] private TextMeshProUGUI statusConditionTurnText;

    /// <summary>
    /// 開始フレームで呼び出される組み込み処理
    /// </summary>
    protected override void StartImpl()
    {
        // バフ情報を非表示にする
        buffBackground.DOFade(0, 0);
        buffIconAll.DOFade(0, 0);
        buffIconAtk.DOFade(0, 0);
        buffIconDef.DOFade(0, 0);
        buffIconSpd.DOFade(0, 0);
        buffTurnText.text = string.Empty;
        // 状態異常情報を非表示にする
        statusConditionBackground.DOFade(0, 0);
        statusConditionIconPoison.DOFade(0, 0);
        statusConditionIconParalysis.DOFade(0, 0);
        statusConditionIconSleep.DOFade(0, 0);
        statusConditionTurnText.text = string.Empty;
    }

    /// <summary>
    /// 更新フレームで呼び出される組み込み処理
    /// </summary>
    protected override void UpdateImpl()
    {
        AdjustUIPositionToCharacter();
    }

    /// <summary>
    /// ステータスUIの座標をキャラクターの位置に合わせる
    /// </summary>
    private void AdjustUIPositionToCharacter()
    {
        rectTransform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, BattleManager.Instance.GetCharacterPosition(type, formationNo) + offset);
    }

    /// <summary>
    /// 味方ステータスUIの値を設定
    /// </summary>
    /// <param name="setting">キャラクター設定</param>
    /// <param name="context">キャラクターコンテクスト</param>
    public void SetAllyStatus(CharacterSetting setting, CharacterContext context)
    {
        SetName(setting);
        SetHp(setting, context);
        SetMp(setting, context);
        SetBuff(context);
        SetStatusCondition(context);
    }

    /// <summary>
    /// 敵ステータスUIの値を設定
    /// </summary>
    /// <param name="setting">キャラクター設定</param>
    /// <param name="context">キャラクターコンテクスト</param>
    public void SetEnemyStatus(CharacterSetting setting, CharacterContext context)
    {
        SetName(setting);
        SetHp(setting, context);
        SetBuff(context);
        SetStatusCondition(context);
    }

    /// <summary>
    /// キャラクター名を設定
    /// </summary>
    /// <param name="setting">キャラクター設定</param>
    private void SetName(CharacterSetting setting)
    {
        nameText.text = setting.CharacterName;
    }

    /// <summary>
    /// HPを設定
    /// </summary>
    /// <param name="setting">キャラクター設定</param>
    /// <param name="context">キャラクターコンテクスト</param>
    private void SetHp(CharacterSetting setting, CharacterContext context)
    {
        hpSlider.value = (float)context.Hp / setting.Hp;
        hpText.text = context.Hp.ToString();
    }

    /// <summary>
    /// MPを設定
    /// </summary>
    /// <param name="setting">キャラクター設定</param>
    /// <param name="context">キャラクターコンテクスト</param>
    private void SetMp(CharacterSetting setting, CharacterContext context)
    {
        mpSlider.value = (float)context.Mp / setting.Mp;
        mpText.text = context.Mp.ToString();
    }

    /// <summary>
    /// バフを設定
    /// </summary>
    /// <param name="context">キャラクターコンテクスト</param>
    private void SetBuff(CharacterContext context)
    {
        // ターン数の表示
        buffTurnText.text = context.BuffTurn.ToString();
        // 背景の表示
        if(context.BuffType == SkillBuffType.None)
            buffBackground.DOFade(0, Constants.Duration.UI_DISPLAYING).SetEase(Ease.InOutSine);
        else
            buffBackground.DOFade(1, Constants.Duration.UI_DISPLAYING).SetEase(Ease.InOutSine);
        // アイコンの表示
        switch(context.BuffType)
        {
            case SkillBuffType.All:
                buffIconAll.DOFade(1, Constants.Duration.UI_DISPLAYING).SetEase(Ease.InOutSine);
                break;
            case SkillBuffType.Atk:
                buffIconAtk.DOFade(1, Constants.Duration.UI_DISPLAYING).SetEase(Ease.InOutSine);
                break;
            case SkillBuffType.Def:
                buffIconDef.DOFade(1, Constants.Duration.UI_DISPLAYING).SetEase(Ease.InOutSine);
                break;
            case SkillBuffType.Spd:
                buffIconSpd.DOFade(1, Constants.Duration.UI_DISPLAYING).SetEase(Ease.InOutSine);
                break;
            case SkillBuffType.None:
                buffTurnText.text = string.Empty;
                if(buffIconAll.color.a > 0.0f)
                    buffIconAll.DOFade(0, Constants.Duration.UI_DISPLAYING).SetEase(Ease.InOutSine);
                if(buffIconAtk.color.a > 0.0f)
                    buffIconAtk.DOFade(0, Constants.Duration.UI_DISPLAYING).SetEase(Ease.InOutSine);
                if(buffIconDef.color.a > 0.0f)
                    buffIconDef.DOFade(0, Constants.Duration.UI_DISPLAYING).SetEase(Ease.InOutSine);
                if(buffIconSpd.color.a > 0.0f)
                    buffIconSpd.DOFade(0, Constants.Duration.UI_DISPLAYING).SetEase(Ease.InOutSine);
                break;
        }
    }

    /// <summary>
    /// 状態異常を設定
    /// </summary>
    /// <param name="context">キャラクターコンテクスト</param>
    private void SetStatusCondition(CharacterContext context)
    {
        // ターン数の表示
        statusConditionTurnText.text = context.StatusConditionTurn.ToString();
        // 背景の表示
        if(context.StatusConditionType == SkillStatusConditionType.None)
            statusConditionBackground.DOFade(0, Constants.Duration.UI_DISPLAYING).SetEase(Ease.InOutSine);
        else
            statusConditionBackground.DOFade(1, Constants.Duration.UI_DISPLAYING).SetEase(Ease.InOutSine);
        // アイコンの表示
        switch(context.StatusConditionType)
        {
            case SkillStatusConditionType.Poison:
                statusConditionIconPoison.DOFade(1, Constants.Duration.UI_DISPLAYING).SetEase(Ease.InOutSine);
                break;
            case SkillStatusConditionType.Paralysis:
                statusConditionIconParalysis.DOFade(1, Constants.Duration.UI_DISPLAYING).SetEase(Ease.InOutSine);
                break;
            case SkillStatusConditionType.Sleep:
                statusConditionIconSleep.DOFade(1, Constants.Duration.UI_DISPLAYING).SetEase(Ease.InOutSine);
                break;
            case SkillStatusConditionType.None:
                statusConditionTurnText.text = string.Empty;
                if(statusConditionIconPoison.color.a > 0.0f)
                    statusConditionIconPoison.DOFade(0, Constants.Duration.UI_DISPLAYING).SetEase(Ease.InOutSine);
                if(statusConditionIconParalysis.color.a > 0.0f)
                    statusConditionIconParalysis.DOFade(0, Constants.Duration.UI_DISPLAYING).SetEase(Ease.InOutSine);
                if(statusConditionIconSleep.color.a > 0.0f)
                    statusConditionIconSleep.DOFade(0, Constants.Duration.UI_DISPLAYING).SetEase(Ease.InOutSine);
                break;
        }
    }
}
