using UnityEngine;

public class HitMagicEffect : MonoBehaviour
{
    private Animator animator;
    private const string triggerName = "StartAttack";
    private const string clipName = "MagicTowerAttackEffect";

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        animator.SetTrigger(triggerName);

        float length = GetClipLengthByName(animator, clipName);
        Invoke(nameof(DisableSelf), length);
    }

    float GetClipLengthByName(Animator anim, string name)
    {
        foreach (var clip in anim.runtimeAnimatorController.animationClips)
        {
            if (clip.name == name)
                return clip.length;
        }

        Debug.LogWarning($"Animation clip '{name}' not found.");
        return 0f;
    }
    void DisableSelf()
    {
        Destroy(gameObject); // ���� ������ ���
        //gameObject.SetActive(false); // Ǯ���� ���ٸ� ��Ȱ��ȭ
    }
}
