using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Cysharp.Threading.Tasks;
using System.Linq;

public class TalkUIController : MonoBehaviour
{
    [Header("Duration")]
    // フェード時間
    [SerializeField] private float fadeDuration;

    [Header("Content")]
    // セリフテキスト
    [SerializeField] private TextMeshProUGUI talkText;
    // セリフ背景
    [SerializeField] private Image talkBackground;
    // セリフ文字送り速度（1文字あたりの時間）
    [SerializeField] private float captionSpeed;

    [Header("Display")]
    // 初期表示オフセット
    [SerializeField] private Vector3 offset;
    // 縦サイズ
    [SerializeField] private float verticalSize;

    // 表示対象のキャラクター
    private AbstractCharacterController targetCharacter;
    // 呼び出し元のイベントエグゼキュータ
    private AbstractEventExecutor invokedExecutor;
    // キャンセルトークンソース
    private CancellationTokenSource cts = new CancellationTokenSource();
    // 表示が完了したか
    private bool isCompeleted = false;
    // 即時に表示するか
    private bool isImmediately = false;

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private RectTransform bgRectTransform;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        bgRectTransform = talkBackground.GetComponent<RectTransform>();
    }

    // Start is called before the first frame update
    protected void Start()
    {
        AdjustSizeAndPosition();
    }

    // Update is called once per frame
    protected void Update()
    {
        AdjustSizeAndPosition();

        if(Input.GetKeyDown(KeyCode.Return))
        {
            if(isCompeleted)
                Hide();
            else
                isImmediately = true;
        }
    }

    /// <summary>
    /// 初期化する
    /// </summary>
    /// <param name="invokedExecutor">呼び出し元のイベントエグゼキュータ</param>
    /// <param name="targetCharacter">表示対象のキャラクター</param>
    public void Init(AbstractEventExecutor invokedExecutor, AbstractCharacterController targetCharacter)
    {
        this.targetCharacter = targetCharacter;
        this.invokedExecutor = invokedExecutor;
        Display();
    }

    /// <summary>
    /// UIを表示する
    /// </summary>
    private void Display()
    {
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, fadeDuration)
                   .SetEase(Ease.OutSine)
                   .SetLink(gameObject);
    }

    /// <summary>
    /// UIを非表示にする
    /// </summary>
    private void Hide()
    {
        canvasGroup.DOFade(0, fadeDuration)
                   .SetEase(Ease.InSine)
                   .SetLink(gameObject)
                   .OnComplete(() => OnCompelete());
    }

    /// <summary>
    /// 表示完了時の処理
    /// </summary>
    private void OnCompelete()
    {
        // イベントを完了させる
        invokedExecutor.Complete();
        // UIを削除する
        Destroy(gameObject);
    }

    /// <summary>
    /// セリフを設定する
    /// </summary>
    /// <param name="content">セリフ</param>
    public async void SetText(string content)
    {
        talkText.text = string.Empty;
        // UIの表示を完了を待機する
        await UniTask.Delay(TimeSpan.FromSeconds(fadeDuration), cancellationToken: cts.Token);
        // 文字送りする
        List<char> contentArray = content.ToCharArray().ToList();
        for(int i = 0; i < contentArray.Count; i++)
        {
            talkText.text += contentArray[i];
            if(!isImmediately)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(captionSpeed), cancellationToken: cts.Token);
            }
        }
        // 表示完了
        isCompeleted = true;
    }

    /// <summary>
    /// 吹き出しのサイズと位置を調整する
    /// </summary>
    private void AdjustSizeAndPosition()
    {
        // セリフの行数取得
        int line = talkText.textInfo.lineCount;
        // 吹き出しのサイズを行数に合わせて変更
        bgRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, verticalSize + talkText.fontSize * (line - 1));
        // 吹き出しの位置をキャラクターの座標と行数に合わせて変更
        if(targetCharacter != null)
        {
            rectTransform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, targetCharacter.transform.position + offset);
        }
    }
}
