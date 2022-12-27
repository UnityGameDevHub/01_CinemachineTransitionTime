using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class CamsToButton : MonoBehaviour {
	CinemachineStateDrivenCamera _cinemachineStateDrivenCamera;
	Animator _cinemachineStateDrivenCameraAnimator;
	[SerializeField] bool _useCinemachineStateDrivenCamera;
	[SerializeField] GameObject _parentBtn;
	[SerializeField] GameObject _btnPref;
	[SerializeField] GameObject _camlist;

	private void Awake() => CreateButtons();

	private void Update() => PrintLog();

	private void CreateButtons() {
		foreach (Transform child in _camlist.transform) {
			GameObject goBtn = Instantiate(_btnPref, _parentBtn.transform);
			goBtn.name = child.name;
			goBtn.transform.GetChild(0).GetComponent<TMP_Text>().text = child.name;

			if(_useCinemachineStateDrivenCamera) {
				_cinemachineStateDrivenCamera = _camlist.GetComponent<CinemachineStateDrivenCamera>();
				_cinemachineStateDrivenCameraAnimator = _camlist.GetComponent<Animator>();
			}
			
			goBtn.GetComponent<Button>().onClick.AddListener(() => ManageClick(child, child.name));
		}
	}

	private void PrintLog() {
		if(_useCinemachineStateDrivenCamera && _cinemachineStateDrivenCamera.IsBlending) {
			// print($"<b>ActiveBlend -> </b> [ BlendWeight: {_cinemachineStateDrivenCamera.ActiveBlend.BlendWeight} ] [ TimeInBlend : {_cinemachineStateDrivenCamera.ActiveBlend.TimeInBlend} ] [ Duration: {_cinemachineStateDrivenCamera.ActiveBlend.Duration} ]");
		}
	}

	void ManageClick(Transform currentCam, string name) {
		if(!_useCinemachineStateDrivenCamera) {
			DiabelAllCams();

			currentCam.gameObject.SetActive(true);
		} else {
			_cinemachineStateDrivenCameraAnimator.Play(name.Split("_")[1]);
		}
	}

	private void DiabelAllCams() {
		foreach (Transform cam in _camlist.transform)
			cam.gameObject.SetActive(false);
	}
}
