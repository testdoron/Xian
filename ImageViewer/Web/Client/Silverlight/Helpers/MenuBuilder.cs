#region License

// Copyright (c) 2010, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ClearCanvas.ImageViewer.Web.Client.Silverlight.Actions;
using ClearCanvas.ImageViewer.Web.Client.Silverlight.AppServiceReference;
using ClearCanvas.Web.Client.Silverlight;

namespace ClearCanvas.ImageViewer.Web.Client.Silverlight.Helpers
{
    internal static class PopupExtensions
    {
        public static void Dispose(this IPopup popup)
        {
            if (popup is IDisposable)
                ((IDisposable)popup).Dispose();
        }
    }

    internal class MenuBuilder
    {
        internal static IPopup BuildContextMenu(WebActionNode model, ServerEventDispatcher dispatcher)
        {
            return BuildContextMenu(model, new ActionDispatcher(dispatcher));
        }

        internal static IPopup BuildContextMenu(WebActionNode model, ActionDispatcher actionDispatcher)
        {
            ContextMenu menu = new ContextMenu();
            return new MenuAdapter(menu, model, actionDispatcher);
        }
    }

    internal class MenuAdapter : PopupProxy, IDisposable
    {
        private readonly ContextMenu _menu;

        public MenuAdapter(ContextMenu menu, WebActionNode model, ActionDispatcher actionDispatcher)
            : base(menu)
        {
            _menu = menu;

            if (model is WebDropDownButtonAction)
            {
                //TODO (CR May 2010): there's probably a more generic way to do this
                WebDropDownButtonAction a = model as WebDropDownButtonAction;
                foreach (WebActionNode node in a.DropDownActions)
                {
                    MenuItem menuItem = BuildMenuItem(node, actionDispatcher);
                    if (menuItem != null)
                        _menu.Items.Add(menuItem);
                }
            }
            else
            {
                foreach (WebActionNode node in model.Children)
                {
                    MenuItem menuItem = BuildMenuItem(node, actionDispatcher);
                    if (menuItem != null)
                        _menu.Items.Add(menuItem);
                }
            }
        }

        public void Dispose()
        {
            foreach (MenuItem item in _menu.Items)
                ReleaseMenuItem(item);
        }

        private void ReleaseMenuItem(MenuItem item)
        {
            MenuItemBinding binding = item.Tag as MenuItemBinding;
            if (binding != null)
                binding.ReleaseDispatcher();

            foreach (MenuItem child in item.Items)
                ReleaseMenuItem(child);
        }

        private static MenuItem BuildMenuItem(WebActionNode node, ActionDispatcher dispatcher)
        {
            MenuItem thisMenu = null;

            if (node.Children != null)
            {
                thisMenu = new MenuItem { Header = node.LocalizedText };

                foreach (WebActionNode subNode in node.Children)
                {
                    if (subNode.Children == null || subNode.Children.Count == 0)
                    {
                        MenuItem menuItem = BuildActionMenuItem(subNode, dispatcher);
                        if (menuItem.IsChecked)
                            thisMenu.IsChecked = true;

                        if (menuItem != null)
                            thisMenu.Items.Add(menuItem);
                    }
                    else
                    {
                        MenuItem menuItem = BuildMenuItem(subNode, dispatcher);
                        if (menuItem != null)
                        {
                            if (menuItem.IsChecked)
                                thisMenu.IsChecked = true;

                            thisMenu.Items.Add(menuItem);
                        }
                    }
                }
            }
            else
            {
                WebAction actionNode = node as WebAction;

                // Skip those that aren't visible
                thisMenu = BuildActionMenuItem(actionNode, dispatcher);
            }

            return thisMenu;
        }

        private static MenuItem BuildActionMenuItem(WebActionNode subNode, ActionDispatcher dispatcher)
        {
            WebAction actionNode = subNode as WebAction;

            MenuItem item = new MenuItem
            {
                IsEnabled = actionNode.Enabled,
                IsChecked = (actionNode is WebClickAction) && (actionNode as WebClickAction).Checked,
                Visibility = actionNode.Visible ? Visibility.Visible : Visibility.Collapsed
            };

            var binding = new MenuItemBinding(actionNode, dispatcher, item);

            binding.SetLabel(actionNode.Label);
            binding.SetIcon();

            item.Tag = binding;

            return item;
        }
    }

    internal class MenuItemBinding : IActionUpdate, IDisposable
    {
        private WebAction _actionItem;
        private ActionDispatcher _actionDispatcher;

        public MenuItemBinding(WebAction action, ActionDispatcher dispatcher, MenuItem item)
        {
            _actionItem = action;
            _actionDispatcher = dispatcher;
            _actionDispatcher.Register(action.Identifier, this);
            item.Click += OnItemClick;
            Item = item;
        }

        private void OnItemClick(object sender, EventArgs e)
        {
            _actionDispatcher.EventDispatcher.DispatchMessage(
                new ActionClickedMessage
                {
                    TargetId = _actionItem.Identifier,
                    Identifier = Guid.NewGuid()
                });
        }

        public void Dispose()
        {
            if (_actionDispatcher != null)
            {
                Item.Click -= OnItemClick;
                ReleaseDispatcher();
                _actionItem = null;
                Item = null;
                _actionDispatcher = null;
            }
        }

        public void ReleaseDispatcher()
        {
            if (_actionDispatcher != null && _actionItem != null)
                _actionDispatcher.Remove(_actionItem.Identifier);
        }

        public MenuItem Item { get; set; }

		public void Update(PropertyChangedEvent e)
        {
            if (e.PropertyName.Equals("Visible"))
            {
				_actionItem.Visible = (bool)e.Value;
				Item.Visibility = _actionItem.Visible ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (e.PropertyName.Equals("Enabled"))
            {
				_actionItem.Enabled = (bool)e.Value;
                Item.IsEnabled = _actionItem.Enabled;
            }
            else if (e.PropertyName.Equals("IconSet"))
            {
				_actionItem.IconSet = e.Value as WebIconSet;
                SetIcon();
            }
            else if (e.PropertyName.Equals("Tooltip"))
            {
				_actionItem.ToolTip = e.Value as string;
            }
            else if (e.PropertyName.Equals("Label"))
            {
				_actionItem.Label = e.Value as string;
                SetLabel(_actionItem.Label);
            }
            else if (e.PropertyName.Equals("Checked"))
            {
				WebClickAction action = _actionItem as WebClickAction;
				action.Checked = (bool)e.Value;
				Item.IsChecked = action.Checked;
            }
        }

        public void SetLabel(string val)
        {
            TextBlock label = new TextBlock();
            if (val.Contains("&"))
            {
                int ampIndex = val.IndexOf("&");
                label.Inlines.Add(new Run { Text = val.Substring(0, ampIndex) });
                label.Inlines.Add(new Run
                {
                    Text = val[ampIndex + 1].ToString(),
                    // TODO (10/19/2010)
                    // Commented out for ticket #7344.  When we start supporting hot keys, this
                    // should be uncommented.
                    //FontSize = 14,
                    //Foreground = new SolidColorBrush(ClearCanvasStyle.ClearCanvasDarkBlue)
                });
                label.Inlines.Add(new Run { Text = val.Substring(ampIndex + 2) });

            }
            else
            {
                label.Text = val;
            }

            Item.Header = label;
        }

        public void SetIcon()
        {
            if (_actionItem.IconSet != null)
            {
                BitmapImage bi = new BitmapImage();
                bi.SetSource(new MemoryStream(_actionItem.IconSet.MediumIcon));
                Item.Icon = bi;
            }
        }
    }
}