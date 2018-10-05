using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FetchContext : ShopContext
{
    public virtual void StartFetch()
    {
        //HttpEvent.Instance.ShopList(OnShopListFetched, this.ShopModule);
    }
	
	
	/*
    private void OnShopListFetched(object sender, ShopListArguments arg)
    {
        if (!string.IsNullOrEmpty(arg.ResponseBaseInfo1.Error))
        {
            this.ShopModule.ChangeContext(new IdleContext());
            this.ShopModule.State = ShopActionState.Fail;
            this.ShopModule.OnRequestFailed(arg.ResponseBaseInfo1.Error);
        }
        else if (arg.ResponseBaseInfo1.ResponseCode != "0")
        {
            Debug.Log(arg.ResponseBaseInfo1.ResponseErrorMessage);
            Debug.Log(arg.ResponseBaseInfo1.ResponseErrorMessageInfo);
            this.ShopModule.ChangeContext(new IdleContext());
            this.ShopModule.State = ShopActionState.Fail;
            this.ShopModule.OnRequestFailed(arg.ResponseBaseInfo1.ResponseErrorMessageInfo);
        }
        else
        {
            List<string> productsID = new List<string>();
            foreach (ShopListDataArguments product in arg.ResponseData)
            {
                productsID.Add(product.Id);
            }
            if (this.IsNeedRequest(productsID))
            {
                RequestingContext requestContext = new RequestingContext();
                this.ShopModule.ChangeContext(requestContext);
                requestContext.StartRequest(productsID, true);
            }
            else
            {
                this.ShopModule.State = ShopActionState.Idle;
                IdleContext idleContext = new IdleContext();
                this.ShopModule.OnRequestSuccessed(this.ShopModule.ShopItems);
                this.ShopModule.ChangeContext(idleContext);
            }
        }
    }
	 */
	
	
    private bool IsNeedRequest(List<string> productsID)
    {
        if (this.ShopModule.ShopItems == null)
        {
            return true;
        }
        foreach (string product in productsID)
        {
            if (!this.ShopModule.ShopItems.Exists((obj) => obj.ProductID.Equals(product)))
            {
                return true;
            }
        }
        return false;
    }
}
