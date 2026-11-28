using System;
using System.Collections.Generic;
using System.Xml.Linq;
using RimWorld.IO;

namespace Verse
{
	// Token: 0x0200050C RID: 1292
	public static class DirectXmlLoaderSimple
	{
		// Token: 0x06002644 RID: 9796 RVA: 0x000E9388 File Offset: 0x000E7588
		public static IEnumerable<DirectXmlLoaderSimple.XmlKeyValuePair> ValuesFromXmlFile(string fileContents)
		{
			return DirectXmlLoaderSimple.ValuesFromXmlFile(VirtualFileInfoExt.LoadAsXDocument(fileContents));
		}

		// Token: 0x06002645 RID: 9797 RVA: 0x000E9395 File Offset: 0x000E7595
		public static IEnumerable<DirectXmlLoaderSimple.XmlKeyValuePair> ValuesFromXmlFile(VirtualFile file)
		{
			return DirectXmlLoaderSimple.ValuesFromXmlFile(file.LoadAsXDocument());
		}

		// Token: 0x06002646 RID: 9798 RVA: 0x000E93A2 File Offset: 0x000E75A2
		public static IEnumerable<DirectXmlLoaderSimple.XmlKeyValuePair> ValuesFromXmlFile(XDocument doc)
		{
			DirectXmlLoaderSimple.<ValuesFromXmlFile>d__3 <ValuesFromXmlFile>d__ = new DirectXmlLoaderSimple.<ValuesFromXmlFile>d__3(-2);
			<ValuesFromXmlFile>d__.<>3__doc = doc;
			return <ValuesFromXmlFile>d__;
		}

		// Token: 0x0200275C RID: 10076
		public struct XmlKeyValuePair
		{
			// Token: 0x0400A115 RID: 41237
			public string key;

			// Token: 0x0400A116 RID: 41238
			public string value;

			// Token: 0x0400A117 RID: 41239
			public int lineNumber;
		}
	}
}
