using System;

namespace ConsoleApp1
{

    // DTO (Data Transfer Object).
    class Item
    {
        public long ItemId { get; set; }
        public long SupplierId { get; set; }            // Поставщик.
        public double BuyingPrice { get; set; }         // Цена закупки.
        public double SellingPrice { get; set; }        // Цена продажи.
        public double RrpPrice { get; set; }            // Рекоменд. цена (РРЦ).
        public string Quantity { get; set; }            // Наличие товара.
        public DateTime RelevanceDate { get; set; }     // Актуальность цены.
        public DateTime ModificationDate { get; set; }  // Дата изменения записи.
    }


}
