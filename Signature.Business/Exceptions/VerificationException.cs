using System;
using System.Collections.Generic;
using System.Text;

namespace Signature.Business.Exceptions
{
    public class VerificationException : Exception
    {
        private string _message;

        public override string Message
        {
            get { return _message; }
        }
        public VerificationException()
        {
            _message = "Het digitaal tekenen is niet gelukt omdat de verificatie van X509Certificate2 is mislukt.";
        }
    }
}
