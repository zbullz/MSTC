using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

public enum PaymentStates
{
	[Description("Swim 5 Credits")] S00599C = 5,
	[Description("Swim 10 Credits")] S001099C = 10,
	[Description("Swim 15 Credits")] S001599C = 15,
	[Description("Duathlon")] E00D101C = 101,
	[Description("5-3-1 Charity Swim")] E00S102C = 102,
	[Description("Triathlon Festival")] E00T103C = 103,

}
