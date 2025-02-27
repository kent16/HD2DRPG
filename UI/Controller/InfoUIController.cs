using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class InfoUIController : AbstractTemporaryUIController
{
    // インフォテキスト
    [SerializeField] private TextMeshProUGUI infoText;

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
    /// インフォUIにテキストを設定
    /// </summary>
    /// <param name="text">表示するテキスト</param>
    public void SetText(string text)
    {
        infoText.text = text;
    }
}
