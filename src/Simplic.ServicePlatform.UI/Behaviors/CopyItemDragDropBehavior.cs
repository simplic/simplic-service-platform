using Telerik.Windows.DragDrop.Behaviors;

namespace Simplic.ServicePlatform.UI
{
    /// <summary>
    /// Custom ListBoxDragDropBehavior that prevents removal of items when drag & drop is completed.
    /// </summary>
    public class CopyItemDragDropBehavior : ListBoxDragDropBehavior
    {
        public override void DragDropCompleted(DragDropState state) { }
    }
}
