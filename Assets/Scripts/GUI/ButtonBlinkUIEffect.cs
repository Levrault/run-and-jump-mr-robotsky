using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
using UnityEngine.UI; 
using UnityEngine.EventSystems; 

/// <summary>
/// Blink effect
/// </summary>
public class ButtonBlinkUIEffect:MonoBehaviour, ISelectHandler, IDeselectHandler {
	public Text text; 

	/// <summary>
	/// Blink when selected
	/// </summary>
	/// <param name="eventData"></param>
	public void OnSelect(BaseEventData eventData) {
		StartCoroutine(Blink()); 
	}

	/// <summary>
	/// UnBlink when deselected
	/// </summary>
	/// <param name="eventData"></param>
	public void OnDeselect(BaseEventData eventData) {
		StopAllCoroutines();
		text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);
	}

	/// <summary>
	/// Blink effect
	/// </summary>
	/// <returns></returns>
	IEnumerator Blink() {
		float transparency = (text.color.a == 0)?1f:0f; 
		text.color = new Color(text.color.r, text.color.g, text.color.b, transparency); 
		yield return new WaitForSeconds(.5f); 
		StartCoroutine(Blink()); 
	}
	
}
