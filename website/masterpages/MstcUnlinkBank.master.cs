using System;
using System.Configuration;
using System.Web.Security;
using Mstc.Core.Domain;
using Mstc.Core.Providers;
using Newtonsoft.Json;
using Mstc.Core.Dto;
using cFront.Umbraco;
using System.Collections.Generic;

public partial class masterpages_MstcUnlinkBank : System.Web.UI.MasterPage
{
    protected bool IsUnlinked;

    private SessionProvider _sessionProvider;
    private GoCardlessProvider _goCardlessProvider;
    private MemberProvider _memberProvider;
    EmailProvider _emailProvider;

    public masterpages_MstcUnlinkBank()
    {
        _sessionProvider = new SessionProvider();
        _goCardlessProvider = new GoCardlessProvider();
        _memberProvider = new MemberProvider();
        _emailProvider = new EmailProvider();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
     
    }

    public void btn_UnlinkBankAccount(object sender, EventArgs e)
    {
        var newmemdata = new Dictionary<String, object>();
        newmemdata[MemberProperty.directDebitMandateId] = "";
        MemberHelper.Update(newmemdata);
        IsUnlinked = true;
    }






}
