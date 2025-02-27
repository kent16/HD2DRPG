using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyUIController : AbstractModalUIController
{
    [Header("Hierarchy")]
    // パーティ選択UIヒエラルキー
    [SerializeField] private Transform partySelectHierarchy;

    [Header("UI")]
    // パーティ選択UI
    [SerializeField] private PartySelectUIController partySelect;

    [Header("Icon")]
    // 味方キャラクターアイコンDB
    [SerializeField] private AllyIconDB iconDB;
    // パーティメンバーアイコン
    [SerializeField] private List<Image> partyMemberIcons;
    // パーティメンバーアイコン背景
    [SerializeField] private List<Image> partyMemberBackgrounds;

    [Header("Color")]
    // 選択時の背景色
    [SerializeField] private Color selectedColor;
    // 非選択時の背景色
    [SerializeField] private Color notSelectedColor;

    // パーティメンバー
    private List<AllyType> partyMembers = new List<AllyType>();
    // 選択中の陣形番号
    private int selectedFormationNo = 0;

    /// <summary>
    /// 開始フレームで呼び出される組み込み処理
    /// </summary>
    protected override void StartImpl()
    {
        // セーブデータを取得
        SaveData saveData = SaveManager.Instance.GetSaveData();
        // パーティメンバーを取得
        partyMembers = saveData.Party;
        
        // パーティ情報を設定する
        SetPartyInfo();
        // 選択時の表示をする
        DisplaySelecting();
    }

    /// <summary>
    /// 更新フレームで呼び出される組み込み処理
    /// </summary>
    protected override void UpdateImpl()
    {
        if(IsAvailable)
        {
            if(Input.GetKeyDown(KeyCode.Return))
                DisplayPartySelectUI();
            if(Input.GetKeyDown(KeyCode.LeftArrow))
                SelectPrevAlly();
            if(Input.GetKeyDown(KeyCode.RightArrow))
                SelectNextAlly();
        }
    }

    /// <summary>
    /// 終了時のコールバック処理
    /// </summary>
    protected override void Callback()
    {
        FieldManager.Instance.IsDisplayingModal = false;
        // 更新したパーティをセーブデータに設定する
        SaveData saveData = new SaveData();
        saveData.Party = partyMembers;
        SaveManager.Instance.SetSaveData(saveData);
        // セーブを実行する
        SaveManager.Instance.Save();
    }

    /// <summary>
    /// パーティ情報を設定する
    /// </summary>
    public void SetPartyInfo()
    {
        // パーティメンバー
        for(int i = 0; i < partyMemberIcons.Count; i++)
        {
            if(i < partyMembers.Count && partyMembers[i] != AllyType.None)
            {
                partyMemberIcons[i].sprite = iconDB.GetAllyIcon(partyMembers[i]);
                partyMemberIcons[i].color = Color.white;
            }
            else
            {
                partyMemberIcons[i].sprite = null;
                partyMemberIcons[i].color = Color.clear;
            }
        }
    }

    /// <summary>
    /// パーティメンバー選択画面を表示する
    /// </summary>
    public void DisplayPartySelectUI()
    {
        Instantiate(partySelect, partySelectHierarchy).Init(this, selectedFormationNo, partyMembers[selectedFormationNo]);
        
        // 操作不可能
        IsAvailable = false;
    }

    /// <summary>
    /// 選択したパーティメンバーを設定する
    /// </summary>
    /// <param name="targetFormationNo">設定対象の陣形番号</param>
    /// <param name="targetAllyType">設定対象の味方タイプ</param>
    public void SetPartyMember(int targetFormationNo, AllyType targetAllyType)
    {
        // 設定しようとしている味方を設定していた陣形番号を取得（未設定の場合は-1）
        int prevFormationNo = -1;
        for(int i = 0; i < Constants.Number.PARTY_MEMBER_NUM; i++)
        {
            if(targetAllyType == partyMembers[i])
            {
                prevFormationNo = i;
            }
        }
        // 設定対象の味方を設定
        if(prevFormationNo == -1)
        {
            // 設定しようとしている味方が未設定の場合
            partyMembers[targetFormationNo] = targetAllyType;
        }
        else
        {
            // 設定しようとしている味方が設定済みの場合
            partyMembers[prevFormationNo] = partyMembers[targetFormationNo];
            partyMembers[targetFormationNo] = targetAllyType;
        }
        // パーティ情報を設定する
        SetPartyInfo();
        // 操作可能
        IsAvailable = true;
    }

    /// <summary>
    /// 次の味方を選択する
    /// </summary>
    public void SelectNextAlly()
    {
        if(selectedFormationNo == 0)
            selectedFormationNo = 1;
        else if(selectedFormationNo == 1)
            return;
        else if(selectedFormationNo == 2)
            selectedFormationNo = 3;
        else if(selectedFormationNo == 3)
            selectedFormationNo = 0;
        DisplaySelecting();
    }

    /// <summary>
    /// 前の味方を選択する
    /// </summary>
    public void SelectPrevAlly()
    {
        if(selectedFormationNo == 0)
            selectedFormationNo = 3;
        else if(selectedFormationNo == 1)
            selectedFormationNo = 0;
        else if(selectedFormationNo == 2)
            return;
        else if(selectedFormationNo == 3)
            selectedFormationNo = 2;
        DisplaySelecting();
    }

    /// <summary>
    /// 選択時の表示をする
    /// </summary>
    public void DisplaySelecting()
    {
        for(int i = 0; i < partyMemberBackgrounds.Count; i++)
        {
            if(selectedFormationNo == i)
                partyMemberBackgrounds[i].color = selectedColor;
            else
                partyMemberBackgrounds[i].color = notSelectedColor;
        }
    }
}
