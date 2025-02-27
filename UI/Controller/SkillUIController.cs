using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillUIController : AbstractPermanentUIController
{
    [Header("Common")]
    // 陣形番号
    [SerializeField] private int formationNo;

    [Header("Skill")]
    // スキルUI
    [SerializeField] private CanvasGroup skillUI;
    // スキル名テキスト
    [SerializeField] private TextMeshProUGUI skillNameText;
    // スキル説明テキスト
    [SerializeField] private TextMeshProUGUI skillDiscriptionText;
    // スキル消費MPテキスト
    [SerializeField] private TextMeshProUGUI skillMPValueText;
    // スキルMP不足テキスト
    [SerializeField] private TextMeshProUGUI skillMPShortageText;

    [Header("Display")]
    // 表示オフセット
    [SerializeField] private Vector3 offset;

    // 選択中スキル番号
    private int selectedSkillNo = 0;
    // 選択中スキル使用対象番号
    private List<int> selectedTargetFormationNos = new List<int>();
    // スキルUI状態
    private SkillUIState state = SkillUIState.None;

    /// <summary>
    /// 開始フレームで呼び出される組み込み処理
    /// </summary>
    protected override void StartImpl()
    {

    }

    /// <summary>
    /// 更新フレームで呼び出される組み込み処理
    /// </summary>
    protected override void UpdateImpl()
    {
        // UIの座標調整
        AdjustUIPositionToCharacter();

        switch(state)
        {
            case SkillUIState.SelectingSkill:
                // キー入力
                if(Input.GetKeyDown(KeyCode.LeftArrow))
                    ChangePrevSkill();
                if(Input.GetKeyDown(KeyCode.RightArrow))
                    ChangeNextSkill();
                if(Input.GetKeyDown(KeyCode.Return))
                    DetermineSkill();
                break;
            case SkillUIState.SelectingSkillTarget:
                // 選択中のキャラクターをハイライトする
                HighLightCharacters();
                // キー入力
                if(Input.GetKeyDown(KeyCode.UpArrow))
                    ChangeUpTarget();
                if(Input.GetKeyDown(KeyCode.DownArrow))
                    ChangeDownTarget();
                if(Input.GetKeyDown(KeyCode.RightArrow))
                    ChangeRightTarget();
                if(Input.GetKeyDown(KeyCode.LeftArrow))
                    ChangeLeftTarget();
                if(Input.GetKeyDown(KeyCode.Return))
                    DetermineTarget();
                if(Input.GetKeyDown(KeyCode.Escape))
                    ReturnSelectingSkill();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// スキルUIの座標をキャラクターの位置に合わせる
    /// </summary>
    private void AdjustUIPositionToCharacter()
    {
        rectTransform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, BattleManager.Instance.GetCharacterPosition(CharacterType.Ally, formationNo) + offset);
    }

    /// <summary>
    /// スキルUIを表示する<br>
    /// スキル表示時に初期化を行うため、親クラスの同メソッドをオーバーライドする
    /// </summary>
    public override void Display()
    {
        // 初期化
        InitSkillUI();
        // スキルUIを表示（処理内容は親クラスで定義）
        base.Display();
    }

    /// <summary>
    /// スキルUIを初期化する
    /// </summary>
    public void InitSkillUI()
    {
        // 状態遷移：スキル選択中
        state = SkillUIState.SelectingSkill;
        // 選択中スキルを初期化
        selectedSkillNo = 0;
        // スキル情報を設定
        SetSkillInfo();
    }

    /// <summary>
    /// スキルUIにスキル情報を設定する
    /// </summary>
    private void SetSkillInfo()
    {
        SkillSetting selectedSkillSetting = GetSelectedSkillSetting();
        skillNameText.text = selectedSkillSetting.SkillName;
        skillDiscriptionText.text = selectedSkillSetting.Discription;
        skillMPValueText.text = selectedSkillSetting.Mp.ToString();
        if(IsSufficientMP())
            skillMPShortageText.alpha = 0;
        else
            skillMPShortageText.alpha = 1;
    }

    /// <summary>
    /// スキル使用キャラクターを取得する
    /// </summary>
    private AbstractCharacterController GetSkillPrincipalCharacter()
    {
        return BattleManager.Instance.CurrentAllyParty[formationNo];
    }

    /// <summary>
    /// キャラクターの全スキルを取得する
    /// </summary>
    private List<AbstractSkillExecutor> GetSkills()
    {
        return GetSkillPrincipalCharacter().Setting.SkillDB.GetSkills();
    }

    /// <summary>
    /// 選択中のスキル設定を取得する
    /// </summary>
    private SkillSetting GetSelectedSkillSetting()
    {
        return GetSkills()[selectedSkillNo].Setting;
    }

    /// <summary>
    /// スキル使用対象のキャラクターを取得する
    /// </summary>
    private List<AbstractCharacterController> GetSkillTargetCharacters()
    {
        List<AbstractCharacterController> skillTargetCharacters = new List<AbstractCharacterController>();
        switch(GetSelectedSkillSetting().TargetCharacterType)
        {
            case CharacterType.Ally:
                selectedTargetFormationNos.ForEach(n => skillTargetCharacters.Add(BattleManager.Instance.CurrentAllyParty[n]));
                break;
            case CharacterType.Enemy:
                selectedTargetFormationNos.ForEach(n => skillTargetCharacters.Add(BattleManager.Instance.CurrentEnemyParty[n]));
                break;
            default:
                break;
        }
        return skillTargetCharacters;
    }

    /// <summary>
    /// 前のスキルに切り替える
    /// </summary>
    public void ChangePrevSkill()
    {
        // 選択中スキル番号を更新
        selectedSkillNo--;
        if(selectedSkillNo < 0)
        {
            selectedSkillNo = GetSkills().Count - 1;
        }
        // スキル情報を設定
        SetSkillInfo();
    }

    /// <summary>
    /// 次のスキルに切り替える
    /// </summary>
    public void ChangeNextSkill()
    {
        // 選択中スキル番号を更新
        selectedSkillNo++;
        if(selectedSkillNo >= GetSkills().Count)
        {
            selectedSkillNo = 0;
        }
        // スキル情報を設定
        SetSkillInfo();
    }

    /// <summary>
    /// 使用するスキルを決定し、スキル使用対象選択に移る
    /// </summary>
    public void DetermineSkill()
    {
        // MPが足りない場合はインフォを表示して終了する
        if(!IsSufficientMP())
        {
            BattleUIManager.Instance.DisplayInfoUI("MPが足りない！");
            return;
        }

        // 状態遷移：スキル使用対象選択中
        state = SkillUIState.SelectingSkillTarget;

        // スキル対象を初期設定
        selectedTargetFormationNos = new List<int>();
        switch(GetSelectedSkillSetting().TargetRangeType)
        {
            case SkillRangeType.OneShort:
            case SkillRangeType.OneLong:
                for(int targetFormationNo = 0; targetFormationNo < Constants.Number.PARTY_MEMBER_NUM; targetFormationNo++)
                {
                    if(ExistSkillTarget(targetFormationNo))
                    {
                        selectedTargetFormationNos.Add(targetFormationNo);
                        break;
                    }
                }
                break;
            case SkillRangeType.ColShort:
            case SkillRangeType.ColLong:
                if(ExistSkillTarget(0) || ExistSkillTarget(1))
                {
                    if(ExistSkillTarget(0))
                        selectedTargetFormationNos.Add(0);
                    if(ExistSkillTarget(1))
                        selectedTargetFormationNos.Add(1);
                }
                else
                {
                    if(ExistSkillTarget(2))
                        selectedTargetFormationNos.Add(2);
                    if(ExistSkillTarget(3))
                        selectedTargetFormationNos.Add(3);
                }
                break;
            case SkillRangeType.Row:
                if(ExistSkillTarget(0) || ExistSkillTarget(2))
                {
                    if(ExistSkillTarget(0))
                        selectedTargetFormationNos.Add(0);
                    if(ExistSkillTarget(2))
                        selectedTargetFormationNos.Add(2);
                }
                else
                {
                    if(ExistSkillTarget(1))
                        selectedTargetFormationNos.Add(1);
                    if(ExistSkillTarget(3))
                        selectedTargetFormationNos.Add(3);
                }
                break;
            case SkillRangeType.All:
                for(int targetFormationNo = 0; targetFormationNo < Constants.Number.PARTY_MEMBER_NUM; targetFormationNo++)
                {
                    if(ExistSkillTarget(targetFormationNo))
                    {
                        selectedTargetFormationNos.Add(targetFormationNo);
                    }
                }
                break;
            case SkillRangeType.Self:
                selectedTargetFormationNos.Add(formationNo);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// スキル使用対象を上に切り替える
    /// </summary>
    public void ChangeUpTarget()
    {
        // 切り替え先のスキル使用対象の陣形番号を取得
        List<int> nextTargetFormationNos = new List<int>();
        switch(GetSelectedSkillSetting().TargetRangeType)
        {
            case SkillRangeType.OneShort:
            case SkillRangeType.OneLong:
                if(selectedTargetFormationNos.Contains(0))
                {
                    if(ExistSkillTarget(1))
                        nextTargetFormationNos.Add(1);
                    else if(ExistSkillTarget(3))
                        nextTargetFormationNos.Add(3);
                }
                if(selectedTargetFormationNos.Contains(2))
                {
                    if(ExistSkillTarget(3))
                        nextTargetFormationNos.Add(3);
                    else if(ExistSkillTarget(1))
                        nextTargetFormationNos.Add(1);
                }
                break;
            case SkillRangeType.Row:
                if(selectedTargetFormationNos.Contains(0)
                || selectedTargetFormationNos.Contains(2))
                {
                    if(ExistSkillTarget(1))
                        nextTargetFormationNos.Add(1);
                    if(ExistSkillTarget(3))
                        nextTargetFormationNos.Add(3);
                }
                break;
            default:
                break;
        }
        // 切り替え先が存在する場合は切り替える
        if(nextTargetFormationNos.Count > 0)
        {
            selectedTargetFormationNos = nextTargetFormationNos;
        }
    }

    /// <summary>
    /// スキル使用対象を下に切り替える
    /// </summary>
    public void ChangeDownTarget()
    {
        // 切り替え先のスキル使用対象の陣形番号を取得
        List<int> nextTargetFormationNos = new List<int>();
        switch(GetSelectedSkillSetting().TargetRangeType)
        {
            case SkillRangeType.OneShort:
            case SkillRangeType.OneLong:
                if(selectedTargetFormationNos.Contains(1))
                {
                    if(ExistSkillTarget(0))
                        nextTargetFormationNos.Add(0);
                    else if(ExistSkillTarget(2))
                        nextTargetFormationNos.Add(2);
                }
                if(selectedTargetFormationNos.Contains(3))
                {
                    if(ExistSkillTarget(2))
                        nextTargetFormationNos.Add(2);
                    else if(ExistSkillTarget(0))
                        nextTargetFormationNos.Add(0);
                }
                break;
            case SkillRangeType.Row:
                if(selectedTargetFormationNos.Contains(1)
                || selectedTargetFormationNos.Contains(3))
                {
                    if(ExistSkillTarget(0))
                        nextTargetFormationNos.Add(0);
                    if(ExistSkillTarget(2))
                        nextTargetFormationNos.Add(2);
                }
                break;
            default:
                break;
        }
        // 切り替え先が存在する場合は切り替える
        if(nextTargetFormationNos.Count > 0)
        {
            selectedTargetFormationNos = nextTargetFormationNos;
        }
    }

    /// <summary>
    /// スキル使用対象を右に切り替える
    /// </summary>
    public void ChangeRightTarget()
    {
        // 切り替え先のスキル使用対象の陣形番号を取得
        List<int> nextTargetFormationNos = new List<int>();
        switch(GetSelectedSkillSetting().TargetCharacterType)
        {
            case CharacterType.Ally:
                switch(GetSelectedSkillSetting().TargetRangeType)
                {
                    case SkillRangeType.OneLong:
                        if(selectedTargetFormationNos.Contains(2))
                        {
                            if(ExistSkillTarget(0))
                                nextTargetFormationNos.Add(0);
                            else if(ExistSkillTarget(1))
                                nextTargetFormationNos.Add(1);
                        }
                        if(selectedTargetFormationNos.Contains(3))
                        {
                            if(ExistSkillTarget(1))
                                nextTargetFormationNos.Add(1);
                            else if(ExistSkillTarget(0))
                                nextTargetFormationNos.Add(0);
                        }
                        break;
                    case SkillRangeType.ColLong:
                        if(selectedTargetFormationNos.Contains(2)
                        || selectedTargetFormationNos.Contains(3))
                        {
                            if(ExistSkillTarget(0))
                                nextTargetFormationNos.Add(0);
                            if(ExistSkillTarget(1))
                                nextTargetFormationNos.Add(1);
                        }
                        break;
                    default:
                        break;
                }
                break;
            case CharacterType.Enemy:
                switch(GetSelectedSkillSetting().TargetRangeType)
                {
                    case SkillRangeType.OneLong:
                        if(selectedTargetFormationNos.Contains(0))
                        {
                            if(ExistSkillTarget(2))
                                nextTargetFormationNos.Add(2);
                            else if(ExistSkillTarget(3))
                                nextTargetFormationNos.Add(3);
                        }
                        if(selectedTargetFormationNos.Contains(1))
                        {
                            if(ExistSkillTarget(3))
                                nextTargetFormationNos.Add(3);
                            else if(ExistSkillTarget(2))
                                nextTargetFormationNos.Add(2);
                        }
                        break;
                    case SkillRangeType.ColLong:
                        if(selectedTargetFormationNos.Contains(0)
                        || selectedTargetFormationNos.Contains(1))
                        {
                            if(ExistSkillTarget(2))
                                nextTargetFormationNos.Add(2);
                            if(ExistSkillTarget(3))
                                nextTargetFormationNos.Add(3);
                        }
                        break;
                    default:
                        break;
                }
                break;
        }
        // 切り替え先が存在する場合は切り替える
        if(nextTargetFormationNos.Count > 0)
        {
            selectedTargetFormationNos = nextTargetFormationNos;
        }
    }

    /// <summary>
    /// スキル使用対象を左に切り替える
    /// </summary>
    public void ChangeLeftTarget()
    {
        // 切り替え先のスキル使用対象の陣形番号を取得
        List<int> nextTargetFormationNos = new List<int>();
        switch(GetSelectedSkillSetting().TargetCharacterType)
        {
            case CharacterType.Ally:
                switch(GetSelectedSkillSetting().TargetRangeType)
                {
                    case SkillRangeType.OneLong:
                        if(selectedTargetFormationNos.Contains(0))
                        {
                            if(ExistSkillTarget(2))
                                nextTargetFormationNos.Add(2);
                            else if(ExistSkillTarget(3))
                                nextTargetFormationNos.Add(3);
                        }
                        if(selectedTargetFormationNos.Contains(1))
                        {
                            if(ExistSkillTarget(3))
                                nextTargetFormationNos.Add(3);
                            else if(ExistSkillTarget(2))
                                nextTargetFormationNos.Add(2);
                        }
                        break;
                    case SkillRangeType.ColLong:
                        if(selectedTargetFormationNos.Contains(0)
                        || selectedTargetFormationNos.Contains(1))
                        {
                            if(ExistSkillTarget(2))
                                nextTargetFormationNos.Add(2);
                            if(ExistSkillTarget(3))
                                nextTargetFormationNos.Add(3);
                        }
                        break;
                    default:
                        break;
                }
                break;
            case CharacterType.Enemy:
                switch(GetSelectedSkillSetting().TargetRangeType)
                {
                    case SkillRangeType.OneLong:
                        if(selectedTargetFormationNos.Contains(2))
                        {
                            if(ExistSkillTarget(0))
                                nextTargetFormationNos.Add(0);
                            else if(ExistSkillTarget(1))
                                nextTargetFormationNos.Add(1);
                        }
                        if(selectedTargetFormationNos.Contains(3))
                        {
                            if(ExistSkillTarget(1))
                                nextTargetFormationNos.Add(1);
                            else if(ExistSkillTarget(0))
                                nextTargetFormationNos.Add(0);
                        }
                        break;
                    case SkillRangeType.ColLong:
                        if(selectedTargetFormationNos.Contains(2)
                        || selectedTargetFormationNos.Contains(3))
                        {
                            if(ExistSkillTarget(0))
                                nextTargetFormationNos.Add(0);
                            if(ExistSkillTarget(1))
                                nextTargetFormationNos.Add(1);
                        }
                        break;
                    default:
                        break;
                }
                break;
        }
        // 切り替え先が存在する場合は切り替える
        if(nextTargetFormationNos.Count > 0)
        {
            selectedTargetFormationNos = nextTargetFormationNos;
        }
    }

    /// <summary>
    /// スキル対象が存在するか
    /// </summary>
    /// <param name="targetFormationNo">存在するか判定する対象の陣形番号</param>
    private bool ExistSkillTarget(int targetFormationNo)
    {
        switch(GetSelectedSkillSetting().TargetCharacterType)
        {
            case CharacterType.Ally:
                if(BattleManager.Instance.CurrentAllyParty[targetFormationNo] != null)
                    return true;
                else
                    return false;
            case CharacterType.Enemy:
                if(BattleManager.Instance.CurrentEnemyParty[targetFormationNo] != null)
                    return true;
                else
                    return false;
            default:
                return false;
        }
    }

    /// <summary>
    /// スキルを使用する対象を決定し、スキルを呼び出す
    /// </summary>
    public void DetermineTarget()
    {
        // スキルを取得
        AbstractSkillExecutor skill = GetSkills()[selectedSkillNo];
        // スキルを使用するキャラクター、およびスキル使用対象のキャラクターを取得
        AbstractCharacterController principal = GetSkillPrincipalCharacter();
        List<AbstractCharacterController> targets = GetSkillTargetCharacters();

        // スキルを呼び出す
        skill.Invoke(principal, targets);

        // スキル対象のハイライトを解除
        StopHighLightCharacters();
        // スキルUIを非表示にする
        BattleUIManager.Instance.HideSkillUI(formationNo);
        // スキルUI状態を初期化
        state = SkillUIState.None;
    }

    /// <summary>
    /// スキル選択に戻り、使用するスキルを再度選択する
    /// </summary>
    public void ReturnSelectingSkill()
    {
        // スキル対象のハイライトを解除
        StopHighLightCharacters();
        // 状態遷移：スキル選択中
        state = SkillUIState.SelectingSkill;
    }

    /// <summary>
    /// スキル使用対象のキャラクターをハイライトする
    /// </summary>
    public void HighLightCharacters()
    {
        List<AbstractCharacterController> allCharacters = BattleManager.Instance.GetAllCharacters();
        List<AbstractCharacterController> targetCharacters = GetSkillTargetCharacters();
        foreach(AbstractCharacterController character in allCharacters)
        {
            if(character != null)
            {
                if(targetCharacters.Contains(character))
                    character.HighLight(true);
                else
                    character.HighLight(false);
            }
        }
    }

    /// <summary>
    /// スキル使用対象のキャラクターのハイライトを解除する
    /// </summary>
    public void StopHighLightCharacters()
    {
        List<AbstractCharacterController> allCharacters = BattleManager.Instance.GetAllCharacters();
        foreach(AbstractCharacterController character in allCharacters)
        {
            if(character != null)
            {
                character.HighLight(false);
            }
        }
    }

    /// <summary>
    /// MPが足りるか
    /// </summary>
    /// <returns>MPが足りるか</returns>
    public bool IsSufficientMP()
    {
        // 現在のMP取得
        int currentMp = GetSkillPrincipalCharacter().Context.Mp;
        // 必要なMP取得
        int requriredMp = GetSelectedSkillSetting().Mp;
        // MPが足りているか
        if(currentMp >= requriredMp)
            return true;
        else
            return false;
    }
}
