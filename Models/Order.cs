using FoodOrderAPI.Models;

public class Order
{
    public int OrderID { get; set; }
    public int CustomerID { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; }

    // 導覽屬性：訂單的所有訂單項目
    public ICollection<OrderItem> OrderItems { get; set; }

    // 導覽屬性：關聯的顧客
    public Customer Customer { get; set; }
}
