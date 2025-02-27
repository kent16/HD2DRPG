using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class QueueUIController : AbstractPermanentUIController
{
    // キャラクターアイコン画像
    [SerializeField] private Image characterIconImage;
    // キュー番号
    [SerializeField] private int queueNo;

    /// <summary>
    /// 開始フレームで呼び出される組み込み処理
    /// </summary>
    protected override void StartImpl()
    {
        InitIconUI();
    }

    /// <summary>
    /// 更新フレームで呼び出される組み込み処理
    /// </summary>
    protected override void UpdateImpl()
    {

    }

    // アイコンを初期化
    public void InitIconUI()
    {
        characterIconImage.sprite = null;
        characterIconImage.DOFade(0, 0);
    }

    // アイコンの更新
    public async void SetIconUI(Sprite characterIcon)
    {
        // 一度透明にする
        characterIconImage.DOFade(0, Constants.Duration.UI_DISPLAYING)
                          .SetEase(Ease.InOutSine)
                          .OnComplete(() => characterIconImage.sprite = null);

        await UniTask.Delay(TimeSpan.FromSeconds(Constants.Duration.UI_DISPLAYING), cancellationToken: this.GetCancellationTokenOnDestroy());
        
        // アイコンを表示
        if(characterIcon != null)
        {
            characterIconImage.DOFade(1, Constants.Duration.UI_DISPLAYING)
                              .SetEase(Ease.InOutSine)
                              .OnPlay(() => characterIconImage.sprite = characterIcon);
        }
    }
}
