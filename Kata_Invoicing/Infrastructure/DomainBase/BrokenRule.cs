using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            get { return this.type; }
        }

        public string Name
        {
            get { return this.name; }
        }

        public string Description
        {
            get { return this.description; }
        }
    }
}
