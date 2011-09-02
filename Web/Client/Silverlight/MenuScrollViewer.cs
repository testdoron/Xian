#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System.Linq;

namespace ClearCanvas.Web.Client.Silverlight
{
    public interface IMenuItemsContainer
    {
        IMenu Parent { get; set; }
    }

    public class MenuScrollViewer : CustomScrollViewer, IMenuItemsContainer
    {
        private IMenu _parent;

        protected override void OnScrolling()
        {
            if (_parent == null)
                return;

            foreach (var item in _parent.Items.OfType<IMenuItem>())
                item.IsExpanded = false;
        }

        #region IMenuItemsContainer Members

        IMenu IMenuItemsContainer.Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        #endregion
    }
}
