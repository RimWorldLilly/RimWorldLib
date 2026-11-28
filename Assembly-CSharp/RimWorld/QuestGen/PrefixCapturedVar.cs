using System;
using System.Xml;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200218B RID: 8587
	public class PrefixCapturedVar
	{
		// Token: 0x0600D672 RID: 54898 RVA: 0x004FA8DC File Offset: 0x004F8ADC
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured PrefixCapturedVar: " + xmlRoot.OuterXml);
				return;
			}
			this.name = xmlRoot.Name;
			this.value = new SlateRef<object>(DirectXmlToObject.InnerTextWithReplacedNewlinesOrXML(xmlRoot));
			TKeySystem.MarkTreatAsList(xmlRoot.ParentNode);
		}

		// Token: 0x040087C4 RID: 34756
		[NoTranslate]
		[TranslationHandle]
		public string name;

		// Token: 0x040087C5 RID: 34757
		public SlateRef<object> value;
	}
}
