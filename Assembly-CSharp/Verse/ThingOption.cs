using System;
using System.Xml;

namespace Verse
{
	// Token: 0x020006B5 RID: 1717
	public sealed class ThingOption
	{
		// Token: 0x0600369B RID: 13979 RVA: 0x0015E932 File Offset: 0x0015CB32
		public ThingOption()
		{
		}

		// Token: 0x0600369C RID: 13980 RVA: 0x0015E945 File Offset: 0x0015CB45
		public ThingOption(ThingDef thingDef, float weight)
		{
			this.thingDef = thingDef;
			this.weight = weight;
		}

		// Token: 0x0600369D RID: 13981 RVA: 0x0015E968 File Offset: 0x0015CB68
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured ThingOption: " + xmlRoot.OuterXml);
				return;
			}
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "thingDef", xmlRoot.Name, null, null, null);
			this.weight = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x0600369E RID: 13982 RVA: 0x0015E9C4 File Offset: 0x0015CBC4
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(",
				(this.thingDef != null) ? this.thingDef.defName : "null",
				", weight=",
				this.weight.ToString("0.##"),
				")"
			});
		}

		// Token: 0x04002343 RID: 9027
		public ThingDef thingDef;

		// Token: 0x04002344 RID: 9028
		public float weight = 1f;
	}
}
