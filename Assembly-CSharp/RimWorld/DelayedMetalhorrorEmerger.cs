using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AEC RID: 6892
	public class DelayedMetalhorrorEmerger : Thing
	{
		// Token: 0x0600B009 RID: 45065 RVA: 0x003E0919 File Offset: 0x003DEB19
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.emergeTick = GenTicks.TicksGame + this.emergeDelayTicks;
			}
		}

		// Token: 0x0600B00A RID: 45066 RVA: 0x003E0938 File Offset: 0x003DEB38
		protected override void Tick()
		{
			if (!base.Spawned)
			{
				return;
			}
			if (GenTicks.TicksGame >= this.emergeTick)
			{
				Pawn[] array = base.Map.mapPawns.AllHumanlike.ToArray();
				for (int i = 0; i < array.Length; i++)
				{
					MetalhorrorUtility.TryEmerge(array[i], null, false);
				}
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600B00B RID: 45067 RVA: 0x003E0990 File Offset: 0x003DEB90
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.emergeDelayTicks, "emergeDelayTicks", 0, false);
			Scribe_Values.Look<int>(ref this.emergeTick, "emergeTick", 0, false);
		}

		// Token: 0x0600B00C RID: 45068 RVA: 0x003E09BC File Offset: 0x003DEBBC
		public static DelayedMetalhorrorEmerger Spawn(Map map, int delayTicks)
		{
			DelayedMetalhorrorEmerger delayedMetalhorrorEmerger = (DelayedMetalhorrorEmerger)ThingMaker.MakeThing(ThingDefOf.DelayedMetalhorrorEmerger, null);
			delayedMetalhorrorEmerger.emergeDelayTicks = delayTicks;
			IntVec3 loc;
			if (!RCellFinder.TryFindRandomPawnEntryCell(out loc, map, 0f, true, null))
			{
				return null;
			}
			GenSpawn.Spawn(delayedMetalhorrorEmerger, loc, map, WipeMode.Vanish);
			return delayedMetalhorrorEmerger;
		}

		// Token: 0x040065A0 RID: 26016
		public int emergeDelayTicks;

		// Token: 0x040065A1 RID: 26017
		private int emergeTick;
	}
}
