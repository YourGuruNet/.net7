
namespace net7.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private static List<Character> characters = new() {
            new Character {Id = 0},
            new Character {Id = 1, Name = "Sam"},
            new Character {Id = 2, Name = "Arv"},
        };

        public async  Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto character)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
             characters.Add(character);
             serviceResponse.Data = characters;
             return serviceResponse;
        }

        public async  Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>
            {
                Data = characters
            };
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var character = characters.FirstOrDefault(character => character.Id == id);
            serviceResponse.Data = character;
            return serviceResponse;  
        }
    }
}