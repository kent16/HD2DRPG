using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class FieldUIManager : MonoBehaviour
{
    [Header("Hierarchy")]
    // インフォUIヒエラルキー
    [SerializeField] private Transform infoHierarchy;
    // ステージUIヒエラルキー
    [SerializeField] private Transform stageHierarchy;
    // パーティUIヒエラルキー
    [SerializeField] private Transform partyHierarchy;

    [Header("UI")]
    // インフォUI
    [SerializeField] private InfoUIController info;
    // ステージUI
    [SerializeField] private StageUIController stage;
    // パーティUI
    [SerializeField] private PartyUIController party;

    // FieldUIManagerインスタンス
    public static FieldUIManager Instance{get; set;}

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
    /// 戦闘開始時のUIを制御する
    /// </summary>
    public void ShowBattleStartingUI()
    {
    }

    /// <summary>
    /// インフォUIを表示する
    /// </summary>
    /// <param name="text">表示するテキスト</param>
    public void DisplayInfoUI(string text)
    {
        Instantiate(info, infoHierarchy).SetText(text);
    }

    /// <summary>
    /// ステージUIを表示する
    /// </summary>
    /// <param name="stageSetting">表示するステージ設定</param>
    public void DisplayStageUI(StageSetting stageSetting)
    {
        stage.Display();
        stage.SetStageInfo(stageSetting);
        // Instantiate(stage, stageHierarchy).SetStageInfo(stageSetting);
    }

    /// <summary>
    /// ステージUIを非表示にする
    /// </summary>
    public void HideStageUI()
    {
        stage.Hide();
    }

    /// <summary>
    /// パーティUIを表示する
    /// </summary>
    public void DisplayPartyUI()
    {
        Instantiate(party, partyHierarchy);
        FieldManager.Instance.IsDisplayingModal = true;
    }
}
