using ApiPersonajesAWS.Models;
using ApiPersonajesAWS.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiPersonajesAWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonajesController : ControllerBase
    {
        private RepositoryPersonajes repo;

        public PersonajesController(RepositoryPersonajes repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<List<Personaje>>> GetPersonajes()
        {
            return await this.repo.GetPersonajesAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Personaje>> FindPersonaje(int id)
        {
            return await this.repo.FindPersonajeAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult> CreatePersonaje(Personaje personaje)
        {
            await this.repo.CreatePersonaje(personaje);
            return Ok();
        }

        [HttpPut("{id}/{nombre}/{imagen}")]
        public async Task<ActionResult> UpdatePersonaje(int id, string nombre, string imagen)
        {
            await this.repo.UpdatePersonajeProcedure(id, nombre, imagen);
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> DeletePersonaje(int id)
        {
            await this.repo.DeletePersonajeProcedure(id);
            return Ok();
        }

        [HttpPost("{nombre}/{imagen}")]
        public async Task<ActionResult> InsertPersonaje(string nombre, string imagen)
        {
            await this.repo.InsertPersonajeAsync(nombre, imagen);
            return Ok();
        }
    }
}
