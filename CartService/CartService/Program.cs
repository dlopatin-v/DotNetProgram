// See https://aka.ms/new-console-template for more information

using CartService.BLL;
using CartService.DAL;
using CartService.DAL.Models;
using Ninject;
using System.Reflection;

StandardKernel kernel = new();
kernel.Load(Assembly.GetExecutingAssembly());
IGatewayCart gatewayCart = kernel.Get<IGatewayCart>();

ManagerCart managerCart = new(gatewayCart);

int cartId = 1;
var item1 = new Item()
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