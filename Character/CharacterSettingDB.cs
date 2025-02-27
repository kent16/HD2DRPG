using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[CreateAssetMenu(fileName = "CharacterSettingDB", menuName="CreateCharacterSettingDB")]
public class CharacterSettingDB : ScriptableObject
{
	[SerializeField] private List<CharacterSetting> characterSettinges;
 
	public List<CharacterSetting> GetCharacterSettinges()
    {
		return characterSettinges;
	}
	public CharacterSetting GetCharacterSettinge(int level)
    {
		return characterSettinges[level - 1];
	}
}
