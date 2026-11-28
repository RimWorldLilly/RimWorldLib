using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x0200010C RID: 268
	public class DefHyperlink
	{
		// Token: 0x06000948 RID: 2376 RVA: 0x00035612 File Offset: 0x00033812
		public DefHyperlink()
		{
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x0003561A File Offset: 0x0003381A
		public DefHyperlink(Def def)
		{
			this.def = def;
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x00035629 File Offset: 0x00033829
		public DefHyperlink(RoyalTitleDef def, Faction faction)
		{
			this.def = def;
			this.faction = faction;
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x00035640 File Offset: 0x00033840
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured DefHyperlink: " + xmlRoot.OuterXml);
				return;
			}
			Type typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(xmlRoot.Name, null);
			if (typeInAnyAssembly == null)
			{
				Log.Error("Misconfigured DefHyperlink. Could not find def of type " + xmlRoot.Name);
				return;
			}
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "def", xmlRoot.FirstChild.Value, null, null, typeInAnyAssembly);
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x000356B6 File Offset: 0x000338B6
		public static implicit operator DefHyperlink(Def def)
		{
			return new DefHyperlink
			{
				def = def
			};
		}

		// Token: 0x0400044C RID: 1100
		public Def def;

		// Token: 0x0400044D RID: 1101
		public Faction faction;
	}
}
