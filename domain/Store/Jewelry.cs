using System;
using System.Text.RegularExpressions;

namespace Store;
public class Jewelry
{
    public int Id { get; }

    public string VendorCode { get; }

    public string Material { get; }    

    public string Title { get; }    

    public string Description { get; }

    public decimal Price { get; }

    public Jewelry(int id, string vendorCode, string material, string title, string description, decimal price)
    {
        Title = title;
        VendorCode = vendorCode;
        Material = material;
        Id = id;
        Description = description;
        Price = price;  
    }

    internal static bool IsVendorCode(string vendorCode)
    {
        if(vendorCode == null)
        {
            return false;
        }

        vendorCode = vendorCode.Replace(" ", "")
                               .ToUpper();

        return Regex.IsMatch(vendorCode, "^VENDORCODE\\d{10}(\\d{3})?$");

    }
}
