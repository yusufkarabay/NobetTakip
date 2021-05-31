using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NobetTakip.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if(context.HttpContext.Session.GetString("Nobsis_PersonelId") == null)
            {
                if(context.HttpContext.Request.Cookies["Nobsis_PersonelId"] == null)
                {
                    context.Result = new RedirectResult("/account/login?returnPath=" + context.HttpContext.Request.Path);
                } 
                else 
                { 
                    String realName, personelId, mailAddress, isletmeAdi, isletmeId;
                    personelId = context.HttpContext.Request.Cookies["Nobsis_PersonelId"].ToString();
                    realName = context.HttpContext.Request.Cookies["Nobsis_RealName"].ToString();
                    mailAddress = context.HttpContext.Request.Cookies["Nobsis_MailAddress"].ToString();
                    isletmeAdi = context.HttpContext.Request.Cookies["Nobsis_Isletme"].ToString();
                    isletmeId = context.HttpContext.Request.Cookies["Nobsis_IsletmeId"].ToString();


                    context.HttpContext.Session.SetString("Nobsis_RealName", realName);
                    context.HttpContext.Session.SetString("Nobsis_PersonelId", personelId);
                    context.HttpContext.Session.SetString("Nobsis_MailAddress", mailAddress);
                    context.HttpContext.Session.SetString("Nobsis_Isletme", isletmeAdi);
                    context.HttpContext.Session.SetString("Nobsis_IsletmeId", isletmeId);
                }
            }
        }
    }
}
