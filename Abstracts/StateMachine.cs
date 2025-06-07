using Godot;

namespace Temptica.GodotExtensions.Abstracts;
[GlobalClass]
public partial class StateMachine : Node
{
    public Node3D Parent => GetParent<Node3D>();
    public Dictionary<string,State> States { get; } = [];
    
    [Export] public State CurrentState;
    public State PreviousState;

    public override void _Ready()
    {
        var states = GetChildren().OfType<State>().ToList();
        foreach (var state in states)
        {
            state.StateMachine = this;
            States.Add(state.Name, state);
            state.Ready();
        }
        
        CurrentState.Enter();
    }
    
    public override void _Process(double delta)
    {
        CurrentState.Update((float)delta);
    }
    
    public override void _PhysicsProcess(double delta)
    {
        CurrentState.PhysicsUpdate((float)delta);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        CurrentState.ProcessInput(@event);
    }
    
    public void ChangeState(string newState)
    {
        if (!States.TryGetValue(newState, out var value))
        {
            GD.PrintErr($"State '{newState}' does not exist in the state machine.");
            return;
        }

        PreviousState = CurrentState;
        CurrentState.Exit();
        CurrentState = value;
        CurrentState.Enter();
        
        GD.Print($"Changed state to: {CurrentState.Name}");
    }
}