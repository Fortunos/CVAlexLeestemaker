using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SubtitleHandler : MonoBehaviour
{
	public static SubtitleHandler instance;

    public Text subtitleText;
    public Dictionary<String, float> lines = new Dictionary<string, float>();
    public float stayTime = 6f;
    private float currentStayTime;

	// Use this for initialization
	void Awake ()
	{
		instance = this;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    currentStayTime -= Time.deltaTime;
        //List<string> toRemove = new List<string>();
        Dictionary<String, float> newValues = new Dictionary<string, float>();
	    foreach (KeyValuePair<String, float> kvp in lines)
	    {
	        float newTime = lines[kvp.Key] - Time.deltaTime;
	        if (newTime <= 0)
	        {
                subtitleText.gameObject.SetActive(true);
	            currentStayTime = stayTime;
	            subtitleText.text = kvp.Key;
                //lines[kvp.Key] = float.MaxValue;
	            newValues[kvp.Key] = float.MaxValue;
	            //toRemove.Add(kvp.Key);
	        }
	        else
	        {
                //lines[kvp.Key] = newTime;
	            newValues[kvp.Key] = newTime;
	        }
	    }
	    foreach (KeyValuePair<String, float> kvp in newValues)
	    {
	        lines[kvp.Key] = kvp.Value;
	    }
        //foreach (string s in toRemove)
        //{
        //    lines.Remove(s);
        //}
        if (currentStayTime <= 0)
            subtitleText.gameObject.SetActive(false);
	}

    public void AddLines(List<String> texts, List<float> delays)
    {
        for (int i = 0; i < texts.Count; i++)
        {
            lines[texts[i]] = delays[i];
        }
    }
}
