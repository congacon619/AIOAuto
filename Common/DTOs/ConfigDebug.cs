namespace AIOAuto.Common.DTOs
{
    public class ConfigDebug
    {
        public bool CkbCheckpoint { get; set; } = false;
        public bool CkbChangedPass { get; set; } = false;
        public bool CkbChangeInfoDevice { get; set; } = false;
        public bool CkbLoginFail { get; set; } = false;
        public bool CkbCheckSpam { get; set; } = false;
    }
}