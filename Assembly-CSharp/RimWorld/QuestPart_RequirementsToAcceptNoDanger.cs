using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010E6 RID: 4326
	public class QuestPart_RequirementsToAcceptNoDanger : QuestPart_RequirementsToAccept
	{
		// Token: 0x1700127B RID: 4731
		// (get) Token: 0x06006C1D RID: 27677 RVA: 0x0025F652 File Offset: 0x0025D852
		public Map TargetMap
		{
			get
			{
				if (this.mapParent != null && this.mapParent.HasMap)
				{
					return this.mapParent.Map;
				}
				if (this.mapPawn != null)
				{
					return this.mapPawn.MapHeld;
				}
				return null;
			}
		}

		// Token: 0x1700127C RID: 4732
		// (get) Token: 0x06006C1E RID: 27678 RVA: 0x0025F68A File Offset: 0x0025D88A
		public override IEnumerable<GlobalTargetInfo> Culprits
		{
			get
			{
				QuestPart_RequirementsToAcceptNoDanger.<get_Culprits>d__6 <get_Culprits>d__ = new QuestPart_RequirementsToAcceptNoDanger.<get_Culprits>d__6(-2);
				<get_Culprits>d__.<>4__this = this;
				return <get_Culprits>d__;
			}
		}

		// Token: 0x06006C1F RID: 27679 RVA: 0x0025F69A File Offset: 0x0025D89A
		public override AcceptanceReport CanAccept()
		{
			if (this.TargetMap != null && GenHostility.AnyHostileActiveThreatTo(this.TargetMap, this.dangerTo, false, false))
			{
				return new AcceptanceReport("QuestRequiresNoDangerOnMap".Translate());
			}
			return true;
		}

		// Token: 0x06006C20 RID: 27680 RVA: 0x0025F6D4 File Offset: 0x0025D8D4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_References.Look<Pawn>(ref this.mapPawn, "mapPawn", false);
			Scribe_References.Look<Faction>(ref this.dangerTo, "dangerTo", false);
		}

		// Token: 0x040044DA RID: 17626
		public MapParent mapParent;

		// Token: 0x040044DB RID: 17627
		public Pawn mapPawn;

		// Token: 0x040044DC RID: 17628
		public Faction dangerTo;
	}
}
