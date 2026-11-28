using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001905 RID: 6405
	public class FocusStrengthOffset_NearbyGraves : FocusStrengthOffset_BuildingDefs
	{
		// Token: 0x0600A091 RID: 41105 RVA: 0x003971AC File Offset: 0x003953AC
		protected override float OffsetForBuilding(Thing b)
		{
			float num = base.OffsetFor(b.def);
			Building_Grave building_Grave = b as Building_Grave;
			if (building_Grave != null && building_Grave.HasCorpse && building_Grave.Corpse.InnerPawn.RaceProps.Humanlike)
			{
				num += this.focusPerFullGrave;
			}
			return num;
		}

		// Token: 0x0600A092 RID: 41106 RVA: 0x003971F9 File Offset: 0x003953F9
		public override float MaxOffset(Thing parent = null)
		{
			return (float)this.maxBuildings * (this.focusPerFullGrave + base.MaxOffsetPerBuilding);
		}

		// Token: 0x0600A093 RID: 41107 RVA: 0x00397210 File Offset: 0x00395410
		public override string GetExplanation(Thing parent)
		{
			if (!parent.Spawned)
			{
				return this.GetExplanationAbstract(parent.def);
			}
			int value = this.BuildingCount(parent);
			return this.explanationKey.Translate(value, this.maxBuildings, base.MaxOffsetPerBuilding.ToString("0%"), (base.MaxOffsetPerBuilding + this.focusPerFullGrave).ToString("0%")) + ": " + this.GetOffset(parent, null).ToStringWithSign("0%");
		}

		// Token: 0x0600A094 RID: 41108 RVA: 0x003972B4 File Offset: 0x003954B4
		public override string GetExplanationAbstract(ThingDef def = null)
		{
			return this.explanationKeyAbstract.Translate(this.maxBuildings, base.MaxOffsetPerBuilding.ToString("0%"), (base.MaxOffsetPerBuilding + this.focusPerFullGrave).ToString("0%")) + ": +0-" + this.MaxOffset(null).ToString("0%");
		}

		// Token: 0x04005ED5 RID: 24277
		public float focusPerFullGrave;
	}
}
