using Godot;

namespace Temptica.GodotExtensions;

public static class TreeExtensions
{
    public static async Task SetTextAsync(this TreeItem treeItem, int idx, string text)
    {
        await treeItem.CallAsync(TreeItem.MethodName.SetText, idx, text);
    }

    public static async Task SetEditableAsync(this TreeItem treeItem, int idx, bool isEditable)
    {
        await treeItem.CallAsync(TreeItem.MethodName.SetEditable, idx, isEditable);
    }

    public static async Task SetCellModeAsync(this TreeItem treeItem, int idx, TreeItem.TreeCellMode mode)
    {
        await treeItem.CallAsync(TreeItem.MethodName.SetCellMode, idx, (int)mode);
    }

    public static async Task<TreeItem.TreeCellMode> GetCellModeAsync(this TreeItem treeItem, int idx)
    {
        return (TreeItem.TreeCellMode)(await treeItem.CallAsync(TreeItem.MethodName.GetCellMode, idx, true)).AsInt64();
    }

    public static async Task SetRangeAsync(this TreeItem treeItem, int idx, int range)
    {
        await treeItem.CallAsync(TreeItem.MethodName.SetRange, idx, range);
    }

    public static async Task<double> GetRangeAsync(this TreeItem treeItem, int idx)
    {
        return (await treeItem.CallAsync(TreeItem.MethodName.GetRange, idx)).AsDouble();
    }

    public static async Task<TreeItem> CreateChildAsync(this TreeItem treeItem)
    {
        return (await treeItem.CallAsync(TreeItem.MethodName.CreateChild)).As<TreeItem>();
    }

    public static async Task<TreeItem> CreateItemAsync(this Tree tree)
    {
        return (await tree.CallAsync(Tree.MethodName.CreateItem)).As<TreeItem>();
    }

    public static async Task ClearAsync(this Tree tree)
    {
        await tree.CallAsync(Tree.MethodName.Clear);
    }

    public static async Task SetColumns(this Tree tree, int columns)
    {
        await tree.CallAsync(Tree.MethodName.SetColumns, columns);
    }
}