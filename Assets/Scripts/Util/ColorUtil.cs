using UnityEngine;					// Access to Color class
using UnityEngine.UI;				// Access to Image class

// Contains utility methods for Unity Colors
public static class ColorUtil {

	public static readonly Color SELECTED_TAB = HexToColor("EFEFEF");
	public static readonly Color UNSELECTED_TAB = HexToColor("FFFFFF");

	// Used to covert a hex string into a Unity Color
	public static Color HexToColor(string hex) {
		hex = hex.Replace ("0x", ""); 	// In case the string is formatted 0xFFFFFF
		hex = hex.Replace ("#", ""); 	// In case the string is formatted #FFFFFF
		byte a = 255;					// Assume fully visible unless specified in hex
		byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);

		// Only use alpha if the string has enough characters
		if(hex.Length == 8){
			a = byte.Parse(hex.Substring(6,2), System.Globalization.NumberStyles.HexNumber);
		}
		
		return new Color32(r,g,b,a);
	}

	// Used to edit the alpha of a sprite
	public static void SetAlpha(Image image, float value) {
		Color tempColor = image.color;
		tempColor.a = value;
		image.color = tempColor;
	}

}
