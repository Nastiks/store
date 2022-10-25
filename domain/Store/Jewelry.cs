using Store.Data;
using System.Text.RegularExpressions;

namespace Store
{
    public class Jewelry
    {
        private readonly JewelryDto dto;

        public int Id => dto.Id;

        public string VendorCode
        {
            get => dto.VendorCode;
            set
            {
                if (TryFormatVendorCode(value, out string formattedVendorCode))
                {
                    dto.VendorCode = formattedVendorCode;
                }
                throw new ArgumentException(nameof(VendorCode));
            }
        }

        public string Material
        {
            get => dto.Material;
            set => dto.Material = value?.Trim();
        }

        public string Title
        {
            get => dto.Title;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(nameof(Title));
                }
                dto.Title = value.Trim();
            }
        }

        public string Description
        {
            get => dto.Description;
            set => dto.Description = value;
        }

        public decimal Price
        {
            get => dto.Price;
            set => dto.Price = value;
        }

        internal Jewelry(JewelryDto dto)
        {
            this.dto = dto;
        }

        public static bool TryFormatVendorCode(string vendorCode, out string formattedVendorCode)
        {
            if (vendorCode == null)
            {
                formattedVendorCode = null;
                return false;
            }

            formattedVendorCode = vendorCode.Replace("-", "")
                                            .Replace(" ", "")
                                            .ToUpper();

            return Regex.IsMatch(formattedVendorCode, @"^VENDORCODE\d{10}(\d{3})?$");
        }

        public static bool IsVendorCode(string vendorCode)
            => TryFormatVendorCode(vendorCode, out _);

        public static class DtoFactory
        {
            public static JewelryDto Create(string vendorCode,
                                            string material,
                                            string tittle,
                                            string description,
                                            decimal price)
            {
                if (TryFormatVendorCode(vendorCode, out string formattedVendorCode))
                {
                    vendorCode = formattedVendorCode;
                }
                else
                {
                    throw new ArgumentException(nameof(vendorCode));
                }

                if (string.IsNullOrWhiteSpace(tittle))
                {
                    throw new ArgumentException(nameof(tittle));
                }

                return new JewelryDto
                {
                    VendorCode = vendorCode,
                    Material = material?.Trim(),
                    Title = tittle.Trim(),
                    Description = description?.Trim(),
                    Price = price,
                };
            }
        }

        public static class Mapper
        {
            public static Jewelry Map(JewelryDto dto) => new Jewelry(dto);

            public static JewelryDto Map(Jewelry domain) => domain.dto;
        }
    }
}