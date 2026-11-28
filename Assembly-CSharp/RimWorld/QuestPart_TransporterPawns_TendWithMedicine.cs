using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001108 RID: 4360
	public class QuestPart_TransporterPawns_TendWithMedicine : QuestPart_TransporterPawns_Tend
	{
		// Token: 0x06006CCB RID: 27851 RVA: 0x00262524 File Offset: 0x00260724
		protected override void DoTend(Pawn pawn)
		{
			Pawn doctor = null;
			if (this.allowSelfTend && pawn.playerSettings != null && pawn.playerSettings.selfTend && pawn.GetStatValue(StatDefOf.MedicalTendQuality, true, -1) > 0.75f)
			{
				doctor = pawn;
			}
			Medicine medicine = (Medicine)ThingMaker.MakeThing(this.medicineDef, null);
			TendUtility.DoTend(doctor, pawn, medicine);
		}

		// Token: 0x06006CCC RID: 27852 RVA: 0x00262580 File Offset: 0x00260780
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.medicineDef, "medicineDef");
			Scribe_Values.Look<bool>(ref this.allowSelfTend, "allowSelfTend", false, false);
		}

		// Token: 0x04004543 RID: 17731
		public ThingDef medicineDef;

		// Token: 0x04004544 RID: 17732
		public bool allowSelfTend;
	}
}
