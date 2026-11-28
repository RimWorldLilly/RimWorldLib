using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020004A0 RID: 1184
	public class Hediff_AddedPart : Hediff_Implant
	{
		// Token: 0x060020D0 RID: 8400 RVA: 0x000C4278 File Offset: 0x000C2478
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			this.pawn.health.RestorePart(base.Part, this, false);
			for (int i = 0; i < base.Part.parts.Count; i++)
			{
				Hediff_MissingPart hediff_MissingPart = (Hediff_MissingPart)HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, this.pawn, null);
				hediff_MissingPart.IsFresh = true;
				hediff_MissingPart.lastInjury = HediffDefOf.SurgicalCut;
				hediff_MissingPart.Part = base.Part.parts[i];
				this.pawn.health.hediffSet.AddDirect(hediff_MissingPart, null, null);
			}
		}
	}
}
