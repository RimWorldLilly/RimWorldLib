using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x0200019D RID: 413
	public class TraitRequirement
	{
		// Token: 0x06000B79 RID: 2937 RVA: 0x0003DB62 File Offset: 0x0003BD62
		public bool Matches(Trait trait)
		{
			return trait.def == this.def && (this.degree == null || trait.Degree == this.degree.Value);
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x0003DB98 File Offset: 0x0003BD98
		public bool HasTrait(Pawn p)
		{
			if (p.story == null)
			{
				return false;
			}
			if (this.degree == null)
			{
				return p.story.traits.HasTrait(this.def);
			}
			return p.story.traits.HasTrait(this.def, this.degree.Value);
		}

		// Token: 0x06000B7B RID: 2939 RVA: 0x0003DBF4 File Offset: 0x0003BDF4
		public Trait GetTrait(Pawn p)
		{
			if (p.story == null)
			{
				return null;
			}
			if (this.degree == null)
			{
				return p.story.traits.GetTrait(this.def);
			}
			return p.story.traits.GetTrait(this.def, this.degree.Value);
		}

		// Token: 0x06000B7C RID: 2940 RVA: 0x0003DC50 File Offset: 0x0003BE50
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			XmlHelper.ParseElements(this, xmlRoot, "def", "degree");
		}

		// Token: 0x04000A01 RID: 2561
		public TraitDef def;

		// Token: 0x04000A02 RID: 2562
		public int? degree;
	}
}
