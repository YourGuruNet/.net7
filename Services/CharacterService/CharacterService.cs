namespace net7.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dataContext;

        public CharacterService(IMapper mapper ,DataContext dataContext)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto character)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

            try
            {
                var newCharacter = _mapper.Map<Character>(character);
                _dataContext.Characters.Add(newCharacter);
                await _dataContext.SaveChangesAsync(); 
                var dbCharacters = await _dataContext.Characters.ToListAsync();
                serviceResponse.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = $"Error adding character to the database. Error - {ex}";
            }

            return serviceResponse;
        }

        public async  Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters(int userId)
        {
            var dbCharacters = await _dataContext.Characters.Where(character => character.User!.Id == userId).ToListAsync();
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>
            {
                Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList()
            };
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var character = await _dataContext.Characters.FirstOrDefaultAsync(character => character.Id == id);
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            return serviceResponse;  
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto character) 
        {   
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            try 
            {
                var newCharacter = await _dataContext.Characters.FirstOrDefaultAsync(c => c.Id == character.Id) ?? throw new Exception($"Characters Id is incorrect");
                _mapper.Map(character, newCharacter);
                await _dataContext.SaveChangesAsync(); 
                serviceResponse.Data = _mapper.Map<GetCharacterDto>(newCharacter);
                return serviceResponse; 
            } 
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                return serviceResponse;
            }
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            try 
            {
                var character = await _dataContext.Characters.FirstAsync(character => character.Id == id) ?? throw new Exception($"Characters Id is incorrect");
                _dataContext.Characters.Remove(character);
                await _dataContext.SaveChangesAsync();
                serviceResponse.Data = await _dataContext.Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
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