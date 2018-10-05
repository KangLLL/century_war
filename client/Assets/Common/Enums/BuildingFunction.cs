using UnityEngine;
using System.Collections;

public enum BuildingFunction  
{
	Update = 0x0001,
	ProduceResource = 0x0002,
	ProduceArmy = 0x0004,
	ProduceItem = 0x0008,
	ProduceMercenary = 0x0010,
	StoreArmy = 0x0020,
	StoreItem = 0x0040,
	UpgradeArmy = 0x0080,
	UpgradeItem = 0x0100,
	AccelerateResource = 0x0200,
	AccelerateArmy = 0x0400,
	AccelerateItem = 0x0800,
	All = 0x07FF
}
