using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200049A RID: 1178
	public class HediffComp_SurgeryInspectableMetalhorror : HediffComp_SurgeryInspectable
	{
		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x06002092 RID: 8338 RVA: 0x000C32BD File Offset: 0x000C14BD
		public new HediffCompProperties_SurgeryInspectableMetalHorror Props
		{
			get
			{
				return (HediffCompProperties_SurgeryInspectableMetalHorror)this.props;
			}
		}

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x06002093 RID: 8339 RVA: 0x000C32CA File Offset: 0x000C14CA
		public Hediff_MetalhorrorImplant Implant
		{
			get
			{
				return (Hediff_MetalhorrorImplant)this.parent;
			}
		}

		// Token: 0x06002094 RID: 8340 RVA: 0x000C32D8 File Offset: 0x000C14D8
		public override SurgicalInspectionOutcome DoSurgicalInspection(Pawn surgeon)
		{
			AnalysisDetails analysisDetails;
			if (!Find.AnalysisManager.TryGetAnalysisProgress(this.Implant.Biosignature, out analysisDetails) || !analysisDetails.Satisfied)
			{
				return SurgicalInspectionOutcome.Nothing;
			}
			Hediff_MetalhorrorImplant hediff_MetalhorrorImplant;
			if (MetalhorrorUtility.IsInfected(surgeon, out hediff_MetalhorrorImplant))
			{
				hediff_MetalhorrorImplant.LiedAboutInspection = true;
				return SurgicalInspectionOutcome.Nothing;
			}
			this.Emerge(surgeon);
			return SurgicalInspectionOutcome.DetectedNoLetter;
		}

		// Token: 0x06002095 RID: 8341 RVA: 0x000C3323 File Offset: 0x000C1523
		public override void DoSurgicalInspectionVisible(Pawn surgeon)
		{
			this.Emerge(surgeon);
		}

		// Token: 0x06002096 RID: 8342 RVA: 0x000C332C File Offset: 0x000C152C
		private void Emerge(Pawn surgeon)
		{
			this.Implant.Emerge("MetalhorrorReasonSurgeryInspected".Translate(base.Pawn.Named("INFECTED"), surgeon.Named("PAWN")), false);
		}
	}
}
