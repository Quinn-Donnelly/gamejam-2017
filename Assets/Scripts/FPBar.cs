using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class FPBar : MonoBehaviour
{
    private Image foregroundImage;
    private PlayerController player;

    void Start()
    {
        foregroundImage = gameObject.GetComponent<Image>();
        GameObject thePlayer = GameObject.Find("Player");
        player = thePlayer.GetComponent<PlayerController>();
        player.maxHealth = player.maxHealth;
    }

    void Update()
    {
        foregroundImage.fillAmount = (player.GetComponent<PlayerController>().currentFP / player.GetComponent<PlayerController>().maxFP);
    }
}