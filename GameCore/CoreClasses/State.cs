using System;
using dgameincsharp.GameCore.Utility;
using Godot;

[GlobalClass]
public partial class State : Node
{

	public virtual void StateEnter(string previousStateName)
	{
		Loggy.Info("State Entered");
	}

	public virtual void StateExit(string nextStateName)
	{
		Loggy.Info("State Exited");
	}
	
}
