using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : ManagerBase<MaterialManager>
{

	public Material milk, oil, butter, acid, yogurt, poison;

	void change()
	{
		if (PlayerManager.instance.slimeMode == Item.ItemType.milk) 
		{
			GetComponent<Renderer> ().material = milk;
		}
		if (PlayerManager.instance.slimeMode  == Item.ItemType.oil) 
		{
			GetComponent<Renderer> ().material =  oil;
		}
		if (PlayerManager.instance.slimeMode == Item.ItemType.butter) 
		{
			GetComponent<Renderer> ().material =  butter;
		}
		if (PlayerManager.instance.slimeMode== Item.ItemType.acid) 
		{
			GetComponent<Renderer> ().material = acid;
		}
		if (PlayerManager.instance.slimeMode == Item.ItemType.yogurt) 
		{
			GetComponent<Renderer> ().material =yogurt;
		}
		if (PlayerManager.instance.slimeMode == Item.ItemType.poison) 
		{
			GetComponent<Renderer> ().material = poison;
		}



	}
}
