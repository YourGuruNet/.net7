
namespace net7.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private static List<Character> characters = new() {
            new Character {Id = 0},
            new Character {Id = 1, Name = "Sam"},
            new Character {Id = 2, Name = "Arv"},
        };

        public List<Character> AddCharacter(Character character)
        {
             characters.Add(character);
             return characters;
        }

        public List<Character> GetAllCharacters()
        {
            return characters;
        }

        public Character GetCharacterById(int id)
        {
            var character =characters.FirstOrDefault(character => character.Id == id);
            if(character is not null) 
                return character;
            
            throw new Exception("Character not found");
        }
    }
}