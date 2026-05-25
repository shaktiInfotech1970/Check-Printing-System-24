using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Data.Entity.Migrations;

namespace CPS.Business
{
    [Table("ChequeSeries")]
    [Serializable]
    public class ChequeSeries
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string SAN { get; set; }

        [Required]
        public int LastChequePrint { get; set; }

        public static ChequeSeries NextValue(int noOfChequeBook, int bookSize, string SAN)
        {
            using (var context = new CPSDbContext())
            {
                var value = context.Set<ChequeSeries>().Where(w => w.SAN == SAN).DefaultIfEmpty().Max(m => m == null ? 120000 : m.LastChequePrint);
                var chequeSeries = new ChequeSeries { SAN = SAN, LastChequePrint = value + (bookSize * noOfChequeBook) };
                context.ChequeSeries.AddOrUpdate(k => k.SAN, chequeSeries);
                context.SaveChanges();

                return chequeSeries;
            }
        }

        public static bool Save(ChequeSeries chequeSeries)
        {
            using (var context = new CPSDbContext())
            {
                context.ChequeSeries.AddOrUpdate(k => k.SAN, chequeSeries);
                context.SaveChanges();
            }

            return true;
        }

    }
}
