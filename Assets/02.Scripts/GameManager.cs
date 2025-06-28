using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
	protected override void Start()
	{
		base.Start();
		Application.targetFrameRate = 60;

	}

	
}
