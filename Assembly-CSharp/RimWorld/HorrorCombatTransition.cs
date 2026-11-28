using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020017C8 RID: 6088
	public class HorrorCombatTransition : MusicTransition
	{
		// Token: 0x06009805 RID: 38917 RVA: 0x0037202C File Offset: 0x0037022C
		public override bool IsTransitionSatisfied()
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			if (!base.IsTransitionSatisfied())
			{
				return false;
			}
			foreach (Map map in Find.Maps)
			{
				if (this.IsValidMap(map))
				{
					foreach (Pawn pawn in map.mapPawns.AllPawnsSpawned)
					{
						if (this.IsValidPawn(pawn))
						{
							return true;
						}
						if (this.IsValidEntity(pawn))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06009806 RID: 38918 RVA: 0x003720F0 File Offset: 0x003702F0
		private bool IsValidMap(Map map)
		{
			return map.mapPawns.ColonistsSpawnedCount > 0;
		}

		// Token: 0x06009807 RID: 38919 RVA: 0x00372100 File Offset: 0x00370300
		private bool IsValidPawn(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			return lord != null && (lord.LordJob is LordJob_PsychicRitual || lord.LordJob is LordJob_HateChant);
		}

		// Token: 0x06009808 RID: 38920 RVA: 0x00372138 File Offset: 0x00370338
		private bool IsValidEntity(Pawn pawn)
		{
			return (pawn.RaceProps.IsAnomalyEntity || pawn.IsSubhuman) && !pawn.Downed && !pawn.IsOnHoldingPlatform && !pawn.IsPsychologicallyInvisible() && !pawn.Fogged() && pawn.HostileTo(Faction.OfPlayer);
		}
	}
}
