using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartySelectUIController : AbstractModalUIController
{
    [Header("Icon")]
    // 味方アイコン背景
    [SerializeField] private List<Image> allyBackgrounds;

    [Header("Color")]
    // 選択時の背景色
    [SerializeField] private Color selectedColor;
    // 非選択時の背景色
    [SerializeField] private Color notSelectedColor;

    // 選択中の味方番号
    private int selectedAllyNo = 0;
    // 選択対象の陣形番号
    private int targetFormationNo = 0;
    // 呼び出し元のパーティ編成画面
    private PartyUIController partyUI;

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
        if(Input.GetKeyDown(KeyCode.LeftArrow))
            SelectPrevAlly();
        if(Input.GetKeyDown(KeyCode.RightArrow))
            SelectNextAlly();
    }

    /// <summary>
    /// 終了時のコールバック処理
    /// </summary>
    protected override void Callback()
    {
        // 設定対象の味方番号をタイプに変換
        AllyType targetAllyType = AllyType.None;
        switch(selectedAllyNo)
        {
            case 0:
                targetAllyType = AllyType.None;
                break;
            case 1:
                targetAllyType = AllyType.Warrior;
                break;
            case 2:
                targetAllyType = AllyType.Archer;
                break;
            case 3:
                targetAllyType = AllyType.Sister;
                break;
            case 4:
                targetAllyType = AllyType.Knight;
                break;
            case 5:
                targetAllyType = AllyType.Magician;
                break;
            case 6:
                targetAllyType = AllyType.Thief;
                break;
            default:
                targetAllyType = AllyType.None;
                break;
        }
        // パーティ編成画面に設定
        partyUI.SetPartyMember(targetFormationNo, targetAllyType);
    }

    /// <summary>
    /// 初期化する
    /// </summary>
    /// <param name="partyUI">呼び出し元のパーティ編成画面</param>
    /// <param name="targetFormationNo">設定対象の陣形番号</param>
    /// <param name="targetAllyType">設定対象の味方タイプ</param>
    public void Init(PartyUIController partyUI, int targetFormationNo, AllyType targetAllyType)
    {
        this.partyUI = partyUI;
        this.targetFormationNo = targetFormationNo;
        // 設定対象の味方タイプを番号に変換
        selectedAllyNo = 0;
        switch(targetAllyType)
        {
            case AllyType.Warrior:
                selectedAllyNo = 1;
                break;
            case AllyType.Archer:
                selectedAllyNo = 2;
                break;
            case AllyType.Sister:
                selectedAllyNo = 3;
                break;
            case AllyType.Knight:
                selectedAllyNo = 4;
                break;
            case AllyType.Magician:
                selectedAllyNo = 5;
                break;
            case AllyType.Thief:
                selectedAllyNo = 6;
                break;
            default:
                selectedAllyNo = 0;
                break;
        }
        // 選択時の表示をする
        DisplaySelecting();
    }

    /// <summary>
    /// 前の味方を選択する
    /// </summary>
    public void SelectPrevAlly()
    {
        if(0 < selectedAllyNo)
        {
            selectedAllyNo--;
            DisplaySelecting();
        }
    }

    /// <summary>
    /// 次の味方を選択する
    /// </summary>
    public void SelectNextAlly()
    {
        if(selectedAllyNo < allyBackgrounds.Count - 1)
        {
            selectedAllyNo++;
            DisplaySelecting();
        }
    }

    /// <summary>
    /// 選択時の表示をする
    /// </summary>
    public void DisplaySelecting()
    {
        for(int i = 0; i < allyBackgrounds.Count; i++)
        {
            if(selectedAllyNo == i)
                allyBackgrounds[i].color = selectedColor;
            else
                allyBackgrounds[i].color = notSelectedColor;
        }
    }
}
