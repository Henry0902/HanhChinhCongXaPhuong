var expanAll = true;
$(document).ready(function () {
    // Create jqxTree
    $('#jqxTree').jqxTree({ height: 'auto', width: 'auto' });
    // Create and initialize Buttons
    //$('#Add').jqxButton({ height: '25px', width: '100px' });
    //$('#AddBefore').jqxButton({ height: '25px', width: '100px' });
    //$('#AddAfter').jqxButton({ height: '25px', width: '100px' });
    //$('#Update').jqxButton({ height: '25px', width: '100px' });
    //$('#Remove').jqxButton({ height: '25px', width: '100px' });
    //$('#Disable').jqxButton({ height: '25px', width: '100px' });
    //$('#EnableAll').jqxButton({ height: '25px', width: '100px' });
    //$('#Expand').jqxButton({ height: '25px', width: '100px' });
    //$('#Collapse').jqxButton({ height: '25px', width: '100px' });
    if ($('#ExpandAll').length > 0) {
        $('#ExpandAll').jqxButton({ height: '25px', width: '60px' });
    }
    if ($('#CollapseAll').length) {
        $('#CollapseAll').jqxButton({ height: '25px', width: '60px' });
    }
    
    //$('#Next').jqxButton({ height: '25px', width: '100px' });
    //$('#Previous').jqxButton({ height: '25px', width: '100px' });
    // Add 
    $('#Add').click(function () {
        var selectedItem = $('#jqxTree').jqxTree('selectedItem');
        if (selectedItem != null) {
            // adds an item with label: 'item' as a child of the selected item. The last parameter determines whether to refresh the Tree or not.
            // If you want to use the 'addTo' method in a loop, set the last parameter to false and call the 'render' method after the loop.
            $('#jqxTree').jqxTree('addTo', { label: 'Item' }, selectedItem.element, false);
            // update the tree.
            $('#jqxTree').jqxTree('render');
        }
        else {
            $('#jqxTree').jqxTree('addTo', { label: 'Item' }, null, false);
            // update the tree.
            $('#jqxTree').jqxTree('render');
        }
    });
    // Add Before
    $('#AddBefore').click(function () {
        var selectedItem = $('#jqxTree').jqxTree('selectedItem');
        if (selectedItem != null) {
            $('#jqxTree').jqxTree('addBefore', { label: 'Item' }, selectedItem.element, false);
            // update the tree.
            $('#jqxTree').jqxTree('render');
        }
    });
    // Update
    $('#Update').click(function () {
        var selectedItem = $('#jqxTree').jqxTree('selectedItem');
        if (selectedItem != null) {
            $('#jqxTree').jqxTree('updateItem', { label: 'Item' }, selectedItem.element);
            // update the tree.
            $('#jqxTree').jqxTree('render');
        }
    });
    // Add After
    $('#AddAfter').click(function () {
        var selectedItem = $('#jqxTree').jqxTree('selectedItem');
        if (selectedItem != null) {
            $('#jqxTree').jqxTree('addAfter', { label: 'Item' }, selectedItem.element, false);
            // update the tree.
            $('#jqxTree').jqxTree('render');
        }
    });
    // Remove 
    $('#Remove').click(function () {
        var selectedItem = $('#jqxTree').jqxTree('selectedItem');
        if (selectedItem != null) {
            // removes the selected item. The last parameter determines whether to refresh the Tree or not.
            // If you want to use the 'removeItem' method in a loop, set the last parameter to false and call the 'render' method after the loop.
            $('#jqxTree').jqxTree('removeItem', selectedItem.element, false);
            // update the tree.
            $('#jqxTree').jqxTree('render');
        }
    });
    // Disable
    $('#Disable').click(function () {
        var selectedItem = $('#jqxTree').jqxTree('selectedItem');
        if (selectedItem != null) {
            $('#jqxTree').jqxTree('disableItem', selectedItem.element);
        }
    });
    // Expand
    $('#Expand').click(function () {
        var selectedItem = $('#jqxTree').jqxTree('selectedItem');
        if (selectedItem != null) {
            $('#jqxTree').jqxTree('expandItem', selectedItem.element);
        }
    });
    // Expand
    $('#Collapse').click(function () {
        var selectedItem = $('#jqxTree').jqxTree('selectedItem');
        if (selectedItem != null) {
            $('#jqxTree').jqxTree('collapseItem', selectedItem.element);
        }
    });
    // Expand All
    $('#ExpandAll').click(function () {
        if (!expanAll) {
            $('#jqxTree').jqxTree('expandAll');
            expanAll = true;
            $(this).val("Thu nhỏ");
        } else {
            $('#CollapseAll').click();
            expanAll = false;
            $(this).val("Mở rộng");
        }
    });
    // Collapse All
    $('#CollapseAll').click(function () {
        $('#jqxTree').jqxTree('collapseAll');
    });
    // Enable All
    $('#EnableAll').click(function () {
        $('#jqxTree').jqxTree('enableAll');
    });
    // Select Next Item
    $('#Next').click(function () {
        var selectedItem = $("#jqxTree").jqxTree('selectedItem');
        var nextItem = $("#jqxTree").jqxTree('getNextItem', selectedItem.element);
        if (nextItem != null) {
            $("#jqxTree").jqxTree('selectItem', nextItem.element);
            $("#jqxTree").jqxTree('ensureVisible', nextItem.element);
        }
    });
    // Select Previous Item
    $('#Previous').click(function () {
        var selectedItem = $("#jqxTree").jqxTree('selectedItem');
        var prevItem = $("#jqxTree").jqxTree('getPrevItem', selectedItem.element);
        if (prevItem != null) {
            $("#jqxTree").jqxTree('selectItem', prevItem.element);
            $("#jqxTree").jqxTree('ensureVisible', prevItem.element);
        }
    });
    $('#jqxTree').css('visibility', 'visible');
    //
    //$('#jqxTree').jqxTree('selectItem', $("#jqxTree").find('li[id=4]')[0]);
    if ($("#jqxMenu").length > 0) {
        var contextMenu = $("#jqxMenu").jqxMenu({ width: '130px', height: '83px', autoOpenPopup: false, mode: 'popup' });
        var clickedItem = null;

        var attachContextMenu = function () {
            // open the context menu when the user presses the mouse right button.
            $(document).on('mousedown', '#jqxTree li', function (event) {
                var target = $(event.target).parents('li:first')[0];
                var rightClick = isRightClick(event);
                if (rightClick && target != null) {
                    $("#jqxTree").jqxTree('selectItem', target);
                    var scrollTop = $(window).scrollTop();
                    var scrollLeft = $(window).scrollLeft();
                    contextMenu.jqxMenu('open', parseInt(event.clientX) + 5 + scrollLeft, parseInt(event.clientY) + 5 + scrollTop);
                    return false;
                }
            });
        }
        attachContextMenu();

        // disable the default browser's context menu.
        $(document).on('contextmenu', function (e) {
            if ($(e.target).parents('.jqx-tree').length > 0) {
                return false;
            }
            return true;
        });
        function isRightClick(event) {
            var rightclick;
            if (!event) var event = window.event;
            if (event.which) rightclick = (event.which == 3);
            else if (event.button) rightclick = (event.button == 2);
            return rightclick;
        }
    }
    
    
    
});