using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley.Menus;
using System;
using System.Collections.Generic;

namespace FrugalFarmMenu
{
    internal class Mod : StardewModdingAPI.Mod
    {
        public override void Entry(IModHelper iModHelper)
        {
            iModHelper.Events.Display.MenuChanged += OnMenuChanged;
        }

        private void OnMenuChanged(object sender, MenuChangedEventArgs menuChangedEventArgs)
        {
            try
            {
                if (menuChangedEventArgs.NewMenu is GameMenu gameMenu)
                {
                    ReplaceAll(
                        gameMenu.pages,
                        page => page is StardewValley.Menus.InventoryPage and not InventoryPage,
                        page => new InventoryPage(this, page.xPositionOnScreen, page.yPositionOnScreen, page.width, page.height));
                }
            }
            catch (Exception ex)
            {
                Monitor.Log($"Failed to replace Inventory Page:\n{ex}", LogLevel.Error);
            }
        }

        private static void ReplaceAll<T>(List<T> list, Predicate<T> match, Func<T, T> map)
        {
            list.FindAll(match).ConvertAll(list.IndexOf).ForEach(i => list[i] = map.Invoke(list[i]));
        }
    }
}