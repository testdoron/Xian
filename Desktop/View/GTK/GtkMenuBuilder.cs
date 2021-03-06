#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;
using System.Collections.Generic;
using System.Text;

//using ClearCanvas.Workstation.Model;
//using ClearCanvas.ImageViewer.Actions;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Actions;
using Gtk;

//namespace ClearCanvas.ImageViewer.View.GTK
namespace ClearCanvas.Desktop.View.GTK
{
    public class GtkMenuBuilder
    {
        public static void BuildMenu(MenuShell menu, ActionModelNode node)
        {
            if (node.PathSegment != null)
            {
				
                MenuItem menuItem;
                if (node.Action != null)
                {
                    // this is a leaf node (terminal menu item)
                    menuItem = new ActiveMenuItem((IClickAction)node.Action);
                }
                else
                {
                    // this menu item has a sub menu
					string menuText = node.PathSegment.LocalizedText.Replace('&', '_');
					menuItem = new MenuItem(menuText);
                    menuItem.Submenu = new Menu();
                }

                menu.Append(menuItem);
                menu = (MenuShell)menuItem.Submenu;
            }

            foreach (ActionModelNode child in node.ChildNodes)
            {
                BuildMenu(menu, child);
            }
        }
    }
}
