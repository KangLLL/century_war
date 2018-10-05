using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class StringConstants
{
    public const string PROMPT_RESOURCE_COST = "购买其余的{0}{1}";
    public const string PROMPT_GEM_COST = "你是否想用{0}个{1}来立即完成{2}的{3}";
    public const string PROMPT_FINISH_INSTANTLY = "立即完成！";
    public const string PROMPT_ACCELERATE_RESOURCE = "确定要在{0}内加速{1}倍生产资源？";
    public const string PROMPT_ACCELERATE_ARMY = "确定要在{0}内加速{1}倍训练士兵？";
    public const string PROMPT_CANCEL_UPGRADE = "确定取消升级？";
    public const string PROMPT_REQUEST_GOLD = "你需要更多的金币";
    public const string PROMT_REQUEST_RESOURCE = "你需要更多的";
    public const string PROMT_IS_ACCELERATE = "加快速度？";
    public const string PROMT_ACCELERATE = "加  速";
    public const string PROMT_ACCELERATE_NOW = "正在加速";
    public const string PROMPT_CANCEL_UPGRADE_CONTEXT = "取消升级只会返还{0}%资源！你确定要取消升级吗？";
    public const string PROMPT_REQUIRE_BUILDING_LEVEL = "需要{0}级{1}";
    public const string PROMPT_TRAIN = "训练";
    public const string PROMPT_UPGRADE = "升级";
    public const string PROMPT_CONSTRUCT = "建造";
    public const string PROMPT_CREATE = "创造";
    public const string PROMPT_ARMY_TYPE = "士兵";
    public const string PROMPT_TO = "到";
    public const string PROMPT_AND = "与";
    public const string PROMPT_LEVEL = "等级";
    public const string PROMPT_LV = "级";
    public const string PROMPT_MAX_LEVEL = "最高等级";
    public const string PROMPT_MAX = "最大";
    public const string PROMPT_MIN = "最小";
    public const string PROMPT_EXP_LEVEL = "/star部落等级";
    public const string PROMPT_IS_FULL = "已满";
    public const string PROMPT_EXCLAMATION_POINT = "！！！";
    public const string LEFT_PARENTHESES = "（";
    public const string RIGHT_PARENTHESES = "）";
    public const string RESOURCE_GOLD = "金币";
    public const string RESOURCE_FOOD = "食物";
    public const string RESOURCE_OIL = "石油";
    public const string COIN_GEM = "宝石";  
    public const string QUESTION_MARK = "？";
    public const string DAY = "天";
    public const string HOUR = "小时";
    public const string MINUTE = "分";
    public const string MINUTES = "分钟";
    public const string SECOND = "秒";
    public const string COLON = "：";
    public const string WITHOUT = "无";
    public const string ELAPSED_AGO = "以前";
    public const string PROMPT_DEFEND_SUCCESS = "你的防御成功了";
    public const string PROMPT_DEFEND_FAIL = "你的防御失败了";
    public const string PROMPT_ATTACK_SUCCESS = "你的进攻成功了";
    public const string PROMPT_ATTACK_FAIL = "你的进攻失败了";
    public const string TITLE_REGIST_ACCOUNT = "注册（绑定）";
    public const string TITLE_EXCHANGE_ACCOUNT = "更换账号";
    public const string PROMPT_91_ACCOUNT = "91账号";
    public const string PROMPT_RECORD_ALREADY_BOUNDED = "记录已绑定到账号";
    public const string PROMPT_REGIST_SUCCESS = "恭喜您，已注册成功！注册邮箱为（{0}）。邮箱以和当前游戏记录绑定，你可以通过设置中的更换账号功能在其他机器使用上登陆使用。";
    public const string PROMT = "提示";
    public const string PROMT_EXCHANGE_ACCOUNT_PROMT = "请注意！当前游戏数据未绑定任何邮箱账号，如果点击“确定”，那么你账号中的游戏数据将会覆盖当前游戏数据，从而造成游戏数据丢失。即使这样你还确定要这么做吗？";
    public const string PROMT_EXCHANGE_ACCOUNT_PROMT2 = "更换账号会退出当前游戏，即使这样你还确定这么做吗？";
    public const string PROMT_GET_EXP = "修建或升级建筑可以获得/star经验。";
    public const string PROMT_REQUIRE_LEVEL = "需要部落等级{0}级";
    public const string PROMT_DESTROY_PROP = "销毁？";
    public const string PROMT_DESTROY_PROP_CONTEXT = "销毁将会使你永久失去这个道具，即使这样您还要把{0}销毁吗？";

    public const string PROMT_PLACE_PROP = "布置";
    public const string PROMT_USE_PROP = "使用";
    public const string PROMT_FIGHT_PROP = "参战";
    public const string PROMT_CANCEL_FIGHT_PROP = "取消参战";
    public const string PROMT_USER_REGIST_SUCCESFULL = "恭喜您，已注册成功！注册邮箱为{0}。邮箱将会与您的游戏记录绑定，你可以通过设置中的更换账号功能在其他机器上登陆使用。";
    
    public const string PROMT_OBSTACLE_COUNT = "障碍数量：{0}/{1}";
    public const string PROMT_REMOVE_PLANT = "移除{0}有几率获得：";

    public const string PROMT_REMOVE_OBJCET_TITLE = "移除？";
    public const string PROMT_REMOVE_OBJCET_TITLE_CONTEXT = "{0}被移除后将会永久消失，即使这样你还确定要移除它吗？";
    public const string PROMT_BUY_RESOURCE = "购买{0}吗？";
    public const string PROMT_CONFORM_BUY_RESOURCE = "确定购买{0}{1}吗？";
    public const string PROMT_GEM_IS_NOT_ENOUGH = "宝石不够";
    public const string PROMT_GET_MORE_GEM = "您想获取更多的宝石吗？";
    public const string PROMT_FILL_PERCENTAGE_STORAGE = "装满{0}仓库的{1}%";
    public const string PROMT_FILL_STORAGE = "装满{0}仓库";
    public const string PROMT_TASK_REMAINING_TIME = "[ff7c7c]剩余时间：";
    
    #region Error message
    public static string[] ERROR_MESSAGE =  { "需要将你的{0}升级到{1}级！",//0
                                                             "集结地已满",//1
                                                       "墙的数量到达上限",//2
                                                 "当前玩家在线，无法攻击",//3
                                                 "没有士兵，请先训练士兵",//4
                                                       "对方已开启保护盾",//5
                                                     "当前玩家正在被攻击",//6
                                       "工人正在移除障碍，无法立即完成！",//7
                                                     "金币不足，无法查找",//8
                                               "需要至少一个单位的士兵！",//9
                                                           "邮箱格式错误",//10
                                                           "邮箱已被注册",//11
                                      "密码至少需要6位，并且不能超过20位",//12
                                               "当前邮箱不存在或密码错误",//13
                                             "当前账号在线，无法更换账号",//14
                                                    "解锁{0}需要{1}{2}级",//15
                 "购买资源后超过储存最大值！请建造资源仓库提升储存容量。",//16
                                               "该建筑已达到当前最大数量",//17
             "用户名不能包含空格或其他非法字符，长度应在1至{0}个字符之间",//18
                                                             "名称已存在",//19
                                                  "你的{0}存储上限不足。",//20
                                              "你的物品仓库储存上限不足。",//21
                                                   "没有更多的此类佣兵。",//22
                                         "升级墙的条件不满足，无法升级。",//23
                                        "成功将{0}个{1}级墙升级至{2}级。",//24
                                         "不能使用敏感词汇，请重新输入。",//25
                                   "道具仓库空间不足，请整理后再做尝试。",//26
                   "道具仓库已满，抢夺道具已丢失，建议尽快整理仓库空间。",//27
                                               "部落等级不足，无法使用。",//28
                                         "当前道具处于冷却中，无法使用。",//29
                                   "可布置防御类道具数量已达到最大限制。",//30
                           "可携带攻击类道具进入战斗数量已达到最大限制。",//31
                                                 "账户不存在或密码错误。",//32
                                                 "密码与确认密码不一致。",//33
                                     "密码应为6-20位英文、数字或下划线。",//34
                         "部落中障碍数量已达到上限，请清理后再尝试购买。",//35
                                           "确定种植后，将无法再次移动。",//36
       "特殊建筑已达到上限，可以通过市政厅信息界面中查看特殊建筑的上限。",//37
                                            "所需{0}道具不足，无法建造。",//38
                                               "没有可修复{0}的{1}道具。",//39
                                                         "{0}升级到{1}级",//40
                                                    "{0}已满，无需购买。" //41
                                            };
    #endregion
    public static Dictionary<float, string> NEWBIEGUIDE_CONTEXT = new Dictionary<float, string>()
    {
		{0.1f,"首领！大事不妙！！！\n附近的[ffb74f]“狗蛋部落”[-]\n派出了大帮人马来攻打我们！赶紧做好准备迎战！"},
        {0.2f,"首领！首领！\n总算找到您了。刚才被[ffb74f]“狗蛋部落”[-]洗劫时，我和我的小伙伴们都被冲散了。现在大伙儿在这里搭建了一个[ffb74f]简陋的部落[-]。"},
		{1.1f,"首领！首领！\n总算找到您了。刚才被[ffb74f]“狗蛋部落”[-]洗劫时，我和我的小伙伴们都被冲散了。现在大伙儿在这里搭建了一个[ffb74f]简陋的部落[-]。"},
        {1.2f,"啊！差点忘了，首领您在大战时头部受到重击导致失忆，这才让敌人有机可乘的。下面我会协助首领重新掌握村落发展的[ffb74f]基本技巧[-]，我们开始吧！"},//delete
        {2.1f,"看来一切都得从头开始了，为我们的新部落[00c0ff]取个名字[-]吧！！！"},
        {3.1f,"名字真好听，不愧为首领！接下来让我们\n[00c0ff]修建一座“金矿”[-]吧。\n[ffb74f]金矿[-]可以[ffb74f]开采出金币[-]，让部落发展的更快更强大。"},
        {3.2f,"修建建筑需要\n[00c0ff]选择“工人”[-]。以后可以点击后面带锁的按钮[ffb74f]解锁工人[-]哦！现在我们先选择工人吧！"},
        {3.3f,"当建筑修建完毕后，将会进入[ffb74f]验收状态[-]，[00c0ff]点击它[-]就可以完成验收。\n一定[ffb74f]不要忘了验收[-]哦！否则建筑[ffb74f]无法正常工作！！[-]"},
        {3.4f,"首领真厉害，这么快就掌握诀窍了。现在您看到的是我们部落[ffb74f]当前存储的金币数量[-]。[ffb74f]修建或升级建筑[-]会使用这里[ffb74f]存储的金币[-]。"},
        {4.1f,"有了[ffb74f]金矿[-]当然不能少了[ffb74f]存储金币[-]的建筑，那么我们来[00c0ff]修建一个“金币仓库”[-]吧！"},
        {4.2f,"让我们再来[00c0ff]验收[-]一次吧，前面忘记告诉您了，验收建筑时，可能[ffb74f]获得一定的奖励[-]哦！"},
        {4.3f,"很好，现在有了[ffb74f]金币仓库[-]就不怕开采出的金币没地方存放了。\n[ffb74f]金币仓库被掠夺的金币比例会比金矿低很多[-]，尽量把金币放在里面吧！"},
        {4.4f,"对了，[ffb74f]金币存储[-]的[ffb74f]最大容量[-]将会显示在[ffb74f]箭头所指位置[-]，[ffb74f]金币数量达到最大[-]后将[ffb74f]不能存储更多的金币[-]，[ffb74f]修建或升级“金币仓库”[-]可以提高最大容量！"},
        {5.1f,"部落的发展除了依靠金币以外，还需要[ffb74f]食物[-]的供给。食物可以用于[ffb74f]建造建筑、升级建筑、生产士兵[-]。快[00c0ff]修建一座“农场”[-]吧！"},
        {5.2f,"时间紧迫，赶紧\n[00c0ff]使用“宝石”来立即完成农场的修建[-]吧！"},
        {5.3f,"啊哈！宝石的力量很神奇吧！以后您还能发现[ffb74f]宝石更多的秘密！[-]言归正传，现在您看到的是部落[ffb74f]当前所存储的食物数量[-]。食物和金币一样重要！"},
        {6.1f,"有了[ffb74f]农场[-]还需要一个[ffb74f]存储食物[-]的建筑，让我们再来[00c0ff]修建一个“食物仓库”[-]吧！"},
        {6.2f,"[ffb74f]食物仓库和金币仓库一样可以减少被掠夺的比例[-]，尽量将食物放在里面哟！[ffb74f]食物存储[-]的[ffb74f]最大容量[-]将会显示在[ffb74f]箭头所指位置[-]。"},
        {7.1f,"很好，是时候复仇了。\n我们需要[ffb74f]训练士兵[-]去攻打\n[ffb74f]“狗蛋部落”[-]，抢回我们的资源。[00c0ff]请修建一个士兵的“集结地”。[-]"},
        {8.1f,"别急！\n我们还需要一个[ffb74f]训练士兵[-]的建筑，那么下面我们再来[00c0ff]修建一个“兵营”[-]。"},
        {8.2f,"有了[ffb74f]“兵营”[-]和[ffb74f]“集结地”[-]后我们就可以[ffb74f]训练自己的军队[-]了，下面我们来一起[00c0ff]训练士兵[-]吧。"},
        {9.1f,"非常好！现在我们有了一批强壮的[ffb74f]“狂战士”[-]，复仇的时刻来了！"},
        {9.2f,"在出发前必须告诉您关于[ffb74f]进攻的注意事项[-]。首先进攻需要[ffb74f]花费少量的金币[-]作为军费，其次在战场中[ffb74f]放下的士兵将不能跟随您回到部落[-]。"},
        {9.3f,"万事俱备，向敌人发起冲锋吧！！！"},
        {9.4f,"这就是刚才洗劫我们的\n[ffb74f]“狗蛋部落”[-]。部落里空无一人，这帮坏蛋一定又是去洗劫其他部落了。对我们来说真是好机会！"},
		{9.41f,"先看看[ffb74f]“狗蛋部落”[-]所能让我们[ffb74f]掠夺的全部战利品[-]。是不是很诱人？让我们打起精神大干一场！！！"},
        {9.5f,"不好！！！\n敌人修建了[ffb74f]“炮台”[-]来进行防御。炮台是防御地面部队的利器，[ffb74f]单体攻击非常高[-]。最好[00c0ff]先攻陷炮台[-]。"},
        {10.1f,"真不愧为首领，这么快就解决了敌人。接下来让我们也[00c0ff]修建一个“炮台”[-]。"},
        {10.2f,"很好，有了“炮台”就不怕敌人进攻了。\n对了！我们的/star[ffb74f]部落等级[-]提升了，可以[00c0ff]升级市政厅[-]了。记住，[ffb74f]市政厅等级不能超过部落等级[-]哦！"},//delete
        {11.1f,"恭喜！\n[ffb74f]“市政厅”升级到2级了[-]。[ffb74f]其他建筑[-]也可以升级了。\n[ffb74f]其他建筑的等级[-]是[ffb74f]不能超过市政厅等级[-]的。"},
        {12.1f,"[ffb74f]工人的数量[-]决定了部落发展的速度。\n[00c0ff]用宝石来修建一座“工棚”[-]。这样就可以[ffb74f]同时修建两座建筑[-]了。"},
        {12.2f,"工棚已修建完毕。告诉您一个秘密！[ffb74f]工棚是可以升级[-]的哦！[ffb74f]升级工棚[-]能使[ffb74f]工人[-]的[ffb74f]工作效率成倍增长！！！极大减少建筑修建和升级的时间[-]。"},
        
        {13.1f,"到目前为止，您还需知道如何[ffb74f]提升部队的战斗力[-]。首先我们需要[ffb74f]升级兵营[-]的等级，升级兵营不但可以[ffb74f]解锁新兵种！[-]还能[ffb74f]升级士兵！[-]"},//delete
        {13.2f,"好了，差不多我能告诉您的也就这么多了，[ffb74f]升级士兵[-]在您以后的发展中再慢慢研究吧。"},//delete

        {13.3f,"下面该轮到介绍最有趣的事情了—[ffb74f]“道具”！\n[-]首先我们来\n[00c0ff]建造一个“道具仓库”。[-]"},//add
        {13.4f,"有了[ffb74f]“道具仓库”[-]后，我们就可以[00c0ff]存放道具[-]了，下面我来教你最快获得道具方法！"},//modify
        {13.41f,"想必您也看到刚刚植物所包含的道具了吧，下面我们就来看看能获得什么样的道具！"},//add

        {13.5f,"哇~哦，看到刚刚出现的[ffb74f]宝箱[-]了吗？里面有[ffb74f]各式各样的道具[-]。挖掘[ffb74f]不同的植物[-]可以[ffb74f]获得不同的道具[-]，下面我们就去看看刚刚挖到的是什么道具吧！"},//add
        {13.6f,"这就是刚刚获得的道具，道具获得后并[ffb74f]不能马上使用[-]，必须等到[ffb74f]冷却时间结束后它才真正属于您。[-]"},//add
        {13.7f,"在这期间如果[ffb74f]道具仓库被敌人摧毁[-]，那么处于[ffb74f]冷却中的道具[-]会被敌人[00c0ff]随机抢走一件。[-]"},//add

        {14.1f,"再废话一下！看到[ffb74f]左边的图标[-]了吗？这是[ffb74f]任务列表[-]，是您[ffb74f]今后的目标[-]。\n[00c0ff]打开来看看吧！[-]"},
        {14.2f,"这就是任务，[ffb74f]完成任务后可以领取奖励[-]。"},
        {14.3f,"对了！[ffb74f]主界面左侧还有折叠的窗口[-]，里面可以查看[ffb74f]荣誉排行榜，进攻防守日志，战斗回放[-]等。"},
        {14.4f,"好了，祝您游戏开心。加油，人民群众需要你！"}
        //{14.3f,"最后再告诉您一个秘密。[ffb74f]移除障碍物[-]（仙人掌，石头等），[ffb74f]可以获得宝石[-]。好了，祝您游戏开心。加油，你的人民需要你！"}//delete
    };
    public static Dictionary<int, string> SENSITIVE_WORD = new Dictionary<int, string>() 
    {
        {0,"钓鱼岛"},
        {1,"尖阁列岛"},
        {2,"釣魚島"},
        {3,"尖閣列島"}
    };
}
