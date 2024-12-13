using SAE.EventSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace SAE.Upgrades
{
    /// <summary>
    /// Handles setting the UpgradeIDs on the upgrade buttons. Also does the same for weapon and bullet switching
    /// </summary>
    public class UpgradeHandler : MonoBehaviour
    {
        [Header("UI")]
        public GameObject UpgradePanel;
        public List<UpgradeButton> Buttons;
        public List<string> UpgradeNames;
        public List<int> UpgradeIDsChosen;
        public int IDChosen = 0;
        public bool isLeveledUp = false;
        public GameEventScriptable UnPause;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                ShowUpgrades();
            }
        }

        /// <summary>
        /// Sets the UpgradeIDs in the list of buttons. These IDs do not repeat in the chosen IDs
        /// </summary>
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
                    //Debug.Log($"{i}: {IDChosen}");
                } while (UpgradeIDsChosen.Contains(IDChosen) || IDChosen == 17 || IDChosen == 18);
                UpgradeIDsChosen.Add(IDChosen);
                Buttons[i].UpgradeID = IDChosen;
                Buttons[i].gameObject.SetActive(true);
            }
        }
        /// <summary>
        /// This controlls whether the upgrade panel is hidden or visible
        /// </summary>
        /// <param name="state">bool => the state to set it to</param>
        public void ShowUpgrades()
        {
            if (!isLeveledUp)
            {
                for (int i = 0; i < Buttons.Count; i++)
                {
                    Buttons[i].gameObject.SetActive(false);
                }
                UpgradePanel.SetActive(false);
                UnPause.Raise();
            }

            if (isLeveledUp)
            {
                SetUpgrades();
                isLeveledUp = false;
                for (int i = 0; i < Buttons.Count; i++)
                {
                    Buttons[i].gameObject.SetActive(true);
                }
                UpgradePanel.SetActive(true);
            }
        }

        public void Skip()
        {
            isLeveledUp = false;
            UpgradePanel.SetActive(false);
        }
    } 
}
