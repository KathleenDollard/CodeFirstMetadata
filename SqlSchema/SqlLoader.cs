using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSchema
{
   public static class SqlLoader
   {

      public static Database LoadFromConnectionString(string connectionString)
      {
         using (var connection = new SqlConnection(connectionString))
         {
            connection.Open();
            return GetDatabaseInfo(connection);
         }
      }

      private static Database GetDatabaseInfo(SqlConnection connection)
      {
         var database = new Database();
         var columns = GetColumns(connection);
         var tables = GetTableOrViews<Table>(connection, "BASE TABLE", columns);
         var views = GetTableOrViews<View>(connection, "VIEW", columns);
         var foreignKeys = GetForeignKeys(connection, tables);
         database.Tables = tables;
         database.Views = views;
         database.Name = tables.First().DatabaseName;
         return database;
      }

      private static IEnumerable<Column> GetColumns(SqlConnection connection)
      {
         var tSQL = @"select C.TABLE_CATALOG, C.TABLE_SCHEMA, C.TABLE_NAME, C.COLUMN_NAME, C.ORDINAL_POSITION, C.COLUMN_DEFAULT,
                     C.IS_NULLABLE, C.DATA_TYPE, C.CHARACTER_MAXIMUM_LENGTH, C.NUMERIC_PRECISION, 
                     C.NUMERIC_SCALE, C.DATETIME_PRECISION
                 from INFORMATION_SCHEMA.Columns as c";
         var newItems = GetData(connection, tSQL,
            reader =>
            {
               var newItem = new Column();
               newItem.DatabaseName = reader.GetString(0);
               newItem.SchemaName = reader.GetString(1);
               newItem.TableName = reader.GetString(2);
               newItem.Name = reader.GetString(3);
               newItem.SqlDataType = reader.GetString(7);
               newItem.AllowNulls = reader.GetString(6).Equals("Yes", StringComparison.OrdinalIgnoreCase);
               newItem.Type = GetNetType(newItem.SqlDataType);
               if (newItem.Type == typeof(string))
               { newItem.MaxLength = reader.GetInt32(8); }
               //newItem.NumericPrecision = reader.GetValue(9) == null ? 0 : (int)reader.GetValue(9);
               //newItem.NumericScale = reader.GetValue(10) == null ? 0 : (int)reader.GetValue(10);
               return newItem;
            });
         SetPrimaryKeys(connection, newItems);
         return newItems;
      }

      private static Type GetNetType(string dbType)
      {
         switch (dbType)
         {
         case "float": return typeof(Double);
         case "image": return typeof(Byte[]);
         case "int": return typeof(Int32);
         case "money": return typeof(Decimal);
         case "nchar": return typeof(String);
         case "ntext": return typeof(String);
         case "numeric": return typeof(Decimal);
         case "nvarchar": return typeof(String);
         case "real": return typeof(Single);
         case "rowversion": return typeof(Byte[]);
         case "smalldatetime": return typeof(DateTime);
         case "smallint": return typeof(Int16);
         case "smallmoney": return typeof(Decimal);
         case "sql_variant": return typeof(Object);
         case "text": return typeof(String);
         case "time": return typeof(TimeSpan);
         case "timestamp": return typeof(Byte[]);
         case "tinyint": return typeof(Byte);
         case "uniqueidentifier": return typeof(Guid);
         case "varbinary": return typeof(Byte[]);
         case "varchar": return typeof(String);
         default:
            return null;
         }
      }

      private static IEnumerable<Associations> GetForeignKeys(SqlConnection connection, IEnumerable<Table> tables)
      {
         var tSQL = @"Select c.TABLE_CATALOG, c.TABLE_SCHEMA, c.TABLE_NAME, c.COLUMN_NAME, 
                  t.TABLE_NAME as PkTable, t.COLUMN_NAME as PkColumnName,
                  r.CONSTRAINT_CATALOG, r.CONSTRAINT_SCHEMA, r.CONSTRAINT_NAME, r.UNIQUE_CONSTRAINT_SCHEMA, r.UNIQUE_CONSTRAINT_NAME
               from INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE as c
               Join INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS   as r
               on r.CONSTRAINT_CATALOG = c.CONSTRAINT_CATALOG
               and r.CONSTRAINT_SCHEMA = c.CONSTRAINT_SCHEMA
               and r.CONSTRAINT_NAME = c.CONSTRAINT_NAME
               Join INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE as t
               on t.CONSTRAINT_CATALOG = r.UNIQUE_CONSTRAINT_CATALOG
               and t.CONSTRAINT_SCHEMA = r.UNIQUE_CONSTRAINT_SCHEMA
               and t.CONSTRAINT_NAME = r.UNIQUE_CONSTRAINT_NAME";
         // TODO: Rewrite below to manage multi-key columns
         // TODO: Below assumes the same database, inconsistent across this project
         var newItems = GetData(connection, tSQL,
            reader =>
            {
               var newItem = new Associations();
               var fkSchemaName = reader.GetString(1);
               var fkTableName = reader.GetString(2);
               var fkColumnName = reader.GetString(3);
               var pkTableName = reader.GetString(4);
               var pkColumnName = reader.GetString(5);

               newItem.ForeignKeyTable = tables
                                          .Where(t => t.SchemaName == fkSchemaName
                                                      && t.Name == fkTableName)
                                          .FirstOrDefault();
               newItem.ForeignKeyColumns = newItem.ForeignKeyTable
                                          .Columns
                                          .Where(c => c.Name == fkColumnName);
               newItem.PrimaryKeyTable = tables
                                         .Where(t => t.Name == pkTableName)
                                         .FirstOrDefault();
               newItem.PrimaryKeyColumns = newItem.PrimaryKeyTable
                                          .Columns
                                          .Where(c => c.Name == pkColumnName);
               return newItem;
            });
         foreach (var table in tables)
         {
            table.ForiegnKeys = newItems
                                 .Where(r => r.ForeignKeyTable == table);
         }
         return newItems;
      }

      private static IEnumerable<T> GetData<T>(SqlConnection connection, string tSQL, Func<SqlDataReader, T> getItem)
      {
         var cmd = connection.CreateCommand();
         cmd.CommandText = tSQL;
         using (var data = cmd.ExecuteReader())
         {
            var ret = new List<T>();
            while (data.Read())
            {
               ret.Add(getItem(data));
            }
            return ret;
         }
      }

      private static void SetPrimaryKeys(SqlConnection connection, IEnumerable<Column> columns)
      {
         var tSQL = @"select tc.TABLE_SCHEMA, tc.TABLE_NAME, c.COLUMN_NAME,tc.CONSTRAINT_SCHEMA, tc.CONSTRAINT_NAME
                  from INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE as c
                  join INFORMATION_SCHEMA.TABLE_CONSTRAINTS as tc
                  on c.CONSTRAINT_SCHEMA = tc.CONSTRAINT_SCHEMA
                  and c.CONSTRAINT_NAME = tc.CONSTRAINT_NAME
                  Where tc.CONSTRAINT_TYPE = 'PRIMARY KEY'";
         var pks = GetData(connection, tSQL,
           reader =>
           {

              var schemaName = reader.GetString(0);
              var tableName = reader.GetString(1);
              var columnName = reader.GetString(2);
              return Tuple.Create(schemaName, tableName, columnName);
           });
         foreach (var pk in pks)
         {
            var candidate = columns
                            .Where(c => c.SchemaName == pk.Item1
                                   && c.TableName == pk.Item2
                                   && c.Name == pk.Item3)
                            .FirstOrDefault();
            if (candidate != null) { candidate.IsPrimaryKey = true; }
         }
      }

      //private static IEnumerable<Table> GetTables(
      //            SqlConnection connection,
      //            IEnumerable<Column> columns)
      //{
      //   var list = new List<Table>();
      //   var tSQL = @"select * from INFORMATION_SCHEMA.TABLES";
      //   var tables = GetData(connection, tSQL,
      //     reader =>
      //     {
      //        var schemaName = reader.GetString(1);
      //        var tableName = reader.GetString(2);
      //        return Tuple.Create(schemaName, tableName);
      //     });
      //   foreach (var t in tables)
      //   {
      //      var cols = columns.Where(x => x.SchemaName == t.Item1
      //                                 && x.TableName == t.Item2);
      //      var newItem = new Table()
      //      {
      //         DatabaseName = cols.First().DatabaseName,
      //         SchemaName = t.Item1,
      //         Name = t.Item2,
      //         Columns = cols
      //      };
      //      list.Add(newItem);
      //   }
      //   return list;
      //}

      private static IEnumerable<TNewItem> GetTableOrViews<TNewItem>(
                 SqlConnection connection,
                 string tableTypeName,
                 IEnumerable<Column> columns)
         where TNewItem : TableOrView 
      {
         var list = new List<TNewItem>();
         var tSQL = string.Format(@"select * 
                        from INFORMATION_SCHEMA.Tables
                        where Table_Type = '{0}'", tableTypeName);
         var tableOrViews = GetData(connection, tSQL,
           reader =>
           {
              var schemaName = reader.GetString(1);
              var tableName = reader.GetString(2);
              return Tuple.Create(schemaName, tableName);
           });
         foreach (var t in tableOrViews)
         {
            var cols = columns.Where(x => x.SchemaName == t.Item1
                                       && x.TableName == t.Item2);
            var newItem = Activator.CreateInstance<TNewItem>();
            newItem.   DatabaseName = cols.First().DatabaseName;
            newItem.   SchemaName = t.Item1;
            newItem.   Name = t.Item2;
            newItem.   Columns = cols;
            list.Add(newItem);
         }
         return list;
      }

      //private static IEnumerable<Table> GetViews(IEnumerable<Column> columns)
      //{

      //   var distinct = columns
      //                  .Select(c => new { c.DatabaseName, c.SchemaName, c.TableName })
      //                  .Distinct();
      //   return distinct
      //          .Select(t => new Table()
      //          {
      //             DatabaseName = t.DatabaseName,
      //             SchemaName = t.SchemaName,
      //             Name = t.TableName,
      //             Columns = columns
      //                         .Where(c => c.DatabaseName == t.DatabaseName
      //                                 && c.SchemaName == t.SchemaName
      //                                 && c.TableName == t.TableName)
      //          });
      //}
   }
}
