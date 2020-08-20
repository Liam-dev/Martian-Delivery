using Godot;
using System;

namespace MartianDelivery
{
	public class Button : Spatial, ISelectable
	{
		[Export(PropertyHint.MultilineText)] public string TooltipDescription { get; set; }

		// Child Nodes

		public AnimationPlayer AnimationPlayer { get { return GetNode<AnimationPlayer>("AnimationPlayer"); } }
		public AudioStreamPlayer3D AudioPlayer { get { return GetNode<AudioStreamPlayer3D>("AudioPlayer"); } }

		[Signal] public delegate void Pressed();

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{

		}

		//  // Called every frame. 'delta' is the elapsed time since the previouws frame.
		//  public override void _Process(float delta)
		//  {
		//      
		//  }

		public void Interact(Player player)
		{
			if (!AnimationPlayer.IsPlaying())
			{
				EmitSignal(nameof(Pressed));
				AnimationPlayer.Play("Pressed");
				AudioPlayer.Play();
			}
		}
	}
}
