using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "CharacterRenderingSetting", menuName="CreateCharacterRenderingSetting")]
public class CharacterRenderingSetting : ScriptableObject
{
    // キャラクターのカラー
    // ポストエフェクトのブルームで白飛びしないように、グレーを指定することを推奨。
    [SerializeField] private Color color;
    // 不透過マテリアル
    // 通常時に使用するマテリアル。TypeがOpaqueなLitシェーダーを適用したマテリアルを設定する。
    // 半透明を表現できない半面、Zディプス（カメラからの奥行き）を書き込むためポストエフェクトの被写界深度が効果的にかかる。
    [SerializeField] private Material opaqueMaterial;
    // 透過マテリアル
    // 半透明な表現が必要な時に使用するマテリアル。TypeがTransparentなLitシェーダーを適用したマテリアルを設定する。
    // Zディプスを書き込まないが、半透明を表現できる。
    [SerializeField] private Material transparentMaterial;
    
    public Color Color{get{return color;}}
    public Material OpaqueMaterial{get{return opaqueMaterial;}}
    public Material TransparentMaterial{get{return transparentMaterial;}}
}
