using System;

[Serializable]
public struct EntityModel {
    public int Health;
    public int Strength;
    public int Def;

    public EntityModel(int health, int strength, int def) {
        Health = health;
        Strength = strength;
        Def = def;
    }
}
