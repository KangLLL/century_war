using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewbieCommonHelper  
{
 	public static void ChangeAllSpritesColor(float percentage, List<tk2dBaseSprite> exclude)
	{
		var sprites = GameObject.FindObjectsOfType(typeof(tk2dBaseSprite));
		foreach (var sp in sprites) 
		{
			tk2dBaseSprite sprite = (tk2dBaseSprite)sp;
			if(exclude == null || !exclude.Contains(sprite))
			{
				NewbieCommonHelper.ChangeSpriteColor(sprite,percentage);
			}
		}
	}
	
	public static void ChangeAllUIColor(float percentage, List<UISprite> exclude)
	{
		var sprites = GameObject.FindObjectsOfType(typeof(UISprite));
		foreach (var sp in sprites) 
		{
			UISprite sprite = (UISprite)sp;
			if(exclude == null || !exclude.Contains(sprite))
			{
				NewbieCommonHelper.ChangeUIColor(sprite,percentage);
			}
		}
	}
	
	public static void ChangeAllLabelColor(float percentage, List<UILabel> exclude)
	{
		var labels = GameObject.FindObjectsOfType(typeof(UILabel));
		foreach (var l in labels) 
		{
			UILabel label = (UILabel)l;
			if(exclude == null || !exclude.Contains(label))
			{
				NewbieCommonHelper.ChangeLabelColor(label,percentage);
			}
		}
	}
	
	public static void ChangeSpriteColor(tk2dBaseSprite sprite, float percentage)
	{
		float alpha = sprite.color.a;
		Color color =sprite.color * percentage;
		color.a = alpha;
		sprite.color = color;
	}
	
	public static void ChangeUIColor(UISprite sprite, float percentage)
	{
		float alpha = sprite.color.a;
		Color color = sprite.color * percentage;
		color.a = alpha;
		sprite.color = color;
	}
	
	public static void ChangeLabelColor(UILabel label, float percentage)
	{
		float alpha = label.color.a;
		Color color = label.color * percentage;
		color.a = alpha;
		label.color = color;
	}
}
