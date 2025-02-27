using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[CreateAssetMenu(fileName = "AllyIconDB", menuName="CreateAllyIconDB")]
public class AllyIconDB : ScriptableObject
{
	[SerializeField] private Sprite warrior;
	[SerializeField] private Sprite archer;
	[SerializeField] private Sprite sister;
	[SerializeField] private Sprite knight;
	[SerializeField] private Sprite magician;
	[SerializeField] private Sprite thief;
 
	public Sprite GetAllyIcon(AllyType targetAllyType)
    {
		switch(targetAllyType)
		{
			case AllyType.Warrior:
				return warrior;
			case AllyType.Archer:
				return archer;
			case AllyType.Sister:
				return sister;
			case AllyType.Knight:
				return knight;
			case AllyType.Magician:
				return magician;
			case AllyType.Thief:
				return thief;
			default:
				return null;
		}
	}
}
