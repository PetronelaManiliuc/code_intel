using System;
using System.Collections.Generic;

namespace Kata_Invoicing.Infrastructure.DomainBase
{
    [Serializable]
    public abstract class BrokenRuleMessages
    {
        private Dictionary<string, string> messages;

        protected Dictionary<string, string> Messages
        {
            get => this.messages;
        }

        protected BrokenRuleMessages()
        {
            this.messages = new Dictionary<string, string>();
            this.PopulateMessages();
        }

        protected abstract void PopulateMessages();

        public string GetRuleDescription(string messageKey)
        {
            string description = string.Empty;
            if (this.messages.ContainsKey(messageKey))
            {
                description = this.messages[messageKey];
            }
            return description;
        }
    }
}
