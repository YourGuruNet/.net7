using Microsoft.AspNetCore.Mvc;

namespace net7.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService characterService;

        public CharacterController(ICharacterService characterService)
        {
            this.characterService = characterService;
        }
       
        [HttpGet("GetAll")]
        public ActionResult<List<Character>> Get() 
        {
            return Ok(characterService.GetAllCharacters());
        }

        [HttpGet("{id}")]
        public ActionResult<Character> GetByIndex(int id) 
        {
            return Ok(characterService.GetCharacterById(id));
        }

        [HttpPost]
        public ActionResult<List<Character>> AddNew(Character character) 
        {
            return Ok(characterService.AddCharacter(character));
        }

    }
}