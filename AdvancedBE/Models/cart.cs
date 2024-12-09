namespace AdvancedBE.Models
{
public class Cart
{
    public List<CartItem> Items { get; set; } = new List<CartItem>();

    public decimal TotalPrice => Items.Sum(item => item.TotalPrice);

    public void AddItem(int productId, string name, decimal price, int quantity)
    {
        var existingItem = Items.FirstOrDefault(i => i.ProductId == productId);
        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            Items.Add(new CartItem
            {
                ProductId = productId,
                Name = name,
                Price = price,
                Quantity = quantity
            });
        }
    }

    public void RemoveItem(int productId)
    {
        Items.RemoveAll(i => i.ProductId == productId);
    }

    public void ClearCart()
    {
        Items.Clear();
    }
}

}
