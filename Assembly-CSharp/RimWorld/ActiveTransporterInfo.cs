using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B4F RID: 6991
	public class ActiveTransporterInfo : IThingHolder, IExposable
	{
		// Token: 0x17002075 RID: 8309
		// (get) Token: 0x0600B371 RID: 45937 RVA: 0x003F3E20 File Offset: 0x003F2020
		// (set) Token: 0x0600B372 RID: 45938 RVA: 0x003F3E55 File Offset: 0x003F2055
		public Thing SingleContainedThing
		{
			get
			{
				if (this.innerContainer.Count == 0)
				{
					return null;
				}
				if (this.innerContainer.Count > 1)
				{
					Log.Error("ContainedThing used on a DropPodInfo holding > 1 thing.");
				}
				return this.innerContainer[0];
			}
			set
			{
				this.innerContainer.Clear();
				this.innerContainer.TryAdd(value, true);
			}
		}

		// Token: 0x17002076 RID: 8310
		// (get) Token: 0x0600B373 RID: 45939 RVA: 0x003F3E70 File Offset: 0x003F2070
		public IThingHolder ParentHolder
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x0600B374 RID: 45940 RVA: 0x003F3E78 File Offset: 0x003F2078
		public ActiveTransporterInfo()
		{
			this.innerContainer = new ThingOwner<Thing>(this);
		}

		// Token: 0x0600B375 RID: 45941 RVA: 0x003F3EAA File Offset: 0x003F20AA
		public ActiveTransporterInfo(IThingHolder parent)
		{
			this.innerContainer = new ThingOwner<Thing>(this);
			this.parent = parent;
		}

		// Token: 0x0600B376 RID: 45942 RVA: 0x003F3EE4 File Offset: 0x003F20E4
		public void ExposeData()
		{
			if (this.savePawnsWithReferenceMode && Scribe.mode == LoadSaveMode.Saving)
			{
				this.tmpThings.Clear();
				this.tmpThings.AddRange(this.innerContainer);
				this.tmpSavedPawns.Clear();
				for (int i = 0; i < this.tmpThings.Count; i++)
				{
					Pawn pawn = this.tmpThings[i] as Pawn;
					if (pawn != null)
					{
						this.innerContainer.Remove(pawn);
						this.tmpSavedPawns.Add(pawn);
					}
				}
				this.tmpThings.Clear();
			}
			Scribe_Values.Look<bool>(ref this.savePawnsWithReferenceMode, "savePawnsWithReferenceMode", false, false);
			if (this.savePawnsWithReferenceMode)
			{
				Scribe_Collections.Look<Pawn>(ref this.tmpSavedPawns, "tmpSavedPawns", LookMode.Reference, Array.Empty<object>());
			}
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Values.Look<int>(ref this.openDelay, "openDelay", 110, false);
			Scribe_Values.Look<bool>(ref this.leaveSlag, "leaveSlag", false, false);
			Scribe_Values.Look<WipeMode?>(ref this.spawnWipeMode, "spawnWipeMode", null, false);
			Scribe_Values.Look<bool>(ref this.despawnPodBeforeSpawningThing, "despawnPodBeforeSpawningThing", false, false);
			Scribe_Values.Look<Rot4?>(ref this.setRotation, "setRotation", null, false);
			Scribe_Values.Look<bool>(ref this.moveItemsAsideBeforeSpawning, "moveItemsAsideBeforeSpawning", false, false);
			Scribe_Collections.Look<string>(ref this.questTags, "questTags", LookMode.Value, Array.Empty<object>());
			Scribe_Defs.Look<ThingDef>(ref this.sentTransporterDef, "sentPodDef");
			Scribe_References.Look<Thing>(ref this.shuttle, "transportShip", false);
			if (this.savePawnsWithReferenceMode && (Scribe.mode == LoadSaveMode.PostLoadInit || Scribe.mode == LoadSaveMode.Saving))
			{
				for (int j = 0; j < this.tmpSavedPawns.Count; j++)
				{
					this.innerContainer.TryAdd(this.tmpSavedPawns[j], true);
				}
				this.tmpSavedPawns.Clear();
			}
		}

		// Token: 0x0600B377 RID: 45943 RVA: 0x003F40CC File Offset: 0x003F22CC
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x0600B378 RID: 45944 RVA: 0x003F40D4 File Offset: 0x003F22D4
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x0600B379 RID: 45945 RVA: 0x003F40E2 File Offset: 0x003F22E2
		public void SetShuttle(Thing shuttle)
		{
			if (shuttle.Spawned)
			{
				shuttle.DeSpawn(DestroyMode.Vanish);
			}
			this.innerContainer.TryAddOrTransfer(shuttle, true);
			this.shuttle = shuttle;
		}

		// Token: 0x0600B37A RID: 45946 RVA: 0x003F4108 File Offset: 0x003F2308
		public Thing GetShuttle()
		{
			return this.shuttle;
		}

		// Token: 0x0600B37B RID: 45947 RVA: 0x003F4110 File Offset: 0x003F2310
		public Thing RemoveShuttle()
		{
			this.innerContainer.Remove(this.shuttle);
			return this.shuttle;
		}

		// Token: 0x0400677B RID: 26491
		public IThingHolder parent;

		// Token: 0x0400677C RID: 26492
		public ThingOwner innerContainer;

		// Token: 0x0400677D RID: 26493
		public int openDelay = 110;

		// Token: 0x0400677E RID: 26494
		public bool leaveSlag;

		// Token: 0x0400677F RID: 26495
		public bool savePawnsWithReferenceMode;

		// Token: 0x04006780 RID: 26496
		public bool despawnPodBeforeSpawningThing;

		// Token: 0x04006781 RID: 26497
		public WipeMode? spawnWipeMode;

		// Token: 0x04006782 RID: 26498
		public Rot4? setRotation;

		// Token: 0x04006783 RID: 26499
		public bool moveItemsAsideBeforeSpawning;

		// Token: 0x04006784 RID: 26500
		public ThingDef sentTransporterDef;

		// Token: 0x04006785 RID: 26501
		private Thing shuttle;

		// Token: 0x04006786 RID: 26502
		public List<string> questTags;

		// Token: 0x04006787 RID: 26503
		public const int DefaultOpenDelay = 110;

		// Token: 0x04006788 RID: 26504
		private List<Thing> tmpThings = new List<Thing>();

		// Token: 0x04006789 RID: 26505
		private List<Pawn> tmpSavedPawns = new List<Pawn>();
	}
}
