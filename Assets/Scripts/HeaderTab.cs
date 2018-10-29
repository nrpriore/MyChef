using UnityEngine;
using UnityEngine.UI;

// Governs the UI actions of the HeaderTabs
public class HeaderTab : MonoBehaviour {
	public static HeaderTab SelectedTab = null;
	private const string StartingTab = "HomeTab";

	private const float LERP_TO = -50f;		// Distance the tab lerps down when selected
	private const float LERP_FACTOR = 25f;	// Speed of lerp animation
	private const float LERP_MIN = 0.01f;	// Lerp threshold to stop lerping infinitesimally
	private float _target;					// Alternates between 0<-->1 (unselected<-->selected)

	private RectTransform _rt;			// Reference to the RectTransform expanding when selected
	private Image _bg;					// Reference to the background Image changing color when selected

	// Runs on scene startup
	void Start () {
		// Set references
		_rt = GetComponent<RectTransform>();
		_bg = transform.Find("Background").GetComponent<Image>();

		// Select starting tab / init others
		if(gameObject.name == StartingTab) {
			SelectedTab = this;
			_rt.offsetMin = new Vector2(_rt.offsetMin.x, LERP_TO);
			_target = 1f;
			_bg.color = ColorUtil.SELECTED_TAB;
		}
		else {
			_target = 0f;
		}
	}
	
	// Runs every frame
	void Update () {
		// Lerp tab if outside threshold
		if(Mathf.Abs(_rt.offsetMin.y - Target) >= LERP_MIN) {
			_rt.offsetMin = new Vector2(_rt.offsetMin.x, Mathf.Lerp(_rt.offsetMin.y, Target, Time.deltaTime * LERP_FACTOR));
		}
	}
	//------------------------------------------------------------------------
	// Called by button in Unity
	public void SelectTab() {
		if(SelectedTab != null) {
			if(SelectedTab != this) {
				SelectedTab.UnselectTab();
			}
		}
		SelectedTab = this;
		_target = 1f;
		_bg.color = ColorUtil.SELECTED_TAB;
	}

	// Called from SelectTab
	public void UnselectTab() {
		_target = 0f;
		_bg.color = ColorUtil.UNSELECTED_TAB;
	}
	
	//------------------------------------------------------------------------
	private float Target {
		get {return _target * LERP_TO;}
	}
}
