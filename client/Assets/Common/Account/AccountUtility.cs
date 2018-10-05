using UnityEngine;
using System.Collections;

public class AccountUtility : MonoBehaviour 
{
	private string m_MountedAccount;
	
	protected ReceiverManager m_MountSuccessListener;
	protected ReceiverManager m_MountFailListener;
	protected ReceiverManager m_SwitchFailListener;
	protected ReceiverManager m_LogoutSuccessListener;
	protected ReceiverManager m_LogoutFailListener;
	
	public virtual void Awake()
	{
		this.m_MountSuccessListener = new ReceiverManager();
		this.m_MountFailListener = new ReceiverManager();
		this.m_SwitchFailListener = new ReceiverManager();
		this.m_LogoutFailListener = new ReceiverManager();
		this.m_LogoutSuccessListener = new ReceiverManager();
	}
	
	public void RegisterMountFailReceiver(Component receiver, string methodName)
	{
		ReceiverInformation info = new ReceiverInformation() {
			Receiver = receiver, MethodName = methodName, IsListenOnce = false
		};
		this.m_MountFailListener.AddReceiver(info);
	}
	
	public void RegisterMountSuccessReceiver(Component receiver, string methodName)
	{
		ReceiverInformation info = new ReceiverInformation() {
			Receiver = receiver, MethodName = methodName, IsListenOnce = false
		};
		this.m_MountSuccessListener.AddReceiver(info);
	}
	
	public void RegisterSwitchFailReceiver(Component receiver, string methodName)
	{
		ReceiverInformation info = new ReceiverInformation() {
			Receiver = receiver, MethodName = methodName, IsListenOnce = false
		};
		this.m_SwitchFailListener.AddReceiver(info);
	}
	
	public void RegisterLogoutSuccessReceiver(Component receiver, string methodName)
	{
		ReceiverInformation info = new ReceiverInformation(){
			Receiver = receiver, MethodName = methodName, IsListenOnce = false
		};
		this.m_LogoutSuccessListener.AddReceiver(info);
	}
	
	public void RegisterLogoutFailReceiver(Component receiver, string methodName)
	{
		ReceiverInformation info = new ReceiverInformation(){
			Receiver = receiver, MethodName = methodName, IsListenOnce = false
		};
		this.m_LogoutFailListener.AddReceiver(info);
	}
	
	protected void MountAccount(string accountID)
	{
		this.m_MountedAccount = accountID;
		this.m_MountSuccessListener.Invoke(null);
	}
	
	public void InitialAccount(string accountID)
	{
		this.m_MountedAccount = accountID;
	}
	
	public bool IsMounted
	{
		get { return !string.IsNullOrEmpty(this.m_MountedAccount); }
	}
	
	public string MountedAccount
	{
		get { return this.m_MountedAccount; } 
	}
	
	protected void SwitchAccount(string accountID)
	{
		this.m_MountedAccount = accountID;
		Application.LoadLevel(ClientStringConstants.LOADING_SCENE_LEVEL_NAME);
	}
}
