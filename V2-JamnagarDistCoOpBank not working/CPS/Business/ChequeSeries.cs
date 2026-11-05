using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CPS.Business
{
    [Table("ChequeSeries")]
    [Serializable]
    public class ChequeSeries
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string SAN { get; set; }

        [Required]
        public int LastChequePrint { get; set; }

        // ------------------------------------------------------------
        // Generate next cheque series value (Insert or Update)
        // ------------------------------------------------------------
        public static ChequeSeries NextValue(int noOfChequeBook, int bookSize, string SAN)
        {
            using (var context = new CPSDbContext())
            {
                // Check if record exists
                var existing = context.ChequeSeries
                                      .FirstOrDefault(x => x.SAN == SAN);

                int lastValue = existing?.LastChequePrint ?? 100000;
                int newValue = lastValue + (bookSize * noOfChequeBook);

                if (existing == null)
                {
                    // Insert new record
                    var chequeSeries = new ChequeSeries
                    {
                        SAN = SAN,
                        LastChequePrint = newValue
                    };

                    context.ChequeSeries.Add(chequeSeries);
                    context.SaveChanges();

                    return chequeSeries;
                }
                else
                {
                    // Update existing record
                    existing.LastChequePrint = newValue;
                    context.SaveChanges();

                    return existing;
                }
            }
        }

        // ------------------------------------------------------------
        // Save method (Insert or Update)
        // ------------------------------------------------------------
        public static bool Save(ChequeSeries chequeSeries)
        {
            using (var context = new CPSDbContext())
            {
                var existing = context.ChequeSeries
                                      .FirstOrDefault(x => x.SAN == chequeSeries.SAN);

                if (existing == null)
                {
                    // Insert
                    context.ChequeSeries.Add(chequeSeries);
                }
                else
                {
                    // Update
                    existing.LastChequePrint = chequeSeries.LastChequePrint;
                }

                context.SaveChanges();
            }

            return true;
        }
    }
}
