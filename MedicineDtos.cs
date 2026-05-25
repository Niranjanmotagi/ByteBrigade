namespace HackathonBackend.Dtos
{
    // ===== Medicine =====
    // Documents the contracts of /api/Medicine endpoints.

    public class MedicineDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Category { get; set; } = "";
        public string Manufacturer { get; set; } = "";
        public string Composition { get; set; } = "";
        public string DosageForm { get; set; } = "";
        public string Strength { get; set; } = "";
        public string PackagingType { get; set; } = "";
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string Description { get; set; } = "";
        public bool RequiresPrescription { get; set; }
        public string ImageUrl { get; set; } = "";
    }

    public class CreateMedicineDto
    {
        public string Name { get; set; } = "";
        public string Category { get; set; } = "";
        public string Manufacturer { get; set; } = "";
        public string Composition { get; set; } = "";
        public string DosageForm { get; set; } = "";
        public string Strength { get; set; } = "";
        public string PackagingType { get; set; } = "";
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string Description { get; set; } = "";
        public bool RequiresPrescription { get; set; }
        public string ImageUrl { get; set; } = "";
    }

    public class UpdateMedicineDto
    {
        public string Name { get; set; } = "";
        public string Category { get; set; } = "";
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string Description { get; set; } = "";
        public bool RequiresPrescription { get; set; }
        public string ImageUrl { get; set; } = "";
    }

    public class UpdateStockDto
    {
        public int StockQuantity { get; set; }
    }
}
