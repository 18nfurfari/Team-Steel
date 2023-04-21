using UnityEngine;

public class SpawnPointManager : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnPoints;

    public static SpawnPointManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    public GameObject[] GetSpawnPoints()
    {
        return spawnPoints;
    }
}
