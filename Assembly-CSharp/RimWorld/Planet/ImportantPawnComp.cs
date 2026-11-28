using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002363 RID: 9059
	public abstract class ImportantPawnComp : WorldObjectComp, ISuspendableThingHolder, IThingHolder, IThingHolderTickable
	{
		// Token: 0x1700277A RID: 10106
		// (get) Token: 0x0600E08F RID: 57487 RVA: 0x00537CDD File Offset: 0x00535EDD
		public bool IsContentsSuspended
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700277B RID: 10107
		// (get) Token: 0x0600E090 RID: 57488
		protected abstract string PawnSaveKey { get; }

		// Token: 0x1700277C RID: 10108
		// (get) Token: 0x0600E091 RID: 57489 RVA: 0x00537CE0 File Offset: 0x00535EE0
		public bool ShouldTickContents
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600E092 RID: 57490 RVA: 0x00537CE3 File Offset: 0x00535EE3
		public ImportantPawnComp()
		{
			this.pawn = new ThingOwner<Pawn>(this, true, LookMode.Deep, true);
		}

		// Token: 0x0600E093 RID: 57491 RVA: 0x00537CFA File Offset: 0x00535EFA
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Deep.Look<ThingOwner<Pawn>>(ref this.pawn, this.PawnSaveKey, new object[]
			{
				this
			});
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x0600E094 RID: 57492 RVA: 0x00537D23 File Offset: 0x00535F23
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x0600E095 RID: 57493 RVA: 0x00537D31 File Offset: 0x00535F31
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.pawn;
		}

		// Token: 0x0600E096 RID: 57494 RVA: 0x00537D3C File Offset: 0x00535F3C
		public override void CompTick()
		{
			bool any = this.pawn.Any;
			this.pawn.DoTick();
			if (any && !base.ParentHasMap)
			{
				if (!this.pawn.Any || this.pawn[0].Destroyed)
				{
					this.parent.Destroy();
					return;
				}
				Pawn pawn = this.pawn[0];
				if (pawn.needs.food != null)
				{
					pawn.needs.food.CurLevelPercentage = 0.8f;
				}
			}
		}

		// Token: 0x0600E097 RID: 57495 RVA: 0x00537DC4 File Offset: 0x00535FC4
		public override void PostDestroy()
		{
			base.PostDestroy();
			this.RemovePawnOnWorldObjectRemoved();
		}

		// Token: 0x0600E098 RID: 57496
		protected abstract void RemovePawnOnWorldObjectRemoved();

		// Token: 0x0400900A RID: 36874
		public ThingOwner<Pawn> pawn;

		// Token: 0x0400900B RID: 36875
		private const float AutoFoodLevel = 0.8f;
	}
}
