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

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Net.Sf.Pkcs11;
using Net.Sf.Pkcs11.Objects;
using Net.Sf.Pkcs11.Wrapper;
using System.Security.Cryptography.X509Certificates;
using Signature.Business.Exceptions;

namespace Signature.Business
{
    public class ReadData
    {
        private Module m = null;
        private string moduleFileName;

        // Default constructor. Will instantiate the beidpkcs11.dll pkcs11 module
        public ReadData()
        {
            moduleFileName = "beidpkcs11.dll";
        }

        public ReadData(string moduleFileName)
        {
            this.moduleFileName = moduleFileName;
        }   

        // Tries to create a Session, throws EIDNotFoundException when no eID is found
        private Session CreateSession(Slot slot)
        {
            try
            {
                return slot.Token.OpenSession(true);
            }
            catch
            {
                throw new EIDNotFoundException();
            }
        }

        // Get firstnames of the owner of the token (eid) in the first non-empty slot (cardreader)
        public string GetFirstnames()
        {
            return GetData("firstnames");
        }

        // Get surname of the owner of the token (eid) in the first non-empty slot (cardreader)
        public string GetSurname()
        {
            return GetData("surname");
        }

        // Get gender of the owner of the token (eid) in the first non-empty slot (cardreader)
        public string GetGender()
        {
            return GetData("gender");
        }

        // Get location of birth of the owner of the token (eid) in the first non-empty slot (cardreader)
        public string GetLocationOfBirth()
        {
            return GetData("location_of_birth");
        }

        // Get date of birth of the owner of the token (eid) in the first non-empty slot (cardreader)
        public string GetDateOfBirth()
        {
            return GetData("date_of_birth");
        }

        // Get nationality of the owner of the token (eid) in the first non-empty slot (cardreader)
        public string GetNationality()
        {
            return GetData("nationality");
        }

        // Get the "signature" leaf certificate file
        public byte[] GetCertificateSignatureFile()
        {
            return GetCertificateFile("Signature");
        }

        // Generic function to get string data objects from label
        // param name="label": Value of label attribute to the object
        private string GetData(string label)
        {
            string value = "";

            if (m == null)
            {
                m = Module.GetInstance(moduleFileName);
            }

            try
            {
                // Get the slots (cardreader) with a token (eid)
                Slot[] slotlist = m.GetSlotList(true);

                if (slotlist.Length > 0)
                {
                    Slot slot = slotlist[0];
                    Session session = CreateSession(slot);

                    if (session != null)
                    {
                        // Search for objects
                        // First, define a search template 

                        // "The label attribute of the objects should equal ..."
                        ByteArrayAttribute classAttribute = new ByteArrayAttribute(CKA.CLASS);
                        classAttribute.Value = BitConverter.GetBytes((uint)CKO.DATA);

                        ByteArrayAttribute labelAttribute = new ByteArrayAttribute(CKA.LABEL);
                        labelAttribute.Value = Encoding.UTF8.GetBytes(label);

                        session.FindObjectsInit(new P11Attribute[] { classAttribute, labelAttribute });
                        P11Object[] foundObjects = session.FindObjects(50);
                        int counter = foundObjects.Length;
                        Data data;

                        while (counter > 0)
                        {
                            data = foundObjects[counter - 1] as Data;
                            label = data.Label.ToString();

                            if (data.Value.Value != null)
                            {
                                value = Encoding.UTF8.GetString(data.Value.Value);
                            }
                            counter--;
                        }

                        session.FindObjectsFinal();
                        session.Dispose();
                    }
                }
                else
                {
                    throw new EIDNotFoundException();
                }
            }
            finally
            {
                m.Dispose();
                m = null;
            }

            return value;
        }

        // returns Root Certificate on the eid.
        private byte[] GetCertificateFile(string certificateName)
        {
            byte[] value = null;

            if (m == null)
            {
                m = Module.GetInstance(moduleFileName);
            }

            try
            {
                // Get the first slot (cardreader) with a token
                Slot[] slotlist = m.GetSlotList(true);
                if (slotlist.Length > 0)
                {
                    Slot slot = slotlist[0];
                    Session session = slot.Token.OpenSession(true);
                    // Search for objects
                    // First, define a search template 

                    // "The label attribute of the objects should equal ..."      
                    ByteArrayAttribute fileLabel = new ByteArrayAttribute(CKA.LABEL);
                    ObjectClassAttribute certificateAttribute = new ObjectClassAttribute(CKO.CERTIFICATE);
                    fileLabel.Value = Encoding.UTF8.GetBytes(certificateName);

                    session.FindObjectsInit(new P11Attribute[] {
                        certificateAttribute,
                        fileLabel
                    });

                    P11Object[] foundObjects = session.FindObjects(1);
                    if (foundObjects.Length != 0)
                    {
                        X509PublicKeyCertificate cert = foundObjects[0] as X509PublicKeyCertificate;
                        value = cert.Value.Value;
                    }

                    session.FindObjectsFinal();
                }
                else
                {
                    throw new EIDNotFoundException();
                }
            }
            finally
            {
                m.Dispose();
                m = null;
            }

            return value;
        }
    }
}
