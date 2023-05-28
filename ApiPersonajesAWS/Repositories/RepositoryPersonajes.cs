using ApiPersonajesAWS.Data;
using ApiPersonajesAWS.Models;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.Data;

#region PROCEDURES

/*
DELIMITER $$
CREATE PROCEDURE SP_UPDATE_PERSONAJE (id INT, nombre NVARCHAR(21845), imagen NVARCHAR(21845))
BEGIN
    UPDATE PERSONAJES SET PERSONAJE = personaje, IMAGEN = imagen WHERE IDPERSONAJE = id;
END$$

DELIMITER $$
CREATE PROCEDURE SP_INSERT_PERSONAJE (nombre NVARCHAR(21845), imagen NVARCHAR(21845))
BEGIN
	DECLARE MAX_ID INT;
    SET MAX_ID = (SELECT (max(IDPERSONAJE)) FROM PERSONAJES) + 1;
	INSERT PERSONAJES VALUES (MAX_ID,nombre,imagen);
END$$

DELIMITER $$
CREATE PROCEDURE SP_DELETE_PERSONAJE (id INT)
BEGIN
	DELETE FROM PERSONAJES WHERE ID = id;
END$$
*/

#endregion

namespace ApiPersonajesAWS.Repositories
{
    public class RepositoryPersonajes
    {
        private PersonajesContext context;

        private MySqlConnection conn;
        private MySqlCommand cmd;
        private string connectionString;

        public RepositoryPersonajes(PersonajesContext context, IConfiguration config)
        {
            this.context = context;
            this.cmd = new MySqlCommand();
            this.conn = new MySqlConnection(config.GetConnectionString("MySqlTelevision"));
            
        }

        private int GetMaxIdPersonaje()
        {
            if (this.context.Personajes.Count() != 0)
            {
                return this.context.Personajes.Max(x => x.IdPersonaje) + 1;
            }

            return 1;
        }
        
        public async Task<List<Personaje>> GetPersonajesAsync()
        {
            return await this.context.Personajes.ToListAsync();
        }

        public async Task<Personaje> FindPersonajeAsync(int id)
        {
            return await this.context.Personajes.FirstOrDefaultAsync(x => x.IdPersonaje == id);
        }

        public async Task CreatePersonaje(Personaje personaje)
        {
            personaje.IdPersonaje = this.GetMaxIdPersonaje();
            this.context.Personajes.Add(personaje);
            await this.context.SaveChangesAsync();
        }

        public async Task UpdatePersonajeProcedure(int id, string nombre, string imagen)
        {
            this.conn.Open();
            this.cmd.Connection = this.conn;

            this.cmd.CommandText = "SP_UPDATE_PERSONAJE";
            this.cmd.CommandType = CommandType.StoredProcedure;

            this.cmd.Parameters.AddWithValue("@id", id);
            this.cmd.Parameters.AddWithValue("@nombre", nombre);
            this.cmd.Parameters.AddWithValue("@imagen", imagen);

            await this.cmd.ExecuteNonQueryAsync();

            this.cmd.Parameters.Clear();
            this.conn.Close();
            
        }

        public async Task InsertPersonajeAsync(string nombre, string imagen)
        {
            this.conn.Open();
            this.cmd.Connection = this.conn;

            this.cmd.CommandText = "SP_INSERT_PERSONAJE";
            this.cmd.CommandType = CommandType.StoredProcedure;

            this.cmd.Parameters.AddWithValue("@nombre", nombre);
            this.cmd.Parameters.AddWithValue("@imagen", imagen);

            await this.cmd.ExecuteNonQueryAsync();

            this.cmd.Parameters.Clear();
            this.conn.Close();

        }

        public async Task DeletePersonajeProcedure(int id)
        {
            this.conn.Open();
            this.cmd.Connection = this.conn;

            this.cmd.CommandText = "SP_DELETE_PERSONAJE";
            this.cmd.CommandType = CommandType.StoredProcedure;

            this.cmd.Parameters.AddWithValue("@id", id);

            await this.cmd.ExecuteNonQueryAsync();

            this.cmd.Parameters.Clear();
            this.conn.Close();
        }


    }
}
