using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Simplic.Localization;
using Telerik.Windows.DragDrop.Behaviors;

namespace Simplic.ServicePlatform.UI
{
    /// <summary>
    /// Custom ListBoxDragDropBehavior that prevents dropping of redundant items.
    /// </summary>
    public class UniqueServiceModuleDragDropBehavior : ListBoxDragDropBehavior
    {
        public override void Drop(DragDropState state)
        {
            var draggedItems = state.DraggedItems as IEnumerable<ServiceModuleViewModel>;
            var draggedModule = draggedItems?.FirstOrDefault();
            if (state.DestinationItemsSource is IEnumerable<ServiceModuleViewModel> modules &&
                modules.Any(m => m.Model.Name == draggedModule?.Model.Name))
            {
                MessageBox.Show(CommonServiceLocator.ServiceLocator.Current.GetInstance<ILocalizationService>().Translate("validation_already_exists", draggedModule?.Model.Name));
                return;
            }
            base.Drop(state);
        }
    }
}
