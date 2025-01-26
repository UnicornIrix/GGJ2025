using Godot;
using System;

public partial class VideoStreamPlayerController : Node2D
{
	[Export]
	VideoStreamPlayer videoStreamPlayer;

	[Export]
	Control control;

	[Export]
	AudioStreamPlayer audioStream;

	bool cutSceneIsFinished = false;

	public void OnCutsceneFinished()
	{
		cutSceneIsFinished = true;
		control.Visible = true;
		videoStreamPlayer.Paused = true;
		videoStreamPlayer.Visible = false;
	}

    public override void _Process(double delta)
    {

		if(cutSceneIsFinished)
		{
			if(Input.IsActionJustPressed("shoot"))
			{
				audioStream.Play();
			}
		}
		if(Input.IsActionJustPressed("shoot"))
		{
			OnCutsceneFinished();
		}
    }

	public void OnAudioFinished()
	{
		GetTree().ChangeSceneToFile("res://Assets/World.tscn");
	}
}
