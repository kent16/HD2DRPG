using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class EnemyController : AbstractCharacterController
{
    [Header("Common")]
    // ステータス
    [SerializeField] private CharacterSetting setting;

    // ノックバックオフセット
    private static readonly Vector3 KNOCK_BACK_OFFSET = new Vector3(0.5f, 0.0f, 0.0f);
    // ノックバック時間
    private const float KNOCK_BACK_DURATION = 0.3f;
    // スキル使用時の移動オフセット
    private static readonly Vector3 SKILL_OFFSET = new Vector3(-0.3f, 0.0f, 0.0f);
    // スキル使用時の移動時間
    private const float SKILL_DURATION = 0.15f;
    // スキル使用時の遅延時間
    private const float SKILL_DELAY = 0.5f;
    // 行動時の遅延
    private const float ACTION_DELAY = 1.0f;

    /// <summary>
    /// 初期化する
    /// </summary>
    /// <param name="formationNo">キャラクターの陣形番号</param>
    public override void Init(int formationNo)
    {
        // 初期化
        Setting = setting;
        Context = new CharacterContext(Setting, formationNo);
        InitSprite(BattleManager.Instance.Setting.CharacterRenderingSetting);
        // ステータスUIをセット
        BattleUIManager.Instance.SetStatusUI(Setting, Context);
    }

    /// <summary>
    /// 行動する
    /// </summary>
    public override async void Action()
    {
        // 状態異常により行動不能の場合は行動終了する
        CheckStatusCondition();

        // 実行するスキルを取得
        AbstractSkillExecutor skill = GetSkill();
        if(skill == null)
        {
            return;
        }

        // スキル実行まで遅延させる
        await UniTask.Delay(TimeSpan.FromSeconds(ACTION_DELAY), cancellationToken: this.GetCancellationTokenOnDestroy());
        // スキル実行
        skill.Invoke(this, GetSkillTargets(skill));
    }

    /// <summary>
    /// 移動する
    /// </summary>
    /// <param name="targetPosition">移動先の座標</param>
    /// <param name="duration">移動にかかる時間</param>
    public override void Move(Vector3 targetPosition, float duration)
    {
        transform.DOMove(targetPosition, duration)
                 .SetEase(Ease.InOutSine)
                 .SetLink(gameObject);
    }

    /// <summary>
    /// 死亡する
    /// </summary>
    protected override void Dead()
    {
        base.Dead();
        // 経験値を追加する
        BattleManager.Instance.AddExp(setting.Exp);
        // 死亡アニメーション
        PlayAnimation(CharacterAnimationType.Dead);
    }

    /// <summary>
    /// アニメーションを再生する
    /// </summary>
    /// <param name="animationType">再生するアニメーションタイプ</param>
    public override async void PlayAnimation(CharacterAnimationType animationType)
    {
        switch(animationType)
        {
            case CharacterAnimationType.Atack:
            case CharacterAnimationType.Action:
                await UniTask.Delay(TimeSpan.FromSeconds(SKILL_DELAY), cancellationToken: this.GetCancellationTokenOnDestroy());
                transform.DOMove(transform.position + SKILL_OFFSET, SKILL_DURATION)
                        .SetEase(Ease.InSine)
                        .SetLink(gameObject);
                await UniTask.Delay(TimeSpan.FromSeconds(SKILL_DURATION), cancellationToken: this.GetCancellationTokenOnDestroy());
                transform.DOMove(BattleManager.Instance.GetCharacterPosition(CharacterType.Enemy, Context.FormationNo), SKILL_DURATION)
                        .SetEase(Ease.OutSine)
                        .SetLink(gameObject);
                break;
            case CharacterAnimationType.Dead:
                transform.DOMove(transform.position + KNOCK_BACK_OFFSET, KNOCK_BACK_DURATION)
                        .SetEase(Ease.OutSine)
                        .SetLink(gameObject)
                        .OnComplete(() => Destroy(gameObject));
                break;
        }
    }

    /// <summary>
    /// 使用するスキルを取得する
    /// </summary>
    private AbstractSkillExecutor GetSkill()
    {
        // スキルを取得
        List<AbstractSkillExecutor> skills = Setting.SkillDB.GetSkills();

        // 実行可能な固定ターンスキルがある場合は、そのスキルを実行する
        int turn = BattleManager.Instance.Turn;
        foreach(AbstractSkillExecutor skill in skills)
        {
            int fixedTurn = skill.Setting.FixedTurn;
            if(fixedTurn != 0 && turn % fixedTurn == 0)
            {
                return skill;
            }
        }

        // スキル重みと最大重みを取得する
        Dictionary<AbstractSkillExecutor, int> skillWeights = new Dictionary<AbstractSkillExecutor, int>();
        int maxWeight = 0;
        foreach(AbstractSkillExecutor skill in skills)
        {
            skillWeights.Add(skill, skill.Setting.Weight);
            maxWeight += skill.Setting.Weight;
        }

        // 重み付けにしたがって実行するスキルを抽選する
        int sumWeight = 0;
        int randomValue = UnityEngine.Random.Range(0, maxWeight);
        foreach(KeyValuePair<AbstractSkillExecutor, int> skillWeight in skillWeights)
        {
            sumWeight += skillWeight.Value;
            if(randomValue < sumWeight)
            {
                return skillWeight.Key;
            }
        }

        return null;
    }

    /// <summary>
    /// スキルの使用対象を取得する
    /// </summary>
    /// <param name="skill">使用するスキル</param>
    private List<AbstractCharacterController> GetSkillTargets(AbstractSkillExecutor skill)
    {
        List<int> skillTargetableFormationNos = new List<int>();
        List<int> skillTargetFormationNos = new List<int>();

        switch(skill.Setting.TargetRangeType)
        {
            case SkillRangeType.OneShort:
                if(BattleManager.Instance.CurrentAllyParty[0])
                    skillTargetableFormationNos.Add(0);
                if(BattleManager.Instance.CurrentAllyParty[1])
                    skillTargetableFormationNos.Add(1);
                if(skillTargetableFormationNos.Count == 0)
                {
                    if(BattleManager.Instance.CurrentAllyParty[2])
                        skillTargetableFormationNos.Add(2);
                    if(BattleManager.Instance.CurrentAllyParty[3])
                        skillTargetableFormationNos.Add(3);
                }
                skillTargetFormationNos.Add(skillTargetableFormationNos.OrderBy(a => Guid.NewGuid()).First());
                break;
            case SkillRangeType.OneLong:
                if(BattleManager.Instance.CurrentAllyParty[0])
                    skillTargetableFormationNos.Add(0);
                if(BattleManager.Instance.CurrentAllyParty[1])
                    skillTargetableFormationNos.Add(1);
                if(BattleManager.Instance.CurrentAllyParty[2])
                    skillTargetableFormationNos.Add(2);
                if(BattleManager.Instance.CurrentAllyParty[3])
                    skillTargetableFormationNos.Add(3);
                skillTargetFormationNos.Add(skillTargetableFormationNos.OrderBy(a => Guid.NewGuid()).First());
                break;
        }

        List<AbstractCharacterController> skillTargets = new List<AbstractCharacterController>();
        skillTargetFormationNos.ForEach(formationNo => skillTargets.Add(BattleManager.Instance.CurrentAllyParty[formationNo]));

        return skillTargets;
    }
}
