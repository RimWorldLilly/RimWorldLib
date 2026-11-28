using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x0200175E RID: 5982
	public class MutatorChance
	{
		// Token: 0x060094D5 RID: 38101 RVA: 0x00363910 File Offset: 0x00361B10
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "mutator", xmlRoot.Name, null, null, null);
			XmlAttribute xmlAttribute = xmlRoot.Attributes["Required"];
			this.required = (xmlAttribute != null && xmlAttribute.Value.Equals("true", StringComparison.InvariantCultureIgnoreCase));
			if (xmlRoot.HasChildNodes)
			{
				this.chance = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
				return;
			}
			this.chance = 1f;
		}

		// Token: 0x040059AD RID: 22957
		public TileMutatorDef mutator;

		// Token: 0x040059AE RID: 22958
		public float chance = 1f;

		// Token: 0x040059AF RID: 22959
		public bool required;
	}
}
