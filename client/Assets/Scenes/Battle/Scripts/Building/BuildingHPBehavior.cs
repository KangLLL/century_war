using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingHPBehavior : BuildingBaseHPBehavior 
{
	[SerializeField]
	private GameObject[] m_GoldPlunderEffectPrefab;
	[SerializeField]
	private GameObject[] m_FoodPlunderEffectPrefab;
	[SerializeField]
	private GameObject[] m_OilPlunderEffectPrefab;
	
	[SerializeField]
	private int m_GoldEffectCD;
	[SerializeField]
	private int m_FoodEffectCD;
	[SerializeField]
	private int m_OilEffectCD;
	
	private int m_GoldCurrentCount;
	private int m_FoodCurrentCount;
	private int m_OilCurrentCount;
	
	private BuildingPropertyBehavior m_Property;
	
	public override void Start ()
	{
		base.Start();
		this.m_Property = (BuildingPropertyBehavior)this.m_BaseProperty;
	}
	
	protected override void FixedUpdate ()
	{
		base.FixedUpdate ();
		this.m_GoldCurrentCount = Mathf.Max(--this.m_GoldCurrentCount, 0);
		this.m_FoodCurrentCount = Mathf.Max(--this.m_FoodCurrentCount, 0);
		this.m_OilCurrentCount = Mathf.Max(--this.m_OilCurrentCount, 0);
	}
	
	protected override void OnDead ()
	{
		BattleRecorder.Instance.RecordDestoryBuilding(this.m_Property.BuildingType);
		this.SceneHelper.DestroyBuilding(gameObject);
		base.OnDead();
	}
	
	protected override void OnHurt (int damage)
	{
		if(this.m_Property.Gold > 0 || this.m_Property.Food > 0 || this.m_Property.Oil > 0)
		{
			if(this.m_CurrentHP == 0)
			{
				this.PlayPlunderEffect(this.m_Property.Gold, this.m_Property.Food, this.m_Property.Oil);
				BattleRecorder.Instance.RecordPlunderResource(this.m_Property.Gold, this.m_Property.Food, this.m_Property.Oil, this.m_Property);
				this.m_Property.Gold = 0;
				this.m_Property.Food = 0;
				this.m_Property.Oil = 0;
			}
			else
			{
				float percentage = this.m_CurrentHP / (float)this.m_TotalHP;
				int goldValue = Mathf.RoundToInt(this.m_Property.OriginalGold * percentage);
				int foodValue = Mathf.RoundToInt(this.m_Property.OriginalFood * percentage);
				int oilValue = Mathf.RoundToInt(this.m_Property.OriginalOil * percentage);
				
				int plunderGold = this.m_Property.Gold - goldValue;
				int plunderFood = this.m_Property.Food - foodValue;
				int plunderOil = this.m_Property.Oil - oilValue;

				if(plunderGold > 0 || plunderFood > 0 || plunderOil > 0)
				{
					this.PlayPlunderEffect(plunderGold, plunderFood, plunderOil);
				
					BattleRecorder.Instance.RecordPlunderResource(plunderGold, plunderFood, plunderOil, this.m_Property);
					this.m_Property.Gold -= plunderGold;
					this.m_Property.Food -= plunderFood;
					this.m_Property.Oil -= plunderOil;
				}
			}
		}
	}
	
	private void PlayPlunderEffect(int gold, int food, int oil)
	{
		if(gold > 0 && this.m_GoldPlunderEffectPrefab.Length > 0)
		{
			AudioController.Play("GoldSteal");
			this.PlayPlunderEffect(gold, ResourceType.Gold);
		}
		if(food > 0 && this.m_FoodPlunderEffectPrefab.Length > 0)
		{
			AudioController.Play("FoodSteal");
			this.PlayPlunderEffect(food, ResourceType.Food);
		}
		if(oil > 0 && this.m_OilPlunderEffectPrefab.Length > 0)
		{
			this.PlayPlunderEffect(oil, ResourceType.Oil);
		}
	}
	
	private void PlayPlunderEffect(int plunderValue, ResourceType type)
	{
		bool isInCD = type == ResourceType.Gold ? this.m_GoldCurrentCount > 0 :
			type == ResourceType.Food ? this.m_FoodCurrentCount > 0 :
				this.m_OilCurrentCount > 0;
		if(!isInCD)
		{
			int maxID = type == ResourceType.Gold ? this.m_GoldPlunderEffectPrefab.Length : 
				type == ResourceType.Food ? this.m_FoodPlunderEffectPrefab.Length : 
					this.m_OilPlunderEffectPrefab.Length;
			int id = 0;
			if(plunderValue > ClientConfigConstants.Instance.Plunder200Criterion)
			{
				id++; 
				if(plunderValue > ClientConfigConstants.Instance.Plunder400Criterion)
				{
					id++;
					if(plunderValue > ClientConfigConstants.Instance.Plunder600Criterion)
					{
						id++;
						if(plunderValue > ClientConfigConstants.Instance.Plunder800Criterion)
						{
							id++;
						}
					}
				}
			}
			id = Mathf.Min(id, maxID);
			
			GameObject go = type == ResourceType.Gold ? GameObject.Instantiate(this.m_GoldPlunderEffectPrefab[id]) as GameObject :
				type == ResourceType.Food ? GameObject.Instantiate(this.m_FoodPlunderEffectPrefab[id]) as GameObject :
					GameObject.Instantiate(this.m_OilPlunderEffectPrefab[id]) as GameObject;
			
			go.transform.position = new Vector3(this.m_Property.AnchorTransform.position.x + go.transform.position.x,
				this.m_Property.AnchorTransform.position.y + go.transform.position.y,
				this.m_Property.AnchorTransform.position.z + go.transform.position.z);
			go.transform.parent = BattleObjectCache.Instance.EffectObjectParent.transform;
			
			if(type == ResourceType.Gold)
			{
				this.m_GoldCurrentCount = this.m_GoldEffectCD;
			}
			else if(type == ResourceType.Food)
			{
				this.m_FoodCurrentCount = this.m_FoodEffectCD;
			}
			else
			{
				this.m_OilCurrentCount = this.m_OilEffectCD;
			}
		}
	}
}
