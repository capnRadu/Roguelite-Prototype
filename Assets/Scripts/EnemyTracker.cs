using System.Collections.Generic;
using UnityEngine;

public class EnemyTracker : MonoBehaviour
{
    public static EnemyTracker Instance {get; private set; }

    private List<Enemy> enemiesInScene = new List<Enemy>();
    private Teleporter teleporter;
    private int bossHpTracker;
    
    private void Awake() 
    { 
        DontDestroyOnLoad(gameObject);
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        } 
        else 
        { 
            Instance = this; 
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // FindObjects();
    }

    public void OnEnemyDeath(int type)
    {
        // if (enemiesInScene.Count == 0)
        // {
        //     FindObjects();
        // }
        if (enemiesInScene.Count > 0)
        {
            enemiesInScene.RemoveAt(0);
        }

        // Debug.Log(enemiesInScene.Count);
        if (enemiesInScene.Count == 0 && teleporter != null)
        {
            teleporter.gameObject.SetActive(true);
            //Debug.Log("Happens");
        }

        if (type == 1)
        {
            bossHpTracker++;
        }
        Debug.Log(bossHpTracker.ToString());
    }

    public void FindObjects()
    {
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        enemiesInScene = new List<Enemy>(enemies);
        // Debug.Log(enemiesInScene.Count);
        
        teleporter = FindAnyObjectByType<Teleporter>();
        // Debug.Log(teleporter);
        if (teleporter != null && enemiesInScene.Count > 0)
        {
            teleporter.gameObject.SetActive(false);
        }
    }

    public int GetBossHpMultiplier()
    {
        return bossHpTracker;
    }
}
