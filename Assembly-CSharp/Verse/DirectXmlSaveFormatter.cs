using System;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Verse
{
	// Token: 0x0200050D RID: 1293
	public static class DirectXmlSaveFormatter
	{
		// Token: 0x06002647 RID: 9799 RVA: 0x000E93B4 File Offset: 0x000E75B4
		public static void AddWhitespaceFromRoot(XElement root)
		{
			if (!root.Elements().Any<XElement>())
			{
				return;
			}
			foreach (XNode xnode in root.Elements().ToList<XElement>())
			{
				XText content = new XText("\n");
				xnode.AddAfterSelf(content);
			}
			root.Elements().First<XElement>().AddBeforeSelf(new XText("\n"));
			root.Elements().Last<XElement>().AddAfterSelf(new XText("\n"));
			foreach (XElement element in root.Elements().ToList<XElement>())
			{
				DirectXmlSaveFormatter.IndentXml(element, 1);
			}
		}

		// Token: 0x06002648 RID: 9800 RVA: 0x000E949C File Offset: 0x000E769C
		private static void IndentXml(XElement element, int depth)
		{
			element.AddBeforeSelf(new XText(DirectXmlSaveFormatter.IndentString(depth, true)));
			bool startWithNewline = element.NextNode == null;
			element.AddAfterSelf(new XText(DirectXmlSaveFormatter.IndentString(depth - 1, startWithNewline)));
			foreach (XElement element2 in element.Elements().ToList<XElement>())
			{
				DirectXmlSaveFormatter.IndentXml(element2, depth + 1);
			}
		}

		// Token: 0x06002649 RID: 9801 RVA: 0x000E9524 File Offset: 0x000E7724
		private static string IndentString(int depth, bool startWithNewline)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (startWithNewline)
			{
				stringBuilder.Append("\n");
			}
			for (int i = 0; i < depth; i++)
			{
				stringBuilder.Append("  ");
			}
			return stringBuilder.ToString();
		}
	}
}
