using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FollowFocusInterface.ViewModels
{
    public interface IMainScreenTabItem : IScreen
    {
    }

    public class MainViewTestTabsViewModel : Conductor<IMainScreenTabItem>.Collection.OneActive
    {
        public MainViewTestTabsViewModel(IEnumerable<IMainScreenTabItem> tabs)
        {
            Items.Add(new ViewTabModel("Foo1"));
            Items.Add(new ViewTabModel("Foo2"));
            Items.AddRange(tabs);
        }
    }

    public sealed class ViewTabModel : Screen, IMainScreenTabItem
    {
        public ViewTabModel(string displayName)
        {
            DisplayName = displayName;
        }
    }
}
