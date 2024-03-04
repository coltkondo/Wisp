using UnityEngine;

public class DestroyAfterAnimation : MonoBehaviour
{
    public float delay =  0.2f; // Adjust this to match the length of your animation

    void Start()
    {
        Destroy(gameObject, delay);
    }
}
