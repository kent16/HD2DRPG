using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[CreateAssetMenu(fileName = "StageSettingDB", menuName="CreateStageSettingDB")]
public class StageSettingDB : ScriptableObject
{
	[SerializeField] private List<StageSetting> stageSettings;
 
	public List<StageSetting> GetStageSettings()
    {
		return stageSettings;
	}
	public StageSetting GetStageSetting(int stageNo)
    {
		return stageSettings[stageNo - 1];
	}
}
