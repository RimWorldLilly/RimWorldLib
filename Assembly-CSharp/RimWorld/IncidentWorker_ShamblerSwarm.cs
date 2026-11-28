using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001197 RID: 4503
	public class IncidentWorker_ShamblerSwarm : IncidentWorker_EntitySwarm
	{
		// Token: 0x1700132B RID: 4907
		// (get) Token: 0x060070B2 RID: 28850 RVA: 0x0027714F File Offset: 0x0027534F
		protected virtual IntRange ShamblerLifespanTicksRange { get; } = new IntRange(150000, 210000);

		// Token: 0x1700132C RID: 4908
		// (get) Token: 0x060070B3 RID: 28851 RVA: 0x00277157 File Offset: 0x00275357
		protected override PawnGroupKindDef GroupKindDef
		{
			get
			{
				return PawnGroupKindDefOf.Shamblers;
			}
		}

		// Token: 0x060070B4 RID: 28852 RVA: 0x0027715E File Offset: 0x0027535E
		protected override LordJob GenerateLordJob(IntVec3 entry, IntVec3 dest)
		{
			return new LordJob_ShamblerSwarm(entry, dest);
		}

		// Token: 0x060070B5 RID: 28853 RVA: 0x00277168 File Offset: 0x00275368
		protected override List<Pawn> GenerateEntities(IncidentParms parms, float points)
		{
			List<Pawn> list = base.GenerateEntities(parms, points);
			this.SetupShamblerHediffs(list, this.ShamblerLifespanTicksRange);
			return list;
		}

		// Token: 0x060070B6 RID: 28854 RVA: 0x0027718C File Offset: 0x0027538C
		protected void SetupShamblerHediffs(List<Pawn> shamblers, IntRange lifespanRange)
		{
			foreach (Pawn pawn in shamblers)
			{
				Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Shambler, false);
				HediffComp_DisappearsAndKills hediffComp_DisappearsAndKills = (firstHediffOfDef != null) ? firstHediffOfDef.TryGetComp<HediffComp_DisappearsAndKills>() : null;
				if (firstHediffOfDef == null || hediffComp_DisappearsAndKills == null)
				{
					Log.ErrorOnce("ShamblerSwarm spawned pawn without Shambler hediff", 63426234);
				}
				else
				{
					hediffComp_DisappearsAndKills.disabled = false;
					hediffComp_DisappearsAndKills.ticksToDisappear = lifespanRange.RandomInRange;
				}
			}
		}

		// Token: 0x060070B7 RID: 28855 RVA: 0x00277220 File Offset: 0x00275420
		protected override void SendLetter(IncidentParms parms, List<Pawn> entities)
		{
			TaggedString baseLetterLabel = (entities.Count > 1) ? "LetterLabelShamblerSwarmArrived".Translate() : "LetterLabelShamblerArrived".Translate();
			TaggedString taggedString = (entities.Count > 1) ? "LetterShamblerSwarmArrived".Translate(entities.Count) : "LetterShamblerArrived".Translate();
			int num = entities.Count((Pawn e) => e.kindDef == PawnKindDefOf.ShamblerGorehulk);
			if (num == 1)
			{
				taggedString += "\n\n" + "LetterText_ShamblerGorehulk".Translate();
			}
			else if (num > 1)
			{
				taggedString += "\n\n" + "LetterText_ShamblerGorehulkPlural".Translate();
			}
			base.SendStandardLetter(baseLetterLabel, taggedString, this.def.letterDef, parms, entities, Array.Empty<NamedArgument>());
		}
	}
}
