using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "CharacterRenderingSetting", menuName="CreateCharacterRenderingSetting")]
public class CharacterRenderingSetting : ScriptableObject
{
    [SerializeField] private Color color;
    [SerializeField] private Material opaqueMaterial;
    [SerializeField] private Material transparentMaterial;
    
    public Color Color{get{return color;}}
    public Material OpaqueMaterial{get{return opaqueMaterial;}}
    public Material TransparentMaterial{get{return transparentMaterial;}}
}
