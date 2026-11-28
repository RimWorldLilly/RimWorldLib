using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200235E RID: 9054
	public class DownedRefugeeComp : ImportantPawnComp, IThingHolder
	{
		// Token: 0x1700276E RID: 10094
		// (get) Token: 0x0600E06A RID: 57450 RVA: 0x005376B6 File Offset: 0x005358B6
		protected override string PawnSaveKey
		{
			get
			{
				return "refugee";
			}
		}

		// Token: 0x0600E06B RID: 57451 RVA: 0x005376C0 File Offset: 0x005358C0
		protected override void RemovePawnOnWorldObjectRemoved()
		{
			if (this.pawn.Any)
			{
				if (!this.pawn[0].Dead)
				{
					if (this.pawn[0].relations != null)
					{
						this.pawn[0].relations.Notify_FailedRescueQuest();
					}
					HealthUtility.HealNonPermanentInjuriesAndRestoreLegs(this.pawn[0]);
				}
				this.pawn.ClearAndDestroyContentsOrPassToWorld(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600E06C RID: 57452 RVA: 0x00537733 File Offset: 0x00535933
		public override string CompInspectStringExtra()
		{
			if (this.pawn.Any)
			{
				return "Refugee".Translate() + ": " + this.pawn[0].LabelCap;
			}
			return null;
		}
	}
}
