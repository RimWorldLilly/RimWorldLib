using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023C1 RID: 9153
	public class SitePart : IExposable, IThingHolderTickable, IThingHolder
	{
		// Token: 0x17002835 RID: 10293
		// (get) Token: 0x0600E44F RID: 58447 RVA: 0x0054D648 File Offset: 0x0054B848
		public IThingHolder ParentHolder
		{
			get
			{
				return this.site;
			}
		}

		// Token: 0x0600E450 RID: 58448 RVA: 0x0054D650 File Offset: 0x0054B850
		public SitePart()
		{
		}

		// Token: 0x0600E451 RID: 58449 RVA: 0x0054D666 File Offset: 0x0054B866
		public SitePart(Site site, SitePartDef def, SitePartParams parms)
		{
			this.site = site;
			this.def = def;
			this.parms = parms;
			this.hidden = def.defaultHidden;
		}

		// Token: 0x17002836 RID: 10294
		// (get) Token: 0x0600E452 RID: 58450 RVA: 0x0054D69D File Offset: 0x0054B89D
		public bool ShouldTickContents
		{
			get
			{
				return this.things != null && this.things.contentsLookMode == LookMode.Deep && !this.things.dontTickContents;
			}
		}

		// Token: 0x0600E453 RID: 58451 RVA: 0x0054D6C8 File Offset: 0x0054B8C8
		public void SitePartTickInterval(int delta)
		{
			if (this.things != null)
			{
				for (int i = 0; i < this.things.Count; i++)
				{
					Pawn pawn = this.things[i] as Pawn;
					if (pawn != null && !pawn.Destroyed && pawn.needs.food != null)
					{
						pawn.needs.food.CurLevelPercentage = 0.8f;
					}
				}
			}
		}

		// Token: 0x0600E454 RID: 58452 RVA: 0x0054D732 File Offset: 0x0054B932
		public void PostDestroy()
		{
			if (this.things != null)
			{
				this.things.ClearAndDestroyContentsOrPassToWorld(DestroyMode.Vanish);
			}
			if (!this.relicWasSpawned)
			{
				Thing thing = this.relicThing;
				if (thing != null)
				{
					thing.Destroy(DestroyMode.Vanish);
				}
				this.relicThing = null;
			}
		}

		// Token: 0x0600E455 RID: 58453 RVA: 0x0054D769 File Offset: 0x0054B969
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x0600E456 RID: 58454 RVA: 0x0054D777 File Offset: 0x0054B977
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.things;
		}

		// Token: 0x0600E457 RID: 58455 RVA: 0x0054D780 File Offset: 0x0054B980
		public void ExposeData()
		{
			Scribe_Deep.Look<SitePartParams>(ref this.parms, "parms", Array.Empty<object>());
			Scribe_Deep.Look<ThingOwner>(ref this.things, "things", new object[]
			{
				this
			});
			Scribe_Collections.Look<ThingDefCount>(ref this.lootThings, "lootThings", LookMode.Deep, Array.Empty<object>());
			Scribe_Defs.Look<SitePartDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.lastRaidTick, "lastRaidTick", -1, false);
			Scribe_Values.Look<bool>(ref this.conditionCauserWasSpawned, "conditionCauserWasSpawned", false, false);
			Scribe_Values.Look<bool>(ref this.hidden, "hidden", false, false);
			Scribe_Values.Look<int>(ref this.expectedEnemyCount, "expectedEnemyCount", -1, false);
			if (this.conditionCauserWasSpawned)
			{
				Scribe_References.Look<Thing>(ref this.conditionCauser, "conditionCauser", false);
			}
			else
			{
				Scribe_Deep.Look<Thing>(ref this.conditionCauser, "conditionCauser", Array.Empty<object>());
			}
			Scribe_Values.Look<bool>(ref this.relicWasSpawned, "relicWasSpawned", false, false);
			if (this.relicWasSpawned)
			{
				Scribe_References.Look<Thing>(ref this.relicThing, "relicThing", false);
				return;
			}
			Scribe_Deep.Look<Thing>(ref this.relicThing, "relicThing", Array.Empty<object>());
		}

		// Token: 0x04009138 RID: 37176
		public Site site;

		// Token: 0x04009139 RID: 37177
		public SitePartDef def;

		// Token: 0x0400913A RID: 37178
		public bool hidden;

		// Token: 0x0400913B RID: 37179
		public SitePartParams parms;

		// Token: 0x0400913C RID: 37180
		public ThingOwner things;

		// Token: 0x0400913D RID: 37181
		public int lastRaidTick = -1;

		// Token: 0x0400913E RID: 37182
		public Thing conditionCauser;

		// Token: 0x0400913F RID: 37183
		public bool conditionCauserWasSpawned;

		// Token: 0x04009140 RID: 37184
		public List<ThingDefCount> lootThings;

		// Token: 0x04009141 RID: 37185
		public int expectedEnemyCount = -1;

		// Token: 0x04009142 RID: 37186
		public Thing relicThing;

		// Token: 0x04009143 RID: 37187
		public bool relicWasSpawned;

		// Token: 0x04009144 RID: 37188
		private const float AutoFoodLevel = 0.8f;
	}
}
