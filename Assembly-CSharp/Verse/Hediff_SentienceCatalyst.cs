using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020004C8 RID: 1224
	public class Hediff_SentienceCatalyst : Hediff
	{
		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x06002215 RID: 8725 RVA: 0x000CA400 File Offset: 0x000C8600
		public override string TipStringExtra
		{
			get
			{
				string text = base.TipStringExtra;
				TrainabilityDef trainability = TrainableUtility.GetTrainability(this.pawn);
				if (this.pawn.training == null || trainability == null || trainability == this.pawn.RaceProps.trainability)
				{
					return text;
				}
				text += "\n";
				return text + ("Trainability".Translate() + ": " + trainability.LabelCap);
			}
		}
	}
}
