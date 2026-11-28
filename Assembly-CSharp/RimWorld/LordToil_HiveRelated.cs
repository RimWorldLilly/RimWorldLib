using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CE0 RID: 3296
	public abstract class LordToil_HiveRelated : LordToil
	{
		// Token: 0x17001042 RID: 4162
		// (get) Token: 0x06005D33 RID: 23859 RVA: 0x002230D3 File Offset: 0x002212D3
		private LordToil_HiveRelatedData Data
		{
			get
			{
				return (LordToil_HiveRelatedData)this.data;
			}
		}

		// Token: 0x06005D34 RID: 23860 RVA: 0x002230E0 File Offset: 0x002212E0
		public LordToil_HiveRelated()
		{
			this.data = new LordToil_HiveRelatedData();
		}

		// Token: 0x06005D35 RID: 23861 RVA: 0x002230F3 File Offset: 0x002212F3
		protected void FilterOutUnspawnedHives()
		{
			this.Data.assignedHives.RemoveAll((KeyValuePair<Pawn, Hive> x) => x.Value == null || !x.Value.Spawned);
		}

		// Token: 0x06005D36 RID: 23862 RVA: 0x00223128 File Offset: 0x00221328
		protected Hive GetHiveFor(Pawn pawn)
		{
			Hive hive;
			if (this.Data.assignedHives.TryGetValue(pawn, out hive))
			{
				return hive;
			}
			hive = this.FindClosestHive(pawn);
			if (hive != null)
			{
				this.Data.assignedHives.Add(pawn, hive);
			}
			return hive;
		}

		// Token: 0x06005D37 RID: 23863 RVA: 0x0022316C File Offset: 0x0022136C
		private Hive FindClosestHive(Pawn pawn)
		{
			if (!pawn.Spawned)
			{
				return null;
			}
			return (Hive)GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(ThingDefOf.Hive), PathEndMode.Touch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 30f, (Thing x) => x.Faction == pawn.Faction, null, 0, 30, false, RegionType.Set_Passable, false, false);
		}

		// Token: 0x06005D38 RID: 23864 RVA: 0x002231EC File Offset: 0x002213EC
		public override void Notify_PawnAcquiredTarget(Pawn detector, Thing newTarg)
		{
			CompCanBeDormant compCanBeDormant = detector.TryGetComp<CompCanBeDormant>();
			if (compCanBeDormant != null)
			{
				compCanBeDormant.WakeUp();
			}
		}
	}
}
