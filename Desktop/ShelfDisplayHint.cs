using System;
using System.Collections.Generic;
using System.Text;

namespace ClearCanvas.Desktop
{
    /// <summary>
    /// Flags that indicate how a shelf should be displayed
    /// </summary>
    [Flags]
    public enum ShelfDisplayHint
    {
		None = 0,
		DockLeft = 1,
		DockRight = 2,
		DockTop = 4,
		DockBottom  = 8,
		DockFloat = 16,
		DockAutoHide = 32,
		MaximizeOnDock = 64,
		HideOnWorkspaceOpen = 128
    }
}
