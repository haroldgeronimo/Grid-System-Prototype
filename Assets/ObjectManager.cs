using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectManager : MonoBehaviour {
	public Selector selector;
	public void SetSelectedObject(Objects obj){
		selector.selectedObject = obj;
	}
}



