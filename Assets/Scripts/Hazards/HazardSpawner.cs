using System.Collections.Generic;
using UnityEngine;

public class HazardSpawner : MonoBehaviour
{
    [Header("Hazards")]
    public GameObject Player;
    [SerializeField] private List<GameObject> hazardPrefabs = new();
    [SerializeField] public  List<Transform> hazardPoints = new();

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            SpawnHazard();
        }
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpawnHazard(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) 
        { 
            SpawnHazard(1);
        }
    }

    public void SpawnHazard(int index = 0)
    {
        int hazardIndex = 0;
        if (index == 0)
        {
            hazardIndex = Random.Range(0, hazardPrefabs.Count);
        }
        else
        {
            hazardIndex = index;
        }
        
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
        else // Check the next hazard, blackhole
        {
            BlackHole blackHole = spawn.GetComponent<BlackHole>();
            if (blackHole != null)
            {
                blackHole.Player = Player;
                blackHole.Target = end;
                blackHole.StartHazard();
            }
        }
    }
}
