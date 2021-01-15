/* ****************************************************************************

 * eID Middleware Project.
 * Copyright (C) 2010-2010 FedICT.
 *
 * This is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser General Public License version
 * 3.0 as published by the Free Software Foundation.
 *
 * This software is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this software; if not, see
 * http://www.gnu.org/licenses/.

**************************************************************************** */

using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Security;

namespace Signature.Business
{
    // To verity certificates and signatures
    public class Integrity
    {
        public Integrity()
        {
        }

        // Verify a signature with a given certificate. It is assumed that the signature is made from a SHA1 hash of the data.
        // param name="dummyPDF": PDF data to be signed
        // param name="signedData"> Signed data
        // name="certificate">Certificate containing the public key used to verify the code
        // returns true if the verification succeeds
        public bool Verify(byte[] dummyPDF, byte[] signedData, byte[] certificate)
        {
            try
            {
                // create certificate object from byte file 'certificate' 
                X509Certificate2 x509Certificate = new X509Certificate2(certificate);

                // use public key from certificate during verification
                RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)x509Certificate.PublicKey.Key;

                // verify signature. Assume that the data was SHA1 hashed.
                return rsa.VerifyData(dummyPDF, "SHA1", signedData);
            }
            catch
            {
                throw new VerificationException();
            }
        }
    }
}
