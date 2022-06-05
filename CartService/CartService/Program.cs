// See https://aka.ms/new-console-template for more information
using CartService.BLL;

Console.WriteLine("Hello, World!");

ManagerCart managerCart = new ManagerCart();

int cartId = 1;
var item1 = new CartService.DAL.Models.Item()
{
    Name = "Milk",
    Price = 2,
    Image = "milk.png",
    Quantity = 3
};
managerCart.GetItems(cartId).ForEach(v => Console.WriteLine(v.Name));
managerCart.AddItem(cartId, item1);
managerCart.GetItems(cartId).ForEach(v => Console.WriteLine(v.Name));
managerCart.RemoveItem(cartId, item1.Id);
managerCart.GetItems(cartId).ForEach(v => Console.WriteLine(v.Name));