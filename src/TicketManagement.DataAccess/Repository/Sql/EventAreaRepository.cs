using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using TicketManagement.DataAccess.Entities;
using TicketManagement.DataAccess.Interfaces;

namespace TicketManagement.DataAccess.Repository.Sql
{
    public class EventAreaRepository : IRepository<EventArea>
    {
        private readonly DbConnection _connection;

        public EventAreaRepository(DbConnection connection)
        {
            _connection = connection;
        }

        public int Insert(EventArea obj)
        {
            int insertedId;

            if (obj != null)
            {
                using (var command = _connection.CreateCommand())
                {
#pragma warning disable S103 // Lines should not be too long
                    command.CommandText = "INSERT INTO EventArea (EventId, Description, CoordX, CoordY, Price) VALUES (@eventid, @description, @coordx, @coordy, @price); SELECT CAST(scope_identity() as int)";
#pragma warning restore S103 // Lines should not be too long
                    command.CommandType = System.Data.CommandType.Text;

                    var eventidParam = new SqlParameter("@eventid", obj.EventId);
                    command.Parameters.Add(eventidParam);

                    var descriptionParam = new SqlParameter("@description", obj.Description);
                    command.Parameters.Add(descriptionParam);

                    var coordxParam = new SqlParameter("@coordx", obj.CoordX);
                    command.Parameters.Add(coordxParam);

                    var coordyParam = new SqlParameter("@coordy", obj.CoordY);
                    command.Parameters.Add(coordyParam);

                    var priceParam = new SqlParameter("@price", obj.Price);
                    command.Parameters.Add(priceParam);

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
                command.CommandText = "DELETE FROM EventArea WHERE Id=@id; SELECT @@ROWCOUNT";
                command.CommandType = System.Data.CommandType.Text;

                var idparam = new SqlParameter("@id", id);
                command.Parameters.Add(idparam);

                _connection.Open();

                rowAffected = (int)command.ExecuteScalar();

                _connection.Close();
            }

            return rowAffected;
        }

        public EventArea GetById(int id)
        {
            EventArea eventArea = null;
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT [Id],[EventId],[Description],[CoordX],[CoordY],[Price] FROM EventArea WHERE Id=@id";
                command.CommandType = System.Data.CommandType.Text;

                var idparam = new SqlParameter("@id", id);
                command.Parameters.Add(idparam);

                _connection.Open();

                using (DbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        eventArea = new EventArea
                        {
                            Id = reader.GetInt32(0),
                            EventId = reader.GetInt32(1),
                            Description = reader.GetString(2),
                            CoordX = reader.GetInt32(3),
                            CoordY = reader.GetInt32(4),
                            Price = reader.GetDecimal(5),
                        };
                    }
                }

                _connection.Close();
            }

            return eventArea;
        }

        public IQueryable<EventArea> GetAll()
        {
            List<EventArea> eventAreas = new List<EventArea>();
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT [Id],[EventId],[Description],[CoordX],[CoordY],[Price] FROM EventArea";
                command.CommandType = System.Data.CommandType.Text;

                _connection.Open();

                using (DbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        eventAreas.Add(new EventArea
                        {
                            Id = reader.GetInt32(0),
                            EventId = reader.GetInt32(1),
                            Description = reader.GetString(2),
                            CoordX = reader.GetInt32(3),
                            CoordY = reader.GetInt32(4),
                            Price = reader.GetDecimal(5),
                        });
                    }
                }

                _connection.Close();
            }

            return eventAreas.AsQueryable();
        }

        public int Update(EventArea obj)
        {
            int rowsAffected = 0;
            if (obj != null)
            {
                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "Update EventArea SET EventId=@eventid, Description=@description, CoordX=@coordx, CoordY=@coordy, Price=@price WHERE Id=@id; SELECT @@ROWCOUNT";
                    command.CommandType = System.Data.CommandType.Text;

                    var eventidParam = new SqlParameter("@eventid", obj.EventId);
                    command.Parameters.Add(eventidParam);

                    var descriptionParam = new SqlParameter("@description", obj.Description);
                    command.Parameters.Add(descriptionParam);

                    var coordxParam = new SqlParameter("@coordx", obj.CoordX);
                    command.Parameters.Add(coordxParam);

                    var coordyParam = new SqlParameter("@coordy", obj.CoordY);
                    command.Parameters.Add(coordyParam);

                    var priceParam = new SqlParameter("@price", obj.Price);
                    command.Parameters.Add(priceParam);

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
