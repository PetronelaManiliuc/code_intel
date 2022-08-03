using System;
using Kata_Invoicing.Infrastructure.DomainBase;

namespace Kata_Invoicing.Model.Settings
{
    [Serializable]
    public class Settings : EntityBase
    {
        public string ExportInvoicesPath { get; set; }


        public new int Key => 0;

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
