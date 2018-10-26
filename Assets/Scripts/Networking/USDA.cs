using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public static class USDA {
	
	public static IEnumerator Search(string search, string max = "25") {
		string sort = "r";
		string request = "https://api.nal.usda.gov/ndb/search/?q=" + search + "&max=" + max + "&sort=" + sort + "&api_key=dAKP4kv5SLEpkysNGLL9vV8wLQNwqHeCfhVTKW4k";
		UnityWebRequest www = UnityWebRequest.Get(request);
		yield return www.SendWebRequest();
 
        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {
            Debug.Log(www.downloadHandler.text);
        }
	}

	public static IEnumerator Get(string ndbno, string type = "b") {
		string request = "https://api.nal.usda.gov/ndb/V2/reports?ndbno=" + ndbno + "&type=" + type + "&api_key=dAKP4kv5SLEpkysNGLL9vV8wLQNwqHeCfhVTKW4k";
		UnityWebRequest www = UnityWebRequest.Get(request);
		yield return www.SendWebRequest();
 
        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {
            Debug.Log(www.downloadHandler.text);
        }
	}

}
