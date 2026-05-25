using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HackathonBackend.Migrations
{
    /// <inheritdoc />
    public partial class SeedMedicines : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Clean up any existing medicines from earlier ad-hoc inserts so the
            // seed (Ids 1..50) can be applied deterministically. Order-item /
            // cart-item foreign keys to these are also cleared to keep the
            // referential integrity intact in a hackathon DB.
            migrationBuilder.Sql("DELETE FROM CartItems;");
            migrationBuilder.Sql("DELETE FROM OrderItems;");
            migrationBuilder.Sql("DELETE FROM Medicines;");
            migrationBuilder.Sql("DBCC CHECKIDENT ('Medicines', RESEED, 0);");

            migrationBuilder.InsertData(
                table: "Medicines",
                columns: new[] { "Id", "Category", "Composition", "Description", "DosageForm", "ImageUrl", "Manufacturer", "Name", "PackagingType", "Price", "RequiresPrescription", "StockQuantity", "Strength" },
                values: new object[,]
                {
                    { 1, "Pain Relief", "Paracetamol 650mg", "Effective relief from fever and mild to moderate pain.", "Tablet", "https://images.unsplash.com/photo-1584308666744-24d5c474f2ae?w=400", "Micro Labs", "Dolo 650", "Strip of 15", 35m, false, 250, "650mg" },
                    { 2, "Pain Relief", "Paracetamol 500mg", "Fast-acting fever and headache relief.", "Tablet", "https://images.unsplash.com/photo-1550572017-edd951b55104?w=400", "GSK", "Crocin Advance", "Strip of 15", 30m, false, 200, "500mg" },
                    { 3, "Allergy", "Cetirizine HCl 10mg", "Antihistamine for allergic rhinitis and skin allergies.", "Tablet", "https://images.unsplash.com/photo-1587854692152-cbe660dbde88?w=400", "Cipla", "Cetirizine 10mg", "Strip of 10", 25m, false, 300, "10mg" },
                    { 4, "Antibiotic", "Amoxicillin Trihydrate 500mg", "Broad-spectrum antibiotic for bacterial infections.", "Capsule", "https://images.unsplash.com/photo-1587854692152-cbe660dbde88?w=400", "Cipla", "Amoxicillin 500mg", "Strip of 10", 85m, true, 150, "500mg" },
                    { 5, "Antibiotic", "Azithromycin 500mg", "Macrolide antibiotic for respiratory and skin infections.", "Tablet", "https://images.unsplash.com/photo-1576602976047-174e57a47881?w=400", "Sun Pharma", "Azithromycin 500mg", "Strip of 3", 110m, true, 120, "500mg" },
                    { 6, "Pain Relief", "Ibuprofen 400mg", "Anti-inflammatory pain reliever.", "Tablet", "https://images.unsplash.com/photo-1471864190281-a93a3070b6de?w=400", "Abbott", "Ibuprofen 400mg", "Strip of 10", 40m, false, 220, "400mg" },
                    { 7, "Gastro", "Pantoprazole Sodium 40mg", "Proton pump inhibitor for acidity and ulcers.", "Tablet", "https://images.unsplash.com/photo-1550572017-edd951b55104?w=400", "Sun Pharma", "Pantoprazole 40mg", "Strip of 15", 90m, false, 180, "40mg" },
                    { 8, "Gastro", "Omeprazole 20mg", "Reduces stomach acid production.", "Capsule", "https://images.unsplash.com/photo-1471864190281-a93a3070b6de?w=400", "Dr. Reddy's", "Omeprazole 20mg", "Strip of 10", 75m, false, 200, "20mg" },
                    { 9, "Diabetes", "Metformin HCl 500mg", "First-line therapy for type 2 diabetes.", "Tablet", "https://images.unsplash.com/photo-1559757175-5700dde675bc?w=400", "USV", "Metformin 500mg", "Strip of 15", 50m, true, 250, "500mg" },
                    { 10, "Diabetes", "Glimepiride 2mg", "Sulfonylurea for type 2 diabetes control.", "Tablet", "https://images.unsplash.com/photo-1559757175-5700dde675bc?w=400", "Sanofi", "Glimepiride 2mg", "Strip of 10", 95m, true, 140, "2mg" },
                    { 11, "Cardiac", "Atorvastatin Calcium 10mg", "Statin to lower cholesterol.", "Tablet", "https://images.unsplash.com/photo-1631549916768-4119b2e5f926?w=400", "Pfizer", "Atorvastatin 10mg", "Strip of 10", 120m, true, 160, "10mg" },
                    { 12, "Cardiac", "Amlodipine Besilate 5mg", "Calcium channel blocker for hypertension.", "Tablet", "https://images.unsplash.com/photo-1631549916768-4119b2e5f926?w=400", "Cipla", "Amlodipine 5mg", "Strip of 10", 60m, true, 180, "5mg" },
                    { 13, "Cardiac", "Telmisartan 40mg", "Angiotensin II receptor blocker for BP.", "Tablet", "https://images.unsplash.com/photo-1471864190281-a93a3070b6de?w=400", "Glenmark", "Telmisartan 40mg", "Strip of 10", 105m, true, 150, "40mg" },
                    { 14, "Vitamins", "Ascorbic Acid 500mg", "Antioxidant immunity booster.", "Tablet", "https://images.unsplash.com/photo-1471864190281-a93a3070b6de?w=400", "HealthVit", "Vitamin C 500mg", "Bottle of 60", 150m, false, 300, "500mg" },
                    { 15, "Vitamins", "Cholecalciferol 60000 IU", "Weekly Vitamin D supplement.", "Capsule", "https://images.unsplash.com/photo-1550572017-edd951b55104?w=400", "Mankind", "Vitamin D3 60K", "Strip of 4", 130m, false, 200, "60000 IU" },
                    { 16, "Vitamins", "Methylcobalamin 1500mcg", "Nerve health and energy support.", "Tablet", "https://images.unsplash.com/photo-1471864190281-a93a3070b6de?w=400", "Zydus", "Vitamin B12 1500mcg", "Strip of 10", 80m, false, 220, "1500mcg" },
                    { 17, "Vitamins", "Calcium Carbonate 500mg + D3 250IU", "Bone health supplement.", "Tablet", "https://images.unsplash.com/photo-1559757175-5700dde675bc?w=400", "Shelcal", "Calcium + Vit D3", "Strip of 15", 110m, false, 200, "500mg" },
                    { 18, "Vitamins", "Multivitamins + Minerals", "Daily wellness supplement.", "Capsule", "https://images.unsplash.com/photo-1584308666744-24d5c474f2ae?w=400", "Revital H", "Multivitamin Daily", "Bottle of 30", 220m, false, 180, "Multi" },
                    { 19, "Cough & Cold", "Diphenhydramine + Ammonium Chloride", "Cough syrup for dry and wet cough.", "Syrup", "https://images.unsplash.com/photo-1587854692152-cbe660dbde88?w=400", "J&J", "Benadryl Syrup", "Bottle 100ml", 95m, false, 160, "100ml" },
                    { 20, "Cough & Cold", "Herbal blend", "Ayurvedic cough relief.", "Syrup", "https://images.unsplash.com/photo-1587854692152-cbe660dbde88?w=400", "Dabur", "Honitus Cough Syrup", "Bottle 100ml", 85m, false, 180, "100ml" },
                    { 21, "Cough & Cold", "Paracetamol + Phenylephrine + CPM", "Multi-symptom cold and flu relief.", "Tablet", "https://images.unsplash.com/photo-1471864190281-a93a3070b6de?w=400", "Centaur", "Sinarest", "Strip of 10", 55m, false, 220, "500mg" },
                    { 22, "Cough & Cold", "Paracetamol + Phenylephrine + Caffeine", "Cold, flu, and headache relief.", "Tablet", "https://images.unsplash.com/photo-1471864190281-a93a3070b6de?w=400", "Reckitt", "D-Cold Total", "Strip of 10", 50m, false, 200, "500mg" },
                    { 23, "Cough & Cold", "Camphor + Menthol + Eucalyptus", "Topical cold relief balm.", "Ointment", "https://images.unsplash.com/photo-1471864190281-a93a3070b6de?w=400", "P&G", "Vicks VapoRub", "Jar 50g", 100m, false, 240, "50g" },
                    { 24, "Hydration", "Glucose + Salts", "Rehydration salts for diarrhoea.", "Powder", "https://images.unsplash.com/photo-1550572017-edd951b55104?w=400", "Electral", "ORS Powder", "Sachet", 22m, false, 400, "21.8g" },
                    { 25, "Gastro", "Sodium Bicarbonate + Citric Acid", "Fast acidity and gas relief.", "Powder", "https://images.unsplash.com/photo-1550572017-edd951b55104?w=400", "GSK", "Eno Fruit Salt", "Sachet", 10m, false, 500, "5g" },
                    { 26, "Gastro", "Magaldrate + Simethicone", "Antacid and anti-flatulent.", "Syrup", "https://images.unsplash.com/photo-1587854692152-cbe660dbde88?w=400", "Abbott", "Digene Gel", "Bottle 200ml", 130m, false, 160, "200ml" },
                    { 27, "Gastro", "Loperamide HCl 2mg", "For acute diarrhoea.", "Tablet", "https://images.unsplash.com/photo-1471864190281-a93a3070b6de?w=400", "Cipla", "Loperamide 2mg", "Strip of 10", 30m, false, 300, "2mg" },
                    { 28, "Gastro", "Ondansetron 4mg", "Anti-emetic for nausea and vomiting.", "Tablet", "https://images.unsplash.com/photo-1471864190281-a93a3070b6de?w=400", "Glenmark", "Ondansetron 4mg", "Strip of 10", 70m, true, 180, "4mg" },
                    { 29, "Allergy", "Montelukast + Levocetirizine", "Allergy and asthma management.", "Tablet", "https://images.unsplash.com/photo-1631549916768-4119b2e5f926?w=400", "Cipla", "Montair LC", "Strip of 10", 145m, true, 150, "10mg" },
                    { 30, "Allergy", "Levocetirizine 5mg", "Non-sedating antihistamine.", "Tablet", "https://images.unsplash.com/photo-1471864190281-a93a3070b6de?w=400", "Dr. Reddy's", "Levocetirizine 5mg", "Strip of 10", 40m, false, 260, "5mg" },
                    { 31, "Respiratory", "Salbutamol 100mcg", "Quick relief bronchodilator.", "Inhaler", "https://images.unsplash.com/photo-1559757175-5700dde675bc?w=400", "Cipla", "Asthalin Inhaler", "Inhaler 200 doses", 175m, true, 100, "100mcg" },
                    { 32, "Respiratory", "Budesonide 200mcg", "Steroid inhaler for asthma control.", "Inhaler", "https://images.unsplash.com/photo-1559757175-5700dde675bc?w=400", "Cipla", "Budecort Inhaler", "Inhaler 200 doses", 320m, true, 80, "200mcg" },
                    { 33, "Supplements", "Iron + Folic Acid", "Anaemia support tonic.", "Syrup", "https://images.unsplash.com/photo-1471864190281-a93a3070b6de?w=400", "Dexorange", "Iron + Folic Acid", "Bottle 200ml", 175m, false, 140, "200ml" },
                    { 34, "Supplements", "Multivitamins + Zinc", "Multivitamin with zinc.", "Tablet", "https://images.unsplash.com/photo-1559757175-5700dde675bc?w=400", "Apex", "Zincovit Tablet", "Strip of 15", 95m, false, 220, "Multi" },
                    { 35, "Supplements", "B-Complex + C", "B-complex supplement.", "Capsule", "https://images.unsplash.com/photo-1471864190281-a93a3070b6de?w=400", "Pfizer", "Becosules Capsules", "Strip of 20", 60m, false, 280, "Multi" },
                    { 36, "Gastro", "Ranitidine 150mg", "H2 blocker for acidity.", "Tablet", "https://images.unsplash.com/photo-1471864190281-a93a3070b6de?w=400", "GSK", "Ranitidine 150mg", "Strip of 10", 35m, false, 200, "150mg" },
                    { 37, "Pain Relief", "Diclofenac Diethylamine 1.16%", "Topical pain relief gel.", "Gel", "https://images.unsplash.com/photo-1559757175-5700dde675bc?w=400", "Volini", "Diclofenac Gel", "Tube 30g", 110m, false, 260, "30g" },
                    { 38, "Pain Relief", "Methyl Salicylate + Menthol", "Backache and muscle relief spray.", "Spray", "https://images.unsplash.com/photo-1631549916768-4119b2e5f926?w=400", "Reckitt", "Moov Spray", "Spray Can 55g", 175m, false, 140, "55g" },
                    { 39, "Cough & Cold", "Sodium Chloride 0.9%", "Gentle nasal congestion relief.", "Drops", "https://images.unsplash.com/photo-1471864190281-a93a3070b6de?w=400", "Nasivion", "Saline Nasal Drops", "Bottle 10ml", 70m, false, 200, "10ml" },
                    { 40, "Cough & Cold", "Xylometazoline 0.1%", "Decongestant nasal spray.", "Spray", "https://images.unsplash.com/photo-1587854692152-cbe660dbde88?w=400", "GSK", "Otrivin Nasal Spray", "Bottle 10ml", 105m, false, 180, "10ml" },
                    { 41, "Antibiotic", "Ciprofloxacin 500mg", "Broad-spectrum antibiotic.", "Tablet", "https://images.unsplash.com/photo-1576602976047-174e57a47881?w=400", "Cipla", "Ciplox 500mg", "Strip of 10", 90m, true, 130, "500mg" },
                    { 42, "Antibiotic", "Amoxicillin + Clavulanic Acid", "Combination antibiotic.", "Tablet", "https://images.unsplash.com/photo-1576602976047-174e57a47881?w=400", "GSK", "Augmentin 625", "Strip of 6", 240m, true, 100, "625mg" },
                    { 43, "Thyroid", "Levothyroxine 50mcg", "Hypothyroidism therapy.", "Tablet", "https://images.unsplash.com/photo-1631549916768-4119b2e5f926?w=400", "Abbott", "Thyronorm 50mcg", "Strip of 100", 145m, true, 160, "50mcg" },
                    { 44, "Gastro", "Liquid Paraffin + Milk of Magnesia", "Laxative for constipation.", "Syrup", "https://images.unsplash.com/photo-1587854692152-cbe660dbde88?w=400", "Abbott", "Cremaffin Plus", "Bottle 225ml", 165m, false, 140, "225ml" },
                    { 45, "Pediatric", "Paracetamol 250mg/5ml", "Child fever and pain syrup.", "Syrup", "https://images.unsplash.com/photo-1587854692152-cbe660dbde88?w=400", "P&G", "Pediatric Paracetamol Syrup", "Bottle 60ml", 65m, false, 220, "60ml" },
                    { 46, "Pediatric", "Multivitamin drops", "Daily drops for children.", "Drops", "https://images.unsplash.com/photo-1559757175-5700dde675bc?w=400", "Pfizer", "Pediatric Multivitamin Drops", "Bottle 15ml", 95m, false, 200, "15ml" },
                    { 47, "Diabetes", "Insulin Glargine", "Long-acting basal insulin.", "Injection", "https://images.unsplash.com/photo-1559757148-5c350d0d3c56?w=400", "Sanofi", "Insulin Glargine 100IU", "Cartridge 3ml", 850m, true, 60, "100IU/ml" },
                    { 48, "Cardiac", "Aspirin 75mg", "Low-dose aspirin antiplatelet.", "Tablet", "https://images.unsplash.com/photo-1631549916768-4119b2e5f926?w=400", "USV", "Ecosprin 75mg", "Strip of 14", 14m, true, 320, "75mg" },
                    { 49, "Personal Care", "Ethanol 70%", "Kills 99.9% germs.", "Liquid", "https://images.unsplash.com/photo-1584308666744-24d5c474f2ae?w=400", "Dettol", "Hand Sanitizer 500ml", "Bottle 500ml", 199m, false, 300, "500ml" },
                    { 50, "Personal Care", "3-ply mask", "Disposable surgical face masks.", "Mask", "https://images.unsplash.com/photo-1584308666744-24d5c474f2ae?w=400", "Generic", "Surgical Face Mask (50)", "Box of 50", 150m, false, 400, "3-ply" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 50);
        }
    }
}
