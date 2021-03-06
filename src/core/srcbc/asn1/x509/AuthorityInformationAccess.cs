/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2019 iText Group NV
    Authors: iText Software.

This program is free software; you can redistribute it and/or modify it under the terms of the GNU Affero General Public License version 3 as published by the Free Software Foundation with the addition of the following permission added to Section 15 as permitted in Section 7(a): FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY iText Group NV, iText Group NV DISCLAIMS THE WARRANTY OF NON INFRINGEMENT OF THIRD PARTY RIGHTS.

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Affero General Public License for more details.
You should have received a copy of the GNU Affero General Public License along with this program; if not, see http://www.gnu.org/licenses or write to the Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA, 02110-1301 USA, or download the license from the following URL:

http://itextpdf.com/terms-of-use/

The interactive user interfaces in modified source and object code versions of this program must display Appropriate Legal Notices, as required under Section 5 of the GNU Affero General Public License.

In accordance with Section 7(b) of the GNU Affero General Public License, a covered work must retain the producer line in every PDF that is created or manipulated using iText.

You can be released from the requirements of the license by purchasing a commercial license. Buying such a license is mandatory as soon as you develop commercial activities involving the iText software without disclosing the source code of your own applications.
These activities include: offering paid services to customers as an ASP, serving PDFs on the fly in a web application, shipping iText with a closed source product.

For more information, please contact iText Software Corp. at this address: sales@itextpdf.com */
using System;
using System.Collections;
using System.Text;

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.X509
{
	/**
	 * The AuthorityInformationAccess object.
	 * <pre>
	 * id-pe-authorityInfoAccess OBJECT IDENTIFIER ::= { id-pe 1 }
	 *
	 * AuthorityInfoAccessSyntax  ::=
	 *      Sequence SIZE (1..MAX) OF AccessDescription
	 * AccessDescription  ::=  Sequence {
	 *       accessMethod          OBJECT IDENTIFIER,
	 *       accessLocation        GeneralName  }
	 *
	 * id-ad OBJECT IDENTIFIER ::= { id-pkix 48 }
	 * id-ad-caIssuers OBJECT IDENTIFIER ::= { id-ad 2 }
	 * id-ad-ocsp OBJECT IDENTIFIER ::= { id-ad 1 }
	 * </pre>
	 */
	public class AuthorityInformationAccess
		: Asn1Encodable
	{
		private readonly AccessDescription[] descriptions;

		public static AuthorityInformationAccess GetInstance(
			object obj)
		{
			if (obj is AuthorityInformationAccess)
				return (AuthorityInformationAccess) obj;

			if (obj is Asn1Sequence)
				return new AuthorityInformationAccess((Asn1Sequence) obj);

			if (obj is X509Extension)
				return GetInstance(X509Extension.ConvertValueToObject((X509Extension) obj));

			throw new ArgumentException("unknown object in factory: " + obj.GetType().Name, "obj");
		}

		private AuthorityInformationAccess(
			Asn1Sequence seq)
		{
			if (seq.Count < 1)
				throw new ArgumentException("sequence may not be empty");

			this.descriptions = new AccessDescription[seq.Count];

			for (int i = 0; i < seq.Count; ++i)
			{
				descriptions[i] = AccessDescription.GetInstance(seq[i]);
			}
		}

		/**
		 * create an AuthorityInformationAccess with the oid and location provided.
		 */
		[Obsolete("Use version taking an AccessDescription instead")]
		public AuthorityInformationAccess(
			DerObjectIdentifier	oid,
			GeneralName			location)
		{
			this.descriptions = new AccessDescription[]{ new AccessDescription(oid, location) };
		}

		public AuthorityInformationAccess(
			AccessDescription description)
		{
			this.descriptions = new AccessDescription[]{ description };
		}

		public AccessDescription[] GetAccessDescriptions()
		{
			return (AccessDescription[]) descriptions.Clone();
		}

		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(descriptions);
		}

		public override string ToString()
		{
			StringBuilder buf = new StringBuilder();
			string sep = Platform.NewLine;

			buf.Append("AuthorityInformationAccess:");
			buf.Append(sep);

			foreach (AccessDescription description in descriptions)
			{
				buf.Append("    ");
				buf.Append(description);
				buf.Append(sep);
			}

			return buf.ToString();
		}
	}
}
