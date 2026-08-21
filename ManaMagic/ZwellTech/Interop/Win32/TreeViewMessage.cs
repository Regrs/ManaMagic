#nullable enable

namespace ZwellTech.Interop.Win32
{
    /// <summary>
    /// Specifies messages for a TreeView control.
    /// </summary>
    public enum TreeViewMessage
    {
        /// <summary>
        /// Inserts a new item in a tree-view control. This is the ASCII version of this message.
        /// </summary>
        InsertItemA = (MessageRange.TreeViewMessageId + 0x0000),
        /// <summary>
        /// Inserts a new item in a tree-view control. This is the Unicode version of this message.
        /// </summary>
        InsertItemW = (MessageRange.TreeViewMessageId + 0x0032),
        /// <summary>
        /// Removes an item and all its children from a tree-view control. 
        /// </summary>
        DeleteItem = (MessageRange.TreeViewMessageId + 0x0001),
        /// <summary>
        /// The TVM_EXPAND message expands or collapses the list of child items associated with the specified parent item, if any.
        /// </summary>
        Expand = (MessageRange.TreeViewMessageId + 0x0002),
        /// <summary>
        /// Retrieves the bounding rectangle for a tree-view item and indicates whether the item is visible.
        /// </summary>
        GetItemRect = (MessageRange.TreeViewMessageId + 0x0004),
        /// <summary>
        /// Retrieves a count of the items in a tree-view control.
        /// </summary>
        GetCount = (MessageRange.TreeViewMessageId + 0x0005),
        /// <summary>
        /// Retrieves the amount, in pixels, that child items are indented relative to their parent items.
        /// </summary>
        GetIndent = (MessageRange.TreeViewMessageId + 0x0006),
        /// <summary>
        /// Sets the width of indentation for a tree-view control and redraws the control to reflect the new width.
        /// </summary>
        SetIndent = (MessageRange.TreeViewMessageId + 0x0007),
        /// <summary>
        /// Retrieves the handle to the normal or state image list associated with a tree-view control.
        /// </summary>
        GetImageList = (MessageRange.TreeViewMessageId + 0x0008),
        /// <summary>
        /// Sets the normal or state image list for a tree-view control and redraws the control using the new images.
        /// </summary>
        SetImageList = (MessageRange.TreeViewMessageId + 0x0009),
        /// <summary>
        /// Retrieves the tree-view item that bears the specified relationship to a specified item.
        /// </summary>
        GetNextItem = (MessageRange.TreeViewMessageId + 0x000A),
        /// <summary>
        /// Selects the specified tree-view item, scrolls the item into view, or redraws the item in the style used to indicate the target of a drag-and-drop operation.
        /// </summary>
        SelectItem = (MessageRange.TreeViewMessageId + 0x000B),
        /// <summary>
        /// Retrieves some or all of a tree-view item's attributes. This is the ASCII version of this message.
        /// </summary>
        GetItemA = (MessageRange.TreeViewMessageId + 0x000C),
        /// <summary>
        /// Retrieves some or all of a tree-view item's attributes. This is the Unicode version of this message.
        /// </summary>
        GetItemW = (MessageRange.TreeViewMessageId + 0x003E),
        /// <summary>
        /// The TVM_SETITEM message sets some or all of a tree-view item's attributes. This is the ASCII version of this message.
        /// </summary>
        SetItemA = (MessageRange.TreeViewMessageId + 0x000D),
        /// <summary>
        /// The TVM_SETITEM message sets some or all of a tree-view item's attributes. This is the Unicode version of this message.
        /// </summary>
        SetItemW = (MessageRange.TreeViewMessageId + 0x003F),
        /// <summary>
        /// Begins in-place editing of the specified item's text, replacing the text of the item with a single-line edit control containing the text. This message implicitly selects and focuses the specified item. This is the ASCII version of this message.
        /// </summary>
        EditLabelA = (MessageRange.TreeViewMessageId + 0x000E),
        /// <summary>
        /// Begins in-place editing of the specified item's text, replacing the text of the item with a single-line edit control containing the text. This message implicitly selects and focuses the specified item. This is the Unicode version of this message.
        /// </summary>
        EditLabelW = (MessageRange.TreeViewMessageId + 0x0041),
        /// <summary>
        /// Retrieves the handle to the edit control being used to edit a tree-view item's text.
        /// </summary>
        GetEditControl = (MessageRange.TreeViewMessageId + 0x000F),
        /// <summary>
        /// Obtains the number of items that can be fully visible in the client window of a tree-view control.
        /// </summary>
        GetVisibleCount = (MessageRange.TreeViewMessageId + 0x0010),
        /// <summary>
        /// Determines the location of the specified point relative to the client area of a tree-view control.
        /// </summary>
        HitTest = (MessageRange.TreeViewMessageId + 0x0011),
        /// <summary>
        /// Creates a dragging bitmap for the specified item in a tree-view control. The message also creates an image list for the bitmap and adds the bitmap to the image list. An application can display the image when dragging the item by using the image list functions.
        /// </summary>
        CreatedRagImage = (MessageRange.TreeViewMessageId + 0x0012),
        /// <summary>
        /// Sorts the child items of the specified parent item in a tree-view control.
        /// </summary>
        SortChildren = (MessageRange.TreeViewMessageId + 0x0013),
        /// <summary>
        /// Ensures that a tree-view item is visible, expanding the parent item or scrolling the tree-view control, if necessary.
        /// </summary>
        EnsureVisible = (MessageRange.TreeViewMessageId + 0x0014),
        /// <summary>
        /// Sorts tree-view items using an application-defined callback function that compares the items.
        /// </summary>
        SortChildrenCB = (MessageRange.TreeViewMessageId + 0x0015),
        /// <summary>
        /// Ends the editing of a tree-view item's label.
        /// </summary>
        EndEditLabelNow = (MessageRange.TreeViewMessageId + 0x0016),
        /// <summary>
        /// Retrieves the incremental search string for a tree-view control. The tree-view control uses the incremental search string to select an item based on characters typed by the user. This is the ASCII version of this message.
        /// </summary>
        GetSearchStringA = (MessageRange.TreeViewMessageId + 0x0017),
        /// <summary>
        /// Retrieves the incremental search string for a tree-view control. The tree-view control uses the incremental search string to select an item based on characters typed by the user. This is the Unicode version of this message.
        /// </summary>
        GetSearchStringW = (MessageRange.TreeViewMessageId + 0x0040),
        /// <summary>
        /// Sets a tree-view control's child tooltip control.
        /// </summary>
        SetToolTips = (MessageRange.TreeViewMessageId + 0x0018),
        /// <summary>
        /// Retrieves the handle to the child tooltip control used by a tree-view control.
        /// </summary>
        GetToolTips = (MessageRange.TreeViewMessageId + 0x0019),
        /// <summary>
        /// Sets the insertion mark in a tree-view control.
        /// </summary>
        SetInsertMark = (MessageRange.TreeViewMessageId + 0x001A),
        /// <summary>
        /// Sets the height of the tree-view items.
        /// </summary>
        SetItemHeight = (MessageRange.TreeViewMessageId + 0x001B),
        /// <summary>
        /// Retrieves the current height of the each tree-view item.
        /// </summary>
        GetItemHeight = (MessageRange.TreeViewMessageId + 0x001C),
        /// <summary>
        /// Sets the background color of the control.
        /// </summary>
        SetBackgroundColor = (MessageRange.TreeViewMessageId + 0x001D),
        /// <summary>
        /// Sets the text color of the control.
        /// </summary>
        SetTextColor = (MessageRange.TreeViewMessageId + 0x001E),
        /// <summary>
        /// Retrieves the current background color of the control.
        /// </summary>
        GetBackgroundColor = (MessageRange.TreeViewMessageId + 0x001F),
        /// <summary>
        /// Retrieves the current text color of the control.
        /// </summary>
        GetTextColor = (MessageRange.TreeViewMessageId + 0x0020),
        /// <summary>
        /// Sets the maximum scroll time for the tree-view control.
        /// </summary>
        SetScrollTime = (MessageRange.TreeViewMessageId + 0x0021),
        /// <summary>
        /// Retrieves the maximum scroll time for the tree-view control.
        /// </summary>
        GetScrollTime = (MessageRange.TreeViewMessageId + 0x0022),
        /// <summary>
        /// Sets the size of the border for the items in a tree-view control.
        /// </summary>
        SetBorderColor = (MessageRange.TreeViewMessageId + 0x0023),
        /// <summary>
        /// Sets the color used to draw the insertion mark for the tree view.
        /// </summary>
        SetInsertMarkColor = (MessageRange.TreeViewMessageId + 0x0025),
        /// <summary>
        /// Retrieves the color used to draw the insertion mark for the tree view.
        /// </summary>
        GetInsertMarkColor = (MessageRange.TreeViewMessageId + 0x0026),
        /// <summary>
        /// Retrieves some or all of a tree-view item's state attributes.
        /// </summary>
        GetItemState = (MessageRange.TreeViewMessageId + 0x0027),
        /// <summary>
        /// Sets the current line color.
        /// </summary>
        SetLineColor = (MessageRange.TreeViewMessageId + 0x0028),
        /// <summary>
        /// Gets the current line color.
        /// </summary>
        GetLineColor = (MessageRange.TreeViewMessageId + 0x0029),
        /// <summary>
        /// Maps an accessibility ID to an HTREEITEM.
        /// </summary>
        MapAccIdToHTreeItem = (MessageRange.TreeViewMessageId + 0x002A),
        /// <summary>
        /// Maps an HTREEITEM to an accessibility ID.
        /// </summary>
        MapHTreeItemToAccId = (MessageRange.TreeViewMessageId + 0x002B),
        /// <summary>
        /// Informs the tree-view control to set extended styles.
        /// </summary>
        SetExtendedStyle = (MessageRange.TreeViewMessageId + 0x002C),
        /// <summary>
        /// Retrieves the extended style for a tree-view control.
        /// </summary>
        GetExtendedStyle = (MessageRange.TreeViewMessageId + 0x002D),
        /// <summary>
        /// Sets information used to determine auto-scroll characteristics.
        /// </summary>
        SetAutoScrollInfo = (MessageRange.TreeViewMessageId + 0x003B),
        /// <summary>
        /// Sets the hot item for a tree-view control. [Intended for internal use; not recommended for use in applications. This message may not be supported in future versions of Windows.]
        /// </summary>
        SetHot = (MessageRange.TreeViewMessageId + 0x003A),
        /// <summary>
        /// This message is not implemented.
        /// </summary>
        GetSelectedCount = (MessageRange.TreeViewMessageId + 0x0046),
        /// <summary>
        /// Shows the infotip for a specified item in a tree-view control.
        /// </summary>
        ShowInfoToolTip = (MessageRange.TreeViewMessageId + 0x0047),
        /// <summary>
        /// This message is not implemented.
        /// </summary>
        GetItemPartRect = (MessageRange.TreeViewMessageId + 0x0048),
        /// <summary>
        /// Sets the Unicode character format flag for the control. This message allows you to change the character set used by the control at run time rather than having to re-create the control.
        /// </summary>
        SetUnicodeFormat = (MessageRange.CommonControlMessageId + 0x005),
        /// <summary>
        /// Retrieves the Unicode character format flag for the control.
        /// </summary>
        GetUnicodeFormat = (MessageRange.CommonControlMessageId + 0x006),
    }
}