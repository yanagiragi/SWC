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
	[SerializeField] Image ThrowImage1;
	[SerializeField] Image ThrowImage2;
    [SerializeField] Image CoolDown;
	[SerializeField] Image HurtEffect;
	[SerializeField] Image GameOverEffect;
    static public bool isThrowUIOpen = false;

    void Update()
	{
        UpdateAtStep();
//        if(!isThrowUIOpen)
//        {
//            PlayerManager.instance.isIdle = true;
//        }

//		if (isThrowUIOpen) {
//			if(Input.GetKeyDown(KeyCode.A)){
//				
//	        }
//
//			if(Input.GetKeyDown(KeyCode.D)){
//				
//	        }
//		}
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
//		Debug.Log ("UpdateBloodBar : " + PlayerManager.instance.satiation);
        bloodBar.fillAmount = PlayerManager.instance.health / 100.0f;
    }

	public void UpdateSatiation()
	{
//		Debug.Log ("UpdateSatiation : " + PlayerManager.instance.satiation);
		satiationBar.fillAmount = PlayerManager.instance.satiation / 1000.0f;
    }

	public void UpdateFoodInfo()
	{
//		Debug.Log ("UpdateFoodInfo : " + PlayerManager.instance.satiation);
		FoodInfo.text = "食物數量 : " + PlayerManager.instance.food;
    }

    public IEnumerator HurtEffectCoroutine()
    {
        HurtEffect.color = new Color(HurtEffect.color.r, 0, 0, 0.5f);
        yield return new WaitForSeconds(0.05f);
        HurtEffect.color = new Color(HurtEffect.color.r, 0, 0, 0f);
    }

    public void PlayHurtEffect()
    {
        StartCoroutine(HurtEffectCoroutine());
    }

	public void OpenThrowUI()
	{
		if (!isThrowUIOpen) {
			ThrowUI.gameObject.SetActive (true);
			Item _item1 =  ItemManager.GetItemData (PlayerManager.instance.subMode1);
			Item _item2 =  ItemManager.GetItemData (PlayerManager.instance.subMode2);
			ThrowImage1.sprite = _item1.icon;
			ThrowImage2.sprite = _item2.icon;
			ThrowUI.gameObject.GetComponent<Animator> ().SetTrigger ("OpenThrowUI");
			isThrowUIOpen = true;
		}
	}
	public void CloseThrowUI()
	{
		if (isThrowUIOpen) {
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

	public static void StartGameOver(){
		instance.GameOverEffect.gameObject.SetActive (true);
	}

	public static void GameOverDone(){
		instance.GameOverEffect.gameObject.SetActive (false);
	}
}
