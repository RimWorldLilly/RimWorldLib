using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001739 RID: 5945
	public abstract class CompRitualEffect_IntervalSpawnBurst : CompRitualEffect_IntervalSpawn
	{
		// Token: 0x0600942C RID: 37932 RVA: 0x003603FC File Offset: 0x0035E5FC
		public override Mote SpawnMote(LordJob_Ritual ritual, Vector3? forcedPos = null)
		{
			for (int i = 0; i < base.Props.spawnCount; i++)
			{
				base.SpawnMote(ritual, null);
			}
			this.lastSpawnTick = GenTicks.TicksGame;
			this.burstsDone++;
			return null;
		}

		// Token: 0x0600942D RID: 37933 RVA: 0x0036044C File Offset: 0x0035E64C
		public override void SpawnFleck(LordJob_Ritual ritual, Vector3? forcedPos = null, float? exactRotation = null)
		{
			for (int i = 0; i < base.Props.spawnCount; i++)
			{
				base.SpawnFleck(ritual, null, null);
			}
			this.lastSpawnTick = GenTicks.TicksGame;
			this.burstsDone++;
		}
	}
}
