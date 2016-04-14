
using System.ComponentModel.DataAnnotations;
namespace Patient_Transport.Models.ViewModel {
    public class LoginModel {

        [Display(Name= "Gebruikersnaam")]
        [Required(ErrorMessage = "Verplicht veld")]
        public string UserName { get; set; }
        
        [Display(Name= "Paswoord")]
        [Required(ErrorMessage = "Verplicht veld")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}