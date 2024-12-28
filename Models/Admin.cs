namespace PetAdminApi.Models
{
    public class Admin : User
    {
        public string? PasswordHash { get; set; }
    }

}
