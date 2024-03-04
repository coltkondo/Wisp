using UnityEngine;

public class PlayAnimationOnStart : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null) {
            Debug.LogError("Animator component not found on the prefab!");
            return;
        }

        // Assuming "SmokeTrailAnimation" is the name of your animation state
        animator.Play("SmokeTrailAnimation");
    }
}
