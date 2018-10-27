using UnityEngine;					// To inherit from MonoBehaviour 
using UnityEngine.UI;				// To access Unity UI classes
using System.Collections.Generic;	// For lists

// Builds and governs tab functionality on the menu
public class MenuTabController : MonoBehaviour {

	// Constants
	public const float 	WIDTH_RATIO 	= 0.5f;		// Selected tab is this percent larger
	private const float LERP_MULT		= 25f;		// Speed multiplier of Lerp
	private const float LERP_THRESHOLD	= 0.25f;	// Threshold distance where we're close enough to stop Lerping

	// References for Lerping
	private List<RectTransform> _menuTabs;			// List of transforms of tabs
	private RectTransform 		_currentTab;		// Transform of currently selected tab
	private List<RectTransform> _skippedTabs;		// List of transforms of tabs skipped by manual selection (ex 1 -> 3)
	private Vector2[] 			_lerpWidthTo;		// Array of amounts to lerp each tab's width to
	private Vector2[] 			_lerpPosTo;			// Array of amounts to lerp each tab's position to
	private bool 				_lerp;				// Should we be lerping? True = yes

	// For tab size manipulation
	private float _screenWidth;						// Width of canvas reference resolution
	private float _baseWidth;						// Width of unselected tab
	private float _tabWidthDiff;					// Difference in width of non/selected tabs calculated via WIDTH_RATIO
	private ScrollSnapRect _scrollSnap;				// Reference to scrollsnap to get the starting page


	// Runs on scene startup
	void Awake() {
		InitVars();
	}

	// Runs every frame
	void Update() {
		if(_lerp) {
			// Update every tab's position & size
			for(int i = 0; i < _menuTabs.Count; i++) {
				RectTransform tab = _menuTabs[i];
				tab.anchoredPosition = Vector2.Lerp(tab.anchoredPosition,_lerpPosTo[i],Time.deltaTime * LERP_MULT);
				tab.sizeDelta = Vector2.Lerp(tab.sizeDelta,_lerpWidthTo[i],Time.deltaTime * LERP_MULT);
			}
			// If within theshold, stop lerping
			if(Vector2.SqrMagnitude(_currentTab.sizeDelta - _lerpWidthTo[_menuTabs.IndexOf(_currentTab)]) < LERP_THRESHOLD) {
				_lerp = false;
			}
		}
	}

	// Initialize variables
	private void InitVars() {
		_scrollSnap = GameObject.Find("ScrollSnap").GetComponent<ScrollSnapRect>();
		GameObject tabs = GameObject.Find("MenuTabs");
		_screenWidth = GameObject.Find("Canvas").GetComponent<CanvasScaler>().referenceResolution.x;
		_baseWidth = _screenWidth/(tabs.transform.childCount + WIDTH_RATIO);
		_tabWidthDiff = _baseWidth * WIDTH_RATIO;
		_lerpWidthTo = new Vector2[tabs.transform.childCount];
		_lerpPosTo = new Vector2[tabs.transform.childCount];

		float _currX = 0;
		float _x = 0;
		_menuTabs = new List<RectTransform>();
		_skippedTabs = new List<RectTransform>();
		// Set width and position arrays
		for(int i = 0; i < tabs.transform.childCount; i++) {
			RectTransform child = tabs.transform.GetChild(i).gameObject.GetComponent<RectTransform>();
			_menuTabs.Add(child);
			_x = (_scrollSnap.startingPage == i)? _baseWidth + _tabWidthDiff : _baseWidth;
			child.sizeDelta = new Vector2(_x, child.rect.size.y);
			child.anchoredPosition = new Vector2(_currX, 0);
			_lerpWidthTo[i] = new Vector2(_x, 0);
			_lerpPosTo[i] = new Vector2(_currX, 0);
			_currX += _x;
		}
	}

	// Set intended tab index
	public void SetTab(int tabIndex) {
		_lerp = true;
		int currIndex = (_currentTab != null)? _menuTabs.IndexOf(_currentTab) : tabIndex;
		int diff = tabIndex - currIndex;

		// If there are skipped tabs (ex tab 1-> 3)
		if(Mathf.Abs(diff) > 1) {
			for(int i = 1; i < Mathf.Abs(diff); i++) {
				_skippedTabs.Add(_menuTabs[currIndex + (i * (diff/Mathf.Abs(diff)))]);
			}
		}

		// Arbitrary math. I know, infuriating, isn't it...
		if(_currentTab != null ) {
			if(tabIndex < currIndex) {
				SetPivot(_currentTab, Vector2.right);
			}else {
				SetPivot(_currentTab, Vector2.zero);
			}
		}
		if(tabIndex <= currIndex) {
			SetPivot(_menuTabs[tabIndex], Vector2.zero);
		}else {
			SetPivot(_menuTabs[tabIndex], Vector2.right);
		}
		_currentTab = _menuTabs[tabIndex];

		// Adjust width and position arrays
		for(int i = 0; i < _menuTabs.Count; i++) {
			RectTransform tab = _menuTabs[i];
			_lerpWidthTo[i] = (tab == _currentTab)? new Vector2(_baseWidth + _tabWidthDiff,tab.rect.size.y) : new Vector2(_baseWidth,tab.rect.size.y);
			if(_skippedTabs.Contains(tab)) {
				_lerpPosTo[i] = (_menuTabs.IndexOf(tab) > _menuTabs.IndexOf(_currentTab))? new Vector2(_lerpPosTo[i].x + _tabWidthDiff,_lerpPosTo[i].y) : new Vector2(_lerpPosTo[i].x - _tabWidthDiff,_lerpPosTo[i].y);
			}
		}
		_skippedTabs.Clear();
	}

	// Resets pivot of gameobject and adjusts x & y to make it appear like it hasn't moved
	private void SetPivot(RectTransform rectTransform, Vector2 pivot) {
         if (rectTransform == null) return;
         float _x = (_currentTab == rectTransform)? _baseWidth + _tabWidthDiff : _baseWidth;
         Vector2 deltaPivot = rectTransform.pivot - pivot;
         Vector3 deltaPosition = new Vector3(deltaPivot.x * _x, 0);
         rectTransform.pivot = pivot;
         rectTransform.localPosition -= deltaPosition;
         for(int i = 0; i < _menuTabs.Count; i++) {
         	RectTransform rect = _menuTabs[i];
         	if(rect == rectTransform) {
         		_lerpPosTo[i] -= (Vector2)deltaPosition;
         	}
         }
     }

}
