using UnityEngine;
using System.Collections;

public interface IProduciable<T> 
{
	T Identity { get; }
	float LogicProduceRemainingWorkload{ get; }
	bool Produce(float efficiency, float seconds, out float remainingSeconds);
	void Reset();
	void FloorOutput();
}
