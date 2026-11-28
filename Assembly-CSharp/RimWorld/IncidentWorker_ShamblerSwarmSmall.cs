using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001198 RID: 4504
	public class IncidentWorker_ShamblerSwarmSmall : IncidentWorker_ShamblerSwarm
	{
		// Token: 0x1700132D RID: 4909
		// (get) Token: 0x060070B9 RID: 28857 RVA: 0x0027731A File Offset: 0x0027551A
		protected override IntRange ShamblerLifespanTicksRange
		{
			get
			{
				return new IntRange(25000, 45000);
			}
		}

		// Token: 0x060070BA RID: 28858 RVA: 0x0027732C File Offset: 0x0027552C
		protected override List<Pawn> GenerateEntities(IncidentParms parms, float points)
		{
			int randomInRange = IncidentWorker_ShamblerSwarmSmall.NumShamblersToSpawn.RandomInRange;
			List<Pawn> list = new List<Pawn>();
			for (int i = 0; i < randomInRange; i++)
			{
				Pawn item = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.ShamblerSwarmer, Faction.OfEntities, PawnGenerationContext.NonPlayer, new PlanetTile?(parms.target.Tile), false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
				list.Add(item);
			}
			base.SetupShamblerHediffs(list, this.ShamblerLifespanTicksRange);
			return list;
		}

		// Token: 0x060070BB RID: 28859 RVA: 0x0027741C File Offset: 0x0027561C
		protected override void SendLetter(IncidentParms parms, List<Pawn> entities)
		{
			string letterLabel = this.def.letterLabel;
			TaggedString baseLetterText = this.def.letterText.Formatted(entities.Count);
			base.SendStandardLetter(letterLabel, baseLetterText, this.def.letterDef, parms, entities, Array.Empty<NamedArgument>());
		}

		// Token: 0x040047B9 RID: 18361
		private static readonly IntRange NumShamblersToSpawn = new IntRange(2, 4);
	}
}
