using UnityEngine;

public class Map : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SpriteRenderer _mapBackground;




    private void Awake()
    {
        Debug.Log($"MapWidth: {GetMapWidth()}, MapHeight: {GetMapHeight()}");
    }

    
    void Update()
    {
        
    }

    public float GetMapWidth() => _mapBackground.bounds.size.x;
    public float GetMapHeight() => _mapBackground.bounds.size.y;
}
