namespace V9MvcCoreProject.Entities.Models;

public class MenuItems
{
    public List<Items> MenuItemList { get; set; }
}
public class Items
{
    public string ItemName { get; set; }
    public string Icon { get; set; }
    public List<SubMenuItems> SubMenuItems { get; set; }
}
public class SubMenuItems
{
    public string submenuItem { get; set; }
    public string NavigationLink { get; set; }
}
