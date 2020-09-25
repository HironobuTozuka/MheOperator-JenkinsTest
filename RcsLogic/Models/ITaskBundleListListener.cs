using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;

namespace RcsLogic.Models
{
    public interface ITaskBundleListListener
    {
        public void ListChanged(NotifyCollectionChangedAction collectionChangedAction);
    }
}