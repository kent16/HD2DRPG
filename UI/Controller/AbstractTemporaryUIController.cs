using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

/// <summary>
/// 一時的に表示するUIの抽象クラス。
/// UI表示の詳細な実装は必ず本クラスを継承して実装する。
/// </summary>
public abstract class AbstractTemporaryUIController : MonoBehaviour
{
    // 表示時間
    [SerializeField] protected float displayDuration;
    // フェード時間
    [SerializeField] protected float fadeDuration;

    protected RectTransform rectTransform;
    protected CanvasGroup canvasGroup;

    protected void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Start is called before the first frame update
    protected void Start()
    {
        // 子クラスで実装する組み込み処理
        StartImpl();
        // 表示
        DisplayAndHide();
    }

    // Update is called once per frame
    protected void Update()
    {
        // 子クラスで実装する組み込み処理
        UpdateImpl();
    }

    /// <summary>
    /// 開始フレームで呼び出される組み込み処理
    /// </summary>
    protected abstract void StartImpl();

    /// <summary>
    /// 更新フレームで呼び出される組み込み処理
    /// </summary>
    protected abstract void UpdateImpl();

    /// <summary>
    /// UIを表示した後、一定時間経過で非表示にする
    /// </summary>
    private async void DisplayAndHide()
    {
        // 表示
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, fadeDuration)
                   .SetEase(Ease.OutSine)
                   .SetLink(gameObject);
        // 一定時間待機
        await UniTask.Delay(TimeSpan.FromSeconds(displayDuration), cancellationToken: this.GetCancellationTokenOnDestroy());
        // 非表示
        canvasGroup.DOFade(0, fadeDuration)
                   .SetEase(Ease.InSine)
                   .SetLink(gameObject)
                   .OnComplete(() => Destroy(gameObject));
    }
}
