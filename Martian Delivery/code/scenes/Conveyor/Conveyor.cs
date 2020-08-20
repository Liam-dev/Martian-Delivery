using Godot;
using System;

namespace MartianDelivery
{
	public class Conveyor : StaticBody
	{
		public MeshInstance Belt { get { return GetNode<MeshInstance>("Belt"); } }
		public Timer Timer { get { return GetNode<Timer>("Timer"); } }
		public AudioStreamPlayer3D AudioPlayer { get { return GetNode<AudioStreamPlayer3D>("AudioPlayer"); } }

		private bool moving;

		public bool Moving
		{
			get { return moving; }

			set
			{
				moving = value;
				if (value)
				{
					Belt.MaterialOverride = (Material)ResourceLoader.Load("res://code/scenes/Conveyor/ConveyorMoving.tres");
					AudioPlayer.Play();
					Timer.Start();
				}
				else
				{
					Belt.MaterialOverride = (Material)ResourceLoader.Load("res://code/scenes/Conveyor/ConveyorStationary.tres");
					AudioPlayer.Stop();
				}
			}
		}

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			Moving = false;
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(float delta)
		{

		}

		private void _on_Timer_timeout()
		{
			Moving = false;
		}
	}
}
