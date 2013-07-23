namespace prepo.Api.Contracts.Models
{
    public class PrepoUser : DbObject
    {
        public PrepoUser(string id)
        {
            Id = id;
        }

        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class PrepoSetting : DbObject
    {
        public PrepoSetting(string id)
        {
            Id = id;
        }

        public string Value;
    }
}