using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quill.Scope;
using System.Data.SqlClient;

namespace Quill.Sample {
    public class SampleDataSource {
        public static List<Entity> Select(string postalCode) {
            if(string.IsNullOrWhiteSpace(postalCode)) {
                return SelectAll();
            }
            return new List<Entity>();
        }

        private static List<Entity> SelectAll() {
            return Tx.Execute(connection => {
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM PostalCode";
                var results = command.ExecuteReader();
                var entities = new List<Entity>();
                while(results.Read()) {
                    var entity = new Entity();
                    entity.Code = results.GetString(0);
                    entity.Address = results.GetString(1);
                    entities.Add(entity);
                }
                return entities;
            });
        }
    }
}
