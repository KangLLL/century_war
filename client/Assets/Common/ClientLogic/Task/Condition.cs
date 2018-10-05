
using ConfigUtilities;
using ConfigUtilities.Enums;
public class Condition
{
	public Task Task { get; private set; }
    public int ConditionID { get; private set; }
	public TaskConditionConfigData ConditionConfigData { get; private set; }
	
	public int Progress { get{ return this.CurrentValue - this.StartValue; } }
	protected int CurrentValue { get; set; }
	protected int StartValue { get; private set; }
	public bool IsComplete { get; protected set; }
	
	public virtual string Description 
	{ 
		get
		{
			return string.Empty;
		}
	}

    public Condition(TaskConditionConfigData conditionConfigData, Task task, int conditionID, int startValue, int currentValue)
    {
		this.Task = task;
		this.ConditionConfigData = conditionConfigData;
        this.ConditionID = conditionID;
        this.CurrentValue = currentValue;
		this.StartValue = startValue;
    }
	
    public virtual void OnUpgradeBuilding(BuildingType buildingType, int level)
    {
    }
	
    public virtual void OnConstructBuilding(BuildingType buildingType)
    {
    }
	
    public virtual void OnProduceArmy(ArmyType armyType)
    {
    }
	
    public virtual void OnUpgradeArmy(ArmyType armyType, int level)
    {
    }
	
    public virtual void OnHonourChanged(int currentHonour)
    {
    }
	
    public virtual void OnPlunderResource(ResourceType resourceType, int number)
    {
    }
	
    public virtual void OnRemoveObject(RemovableObjectType objectType)
    {
    }
	
    public virtual void OnDestroyBuilding(BuildingType buildingType)
	{
    }
	
	public virtual void OnHireMercenary(MercenaryType mercenaryType)
	{
	}
}