using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;

public class MercenaryProduceComponent : BuildingProduceLogicComponent 
{
	private MercenaryType m_Type;
	private MercenaryProductLogicObject m_Product;
	
	public MercenaryProduceComponent(MercenaryType type)
	{
		this.m_Type = type;
	}
	
	public override void Initial (BuildingData data)
	{
		base.Initial (data);
		this.m_Product = this.m_BuildingData.ProduceMercenary.GetProductLogicObject(this.m_Type);
	}
	
	protected override void ProduceAdvance (float elapsedSeconds)
	{
		this.m_Product.Produce(elapsedSeconds);
	}
	
	protected override bool BlockLogic ()
	{
		if(!this.m_Product.Data.RemainingTime.HasValue)
		{
			return true;
		}
		return base.BlockLogic ();
	}
}
