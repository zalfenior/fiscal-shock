﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FeedbackController : MonoBehaviour
{
    public static Canvas HUD;
    private static TextMeshProUGUI clone;
    public TextMeshProUGUI shotLoss;
    public TextMeshProUGUI earn;
    public TextMeshProUGUI temp;
    public Image hitVignette;

    public void Start() {
        hitVignette.enabled = false;
    }

    public void shoot(int cost) {
        HUD = GameObject.FindGameObjectWithTag("HUD").GetComponent<Canvas>();
        temp = shotLoss;
        TextMeshProUGUI clone = Object.Instantiate(shotLoss);

        clone.transform.SetParent(HUD.transform);
        clone.text = "-" + (cost.ToString());
        clone.transform.localPosition = new Vector3(0,0,0);
        clone.transform.Translate(Random.Range(-10.6f, 10.0f),  Random.Range(-10.1f, 10.0f), Random.Range(-10.0f, 10.0f), Space.Self);

        Destroy(clone.gameObject, 2f);
    }

    public void profit(float amount) {
        HUD = GameObject.FindGameObjectWithTag("HUD").GetComponent<Canvas>();
        temp = earn;
        TextMeshProUGUI clone = Object.Instantiate(earn);

        clone.transform.SetParent(HUD.transform);
        clone.text = "+" + (amount.ToString());
        clone.transform.localPosition = new Vector3(0,0,0);
        clone.transform.Translate(Random.Range(160.6f, 170.0f),  Random.Range(-10.1f, 10.0f), Random.Range(-10.0f, 10.0f), Space.Self);

        Destroy(clone.gameObject, 2f);
    }

}
