using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020015B9 RID: 5561
	public class Pawn_OutfitTracker : IExposable
	{
		// Token: 0x170017C5 RID: 6085
		// (get) Token: 0x060089D7 RID: 35287 RVA: 0x0031E8A4 File Offset: 0x0031CAA4
		// (set) Token: 0x060089D8 RID: 35288 RVA: 0x0031E911 File Offset: 0x0031CB11
		public ApparelPolicy CurrentApparelPolicy
		{
			get
			{
				if (this.pawn.IsMutant && (this.pawn.mutant.Def.disableApparel || this.pawn.mutant.Def.disablePolicies))
				{
					return null;
				}
				if (this.curApparelPolicy == null)
				{
					this.curApparelPolicy = Current.Game.outfitDatabase.DefaultOutfit();
				}
				return this.curApparelPolicy;
			}
			set
			{
				if (this.curApparelPolicy == value)
				{
					return;
				}
				this.curApparelPolicy = value;
				if (this.pawn.mindState != null)
				{
					this.pawn.mindState.Notify_OutfitChanged();
				}
			}
		}

		// Token: 0x060089D9 RID: 35289 RVA: 0x0031E941 File Offset: 0x0031CB41
		public Pawn_OutfitTracker()
		{
		}

		// Token: 0x060089DA RID: 35290 RVA: 0x0031E954 File Offset: 0x0031CB54
		public Pawn_OutfitTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060089DB RID: 35291 RVA: 0x0031E96E File Offset: 0x0031CB6E
		public void ExposeData()
		{
			Scribe_References.Look<ApparelPolicy>(ref this.curApparelPolicy, "curOutfit", false);
			Scribe_Deep.Look<OutfitForcedHandler>(ref this.forcedHandler, "overrideHandler", Array.Empty<object>());
		}

		// Token: 0x040054DD RID: 21725
		public Pawn pawn;

		// Token: 0x040054DE RID: 21726
		private ApparelPolicy curApparelPolicy;

		// Token: 0x040054DF RID: 21727
		public OutfitForcedHandler forcedHandler = new OutfitForcedHandler();
	}
}
