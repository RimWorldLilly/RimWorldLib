using System;
using System.Collections.Generic;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001B4C RID: 6988
	public class ActiveTransporter : Thing, IActiveTransporter, IThingHolder
	{
		// Token: 0x17002072 RID: 8306
		// (get) Token: 0x0600B35F RID: 45919 RVA: 0x003F37C7 File Offset: 0x003F19C7
		// (set) Token: 0x0600B360 RID: 45920 RVA: 0x003F37CF File Offset: 0x003F19CF
		public ActiveTransporterInfo Contents
		{
			get
			{
				return this.contents;
			}
			set
			{
				if (this.contents != null)
				{
					this.contents.parent = null;
				}
				if (value != null)
				{
					value.parent = this;
				}
				this.contents = value;
			}
		}

		// Token: 0x0600B361 RID: 45921 RVA: 0x003F37F6 File Offset: 0x003F19F6
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
			Scribe_Deep.Look<ActiveTransporterInfo>(ref this.contents, "contents", new object[]
			{
				this
			});
		}

		// Token: 0x0600B362 RID: 45922 RVA: 0x003F382A File Offset: 0x003F1A2A
		public ThingOwner GetDirectlyHeldThings()
		{
			return null;
		}

		// Token: 0x0600B363 RID: 45923 RVA: 0x003F382D File Offset: 0x003F1A2D
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
			if (this.contents != null)
			{
				outChildren.Add(this.contents);
			}
		}

		// Token: 0x0600B364 RID: 45924 RVA: 0x003F384F File Offset: 0x003F1A4F
		protected override void Tick()
		{
			if (this.contents == null)
			{
				return;
			}
			if (!base.Spawned)
			{
				return;
			}
			this.age++;
			if (this.age > this.contents.openDelay)
			{
				this.PodOpen();
			}
		}

		// Token: 0x0600B365 RID: 45925 RVA: 0x003F388C File Offset: 0x003F1A8C
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (this.contents != null)
			{
				this.contents.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
			}
			Map map = base.Map;
			base.Destroy(mode);
			if (mode == DestroyMode.KillFinalize)
			{
				for (int i = 0; i < 1; i++)
				{
					GenPlace.TryPlaceThing(ThingMaker.MakeThing(ThingDefOf.ChunkSlagSteel, null), base.Position, map, ThingPlaceMode.Near, null, null, null, 1);
				}
			}
		}

		// Token: 0x0600B366 RID: 45926 RVA: 0x003F38F8 File Offset: 0x003F1AF8
		private void PodOpen()
		{
			Map map = base.Map;
			if (this.contents.despawnPodBeforeSpawningThing)
			{
				this.DeSpawn(DestroyMode.Vanish);
			}
			for (int i = this.contents.innerContainer.Count - 1; i >= 0; i--)
			{
				Thing thing = this.contents.innerContainer[i];
				Rot4 rot = this.contents.setRotation ?? Rot4.North;
				if (this.contents.moveItemsAsideBeforeSpawning)
				{
					GenSpawn.CheckMoveItemsAside(base.Position, rot, thing.def, map);
				}
				Thing thing2;
				if (this.contents.spawnWipeMode == null)
				{
					GenPlace.TryPlaceThing(thing, base.Position, map, ThingPlaceMode.Near, out thing2, delegate(Thing placedThing, int count)
					{
						if (Find.TickManager.TicksGame < 1200 && TutorSystem.TutorialMode && placedThing.def.category == ThingCategory.Item)
						{
							Find.TutorialState.AddStartingItem(placedThing);
						}
					}, null, new Rot4?(rot), 1);
				}
				else if (this.contents.setRotation != null)
				{
					thing2 = GenSpawn.Spawn(thing, base.Position, map, this.contents.setRotation.Value, this.contents.spawnWipeMode.Value, false, false);
				}
				else
				{
					thing2 = GenSpawn.Spawn(thing, base.Position, map, this.contents.spawnWipeMode.Value);
				}
				Pawn pawn = thing2 as Pawn;
				if (pawn != null)
				{
					if (pawn.RaceProps.Humanlike)
					{
						TaleRecorder.RecordTale(TaleDefOf.LandedInPod, new object[]
						{
							pawn
						});
					}
					if (pawn.IsColonist && pawn.Spawned && !map.IsPlayerHome)
					{
						pawn.drafter.Drafted = true;
					}
					if (pawn.guest != null && pawn.guest.IsPrisoner)
					{
						pawn.guest.WaitInsteadOfEscapingForDefaultTicks();
					}
				}
			}
			this.contents.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
			if (this.contents.leaveSlag)
			{
				for (int j = 0; j < 1; j++)
				{
					GenPlace.TryPlaceThing(ThingMaker.MakeThing(ThingDefOf.ChunkSlagSteel, null), base.Position, map, ThingPlaceMode.Near, null, null, null, 1);
				}
			}
			if (this.def.soundOpen != null)
			{
				this.def.soundOpen.PlayOneShot(new TargetInfo(base.Position, map, false));
			}
			this.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x04006779 RID: 26489
		public int age;

		// Token: 0x0400677A RID: 26490
		private ActiveTransporterInfo contents;
	}
}
