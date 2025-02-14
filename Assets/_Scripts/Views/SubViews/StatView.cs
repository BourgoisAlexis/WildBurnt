using TMPro;
using UnityEngine;

public class StatView : UIElement {
    [SerializeField] private TextMeshProUGUI _tmproHealth;
    [SerializeField] private TextMeshProUGUI _tmproStrength;
    [SerializeField] private TextMeshProUGUI _tmproMagic;


    public void DisplayStats(StatModel statModel) {
        _tmproHealth.text = $"{statModel.CurrentHealth}/{statModel.MaxHealth}";
        _tmproStrength.text = $"{statModel.Strength}";
        _tmproMagic.text = $"{statModel.Magic}";
    }
}
