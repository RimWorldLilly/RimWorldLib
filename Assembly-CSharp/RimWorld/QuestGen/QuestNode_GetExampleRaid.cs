using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020022C0 RID: 8896
	public class QuestNode_GetExampleRaid : QuestNode
	{
		// Token: 0x0600DB7F RID: 56191 RVA: 0x0051C60E File Offset: 0x0051A80E
		protected override bool TestRunInt(Slate slate)
		{
			return slate.Exists("map", false);
		}

		// Token: 0x0600DB80 RID: 56192 RVA: 0x0051C61C File Offset: 0x0051A81C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Map target = slate.Get<Map>("map", null, false);
			PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms();
			pawnGroupMakerParms.groupKind = PawnGroupKindDefOf.Combat;
			pawnGroupMakerParms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
			Faction faction;
			if ((faction = this.faction.GetValue(slate)) == null)
			{
				faction = (from x in Find.FactionManager.GetFactions(false, false, true, TechLevel.Industrial, false)
				where x.HostileTo(Faction.OfPlayer)
				select x).RandomElement<Faction>();
			}
			pawnGroupMakerParms.faction = faction;
			PawnGroupMakerParms pawnGroupMakerParms2 = pawnGroupMakerParms;
			pawnGroupMakerParms2.points = IncidentWorker_Raid.AdjustedRaidPoints(this.points.GetValue(slate), PawnsArrivalModeDefOf.EdgeWalkIn, RaidStrategyDefOf.ImmediateAttack, pawnGroupMakerParms2.faction, PawnGroupKindDefOf.Combat, target, null);
			IEnumerable<PawnKindDef> pawnKinds = PawnGroupMakerUtility.GeneratePawnKindsExample(pawnGroupMakerParms2);
			slate.Set<string>(this.storeAs.GetValue(slate), PawnUtility.PawnKindsToLineList(pawnKinds, "  - ", ColoredText.ThreatColor), false);
		}

		// Token: 0x04008C8B RID: 35979
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04008C8C RID: 35980
		public SlateRef<Faction> faction;

		// Token: 0x04008C8D RID: 35981
		public SlateRef<float> points;
	}
}
