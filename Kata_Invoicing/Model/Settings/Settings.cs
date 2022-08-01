using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kata_Invoicing.Infrastructure.DomainBase;

namespace Kata_Invoicing.Model.Settings
{
    [Serializable]
    public class Settings : EntityBase
    {
        public string ExportInvoicesPath { get; set; }


        public int Key
        {
            get { return 0; }
        }

        public override void Validate()
        {

        }

        protected override BrokenRuleMessages GetBrokenRuleMessages()
        {
            BrokenRuleMessages brokenRuleMessages = new SettingsRuleMessages();
            return brokenRuleMessages;
        }
    }
}
