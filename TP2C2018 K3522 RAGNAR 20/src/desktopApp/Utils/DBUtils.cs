using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PalcoNet.Model;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using PalcoNet.BaseDeDatos;

namespace PalcoNet.Utils
{
    class DBUtils {

        public static void guardar(RagnarEntities db) {
            save(db);
        }

        public static void guardar() {
            save(BaseDeDatos.BaseDeDatos.finalDb(null));
        }

        private static void save(RagnarEntities db) {
            try {
                db.SaveChanges();
            } catch (DbEntityValidationException e) {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Value: \"{1}\", Error: \"{2}\"",
                            ve.PropertyName,
                            eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName),
                            ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (DbUpdateException dbu)
            {
                var builder = new StringBuilder("A DbUpdateException was caught while saving changes. ");

                try
                {
                    foreach (var result in dbu.Entries)
                    {
                        builder.AppendFormat("Type: {0} was part of the problem. ", result.Entity.GetType().Name);
                    }
                }
                catch (Exception e)
                {
                    builder.Append("Error parsing DbUpdateException: " + e.ToString());
                }

                string message = builder.ToString();

                throw new Exception(message, dbu);
            }
        }
    }    
}
