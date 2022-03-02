using Telerik.Windows.DragDrop.Behaviors;

namespace Simplic.ServicePlatform.UI
{
    public class CustomListBoxDragDropBehavior : Telerik.Windows.DragDrop.Behaviors.ListBoxDragDropBehavior
	{
        public override void DragDropCompleted(DragDropState state)
        {
            return;
            //base.DragDropCompleted(state);
        }
        public override void Drop(DragDropState state)
		{
			//return;
			base.Drop(state);
		}
	}
}
