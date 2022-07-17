using UnityEngine;
using UnityEditor;

static class HeroUnityIntegration {

	[MenuItem("Assets/Create/HeroAsset")]
	public static void CreateYourScriptableObject() {
		ScriptableObjectUtility2.CreateAsset<HeroAsset>();
	}

}
