using UnityEngine;

public class SkillEffectController : MonoBehaviour
{
    // スキルエフェクトを削除するまでの時間
    [SerializeField] private float duration; 

    // Start is called before the first frame update
    public void Start()
    {
        // 一定時間後に削除
        Destroy(gameObject, duration);
    }

    // Update is called once per frame
    public void Update()
    {

    }
}
