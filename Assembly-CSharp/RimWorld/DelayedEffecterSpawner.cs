using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001AEB RID: 6891
	public class DelayedEffecterSpawner : Thing
	{
		// Token: 0x0600B004 RID: 45060 RVA: 0x003E071E File Offset: 0x003DE91E
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.spawnTick = GenTicks.TicksGame + this.emergeDelayTicks;
				if (this.preEmergeEffecter != null)
				{
					this.spawnPreEffectTick = this.spawnTick - this.preEmergeEffecter.maintainTicks;
				}
			}
		}

		// Token: 0x0600B005 RID: 45061 RVA: 0x003E0760 File Offset: 0x003DE960
		protected override void Tick()
		{
			if (!base.Spawned)
			{
				return;
			}
			if (GenTicks.TicksGame >= this.spawnPreEffectTick && !this.spawnedMistEffect)
			{
				this.spawnedMistEffect = true;
				EffecterDef effecterDef = this.preEmergeEffecter;
				if (effecterDef != null)
				{
					effecterDef.Spawn(base.Position, base.Map, 1f).Cleanup();
				}
			}
			if (GenTicks.TicksGame >= this.spawnTick)
			{
				Map map = base.Map;
				IntVec3 position = base.Position;
				this.Destroy(DestroyMode.Vanish);
				GenSpawn.Spawn(this.pawnToSpawn, position, map, WipeMode.Vanish);
				EffecterDef effecterDef2 = this.emergeEffecter;
				if (effecterDef2 != null)
				{
					effecterDef2.Spawn(position, map, 1f).Cleanup();
				}
				SoundDef soundDef = this.emergeSound;
				if (soundDef == null)
				{
					return;
				}
				soundDef.PlayOneShot(this.pawnToSpawn);
			}
		}

		// Token: 0x0600B006 RID: 45062 RVA: 0x003E0824 File Offset: 0x003DEA24
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.emergeDelayTicks, "emergeDelayTicks", 0, false);
			Scribe_Values.Look<int>(ref this.spawnPreEffectTick, "spawnPreEffectTick", 0, false);
			Scribe_Values.Look<bool>(ref this.spawnedMistEffect, "spawnedMistEffect", false, false);
			Scribe_Values.Look<int>(ref this.spawnTick, "spawnTick", 0, false);
			Scribe_Defs.Look<EffecterDef>(ref this.emergeEffecter, "emergeEffecter");
			Scribe_Defs.Look<SoundDef>(ref this.emergeSound, "emergeSound");
			Scribe_Defs.Look<EffecterDef>(ref this.preEmergeEffecter, "preEmergeEffecter");
			Scribe_Deep.Look<Pawn>(ref this.pawnToSpawn, "pawnToSpawn", Array.Empty<object>());
		}

		// Token: 0x0600B007 RID: 45063 RVA: 0x003E08C4 File Offset: 0x003DEAC4
		public static DelayedEffecterSpawner Spawn(Pawn pawn, IntVec3 pos, Map map, int delayTicks, EffecterDef emergeEffect = null, EffecterDef preEmergeEffect = null, SoundDef emergeSound = null)
		{
			DelayedEffecterSpawner delayedEffecterSpawner = (DelayedEffecterSpawner)ThingMaker.MakeThing(ThingDefOf.DelayedEffecterSpawner, null);
			delayedEffecterSpawner.pawnToSpawn = pawn;
			delayedEffecterSpawner.emergeDelayTicks = delayTicks;
			delayedEffecterSpawner.preEmergeEffecter = preEmergeEffect;
			delayedEffecterSpawner.emergeEffecter = emergeEffect;
			delayedEffecterSpawner.emergeSound = emergeSound;
			GenSpawn.Spawn(delayedEffecterSpawner, pos, map, WipeMode.Vanish);
			return delayedEffecterSpawner;
		}

		// Token: 0x04006598 RID: 26008
		public Pawn pawnToSpawn;

		// Token: 0x04006599 RID: 26009
		public int emergeDelayTicks;

		// Token: 0x0400659A RID: 26010
		public EffecterDef emergeEffecter;

		// Token: 0x0400659B RID: 26011
		public SoundDef emergeSound;

		// Token: 0x0400659C RID: 26012
		public EffecterDef preEmergeEffecter;

		// Token: 0x0400659D RID: 26013
		private int spawnTick;

		// Token: 0x0400659E RID: 26014
		private int spawnPreEffectTick;

		// Token: 0x0400659F RID: 26015
		private bool spawnedMistEffect;
	}
}
