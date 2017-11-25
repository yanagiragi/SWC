using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManger : ManagerBase<UIManger>
{
	[SerializeField] Image slimeModeImage;
    [SerializeField] Image bloodBar;
    [SerializeField] Image satiationBar;
    [SerializeField] Text FoodInfo;
    [SerializeField] Image ThrowUI;
    [SerializeField] Image CoolDown;
    static public bool isThrowUIOpen = false;

    void Update()
	{
        UpdateAtStep();
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
//        UpdateBloodBar();
//        UpdateSatiation();
//        UpdateFoodInfo();
    }

	public void UpdateSlimeMode()
	{
		Item _item = ItemManager.GetItemData(PlayerManager.instance.slimeMode);
		slimeModeImage.sprite = _item.icon;
	}

    public void UpdateBloodBar()
    {
		Debug.Log ("UpdateBloodBar : " + PlayerManager.instance.satiation);
        bloodBar.fillAmount = PlayerManager.instance.health / 100.0f;
    }

	public void UpdateSatiation()
	{
		Debug.Log ("UpdateSatiation : " + PlayerManager.instance.satiation);
		satiationBar.fillAmount = PlayerManager.instance.satiation / 1000.0f;
    }

	public void UpdateFoodInfo()
	{
		Debug.Log ("UpdateFoodInfo : " + PlayerManager.instance.satiation);
		FoodInfo.text = "食物數量 : " + PlayerManager.instance.food;
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

    public void AbilityCoolDown(float value)
    {
        CoolDown.fillAmount = value;
    }
}
