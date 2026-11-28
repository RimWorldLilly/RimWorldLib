using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020004A2 RID: 1186
	public class Hediff_Hangover : HediffWithComps
	{
		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x060020D5 RID: 8405 RVA: 0x000C43D8 File Offset: 0x000C25D8
		public override bool Visible
		{
			get
			{
				return !this.pawn.health.hediffSet.HasHediff(HediffDefOf.AlcoholHigh, false) && base.Visible;
			}
		}
	}
}
