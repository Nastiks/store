using System;
using System.Text.RegularExpressions;

namespace Store;
public class Jewelry
{
    public int Id { get; }

    public string VendorCode { get; }

    public string Material { get; }    

    public string Title { get; }    

    public Jewelry(int id, string vendorCode, string material, string title)
    {
        Title = title;
        VendorCode = vendorCode;
        Material = material;
        Id = id;
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
