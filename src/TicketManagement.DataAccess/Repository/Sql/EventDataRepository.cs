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
    public class EventDataRepository : IRepository<EventData>, IEventDataRepositoryExtension
    {
        private readonly DbConnection _connection;

        public EventDataRepository(DbConnection connection)
        {
            _connection = connection;
        }

        public int Insert(EventData obj)
        {
            int insertedId;

            if (obj != null)
            {
                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "Sp_InsertEvent";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    var nameParam = new SqlParameter("@name", obj.Name);
                    command.Parameters.Add(nameParam);

                    var descriptionParam = new SqlParameter("@description", obj.Description);
                    command.Parameters.Add(descriptionParam);

                    var layoutIdParam = new SqlParameter("@layoutId", obj.LayoutId);
                    command.Parameters.Add(layoutIdParam);

                    var dateBeginParam = new SqlParameter("@dateBegin", obj.StartDateTime);
                    command.Parameters.Add(dateBeginParam);

                    var dateEndParam = new SqlParameter("@dateEnd", obj.EndDateTime);
                    command.Parameters.Add(dateEndParam);

                    var categoryParam = new SqlParameter("@category", (int)obj.Category);
                    command.Parameters.Add(categoryParam);

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
                command.CommandText = "Sp_DeleteEvent";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var idparam = new SqlParameter("@id", id);
                command.Parameters.Add(idparam);

                _connection.Open();

                rowAffected = (int)command.ExecuteScalar();

                _connection.Close();
            }

            return rowAffected;
        }

        public List<EventData> FilterByVenueId(int input)
        {
            var events = new List<EventData>();
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "Sp_FilterEventsByVenueId";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var venueParam = new SqlParameter("@venue", input);
                command.Parameters.Add(venueParam);

                _connection.Open();

                using (DbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        events.Add(new EventData
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2),
                            LayoutId = reader.GetInt32(3),
                            StartDateTime = reader.GetDateTime(4),
                            EndDateTime = reader.GetDateTime(5),
                            Category = (Category)reader.GetInt32(6),
                        });
                    }
                }

                _connection.Close();
            }

            return events;
        }

        public EventData GetById(int id)
        {
            EventData eventData = null;
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "Sp_GetByIdEvent";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var idparam = new SqlParameter("@id", id);
                command.Parameters.Add(idparam);

                _connection.Open();

                using (DbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        eventData = new EventData
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2),
                            LayoutId = reader.GetInt32(3),
                            StartDateTime = reader.GetDateTime(4),
                            EndDateTime = reader.GetDateTime(5),
                            Category = (Category)reader.GetInt32(6),
                        };
                    }
                }

                _connection.Close();
            }

            return eventData;
        }

        public IQueryable<EventData> GetAll()
        {
            List<EventData> eventdbs = new List<EventData>();
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "Sp_GetValuesEvent";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                _connection.Open();

                using (DbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        eventdbs.Add(new EventData
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2),
                            LayoutId = reader.GetInt32(3),
                            StartDateTime = reader.GetDateTime(4),
                            EndDateTime = reader.GetDateTime(5),
                            Category = (Category)reader.GetInt32(6),
                        });
                    }
                }

                _connection.Close();
            }

            return eventdbs.AsQueryable();
        }

        public int Update(EventData obj)
        {
            int rowsAffected = 0;

            if (obj != null)
            {
                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "Sp_UpdateEvent";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    var nameParam = new SqlParameter("@name", obj.Name);
                    command.Parameters.Add(nameParam);

                    var descriptionParam = new SqlParameter("@description", obj.Description);
                    command.Parameters.Add(descriptionParam);

                    var layoutIdParam = new SqlParameter("@layoutId", obj.LayoutId);
                    command.Parameters.Add(layoutIdParam);

                    var dateBeginParam = new SqlParameter("@dateBegin", obj.StartDateTime);
                    command.Parameters.Add(dateBeginParam);

                    var dateEndParam = new SqlParameter("@dateEnd", obj.EndDateTime);
                    command.Parameters.Add(dateEndParam);

                    var idparam = new SqlParameter("@id", obj.Id);
                    command.Parameters.Add(idparam);

                    var categoryParam = new SqlParameter("@category", (int)obj.Category);
                    command.Parameters.Add(categoryParam);

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
