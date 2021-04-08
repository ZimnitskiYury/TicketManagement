using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using TicketManagement.DataAccess.Entities;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Interfaces.RepositoryExtension;

namespace TicketManagement.DataAccess.Repository.Sql
{
    public class VenueRepository : IRepository<Venue>, IVenueRepositoryExtension
    {
        private readonly DbConnection _connection;

        public VenueRepository(DbConnection connection)
        {
            _connection = connection;
        }

        public int Insert(Venue obj)
        {
            int insertedId;

            if (obj != null)
            {
                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Venue (Description, Address, Phone) VALUES (@description, @address, @phone); SELECT CAST(scope_identity() as int)";
                    command.CommandType = System.Data.CommandType.Text;

                    var descriptionParam = new SqlParameter("@description", obj.Description);
                    command.Parameters.Add(descriptionParam);

                    var addressParam = new SqlParameter("@address", obj.Address);
                    command.Parameters.Add(addressParam);

                    var phoneParam = new SqlParameter("@phone", obj.Phone);
                    command.Parameters.Add(phoneParam);

                    _connection.Open();

                    insertedId = (int)command.ExecuteScalar();

                    _connection.Close();
                }

                return insertedId;
            }
            else
            {
                throw new ArgumentNullException(nameof(obj));
            }
        }

        public int Delete(int id)
        {
            int rowAffected = 0;
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM Venue WHERE Id=@id; SELECT @@ROWCOUNT";
                command.CommandType = System.Data.CommandType.Text;

                var idparam = new SqlParameter("@id", id);
                command.Parameters.Add(idparam);

                _connection.Open();

                rowAffected = (int)command.ExecuteScalar();

                _connection.Close();
            }

            return rowAffected;
        }

        public List<Venue> FilterByName(Venue input)
        {
            var venues = new List<Venue>();
            if (input != null)
            {
                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "SELECT [Id] FROM Venue WHERE (Description=@description)";
                    command.CommandType = System.Data.CommandType.Text;

                    var descriptionParam = new SqlParameter("@description", input.Description);
                    command.Parameters.Add(descriptionParam);

                    _connection.Open();

                    using (DbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            venues.Add(new Venue
                            {
                                Id = reader.GetInt32(0),
                                Description = reader.GetString(1),
                                Address = reader.GetString(2),
                                Phone = reader.GetString(3),
                            });
                        }
                    }

                    _connection.Close();
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(input));
            }

            return venues;
        }

        public Venue GetById(int id)
        {
            Venue venue = null;
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT [Id],[Description],[Address],[Phone] FROM Venue WHERE Id=@id";
                command.CommandType = System.Data.CommandType.Text;

                var idparam = new SqlParameter("@id", id);
                command.Parameters.Add(idparam);

                _connection.Open();

                using (DbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        venue = new Venue
                        {
                            Id = reader.GetInt32(0),
                            Description = reader.GetString(1),
                            Address = reader.GetString(2),
                            Phone = reader.GetString(3),
                        };
                    }
                }

                _connection.Close();
            }

            return venue;
        }

        public IQueryable<Venue> GetAll()
        {
            List<Venue> venues = new List<Venue>();
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT [Id],[Description],[Address],[Phone] FROM Venue";
                command.CommandType = System.Data.CommandType.Text;

                _connection.Open();

                using (DbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        venues.Add(new Venue
                        {
                            Id = reader.GetInt32(0),
                            Description = reader.GetString(1),
                            Address = reader.GetString(2),
                            Phone = reader.GetString(3),
                        });
                    }
                }

                _connection.Close();
            }

            return venues.AsQueryable();
        }

        public int Update(Venue obj)
        {
            int rowsAffected = 0;
            if (obj != null)
            {
                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "Update Venue SET Description=@description, Address=@address, Phone=@phone WHERE Id=@id; SELECT @@ROWCOUNT";
                    command.CommandType = System.Data.CommandType.Text;

                    var descriptionParam = new SqlParameter("@description", obj.Description);
                    command.Parameters.Add(descriptionParam);

                    var addressParam = new SqlParameter("@address", obj.Address);
                    command.Parameters.Add(addressParam);

                    var phoneParam = new SqlParameter("@phone", obj.Phone);
                    command.Parameters.Add(phoneParam);

                    var idparam = new SqlParameter("@id", obj.Id);
                    command.Parameters.Add(idparam);

                    _connection.Open();

                    rowsAffected = (int)command.ExecuteScalar();

                    _connection.Close();
                }

                return rowsAffected;
            }
            else
            {
                throw new ArgumentNullException(nameof(obj));
            }
        }
    }
}
