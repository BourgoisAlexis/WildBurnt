using System;

[Serializable]
public struct EntityModel {
    public int Health;
    public int Strength;
    public int Magic;


    public EntityModel(int health, int strength, int magic) {
        Health = health;
        Strength = strength;
        Magic = magic;
    }
}
