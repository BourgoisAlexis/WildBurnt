using System;

[Serializable]
public struct StatModel {
    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }
    public int Strength { get; private set; }
    public int Magic { get; private set; }


    public StatModel(int maxHealth, int currentHealth, int strength, int magic) {
        MaxHealth = maxHealth;
        CurrentHealth = currentHealth;
        Strength = strength;
        Magic = magic;
    }
}
