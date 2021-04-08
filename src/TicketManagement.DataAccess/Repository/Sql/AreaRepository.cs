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
    public class AreaRepository : IRepository<Area>, IAreaRepositoryExtension
    {
        private readonly DbConnection _connection;

        public AreaRepository(DbConnection connection)
        {
            _connection = connection;
        }

        public int Insert(Area obj)
        {
            int insertedId;
            if (obj != null)
            {
                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Area (LayoutId, Description, CoordX, CoordY) VALUES (@layoutid, @description, @coordx, @coordy); SELECT CAST(scope_identity() as int)";
                    command.CommandType = System.Data.CommandType.Text;

                    var layoutidParam = new SqlParameter("@layoutid", obj.LayoutId);
                    command.Parameters.Add(layoutidParam);

                    var descriptionParam = new SqlParameter("@description", obj.Description);
                    command.Parameters.Add(descriptionParam);

                    var coordxParam = new SqlParameter("@coordx", obj.CoordX);
                    command.Parameters.Add(coordxParam);

                    var coordyParam = new SqlParameter("@coordy", obj.CoordY);
                    command.Parameters.Add(coordyParam);

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
                command.CommandText = "DELETE FROM Area WHERE Id=@id; SELECT @@ROWCOUNT";
                command.CommandType = System.Data.CommandType.Text;

                var idparam = new SqlParameter("@id", id);
                command.Parameters.Add(idparam);

                _connection.Open();

                rowAffected = (int)command.ExecuteScalar();

                _connection.Close();
            }

            return rowAffected;
        }

        public List<Area> FilterByNameInLayout(Area input)
        {
            if (input != null)
            {
                var areas = new List<Area>();

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "SELECT [Id] FROM Area WHERE (Description=@description) And (LayoutId=@layout) ";
                    command.CommandType = System.Data.CommandType.Text;

                    var descriptionParam = new SqlParameter("@description", input.Description);
                    command.Parameters.Add(descriptionParam);

                    var layoutParam = new SqlParameter("@layout", input.LayoutId);
                    command.Parameters.Add(layoutParam);

                    _connection.Open();

                    using (DbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            areas.Add(new Area
                            {
                                Id = reader.GetInt32(0),
                                LayoutId = reader.GetInt32(1),
                                Description = reader.GetString(2),
                                CoordX = reader.GetInt32(3),
                                CoordY = reader.GetInt32(4),
                            });
                        }
                    }

                    _connection.Close();
                }

                return areas;
            }
            else
            {
                throw new ArgumentNullException(nameof(input));
            }
        }

        public Area GetById(int id)
        {
            Area area = null;
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT [Id], [LayoutId], [Description], [CoordX], [CoordY] FROM Area WHERE Id=@id";
                command.CommandType = System.Data.CommandType.Text;

                var idparam = new SqlParameter("@id", id);
                command.Parameters.Add(idparam);

                _connection.Open();

                using (DbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        area = new Area
                        {
                            Id = reader.GetInt32(0),
                            LayoutId = reader.GetInt32(1),
                            Description = reader.GetString(2),
                            CoordX = reader.GetInt32(3),
                            CoordY = reader.GetInt32(4),
                        };
                    }
                }

                _connection.Close();
            }

            return area;
        }

        public IQueryable<Area> GetAll()
        {
            var areas = new List<Area>();
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT [Id], [LayoutId], [Description], [CoordX], [CoordY] FROM Area";
                command.CommandType = System.Data.CommandType.Text;

                _connection.Open();

                using (DbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        areas.Add(new Area
                        {
                            Id = reader.GetInt32(0),
                            LayoutId = reader.GetInt32(1),
                            Description = reader.GetString(2),
                            CoordX = reader.GetInt32(3),
                            CoordY = reader.GetInt32(4),
                        });
                    }
                }

                _connection.Close();
            }

            return areas.AsQueryable();
        }

        public int Update(Area obj)
        {
            int rowsAffected = 0;
            if (obj != null)
            {
                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "Update Area SET LayoutId=@layoutid, Description=@description, CoordX=@coordx, CoordY=@coordy WHERE Id=@id; SELECT @@ROWCOUNT";
                    command.CommandType = System.Data.CommandType.Text;

                    var layoutidParam = new SqlParameter("@layoutid", obj.LayoutId);
                    command.Parameters.Add(layoutidParam);

                    var descriptionParam = new SqlParameter("@description", obj.Description);
                    command.Parameters.Add(descriptionParam);

                    var coordxParam = new SqlParameter("@coordx", obj.CoordX);
                    command.Parameters.Add(coordxParam);

                    var coordyParam = new SqlParameter("@coordy", obj.CoordY);
                    command.Parameters.Add(coordyParam);

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
