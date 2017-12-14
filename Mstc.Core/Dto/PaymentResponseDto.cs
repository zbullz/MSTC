using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Management;

namespace Mstc.Core.Dto
{
    public enum PaymentResponseDto
    {
        Success = 1,
        MandateError = 2,
        UnknownError = 3
    }
}
