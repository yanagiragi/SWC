﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManger : ManagerBase<UIManger>
{

    [SerializeField] int blood;
    [SerializeField] int Satiation;
    [SerializeField] int FoodAmount;
    [SerializeField] Image bloodBar;
    [SerializeField] Image satiationBar;
    [SerializeField] Text FoodInfo;
    [SerializeField] Image ThrowUI;
    static public bool isThrowUIOpen = false;

    void Update()
	{
        if(!isThrowUIOpen)
        {
            PlayerManager.instance.isIdle = true;
        }
		/*if(Input.GetKeyDown(KeyCode.C))
		{
            OpenThrowUI();
        }
        if(Input.GetKeyUp(KeyCode.C))
		{
            OpenThrowUI();
        }*/
	}
    void UpdateAtStep()
    {
        UpdateBloodBar();
        UpdateSatiation();
        UpdateFoodInfo();
    }

    public void UpdateBloodBar()
    {
        bloodBar.fillAmount = blood / 100.0f;
    }

	public void UpdateSatiation()
	{
        satiationBar.fillAmount = Satiation / 100.0f;
    }

	public void UpdateFoodInfo()
	{
        FoodInfo.text = "食物數量 : " + FoodAmount;
    }

	public void OpenThrowUI()
	{
        if (!isThrowUIOpen)
        {
            ThrowUI.gameObject.SetActive(true);
            ThrowUI.gameObject.GetComponent<Animator>().SetTrigger("OpenThrowUI");
            isThrowUIOpen = true;
        }
		else
		{
			isThrowUIOpen = false;
            ThrowUI.gameObject.GetComponent<Animator>().SetTrigger("CloseThrowUI");
		}
    }

    public void OpenShopUI()
    {

    }
}
