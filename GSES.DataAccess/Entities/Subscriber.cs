using GSES.DataAccess.Entities.Bases;

namespace GSES.DataAccess.Entities
{
    public class Subscriber : BaseEntity
    {
        public string Email { get; set; }

        public override bool Equals(object obj) => ((Subscriber)obj).Email == this.Email;
    }
}
