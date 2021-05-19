using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteScaler : MonoBehaviour
{
    [SerializeField]
    private float targetAspect = 16f/9f;
    private float offsetScaling = 0f;

    public void Scale(){
        if(Camera.main.aspect < targetAspect){
            offsetScaling = Mathf.Abs(Camera.main.aspect - targetAspect); 
            offsetScaling /=2f;
            this.gameObject.transform.localScale -= new Vector3(offsetScaling, offsetScaling, offsetScaling);
        }
    }
}
