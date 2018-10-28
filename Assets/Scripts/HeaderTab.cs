using UnityEngine;
using UnityEngine.UI;

// Governs the UI actions of the HeaderTabs
public class HeaderTab : MonoBehaviour {
	public static HeaderTab SelectedTab = null;
	private const string StartingTab = "HomeTab";

	private const float LERP_TO = -50f;
	private const float LERP_MIN = 0.01f;
	private float _target;

	private RectTransform _rt;

	// Use this for initialization
	void Start () {
		_rt = GetComponent<RectTransform>();

		if(gameObject.name == StartingTab) {
			SelectedTab = this;
			_rt.offsetMin = new Vector2(_rt.offsetMin.x, LERP_TO);
			_target = 1f;
		}
		else {
			_target = 0;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Mathf.Abs(_rt.offsetMin.y - Target) >= LERP_MIN) {
			_rt.offsetMin = new Vector2(_rt.offsetMin.x, Mathf.Lerp(_rt.offsetMin.y, Target, Time.deltaTime * 25f));
		}
	}

	public void SelectTab() {
		if(SelectedTab != null) {
			if(SelectedTab != this) {
				SelectedTab.UnselectTab();
			}
		}
		SelectedTab = this;
		_target = 1f;
	}

	public void UnselectTab() {
		_target = 0;
	}

	private float Target {
		get {return _target * LERP_TO;}
	}
}
