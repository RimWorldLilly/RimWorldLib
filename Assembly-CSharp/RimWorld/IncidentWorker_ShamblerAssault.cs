using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020011B5 RID: 4533
	public class IncidentWorker_ShamblerAssault : IncidentWorker_RaidEnemy
	{
		// Token: 0x06007136 RID: 28982 RVA: 0x0027A023 File Offset: 0x00278223
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			parms.pawnGroupKind = PawnGroupKindDefOf.Shamblers;
			return base.TryExecuteWorker(parms);
		}

		// Token: 0x06007137 RID: 28983 RVA: 0x0027A040 File Offset: 0x00278240
		protected override bool TryResolveRaidFaction(IncidentParms parms)
		{
			parms.faction = Faction.OfEntities;
			return true;
		}

		// Token: 0x06007138 RID: 28984 RVA: 0x0027A04E File Offset: 0x0027824E
		public override void ResolveRaidStrategy(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			parms.raidStrategy = RaidStrategyDefOf.ShamblerAssault;
		}

		// Token: 0x06007139 RID: 28985 RVA: 0x0027A05C File Offset: 0x0027825C
		protected override void PostProcessSpawnedPawns(IncidentParms parms, List<Pawn> pawns)
		{
			foreach (Pawn pawn in pawns)
			{
				Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Shambler, false);
				HediffComp_DisappearsAndKills hediffComp_DisappearsAndKills = (firstHediffOfDef != null) ? firstHediffOfDef.TryGetComp<HediffComp_DisappearsAndKills>() : null;
				if (firstHediffOfDef == null || hediffComp_DisappearsAndKills == null)
				{
					Log.ErrorOnce("ShamblerAssault spawned pawn without Shambler hediff", 23407834);
				}
				else
				{
					hediffComp_DisappearsAndKills.disabled = false;
					hediffComp_DisappearsAndKills.ticksToDisappear = IncidentWorker_ShamblerAssault.ShamblerLifespanTicksRange.RandomInRange;
				}
			}
			if (AnomalyIncidentUtility.IncidentShardChance(parms.points))
			{
				AnomalyIncidentUtility.PawnShardOnDeath(pawns.RandomElement<Pawn>());
			}
		}

		// Token: 0x0600713A RID: 28986 RVA: 0x0027A110 File Offset: 0x00278310
		protected override string GetLetterLabel(IncidentParms parms)
		{
			return parms.raidStrategy.letterLabelEnemy;
		}

		// Token: 0x0600713B RID: 28987 RVA: 0x0027A120 File Offset: 0x00278320
		protected override string GetLetterText(IncidentParms parms, List<Pawn> pawns)
		{
			string text = string.Format(parms.raidArrivalMode.textEnemy, parms.pawnCount).CapitalizeFirst();
			text += "\n\n";
			text += parms.raidStrategy.arrivalTextEnemy;
			if (parms.pawnGroups != null && parms.PawnGroupCount > 1)
			{
				text += "\n\n" + "AttackingFromMultipleDirections".Translate();
			}
			int num = pawns.Count((Pawn e) => e.kindDef == PawnKindDefOf.ShamblerGorehulk);
			if (num == 1)
			{
				text += "\n\n" + "LetterText_ShamblerGorehulk".Translate();
			}
			else if (num > 1)
			{
				text += "\n\n" + "LetterText_ShamblerGorehulkPlural".Translate();
			}
			return text;
		}

		// Token: 0x040047C5 RID: 18373
		private static readonly IntRange ShamblerLifespanTicksRange = new IntRange(10000, 25000);
	}
}
