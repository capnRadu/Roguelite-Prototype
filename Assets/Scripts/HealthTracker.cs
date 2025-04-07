using UnityEngine;

public class HealthTracker : MonoBehaviour
{
    public static HealthTracker Instance { get; private set; }

    [HideInInspector] public float currentLoopHealth;
    
    private void Awake() 
    { 
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }
}
