using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microcomm.Configuration.Apollo
{
    public sealed class ConfigChangedEventArgs:EventArgs
    {
        public ConfigChangedEventArgs( IEnumerable<ConfigChangedItem> configChangeValues)
        {
            if (configChangeValues == null)
                throw new ArgumentNullException("变更的数据集合为空。");
            this.ChangedItems = configChangeValues;
        }

        public IEnumerable<ConfigChangedItem> ChangedItems { get; private set; }



    }
}
