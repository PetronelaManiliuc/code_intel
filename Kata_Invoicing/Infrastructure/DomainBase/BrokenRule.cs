using System;

namespace Kata_Invoicing.Infrastructure.DomainBase
{
    public enum BrokenRuleType
    {
        Error,
        Warning
    }

    [Serializable]
    public class BrokenRule
    {
        private BrokenRuleType type;
        private string name;
        private string description;

        public BrokenRule(string name, string description, BrokenRuleType type)
        {
            this.type = type;
            this.name = name;
            this.description = description;
        }

        public BrokenRule()
        {
        }

        public BrokenRuleType Type
        {
            get => this.type;
        }

        public string Name
        {
            get => this.name;
        }

        public string Description
        {
            get => this.description;
        }
    }
}
