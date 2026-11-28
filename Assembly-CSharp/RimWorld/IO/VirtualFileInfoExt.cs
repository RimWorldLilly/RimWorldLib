using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace RimWorld.IO
{
	// Token: 0x02002407 RID: 9223
	public static class VirtualFileInfoExt
	{
		// Token: 0x0600E6E1 RID: 59105 RVA: 0x0055D1B4 File Offset: 0x0055B3B4
		public static XDocument LoadAsXDocument(this VirtualFile file)
		{
			XDocument result;
			using (Stream stream = file.CreateReadStream())
			{
				result = XDocument.Load(XmlReader.Create(stream), LoadOptions.SetLineInfo);
			}
			return result;
		}

		// Token: 0x0600E6E2 RID: 59106 RVA: 0x0055D1F4 File Offset: 0x0055B3F4
		public static XDocument LoadAsXDocument(string fileContents)
		{
			XDocument result;
			using (StringReader stringReader = new StringReader(fileContents))
			{
				result = XDocument.Load(stringReader, LoadOptions.SetLineInfo);
			}
			return result;
		}
	}
}
