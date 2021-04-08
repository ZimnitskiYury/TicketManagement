using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using TicketManagement.DataAccess.Entities;
using TicketManagement.DataAccess.Interfaces;

namespace TicketManagement.DataAccess.Repository.Sql
{
    public class EventSeatRepository : IRepository<EventSeat>
    {
        private readonly DbConnection _connection;

        public EventSeatRepository(DbConnection connection)
        {
            _connection = connection;
        }

        public int Insert(EventSeat obj)
        {
            int insertedId;

            if (obj != null)
            {
                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO EventSeat (EventAreaId, Row, Number, State) VALUES (@eventareaid, @row, @number, @state); SELECT CAST(scope_identity() as int)";
                    command.CommandType = System.Data.CommandType.Text;

                    var eventareaidParam = new SqlParameter("@eventareaid", obj.EventAreaId);
                    command.Parameters.Add(eventareaidParam);

                    var rowParam = new SqlParameter("@row", obj.Row);
                    command.Parameters.Add(rowParam);

                    var numberParam = new SqlParameter("@number", obj.Number);
                    command.Parameters.Add(numberParam);

                    var stateParam = new SqlParameter("@state", (int)obj.State);
                    command.Parameters.Add(stateParam);

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
                command.CommandText = "DELETE FROM EventSeat WHERE Id=@id; SELECT @@ROWCOUNT";
                command.CommandType = System.Data.CommandType.Text;

                var idparam = new SqlParameter("@id", id);
                command.Parameters.Add(idparam);

                _connection.Open();

                rowAffected = (int)command.ExecuteScalar();

                _connection.Close();
            }

            return rowAffected;
        }

        public EventSeat GetById(int id)
        {
            EventSeat eventSeat = null;
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT [Id],[EventAreaId],[Row],[Number],[State] FROM EventSeat WHERE Id=@id";
                command.CommandType = System.Data.CommandType.Text;

                var idparam = new SqlParameter("@id", id);
                command.Parameters.Add(idparam);

                _connection.Open();

                using (DbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        eventSeat = new EventSeat
                        {
                            Id = reader.GetInt32(0),
                            EventAreaId = reader.GetInt32(1),
                            Row = reader.GetInt32(2),
                            Number = reader.GetInt32(3),
                            State = (StateSeat)reader.GetInt32(4),
                        };
                    }
                }

                _connection.Close();
            }

            return eventSeat;
        }

        public IQueryable<EventSeat> GetAll()
        {
            List<EventSeat> eventSeats = new List<EventSeat>();
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT [Id],[EventAreaId],[Row],[Number],[State] FROM EventSeat";
                command.CommandType = System.Data.CommandType.Text;

                _connection.Open();

                using (DbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        eventSeats.Add(new EventSeat
                        {
                            Id = reader.GetInt32(0),
                            EventAreaId = reader.GetInt32(1),
                            Row = reader.GetInt32(2),
                            Number = reader.GetInt32(3),
                            State = (StateSeat)reader.GetInt32(4),
                        });
                    }
                }

                _connection.Close();
            }

            return eventSeats.AsQueryable();
        }

        public int Update(EventSeat obj)
        {
            int rowsAffected = 0;
            if (obj != null)
            {
                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "Update EventSeat SET EventAreaId=@eventareaid, Row=@row, Number=@number, State=@state WHERE Id=@id; SELECT @@ROWCOUNT";
                    command.CommandType = System.Data.CommandType.Text;

                    var idparam = new SqlParameter("@id", obj.Id);
                    command.Parameters.Add(idparam);

                    var eventareaidParam = new SqlParameter("@eventareaid", obj.EventAreaId);
                    command.Parameters.Add(eventareaidParam);

                    var rowParam = new SqlParameter("@row", obj.Row);
                    command.Parameters.Add(rowParam);

                    var numberParam = new SqlParameter("@number", obj.Number);
                    command.Parameters.Add(numberParam);

                    var stateParam = new SqlParameter("@state", (int)obj.State);
                    command.Parameters.Add(stateParam);

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
