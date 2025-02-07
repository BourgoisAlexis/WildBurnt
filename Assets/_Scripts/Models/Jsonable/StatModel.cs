using System;

[Serializable]
public struct StatModel {
    public int MaxHealth;
    public int CurrentHealth;
    public int Strength;
    public int Magic;


    public StatModel(int maxHealth, int currentHealth, int strength, int magic) {
        MaxHealth = maxHealth;
        CurrentHealth = currentHealth;
        Strength = strength;
        Magic = magic;
    }

    public static StatModel operator +(StatModel a, StatModel b) {
        StatModel result = new StatModel(
            a.MaxHealth + b.MaxHealth,
            a.CurrentHealth + b.CurrentHealth,
            a.Strength + b.Strength,
            a.Magic + b.Magic
        );

        return result;
    }

    public static StatModel operator -(StatModel a, StatModel b) {
        StatModel result = new StatModel(
            a.MaxHealth - b.MaxHealth,
            a.CurrentHealth - b.CurrentHealth,
            a.Strength - b.Strength,
            a.Magic - b.Magic
        );

        return result;
    }
}
