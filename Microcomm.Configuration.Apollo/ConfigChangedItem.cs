using Com.Ctrip.Framework.Apollo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microcomm.Configuration.Apollo
{
    public sealed class ConfigChangedItem
    {
        //internal ConfigChangedItem(string nameSpace, 
        //    string propertyName,
        //    string oldValue,
        //    string newValue,
        //    ConfigChangeType changeType)
        //{
        //    this.Namespace = nameSpace;
        //    this.PropertyName = propertyName;
        //    this.OldValue = oldValue;
        //    this.NewValue = newValue;
        //    this.ChangeType = changeType;
        //}

        internal ConfigChangedItem(ConfigChange raw)
        {
            this.Namespace = raw.Namespace;
            this.OldValue = raw.OldValue;
            this.NewValue = raw.NewValue;
            this.PropertyName = raw.PropertyName;
            this.ChangeType = (ConfigChangeType) (int)raw.ChangeType;
        }


        public string PropertyName { get; private set; }

        public string OldValue { get; private set; }

        public string NewValue { get; private set; }

        public ConfigChangeType ChangeType { get;private set; }

        public string Namespace { get; private set; }

        public override bool Equals(object obj)
        {
            var other = obj as ConfigChangedItem;
            if (other == null)
                return false;
            if (this.Namespace != other.Namespace)
                return false;
            return this.PropertyName == other.PropertyName;
        }

    }
}
