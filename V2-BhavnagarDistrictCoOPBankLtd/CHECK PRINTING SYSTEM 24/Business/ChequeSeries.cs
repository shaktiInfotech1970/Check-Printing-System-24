using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Data.Entity.Migrations;
using System.Windows;

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

        public static int GetLastChequePrint(string SAN)
        {
            using (var context = new CPSDbContext())
            {
                var chequeSeries = context.ChequeSeries
                    .FirstOrDefault(x => x.SAN == SAN);

                if (chequeSeries == null)
                    return 400000;

                if (chequeSeries.LastChequePrint >= 0 &&
                    chequeSeries.LastChequePrint < 400000)
                    return 400000;

                return chequeSeries.LastChequePrint;
            }
        }

        public static ChequeSeries NextValue(int noOfChequeBook, int bookSize, string SAN)
        {
            using (var context = new CPSDbContext())
            {
                var value = context.Set<ChequeSeries>().Where(w => w.SAN ==  SAN).DefaultIfEmpty().Max(m => m == null ? 400000 : m.LastChequePrint);

                if (value >= 0 && value < 400000)
                    value = 400000;

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
