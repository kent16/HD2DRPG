using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyTransition;

public class EventManager : MonoBehaviour
{
    [Header("Hierarchy")]
    // 背景ヒエラルキー
    [SerializeField] private Transform backgroundHierarchy;
    // キャラクターヒエラルキー
    [SerializeField] private Transform characterHierarchy;
    // イベントUIヒエラルキー
    [SerializeField] private Transform eventUIHierarchy;
    public Transform CharacterHierarchy{get{return characterHierarchy;}}
    public Transform EventUIHierarchy{get{return eventUIHierarchy;}}

    // EventManagerインスタンス
    public static EventManager Instance{get; set;}
    // 実行中エグゼキュータ数
    public int ActiveExecutorNum{get; set;}

    // イベント設定
    private EventSetting setting;
    // 生成したキャラクター
    private List<AbstractCharacterController> instantiatedCharacters = new List<AbstractCharacterController>();
    // 現在のイベント番号
    private int eventNo = 0;
    // すべて完了したか
    private bool isAllCompleted = false;

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
        ExecuteEventNode();
    }

    // Update is called once per frame
    void Update()
    {
        // すべて完了済みの場合は中断
        if(isAllCompleted)
        {
            return;
        }

        // 実行中のエグゼキュータがない場合、次のイベントを実行する
        if(ActiveExecutorNum == 0)
        {
            ExecuteEventNode();
        }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void Init()
    {
        // イベント設定取得
        setting = GameManager.Instance.GetEventSetting();
        // 背景初期化
        Instantiate(setting.Background, backgroundHierarchy);
        // キャラクター初期化
        for(int i = 0; i < setting.Characters.Count; i++)
        {
            AbstractCharacterController instantiatedCharacter = Instantiate(setting.Characters[i], 
                                                                            setting.InitCharacterPositions[i], 
                                                                            setting.Characters[i].transform.rotation, 
                                                                            characterHierarchy)
                                                                            .GetComponent<AbstractCharacterController>();
            instantiatedCharacter.InitSprite(setting.CharacterRenderingSetting);
            instantiatedCharacters.Add(instantiatedCharacter);
        }
    }

    /// <summary>
    /// イベントを実行する
    /// </summary>
    private void ExecuteEventNode()
    {
        // すべて完了したか
        if(setting.EventNodes.Count <= eventNo)
        {
            GameManager.Instance.NextStagePhase();
            isAllCompleted = true;

            Debug.Log("All event nodes completed.");
            return;
        }

        // 実行するエグゼキュータを取得
        List<AbstractEventExecutor> executors = setting.EventNodes[eventNo].GetComponents<AbstractEventExecutor>().ToList();
        ActiveExecutorNum = executors.Count;

        // エグゼキュータを実行
        executors.ForEach(executor => executor.Execute());

        eventNo++;
    }

    /// <summary>
    /// キャラクターを取得する
    /// </summary>
    /// <param name="characterNo">キャラクター番号（charactersのListの添え字と対応する）</param>
    /// <returns>キャラクター</returns>
    public AbstractCharacterController GetCharacter(int characterNo)
    {
        return instantiatedCharacters[characterNo];
    }
}
