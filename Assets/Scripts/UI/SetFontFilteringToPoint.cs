using UnityEngine;
using UnityEngine.UI;
 
[ExecuteInEditMode]
public class SetFontFilteringToPoint : MonoBehaviour
{
	void Start()
	{
		GetComponent<Text> ().font.material.mainTexture.filterMode = FilterMode.Point;
 
	}
}
