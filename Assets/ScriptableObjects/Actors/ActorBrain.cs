using UnityEngine;

[CreateAssetMenu]
public abstract class ActorBrain : ScriptableObject
{
    public virtual void Initialize() { }
    public abstract void Think();
}
