using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CI.QuickSave;

public class SaveManager : MonoBehaviour
{
    // セーブファイル名
    private const string FILE_NAME = "save_data";
    // セーブ項目
    private const string PARTY = "party";
    private const string LEVEL_WARRIOR = "level_warrior";
    private const string LEVEL_ARCHER = "level_archer";
    private const string LEVEL_SISTER = "level_sister";
    private const string LEVEL_KNIGHT = "level_knight";
    private const string LEVEL_MAGICIAN = "level_magician";
    private const string LEVEL_THIEF = "level_thief";
    private const string NEXT_EXP_WARRIOR = "next_exp_warrior";
    private const string NEXT_EXP_ARCHER = "next_exp_archer";
    private const string NEXT_EXP_SISTER = "next_exp_sister";
    private const string NEXT_EXP_KNIGHT = "next_exp_knight";
    private const string NEXT_EXP_MAGICIAN = "next_exp_magician";
    private const string NEXT_EXP_THIEF = "next_exp_thief";

    // SaveManagerインスタンス
    public static SaveManager Instance{get; set;}

    // キャッシュ
    private SaveData cachedSaveData;

    void Awake()
    {
        Instance = this;
        Load();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// キャッシュしているセーブデータを取得する
    /// </summary>
    /// <returns>キャッシュしているセーブデータ</returns>
    public SaveData GetSaveData()
    {
        return cachedSaveData;
    }

    /// <summary>
    /// キャッシュしているセーブデータに設定する
    /// </summary>
    /// <param name="saveData">キャッシュに設定するセーブデータ</param>
    public void SetSaveData(SaveData saveData)
    {
        // パーティ
        if(saveData.Party != null)
            cachedSaveData.Party = saveData.Party;
        // レベル
        if(saveData.LevelWarrior != Constants.Default.INT_NULL)
            cachedSaveData.LevelWarrior = saveData.LevelWarrior;
        if(saveData.LevelArcher != Constants.Default.INT_NULL)
            cachedSaveData.LevelArcher = saveData.LevelArcher;
        if(saveData.LevelSister != Constants.Default.INT_NULL)
            cachedSaveData.LevelSister = saveData.LevelSister;
        if(saveData.LevelKnight != Constants.Default.INT_NULL)
            cachedSaveData.LevelKnight = saveData.LevelKnight;
        if(saveData.LevelMagician != Constants.Default.INT_NULL)
            cachedSaveData.LevelMagician = saveData.LevelMagician;
        if(saveData.LevelThief != Constants.Default.INT_NULL)
            cachedSaveData.LevelThief = saveData.LevelThief;
        // 経験値
        if(saveData.NextExpWarrior != Constants.Default.INT_NULL)
            cachedSaveData.NextExpWarrior = saveData.NextExpWarrior;
        if(saveData.NextExpArcher != Constants.Default.INT_NULL)
            cachedSaveData.NextExpArcher = saveData.NextExpArcher;
        if(saveData.NextExpSister != Constants.Default.INT_NULL)
            cachedSaveData.NextExpSister = saveData.NextExpSister;
        if(saveData.NextExpKnight != Constants.Default.INT_NULL)
            cachedSaveData.NextExpKnight = saveData.NextExpKnight;
        if(saveData.NextExpMagician != Constants.Default.INT_NULL)
            cachedSaveData.NextExpMagician = saveData.NextExpMagician;
        if(saveData.NextExpThief != Constants.Default.INT_NULL)
            cachedSaveData.NextExpThief = saveData.NextExpThief;
    }

    /// <summary>
    /// キャッシュしているセーブデータを保存する
    /// </summary>
    public void Save()
    {
        Write(cachedSaveData);
    }

    /// <summary>
    /// キャッシュしているセーブデータに設定する
    /// </summary>
    public void Load()
    {
        cachedSaveData = Read();
    }

    /// <summary>
    /// セーブファイルを初期化する
    /// </summary>
    public void Init()
    {
        Write(new SaveData(true));
    }

    /// <summary>
    /// セーブファイルに書き込みする
    /// </summary>
    /// <param name="saveData">保存するセーブデータ</param>
    public void Write(SaveData saveData)
    {
        QuickSaveWriter writer = GetWriter();

        // パーティ
        writer.Write(PARTY, saveData.Party);
        // レベル
        writer.Write(LEVEL_WARRIOR, saveData.LevelWarrior);
        writer.Write(LEVEL_ARCHER, saveData.LevelArcher);
        writer.Write(LEVEL_SISTER, saveData.LevelSister);
        writer.Write(LEVEL_KNIGHT, saveData.LevelKnight);
        writer.Write(LEVEL_MAGICIAN, saveData.LevelMagician);
        writer.Write(LEVEL_THIEF, saveData.LevelThief);
        // 経験値
        writer.Write(NEXT_EXP_WARRIOR, saveData.NextExpWarrior);
        writer.Write(NEXT_EXP_ARCHER, saveData.NextExpArcher);
        writer.Write(NEXT_EXP_SISTER, saveData.NextExpSister);
        writer.Write(NEXT_EXP_KNIGHT, saveData.NextExpKnight);
        writer.Write(NEXT_EXP_MAGICIAN, saveData.NextExpMagician);
        writer.Write(NEXT_EXP_THIEF, saveData.NextExpThief);
        // 書き込み
        writer.Commit();

        Debug.Log("Wrote Save Data");
    }

    /// <summary>
    /// セーブファイルを読み込む
    /// </summary>
    /// <returns>セーブデータ</returns>
    public SaveData Read()
    {
        QuickSaveReader reader = GetReader();

        SaveData saveData = new SaveData();
        // パーティ
        saveData.Party = reader.Read<List<AllyType>>(PARTY);
        // レベル
        saveData.LevelWarrior = reader.Read<int>(LEVEL_WARRIOR);
        saveData.LevelArcher = reader.Read<int>(LEVEL_ARCHER);
        saveData.LevelSister = reader.Read<int>(LEVEL_SISTER);
        saveData.LevelKnight = reader.Read<int>(LEVEL_KNIGHT);
        saveData.LevelMagician = reader.Read<int>(LEVEL_MAGICIAN);
        saveData.LevelThief = reader.Read<int>(LEVEL_THIEF);
        // 経験値
        saveData.NextExpWarrior = reader.Read<int>(NEXT_EXP_WARRIOR);
        saveData.NextExpArcher = reader.Read<int>(NEXT_EXP_ARCHER);
        saveData.NextExpSister = reader.Read<int>(NEXT_EXP_SISTER);
        saveData.NextExpKnight = reader.Read<int>(NEXT_EXP_KNIGHT);
        saveData.NextExpMagician = reader.Read<int>(NEXT_EXP_MAGICIAN);
        saveData.NextExpThief = reader.Read<int>(NEXT_EXP_THIEF);

        Debug.Log("Read Save Data");
        return saveData;
    }

    /// <summary>
    /// セーブ設定を取得する
    /// </summary>
    /// <returns>セーブ設定</returns>
    private QuickSaveSettings GetSetting()
    {
        QuickSaveSettings settings = new QuickSaveSettings();
        settings.SecurityMode = SecurityMode.None;
        settings.CompressionMode = CompressionMode.None;
        return settings;
    }

    /// <summary>
    /// QuickSaveWriterを取得する
    /// </summary>
    /// <returns>QuickSaveWriter</returns>
    private QuickSaveWriter GetWriter()
    {
        return QuickSaveWriter.Create(FILE_NAME, GetSetting());
    }

    /// <summary>
    /// QuickSaveReaderを取得する
    /// </summary>
    /// <returns>QuickSaveReader</returns>
    private QuickSaveReader GetReader()
    {
        QuickSaveReader reader;
        try
        {
            reader = QuickSaveReader.Create(FILE_NAME, GetSetting());
        }
        catch(QuickSaveException)
        {
            Init();
            reader = QuickSaveReader.Create(FILE_NAME, GetSetting());
            Debug.Log("Create init save file.");
        }
        return reader;
    }
}
