using System;
namespace UtilityNet
{
    [Serializable]
    public struct AjaxResult
    {
        public string Message;
        public object Data;
        public bool Success;
        public bool NeedPassport;
    }
}