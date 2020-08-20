using Godot;
using System;

namespace MartianDelivery
{
	public class Tooltip : MarginContainer
	{
		protected Label Label { get { return GetNode("CenterContainer").GetNode<Label>("Label"); } }
		protected AnimationPlayer AnimationPlayer { get { return GetNode<AnimationPlayer>("AnimationPlayer"); } }

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			Hide();
		}

		//  // Called every frame. 'delta' is the elapsed time since the previous frame.
		//  public override void _Process(float delta)
		//  {
		//      
		//  }

		public void Show(string text)
		{
			Label.Text = text;
			if (!Visible)
			{			
				Show();
				AnimationPlayer.Play("FadeIn");
			}			
		}

		public void Fade()
		{
			if (Visible)
			{
				AnimationPlayer.Play("FadeOut");			
			}			
		}

		private void _on_AnimationPlayer_animation_finished(string anim_name)
		{
			if (anim_name == "FadeOut")
			{
				Hide();
			}
		}
	}
}



