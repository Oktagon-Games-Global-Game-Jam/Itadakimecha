using Unity.Entities;

[GenerateAuthoringComponent]
public struct JumpComponentData : IComponentData
{
    public int jumpForce;
}
