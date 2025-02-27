using UnityEngine;

public class AnimationRandomStartController : MonoBehaviour
{
    public Animator animator;

    void Start()
    {
        animator.Play(animator.GetCurrentAnimatorStateInfo(0).shortNameHash, 0, Random.Range(0.0f, 1.0f));
    }
}
