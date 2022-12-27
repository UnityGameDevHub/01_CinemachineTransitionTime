using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class CamsToButton : MonoBehaviour {
	[SerializeField] CinemachineBrain _cameraBrain;
	[SerializeField] GameObject _parentBtn;
	[SerializeField] GameObject _btnPref;
	[SerializeField] GameObject _camlist;

	Coroutine _blendCoroutine = null;

	private void Awake() => CreateButtons();

	private void Update() => PrintLog();

	private void CreateButtons() {
		foreach (Transform child in _camlist.transform) {
			GameObject goBtn = Instantiate(_btnPref, _parentBtn.transform);
			goBtn.name = child.name;
			goBtn.transform.GetChild(0).GetComponent<TMP_Text>().text = child.name;

			goBtn.GetComponent<Button>().onClick.AddListener(() => ManageClick(child, child.name));
		}

		_cameraBrain.m_CameraActivatedEvent.AddListener((ICinemachineCamera a, ICinemachineCamera b) => {
			print($" ---> active {a.Name}");
			// WaitForCinemachineBlend();
		});
	}

	private void PrintLog() {
		if(_cameraBrain && _cameraBrain.IsBlending) {
			print($"<b>ActiveBlend -> </b> [ BlendWeight: {_cameraBrain.ActiveBlend.BlendWeight} ] [ TimeInBlend : {_cameraBrain.ActiveBlend.TimeInBlend} ] [ Duration: {_cameraBrain.ActiveBlend.Duration} ]");
		}		
	}

	
	void WaitForCinemachineBlend() {
		if(_blendCoroutine == null)
			_blendCoroutine = StartCoroutine(WaitForCinemachineBlendCourutien());
	}

	IEnumerator WaitForCinemachineBlendCourutien() {
		yield return null;
		if (_cameraBrain.IsBlending && _blendCoroutine != null) {
			StartCoroutine(WaitForCinemachineBlendCourutien());
		} else {
			_blendCoroutine = null;
			OnCompleteBlend(_cameraBrain.ActiveVirtualCamera.Name);
		}
	}

	private void OnCompleteBlend(string endCameraName) {
		print($"<b> ---> OnCompleteBlend -- endCameraName: {endCameraName}</b>");
	}

	void ManageClick(Transform currentCam, string name) {
		WaitForCinemachineBlend();
		DiabelAllCams();

		currentCam.gameObject.SetActive(true);
	}

	private void DiabelAllCams() {
		foreach (Transform cam in _camlist.transform)
			cam.gameObject.SetActive(false);
	}
}
