using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Text;

namespace p7_C00231110_C00439696.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
    
    [BindProperty]
    public string Discontinued { get; set; }
    
    [BindProperty]
    public string CountryCus { get; set; }
    
    [BindProperty]
    public string CountrySup { get; set; }
    
    [BindProperty]
    public string Supplier { get; set; }
    
    [BindProperty]
    public string Order { get; set; }
    
}
// 3. a) Check database for discontinued items
public static class IsDiscontinued
{
    public static string getIsDiscontinued(string Disc)
    {
        using var db = new smallbusiness.smallbusiness();
        {
            if (Disc != null)
            {

                var results =
                    from p in db.Products
                    where p.IsDiscontinued.ToString() == "1"
                    select p.ProductName;

                var empty = $"";
                if (!results.Any())
                {
                    return empty;
                }

                string productName = "Products that are discontinued: ";
                foreach (var p in results)
                {
                    productName += $"{p}, ";
                }

                return productName;
            }
            else
            {
                return "";
            }
        }
    }
}

// 3. b) List customer info based on country
    public static class ListCustomersInfo
    {
        public static string getListCustomersInfo(string CountryCus)
        {
            using var db = new smallbusiness.smallbusiness();
            {
                if (CountryCus != null)
                {
                    var results =
                        from co in db.Customers
                        where co.Country == CountryCus
                        select new {co.FirstName, co.LastName, co.Phone};
                    var empty = $"";
                    if (!results.Any())
                    {
                        return empty;
                    }

                    string cusInfo = $"Customers that are in {CountryCus}: ";
                    foreach (var p in results)
                    {
                        cusInfo += $"{p}, ";
                    }

                    return cusInfo;
                }
                else
                {
                    return $"";
                }
                
            }
        }
    }


// 3. c) List supplier info based on country
    public static class ListSupplierInfo
    {
        public static string getListSupplierInfo(string CountrySup)
        {
            using var db = new smallbusiness.smallbusiness();
            {
                if (CountrySup != null)
                {
                    var results =
                        from su in db.Suppliers
                        where su.Country == CountrySup
                        select new {su.Id, su.CompanyName, su.Phone, su.Fax, su.City};
                    var empty = $"";
                    if (!results.Any())
                    {
                        return empty;
                    }

                    string supInfo = $"Suppliers that are in {CountrySup}: ";
                    foreach (var p in results)
                    {
                        supInfo += $"{p}, ";
                    }

                    return supInfo;
                }
                else
                {
                    return $"";
                }
            }
        }
    }

// 3. d) List supported products based on supplier
    public static class isNotDiscontinued
    {
        public static string getIsNotDiscontinued(string Supplier)
        {
            using var db = new smallbusiness.smallbusiness();
            {
                if (Supplier != null)
                {
                    var results =
                        from ss in db.Suppliers
                        join sp in db.Products
                            on ss.Id equals sp.SupplierId
                        where sp.IsDiscontinued.ToString() != "1" && ss.CompanyName == Supplier
                        select new
                        {
                            ss.CompanyName, sp.ProductName, UnitPrice = Encoding.UTF8.GetString(sp.UnitPrice), sp.Package
                        };

                    var empty = $"";
                    if (results.Count() == 0)
                    {
                        //Console.WriteLine($"No supported products from {Supplier}.");
                        return empty;
                    }

                    string sup = $"Products that are not discontinued in {Supplier}: ";
                    foreach (var s in results)
                    {
                        sup += $"{s}, ";
                    }

                    return sup;
                }
                else
                {
                    return $"";
                }


                /* Console.WriteLine($"Products supported by {Supplier}:");
                 foreach (var x in results)
                     Console.WriteLine(" { CompanyName = " + x.CompanyName + ", ProductName = " + x.ProductName +
                                       ", UnitPrice = "
                                       + Encoding.UTF8.GetString(x.UnitPrice) + ", Package = " + x.Package + " } ");
                 Console.WriteLine(); */
            }
        }
    }

// 3. e) List order items based on order number
    public static class Order
    {
        public static string getOrder(string Order)
        {
            using var db = new smallbusiness.smallbusiness();
            {
                if (Order != null)
                {
                    var results =
                        from ot in db.Orders
                        join co in db.Customers
                            on ot.CustomerId equals co.Id
                        join od in db.OrderItems
                            on ot.Id equals od.OrderId
                        join pi in db.Products
                            on od.ProductId equals pi.Id
                        where ot.OrderNumber == Order
                        select new
                        {
                            co.FirstName, co.LastName, pi.ProductName, UnitPrice = Encoding.UTF8.GetString(od.UnitPrice),
                            Subtotal = (double.Parse(Encoding.UTF8.GetString(od.UnitPrice)) * od.Quantity).ToString(),
                            TotalPrice = Encoding.UTF8.GetString(ot.TotalAmount)
                        };

                    var empty = $"";
                    if (results.Count() == 0)
                    {
                        //Console.WriteLine($"No supported products from {Supplier}.");
                        return empty;
                    }

                    string order = $"Items in order {Order}:";
                    foreach (var s in results)
                    {
                        order += $"{s}, ";
                    }

                    return order;
                }
                else
                {
                    return $"";
                }
                

                /*foreach (var y in results)
                {
                    var s = Encoding.UTF8.GetString(y.UnitPrice);
                    var d = double.Parse(s);
                    Console.WriteLine(" { FirstName = " + y.FirstName + ", LastName = " + y.LastName
                                      + ", ProductName = " + y.ProductName + ", UnitPrice = " + s + ", Quantity = "
                                      + y.Quantity + ", Subtotal = " + (d * y.Quantity) + ", TotalAmount = "
                                      + Encoding.UTF8.GetString(y.TotalAmount) + " } ");
                }

                Console.WriteLine();*/
            }
        }
    }

    