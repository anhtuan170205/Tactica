using UnityEngine;

public class Lifetime : MonoBehaviour
{
    [SerializeField] private float lifeTime = 5f;
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
