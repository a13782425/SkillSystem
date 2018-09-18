using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    public TextAsset text1;

	// Use this for initialization
	void Start () {
        TSSkill.SkillSystem.Instance.Init(text1.text);
        TSSkill.SkillSystem.Instance.CreateSkillEntity(1001, 1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}


}
