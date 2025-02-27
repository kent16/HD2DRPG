using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class DamageUIController : AbstractTemporaryUIController
{
    // ダメージテキスト
    [SerializeField] private TextMeshProUGUI damageText;
    // 表示オフセット
    [SerializeField] private Vector3 offset;
    // 移動速度
    [SerializeField] private Vector3 speed;

    // ダメージを受けたキャラクターのRectTransform座標
    private Vector3 damagedCharacterRectTransformPosition;
    // 移動距離
    private Vector3 movementDistance = Vector3.zero;

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
        // 移動距離の加算
        movementDistance += speed * Time.deltaTime;
        // ダメージを受けたキャラクターのRectTransform座標を基準に、ダメージUIを移動させる
        rectTransform.position = damagedCharacterRectTransformPosition + movementDistance;
    }

    /// <summary>
    /// ダメージUIにダメージを設定
    /// </summary>
    /// <param name="type">設定対象のキャラクタータイプ</param>
    /// <param name="formationNo">設定対象の陣形番号</param>
    /// <param name="dmg">ダメージ値</param>
    public void SetDamage(CharacterType type, int formationNo, int dmg)
    {
        // 値を設定
        damageText.text = dmg.ToString();
        // ダメージを受けたキャラクターのRectTransform座標
        damagedCharacterRectTransformPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, BattleManager.Instance.GetCharacterPosition(type, formationNo) + offset);
    }
}
