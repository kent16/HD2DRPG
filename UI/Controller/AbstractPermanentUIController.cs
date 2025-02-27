using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 恒常的に表示するUIの抽象クラス。
/// UI表示の詳細な実装は必ず本クラスを継承して実装する。
/// </summary>
public abstract class AbstractPermanentUIController : MonoBehaviour
{
    // フェード時間
    [SerializeField] protected float fadeDuration;

    protected RectTransform rectTransform;
    protected CanvasGroup canvasGroup;

    protected void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
    }

    // Start is called before the first frame update
    protected void Start()
    {
        // 子クラスで実装する組み込み処理
        StartImpl();
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
    /// UIを表示する
    /// </summary>
    public virtual void Display()
    {
        canvasGroup.DOFade(1, fadeDuration).SetEase(Ease.OutSine);
    }

    /// <summary>
    /// UIを非表示にする
    /// </summary>
    public virtual void Hide()
    {
        canvasGroup.DOFade(0, fadeDuration).SetEase(Ease.InSine);
    }
}
