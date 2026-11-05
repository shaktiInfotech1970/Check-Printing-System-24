using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Data.Entity.Migrations;

namespace CPS.Business
{
    public class Counters
    {
        public const string Request = "Request";
        public const string PrintJob = "PrintJob";
    }

    [Table("Counter")]
    public class Counter
    {
        [Key, MaxLength(32)]
        public string Key { get; set; }
        public int Value { get; set; }

        public static int NextValue(string key)
        {
            using(var context = new CPSDbContext())
            {

                var value = context.Set<Counter>().Where(w => w.Key == key).DefaultIfEmpty().Max(m => m == null ? 0 : m.Value);
                value++;
                var counter = new Counter { Key = key, Value = value };
                context.Counter.AddOrUpdate(k => k.Key, counter);
                context.SaveChanges();

                return value;
            }
        }
    }
}
