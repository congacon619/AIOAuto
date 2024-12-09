using System.Windows.Forms;

namespace AIOAuto.Common.Models
{
    public class CTreeView : TreeView
    {
        protected override CreateParams CreateParams
        {
            get
            {
                var pCreateParams = base.CreateParams;
                pCreateParams.Style |= 0x80; // Turn on TVS_NOTOOLTIPS
                return pCreateParams;
            }
        }
    }
}