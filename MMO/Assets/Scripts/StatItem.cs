using UnityEngine;
using UnityEngine.UI;

public class StatItem : MonoBehaviour {
    
    [SerializeField] Text value;

    public void ChangeStat(int stat) {
        value.text = stat.ToString();
    }

    // кнопка апгрейда стата
    [SerializeField] Button upgradeButton;

    // установка активности кнопки апгрейда
    public void SetUpgradable(bool upgradable)
    {
        upgradeButton.gameObject.SetActive(upgradable);
    }

}
