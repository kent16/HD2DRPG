using UnityEngine;

public class TalkEventExecutor : AbstractEventExecutor
{
    // イベント対象キャラクター番号
    [SerializeField] private int targetCharacterNo;
    // セリフ内容
    [SerializeField] private string talkingText;

    // セリフUI
    [SerializeField] private TalkUIController talkUI;

    /// <summary>
    /// 実行する
    /// Completeメソッドはエンターキー押下時にTalkUIControllerから呼び出される
    /// </summary>
    public override void Execute()
    {
        AbstractCharacterController target = EventManager.Instance.GetCharacter(targetCharacterNo);

        // セリフUIを表示する
        TalkUIController instantiatedTalkUI = Instantiate(talkUI, EventManager.Instance.EventUIHierarchy);
        instantiatedTalkUI.Init(this, target);
        instantiatedTalkUI.SetText(talkingText);
    }
}
