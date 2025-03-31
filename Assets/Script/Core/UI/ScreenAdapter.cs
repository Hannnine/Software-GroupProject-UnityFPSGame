using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ScreenAdapter : MonoBehaviour {
	[Header("Display Settings")]
	[SerializeField] Vector2Int defaultResolution = new Vector2Int(2560, 1440);
	[SerializeField] Vector2Int minResolution = new Vector2Int(1280, 720);
	[SerializeField] float targetAspect = 1.7778f;
	[SerializeField] bool startFullscreen = true;

	private Camera mainCamera;
	private Resolution[] supportedResolutions;

	void Start() {
		mainCamera = GetComponent<Camera>();
		InitializeDisplay();
		UpdateAspectRatio();
	}

	void InitializeDisplay() {
		supportedResolutions = Screen.resolutions;
		Screen.SetResolution(
				Mathf.Clamp(defaultResolution.x, minResolution.x, Screen.currentResolution.width),
				Mathf.Clamp(defaultResolution.y, minResolution.y, Screen.currentResolution.height),
				startFullscreen);
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.F11))
			ToggleFullscreen();
		if (!Screen.fullScreen)
			CheckWindowSize();
	}

	void CheckWindowSize() {
		if (Screen.width < minResolution.x || Screen.height < minResolution.y) {
			Screen.SetResolution(
					Mathf.Max(Screen.width, minResolution.x),
					Mathf.Max(Screen.height, minResolution.y),
					false);
		}
	}

	void ToggleFullscreen() {
		Screen.fullScreen = !Screen.fullScreen;
		if (!Screen.fullScreen)
			UpdateAspectRatio();
	}

	void UpdateAspectRatio() {
		float currentAspect = (float)Screen.width / Screen.height;
		float scaleValue = currentAspect / targetAspect;

		Rect cameraRect = mainCamera.rect;

		if (scaleValue < 1.0f) {
			cameraRect.height = scaleValue;
			cameraRect.y = (1.0f - scaleValue) * 0.5f;
		}
		else {
			cameraRect.width = 1.0f / scaleValue;
			cameraRect.x = (1.0f - cameraRect.width) * 0.5f;
		}

		mainCamera.rect = cameraRect;
	}
}