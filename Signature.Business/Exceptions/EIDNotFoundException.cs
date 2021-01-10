using System;
using System.Collections.Generic;
using System.Text;

namespace Signature.Business.Exceptions
{
    public class EIDNotFoundException : Exception
    {
        private string _message;

        public override string Message
        {
            get { return _message; }
        }
        public EIDNotFoundException()
        {
            _message = "Geen eID gevonden. Plaats een geldige eID in de kaartlezer en probeer opnieuw.";
        }
    }
}
