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
using System.IO;

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1
{
	public class BerTaggedObjectParser
		: Asn1TaggedObjectParser
	{
		private bool				_constructed;
		private int					_tagNumber;
		private Asn1StreamParser	_parser;

		[Obsolete]
		internal BerTaggedObjectParser(
			int		baseTag,
			int		tagNumber,
			Stream	contentStream)
			: this((baseTag & Asn1Tags.Constructed) != 0, tagNumber, new Asn1StreamParser(contentStream))
		{
		}

		internal BerTaggedObjectParser(
			bool				constructed,
			int					tagNumber,
			Asn1StreamParser	parser)
		{
			_constructed = constructed;
			_tagNumber = tagNumber;
			_parser = parser;
		}

		public bool IsConstructed
		{
			get { return _constructed; }
		}

		public int TagNo
		{
			get { return _tagNumber; }
		}

		public IAsn1Convertible GetObjectParser(
			int		tag,
			bool	isExplicit)
		{
			if (isExplicit)
			{
				if (!_constructed)
					throw new IOException("Explicit tags must be constructed (see X.690 8.14.2)");

				return _parser.ReadObject();
			}

			return _parser.ReadImplicit(_constructed, tag);
		}

		public Asn1Object ToAsn1Object()
		{
			try
			{
				return _parser.ReadTaggedObject(_constructed, _tagNumber);
			}
			catch (IOException e)
			{
				throw new Asn1ParsingException(e.Message);
			}
		}
	}
}
