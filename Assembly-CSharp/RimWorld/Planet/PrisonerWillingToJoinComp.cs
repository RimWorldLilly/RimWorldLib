using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002365 RID: 9061
	public class PrisonerWillingToJoinComp : ImportantPawnComp, IThingHolder
	{
		// Token: 0x1700277D RID: 10109
		// (get) Token: 0x0600E09E RID: 57502 RVA: 0x00537E32 File Offset: 0x00536032
		protected override string PawnSaveKey
		{
			get
			{
				return "prisoner";
			}
		}

		// Token: 0x0600E09F RID: 57503 RVA: 0x00537E39 File Offset: 0x00536039
		protected override void RemovePawnOnWorldObjectRemoved()
		{
			this.pawn.ClearAndDestroyContentsOrPassToWorld(DestroyMode.Vanish);
		}

		// Token: 0x0600E0A0 RID: 57504 RVA: 0x00537E47 File Offset: 0x00536047
		public override string CompInspectStringExtra()
		{
			if (this.pawn.Any)
			{
				return "Prisoner".Translate() + ": " + this.pawn[0].LabelCap;
			}
			return null;
		}
	}
}
