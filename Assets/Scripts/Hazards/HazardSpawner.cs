using System.Collections.Generic;
using UnityEngine;

public class HazardSpawner : MonoBehaviour
{
    [Header("Hazards")]
    [SerializeField] private List<GameObject> hazardPrefabs = new();
    [SerializeField] public  List<Transform> hazardPoints = new();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            SpawnHazard();
        }
    }

    public void SpawnHazard()
    {
        int hazardIndex = Random.Range(0, hazardPrefabs.Count);
        GameObject hazard = hazardPrefabs[hazardIndex];

        int startIndex = Random.Range(0, hazardPoints.Count);
        int endIndex = 0;

        // Select a random end point that needs to be different fromt the start point
        do
        {
            endIndex = Random.Range(0, hazardPoints.Count);
        } while (endIndex == startIndex);
        Transform start = hazardPoints[startIndex];
        Transform end = hazardPoints[endIndex];

        var spawn = Instantiate(hazard, start.position, Quaternion.identity);

        CometHazard comet = spawn.GetComponent<CometHazard>();
        if (comet != null) // If we spawn in a comet, we need to set its end point
        {
            comet.TargetPoint = end;
            StartCoroutine(comet.CometStart());
        }
    }
}
