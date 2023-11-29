using UnityEngine;

public class Player : MonoBehaviour
{
	public static Player Instance { get; private set; }

	private void Awake()
	{
		Instance = this;
		Cursor.lockState = CursorLockMode.Locked;
	}
}