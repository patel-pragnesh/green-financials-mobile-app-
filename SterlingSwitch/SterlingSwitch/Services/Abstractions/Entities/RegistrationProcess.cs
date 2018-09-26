namespace switch_mobile.Services.Abstractions.Entities
{
    public class RegistrationProcess
    {
        public static string Firstname { get; set; }
        public static string Lastname { get; set; }
        public static string EmailAddress { get; set; }
        public static string PhoneNumber { get; set; }
        public static string DateOfBirth { get; set; }
        public static string CountryCode { get; set; }
        public static string DefaultAccountNumber { get; set; }
        public static string Gender { get; set; }
        public static string RefferedBy { get; set; }
        public static string Pin { get; set; }
        public static string ConfirmPin { get; set; }

        public static double RegistrationProgress { get; set; }
    }

}
