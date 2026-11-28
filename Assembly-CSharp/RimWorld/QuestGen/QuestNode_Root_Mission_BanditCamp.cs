using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002281 RID: 8833
	public class QuestNode_Root_Mission_BanditCamp : QuestNode_Root_Mission
	{
		// Token: 0x1700261D RID: 9757
		// (get) Token: 0x0600DA26 RID: 55846 RVA: 0x0051169F File Offset: 0x0050F89F
		protected override string QuestTag
		{
			get
			{
				return "BanditCamp";
			}
		}

		// Token: 0x1700261E RID: 9758
		// (get) Token: 0x0600DA27 RID: 55847 RVA: 0x005116A6 File Offset: 0x0050F8A6
		protected override bool AddCampLootReward
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700261F RID: 9759
		// (get) Token: 0x0600DA28 RID: 55848 RVA: 0x005116AC File Offset: 0x0050F8AC
		private QuestGen_Pawns.GetPawnParms GetAskerParms
		{
			get
			{
				return new QuestGen_Pawns.GetPawnParms
				{
					mustBeOfKind = PawnKindDefOf.Empire_Royal_NobleWimp,
					mustHaveRoyalTitleInCurrentFaction = true,
					canGeneratePawn = true
				};
			}
		}

		// Token: 0x0600DA29 RID: 55849 RVA: 0x005116DE File Offset: 0x0050F8DE
		protected override Pawn GetAsker(Quest quest)
		{
			if (Rand.Chance(0.1f))
			{
				return (from f in Find.FactionManager.AllFactions
				where this.factionsToDrawLeaderFrom.Contains(f.def)
				select f).RandomElement<Faction>().leader;
			}
			return quest.GetPawn(this.GetAskerParms);
		}

		// Token: 0x0600DA2A RID: 55850 RVA: 0x00511720 File Offset: 0x0050F920
		protected override bool CanGetAsker()
		{
			Pawn pawn;
			return QuestGen_Pawns.GetPawnTest(this.GetAskerParms, out pawn);
		}

		// Token: 0x0600DA2B RID: 55851 RVA: 0x0051173A File Offset: 0x0050F93A
		private float GetSiteThreatPoints(float threatPoints, int population, int pawnCount)
		{
			return threatPoints * ((float)pawnCount / (float)population) * QuestNode_Root_Mission_BanditCamp.PawnCountToSitePointsFactorCurve.Evaluate((float)pawnCount);
		}

		// Token: 0x0600DA2C RID: 55852 RVA: 0x00511750 File Offset: 0x0050F950
		protected override int GetRequiredPawnCount(int population, float threatPoints)
		{
			if (population == 0)
			{
				return -1;
			}
			int num = -1;
			for (int i = 1; i < population; i++)
			{
				if (this.GetSiteThreatPoints(threatPoints, population, i) >= 200f)
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				return -1;
			}
			int maxInclusive = Math.Max(num, (int)(0.5f * (float)population));
			return Rand.RangeInclusive(num, maxInclusive);
		}

		// Token: 0x0600DA2D RID: 55853 RVA: 0x005117A4 File Offset: 0x0050F9A4
		protected override Site GenerateSite(Pawn asker, float threatPoints, int pawnCount, int population, PlanetTile tile)
		{
			Faction faction;
			this.TryGetSiteFaction(out faction);
			Site site = QuestGen_Sites.GenerateSite(new SitePartDefWithParams[]
			{
				new SitePartDefWithParams(SitePartDefOf.BanditCamp, new SitePartParams
				{
					threatPoints = this.GetSiteThreatPoints(threatPoints, population, pawnCount)
				})
			}, tile, faction, false, null, null);
			site.factionMustRemainHostile = true;
			site.desiredThreatPoints = site.ActualThreatPoints;
			site.preventGravshipLanding = true;
			return site;
		}

		// Token: 0x0600DA2E RID: 55854 RVA: 0x00511807 File Offset: 0x0050FA07
		private bool TryGetSiteFaction(out Faction faction)
		{
			return (from f in Find.FactionManager.AllFactions
			where !f.temporary && this.siteFactions.Contains(f.def)
			select f).TryRandomElement(out faction);
		}

		// Token: 0x0600DA2F RID: 55855 RVA: 0x0051182A File Offset: 0x0050FA2A
		protected override bool DoesPawnCountAsAvailableForFight(Pawn p)
		{
			return QuestNode_Root_Mission.PawnCanFight(p);
		}

		// Token: 0x0600DA30 RID: 55856 RVA: 0x00511834 File Offset: 0x0050FA34
		protected override bool TestRunInt(Slate slate)
		{
			Faction faction;
			return base.TestRunInt(slate) && this.TryGetSiteFaction(out faction);
		}

		// Token: 0x04008B7B RID: 35707
		private const float LeaderChance = 0.1f;

		// Token: 0x04008B7C RID: 35708
		private const float MinSiteThreatPoints = 200f;

		// Token: 0x04008B7D RID: 35709
		private static readonly SimpleCurve PawnCountToSitePointsFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(1f, 0.33f),
				true
			},
			{
				new CurvePoint(3f, 0.37f),
				true
			},
			{
				new CurvePoint(5f, 0.45f),
				true
			},
			{
				new CurvePoint(10f, 0.5f),
				true
			}
		};

		// Token: 0x04008B7E RID: 35710
		public List<FactionDef> factionsToDrawLeaderFrom;

		// Token: 0x04008B7F RID: 35711
		public List<FactionDef> siteFactions;
	}
}
