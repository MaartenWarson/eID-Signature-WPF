﻿/* ****************************************************************************

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

using System;
using System.Text;
using Net.Sf.Pkcs11;
using Net.Sf.Pkcs11.Objects;
using Net.Sf.Pkcs11.Wrapper;
using Signature.Business.Exceptions;
using SautinSoft.Document;
using System.Linq;

namespace Signature.Business
{
    public class Sign
    {
        private Module m = null;
        private string moduleFileName;

        // Default constructor. Will instantiate the beidpkcs11.dll pkcs11 module
        public Sign()
        {
            moduleFileName = "beidpkcs11.dll";
        }
        public Sign(string moduleFileName)
        {
            this.moduleFileName = moduleFileName;
        }

        // Sign data with a named private key
        // param name="data": Data to be signed
        // param name="privatekeylabel": Label for private key. (Can be "Signature" or "Authentication")
        // returns Signed data
        public byte[] DoSign(byte[] data, string privatekeylabel)
        {
            byte[] encryptedData = null;
            Session session = null;

            if (m == null)
            {
                m = Module.GetInstance(moduleFileName);
            }

            try
            {
                // Get the first slot (cardreader) with a token (eid)
                Slot slot = m.GetSlotList(true)[0];
                session = slot.Token.OpenSession(true);
                ObjectClassAttribute classAttribute = new ObjectClassAttribute(CKO.PRIVATE_KEY);
                ByteArrayAttribute keyLabelAttribute = new ByteArrayAttribute(CKA.LABEL);
                keyLabelAttribute.Value = Encoding.UTF8.GetBytes(privatekeylabel);

                session.FindObjectsInit(new P11Attribute[] {
                     classAttribute,
                     keyLabelAttribute
                    }
                );

                P11Object[] privatekeys = session.FindObjects(1);
                session.FindObjectsFinal();

                if (privatekeys.Length >= 1)
                {
                    session.SignInit(new Mechanism(CKM.SHA1_RSA_PKCS), (PrivateKey)privatekeys[0]);
                    encryptedData = session.Sign(data);
                }
            }
            catch (TokenException)
            {
                if (session == null)
                {
                    throw new EIDNotFoundException();
                }
                else if (encryptedData == null)
                {
                    throw new SignatureCanceledException();
                }
            }
            finally
            {
                m.Dispose();
                m = null;
            }

            return encryptedData;
        }

        public bool SignPhysically(string filePath, string firstnames, string surname, string certificateSerialNumber)
        {
            string fileResult = filePath + "/Dummy file (signed).pdf";
            DocumentCore dc = DocumentCore.Load(filePath + "/Dummy file.pdf");

            ContentRange cr = dc.Content.Find("_").FirstOrDefault();

            if (cr != null)
            {
                cr.Start.Insert($"Digitaal getekend door {firstnames} {surname} op {DateTime.Now}. \n\r Serienummer certificaat: {certificateSerialNumber}");
                dc.Save(fileResult);

                return true;
            }

            return false;
        }
    }
}
