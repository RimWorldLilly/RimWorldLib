using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F16 RID: 3862
	public class StatModifier
	{
		// Token: 0x17001110 RID: 4368
		// (get) Token: 0x0600642C RID: 25644 RVA: 0x0023D684 File Offset: 0x0023B884
		public string ValueToStringAsOffset
		{
			get
			{
				return this.stat.Worker.ValueToString(this.value, false, ToStringNumberSense.Offset);
			}
		}

		// Token: 0x17001111 RID: 4369
		// (get) Token: 0x0600642D RID: 25645 RVA: 0x0023D69E File Offset: 0x0023B89E
		public string ToStringAsFactor
		{
			get
			{
				return this.stat.Worker.ValueToString(this.value, false, ToStringNumberSense.Factor);
			}
		}

		// Token: 0x0600642E RID: 25646 RVA: 0x0023D6B8 File Offset: 0x0023B8B8
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "stat", xmlRoot.Name, null, null, null);
			this.value = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x0600642F RID: 25647 RVA: 0x0023D6E4 File Offset: 0x0023B8E4
		public override string ToString()
		{
			if (this.stat == null)
			{
				return "(null stat)";
			}
			return this.stat.defName + "-" + this.value.ToString();
		}

		// Token: 0x0400393F RID: 14655
		public StatDef stat;

		// Token: 0x04003940 RID: 14656
		public float value;
	}
}
