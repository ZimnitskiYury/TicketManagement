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
    public class LayoutRepository : IRepository<LayoutData>, ILayoutRepositoryExtension
    {
        private readonly DbConnection _connection;

        public LayoutRepository(DbConnection connection)
        {
            _connection = connection;
        }

        public int Insert(LayoutData obj)
        {
            int insertedId;

            if (obj != null)
            {
                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Layout (VenueId, Description) VALUES (@venueid, @description); SELECT CAST(scope_identity() as int)";
                    command.CommandType = System.Data.CommandType.Text;

                    var venueidParam = new SqlParameter("@venueid", obj.VenueId);
                    command.Parameters.Add(venueidParam);

                    var descriptionParam = new SqlParameter("@description", obj.Description);
                    command.Parameters.Add(descriptionParam);

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
                command.CommandText = "DELETE FROM Layout WHERE Id=@id; SELECT @@ROWCOUNT";
                command.CommandType = System.Data.CommandType.Text;

                var idparam = new SqlParameter("@id", id);
                command.Parameters.Add(idparam);

                _connection.Open();

                rowAffected = (int)command.ExecuteScalar();

                _connection.Close();
            }

            return rowAffected;
        }

        public List<LayoutData> FilterByNameInVenue(LayoutData input)
        {
            if (input != null)
            {
                var layouts = new List<LayoutData>();

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "SELECT [Id] FROM Layout WHERE (Description=@description) And (VenueId=@venue) ";
                    command.CommandType = System.Data.CommandType.Text;

                    var descriptionParam = new SqlParameter("@description", input.Description);
                    command.Parameters.Add(descriptionParam);

                    var venueParam = new SqlParameter("@layout", input.VenueId);
                    command.Parameters.Add(venueParam);

                    _connection.Open();

                    using (DbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            layouts.Add(new LayoutData
                            {
                                Id = reader.GetInt32(0),
                                VenueId = reader.GetInt32(1),
                                Description = reader.GetString(2),
                            });
                        }
                    }

                    _connection.Close();
                }

                return layouts;
            }
            else
            {
                throw new ArgumentNullException(nameof(input));
            }
        }

        public LayoutData GetById(int id)
        {
            LayoutData layout = null;
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT [Id], [VenueId], [Description] FROM Layout WHERE Id=@id";
                command.CommandType = System.Data.CommandType.Text;

                var idparam = new SqlParameter("@id", id);
                command.Parameters.Add(idparam);

                _connection.Open();

                using (DbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        layout = new LayoutData
                        {
                            Id = reader.GetInt32(0),
                            VenueId = reader.GetInt32(1),
                            Description = reader.GetString(2),
                        };
                    }
                }

                _connection.Close();
            }

            return layout;
        }

        public IQueryable<LayoutData> GetAll()
        {
            List<LayoutData> layouts = new List<LayoutData>();
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT [Id], [VenueId], [Description] FROM Layout";
                command.CommandType = System.Data.CommandType.Text;

                _connection.Open();

                using (DbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        layouts.Add(new LayoutData
                        {
                            Id = reader.GetInt32(0),
                            VenueId = reader.GetInt32(1),
                            Description = reader.GetString(2),
                        });
                    }
                }

                _connection.Close();
            }

            return layouts.AsQueryable();
        }

        public int Update(LayoutData obj)
        {
            int rowsAffected = 0;
            if (obj != null)
            {
                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "Update Layout SET VenueId=@venueid, Description=@description WHERE Id=@id; SELECT @@ROWCOUNT";
                    command.CommandType = System.Data.CommandType.Text;

                    var venueidParam = new SqlParameter("@venueid", obj.VenueId);
                    command.Parameters.Add(venueidParam);

                    var descriptionParam = new SqlParameter("@description", obj.Description);
                    command.Parameters.Add(descriptionParam);

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
