using Dapper;
using Rental.Business.Contracts;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Rental.Business.Modules.Rental
{
    public sealed class RentalService : IRentalService
    {
        readonly IConfig configuration;
        public RentalService(IConfig configuration)
        {
            this.configuration = configuration;
        }

        public bool Checkin(int item, int customer)
        {
            try
            {
                var connection = "";
                var sp = "";
                configuration.Get(nameof(IRentalService), "sqlconnection", out connection, true);
                if (string.IsNullOrWhiteSpace(connection)) throw new ArgumentException("ConnectionString cannot be empty!");

                configuration.Get(nameof(IRentalService), "checkin", out sp, true);
                if (string.IsNullOrWhiteSpace(sp)) throw new ArgumentException("Checkin SP cannot be empty!");

                var p = new DynamicParameters();
                p.Add("@item", item);
                p.Add("@customer", customer);
                p.Add("@throw", true);
                p.Add("@result", 0, direction: System.Data.ParameterDirection.Output);

                using (var cnn = new SqlConnection(connection))
                {
                    cnn.Execute(sp, param: p, commandType: System.Data.CommandType.StoredProcedure);
                }

                return p.Get<int>("@result") == 1;
            }
            catch (Exception e)
            {
                // handle
                return false;
            }
        }

        public bool Checkout(int item, int customer, out DateTime? created)
        {
            created = null;
            try
            {
                var connection = "";
                var sp = "";
                configuration.Get(nameof(IRentalService), "sqlconnection", out connection, true);
                if (string.IsNullOrWhiteSpace(connection)) throw new ArgumentException("ConnectionString cannot be empty!");

                configuration.Get(nameof(IRentalService), "checkout", out sp, true);
                if (string.IsNullOrWhiteSpace(sp)) throw new ArgumentException("Checkout SP cannot be empty!");

                var p = new DynamicParameters();
                p.Add("@item", item);
                p.Add("@customer", customer);
                p.Add("@throw", true);
                p.Add("@result", 0, direction: System.Data.ParameterDirection.Output);
                p.Add("@created", null, direction: System.Data.ParameterDirection.Output);

                using (var cnn = new SqlConnection(connection))
                {
                    cnn.Execute(sp, param: p, commandType: System.Data.CommandType.StoredProcedure);
                }

                created = p.Get<DateTime?>("@created");

                return created is null;
            }
            catch (Exception e)
            {
                // handle
                return false;
            }
        }

        public IEnumerable<(int item, string itemName, int type, string typeName)> GetAvailable(int customer)
        {
            try
            {
                var connection = "";
                var sp = "";
                configuration.Get(nameof(IRentalService), "sqlconnection", out connection, true);
                if (string.IsNullOrWhiteSpace(connection)) throw new ArgumentException("ConnectionString cannot be empty!");

                configuration.Get(nameof(IRentalService), "get_available_by_customer", out sp, true);
                if (string.IsNullOrWhiteSpace(sp)) throw new ArgumentException("Checkout SP cannot be empty!");

                var p = new DynamicParameters();
                p.Add("@customer", customer);

                using (var cnn = new SqlConnection(connection))
                {
                    return cnn.Query<(int item, string itemName, int type, string typeName)>(sp, param: p, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch (Exception e)
            {
                // handle
                return Array.Empty<(int item, string itemName, int type, string typeName)>();
            }
        }

        public bool GetCustomer(string name, string phone, out uint customer)
        {
            customer = 0;
            try
            {
                var connection = "";
                var sp = "";
                configuration.Get(nameof(IRentalService), "sqlconnection", out connection, true);
                if (string.IsNullOrWhiteSpace(connection)) throw new ArgumentException("ConnectionString cannot be empty!");

                configuration.Get(nameof(IRentalService), "get_customer", out sp, true);
                if (string.IsNullOrWhiteSpace(sp)) throw new ArgumentException("Checkout SP cannot be empty!");

                var p = new DynamicParameters();
                p.Add("@name", name);
                p.Add("@phone", phone);
                p.Add("@result", 0, direction: System.Data.ParameterDirection.Output);

                using (var cnn = new SqlConnection(connection))
                {
                    cnn.Execute(sp, param: p, commandType: System.Data.CommandType.StoredProcedure);
                }

                customer = p.Get<uint>("@customer");
                return p.Get<int>("@result") == 1;
            }
            catch (Exception e)
            {
                // handle
                return false;
            }
        }

        public bool GetItemRate(int item, out uint rate)
        {
            rate = 1; // This would come from db
            return true;
        }
    }
}
