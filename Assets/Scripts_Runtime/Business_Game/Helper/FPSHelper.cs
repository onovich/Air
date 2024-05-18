#if UNITY_EDITOR

using UnityEngine;
namespace Air {

	public static class FPSHelper {
		static float t = 0.0f;

		public static void Tick(float dt) {
			t += (dt - t) * 0.1f;
		}

		public static void OnGUI() {
			int w = Screen.width, h = Screen.height;
			GUIStyle style = new GUIStyle();
			Rect rect = new Rect(0, 0, w, h * 2 / 100);
			style.alignment = TextAnchor.UpperLeft;
			style.fontSize = h * 2 / 100;
			style.normal.textColor = Color.white;
			float msec = t * 1000.0f;
			float fps = 1.0f / t;
			string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
			GUI.Label(rect, text, style);
		}

	}

}
#endif