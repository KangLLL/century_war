using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIWindowBuyGem : UIWindowCommon {
    [SerializeField] UIBuyGemModule m_UIBuyGemModule;
    [SerializeField] UILabel[] m_UILabelText;//0=title;1=gold;2=food;3=gem;
	[SerializeField] GameObject m_ActivatorView;
	[SerializeField] GameObject m_ActivatorBackground;
	
	private bool m_IsRegistered;
	
    void Awake()
    {
        this.GetTweenComponent();
    }
	
    public override void ShowWindow()
    {
        DestroyImmediate(this.m_UIBuyGemModule.GetComponent<SpringPanel>());
		if(CommonHelper.PlatformType == ConfigUtilities.Enums.PlatformType.Nd)
		{
	        if (!this.m_IsRegistered)
	        {
	            NdShopUtility.Instance.RegisterPurchaseSuccessedListener(this, "OnPurchaseSuccess");
	            NdShopUtility.Instance.RegisterPurchaseFailedListener(this, "OnPurchaseFail");
				NdShopUtility.Instance.RegisterStateChangeListener(this,"OnStateChange");
	            this.m_IsRegistered = true;
	        }
			NdShopUtility.Instance.Initialize();
	        if (NdShopUtility.Instance.State == ShopActionState.Operating)
	        {
	            LockScreen.Instance.DisableInput();
				this.m_ActivatorView.SetActive(true);
				this.m_ActivatorBackground.SetActive(true);
	        }
			this.m_UIBuyGemModule.SetModulItem();
		}
		else if(CommonHelper.PlatformType == ConfigUtilities.Enums.PlatformType.iOS)
		{
			if(iOSShopUtility.Instance.CanMakePayment)
			{
				if(!this.m_IsRegistered)
				{
					iOSShopUtility.Instance.RegisterRequestSuccessedListener(this, "OnRequestSuccess");
					iOSShopUtility.Instance.RegisterRequestFailedListener(this, "OnRequestFail");
					iOSShopUtility.Instance.RegisterPurchaseSuccessedListener(this, "OnPurchaseSuccess");
		            iOSShopUtility.Instance.RegisterPurchaseFailedListener(this, "OnPurchaseFail");
					iOSShopUtility.Instance.RegisterStateChangeListener(this,"OnStateChange");
					this.m_IsRegistered = true;
				}
				iOSShopUtility.Instance.Initialize();
				
				if(iOSShopUtility.Instance.State != ShopActionState.Operating && iOSShopUtility.Instance.ShopItems == null)
				{
					((iOSShopUtility)(iOSShopUtility.Instance)).RequestProduct();
					this.m_UIBuyGemModule.DestroyItems();
				}
				else
				{
					this.m_UIBuyGemModule.SetModulItem();
				}
			
				if(iOSShopUtility.Instance.State == ShopActionState.Operating)
				{
					LockScreen.Instance.DisableInput();
                    this.m_ActivatorView.SetActive(true);
					this.m_ActivatorBackground.SetActive(true);
				}
			}
		}
		
        UIManager.Instance.UIWindowMain.gameObject.SetActive(false);
        
        base.ShowWindowImmediately();
        this.SetWindowModul();
    }
	
	private void OnRequestFail(string errorString)
	{
		this.m_ActivatorView.SetActive(false);
		this.m_ActivatorBackground.SetActive(false);
		UIErrorMessage.Instance.ErrorMessage(errorString);
		LockScreen.Instance.EnableInput();
	}
	
	private void OnRequestSuccess(List<ShopItemInformation> shopItems)
	{
		this.m_ActivatorView.SetActive(false);
		this.m_ActivatorBackground.SetActive(false);
		LockScreen.Instance.EnableInput();
		this.m_UIBuyGemModule.SetModulItem();
	}
	
	private void OnPurchaseSuccess(ShopItemInformation shopItem)
	{
		LogicController.Instance.Purchase(shopItem);
		LockScreen.Instance.EnableInput();
        m_UILabelText[3].text = LogicController.Instance.PlayerData.CurrentStoreGem.ToString();
		UIErrorMessage.Instance.ErrorMessage(ClientStringConstants.PURCHASE_SUCCESSFUL_TIPS, ClientSystemConstants.PURCHASE_SUCCESS_TIPS_COLOR);
		AudioController.Play("DiamondsCollect");
		
		if(CommonHelper.PlatformType == ConfigUtilities.Enums.PlatformType.iOS && 
			iOSShopUtility.Instance.ShopItems == null)
		{
			((iOSShopUtility)iOSShopUtility.Instance).RequestProduct();
		}
	}
	
	private void OnPurchaseFail(PurchaseFailInformation failInformation)
	{
		UIErrorMessage.Instance.ErrorMessage(failInformation.ErrorDescription);
		LockScreen.Instance.EnableInput();
		
		if(CommonHelper.PlatformType == ConfigUtilities.Enums.PlatformType.iOS &&
			iOSShopUtility.Instance.ShopItems == null)
		{
			((iOSShopUtility)iOSShopUtility.Instance).RequestProduct();
		}
	}
	
	private void OnStateChange(ContextChangeParameter changeInformation)
	{
		if(changeInformation.ToContext is NdConfirmContext)
		{
			this.m_ActivatorView.SetActive(true);
			this.m_ActivatorBackground.SetActive(true);
		}
		else if(changeInformation.FromContext is NdConfirmContext)
		{
			this.m_ActivatorView.SetActive(false);
			this.m_ActivatorBackground.SetActive(false);
		}
		else if(changeInformation.ToContext is iOSIdleContext)
		{
			this.m_ActivatorView.SetActive(false);
			this.m_ActivatorBackground.SetActive(false);
		}
		else if(changeInformation.FromContext is iOSIdleContext)
		{
			this.m_ActivatorView.SetActive(true);
			this.m_ActivatorBackground.SetActive(true);
		}
	}

    public override void HideWindow()
    {
        base.HideWindowImmediately(true, true);
        UIManager.Instance.UIWindowMain.gameObject.SetActive(true);
    }
    protected override void GetTweenComponent()
    {
        base.GetTweenComponent();
    }
    void SetWindowModul()
    {
        //m_UILabelText[0].text = ClientSystemConstants.UIMENU_TYPE_DICTIONARY[uiMenuType];
        m_UILabelText[1].text = LogicController.Instance.PlayerData.CurrentStoreGold.ToString();
        m_UILabelText[2].text = LogicController.Instance.PlayerData.CurrentStoreFood.ToString();
        m_UILabelText[3].text = LogicController.Instance.PlayerData.CurrentStoreGem.ToString();
    }
}
