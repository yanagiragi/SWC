using System;
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
}
