using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class BuildingProduciableComponent<T,P> : BuildingProduceLogicComponent where P : IProduciable<T>
{
	public event Action<P, int, float> ProduceFinished;
	
	protected abstract int ProduceEfficiency
	{
		get;
	}
	
	protected abstract P CurrentProducingProduct
	{
		get;
	}
	
	protected abstract P GetProduct(int order);
	
	private P m_PreviousProduceObject;
	
	public override void Initial (BuildingData data)
	{
		base.Initial (data);
		if(this.CurrentProducingProduct != null)
		{
			this.m_PreviousProduceObject = this.CurrentProducingProduct;
		}
	}
	
	public override void Process ()
	{
		if(this.m_PreviousProduceObject != null && this.CurrentProducingProduct != null && 
			!ReferenceEquals(this.m_PreviousProduceObject,this.CurrentProducingProduct) )
		{
			this.Reset();
			this.m_PreviousProduceObject = this.CurrentProducingProduct;	
		}
		
		base.Process ();
	}
	
	protected override void ProduceAdvance (float elapsedSeconds)
	{
		float efficiency = this.ProduceEfficiency;
		if(this.Accelerate != null)
		{
			efficiency = efficiency * this.Accelerate.Effect;
		}
		float remainingSeconds = elapsedSeconds;
		int productIndex = 1;
		P product = this.CurrentProducingProduct;
		while(remainingSeconds > 0 && product != null)
		{
			if(product.Produce(efficiency, remainingSeconds, out remainingSeconds))
			{
				if(this.ProduceFinished != null)
				{
					this.ProduceFinished(product, (int)(elapsedSeconds - remainingSeconds), remainingSeconds);
				}
				product = this.GetProduct(productIndex++);
			}
		}
	}
	
	protected override bool BlockLogic ()
	{
		if(base.BlockLogic())
			return true;
		else
		{
			if(this.CurrentProducingProduct == null)
				return true;
			else
			{
				return this.CurrentProducingProduct.LogicProduceRemainingWorkload.IsZero();
			}
		}
	}
	
	protected override void FloorOutput ()
	{
		if(this.CurrentProducingProduct != null)
		{
			this.CurrentProducingProduct.FloorOutput();
		}
	}
}
