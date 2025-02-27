using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAllyExpUpdatable
{
    /// <summary>
    /// レベルアップに必要な経験値を更新する
    /// </summary>
    /// <param name="exp">新たに獲得した経験値</param>
    public void UpdateNextLevelExp(int exp);
}
