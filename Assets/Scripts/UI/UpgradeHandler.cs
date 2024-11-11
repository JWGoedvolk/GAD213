using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeHandler : MonoBehaviour
{
    [Header("Player References")]
    public Rigidbody2D RB;
    public FloatVarScriptable Speed;
    public FloatVarScriptable MaxSpeed;

    [Header("UI")]
    public GameObject UpgradePanel;
    public List<UpgradeButton> Buttons;
    public List<string> UpgradeNames;
    public List<int> UpgradeIDsChosen;
    public int IDChosen = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            HideUpgrades(true);
        }
    }

    public void SetUpgrades()
    {
        // Reset and set button 1
        UpgradeIDsChosen.Clear();
        
        // Set All the upgrades
        for (int i = 0; i < Buttons.Count; i++)
        {
            do
            {
                IDChosen = Random.Range(1, UpgradeNames.Count);
                Debug.Log($"{i}: {IDChosen}");
            } while (UpgradeIDsChosen.Contains(IDChosen));
            UpgradeIDsChosen.Add(IDChosen);
            Buttons[i].UpgradeID = IDChosen;
            Buttons[i].gameObject.SetActive(true);
        }
    }
    public void HideUpgrades(bool state = false)
    {
        if (state) SetUpgrades();
        for (int i = 0; i < Buttons.Count; i++)
        {
            Buttons[i].gameObject.SetActive(state);
        }
        UpgradePanel.SetActive(state);
    }

    public void Upgrade(int upgradeID)
    {
        switch (upgradeID)
        {
            case 0:
                break;
            case 1:
                RB.mass += 0.5f;
                break;
            case 2:
                RB.mass -= 0.5f;
                break;
            case 3:
                RB.drag -= 0.1f;
                break;
            case 4:
                RB.drag += 0.1f;
                break;
            case 5:
                Speed.Value += 0.5f;
                break;
            case 6:
                Speed.Value -= 0.5f;
                break;
            case 7:
                MaxSpeed.Value += 0.5f;
                break;
            case 8:
                MaxSpeed.Value -= 0.5f;
                break;
            default:
                break;
        }
    }
}
