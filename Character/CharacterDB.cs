using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[CreateAssetMenu(fileName = "CharacterDB", menuName="CreateCharacterDB")]
public class CharacterDB : ScriptableObject
{
	[SerializeField] private List<GameObject> characters;
 
	public List<GameObject> GetCharacters()
    {
		return characters;
	}
	public GameObject GetAllyCharacter(AllyType targetAllyType)
    {
		foreach(GameObject ally in characters)
		{
			if(targetAllyType == ally.GetComponent<AllyController>().AllyType)
			{
				return ally;
			}
		}
		return null;
	}
}
