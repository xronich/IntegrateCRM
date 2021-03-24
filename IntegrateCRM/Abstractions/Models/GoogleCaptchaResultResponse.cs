using System.Collections.Generic;

namespace ContactUs.Models
{
    public class GoogleCaptchaResultResponse
    {
        public bool Success { get; set; }
        public List<string> ErrorCodes { get; set; }
        public int Score { get; set; }
    }
}
