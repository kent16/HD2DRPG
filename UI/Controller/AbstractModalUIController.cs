using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

/// <summary>
/// モーダルなUIの抽象クラス。
/// UI表示の詳細な実装は必ず本クラスを継承して実装する。
/// </summary>
public abstract class AbstractModalUIController : MonoBehaviour
{
    // フェード時間
    [SerializeField] protected float fadeDuration;
    // モーダル終了キー
    [SerializeField] protected KeyCode hideKey;
    // モーダル終了時に非表示にするか
    [SerializeField] protected bool shouldHide;

    // 操作可能か
    public bool IsAvailable{get; set;}

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
        Display();
    }

    // Update is called once per frame
    protected void Update()
    {
        // 子クラスで実装する組み込み処理
        UpdateImpl();

        // モーダル終了キー押下でUIを非表示にし、コールバック処理を呼び出す
        if(IsAvailable)
        {
            if(Input.GetKeyDown(hideKey))
            {
                if(shouldHide)
                {
                    Hide();
                }
                Callback();
            }
        }
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
    /// UIを非表示にした際に呼び出される処理
    /// </summary>
    protected abstract void Callback();

    /// <summary>
    /// UIを表示する
    /// </summary>
    private void Display()
    {
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, fadeDuration)
                   .SetEase(Ease.OutSine)
                   .SetLink(gameObject);
        IsAvailable = true;
    }

    /// <summary>
    /// UIを非表示にする
    /// </summary>
    private void Hide()
    {
        canvasGroup.DOFade(0, fadeDuration)
                   .SetEase(Ease.InSine)
                   .SetLink(gameObject)
                   .OnComplete(() => Destroy(gameObject));
    }
}
