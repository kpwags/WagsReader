using System;
using System.Collections.Generic;
using System.Text;

namespace WagsReader.Interfaces
{
    public interface IDeviceInfo
    {
        string GetInoreaderOAuthRedirectUrl();

        bool IsUsingNativeUI();
    }
}
