using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VictoryUIController : AbstractModalUIController
{
    // 経験値テキスト
    [SerializeField] private TextMeshProUGUI expText;
    // レベルテキスト
    [SerializeField] private List<TextMeshProUGUI> levelTexts;
    // レベル経験値テキスト
    [SerializeField] private List<TextMeshProUGUI> levelExpTexts;
    // レベルアップテキスト
    [SerializeField] private List<TextMeshProUGUI> levelUpTexts;
    // アイコン
    [SerializeField] private List<Image> icons;

    // 固定文字列
    private const string EXP = "Exp ";
    private const string LEVEL = "Lv.";
    private const string NEXT_EXP = "Next ";
    private const string LEVEL_UP = "LEVEL UP!";

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
    /// 勝利画面を終了した際に呼び出される
    /// </summary>
    protected override void Callback()
    {
        // 次のフェーズに移る
        GameManager.Instance.NextStagePhase();
    }

    /// <summary>
    /// 勝利情報を設定を設定する
    /// </summary>
    /// <param name="exp">新たに獲得した経験値</param>
    /// <param name="allyParty">味方パーティ</param>
    public void SetVictoryInfo(int exp, List<AbstractCharacterController> allyParty)
    {
        // テキスト・アイコンを設定
        expText.text = EXP + exp.ToString();
        for(int i = 0; i < Constants.Number.PARTY_MEMBER_NUM; i++)
        {
            if(allyParty[i] != null)
            {
                CharacterSetting setting = allyParty[i].Setting;
                CharacterContext context = allyParty[i].Context;
                
                levelTexts[i].text = LEVEL + context.Level.ToString();
                levelExpTexts[i].text = NEXT_EXP + context.NextLevelExp.ToString();
                icons[i].sprite = setting.Icon;
                icons[i].color = Color.white;
                if(context.IsLevelUpped)
                    levelUpTexts[i].text = LEVEL_UP;
                else
                    levelUpTexts[i].text = string.Empty;
            }
            else
            {
                levelTexts[i].text = string.Empty;
                levelExpTexts[i].text = string.Empty;
                levelUpTexts[i].text = string.Empty;
                icons[i].color = Color.clear;
            }
        }
    }
}
