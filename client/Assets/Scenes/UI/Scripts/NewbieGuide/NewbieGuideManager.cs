using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class NewbieGuideManager : MonoBehaviour {
    [SerializeField] GameObject[] m_MainUIController;//0 = gold ;1 = food 2 = gem; 3 = buy building;4 = shopping;5 = exp; 6 = honour; 7 = task; 8 = attack  9 = popmenu;10 = builder; 11 = shield;
    [SerializeField] GameObject[] m_MainUIControllerParent;// 0 = gold,food,gem;1 = exp,honor,task;2 = shopping,construct;3 =popmenu,attack;4 = builder,sheild;
    public GameObject[] MainUIControllerParent { get { return m_MainUIControllerParent; } }
    public GameObject[] MainUIController { get { return m_MainUIController; } }
    [SerializeField] GameObject[] m_WindowUIController;//0 = CBW:resource;1 = BBW:buy goldmine; 2 = SBW:builder ; 3 = BBW:buy goldstorage; 4 = BBW:buy farm; 5 = CPW:cost btn; 6 = BBW:buy gold storage; 7 = BBW:roll panel resource; 8 = CBW:Military;9 = BBW:buy Army camp;10 =BBW:scroll Panel Military ; 11 = BBW:buy barracks;12 = CBW:defense;13 = BBW:buy fortress;14 = BBW:scroll Panel Defense; 15 =  BBW:buy builderhut; 16 = BBW:builder; 17 = btn buy army ; 18 = btn buy army Finish now ;19 = btn buy army information;20 = CBW:Composite;21 =BBW:scroll Panel Composite;22 =BBW:buy propStorage;23 = PSW:close;24 = SPW: = Plant;25 = SPW: Buy Plant conform
   
    public GameObject[] WindowUIController { get { return m_WindowUIController; } }
    [SerializeField] UISprite[] m_MainUIBarSprite;//0 = exp bar;1 = gold bar; 2 = food bar;
    public UISprite[] MainUIBarSprite { get { return m_MainUIBarSprite; } }
    public UILabel[] m_MainUIBarLabel;//0 = gold max label name; 1 =gold max label value 2 = food max label name; 3 = food max label value;
    public UILabel[] MainUIBarLabel { get { return m_MainUIBarLabel; } }
    public Color[] MainUIBarSpriteColor { get; set; }
    [SerializeField] GameObject[] m_PopupController;// 0 = btn parent ;1 = btn Finish now; 2 = btn Upgrade; 3 = TrainTroops ;4 = btn removeobstacel ;5 = btn propsStorage
    public GameObject[] PopupController { get { return m_PopupController; } }
    [SerializeField] GameObject[] m_CellGrid;//0 =PSW:All props 
    public GameObject[] CellGrid { get { return m_CellGrid; } } 
    [SerializeField] UIWindowLogin m_UIWindowLogin;
    public UIWindowLogin UIWindowLogin { get { return m_UIWindowLogin; } }
    [SerializeField] UIWindowGuide m_UIWindowGuide;
    public UIWindowGuide UIWindowGuide { get { return m_UIWindowGuide; } }

    [SerializeField] GameObject[] m_UILayerGuideArrowPrefab;// 0 = down 1 = up 2 = left 3 = right
    public GameObject[] UILayerGuideArrowPrefab { get { return m_UILayerGuideArrowPrefab; } }
    [SerializeField] GameObject[] m_SceneLayerGuideArrowPrefab;// 0 = down 1 = up 2 = left 3 = right
    public GameObject[] SceneLayerGuideArrowPrefab { get { return m_SceneLayerGuideArrowPrefab; } }
    public GameObject CurrentGuideArrow { get; set; }
    [SerializeField] Vector3[] m_GuideArrowOffset;
    public Vector3[] GuideArrowOffset { get { return m_GuideArrowOffset; } }

    [SerializeField] UICamera[] m_UICameraInput;//0 = uiLayer ; 1 = sceneLayer;
    public UICamera[] UICameraInput { get { return m_UICameraInput; } }

    static NewbieGuideManager s_Instance;
    public static NewbieGuideManager Instance { get { return s_Instance; } }
    int m_NewbieProgresLength;
    int m_CurrentNewbieProgress = -1;
    public int CurrentNewbieProgress { get { return m_CurrentNewbieProgress; } }
    Func<int,float, string> a;
    event Func<bool> UpdateEvent;
    Queue<Func<bool>> UpdateEventNext = new Queue<Func<bool>>();
    Dictionary<int, Action> m_GuideDictionary = new Dictionary<int, Action>();
    int m_TimeTick = 0;
    void Awake()
    {  
        s_Instance = this;
        this.GetProgressBarColor();
    }
	// Use this for initialization
	void Start () 
    {
	}
    void LateUpdate()
    {
        if (m_TimeTick == 1)
            this.InitialNewbieGuide();
        m_TimeTick++;
    }
 
    void FixedUpdate()
    {
        if (this.UpdateEvent != null)
        {
            bool state = this.UpdateEvent();
            if (state)
            {
                this.UpdateEvent = null;
                if (this.UpdateEventNext.Count > 0)
                    this.UpdateEvent = this.UpdateEventNext.Dequeue();
            }
        }
    }
    public void ClearEventQueue()
    {
        this.UpdateEvent = null;
        this.UpdateEventNext.Clear();
    }
    public void Waiting(int delayFrame, Action task)
    {
        int timeTick = 0;
        this.TriggerCondition(() => { timeTick++; return timeTick > delayFrame; }, () => { task.Invoke(); });
    }
    public void TriggerCondition(Func<bool> condition, Action task)
    {
        if (this.UpdateEvent == null)
            this.UpdateEvent += () =>
            {
                bool state = (bool)condition.Invoke();
                if (state)
                    task.Invoke();
                return state;
            };
        else
        {
            this.UpdateEventNext.Enqueue(() =>
            {
                bool state = (bool)condition.Invoke();
                if (state)
                    task.Invoke();
                return state;
            });
        }
    }
    void InitialNewbieGuide()
    {
        Action[] guideArray = SystemFunction.ConverTObjectToArray(
                                                                  (new Introduction()).OnIntroduction,
                                                                  (new IntroLogin()).OnIntroLogin,
                                                                  (new IntroConstructGoldMine()).OnIntroConstructGoldMine,
                                                                  (new IntroConstructGoldMine()).OnConstructGoldMine,
                                                                  (new IntroConstructGoldStorage()).OnIntroConstructGoldStorage,
                                                                  (new IntroConstructGoldStorage()).OnConstructGoldStorage,
                                                                  (new IntroConstructFarm()).OnIntroConstructFarm,
                                                                  (new IntroConstructFarm()).OnConstructFarm,
                                                                  (new IntroConstructFoodStorage()).OnIntroConstructFoodStorage,
                                                                  (new IntroConstructFoodStorage()).OnConstructFoodStorage,
                                                                  (new IntroConstructArmyCamp()).OnIntroConstructArmyCamp,
                                                                  (new IntroConstructArmyCamp()).OnConstructArmyCamp,
                                                                  (new IntroConstructBarracks()).OnIntroConstructBarracks,
                                                                  (new IntroConstructBarracks()).OnConstructBarracks,
                                                                  (new IntroConstructBarracks()).OnProduceArmy,
                                                                  (new IntroAttack()).OnIntroAttack,
                                                                  (new IntroConstructFortress()).OnIntroConstructFortress,
                                                                  (new IntroConstructFortress()).OnConstructFortress,
                                                                  (new IntroUpgradeCityHall()).OnIntroUpgradeCityHall,
                                                                  (new IntroUpgradeCityHall()).OnUpgradeCityHall,
                                                                  (new IntroConstructBuilderHut()).OnIntroConstructBuilderHut,
                                                                  (new IntroPropsStorage()).OnIntroPropsStorage,
                                                                  (new IntroPropsStorage()).OnConstructPropsStorage,
                                                                  (new IntroPropsStorage()).OnIntroPlant,
                                                                  (new IntroPropsStorage()).OnIntroRemoveObstacle,
                                                                  (new IntroPropsStorage()).OnRemoveObstacle,
                                                                  ////(new IntroUpgradeBarracks()).OnIntroUpgradeBarracks,
                                                                  ////(new IntroUpgradeBarracks()).OnUpgradeBarracks,
                                                                  (new IntroTask()).OnIntroTask
                                                                  );

        this.m_NewbieProgresLength = guideArray.Length;
        for (int i = 0; i < guideArray.Length; i++)
            m_GuideDictionary.Add(i, guideArray[i]);
        this.InvokeNextGuide();
    }
    public void InvokeNextGuide()
    {
        this.m_CurrentNewbieProgress++;
        print("m_CurrentNewbieProgress =" + m_CurrentNewbieProgress);
        if (this.m_CurrentNewbieProgress < this.m_NewbieProgresLength)
            this.m_GuideDictionary[this.m_CurrentNewbieProgress].Invoke();
        
    }
    void GetProgressBarColor()
    {
        this.MainUIBarSpriteColor = new Color[m_MainUIBarSprite.Length];
        for (int i = 0; i < m_MainUIBarSprite.Length; i++)
            MainUIBarSpriteColor[i] = m_MainUIBarSprite[i].color;
    }
}
