using System.ComponentModel;

namespace Mstc.Core.Domain
{
	public enum PaymentStates
	{
		[Description("Swim Credits GBP16")] S00599C = 16,
		[Description("Swim Credits GBP32")] S001099C = 32,
		[Description("Swim Credits GBP44")] S001599C = 44,
        [Description("Swim Credits GBP24")] S002499C = 24,

        [Description("Duathlon")] E00D101C = 101,
	
		[Description("Triathlon Festival - Olympic Individual")] E00TRIOI201C = 201,
		[Description("Triathlon Festival - Olympic Relay")] E00TRIOR202C = 202,
		[Description("Triathlon Festival - Middle Individual")] E00TRIMI203C = 203,
		[Description("Triathlon Festival - Middle Relay")] E00TRIMR204C = 204,

		[Description("Charity Swim - 1km")] E00S1KM301C = 301,
		[Description("Charity Swim - 3km")] E00S3KM302C = 302,
		[Description("Charity Swim - 5km")] E00S5KM303C = 303,
		[Description("Charity Swim - 1km, 3km")] E00S1KM3KM304C = 304,
		[Description("Charity Swim - 1km, 5km")] E00S1KM5KM305C = 305,
		[Description("Charity Swim - 3km, 5km")] E00S3KM5KM306C = 306,
		[Description("Charity Swim - 1km, 3km, 5km")] E00S1KM3KM5KM307C = 307,

		[Description("Swim subs Apr to Sept")] SS05991 = 401,
		[Description("Swim subs Oct to Mar")] SS05992 = 402,
		[Description("Swim subs Jan to Mar")] SS05996 = 403,
	}
}
