using Godot;

namespace Temptica.GodotExtensions.Abstracts;

[GlobalClass]
public abstract partial class State : Node
{
    public StateMachine StateMachine = null!;

    public new virtual void Ready()
    {
        StateMachine = GetParent<StateMachine>();
    }
    public virtual void Enter(){}
    public virtual void Exit(){}
    public virtual void Update(float delta){}
    public virtual void PhysicsUpdate(float delta){}
    public virtual void ProcessInput(InputEvent inputEvent){}
}