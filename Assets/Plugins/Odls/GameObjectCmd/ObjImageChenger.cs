using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[AddComponentMenu("Odls Obj Cmd/Image Chenger")]
public class ObjImageChenger : ObjCmdArgBase<Sprite> {
	[NullAlarm]public Image image;
	[Header("預設值")]
	public Sprite sprite;
	Sprite lastSprite;
	override protected bool CmdDefault(){
		return Cmd (sprite);
	}
	override protected bool Cmd(Sprite p_arg){
		lastSprite = image.sprite;

		if (lastSprite.Equals(p_arg)) {
			return false;
		} else {
			image.sprite = p_arg;
			return true;
		}
	}
	public override void Reset(){
		image.sprite = lastSprite;
	}
	[Button]public string setAsNowPosBut = "SetAsNowSprite";
	public void SetAsNowSprite(){
		sprite = image.sprite;
	}

	public void SetSprite(Sprite p_sprite){
		sprite = p_sprite;
	}
}