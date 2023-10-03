namespace net7.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private static List<Character> characters = new() {
            new Character {Id = 0},
            new Character {Id = 1, Name = "Sam"},
            new Character {Id = 2, Name = "Arv"},
        };
        private readonly IMapper mapper;

        public CharacterService(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public async  Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto character)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var newCharacter = mapper.Map<Character>(character);
            newCharacter.Id = characters.Count + 1;
             characters.Add(newCharacter);
             serviceResponse.Data = characters.Select(c => mapper.Map<GetCharacterDto>(c)).ToList();
             return serviceResponse;
        }

        public async  Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>
            {
                Data = characters.Select(c => mapper.Map<GetCharacterDto>(c)).ToList()
            };
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var character = characters.FirstOrDefault(character => character.Id == id);
            serviceResponse.Data = mapper.Map<GetCharacterDto>(character);
            return serviceResponse;  
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto character) 
        {   
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            try {
                var newCharacter = characters.FirstOrDefault(c => c.Id == character.Id) ?? throw new Exception($"Characters Id is incorrect");
                mapper.Map(character, newCharacter);
                serviceResponse.Data = mapper.Map<GetCharacterDto>(newCharacter);
                return serviceResponse; 
            } 
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                return serviceResponse;
            }
        }
    }
}