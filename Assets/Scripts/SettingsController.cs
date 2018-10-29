using UnityEngine;
using UnityEngine.UI;

// Governs UI actions of the settings menu
public class SettingsController : MonoBehaviour {

	private const float LERP_FACTOR = 15f;	// Speed of lerp animation
	private const float LERP_MIN = 0.01f;	// Lerp threshold to stop lerping infinitely
	private float _target;					// Alternates between 0<-->1 (unselected<-->selected)

	private RectTransform _menu;		// Reference to the settings menu RectTransform that 'opens'
	private RectTransform _icon;		// Reference to the settings icon RectTransform that rotates on open
	private Image _bg;					// Reference to the gray background when menu is open

	// Runs on scene startup
	void Start () {
		// Set references
		_menu = transform.Find("Menu").GetComponent<RectTransform>();
		_icon = transform.Find("Icon").GetComponent<RectTransform>();
		_bg = transform.Find("Background").GetComponent<Image>();

		_target = 0f;
	}
	
	// Runs every frame
	void Update () {
		// Lerp objects if outside threshold
		if(Mathf.Abs(_menu.localPosition.x - MenuTarget) >= LERP_MIN) {
			_menu.localPosition = new Vector2(Mathf.Lerp(_menu.localPosition.x, MenuTarget, Time.deltaTime * LERP_FACTOR), _menu.localPosition.y);
		}
		if(Mathf.Abs(_icon.eulerAngles.z - IconTarget) >= LERP_MIN) {
			_icon.rotation = Quaternion.Euler(_icon.eulerAngles.x, _icon.eulerAngles.y, Mathf.LerpAngle(_icon.eulerAngles.z, IconTarget, Time.deltaTime * LERP_FACTOR));
		}
		if(Mathf.Abs(_bg.color.a - BGTarget) >= LERP_MIN / 100f) {
			ColorUtil.SetAlpha(_bg, Mathf.Lerp(_bg.color.a, BGTarget, Time.deltaTime * LERP_FACTOR));
		}
	}

	//------------------------------------------------------------------------
	public void ToggleMenu() {
		if(_target == 0f) {	// Open menu
			_target = 1f;
			_bg.raycastTarget = true;
		}
		else {				// Close menu
			_target = 0f;
			_bg.raycastTarget = false;
		}
	}

	//------------------------------------------------------------------------
	private float MenuTarget {
		get {return _target * _menu.sizeDelta.x;}
	}
	private float IconTarget {
		get {return _target * -90f;}
	}
	private float BGTarget {
		get {return _target * 125f / 255f;}
	}
}
