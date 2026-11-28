using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F30 RID: 3888
	public class PossessionThingDefCountClass
	{
		// Token: 0x06006477 RID: 25719 RVA: 0x0023E2EF File Offset: 0x0023C4EF
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "key", xmlRoot.Name, null, null, null);
			if (xmlRoot.HasChildNodes)
			{
				this.value = ParseHelper.FromString<IntRange>(xmlRoot.FirstChild.Value);
				return;
			}
			this.value = IntRange.One;
		}

		// Token: 0x040039AF RID: 14767
		public ThingDef key;

		// Token: 0x040039B0 RID: 14768
		public IntRange value = IntRange.One;
	}
}
