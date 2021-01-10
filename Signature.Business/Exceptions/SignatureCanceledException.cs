using System;
using System.Collections.Generic;
using System.Text;

namespace Signature.Business.Exceptions
{
    public class SignatureCanceledException : Exception
    {
        private string _message;

        public override string Message
        {
            get { return _message; }
        }
        public SignatureCanceledException()
        {
            _message = "Het plaatsen van een digitale handtekening is geannuleerd.";
        }
    }
}
