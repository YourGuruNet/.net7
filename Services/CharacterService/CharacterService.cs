
namespace net7.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private static List<Character> characters = new() {
            new Character {Id = 0},
            new Character {Id = 1, Name = "Sam"},
            new Character {Id = 2, Name = "Arv"},
        };

        public async Task<ServiceResponse<List<Character>>> AddCharacter(Character character)
        {
            var serviceResponse = new ServiceResponse<List<Character>>();
             characters.Add(character);
             serviceResponse.Data = characters;
             return serviceResponse;
        }

        public async Task<ServiceResponse<List<Character>>> GetAllCharacters()
        {
            var serviceResponse = new ServiceResponse<List<Character>>
            {
                Data = characters
            };
            return serviceResponse;
        }

        public async Task<ServiceResponse<Character>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<Character>();
            var character = characters.FirstOrDefault(character => character.Id == id);
            serviceResponse.Data = character;
            return serviceResponse;  
        }
    }
}