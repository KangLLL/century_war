using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using ConfigUtilities;
using CommonUtilities;
using System.Text.RegularExpressions;
using System.Linq;
public class SystemFunction
{
    public static string TimeSpanToString(int seconds)
    {
        TimeSpan timeSpan = new TimeSpan(0, 0, 0, seconds);

        string sDay = timeSpan.Days + StringConstants.DAY;
        string sHours = timeSpan.Hours + StringConstants.HOUR;
        string sMinute = timeSpan.Minutes + StringConstants.MINUTE;
        string sMinutes = timeSpan.Minutes + StringConstants.MINUTES;
        string sSeconds = timeSpan.Seconds + StringConstants.SECOND;

        //string remainingTime = string.Empty;
        if (timeSpan.Days > 0) 
            return timeSpan.Hours > 0 ? sDay + sHours : sDay;

        if (timeSpan.Hours > 0)
            return timeSpan.Minutes > 0 ? sHours + sMinute : sHours;

        if (timeSpan.Minutes > 0)
            return timeSpan.Seconds > 0 ? sMinute + sSeconds : sMinutes;

        return sSeconds;
    }
    public static T[] ConverTObjectToArray<T>(params T[] param)
    {
        return param;
    }
    public static Action[] ConverTObjectToArray(params Action[] param)
    {
        return param;
    }
    public static ProgressParam CalculateParam(float currentValue, float nextValue, float maxValue)
    {
        ProgressParam progressParam = new ProgressParam()
        {
            ProgressCurrent = maxValue == 0 ? 0 : (float)currentValue / maxValue,
            ProgressNext = maxValue == 0 ? 0 : (float)nextValue / maxValue,
            Value = maxValue == 0 ? "0" : + Mathf.RoundToInt(currentValue) + "[01e806]" + "+" + Mathf.RoundToInt(nextValue - currentValue) + "[-]"
        };
        return progressParam;
    }
    #region Balance
    public static Dictionary<CostType, int> UpgradeCosBalance(BuildingConfigData buildingConfigData)
    {
        Dictionary<CostType, int> costValue = new Dictionary<CostType, int>() { { CostType.Gold, buildingConfigData.UpgradeGold },
                                                                                { CostType.Food, buildingConfigData.UpgradeFood},
                                                                                { CostType.Oil, buildingConfigData.UpgradeOil},
                                                                                { CostType.Gem, buildingConfigData.UpgradeGem}};
        Dictionary<CostType, int> userHasValue = new Dictionary<CostType, int>() {{ CostType.Gold, LogicController.Instance.PlayerData.CurrentStoreGold}, 
                                                                                  { CostType.Food, LogicController.Instance.PlayerData.CurrentStoreFood}, 
                                                                                  { CostType.Oil, LogicController.Instance.PlayerData.CurrentStoreOil}, 
                                                                                  { CostType.Gem, LogicController.Instance.PlayerData.CurrentStoreGem}};

        Dictionary<CostType, int> result = new Dictionary<CostType, int>();
        foreach (KeyValuePair<CostType, int> coat in costValue)
        {
            if (userHasValue[coat.Key] < coat.Value)
            {
                result.Add(coat.Key, coat.Value - userHasValue[coat.Key]);
            }
        }
        return result;
    }
    public static Dictionary<CostType, int> UpgradeCostBalance(BuildingLogicData buildingLogicData)
    {
        Dictionary<CostType, int> costValue = new Dictionary<CostType, int>() { { CostType.Gold, buildingLogicData.UpgradeGold },
                                                                                { CostType.Food, buildingLogicData.UpgradeFood},
                                                                                { CostType.Oil, buildingLogicData.UpgradeOil},
                                                                                { CostType.Gem, buildingLogicData.UpgradeGem}};
        Dictionary<CostType, int> userHasValue = new Dictionary<CostType, int>() {{ CostType.Gold, LogicController.Instance.PlayerData.CurrentStoreGold}, 
                                                                                  { CostType.Food, LogicController.Instance.PlayerData.CurrentStoreFood}, 
                                                                                  { CostType.Oil, LogicController.Instance.PlayerData.CurrentStoreOil}, 
                                                                                  { CostType.Gem, LogicController.Instance.PlayerData.CurrentStoreGem}};

        Dictionary<CostType, int> result = new Dictionary<CostType, int>();
        foreach (KeyValuePair<CostType, int> coat in costValue)
        {
            if (userHasValue[coat.Key] < coat.Value)
            {
                result.Add(coat.Key, coat.Value - userHasValue[coat.Key]);
            }
        }
        return result;
    }
    public static Dictionary<CostType, int> UpgradeCostBalance(RemovableObjectLogicData removableObjectLogicData)
    {
        Dictionary<CostType, int> costValue = new Dictionary<CostType, int>() { { CostType.Gold, removableObjectLogicData.GoldCost },
                                                                                { CostType.Food, removableObjectLogicData.FoodCost},
                                                                                { CostType.Oil, removableObjectLogicData.OilCost},
                                                                                { CostType.Gem, removableObjectLogicData.GemCost}};
        Dictionary<CostType, int> userHasValue = new Dictionary<CostType, int>() {{ CostType.Gold, LogicController.Instance.PlayerData.CurrentStoreGold}, 
                                                                                  { CostType.Food, LogicController.Instance.PlayerData.CurrentStoreFood}, 
                                                                                  { CostType.Oil, LogicController.Instance.PlayerData.CurrentStoreOil}, 
                                                                                  { CostType.Gem, LogicController.Instance.PlayerData.CurrentStoreGem}};

        Dictionary<CostType, int> result = new Dictionary<CostType, int>();
        foreach (KeyValuePair<CostType, int> coat in costValue)
        {
            if (userHasValue[coat.Key] < coat.Value)
            {
                result.Add(coat.Key, coat.Value - userHasValue[coat.Key]);
            }
        }
        return result;
    }
    public static Dictionary<CostType, int> UpgradeCostBalance(ArmyConfigData ArmyConfigData)
    {
        Dictionary<CostType, int> costValue = new Dictionary<CostType, int>() { { CostType.Gold, ArmyConfigData.UpgradeCostGold },
                                                                                { CostType.Food, ArmyConfigData.UpgradeCostFood},
                                                                                { CostType.Oil, ArmyConfigData.UpgradeCostOil},
                                                                                { CostType.Gem, ArmyConfigData.UpgradeCostGem}};
        Dictionary<CostType, int> userHasValue = new Dictionary<CostType, int>() {{ CostType.Gold, LogicController.Instance.PlayerData.CurrentStoreGold}, 
                                                                                  { CostType.Food, LogicController.Instance.PlayerData.CurrentStoreFood}, 
                                                                                  { CostType.Oil, LogicController.Instance.PlayerData.CurrentStoreOil}, 
                                                                                  { CostType.Gem, LogicController.Instance.PlayerData.CurrentStoreGem}};

        Dictionary<CostType, int> result = new Dictionary<CostType, int>();
        foreach (KeyValuePair<CostType, int> coat in costValue)
        {
            if (userHasValue[coat.Key] < coat.Value)
            {
                result.Add(coat.Key, coat.Value - userHasValue[coat.Key]);
            }
        }
        return result;
    }
    public static Dictionary<CostType, int> ProduceCostBalance(ArmyConfigData ArmyConfigData)
    {
        Dictionary<CostType, int> costValue = new Dictionary<CostType, int>() { { CostType.Gold, ArmyConfigData.ProduceCostGold },
                                                                                { CostType.Food, ArmyConfigData.ProduceCostFood},
                                                                                { CostType.Oil, ArmyConfigData.ProduceCostOil},
                                                                                { CostType.Gem, ArmyConfigData.ProduceCostGem}};
        Dictionary<CostType, int> userHasValue = new Dictionary<CostType, int>() {{ CostType.Gold, LogicController.Instance.PlayerData.CurrentStoreGold}, 
                                                                                  { CostType.Food, LogicController.Instance.PlayerData.CurrentStoreFood}, 
                                                                                  { CostType.Oil, LogicController.Instance.PlayerData.CurrentStoreOil}, 
                                                                                  { CostType.Gem, LogicController.Instance.PlayerData.CurrentStoreGem}};

        Dictionary<CostType, int> result = new Dictionary<CostType, int>();
        foreach (KeyValuePair<CostType, int> coat in costValue)
        {
            if (userHasValue[coat.Key] < coat.Value)
            {
                result.Add(coat.Key, coat.Value - userHasValue[coat.Key]);
            }
        }
        return result;
    }
    public static Dictionary<CostType, int> ProduceCostBalance(MercenaryConfigData MercenaryConfigData)
    {
        Dictionary<CostType, int> costValue = new Dictionary<CostType, int>() { { CostType.Gold, MercenaryConfigData.HireCostGold },
                                                                                { CostType.Food, MercenaryConfigData.HireCostFood},
                                                                                { CostType.Oil, MercenaryConfigData.HireCostOil},
                                                                                { CostType.Gem, MercenaryConfigData.HireCostGem}};
        Dictionary<CostType, int> userHasValue = new Dictionary<CostType, int>() {{ CostType.Gold, LogicController.Instance.PlayerData.CurrentStoreGold}, 
                                                                                  { CostType.Food, LogicController.Instance.PlayerData.CurrentStoreFood}, 
                                                                                  { CostType.Oil, LogicController.Instance.PlayerData.CurrentStoreOil}, 
                                                                                  { CostType.Gem, LogicController.Instance.PlayerData.CurrentStoreGem}};

        Dictionary<CostType, int> result = new Dictionary<CostType, int>();
        foreach (KeyValuePair<CostType, int> coat in costValue)
        {
            if (userHasValue[coat.Key] < coat.Value)
            {
                result.Add(coat.Key, coat.Value - userHasValue[coat.Key]);
            }
        }
        return result;
    }
    #endregion
    #region total cost
    public static Dictionary<CostType, int> UpgradeTotalCost(BuildingConfigData buildingConfigData)
    {
        return new Dictionary<CostType, int>() { { CostType.Gold, buildingConfigData.UpgradeGold },
                                                 { CostType.Food, buildingConfigData.UpgradeFood},
                                                 { CostType.Oil, buildingConfigData.UpgradeOil},
                                                 { CostType.Gem, buildingConfigData.UpgradeGem}};
    }
    public static Dictionary<CostType, int> UpgradeTotalCost(BuildingLogicData buildingLogicData)
    {
        return new Dictionary<CostType, int>() { { CostType.Gold, buildingLogicData.UpgradeGold},
                                                 { CostType.Food, buildingLogicData.UpgradeFood},
                                                 { CostType.Oil, buildingLogicData.UpgradeOil},
                                                 { CostType.Gem, buildingLogicData.UpgradeGem}};
    }
    public static Dictionary<CostType, int> UpgradeTotalCost(RemovableObjectLogicData removableObjectLogicData)
    {
        return new Dictionary<CostType, int>() { { CostType.Gold, removableObjectLogicData.GoldCost },
                                                 { CostType.Food, removableObjectLogicData.FoodCost},
                                                 { CostType.Oil, removableObjectLogicData.OilCost},
                                                 { CostType.Gem, removableObjectLogicData.GemCost}};
    }
    public static Dictionary<CostType, int> UpgradeTotalCost(ArmyConfigData ArmyConfigData)
    {
        return new Dictionary<CostType, int>() { { CostType.Gold, ArmyConfigData.UpgradeCostGold },
                                                 { CostType.Food, ArmyConfigData.UpgradeCostFood},
                                                 { CostType.Oil, ArmyConfigData.UpgradeCostOil},
                                                 { CostType.Gem, ArmyConfigData.UpgradeCostGem}};
    }
    public static Dictionary<CostType, int> ProduceTotalCost(ArmyConfigData ArmyConfigData)
    {
        return new Dictionary<CostType, int>() { { CostType.Gold, ArmyConfigData.ProduceCostGold },
                                                 { CostType.Food, ArmyConfigData.ProduceCostFood},
                                                 { CostType.Oil, ArmyConfigData.ProduceCostOil},
                                                 { CostType.Gem, ArmyConfigData.ProduceCostGem}};
    }
    public static Dictionary<CostType, int> ProduceTotalCost(MercenaryConfigData MercenaryConfigData)
    {
        return new Dictionary<CostType, int>() { { CostType.Gold, MercenaryConfigData.HireCostGold },
                                                 { CostType.Food, MercenaryConfigData.HireCostFood},
                                                 { CostType.Oil, MercenaryConfigData.HireCostOil},
                                                 { CostType.Gem, MercenaryConfigData.HireCostGem}};
    }
    public static Dictionary<CostType, int> PlayerTotalResource()
    {
        return new Dictionary<CostType, int>() { { CostType.Gold, LogicController.Instance.PlayerData.CurrentStoreGold},
                                                 { CostType.Food, LogicController.Instance.PlayerData.CurrentStoreFood},
                                                 { CostType.Oil, LogicController.Instance.PlayerData.CurrentStoreOil},
                                                 { CostType.Gem, LogicController.Instance.PlayerData.CurrentStoreGem}};
    }
    #endregion

    #region cost uplimit check
    public static bool CostUplimitCheck(int costGold,int costFood, int costOil)
    {
        if (LogicController.Instance.PlayerData.GoldMaxCapacity < costGold)
            return false;
        if (LogicController.Instance.PlayerData.FoodMaxCapacity < costFood)
            return false;
        if (LogicController.Instance.PlayerData.OilMaxCapacity < costOil)
            return false;
        return true;
    }
    #endregion
    public static int ResourceConvertToGem(Dictionary<CostType, int> resource)
    {
        int gem = 0;
        foreach (KeyValuePair<CostType, int> cost in resource)
        {
            switch (cost.Key)
            {
                case CostType.Food:
                    gem += MarketCalculator.GetFoodCost(cost.Value);
                    break;
                case CostType.Gold:
                    gem += MarketCalculator.GetGoldCost(cost.Value);
                    break;
                case CostType.Gem:
                    gem += cost.Value;
                    break;
                case CostType.Oil:
                    gem += MarketCalculator.GetOilCost(cost.Value);
                    break;
            }
        }
        return gem;
    }
    public static string BuyReusoureContext(Dictionary<CostType, int> resource)
    {
        string context = string.Empty;
        foreach (KeyValuePair<CostType, int> cost in resource)
        {
            switch (cost.Key)
            {
                case CostType.Food:
                    context += string.Format(StringConstants.PROMPT_RESOURCE_COST, cost.Value, StringConstants.RESOURCE_FOOD) + StringConstants.QUESTION_MARK + "\n";
                    break;
                case CostType.Gold:
                    context += string.Format(StringConstants.PROMPT_RESOURCE_COST, cost.Value, StringConstants.RESOURCE_GOLD) + StringConstants.QUESTION_MARK + "\n";
                    break;
                case CostType.Gem:
                    context += string.Format(StringConstants.PROMPT_RESOURCE_COST, cost.Value, StringConstants.COIN_GEM) + StringConstants.QUESTION_MARK + "\n";
                    break;
                case CostType.Oil:
                    context += string.Format(StringConstants.PROMPT_RESOURCE_COST, cost.Value, StringConstants.RESOURCE_OIL) + StringConstants.QUESTION_MARK + "\n";
                    break;
            }
        }
        return context;
    }
    public static string BuyReusoureTitle(Dictionary<CostType, int> resource)
    {
        List<string> title = new List<string>();
        foreach (KeyValuePair<CostType, int> cost in resource)
        {
            switch (cost.Key)
            {
                case CostType.Gold:
                    title.Add(StringConstants.RESOURCE_GOLD);
                    break;
                case CostType.Food:
                    title.Add(StringConstants.RESOURCE_FOOD);
                    break;
                case CostType.Oil:
                    title.Add(StringConstants.RESOURCE_OIL);
                    break;
                case CostType.Gem:
                    title.Add(StringConstants.COIN_GEM);
                    break;
            }
        }
        return StringConstants.PROMT_REQUEST_RESOURCE + string.Join(StringConstants.PROMPT_AND, title.ToArray());
 
    }
    public static void BuyResources(Dictionary<CostType, int> resources)
    {
        foreach (KeyValuePair<CostType, int> resource in resources)
        {
            switch (resource.Key)
            {
                case CostType.Food:
                    LogicController.Instance.BuyFood(resource.Value);
                    break;
                case CostType.Gem:
                    break;
                case CostType.Gold:
                    LogicController.Instance.BuyGold(resource.Value);
                    break;
                case CostType.Oil:
                    LogicController.Instance.BuyOil(resource.Value);
                    break; 
            }
        }
    }

    public static float Division(float dividend, float divisor)
    {
        if (divisor == 0)
            return 0;
        return dividend / divisor;
    }
    //-1 = player capacity >= max capacity; 0 = current capacity < 1% ; 1 = enable collect
    public static int CheckCollectValidity(BuildingLogicData buildingLogicData, ResourceType resourceType)
    {
        int validity = -1;
        int produceRatePerHour = 0;
        switch (resourceType)
        {
            case ResourceType.Gold:
                produceRatePerHour = Mathf.RoundToInt((buildingLogicData.ProduceGoldEfficiency * ClientConfigConstants.Instance.HourToSecond) * ClientConfigConstants.Instance.ProduceRateHourPercentage);
                validity = (LogicController.Instance.PlayerData.CurrentStoreGold >= LogicController.Instance.PlayerData.GoldMaxCapacity && buildingLogicData.CurrentStoreGold >= produceRatePerHour) ? -1 : buildingLogicData.CurrentStoreGold < produceRatePerHour ? 0 : 1;
                //validity = LogicController.Instance.PlayerData.CurrentStoreGold >= LogicController.Instance.PlayerData.GoldMaxCapacity ? -1 : buildingLogicData.CurrentStoreGold < produceRatePerHour ? 0 : 1;
                break;
            case ResourceType.Food:
                produceRatePerHour = Mathf.RoundToInt((buildingLogicData.ProduceFoodEfficiency * ClientConfigConstants.Instance.HourToSecond) * ClientConfigConstants.Instance.ProduceRateHourPercentage);
                validity = (LogicController.Instance.PlayerData.CurrentStoreFood >= LogicController.Instance.PlayerData.FoodMaxCapacity && buildingLogicData.CurrentStoreFood >= produceRatePerHour) ? -1 : buildingLogicData.CurrentStoreFood < produceRatePerHour ? 0 : 1;
                break;
            case ResourceType.Oil:
                produceRatePerHour = Mathf.RoundToInt((buildingLogicData.ProduceOilEfficiency * ClientConfigConstants.Instance.HourToSecond) * ClientConfigConstants.Instance.ProduceRateHourPercentage);
                validity = (LogicController.Instance.PlayerData.CurrentStoreOil >= LogicController.Instance.PlayerData.OilMaxCapacity && buildingLogicData.CurrentStoreOil >= produceRatePerHour) ? -1 : buildingLogicData.CurrentStoreOil < produceRatePerHour ? 0 : 1;
                break;
        }
        return validity;
    }
    public static bool CheckCollectValidityByButton(BuildingLogicData buildingLogicData, ResourceType resourceType)
    {
        switch (resourceType)
        {
            case ResourceType.Gold:
                return (buildingLogicData.CurrentStoreGold > 0 && LogicController.Instance.PlayerData.CurrentStoreGold < LogicController.Instance.PlayerData.GoldMaxCapacity);
            case ResourceType.Food:
                return (buildingLogicData.CurrentStoreFood > 0 && LogicController.Instance.PlayerData.CurrentStoreFood < LogicController.Instance.PlayerData.FoodMaxCapacity);
            case ResourceType.Oil:
                return (buildingLogicData.CurrentStoreOil > 0 && LogicController.Instance.PlayerData.CurrentStoreOil < LogicController.Instance.PlayerData.OilMaxCapacity);
        }
        return false;
    }
    // Bug 
    public static int GetCollectPercentageRange(BuildingLogicData buildingLogicData, ResourceType resourceType)
    {
        int percentage = 0;
        switch (resourceType)
        {
            case ResourceType.Gold:
                percentage = Mathf.RoundToInt(((float)buildingLogicData.CurrentStoreGold / buildingLogicData.StoreGoldCapacity) * 100);
                break;
            case ResourceType.Food:
                percentage = Mathf.RoundToInt(((float)buildingLogicData.CurrentStoreFood / buildingLogicData.StoreFoodCapacity) * 100);
                break;
            case ResourceType.Oil:
                percentage = Mathf.RoundToInt(((float)buildingLogicData.CurrentStoreOil / buildingLogicData.StoreOilCapacity) * 100);
                break;
        }
        int scope = percentage / 20;
        return scope > 4 ? 4 : scope;
    }
    public static int GetCollectPercentageRange(int collected, BuildingLogicData buildingLogicData, ResourceType resourceType)
    {
        int percentage = 0;
        switch (resourceType)
        {
            case ResourceType.Gold:
                percentage = Mathf.RoundToInt(((float)collected / buildingLogicData.StoreGoldCapacity) * 100);
                break;
            case ResourceType.Food:
                percentage = Mathf.RoundToInt(((float)collected / buildingLogicData.StoreFoodCapacity) * 100);
                break;
            case ResourceType.Oil:
                percentage = Mathf.RoundToInt(((float)collected / buildingLogicData.StoreOilCapacity) * 100);
                break;
        }
        int scope = percentage / 20;
        return scope > 4 ? 4 : scope;
    }
    public static float GetCollectPercentage(BuildingLogicData buildingLogicData, ResourceType resourceType)
    {
        float percentage = 0;
        switch (resourceType)
        {
            case ResourceType.Gold:
                percentage =  (float)buildingLogicData.CurrentStoreGold / buildingLogicData.StoreGoldCapacity;
                break;
            case ResourceType.Food:
                percentage = (float)buildingLogicData.CurrentStoreFood / buildingLogicData.StoreFoodCapacity;
                break;
            case ResourceType.Oil:
                percentage = (float)buildingLogicData.CurrentStoreOil / buildingLogicData.StoreOilCapacity;
                break;
        } 
        return percentage;
    }
    public static bool RegexEmail(string input)
    {
        //string pattern1 = @"^[a-z0-9_\-]+(\.[_a-z0-9\-]+)*@([_a-z0-9\-]+\.)+([a-z]{2}|aero|arpa|biz|com|coop|edu|gov|info|int|jobs|mil|museum|name|nato|net|org|pro|travel)$";
        //string pattern2 = @"^\s*([A-Za-z0-9_-]+(\.\w+)*@([\w-]+\.)+\w{2,10})\s*$";
        //string pattern3 = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        //string pattern4 = @"[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})";
        string pattern5 = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                          
        return Regex.IsMatch(input,pattern5);
    }
    public static bool RegexUserName(string input)
    {
        //中日韩
        string pattert1 = @"^[a-zA-Z0-9_\u2E80-\u9FFF]{1,16}$";
        //中英
        //string pattern2 = @"^[a-zA-Z0-9_\u4e00-\u9fa5]+$";
        return Regex.IsMatch(input, pattert1);
    }
    public static bool RegexPassword(string input)
    {
        //验证密码 由不小于6位不大于20位的字母数字下划线特殊符号组成!
        //string pattern1 = @"^.{6,20}___FCKpd___0quot";
        //由不小于6位不大于20位的字母数字下划
        string pattern2 = @"^[0-9a-zA-z_]{6,20}$";
        //英文数字特殊字符6-20
        //string pattern3 = @"^[0-9a-zA-Z`~!@#$%\^&*()_+-={}|\[\]:"";'<>?,.]{6,20}$";
        return Regex.IsMatch(input, pattern2);
    }
    public static bool RegexUserEmoji(string input)
    {
        //string number = @"[\u0030-\u0039]";
        //string char1 = @"[\u0041-\u005a]";
        //string char2 = @"[\u0061-\u007a]";
        //string line = @"[\u005f]";
        string pattern2 = @"[\u00a9\u00ae\u203c-\u2b55\u3030\u303d\u3297\u3299\ud83c-\udff0\ufe0f\u20e3]";
        return Regex.IsMatch(input, pattern2);
    }
    public static string ReplaceEmoji(string input)
    {
        string text = input;
        if (SystemFunction.RegexUserEmoji(text)) 
          (new List<char>(text.Where(a => SystemFunction.RegexUserEmoji(new string(a, 1))))).ForEach(c=> text = text.Replace(new string(c, 1), ""));
        return text;
    }
}
