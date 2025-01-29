using System;

[Serializable]
public struct EntityData {
    public int Health;
    public int Strength;
    public int Def;

    public EntityData(int health, int strength, int def) {
        Health = health;
        Strength = strength;
        Def = def;
    }
}
