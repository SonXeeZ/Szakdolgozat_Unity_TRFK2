using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FormController : MonoBehaviour
{ // -- FK -- 2023.02.08
    [SerializeField] private Canvas canvas;



    public void Start(){
        canvas = GetComponent<Canvas>();
        OpenSpecificPanel("BasePanel");
    }

    // https://docs.unity3d.com/ScriptReference/Canvas.html
    public void OpenSpecificPanel(string panelName){
        foreach(Transform panel in canvas.GetComponentInChildren<Transform>()){
            if(panel.name == panelName){
                panel.gameObject.SetActive(true);
            } else{
                panel.gameObject.SetActive(false);
            }
            
        }
    }
}
