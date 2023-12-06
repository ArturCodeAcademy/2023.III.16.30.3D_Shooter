using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	[SerializeField] private int _gameSceneIndex;

	private void Awake()
	{
		Cursor.lockState = CursorLockMode.None;
	}

	public void Play()
	{
		SceneManager.LoadScene(_gameSceneIndex);
	}

	public void Quit()
	{
		Application.Quit();
	}
}
