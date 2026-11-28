using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000431 RID: 1073
	public class HediffComp_FleshbeastEmerge : HediffComp
	{
		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x06001F28 RID: 7976 RVA: 0x000BDE4C File Offset: 0x000BC04C
		public HediffCompProperties_FleshbeastEmerge Props
		{
			get
			{
				return (HediffCompProperties_FleshbeastEmerge)this.props;
			}
		}

		// Token: 0x06001F29 RID: 7977 RVA: 0x000BDE59 File Offset: 0x000BC059
		public override void Notify_SurgicallyRemoved(Pawn surgeon)
		{
			this.TentacleAttack(surgeon);
		}

		// Token: 0x06001F2A RID: 7978 RVA: 0x000BDE62 File Offset: 0x000BC062
		public override void Notify_SurgicallyReplaced(Pawn surgeon)
		{
			this.TentacleAttack(surgeon);
		}

		// Token: 0x06001F2B RID: 7979 RVA: 0x000BDE6C File Offset: 0x000BC06C
		private void TentacleAttack(Pawn surgeon)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return;
			}
			Pawn pawn = this.parent.pawn;
			PawnKindDef fingerspike = PawnKindDefOf.Fingerspike;
			Faction ofEntities = Faction.OfEntities;
			PawnGenerationContext context = PawnGenerationContext.NonPlayer;
			float? fixedBiologicalAge = new float?(0f);
			float? fixedChronologicalAge = new float?(0f);
			Pawn pawn2 = PawnGenerator.GeneratePawn(new PawnGenerationRequest(fingerspike, ofEntities, context, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, fixedBiologicalAge, fixedChronologicalAge, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
			GenSpawn.Spawn(pawn2, CellFinder.StandableCellNear(pawn.Position, pawn.Map, 2f, null), pawn.Map, WipeMode.Vanish);
			pawn2.stances.stunner.StunFor(HediffComp_FleshbeastEmerge.StunDuration.RandomInRange, surgeon, true, true, false);
			CompInspectStringEmergence compInspectStringEmergence = pawn2.TryGetComp<CompInspectStringEmergence>();
			if (compInspectStringEmergence != null)
			{
				compInspectStringEmergence.sourcePawn = pawn;
			}
			TaggedString label = this.Props.letterLabel.Formatted(pawn.Named("PAWN"));
			TaggedString text = this.Props.letterText.Formatted(pawn.Named("PAWN"));
			Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.ThreatBig, pawn2, null, null, null, null, 0, true);
		}

		// Token: 0x04001856 RID: 6230
		private static readonly IntRange StunDuration = new IntRange(120, 240);
	}
}
