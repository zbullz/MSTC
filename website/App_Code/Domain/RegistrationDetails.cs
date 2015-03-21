using System;

[Serializable]
public class RegistrationDetails
{
	public string FullName { get; set; }
	public string Email { get; set; }
	public string Password { get; set; }
	public string Gender { get; set; }
	public DateTime DateOfBirth { get; set; }
	public string Address1 { get; set; }
	public string City { get; set; }
	public string Postcode { get; set; }
	public string PhoneNumber { get; set; }
	public string BTFNumber { get; set; }
	public string MedicalConditions { get; set; }
	public string EmergencyContactName { get; set; }
	public string EmergencyContactPhone { get; set; }
}