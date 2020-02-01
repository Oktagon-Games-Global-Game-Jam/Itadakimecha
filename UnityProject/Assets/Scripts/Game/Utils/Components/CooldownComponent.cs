using Unity.Entities;

public struct C_CooldownComponent : IComponentData
{
    public float DeltaTime;
    public int Cooldown;
}

public struct TC_CooldownCompleted : IComponentData {}
