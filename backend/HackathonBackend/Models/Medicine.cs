namespace HackathonBackend.Models
{
    public class Medicine
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public string Category { get; set; } = "";

        // ---- SRS Product fields (kept simple — no separate Category/Inventory tables) ----
        public string Manufacturer { get; set; } = "";
        public string Composition { get; set; } = "";
        public string DosageForm { get; set; } = "";    // Tablet, Syrup, Injection, ...
        public string Strength { get; set; } = "";      // 500mg, 10ml, ...
        public string PackagingType { get; set; } = ""; // Strip of 10, Bottle 100ml, ...

        public decimal Price { get; set; }

        // StockQuantity stays on Medicine (no separate Inventory table — hackathon rule)
        public int StockQuantity { get; set; }

        public string Description { get; set; } = "";

        public bool RequiresPrescription { get; set; }

        public string ImageUrl { get; set; } = "";
    }
}
