using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[CreateAssetMenu(fileName = "SkillDB", menuName="CreateSkillDB")]
public class SkillDB : ScriptableObject
{
	[SerializeField] private List<GameObject> skills;
 
	public List<AbstractSkillExecutor> GetSkills()
    {
		List<AbstractSkillExecutor> skillExecutors = new List<AbstractSkillExecutor>();
		skills.ForEach(skill => skillExecutors.Add(skill.GetComponent<AbstractSkillExecutor>()));
		return skillExecutors;
	}
}
