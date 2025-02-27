using System.Threading;
using UnityEngine;

public abstract class AbstractEventExecutor : MonoBehaviour
{
    protected CancellationTokenSource cts = new CancellationTokenSource();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 実行する
    /// 処理がすべて完了したら必ずComplete()を呼び出す。
    /// </summary>
    public abstract void Execute();

    /// <summary>
    /// 完了する
    /// </summary>
    public void Complete()
    {
        // イベントマネージャの実行中エグゼキュータ数を更新する
        EventManager.Instance.ActiveExecutorNum--;
    }
}
