using FoodOrderAPI.Models;

public class OrderItem
{
    public int OrderItemID { get; set; }
    public int OrderID { get; set; }
    public int MenuItemID { get; set; }
    public int Quantity { get; set; }
    public decimal SubTotal { get; set; }

    // 導覽屬性：關聯的訂單（可選）
    [System.Text.Json.Serialization.JsonIgnore] // 避免循環
    public Order? Order { get; set; }

    // 導覽屬性：關聯的菜單項目
    public MenuItem MenuItem { get; set; }
}
