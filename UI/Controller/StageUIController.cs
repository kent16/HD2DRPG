using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class StageUIController : AbstractPermanentUIController
{
    // ステージ名テキスト
    [SerializeField] private TextMeshProUGUI stageNameText;
    // ステージ説明テキスト
    [SerializeField] private TextMeshProUGUI stageDiscriptionText;
    // エンカウントする敵のアイコン
    [SerializeField] private List<Image> stageEnemyIcons;

    /// <summary>
    /// 開始フレームで呼び出される組み込み処理
    /// </summary>
    protected override void StartImpl()
    {

    }

    /// <summary>
    /// 更新フレームで呼び出される組み込み処理
    /// </summary>
    protected override void UpdateImpl()
    {
        
    }

    /// <summary>
    /// ステージ情報を設定する
    /// </summary>
    /// <param name="stageSetting">ステージ設定</param>
    public void SetStageInfo(StageSetting stageSetting)
    {
        stageNameText.text = stageSetting.StageName;
        stageDiscriptionText.text = stageSetting.StageDiscription;
        for(int i = 0; i < 3; i++)
        {
            if(stageSetting.StageEnemyIcons.Count > i
            && stageSetting.StageEnemyIcons[i] != null)
            {
                stageEnemyIcons[i].sprite = stageSetting.StageEnemyIcons[i];
                stageEnemyIcons[i].color = Color.white;
            }
            else
            {
                stageEnemyIcons[i].sprite = null;
                stageEnemyIcons[i].color = Color.clear;
            }
        }
    }
}
