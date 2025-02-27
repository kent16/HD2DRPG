using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatUIController : AbstractModalUIController
{
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
    /// 敗北画面を終了した際に呼び出される
    /// </summary>
    protected override void Callback()
    {
        // ステージを終了する
        GameManager.Instance.EndStage();
    }
}
