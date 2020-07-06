using UnityEngine;

public class SkillsPanel : MonoBehaviour {

    #region Singleton
    public static SkillsPanel instance;

    private void Awake() {
        if (instance != null) {
            Debug.LogError("More than one instance of SkillsPanel found!");
            return;
        }
        instance = this;
    }
    #endregion

    [SerializeField] SkillPanelItem[] items;

    UnitSkills skills;

    public void SetSkills(UnitSkills skills) {
        this.skills = skills;
        for (int i = 0; i < items.Length; i++) {
            items[i].SetSkill(i < skills.Count ? skills[i] : null);
        }
    }

    private void Update() {
        if (skills != null) {
            bool inCast = skills.inCast;
            for (int i = 0; i < skills.Count && i < items.Length; i++) {
                items[i].SetCastTime(skills[i].castDelay);
                items[i].SetHolder(inCast || skills[i].castDelay > 0 || skills[i].cooldownDelay > 0);
            }
        }
    }
}
