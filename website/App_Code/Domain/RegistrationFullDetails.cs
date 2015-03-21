using System;

[Serializable]
public class RegistrationFullDetails
{
	public RegistrationDetails RegistrationDetails { get; set; }
	public MembershipOptions MembershipOptions { get; set; }
}