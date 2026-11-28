using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001745 RID: 5957
	public class CompRitualEffect_Drum : CompRitualEffect_IntervalSpawn
	{
		// Token: 0x06009448 RID: 37960 RVA: 0x00360B30 File Offset: 0x0035ED30
		protected override Vector3 SpawnPos(LordJob_Ritual ritual)
		{
			return Vector3.zero;
		}

		// Token: 0x1700195F RID: 6495
		// (get) Token: 0x06009449 RID: 37961 RVA: 0x00360B37 File Offset: 0x0035ED37
		protected new CompProperties_RitualEffectDrum Props
		{
			get
			{
				return (CompProperties_RitualEffectDrum)this.props;
			}
		}

		// Token: 0x0600944A RID: 37962 RVA: 0x00360B44 File Offset: 0x0035ED44
		public override void SpawnFleck(LordJob_Ritual ritual, Vector3? forcedPos = null, float? exactRotation = null)
		{
			foreach (Thing thing in ritual.Map.listerBuldingOfDefInProximity.GetForCell(ritual.selectedTarget.Cell, (float)this.Props.maxDistance, ThingDefOf.Drum, null))
			{
				Building_MusicalInstrument building_MusicalInstrument = thing as Building_MusicalInstrument;
				if (building_MusicalInstrument != null && thing.GetRoom(RegionType.Set_All) == ritual.selectedTarget.Cell.GetRoom(ritual.Map) && building_MusicalInstrument.IsBeingPlayed)
				{
					for (int i = 0; i < this.Props.spawnCount; i++)
					{
						float num = (float)Rand.Sign;
						float num2 = (float)Rand.Sign;
						Vector3 b = new Vector3(num * Rand.Value * this.Props.maxOffset, 0f, num2 * Rand.Value * this.Props.maxOffset);
						base.SpawnFleck(ritual, new Vector3?(thing.Position.ToVector3Shifted() + b), null);
					}
				}
			}
			this.burstsDone++;
			this.lastSpawnTick = GenTicks.TicksGame;
		}
	}
}
