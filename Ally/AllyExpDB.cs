using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[CreateAssetMenu(fileName = "AllyExpDB", menuName="CreateAllyExpDB")]
public class AllyExpDB : ScriptableObject
{
	[SerializeField] private List<int> AllyExps;
 
	public List<int> GetExps()
    {
		return AllyExps;
	}
	public int GetExp(int level)
    {
		return AllyExps[level - 1];
	}
}
