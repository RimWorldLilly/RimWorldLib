using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020004D6 RID: 1238
	public class Pawn : ThingWithComps, IStrippable, IBillGiver, IVerbOwner, ITrader, IAttackTarget, ILoadReferenceable, IAttackTargetSearcher, IThingHolder, IObservedThoughtGiver, ISearchableContents, IEquatable<Pawn>
	{
		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x06002296 RID: 8854 RVA: 0x000CCE24 File Offset: 0x000CB024
		// (set) Token: 0x06002297 RID: 8855 RVA: 0x000CCE2C File Offset: 0x000CB02C
		public Name Name
		{
			get
			{
				return this.nameInt;
			}
			set
			{
				this.nameInt = value;
			}
		}

		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x06002298 RID: 8856 RVA: 0x000CCE35 File Offset: 0x000CB035
		public RaceProperties RaceProps
		{
			get
			{
				return this.def.race;
			}
		}

		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x06002299 RID: 8857 RVA: 0x000CCE42 File Offset: 0x000CB042
		public Job CurJob
		{
			get
			{
				Pawn_JobTracker pawn_JobTracker = this.jobs;
				if (pawn_JobTracker == null)
				{
					return null;
				}
				return pawn_JobTracker.curJob;
			}
		}

		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x0600229A RID: 8858 RVA: 0x000CCE55 File Offset: 0x000CB055
		public JobDef CurJobDef
		{
			get
			{
				Job curJob = this.CurJob;
				if (curJob == null)
				{
					return null;
				}
				return curJob.def;
			}
		}

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x0600229B RID: 8859 RVA: 0x000CCE68 File Offset: 0x000CB068
		public bool Downed
		{
			get
			{
				return this.health.Downed;
			}
		}

		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x0600229C RID: 8860 RVA: 0x000CCE75 File Offset: 0x000CB075
		public bool Crawling
		{
			get
			{
				return this.Downed && this.health.CanCrawl && this.CurJobDef != null && this.CurJobDef.isCrawlingIfDowned && !this.InBed();
			}
		}

		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x0600229D RID: 8861 RVA: 0x000CCEAC File Offset: 0x000CB0AC
		public bool CanAttackWhileCrawling
		{
			get
			{
				return this.IsMutant && this.mutant.Def.canAttackWhileCrawling;
			}
		}

		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x0600229E RID: 8862 RVA: 0x000CCEC8 File Offset: 0x000CB0C8
		public bool Flying
		{
			get
			{
				Pawn_FlightTracker pawn_FlightTracker = this.flight;
				return pawn_FlightTracker != null && pawn_FlightTracker.Flying;
			}
		}

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x0600229F RID: 8863 RVA: 0x000CCEDB File Offset: 0x000CB0DB
		public bool Swimming
		{
			get
			{
				Job curJob = this.CurJob;
				return curJob != null && curJob.swimming;
			}
		}

		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x060022A0 RID: 8864 RVA: 0x000CCEEE File Offset: 0x000CB0EE
		public bool Dead
		{
			get
			{
				return this.health.Dead;
			}
		}

		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x060022A1 RID: 8865 RVA: 0x000CCEFB File Offset: 0x000CB0FB
		public bool DeadOrDowned
		{
			get
			{
				return this.Dead || this.Downed;
			}
		}

		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x060022A2 RID: 8866 RVA: 0x000CCF0D File Offset: 0x000CB10D
		public string KindLabel
		{
			get
			{
				return GenLabel.BestKindLabel(this, false, false, false, -1);
			}
		}

		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x060022A3 RID: 8867 RVA: 0x000CCF19 File Offset: 0x000CB119
		public bool InMentalState
		{
			get
			{
				return !this.Dead && this.mindState.mentalStateHandler.InMentalState;
			}
		}

		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x060022A4 RID: 8868 RVA: 0x000CCF35 File Offset: 0x000CB135
		public MentalState MentalState
		{
			get
			{
				if (!this.Dead)
				{
					return this.mindState.mentalStateHandler.CurState;
				}
				return null;
			}
		}

		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x060022A5 RID: 8869 RVA: 0x000CCF51 File Offset: 0x000CB151
		public MentalStateDef MentalStateDef
		{
			get
			{
				if (!this.Dead)
				{
					return this.mindState.mentalStateHandler.CurStateDef;
				}
				return null;
			}
		}

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x060022A6 RID: 8870 RVA: 0x000CCF6D File Offset: 0x000CB16D
		public bool InAggroMentalState
		{
			get
			{
				return !this.Dead && this.mindState.mentalStateHandler.InMentalState && this.mindState.mentalStateHandler.CurStateDef.IsAggro;
			}
		}

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x060022A7 RID: 8871 RVA: 0x000CCFA0 File Offset: 0x000CB1A0
		public bool Inspired
		{
			get
			{
				if (!this.Dead)
				{
					Pawn_MindState pawn_MindState = this.mindState;
					if (((pawn_MindState != null) ? pawn_MindState.inspirationHandler : null) != null)
					{
						return this.mindState.inspirationHandler.Inspired;
					}
				}
				return false;
			}
		}

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x060022A8 RID: 8872 RVA: 0x000CCFD0 File Offset: 0x000CB1D0
		public Inspiration Inspiration
		{
			get
			{
				if (!this.Dead)
				{
					return this.mindState.inspirationHandler.CurState;
				}
				return null;
			}
		}

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x060022A9 RID: 8873 RVA: 0x000CCFEC File Offset: 0x000CB1EC
		public InspirationDef InspirationDef
		{
			get
			{
				if (!this.Dead)
				{
					return this.mindState.inspirationHandler.CurStateDef;
				}
				return null;
			}
		}

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x060022AA RID: 8874 RVA: 0x000CD008 File Offset: 0x000CB208
		public override Vector3 DrawPos
		{
			get
			{
				return this.Drawer.DrawPos;
			}
		}

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x060022AB RID: 8875 RVA: 0x000CD015 File Offset: 0x000CB215
		public VerbTracker VerbTracker
		{
			get
			{
				return this.verbTracker;
			}
		}

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x060022AC RID: 8876 RVA: 0x000CD01D File Offset: 0x000CB21D
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return this.def.Verbs;
			}
		}

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x060022AD RID: 8877 RVA: 0x000CD02A File Offset: 0x000CB22A
		public List<Tool> Tools
		{
			get
			{
				return this.def.tools;
			}
		}

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x060022AE RID: 8878 RVA: 0x000CD037 File Offset: 0x000CB237
		public bool ShouldAvoidFences
		{
			get
			{
				return this.FenceBlocked || (this.roping != null && this.roping.AnyRopeesFenceBlocked);
			}
		}

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x060022AF RID: 8879 RVA: 0x000CD058 File Offset: 0x000CB258
		public float? RoamMtbDays
		{
			get
			{
				Pawn_HealthTracker pawn_HealthTracker = this.health;
				if (pawn_HealthTracker != null)
				{
					HediffSet hediffSet = pawn_HealthTracker.hediffSet;
					if (hediffSet != null && hediffSet.RemoveRoamMtb)
					{
						return null;
					}
				}
				RaceProperties raceProps = this.RaceProps;
				if (raceProps == null)
				{
					return null;
				}
				return raceProps.roamMtbDays;
			}
		}

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x060022B0 RID: 8880 RVA: 0x000CD0A4 File Offset: 0x000CB2A4
		public bool Roamer
		{
			get
			{
				return this.RoamMtbDays != null;
			}
		}

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x060022B1 RID: 8881 RVA: 0x000CD0BF File Offset: 0x000CB2BF
		public bool FenceBlocked
		{
			get
			{
				return this.Roamer && (this.CurJobDef == null || !this.CurJobDef.ignoreFenceBlocked);
			}
		}

		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x060022B2 RID: 8882 RVA: 0x000CD0E3 File Offset: 0x000CB2E3
		public bool CanPassFences
		{
			get
			{
				return !this.FenceBlocked;
			}
		}

		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x060022B3 RID: 8883 RVA: 0x000CD0F0 File Offset: 0x000CB2F0
		public bool DrawNonHumanlikeSwimmingGraphic
		{
			get
			{
				return base.Spawned && this.WaterCellCost != null && !this.RaceProps.Humanlike && this.ageTracker.CurKindLifeStage.swimmingGraphicData != null && base.Position.GetTerrain(base.Map).IsWater;
			}
		}

		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x060022B4 RID: 8884 RVA: 0x000CD14E File Offset: 0x000CB34E
		public bool DrawNonHumanlikeStationaryGraphic
		{
			get
			{
				return base.Spawned && !this.RaceProps.Humanlike && !this.pather.Moving && this.ageTracker.CurKindLifeStage.stationaryGraphicData != null;
			}
		}

		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x060022B5 RID: 8885 RVA: 0x000CD189 File Offset: 0x000CB389
		public bool CanOpenDoors
		{
			get
			{
				return (!this.IsMutant || this.mutant.Def.canOpenDoors) && this.kindDef.canOpenDoors;
			}
		}

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x060022B6 RID: 8886 RVA: 0x000CD1B8 File Offset: 0x000CB3B8
		public bool CanOpenAnyDoor
		{
			get
			{
				if (WildManUtility.WildManShouldReachOutsideNow(this))
				{
					return true;
				}
				Lord lord = this.lord;
				return (((lord != null) ? lord.LordJob : null) != null && this.lord.LordJob.CanOpenAnyDoor(this)) || (this.IsMutant && this.mutant.Def.canOpenAnyDoor) || this.kindDef.canOpenAnyDoor;
			}
		}

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x060022B7 RID: 8887 RVA: 0x000CD224 File Offset: 0x000CB424
		public int? WaterCellCost
		{
			get
			{
				if (this.Flying)
				{
					return new int?(1);
				}
				if (ModsConfig.BiotechActive && this.genes != null && this.genes.WaterCellCost != null)
				{
					return this.genes.WaterCellCost;
				}
				if (this.def.race.waterCellCost != null)
				{
					return this.def.race.waterCellCost;
				}
				if (!this.Swimming)
				{
					return null;
				}
				return new int?(1);
			}
		}

		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x060022B8 RID: 8888 RVA: 0x000CD2B0 File Offset: 0x000CB4B0
		public bool IsColonist
		{
			get
			{
				return base.Faction != null && base.Faction.IsPlayer && this.RaceProps.Humanlike && (!this.IsSlave || this.guest.SlaveIsSecure) && !this.IsSubhuman;
			}
		}

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x060022B9 RID: 8889 RVA: 0x000CD2FF File Offset: 0x000CB4FF
		public bool IsFreeColonist
		{
			get
			{
				return this.IsColonist && this.HostFaction == null;
			}
		}

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x060022BA RID: 8890 RVA: 0x000CD314 File Offset: 0x000CB514
		public bool IsFreeNonSlaveColonist
		{
			get
			{
				return this.IsFreeColonist && !this.IsSlave;
			}
		}

		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x060022BB RID: 8891 RVA: 0x000CD329 File Offset: 0x000CB529
		public bool CanTakeOrder
		{
			get
			{
				return this.IsColonistPlayerControlled || this.IsColonyMech || this.IsColonySubhumanPlayerControlled;
			}
		}

		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x060022BC RID: 8892 RVA: 0x000CD343 File Offset: 0x000CB543
		public bool IsCreepJoiner
		{
			get
			{
				return ModsConfig.AnomalyActive && this.creepjoiner != null;
			}
		}

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x060022BD RID: 8893 RVA: 0x000CD357 File Offset: 0x000CB557
		public Faction HostFaction
		{
			get
			{
				Pawn_GuestTracker pawn_GuestTracker = this.guest;
				if (pawn_GuestTracker == null)
				{
					return null;
				}
				return pawn_GuestTracker.HostFaction;
			}
		}

		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x060022BE RID: 8894 RVA: 0x000CD36A File Offset: 0x000CB56A
		public Faction SlaveFaction
		{
			get
			{
				Pawn_GuestTracker pawn_GuestTracker = this.guest;
				if (pawn_GuestTracker == null)
				{
					return null;
				}
				return pawn_GuestTracker.SlaveFaction;
			}
		}

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x060022BF RID: 8895 RVA: 0x000CD37D File Offset: 0x000CB57D
		public Ideo Ideo
		{
			get
			{
				Pawn_IdeoTracker pawn_IdeoTracker = this.ideo;
				if (pawn_IdeoTracker == null)
				{
					return null;
				}
				return pawn_IdeoTracker.Ideo;
			}
		}

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x060022C0 RID: 8896 RVA: 0x000CD390 File Offset: 0x000CB590
		public bool ShouldHaveIdeo
		{
			get
			{
				return !this.DevelopmentalStage.Baby() && !this.kindDef.preventIdeo && (!this.IsMutant || !this.mutant.Def.disablesIdeo);
			}
		}

		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x060022C1 RID: 8897 RVA: 0x000CD3CB File Offset: 0x000CB5CB
		public bool Drafted
		{
			get
			{
				return this.drafter != null && this.drafter.Drafted;
			}
		}

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x060022C2 RID: 8898 RVA: 0x000CD3E2 File Offset: 0x000CB5E2
		public bool IsPrisoner
		{
			get
			{
				return this.guest != null && this.guest.IsPrisoner;
			}
		}

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x060022C3 RID: 8899 RVA: 0x000CD3F9 File Offset: 0x000CB5F9
		public bool IsPrisonerOfColony
		{
			get
			{
				return this.guest != null && this.guest.IsPrisoner && this.guest.HostFaction.IsPlayer;
			}
		}

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x060022C4 RID: 8900 RVA: 0x000CD422 File Offset: 0x000CB622
		public bool IsSlave
		{
			get
			{
				return this.guest != null && this.guest.IsSlave;
			}
		}

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x060022C5 RID: 8901 RVA: 0x000CD439 File Offset: 0x000CB639
		public bool IsSlaveOfColony
		{
			get
			{
				return this.IsSlave && base.Faction.IsPlayer;
			}
		}

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x060022C6 RID: 8902 RVA: 0x000CD450 File Offset: 0x000CB650
		public bool IsFreeman
		{
			get
			{
				return this.HostFaction == null;
			}
		}

		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x060022C7 RID: 8903 RVA: 0x000CD45B File Offset: 0x000CB65B
		public bool IsMutant
		{
			get
			{
				return this.mutant != null;
			}
		}

		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x060022C8 RID: 8904 RVA: 0x000CD466 File Offset: 0x000CB666
		public bool IsSubhuman
		{
			get
			{
				return this.IsMutant && this.mutant.Def.consideredSubhuman;
			}
		}

		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x060022C9 RID: 8905 RVA: 0x000CD482 File Offset: 0x000CB682
		public bool IsDuplicate
		{
			get
			{
				return ModsConfig.AnomalyActive && this.duplicate != null && this.duplicate.duplicateOf != int.MinValue && this.duplicate.duplicateOf != this.thingIDNumber;
			}
		}

		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x060022CA RID: 8906 RVA: 0x000CD4BD File Offset: 0x000CB6BD
		public bool IsEntity
		{
			get
			{
				return ModsConfig.AnomalyActive && (!this.RaceProps.Humanlike || this.IsSubhuman || this.IsShambler) && base.Faction == Faction.OfEntities;
			}
		}

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x060022CB RID: 8907 RVA: 0x000CD4F2 File Offset: 0x000CB6F2
		public bool IsShambler
		{
			get
			{
				return ModsConfig.AnomalyActive && ((this.IsMutant && this.mutant.Def == MutantDefOf.Shambler) || this.health.hediffSet.HasHediff(HediffDefOf.ShamblerCorpse, false));
			}
		}

		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x060022CC RID: 8908 RVA: 0x000CD52F File Offset: 0x000CB72F
		public bool IsGhoul
		{
			get
			{
				return ModsConfig.AnomalyActive && this.IsMutant && this.mutant.Def == MutantDefOf.Ghoul;
			}
		}

		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x060022CD RID: 8909 RVA: 0x000CD554 File Offset: 0x000CB754
		public bool IsAwokenCorpse
		{
			get
			{
				return ModsConfig.AnomalyActive && this.IsMutant && this.mutant.Def == MutantDefOf.AwokenCorpse;
			}
		}

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x060022CE RID: 8910 RVA: 0x000CD579 File Offset: 0x000CB779
		public bool IsAnimal
		{
			get
			{
				return this.RaceProps.Animal && !this.IsSubhuman;
			}
		}

		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x060022CF RID: 8911 RVA: 0x000CD593 File Offset: 0x000CB793
		public bool HasShowGizmosOnCorpseHediff
		{
			get
			{
				Pawn_HealthTracker pawn_HealthTracker = this.health;
				return ((pawn_HealthTracker != null) ? pawn_HealthTracker.hediffSet : null) != null && this.health.hediffSet.HasHediffShowGizmosOnCorpse();
			}
		}

		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x060022D0 RID: 8912 RVA: 0x000CD5BC File Offset: 0x000CB7BC
		public DevelopmentalStage DevelopmentalStage
		{
			get
			{
				Pawn_AgeTracker pawn_AgeTracker = this.ageTracker;
				DevelopmentalStage? developmentalStage;
				if (pawn_AgeTracker == null)
				{
					developmentalStage = null;
				}
				else
				{
					LifeStageDef curLifeStage = pawn_AgeTracker.CurLifeStage;
					developmentalStage = ((curLifeStage != null) ? new DevelopmentalStage?(curLifeStage.developmentalStage) : null);
				}
				DevelopmentalStage? developmentalStage2 = developmentalStage;
				return developmentalStage2.GetValueOrDefault(DevelopmentalStage.Adult);
			}
		}

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x060022D1 RID: 8913 RVA: 0x000CD608 File Offset: 0x000CB808
		public GuestStatus? GuestStatus
		{
			get
			{
				if (this.guest != null && (this.HostFaction != null || this.guest.GuestStatus != RimWorld.GuestStatus.Guest))
				{
					return new GuestStatus?(this.guest.GuestStatus);
				}
				return null;
			}
		}

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x060022D2 RID: 8914 RVA: 0x000CD64C File Offset: 0x000CB84C
		public bool IsColonistPlayerControlled
		{
			get
			{
				return base.Spawned && this.IsColonist && this.MentalStateDef == null && (this.HostFaction == null || this.IsSlave);
			}
		}

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x060022D3 RID: 8915 RVA: 0x000CD678 File Offset: 0x000CB878
		public bool IsColonyMech
		{
			get
			{
				return ModsConfig.BiotechActive && this.RaceProps.IsMechanoid && base.Faction == Faction.OfPlayer && this.MentalStateDef == null && (this.HostFaction == null || this.IsSlave);
			}
		}

		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x060022D4 RID: 8916 RVA: 0x000CD6B5 File Offset: 0x000CB8B5
		public bool IsColonyMechPlayerControlled
		{
			get
			{
				return base.Spawned && this.IsColonyMech && this.OverseerSubject != null && this.OverseerSubject.State == OverseerSubjectState.Overseen;
			}
		}

		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x060022D5 RID: 8917 RVA: 0x000CD6DF File Offset: 0x000CB8DF
		public bool IsColonySubhuman
		{
			get
			{
				return this.IsSubhuman && base.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x060022D6 RID: 8918 RVA: 0x000CD6F8 File Offset: 0x000CB8F8
		public bool IsColonySubhumanPlayerControlled
		{
			get
			{
				return base.Spawned && this.IsColonySubhuman && this.mutant.Def.canBeDrafted;
			}
		}

		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x060022D7 RID: 8919 RVA: 0x000CD71C File Offset: 0x000CB91C
		public bool IsColonyAnimal
		{
			get
			{
				return this.IsAnimal && base.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x060022D8 RID: 8920 RVA: 0x000CD735 File Offset: 0x000CB935
		public bool IsPlayerControlled
		{
			get
			{
				return this.IsColonistPlayerControlled || this.IsColonyMechPlayerControlled || this.IsColonySubhumanPlayerControlled;
			}
		}

		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x060022D9 RID: 8921 RVA: 0x000CD74F File Offset: 0x000CB94F
		public IEnumerable<IntVec3> IngredientStackCells
		{
			get
			{
				Pawn.<get_IngredientStackCells>d__226 <get_IngredientStackCells>d__ = new Pawn.<get_IngredientStackCells>d__226(-2);
				<get_IngredientStackCells>d__.<>4__this = this;
				return <get_IngredientStackCells>d__;
			}
		}

		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x060022DA RID: 8922 RVA: 0x000CD75F File Offset: 0x000CB95F
		public bool InContainerEnclosed
		{
			get
			{
				return base.ParentHolder.IsEnclosingContainer();
			}
		}

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x060022DB RID: 8923 RVA: 0x000CD76C File Offset: 0x000CB96C
		public Corpse Corpse
		{
			get
			{
				return base.ParentHolder as Corpse;
			}
		}

		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x060022DC RID: 8924 RVA: 0x000CD77C File Offset: 0x000CB97C
		public Pawn CarriedBy
		{
			get
			{
				Pawn_CarryTracker pawn_CarryTracker = base.ParentHolder as Pawn_CarryTracker;
				if (pawn_CarryTracker == null)
				{
					return null;
				}
				return pawn_CarryTracker.pawn;
			}
		}

		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x060022DD RID: 8925 RVA: 0x000CD7A0 File Offset: 0x000CB9A0
		public virtual bool CanAttackWhenPathingBlocked
		{
			get
			{
				return !this.IsAwokenCorpse;
			}
		}

		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x060022DE RID: 8926 RVA: 0x000CD7AC File Offset: 0x000CB9AC
		public bool HarmedByVacuum
		{
			get
			{
				return ModsConfig.OdysseyActive && !this.RaceProps.IsMechanoid && (!this.IsMutant || this.mutant.Def.breathesAir) && this.GetStatValue(StatDefOf.VacuumResistance, true, 60) < 1f;
			}
		}

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x060022DF RID: 8927 RVA: 0x000CD800 File Offset: 0x000CBA00
		public bool ConcernedByVacuum
		{
			get
			{
				return ModsConfig.OdysseyActive && !this.RaceProps.IsMechanoid && (!this.IsMutant || this.mutant.Def.breathesAir) && this.GetStatValue(StatDefOf.VacuumResistance, true, 60) < 0.75f;
			}
		}

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x060022E0 RID: 8928 RVA: 0x000CD854 File Offset: 0x000CBA54
		private string LabelPrefix
		{
			get
			{
				if (this.IsMutant && this.mutant.HasTurned)
				{
					return this.mutant.Def.namePrefix;
				}
				return string.Empty;
			}
		}

		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x060022E1 RID: 8929 RVA: 0x000CD884 File Offset: 0x000CBA84
		public override string LabelNoCount
		{
			get
			{
				if (this.Name == null)
				{
					return this.LabelPrefix + this.KindLabel;
				}
				if (this.story == null || this.story.TitleShortCap.NullOrEmpty() || this.IsSubhuman)
				{
					return this.LabelPrefix + this.Name.ToStringShort;
				}
				return this.LabelPrefix + this.Name.ToStringShort + (", " + this.story.TitleShortCap).Colorize(ColoredText.SubtleGrayColor);
			}
		}

		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x060022E2 RID: 8930 RVA: 0x000CD919 File Offset: 0x000CBB19
		public override string LabelShort
		{
			get
			{
				if (this.Name != null)
				{
					return this.LabelPrefix + this.Name.ToStringShort;
				}
				return this.LabelNoCount;
			}
		}

		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x060022E3 RID: 8931 RVA: 0x000CD940 File Offset: 0x000CBB40
		public TaggedString LabelNoCountColored
		{
			get
			{
				if (this.Name == null)
				{
					return this.LabelPrefix + this.KindLabel;
				}
				if (this.story == null || this.story.TitleShortCap.NullOrEmpty() || this.IsSubhuman)
				{
					return this.LabelPrefix + this.Name.ToStringShort.Colorize(ColoredText.NameColor);
				}
				return this.LabelPrefix + this.Name.ToStringShort.Colorize(ColoredText.NameColor) + (", " + this.story.TitleShortCap).Colorize(ColoredText.SubtleGrayColor);
			}
		}

		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x060022E4 RID: 8932 RVA: 0x000CD9FC File Offset: 0x000CBBFC
		public TaggedString NameShortColored
		{
			get
			{
				if (this.Name != null)
				{
					return this.LabelPrefix + this.Name.ToStringShort.Colorize(ColoredText.NameColor);
				}
				return this.LabelPrefix + this.KindLabel;
			}
		}

		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x060022E5 RID: 8933 RVA: 0x000CDA50 File Offset: 0x000CBC50
		public TaggedString NameFullColored
		{
			get
			{
				if (this.Name != null)
				{
					return this.LabelPrefix + this.Name.ToStringFull.Colorize(ColoredText.NameColor);
				}
				return this.LabelPrefix + this.KindLabel;
			}
		}

		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x060022E6 RID: 8934 RVA: 0x000CDAA4 File Offset: 0x000CBCA4
		public TaggedString LegalStatus
		{
			get
			{
				if (this.IsSlave)
				{
					return "Slave".Translate().CapitalizeFirst();
				}
				if (base.Faction != null)
				{
					return new TaggedString(base.Faction.def.pawnSingular);
				}
				return "Colonist".Translate();
			}
		}

		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x060022E7 RID: 8935 RVA: 0x000CDAF4 File Offset: 0x000CBCF4
		public float TicksPerMoveCardinal
		{
			get
			{
				return this.TicksPerMove(false);
			}
		}

		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x060022E8 RID: 8936 RVA: 0x000CDAFD File Offset: 0x000CBCFD
		public float TicksPerMoveDiagonal
		{
			get
			{
				return this.TicksPerMove(true);
			}
		}

		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x060022E9 RID: 8937 RVA: 0x000CDB06 File Offset: 0x000CBD06
		public override string DescriptionDetailed
		{
			get
			{
				return this.DescriptionFlavor;
			}
		}

		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x060022EA RID: 8938 RVA: 0x000CDB10 File Offset: 0x000CBD10
		public override string DescriptionFlavor
		{
			get
			{
				if (ModsConfig.AnomalyActive && this.IsSubhuman && !this.mutant.Def.description.NullOrEmpty())
				{
					return this.mutant.Def.description;
				}
				if (this.IsBaseliner())
				{
					return this.def.description;
				}
				string t;
				if (this.genes.Xenotype == XenotypeDefOf.Baseliner)
				{
					t = ((this.genes.CustomXenotype == null) ? this.genes.Xenotype.description : "UniqueXenotypeDesc".Translate());
				}
				else
				{
					t = this.genes.Xenotype.description;
				}
				return "StatsReport_NonBaselinerDescription".Translate(this.genes.XenotypeLabel) + "\n\n" + t;
			}
		}

		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x060022EB RID: 8939 RVA: 0x000CDBEB File Offset: 0x000CBDEB
		public override IEnumerable<DefHyperlink> DescriptionHyperlinks
		{
			get
			{
				Pawn.<get_DescriptionHyperlinks>d__262 <get_DescriptionHyperlinks>d__ = new Pawn.<get_DescriptionHyperlinks>d__262(-2);
				<get_DescriptionHyperlinks>d__.<>4__this = this;
				return <get_DescriptionHyperlinks>d__;
			}
		}

		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x060022EC RID: 8940 RVA: 0x000CDBFC File Offset: 0x000CBDFC
		public Pawn_DrawTracker Drawer
		{
			get
			{
				Pawn_DrawTracker result;
				if ((result = this.drawer) == null)
				{
					result = (this.drawer = new Pawn_DrawTracker(this));
				}
				return result;
			}
		}

		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x060022ED RID: 8941 RVA: 0x000CDC24 File Offset: 0x000CBE24
		public Faction HomeFaction
		{
			get
			{
				if (base.Faction == null || !base.Faction.IsPlayer)
				{
					return base.Faction;
				}
				if (this.IsSlave && this.SlaveFaction != null)
				{
					return this.SlaveFaction;
				}
				if (this.HasExtraMiniFaction(null))
				{
					return this.GetExtraMiniFaction(null);
				}
				return this.GetExtraHomeFaction(null) ?? base.Faction;
			}
		}

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x060022EE RID: 8942 RVA: 0x000CDC86 File Offset: 0x000CBE86
		public bool Deathresting
		{
			get
			{
				return ModsConfig.BiotechActive && this.health.hediffSet.HasHediff(HediffDefOf.Deathrest, false);
			}
		}

		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x060022EF RID: 8943 RVA: 0x000CDCA7 File Offset: 0x000CBEA7
		public bool HasDeathRefusalOrResurrecting
		{
			get
			{
				return ModsConfig.AnomalyActive && (this.health.hediffSet.HasHediff<Hediff_DeathRefusal>(false) || this.health.hediffSet.HasHediff(HediffDefOf.Rising, false));
			}
		}

		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x060022F0 RID: 8944 RVA: 0x000CDCDD File Offset: 0x000CBEDD
		public override bool Suspended
		{
			get
			{
				return base.Suspended || Find.WorldPawns.GetSituation(this) == WorldPawnSituation.ReservedByQuest;
			}
		}

		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x060022F1 RID: 8945 RVA: 0x000CDCFB File Offset: 0x000CBEFB
		public Faction DeadlifeDustFaction
		{
			get
			{
				if (GenTicks.TicksGame - this.deadlifeDustFactionTick < 12500 && this.deadlifeDustFaction != null)
				{
					return this.deadlifeDustFaction;
				}
				return Faction.OfPlayer;
			}
		}

		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x060022F2 RID: 8946 RVA: 0x000CDD24 File Offset: 0x000CBF24
		public bool HasPsylink
		{
			get
			{
				Pawn_PsychicEntropyTracker pawn_PsychicEntropyTracker = this.psychicEntropy;
				return ((pawn_PsychicEntropyTracker != null) ? pawn_PsychicEntropyTracker.Psylink : null) != null;
			}
		}

		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x060022F3 RID: 8947 RVA: 0x000CDD3B File Offset: 0x000CBF3B
		public CompOverseerSubject OverseerSubject
		{
			get
			{
				if (ModsConfig.BiotechActive && this.overseerSubject == null && this.RaceProps.IsMechanoid)
				{
					this.overseerSubject = base.GetComp<CompOverseerSubject>();
				}
				return this.overseerSubject;
			}
		}

		// Token: 0x060022F4 RID: 8948 RVA: 0x000CDD6B File Offset: 0x000CBF6B
		public virtual bool ShouldShowQuestionMark()
		{
			if (ModsConfig.AnomalyActive && this.creepjoiner != null)
			{
				return this.creepjoiner.IsOnEntryLord;
			}
			return this.CanTradeNow;
		}

		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x060022F5 RID: 8949 RVA: 0x000CDD8E File Offset: 0x000CBF8E
		public override int UpdateRateTicks
		{
			get
			{
				if (!this.RaceProps.Animal)
				{
					return base.UpdateRateTicks;
				}
				return 15;
			}
		}

		// Token: 0x060022F6 RID: 8950 RVA: 0x000CDDA6 File Offset: 0x000CBFA6
		public string GetKindLabelSingular()
		{
			return GenLabel.BestKindLabel(this, false, false, false, -1);
		}

		// Token: 0x060022F7 RID: 8951 RVA: 0x000CDDB2 File Offset: 0x000CBFB2
		public string GetKindLabelPlural(int count = -1)
		{
			return GenLabel.BestKindLabel(this, false, false, true, count);
		}

		// Token: 0x060022F8 RID: 8952 RVA: 0x000CDDBE File Offset: 0x000CBFBE
		public static void ResetStaticData()
		{
			Pawn.NotSurgeryReadyTrans = "NotSurgeryReady".Translate();
			Pawn.CannotReachTrans = "CannotReach".Translate();
		}

		// Token: 0x060022F9 RID: 8953 RVA: 0x000CDDE8 File Offset: 0x000CBFE8
		public override void Notify_DefsHotReloaded()
		{
			base.Notify_DefsHotReloaded();
			this.Drawer.renderer.SetAllGraphicsDirty();
		}

		// Token: 0x060022FA RID: 8954 RVA: 0x000CDE00 File Offset: 0x000CC000
		public override void Notify_Unfogged()
		{
			base.Notify_Unfogged();
			if (this.mindState == null || this.mindState.Active)
			{
				return;
			}
			this.mindState.Active = true;
			if (FloodFillerFog.TryDeferPawnUnfoggedWake(this))
			{
				return;
			}
			Lord lord = this.GetLord();
			LordJob_SleepThenAssaultColony lordJob_SleepThenAssaultColony = ((lord != null) ? lord.LordJob : null) as LordJob_SleepThenAssaultColony;
			if (lordJob_SleepThenAssaultColony != null)
			{
				lordJob_SleepThenAssaultColony.Notify_PawnUnfogged();
			}
		}

		// Token: 0x060022FB RID: 8955 RVA: 0x000CDE5F File Offset: 0x000CC05F
		public void MarkDeadlifeDustForFaction(Faction faction)
		{
			this.deadlifeDustFaction = faction;
			this.deadlifeDustFactionTick = GenTicks.TicksGame;
		}

		// Token: 0x060022FC RID: 8956 RVA: 0x000CDE74 File Offset: 0x000CC074
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (this.Dead)
			{
				Log.Warning("Tried to spawn Dead Pawn " + this.ToStringSafe<Pawn>() + ". Replacing with corpse.");
				Corpse corpse = (Corpse)ThingMaker.MakeThing(this.RaceProps.corpseDef, null);
				corpse.InnerPawn = this;
				GenSpawn.Spawn(corpse, base.Position, map, WipeMode.Vanish);
				return;
			}
			if (this.def == null || this.kindDef == null)
			{
				Log.Warning("Tried to spawn pawn without def " + this.ToStringSafe<Pawn>() + ".");
				return;
			}
			base.SpawnSetup(map, respawningAfterLoad);
			if (Find.WorldPawns.Contains(this))
			{
				Find.WorldPawns.RemovePawn(this);
			}
			PawnComponentsUtility.AddComponentsForSpawn(this);
			if (!PawnUtility.InValidState(this))
			{
				Log.Error("Pawn " + this.ToStringSafe<Pawn>() + " spawned in invalid state. Destroying...");
				try
				{
					this.DeSpawn(DestroyMode.Vanish);
				}
				catch (Exception ex)
				{
					string str = "Tried to despawn ";
					string str2 = this.ToStringSafe<Pawn>();
					string str3 = " because of the previous error but couldn't: ";
					Exception ex2 = ex;
					Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
				}
				Find.WorldPawns.PassToWorld(this, PawnDiscardDecideMode.Discard);
				return;
			}
			this.Drawer.Notify_Spawned();
			this.rotationTracker.Notify_Spawned();
			if (!respawningAfterLoad)
			{
				this.pather.ResetToCurrentPosition();
			}
			base.Map.mapPawns.RegisterPawn(this);
			base.Map.autoSlaughterManager.Notify_PawnSpawned();
			if (this.relations != null)
			{
				this.relations.everSeenByPlayer = true;
			}
			AddictionUtility.CheckDrugAddictionTeachOpportunity(this);
			Pawn_NeedsTracker pawn_NeedsTracker = this.needs;
			if (pawn_NeedsTracker != null)
			{
				Need_Mood mood = pawn_NeedsTracker.mood;
				if (mood != null)
				{
					PawnRecentMemory recentMemory = mood.recentMemory;
					if (recentMemory != null)
					{
						recentMemory.Notify_Spawned(respawningAfterLoad);
					}
				}
			}
			Pawn_EquipmentTracker pawn_EquipmentTracker = this.equipment;
			if (pawn_EquipmentTracker != null)
			{
				pawn_EquipmentTracker.Notify_PawnSpawned();
			}
			Pawn_HealthTracker pawn_HealthTracker = this.health;
			if (pawn_HealthTracker != null)
			{
				pawn_HealthTracker.Notify_Spawned();
			}
			Pawn_MechanitorTracker pawn_MechanitorTracker = this.mechanitor;
			if (pawn_MechanitorTracker != null)
			{
				pawn_MechanitorTracker.Notify_PawnSpawned(respawningAfterLoad);
			}
			Pawn_MutantTracker pawn_MutantTracker = this.mutant;
			if (pawn_MutantTracker != null)
			{
				pawn_MutantTracker.Notify_Spawned(respawningAfterLoad);
			}
			Pawn_InfectionVectorTracker pawn_InfectionVectorTracker = this.infectionVectors;
			if (pawn_InfectionVectorTracker != null)
			{
				pawn_InfectionVectorTracker.NotifySpawned(respawningAfterLoad);
			}
			if (base.Faction == Faction.OfPlayer)
			{
				Ideo ideo = this.Ideo;
				if (ideo != null)
				{
					ideo.RecacheColonistBelieverCount();
				}
			}
			if (!respawningAfterLoad)
			{
				if ((base.Faction == Faction.OfPlayer || this.IsPlayerControlled) && base.Position.Fogged(map))
				{
					FloodFillerFog.FloodUnfog(base.Position, map);
				}
				Find.GameEnder.CheckOrUpdateGameOver();
				if (base.Faction == Faction.OfPlayer)
				{
					Find.StoryWatcher.statsRecord.UpdateGreatestPopulation();
					Find.World.StoryState.RecordPopulationIncrease();
				}
				if (!this.IsSubhuman)
				{
					PawnDiedOrDownedThoughtsUtility.RemoveDiedThoughts(this);
				}
				if (this.IsQuestLodger())
				{
					for (int i = this.health.hediffSet.hediffs.Count - 1; i >= 0; i--)
					{
						if (this.health.hediffSet.hediffs[i].def.removeOnQuestLodgers)
						{
							this.health.RemoveHediff(this.health.hediffSet.hediffs[i]);
						}
					}
				}
			}
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				if (this.IsPlayerControlled && base.PositionHeld.Fogged(base.Map))
				{
					FloodFillerFog.FloodUnfog(base.PositionHeld, base.Map);
				}
			});
			if (this.RaceProps.soundAmbience != null)
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.sustainerAmbient = this.RaceProps.soundAmbience.TrySpawnSustainer(SoundInfo.InMap(this, MaintenanceType.PerTick));
				});
			}
			if (this.RaceProps.soundMoving != null)
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.sustainerMoving = this.RaceProps.soundMoving.TrySpawnSustainer(SoundInfo.InMap(this, MaintenanceType.PerTick));
				});
			}
			if (this.Ideo != null && this.Ideo.hidden)
			{
				this.Ideo.hidden = false;
			}
		}

		// Token: 0x060022FD RID: 8957 RVA: 0x000CE1E4 File Offset: 0x000CC3E4
		public override void PostMake()
		{
			base.PostMake();
			this.canBeDormant = base.GetComp<CompCanBeDormant>();
			this.activity = base.GetComp<CompActivity>();
		}

		// Token: 0x060022FE RID: 8958 RVA: 0x000CE204 File Offset: 0x000CC404
		public override void PostMapInit()
		{
			base.PostMapInit();
			this.pather.TryResumePathingAfterLoading();
		}

		// Token: 0x060022FF RID: 8959 RVA: 0x000CE217 File Offset: 0x000CC417
		public void DrawShadowAt(Vector3 drawLoc)
		{
			this.Drawer.DrawShadowAt(drawLoc);
		}

		// Token: 0x06002300 RID: 8960 RVA: 0x000CE228 File Offset: 0x000CC428
		public override void DynamicDrawPhaseAt(DrawPhase phase, Vector3 drawLoc, bool flip = false)
		{
			base.DynamicDrawPhaseAt(phase, drawLoc, flip);
			this.Drawer.renderer.DynamicDrawPhaseAt(phase, drawLoc, null, false);
		}

		// Token: 0x06002301 RID: 8961 RVA: 0x000CE25A File Offset: 0x000CC45A
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.Comps_PostDraw();
			Pawn_MechanitorTracker pawn_MechanitorTracker = this.mechanitor;
			if (pawn_MechanitorTracker == null)
			{
				return;
			}
			pawn_MechanitorTracker.DrawCommandRadius();
		}

		// Token: 0x06002302 RID: 8962 RVA: 0x000CE274 File Offset: 0x000CC474
		public override void DrawGUIOverlay()
		{
			this.Drawer.ui.DrawPawnGUIOverlay();
			for (int i = 0; i < base.AllComps.Count; i++)
			{
				base.AllComps[i].DrawGUIOverlay();
			}
			SilhouetteUtility.DrawGUISilhouette(this);
			if (DebugViewSettings.drawPatherState)
			{
				this.pather.DrawDebugGUI();
			}
		}

		// Token: 0x06002303 RID: 8963 RVA: 0x000CE2D0 File Offset: 0x000CC4D0
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (this.IsPlayerControlled)
			{
				PawnPath curPath = this.pather.curPath;
				if (curPath != null)
				{
					curPath.DrawPath(this);
				}
				this.jobs.DrawLinesBetweenTargets();
			}
		}

		// Token: 0x06002304 RID: 8964 RVA: 0x000CE304 File Offset: 0x000CC504
		public override void TickRare()
		{
			base.TickRare();
			if (!this.Suspended)
			{
				Pawn_ApparelTracker pawn_ApparelTracker = this.apparel;
				if (pawn_ApparelTracker != null)
				{
					pawn_ApparelTracker.ApparelTrackerTickRare();
				}
			}
			Pawn_TrainingTracker pawn_TrainingTracker = this.training;
			if (pawn_TrainingTracker != null)
			{
				pawn_TrainingTracker.TrainingTrackerTickRare();
			}
			if (base.Spawned && this.RaceProps.IsFlesh && base.AmbientTemperature < 40f)
			{
				GenTemperature.PushHeat(this, 0.3f * this.BodySize * 4.1666665f * (this.def.race.Humanlike ? 1f : 0.6f));
			}
		}

		// Token: 0x06002305 RID: 8965 RVA: 0x000CE39C File Offset: 0x000CC59C
		protected override void Tick()
		{
			if (DebugSettings.noAnimals && base.Spawned && this.IsAnimal)
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			base.Tick();
			if (this.IsHashIntervalTick(250))
			{
				this.TickRare();
			}
			bool suspended = this.Suspended;
			if (!suspended)
			{
				if (base.Spawned)
				{
					this.pather.PatherTick();
				}
				if (base.Spawned)
				{
					this.verbTracker.VerbsTick();
				}
				if (base.Spawned)
				{
					Pawn_RopeTracker pawn_RopeTracker = this.roping;
					if (pawn_RopeTracker != null)
					{
						pawn_RopeTracker.RopingTick();
					}
					Pawn_FlightTracker pawn_FlightTracker = this.flight;
					if (pawn_FlightTracker != null)
					{
						pawn_FlightTracker.FlightTick();
					}
					this.natives.NativeVerbsTick();
				}
				if (base.Spawned)
				{
					this.stances.StanceTrackerTick();
				}
				if (!this.IsWorldPawn())
				{
					Pawn_JobTracker pawn_JobTracker = this.jobs;
					if (pawn_JobTracker != null)
					{
						pawn_JobTracker.JobTrackerTick();
					}
				}
				this.health.HealthTick();
				if (base.Spawned && this.IsHiddenFromPlayer() && Find.Selector.IsSelected(this))
				{
					Find.Selector.Deselect(this);
				}
			}
			if (!suspended)
			{
				if (this.equipment != null)
				{
					using (ProfilerBlock.Scope("equipment"))
					{
						this.equipment.EquipmentTrackerTick();
					}
				}
				Pawn_AbilityTracker pawn_AbilityTracker = this.abilities;
				if (pawn_AbilityTracker != null)
				{
					pawn_AbilityTracker.AbilitiesTick();
				}
				Pawn_InventoryTracker pawn_InventoryTracker = this.inventory;
				if (pawn_InventoryTracker != null)
				{
					pawn_InventoryTracker.InventoryTrackerTick();
				}
				Pawn_GeneTracker pawn_GeneTracker = this.genes;
				if (pawn_GeneTracker != null)
				{
					pawn_GeneTracker.GeneTrackerTick();
				}
				if (ModsConfig.AnomalyActive && base.Spawned)
				{
					Pawn_MutantTracker pawn_MutantTracker = this.mutant;
					if (pawn_MutantTracker != null)
					{
						pawn_MutantTracker.MutantTrackerTick();
					}
					BloodRainUtility.BloodRainTick(this);
				}
			}
			if (base.Spawned && !base.Position.Fogged(base.Map))
			{
				if (this.RaceProps.soundAmbience != null && (this.sustainerAmbient == null || this.sustainerAmbient.Ended))
				{
					this.sustainerAmbient = this.RaceProps.soundAmbience.TrySpawnSustainer(SoundInfo.InMap(this, MaintenanceType.PerTick));
				}
				Sustainer sustainer = this.sustainerAmbient;
				if (sustainer != null)
				{
					sustainer.Maintain();
				}
				if (this.pather != null && this.pather.Moving && this.RaceProps.soundMoving != null)
				{
					if (this.sustainerMoving == null || this.sustainerMoving.Ended)
					{
						this.sustainerMoving = this.RaceProps.soundMoving.TrySpawnSustainer(SoundInfo.InMap(this, MaintenanceType.PerTick));
					}
					Sustainer sustainer2 = this.sustainerMoving;
					if (sustainer2 != null)
					{
						sustainer2.Maintain();
					}
				}
			}
			Pawn_DrawTracker pawn_DrawTracker = this.drawer;
			if (pawn_DrawTracker == null)
			{
				return;
			}
			pawn_DrawTracker.renderer.EffectersTick(suspended || this.IsWorldPawn());
		}

		// Token: 0x06002306 RID: 8966 RVA: 0x000CE648 File Offset: 0x000CC848
		protected override void TickInterval(int delta)
		{
			if (DebugSettings.noAnimals && base.Spawned && this.IsAnimal)
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			base.TickInterval(delta);
			bool suspended = this.Suspended;
			if (!suspended)
			{
				if (!this.IsWorldPawn())
				{
					using (ProfilerBlock.Scope("jobs interval"))
					{
						Pawn_JobTracker pawn_JobTracker = this.jobs;
						if (pawn_JobTracker != null)
						{
							pawn_JobTracker.JobTrackerTickInterval(delta);
						}
					}
				}
				using (ProfilerBlock.Scope("health interval"))
				{
					this.health.HealthTickInterval(delta);
				}
				if (!this.Dead)
				{
					using (ProfilerBlock.Scope("mind state interval"))
					{
						this.mindState.MindStateTickInterval(delta);
					}
					this.carryTracker.CarryHandsTickInterval(delta);
					if (!base.InCryptosleep && this.RaceProps.Humanlike)
					{
						Pawn_InfectionVectorTracker pawn_InfectionVectorTracker = this.infectionVectors;
						if (pawn_InfectionVectorTracker != null)
						{
							pawn_InfectionVectorTracker.InfectionTickInterval(delta);
						}
					}
					if (this.showNamePromptOnTick != -1 && this.showNamePromptOnTick == Find.TickManager.TicksGame)
					{
						Find.WindowStack.Add(this.NamePawnDialog(null));
					}
				}
			}
			if (!base.Spawned)
			{
				Thing firstParentThing = ThingOwnerUtility.GetFirstParentThing(this);
				if (firstParentThing != null)
				{
					PawnUtility.GainComfortFromThingIfPossible(this, firstParentThing, delta);
				}
			}
			if (!this.Dead)
			{
				this.needs.NeedsTrackerTickInterval(delta);
			}
			if (!suspended)
			{
				Pawn_ApparelTracker pawn_ApparelTracker = this.apparel;
				if (pawn_ApparelTracker != null)
				{
					pawn_ApparelTracker.ApparelTrackerTickInterval(delta);
				}
				if (this.interactions != null && base.Spawned)
				{
					using (ProfilerBlock.Scope("interactions"))
					{
						this.interactions.InteractionsTrackerTickInterval(delta);
					}
				}
				Pawn_CallTracker pawn_CallTracker = this.caller;
				if (pawn_CallTracker != null)
				{
					pawn_CallTracker.CallTrackerTickInterval(delta);
				}
				Pawn_SkillTracker pawn_SkillTracker = this.skills;
				if (pawn_SkillTracker != null)
				{
					pawn_SkillTracker.SkillsTickInterval(delta);
				}
				Pawn_DraftController pawn_DraftController = this.drafter;
				if (pawn_DraftController != null)
				{
					pawn_DraftController.DraftControllerTickInterval(delta);
				}
				Pawn_RelationsTracker pawn_RelationsTracker = this.relations;
				if (pawn_RelationsTracker != null)
				{
					pawn_RelationsTracker.RelationsTrackerTickInterval(delta);
				}
				if (ModsConfig.RoyaltyActive && this.psychicEntropy != null)
				{
					this.psychicEntropy.PsychicEntropyTrackerTickInterval(delta);
				}
				if (this.RaceProps.Humanlike)
				{
					this.guest.GuestTrackerTickInterval(delta);
				}
				Pawn_IdeoTracker pawn_IdeoTracker = this.ideo;
				if (pawn_IdeoTracker != null)
				{
					pawn_IdeoTracker.IdeoTrackerTickInterval(delta);
				}
				Pawn_GeneTracker pawn_GeneTracker = this.genes;
				if (pawn_GeneTracker != null)
				{
					pawn_GeneTracker.GeneTrackerTickInterval(delta);
				}
				if (this.royalty != null && ModsConfig.RoyaltyActive)
				{
					this.royalty.RoyaltyTrackerTickInterval(delta);
				}
				if (this.style != null && ModsConfig.IdeologyActive)
				{
					this.style.StyleTrackerTickInterval(delta);
				}
				if (this.styleObserver != null && ModsConfig.IdeologyActive)
				{
					this.styleObserver.StyleObserverTickInterval(delta);
				}
				if (this.surroundings != null && ModsConfig.IdeologyActive)
				{
					this.surroundings.SurroundingsTrackerTickInterval(delta);
				}
				if (ModsConfig.BiotechActive)
				{
					Pawn_LearningTracker pawn_LearningTracker = this.learning;
					if (pawn_LearningTracker != null)
					{
						pawn_LearningTracker.LearningTickInterval(delta);
					}
					PollutionUtility.PawnPollutionTickInterval(this, delta);
				}
				GasUtility.PawnGasEffectsTickInterval(this, delta);
				ToxicUtility.PawnToxicTickInterval(this, delta);
				VacuumUtility.PawnVacuumTickInterval(this, delta);
				if (ModsConfig.AnomalyActive && base.Spawned)
				{
					Pawn_MutantTracker pawn_MutantTracker = this.mutant;
					if (pawn_MutantTracker != null)
					{
						pawn_MutantTracker.MutantTrackerTickInterval(delta);
					}
					Pawn_CreepJoinerTracker pawn_CreepJoinerTracker = this.creepjoiner;
					if (pawn_CreepJoinerTracker != null)
					{
						pawn_CreepJoinerTracker.TickInterval(delta);
					}
				}
				if (!this.IsMutant || !this.mutant.Def.disableAging)
				{
					this.ageTracker.AgeTickInterval(delta);
				}
				this.records.RecordsTickInterval(delta);
			}
			Pawn_GuiltTracker pawn_GuiltTracker = this.guilt;
			if (pawn_GuiltTracker == null)
			{
				return;
			}
			pawn_GuiltTracker.GuiltTrackerTickInterval(delta);
		}

		// Token: 0x06002307 RID: 8967 RVA: 0x000CE9E0 File Offset: 0x000CCBE0
		public void ProcessPostTickVisuals(int ticksPassed, CellRect viewRect)
		{
			if (!this.Suspended && base.Spawned)
			{
				if (Current.ProgramState != ProgramState.Playing || viewRect.Contains(base.Position))
				{
					this.Drawer.ProcessPostTickVisuals(ticksPassed);
				}
				this.rotationTracker.ProcessPostTickVisuals(ticksPassed);
			}
		}

		// Token: 0x06002308 RID: 8968 RVA: 0x000CEA2C File Offset: 0x000CCC2C
		public void TickMothballed(int interval)
		{
			if (!this.Suspended)
			{
				this.ageTracker.AgeTickMothballed(interval);
				this.records.RecordsTickMothballed(interval);
			}
		}

		// Token: 0x06002309 RID: 8969 RVA: 0x000CEA50 File Offset: 0x000CCC50
		public void Notify_Teleported(bool endCurrentJob = true, bool resetTweenedPos = true)
		{
			if (resetTweenedPos)
			{
				this.Drawer.tweener.Notify_Teleported();
			}
			this.pather.Notify_Teleported_Int();
			if (endCurrentJob)
			{
				Pawn_JobTracker pawn_JobTracker = this.jobs;
				if (((pawn_JobTracker != null) ? pawn_JobTracker.curJob : null) != null)
				{
					this.jobs.EndCurrentJob(JobCondition.InterruptForced, this.jobs.curJob.startTick != GenTicks.TicksGame, true);
				}
			}
		}

		// Token: 0x0600230A RID: 8970 RVA: 0x000CEABC File Offset: 0x000CCCBC
		public virtual SurgicalInspectionOutcome DoSurgicalInspection(Pawn surgeon, out string desc)
		{
			if (!ModsConfig.AnomalyActive)
			{
				desc = "";
				return SurgicalInspectionOutcome.Nothing;
			}
			bool flag = false;
			bool flag2 = false;
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = this.health.hediffSet.hediffs.Count - 1; i >= 0; i--)
			{
				Hediff hediff = this.health.hediffSet.hediffs[i];
				HediffComp_SurgeryInspectable hediffComp_SurgeryInspectable;
				if (hediff.TryGetComp(out hediffComp_SurgeryInspectable))
				{
					if (hediff.Visible)
					{
						hediffComp_SurgeryInspectable.DoSurgicalInspectionVisible(surgeon);
						if (hediffComp_SurgeryInspectable.Props.preventLetterIfPreviouslyDetected)
						{
							flag2 = true;
						}
					}
					else
					{
						SurgicalInspectionOutcome surgicalInspectionOutcome = hediffComp_SurgeryInspectable.DoSurgicalInspection(surgeon);
						if (surgicalInspectionOutcome == SurgicalInspectionOutcome.DetectedNoLetter)
						{
							flag2 = true;
							hediff.SetVisible();
						}
						else if (surgicalInspectionOutcome == SurgicalInspectionOutcome.Detected)
						{
							flag = true;
							if (!string.IsNullOrEmpty(hediffComp_SurgeryInspectable.Props.surgicalDetectionDesc))
							{
								stringBuilder.Append("\n\n" + hediffComp_SurgeryInspectable.Props.surgicalDetectionDesc.Formatted(this.Named("PAWN"), surgeon.Named("SURGEON")));
							}
							hediff.SetVisible();
						}
					}
				}
			}
			if (this.IsCreepJoiner && this.creepjoiner.DoSurgicalInspection(surgeon, stringBuilder))
			{
				flag = true;
			}
			desc = stringBuilder.ToString();
			if (flag2)
			{
				return SurgicalInspectionOutcome.DetectedNoLetter;
			}
			if (!flag)
			{
				return SurgicalInspectionOutcome.Nothing;
			}
			return SurgicalInspectionOutcome.Detected;
		}

		// Token: 0x0600230B RID: 8971 RVA: 0x000CEBF8 File Offset: 0x000CCDF8
		public void Notify_BecameVisible()
		{
			List<ThingComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				allComps[i].Notify_BecameVisible();
			}
		}

		// Token: 0x0600230C RID: 8972 RVA: 0x000CEC2C File Offset: 0x000CCE2C
		public void Notify_BecameInvisible()
		{
			List<ThingComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				allComps[i].Notify_BecameInvisible();
			}
		}

		// Token: 0x0600230D RID: 8973 RVA: 0x000CEC60 File Offset: 0x000CCE60
		public void Notify_ForcedVisible()
		{
			List<ThingComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				allComps[i].Notify_ForcedVisible();
			}
		}

		// Token: 0x0600230E RID: 8974 RVA: 0x000CEC94 File Offset: 0x000CCE94
		public void Notify_PassedToWorld()
		{
			if (((base.Faction == null && this.RaceProps.Humanlike) || (base.Faction != null && base.Faction.IsPlayer) || base.Faction == Faction.OfAncients || base.Faction == Faction.OfAncientsHostile) && !this.Dead && Find.WorldPawns.GetSituation(this) == WorldPawnSituation.Free)
			{
				bool tryMedievalOrBetter = base.Faction != null && base.Faction.def.techLevel >= TechLevel.Medieval;
				Faction faction;
				if (this.HasExtraHomeFaction(null) && !this.GetExtraHomeFaction(null).IsPlayer)
				{
					if (base.Faction != this.GetExtraHomeFaction(null))
					{
						this.SetFaction(this.GetExtraHomeFaction(null), null);
					}
				}
				else if (Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out faction, tryMedievalOrBetter, false, TechLevel.Undefined, TechLevel.Undefined, false, false))
				{
					if (base.Faction != faction)
					{
						this.SetFaction(faction, null);
					}
				}
				else if (Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out faction, tryMedievalOrBetter, true, TechLevel.Undefined, TechLevel.Undefined, false, false))
				{
					if (base.Faction != faction)
					{
						this.SetFaction(faction, null);
					}
				}
				else if (base.Faction != null)
				{
					this.SetFaction(null, null);
				}
			}
			this.becameWorldPawnTickAbs = GenTicks.TicksAbs;
			if (!this.IsCaravanMember() && !PawnUtility.IsTravelingInTransportPodWorldObject(this))
			{
				this.ClearMind_NewTemp(false, false, true, false);
			}
			Pawn_RelationsTracker pawn_RelationsTracker = this.relations;
			if (pawn_RelationsTracker != null)
			{
				pawn_RelationsTracker.Notify_PassedToWorld();
			}
			foreach (ThingComp thingComp in base.AllComps)
			{
				thingComp.Notify_PassedToWorld();
			}
			Pawn_DrawTracker pawn_DrawTracker = this.drawer;
			if (pawn_DrawTracker == null)
			{
				return;
			}
			PawnRenderer renderer = pawn_DrawTracker.renderer;
			if (renderer == null)
			{
				return;
			}
			PawnRenderTree renderTree = renderer.renderTree;
			if (renderTree == null)
			{
				return;
			}
			renderTree.SetDirty();
		}

		// Token: 0x0600230F RID: 8975 RVA: 0x000CEE58 File Offset: 0x000CD058
		public override void Notify_LeftBehind()
		{
			base.Notify_LeftBehind();
			Pawn_RelationsTracker pawn_RelationsTracker = this.relations;
			if (pawn_RelationsTracker == null)
			{
				return;
			}
			pawn_RelationsTracker.Notify_PawnLeftBehind();
		}

		// Token: 0x06002310 RID: 8976 RVA: 0x000CEE70 File Offset: 0x000CD070
		public void Notify_AddBedThoughts()
		{
			foreach (ThingComp thingComp in base.AllComps)
			{
				thingComp.Notify_AddBedThoughts(this);
			}
			Ideo ideo = this.Ideo;
			if (ideo == null)
			{
				return;
			}
			ideo.Notify_AddBedThoughts(this);
		}

		// Token: 0x06002311 RID: 8977 RVA: 0x000CEED4 File Offset: 0x000CD0D4
		public override void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			float num = 1f;
			if (ModsConfig.BiotechActive && this.genes != null)
			{
				num *= this.genes.FactorForDamage(dinfo);
			}
			num *= this.health.FactorForDamage(dinfo);
			dinfo.SetAmount(dinfo.Amount * num);
			base.PreApplyDamage(ref dinfo, out absorbed);
			if (absorbed)
			{
				return;
			}
			this.health.PreApplyDamage(dinfo, out absorbed);
		}

		// Token: 0x06002312 RID: 8978 RVA: 0x000CEF4C File Offset: 0x000CD14C
		public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			base.PostApplyDamage(dinfo, totalDamageDealt);
			if (dinfo.Def.ExternalViolenceFor(this))
			{
				this.records.AddTo(RecordDefOf.DamageTaken, totalDamageDealt);
			}
			if (dinfo.Def.makesBlood && this.health.CanBleed && !dinfo.InstantPermanentInjury && totalDamageDealt > 0f && Rand.Chance(0.5f))
			{
				this.health.DropBloodFilth();
			}
			this.health.PostApplyDamage(dinfo, totalDamageDealt);
			if (!this.Dead)
			{
				this.mindState.Notify_DamageTaken(dinfo);
				if (ModsConfig.AnomalyActive)
				{
					Pawn pawn = dinfo.Instigator as Pawn;
					if (pawn != null)
					{
						List<InfectionPathwayDef> list = dinfo.Def.isRanged ? pawn.kindDef.rangedAttackInfectionPathways : pawn.kindDef.meleeAttackInfectionPathways;
						if (list != null)
						{
							InfectionPathwayUtility.AddInfectionPathways(list, this, pawn);
						}
					}
				}
			}
		}

		// Token: 0x06002313 RID: 8979 RVA: 0x000CF02E File Offset: 0x000CD22E
		public override Thing SplitOff(int count)
		{
			if (count <= 0 || count >= this.stackCount)
			{
				return base.SplitOff(count);
			}
			throw new NotImplementedException("Split off on Pawns is not supported (unless we're taking a full stack).");
		}

		// Token: 0x06002314 RID: 8980 RVA: 0x000CF050 File Offset: 0x000CD250
		private float TicksPerMove(bool diagonal)
		{
			float num = this.GetStatValue(StatDefOf.MoveSpeed, true, -1);
			if (this.Downed && this.health.CanCrawl)
			{
				num = this.GetStatValue(StatDefOf.CrawlSpeed, true, -1);
			}
			if (RestraintsUtility.InRestraints(this))
			{
				num *= 0.35f;
			}
			Pawn_CarryTracker pawn_CarryTracker = this.carryTracker;
			if (((pawn_CarryTracker != null) ? pawn_CarryTracker.CarriedThing : null) != null && this.carryTracker.CarriedThing.def.category == ThingCategory.Pawn)
			{
				num *= 0.6f;
			}
			float num2 = num / 60f;
			float num3;
			if (num2 == 0f)
			{
				num3 = 450f;
			}
			else
			{
				num3 = 1f / num2;
				if (base.Spawned && !base.Map.roofGrid.Roofed(base.Position))
				{
					num3 /= base.Map.weatherManager.CurMoveSpeedMultiplier;
				}
				if (diagonal)
				{
					num3 *= 1.41421f;
				}
			}
			num3 = Mathf.Clamp(num3, 1f, 450f);
			if (this.debugMaxMoveSpeed)
			{
				return 1f;
			}
			return num3;
		}

		// Token: 0x06002315 RID: 8981 RVA: 0x000CF150 File Offset: 0x000CD350
		private void DoKillSideEffects(DamageInfo? dinfo, Hediff exactCulprit, bool spawned)
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				Find.Storyteller.Notify_PawnEvent(this, AdaptationEvent.Died, null);
			}
			if (this.IsColonist && !this.wasLeftBehindStartingPawn)
			{
				Find.StoryWatcher.statsRecord.Notify_ColonistKilled();
			}
			if (spawned && ((dinfo != null && dinfo.Value.Def.ExternalViolenceFor(this)) || (((exactCulprit != null) ? exactCulprit.sourceDef : null) != null && exactCulprit.sourceDef.IsWeapon)))
			{
				LifeStageUtility.PlayNearestLifestageSound(this, (LifeStageAge lifeStage) => lifeStage.soundDeath, (GeneDef gene) => gene.soundDeath, (MutantDef mutantDef) => mutantDef.soundDeath, 1f);
			}
			if (((dinfo != null) ? dinfo.GetValueOrDefault().Instigator : null) != null)
			{
				Pawn pawn = dinfo.Value.Instigator as Pawn;
				if (pawn != null)
				{
					RecordsUtility.Notify_PawnKilled(this, pawn);
					Pawn_EquipmentTracker pawn_EquipmentTracker = pawn.equipment;
					if (pawn_EquipmentTracker != null)
					{
						pawn_EquipmentTracker.Notify_KilledPawn();
					}
					Need_KillThirst need_KillThirst;
					if (this.RaceProps.Humanlike && pawn.needs != null && pawn.needs.TryGetNeed<Need_KillThirst>(out need_KillThirst))
					{
						need_KillThirst.Notify_KilledPawn(dinfo);
					}
					if (pawn.health.hediffSet != null)
					{
						for (int i = 0; i < pawn.health.hediffSet.hediffs.Count; i++)
						{
							pawn.health.hediffSet.hediffs[i].Notify_KilledPawn(this, dinfo);
						}
					}
					if (HistoryEventUtility.IsKillingInnocentAnimal(pawn, this))
					{
						Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.KilledInnocentAnimal, pawn.Named(HistoryEventArgsNames.Doer), this.Named(HistoryEventArgsNames.Victim)), true);
					}
				}
			}
			TaleUtility.Notify_PawnDied(this, dinfo);
			if (spawned)
			{
				Find.BattleLog.Add(new BattleLogEntry_StateTransition(this, this.RaceProps.DeathActionWorker.DeathRules, ((dinfo != null) ? dinfo.GetValueOrDefault().Instigator : null) as Pawn, exactCulprit, (dinfo != null) ? dinfo.GetValueOrDefault().HitPart : null));
			}
		}

		// Token: 0x06002316 RID: 8982 RVA: 0x000CF3AC File Offset: 0x000CD5AC
		private void PreDeathPawnModifications(DamageInfo? dinfo, Map map)
		{
			this.health.surgeryBills.Clear();
			for (int i = 0; i < this.health.hediffSet.hediffs.Count; i++)
			{
				this.health.hediffSet.hediffs[i].Notify_PawnKilled();
			}
			Pawn_ApparelTracker pawn_ApparelTracker = this.apparel;
			if (pawn_ApparelTracker != null)
			{
				pawn_ApparelTracker.Notify_PawnKilled(dinfo);
			}
			Pawn_RelationsTracker pawn_RelationsTracker = this.relations;
			if (pawn_RelationsTracker != null)
			{
				pawn_RelationsTracker.Notify_PawnKilled(dinfo, map);
			}
			Pawn_ConnectionsTracker pawn_ConnectionsTracker = this.connections;
			if (pawn_ConnectionsTracker != null)
			{
				pawn_ConnectionsTracker.Notify_PawnKilled();
			}
			this.meleeVerbs.Notify_PawnKilled();
		}

		// Token: 0x06002317 RID: 8983 RVA: 0x000CF448 File Offset: 0x000CD648
		private void DropBeforeDying(DamageInfo? dinfo, ref Map map, ref bool spawned)
		{
			Pawn_CarryTracker pawn_CarryTracker = base.ParentHolder as Pawn_CarryTracker;
			Thing thing;
			if (pawn_CarryTracker != null && this.holdingOwner.TryDrop(this, pawn_CarryTracker.pawn.Position, pawn_CarryTracker.pawn.Map, ThingPlaceMode.Near, out thing, null, null, true))
			{
				map = pawn_CarryTracker.pawn.Map;
				spawned = true;
			}
			PawnDiedOrDownedThoughtsUtility.RemoveLostThoughts(this);
			PawnDiedOrDownedThoughtsUtility.RemoveResuedRelativeThought(this);
			PawnDiedOrDownedThoughtsUtility.TryGiveThoughts(this, dinfo, PawnDiedOrDownedThoughtsKind.Died);
			if (this.IsAnimal)
			{
				PawnDiedOrDownedThoughtsUtility.GiveVeneratedAnimalDiedThoughts(this, map);
			}
		}

		// Token: 0x06002318 RID: 8984 RVA: 0x000CF4C4 File Offset: 0x000CD6C4
		private void RemoveFromHoldingContainer(ref Map map, ref bool spawned, DamageInfo? dinfo)
		{
			if (ModsConfig.AnomalyActive)
			{
				Building_HoldingPlatform building_HoldingPlatform = base.ParentHolder as Building_HoldingPlatform;
				if (building_HoldingPlatform != null && building_HoldingPlatform.Spawned)
				{
					building_HoldingPlatform.Notify_PawnDied(this, dinfo);
					spawned = true;
					map = building_HoldingPlatform.Map;
				}
			}
			CompTransporter compTransporter = base.ParentHolder as CompTransporter;
			if (compTransporter != null)
			{
				Thing thing;
				compTransporter.innerContainer.TryDrop(this, ThingPlaceMode.Near, out thing, null, null);
			}
		}

		// Token: 0x06002319 RID: 8985 RVA: 0x000CF524 File Offset: 0x000CD724
		public override void Kill(DamageInfo? dinfo, Hediff exactCulprit = null)
		{
			int num = 0;
			this.health.isBeingKilled = true;
			try
			{
				num = 1;
				IntVec3 positionHeld = base.PositionHeld;
				Map map = base.Map;
				Map map2 = this.prevMap = base.MapHeld;
				Lord prevLord = this.GetLord();
				bool spawned = base.Spawned;
				bool spawnedOrAnyParentSpawned = base.SpawnedOrAnyParentSpawned;
				bool wasWorldPawn = this.IsWorldPawn();
				Pawn_GuiltTracker pawn_GuiltTracker = this.guilt;
				bool? flag = (pawn_GuiltTracker != null) ? new bool?(pawn_GuiltTracker.IsGuilty) : null;
				Caravan caravan = this.GetCaravan();
				bool isShambler = this.IsShambler;
				Building_Grave assignedGrave = null;
				if (this.ownership != null)
				{
					assignedGrave = this.ownership.AssignedGrave;
				}
				Building_Bed currentBed = this.CurrentBed();
				this.RemoveFromHoldingContainer(ref map, ref spawned, dinfo);
				ThingOwner thingOwner = null;
				bool inContainerEnclosed = this.InContainerEnclosed;
				if (inContainerEnclosed)
				{
					thingOwner = this.holdingOwner;
					thingOwner.Remove(this);
				}
				bool flag2 = false;
				bool flag3 = false;
				bool flag4 = false;
				if (Current.ProgramState == ProgramState.Playing && map != null)
				{
					flag2 = (map.designationManager.DesignationOn(this, DesignationDefOf.Hunt) != null);
					flag3 = this.ShouldBeSlaughtered();
					foreach (Lord lord in map.lordManager.lords)
					{
						LordJob_Ritual lordJob_Ritual = lord.LordJob as LordJob_Ritual;
						if (lordJob_Ritual != null && lordJob_Ritual.pawnsDeathIgnored.Contains(this))
						{
							flag4 = true;
							break;
						}
					}
				}
				bool flag5 = PawnUtility.ShouldSendNotificationAbout(this) && ((!flag3 && !flag4) || dinfo == null || dinfo.Value.Def != DamageDefOf.ExecutionCut) && !this.ForceNoDeathNotification;
				num = 2;
				this.DoKillSideEffects(dinfo, exactCulprit, spawned);
				num = 3;
				this.PreDeathPawnModifications(dinfo, map);
				num = 4;
				this.DropBeforeDying(dinfo, ref map, ref spawned);
				num = 5;
				this.health.SetDead();
				if (this.health.deflectionEffecter != null)
				{
					this.health.deflectionEffecter.Cleanup();
					this.health.deflectionEffecter = null;
				}
				if (this.health.woundedEffecter != null)
				{
					this.health.woundedEffecter.Cleanup();
					this.health.woundedEffecter = null;
				}
				if (caravan != null)
				{
					caravan.Notify_MemberDied(this);
				}
				Lord lord2 = this.GetLord();
				if (lord2 != null)
				{
					lord2.Notify_PawnLost(this, PawnLostCondition.Killed, dinfo);
				}
				if (ModsConfig.AnomalyActive)
				{
					Find.Anomaly.Notify_PawnDied(this);
				}
				MeditationFocusTypeAvailabilityCache.Notify_PawnDiedOrDestroyed(this);
				bool flag6 = base.DeSpawnOrDeselect(DestroyMode.Vanish);
				if (this.royalty != null)
				{
					this.royalty.Notify_PawnKilled();
				}
				Corpse corpse = null;
				if (!PawnGenerator.IsPawnBeingGeneratedAndNotAllowsDead(this) && this.RaceProps.corpseDef != null)
				{
					if (inContainerEnclosed)
					{
						corpse = this.MakeCorpse(assignedGrave, currentBed);
						if (!thingOwner.TryAdd(corpse, true))
						{
							corpse.Destroy(DestroyMode.Vanish);
							corpse = null;
						}
					}
					else if (spawnedOrAnyParentSpawned)
					{
						if (this.holdingOwner != null)
						{
							this.holdingOwner.Remove(this);
						}
						corpse = this.MakeCorpse(assignedGrave, currentBed);
						if (GenPlace.TryPlaceThing(corpse, positionHeld, map2, ThingPlaceMode.Direct, null, null, null, 1) || GenPlace.TryPlaceThing(corpse, positionHeld, map2, ThingPlaceMode.Near, null, null, null, 1))
						{
							corpse.Rotation = base.Rotation;
							if (HuntJobUtility.WasKilledByHunter(this, dinfo))
							{
								((Pawn)dinfo.Value.Instigator).Reserve(corpse, ((Pawn)dinfo.Value.Instigator).CurJob, 1, -1, null, true, false);
							}
							else if (!flag2 && !flag3)
							{
								corpse.SetForbiddenIfOutsideHomeArea();
							}
							Fire fire = this.GetAttachment(ThingDefOf.Fire) as Fire;
							if (fire != null)
							{
								FireUtility.TryStartFireIn(corpse.Position, corpse.Map, fire.CurrentSize(), fire.instigator, null);
							}
						}
						else
						{
							corpse.Destroy(DestroyMode.Vanish);
							corpse = null;
						}
					}
					else if (caravan != null && caravan.Spawned)
					{
						corpse = this.MakeCorpse(assignedGrave, currentBed);
						caravan.AddPawnOrItem(corpse, true);
					}
					else if (this.holdingOwner != null || this.IsWorldPawn())
					{
						Corpse.PostCorpseDestroy(this, false);
					}
					else
					{
						corpse = this.MakeCorpse(assignedGrave, currentBed);
					}
				}
				if (spawned)
				{
					this.DropAndForbidEverything(false, false);
				}
				if (spawned)
				{
					GenLeaving.DoLeavingsFor(this, map, DestroyMode.KillFinalize, null);
				}
				if (corpse != null)
				{
					Hediff firstHediffOfDef = this.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.ToxicBuildup, false);
					Hediff firstHediffOfDef2 = this.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Scaria, false);
					CompRottable comp = corpse.GetComp<CompRottable>();
					if (comp != null && ((firstHediffOfDef != null && Rand.Value < firstHediffOfDef.Severity) || (firstHediffOfDef2 != null && Rand.Chance(Find.Storyteller.difficulty.scariaRotChance))))
					{
						comp.RotImmediately(RotStage.Rotting);
					}
					if (this.addCorpseToLord && lord2 != null)
					{
						lord2.AddCorpse(corpse);
					}
				}
				this.Drawer.renderer.SetAllGraphicsDirty();
				if (ModsConfig.AnomalyActive && this.kindDef == PawnKindDefOf.Revenant)
				{
					RevenantUtility.OnRevenantDeath(this, map);
				}
				Pawn_DuplicateTracker pawn_DuplicateTracker = this.duplicate;
				if (pawn_DuplicateTracker != null)
				{
					pawn_DuplicateTracker.Notify_PawnKilled();
				}
				this.Drawer.renderer.SetAnimation(null);
				if (!base.Destroyed)
				{
					base.Kill(dinfo, exactCulprit);
				}
				PawnComponentsUtility.RemoveComponentsOnKilled(this);
				this.health.hediffSet.DirtyCache();
				PortraitsCache.SetDirty(this);
				GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(this);
				if (flag6 && corpse != null && !corpse.Destroyed)
				{
					Find.Selector.Select(corpse, false, false);
				}
				num = 6;
				this.health.hediffSet.Notify_PawnDied(dinfo, exactCulprit);
				if (this.IsMutant)
				{
					this.mutant.Notify_Died(corpse, dinfo, exactCulprit);
				}
				Pawn_GeneTracker pawn_GeneTracker = this.genes;
				if (pawn_GeneTracker != null)
				{
					pawn_GeneTracker.Notify_PawnDied(dinfo, exactCulprit);
				}
				Faction homeFaction = this.HomeFaction;
				if (homeFaction != null)
				{
					homeFaction.Notify_MemberDied(this, dinfo, wasWorldPawn, flag.GetValueOrDefault(), map2);
				}
				if (corpse != null)
				{
					if (this.RaceProps.DeathActionWorker != null && spawned && !isShambler)
					{
						this.RaceProps.DeathActionWorker.PawnDied(corpse, prevLord);
					}
					if (Find.Scenario != null)
					{
						Find.Scenario.Notify_PawnDied(corpse);
					}
				}
				if (base.Faction != null && base.Faction.IsPlayer)
				{
					BillUtility.Notify_ColonistUnavailable(this);
				}
				if (spawnedOrAnyParentSpawned)
				{
					GenHostility.Notify_PawnLostForTutor(this, map2);
				}
				if (base.Faction != null && base.Faction.IsPlayer && Current.ProgramState == ProgramState.Playing)
				{
					Find.ColonistBar.MarkColonistsDirty();
				}
				Pawn_PsychicEntropyTracker pawn_PsychicEntropyTracker = this.psychicEntropy;
				if (pawn_PsychicEntropyTracker != null)
				{
					pawn_PsychicEntropyTracker.Notify_PawnDied();
				}
				try
				{
					Ideo ideo = this.Ideo;
					if (ideo != null)
					{
						ideo.Notify_MemberDied(this);
					}
					Ideo ideo2 = this.Ideo;
					if (ideo2 != null)
					{
						ideo2.Notify_MemberLost(this, map);
					}
				}
				catch (Exception ex)
				{
					string str = "Error while notifying ideo of pawn death: ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
				if (this.IsMutant && this.mutant.Def.clearMutantStatusOnDeath)
				{
					if (this.mutant.HasTurned)
					{
						this.mutant.Revert(true);
					}
					else
					{
						this.mutant = null;
					}
				}
				if (flag5)
				{
					this.health.NotifyPlayerOfKilled(dinfo, exactCulprit, caravan);
				}
				Find.QuestManager.Notify_PawnKilled(this, dinfo);
				Find.FactionManager.Notify_PawnKilled(this);
				Find.IdeoManager.Notify_PawnKilled(this);
				if (ModsConfig.BiotechActive && MechanitorUtility.IsMechanitor(this))
				{
					Find.History.Notify_MechanitorDied();
				}
				this.Notify_DisabledWorkTypesChanged();
				Find.BossgroupManager.Notify_PawnKilled(this);
				if (this.IsCreepJoiner)
				{
					this.creepjoiner.Notify_CreepJoinerKilled();
				}
				this.prevMap = null;
				this.health.isBeingKilled = false;
			}
			catch (Exception arg)
			{
				Log.Error(string.Format("Error while killing {0} during phase {1}: {2}", this.ToStringSafe<Pawn>(), num, arg));
			}
		}

		// Token: 0x0600231A RID: 8986 RVA: 0x000CFCF8 File Offset: 0x000CDEF8
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (mode != DestroyMode.Vanish && mode != DestroyMode.KillFinalize)
			{
				Log.Error(string.Concat(new string[]
				{
					"Destroyed pawn ",
					(this != null) ? this.ToString() : null,
					" with unsupported mode ",
					mode.ToString(),
					"."
				}));
			}
			Map mapHeld = base.MapHeld;
			base.Destroy(mode);
			Find.WorldPawns.Notify_PawnDestroyed(this);
			if (this.ownership != null)
			{
				Building_Grave assignedGrave = this.ownership.AssignedGrave;
				this.ownership.UnclaimAll();
				if (mode == DestroyMode.KillFinalize && assignedGrave != null)
				{
					assignedGrave.CompAssignableToPawn.TryAssignPawn(this);
				}
			}
			this.ClearMind_NewTemp(false, true, true, false);
			Lord lord = this.GetLord();
			if (lord != null)
			{
				PawnLostCondition cond = (mode == DestroyMode.KillFinalize) ? PawnLostCondition.Killed : PawnLostCondition.Vanished;
				lord.Notify_PawnLost(this, cond, null);
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				Find.GameEnder.CheckOrUpdateGameOver();
				Find.TaleManager.Notify_PawnDestroyed(this);
			}
			foreach (Pawn pawn in from p in PawnsFinder.AllMapsWorldAndTemporary_Alive
			where p.playerSettings != null && p.playerSettings.Master == this
			select p)
			{
				pawn.playerSettings.Master = null;
			}
			Pawn_EquipmentTracker pawn_EquipmentTracker = this.equipment;
			if (pawn_EquipmentTracker != null)
			{
				pawn_EquipmentTracker.Notify_PawnDied();
			}
			if (ModsConfig.AnomalyActive && Find.Anomaly != null)
			{
				Find.Anomaly.Notify_PawnDied(this);
			}
			if (mode != DestroyMode.KillFinalize)
			{
				Pawn_EquipmentTracker pawn_EquipmentTracker2 = this.equipment;
				if (pawn_EquipmentTracker2 != null)
				{
					pawn_EquipmentTracker2.DestroyAllEquipment(DestroyMode.Vanish);
				}
				Pawn_InventoryTracker pawn_InventoryTracker = this.inventory;
				if (pawn_InventoryTracker != null)
				{
					pawn_InventoryTracker.DestroyAll(DestroyMode.Vanish);
				}
				Pawn_ApparelTracker pawn_ApparelTracker = this.apparel;
				if (pawn_ApparelTracker != null)
				{
					pawn_ApparelTracker.DestroyAll(DestroyMode.Vanish);
				}
			}
			WorldPawns worldPawns = Find.WorldPawns;
			if (!worldPawns.IsBeingDiscarded(this) && !worldPawns.Contains(this))
			{
				worldPawns.PassToWorld(this, PawnDiscardDecideMode.Decide);
			}
			if (base.Faction.IsPlayerSafe())
			{
				Ideo ideo = this.Ideo;
				if (ideo != null)
				{
					ideo.RecacheColonistBelieverCount();
				}
			}
			Pawn_RelationsTracker pawn_RelationsTracker = this.relations;
			if (pawn_RelationsTracker != null)
			{
				pawn_RelationsTracker.Notify_PawnDestroyed(mode);
			}
			MeditationFocusTypeAvailabilityCache.Notify_PawnDiedOrDestroyed(this);
			Pawn_DrawTracker pawn_DrawTracker = this.Drawer;
			if (pawn_DrawTracker == null)
			{
				return;
			}
			PawnRenderer renderer = pawn_DrawTracker.renderer;
			if (renderer == null)
			{
				return;
			}
			PawnRenderTree renderTree = renderer.renderTree;
			if (renderTree == null)
			{
				return;
			}
			renderTree.SetDirty();
		}

		// Token: 0x0600231B RID: 8987 RVA: 0x000CFF24 File Offset: 0x000CE124
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			Pawn_JobTracker pawn_JobTracker = this.jobs;
			if (((pawn_JobTracker != null) ? pawn_JobTracker.curJob : null) != null)
			{
				this.jobs.StopAll(false, true);
			}
			base.DeSpawn(mode);
			Pawn_PathFollower pawn_PathFollower = this.pather;
			if (pawn_PathFollower != null)
			{
				pawn_PathFollower.StopDead();
			}
			Pawn_RopeTracker pawn_RopeTracker = this.roping;
			if (pawn_RopeTracker != null)
			{
				pawn_RopeTracker.Notify_DeSpawned();
			}
			this.mindState.droppedWeapon = null;
			Pawn_NeedsTracker pawn_NeedsTracker = this.needs;
			if (pawn_NeedsTracker != null)
			{
				Need_Mood mood = pawn_NeedsTracker.mood;
				if (mood != null)
				{
					mood.thoughts.situational.Notify_SituationalThoughtsDirty();
				}
			}
			Pawn_MeleeVerbs pawn_MeleeVerbs = this.meleeVerbs;
			if (pawn_MeleeVerbs != null)
			{
				pawn_MeleeVerbs.Notify_PawnDespawned();
			}
			Pawn_MechanitorTracker pawn_MechanitorTracker = this.mechanitor;
			if (pawn_MechanitorTracker != null)
			{
				pawn_MechanitorTracker.Notify_DeSpawned(mode);
			}
			MeditationFocusTypeAvailabilityCache.Notify_PawnDiedOrDestroyed(this);
			this.ClearAllReservations(false);
			if (map != null)
			{
				map.mapPawns.DeRegisterPawn(this);
				map.autoSlaughterManager.Notify_PawnDespawned();
			}
			PawnComponentsUtility.RemoveComponentsOnDespawned(this);
			if (this.sustainerAmbient != null)
			{
				this.sustainerAmbient.End();
				this.sustainerAmbient = null;
			}
			if (this.sustainerMoving != null)
			{
				this.sustainerMoving.End();
				this.sustainerMoving = null;
			}
		}

		// Token: 0x0600231C RID: 8988 RVA: 0x000D0038 File Offset: 0x000CE238
		public override void Discard(bool silentlyRemoveReferences = false)
		{
			if (Find.WorldPawns.Contains(this))
			{
				Log.Warning("Tried to discard a world pawn " + ((this != null) ? this.ToString() : null) + ".");
				return;
			}
			base.Discard(silentlyRemoveReferences);
			if (this.relations != null)
			{
				if (this.RaceProps.Humanlike)
				{
					if (this.relations.Children.Count((Pawn x) => !x.markedForDiscard) > 1)
					{
						foreach (Pawn pawn in this.relations.Children)
						{
							if (!pawn.markedForDiscard)
							{
								DirectPawnRelation directRelation = pawn.relations.GetDirectRelation(PawnRelationDefOf.Parent, this);
								pawn.relations.ElevateToVirtualRelation(directRelation);
							}
						}
					}
				}
				this.relations.ClearAllRelations();
			}
			if (this.pather != null)
			{
				this.pather.DisposeAndClearCurPathRequest();
				this.pather.DisposeAndClearCurPath();
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				Find.PlayLog.Notify_PawnDiscarded(this, silentlyRemoveReferences);
				Find.BattleLog.Notify_PawnDiscarded(this, silentlyRemoveReferences);
				Find.TaleManager.Notify_PawnDiscarded(this, silentlyRemoveReferences);
				Find.QuestManager.Notify_PawnDiscarded(this);
			}
			foreach (Pawn pawn2 in PawnsFinder.AllMapsWorldAndTemporary_Alive)
			{
				Pawn_NeedsTracker pawn_NeedsTracker = pawn2.needs;
				if (pawn_NeedsTracker != null)
				{
					Need_Mood mood = pawn_NeedsTracker.mood;
					if (mood != null)
					{
						mood.thoughts.memories.Notify_PawnDiscarded(this);
					}
				}
			}
			Corpse.PostCorpseDestroy(this, true);
		}

		// Token: 0x0600231D RID: 8989 RVA: 0x000D01F4 File Offset: 0x000CE3F4
		public Corpse MakeCorpse(Building_Grave assignedGrave, Building_Bed currentBed)
		{
			return this.MakeCorpse(assignedGrave, currentBed != null, (currentBed != null) ? currentBed.Rotation.AsAngle : 0f);
		}

		// Token: 0x0600231E RID: 8990 RVA: 0x000D0224 File Offset: 0x000CE424
		public Corpse MakeCorpse(Building_Grave assignedGrave, bool inBed, float bedRotation)
		{
			if (this.holdingOwner != null)
			{
				string str = "We can't make corpse because the pawn is in a ThingOwner. Remove him from the container first. This should have been already handled before calling this method. holder=";
				IThingHolder parentHolder = base.ParentHolder;
				Log.Warning(str + ((parentHolder != null) ? parentHolder.ToString() : null));
				return null;
			}
			if (this.RaceProps.corpseDef == null)
			{
				return null;
			}
			Corpse corpse = (Corpse)ThingMaker.MakeThing(this.RaceProps.corpseDef, null);
			corpse.InnerPawn = this;
			if (assignedGrave != null)
			{
				corpse.InnerPawn.ownership.ClaimGrave(assignedGrave);
			}
			if (inBed)
			{
				corpse.InnerPawn.Drawer.renderer.wiggler.SetToCustomRotation(bedRotation + 180f);
			}
			return corpse;
		}

		// Token: 0x0600231F RID: 8991 RVA: 0x000D02C4 File Offset: 0x000CE4C4
		public virtual void ExitMap(bool allowedToJoinOrCreateCaravan, Rot4 exitDir)
		{
			if (this.IsWorldPawn())
			{
				Log.Warning("Called ExitMap() on world pawn " + ((this != null) ? this.ToString() : null));
				return;
			}
			Ideo ideo = this.Ideo;
			if (ideo != null)
			{
				ideo.Notify_MemberLost(this, base.Map);
			}
			if (allowedToJoinOrCreateCaravan && CaravanExitMapUtility.CanExitMapAndJoinOrCreateCaravanNow(this))
			{
				CaravanExitMapUtility.ExitMapAndJoinOrCreateCaravan(this, exitDir);
				return;
			}
			Lord lord = this.GetLord();
			if (lord != null)
			{
				lord.Notify_PawnLost(this, PawnLostCondition.ExitedMap, null);
			}
			Pawn_CarryTracker pawn_CarryTracker = this.carryTracker;
			if (((pawn_CarryTracker != null) ? pawn_CarryTracker.CarriedThing : null) != null)
			{
				Pawn pawn = this.carryTracker.CarriedThing as Pawn;
				if (pawn != null)
				{
					if (base.Faction != null && base.Faction != pawn.Faction)
					{
						base.Faction.kidnapped.Kidnap(pawn, this);
					}
					else
					{
						if (!this.teleporting)
						{
							this.carryTracker.innerContainer.Remove(pawn);
						}
						pawn.teleporting = this.teleporting;
						pawn.ExitMap(false, exitDir);
						pawn.teleporting = false;
					}
				}
				else
				{
					this.carryTracker.CarriedThing.Destroy(DestroyMode.Vanish);
				}
				if (!this.teleporting || pawn == null)
				{
					this.carryTracker.innerContainer.Clear();
				}
			}
			bool flag = ThingOwnerUtility.AnyParentIs<ActiveTransporterInfo>(this) || ThingOwnerUtility.AnyParentIs<TravellingTransporters>(this);
			bool flag2 = this.IsCaravanMember() || this.teleporting || flag;
			bool flag3 = !flag2 || (!this.IsPrisoner && !this.IsSlave && !flag) || (this.guest != null && this.guest.Released);
			bool flag4 = flag3 && (this.IsPrisoner || this.IsSlave) && this.guest != null && this.guest.Released;
			bool flag5 = flag4 || (this.guest != null && this.guest.HostFaction == Faction.OfPlayer);
			if (flag3 && !flag2)
			{
				foreach (Thing thing in this.EquippedWornOrInventoryThings)
				{
					Precept_ThingStyle styleSourcePrecept = thing.GetStyleSourcePrecept();
					if (styleSourcePrecept != null)
					{
						styleSourcePrecept.Notify_ThingLost(thing, false);
					}
				}
			}
			Faction faction = base.Faction;
			if (faction != null)
			{
				faction.Notify_MemberExitedMap(this, flag4);
			}
			if (base.Faction == Faction.OfPlayer && this.IsSlave && this.SlaveFaction != null && this.SlaveFaction != Faction.OfPlayer && this.guest.Released)
			{
				this.SlaveFaction.Notify_MemberExitedMap(this, flag4);
			}
			if (this.ownership != null && flag5)
			{
				this.ownership.UnclaimAll();
			}
			if (this.guest != null)
			{
				bool isPrisonerOfColony = this.IsPrisonerOfColony;
				if (flag4)
				{
					this.guest.SetGuestStatus(null, RimWorld.GuestStatus.Guest);
				}
				if (isPrisonerOfColony)
				{
					this.guest.SetNoInteraction();
					if (!this.guest.Released && flag3)
					{
						GuestUtility.Notify_PrisonerEscaped(this);
					}
				}
				this.guest.Released = false;
			}
			base.DeSpawnOrDeselect(DestroyMode.Vanish);
			this.inventory.UnloadEverything = false;
			if (flag3)
			{
				this.ClearMind_NewTemp(false, false, true, false);
			}
			Pawn_RelationsTracker pawn_RelationsTracker = this.relations;
			if (pawn_RelationsTracker != null)
			{
				pawn_RelationsTracker.Notify_ExitedMap();
			}
			Find.WorldPawns.PassToWorld(this, PawnDiscardDecideMode.Decide);
			QuestUtility.SendQuestTargetSignals(this.questTags, "LeftMap", this.Named("SUBJECT"));
			Find.FactionManager.Notify_PawnLeftMap(this);
			Find.IdeoManager.Notify_PawnLeftMap(this);
		}

		// Token: 0x06002320 RID: 8992 RVA: 0x000D062C File Offset: 0x000CE82C
		public override void PreTraded(TradeAction action, Pawn playerNegotiator, ITrader trader)
		{
			base.PreTraded(action, playerNegotiator, trader);
			if (base.SpawnedOrAnyParentSpawned)
			{
				this.DropAndForbidEverything(false, false);
			}
			Pawn_Ownership pawn_Ownership = this.ownership;
			if (pawn_Ownership != null)
			{
				pawn_Ownership.UnclaimAll();
			}
			if (action == TradeAction.PlayerSells)
			{
				Faction faction = this.GetExtraHomeFaction(null) ?? this.GetExtraHostFaction(null);
				if (faction != null && faction != Faction.OfPlayer)
				{
					Faction.OfPlayer.TryAffectGoodwillWith(faction, Faction.OfPlayer.GoodwillToMakeHostile(faction), true, true, HistoryEventDefOf.MemberSold, new GlobalTargetInfo?(this));
				}
			}
			Pawn_GuestTracker pawn_GuestTracker = this.guest;
			if (pawn_GuestTracker != null)
			{
				pawn_GuestTracker.SetGuestStatus(null, RimWorld.GuestStatus.Guest);
			}
			if (action == TradeAction.PlayerBuys)
			{
				if (this.guest != null && this.guest.joinStatus == JoinStatus.JoinAsSlave)
				{
					this.guest.SetGuestStatus(Faction.OfPlayer, RimWorld.GuestStatus.Slave);
				}
				else
				{
					Need_Mood mood = this.needs.mood;
					if (mood != null)
					{
						mood.thoughts.memories.TryGainMemory(ThoughtDefOf.FreedFromSlavery, null, null);
					}
					this.SetFaction(Faction.OfPlayer, null);
				}
			}
			else if (action == TradeAction.PlayerSells)
			{
				if (this.RaceProps.Humanlike)
				{
					TaleRecorder.RecordTale(TaleDefOf.SoldPrisoner, new object[]
					{
						playerNegotiator,
						this,
						trader
					});
				}
				if (base.Faction != null)
				{
					this.SetFaction(null, null);
				}
				if (this.RaceProps.IsFlesh)
				{
					this.relations.Notify_PawnSold(playerNegotiator);
				}
			}
			this.ClearMind_NewTemp(false, false, true, false);
		}

		// Token: 0x06002321 RID: 8993 RVA: 0x000D0788 File Offset: 0x000CE988
		public void PreKidnapped(Pawn kidnapper)
		{
			Find.Storyteller.Notify_PawnEvent(this, AdaptationEvent.Kidnapped, null);
			if (this.IsColonist && kidnapper != null)
			{
				TaleRecorder.RecordTale(TaleDefOf.KidnappedColonist, new object[]
				{
					kidnapper,
					this
				});
			}
			Pawn_Ownership pawn_Ownership = this.ownership;
			if (pawn_Ownership != null)
			{
				pawn_Ownership.UnclaimAll();
			}
			if (this.guest != null && !this.guest.IsSlave)
			{
				this.guest.SetGuestStatus(null, RimWorld.GuestStatus.Guest);
			}
			if (this.RaceProps.IsFlesh)
			{
				this.relations.Notify_PawnKidnapped();
			}
			this.ClearMind_NewTemp(false, false, true, false);
		}

		// Token: 0x06002322 RID: 8994 RVA: 0x000D0823 File Offset: 0x000CEA23
		public override void PreSwapMap()
		{
			base.PreSwapMap();
			Pawn_FlightTracker pawn_FlightTracker = this.flight;
			if (pawn_FlightTracker == null)
			{
				return;
			}
			pawn_FlightTracker.Notify_PreSwapMap();
		}

		// Token: 0x06002323 RID: 8995 RVA: 0x000D083B File Offset: 0x000CEA3B
		public override AcceptanceReport ClaimableBy(Faction by)
		{
			return false;
		}

		// Token: 0x06002324 RID: 8996 RVA: 0x000D0844 File Offset: 0x000CEA44
		public override bool AdoptableBy(Faction by, StringBuilder reason = null)
		{
			if (base.Faction == by)
			{
				return false;
			}
			Pawn_AgeTracker pawn_AgeTracker = this.ageTracker;
			bool flag;
			if (pawn_AgeTracker == null)
			{
				flag = false;
			}
			else
			{
				LifeStageDef curLifeStage = pawn_AgeTracker.CurLifeStage;
				bool? flag2 = (curLifeStage != null) ? new bool?(curLifeStage.claimable) : null;
				bool flag3 = false;
				flag = (flag2.GetValueOrDefault() == flag3 & flag2 != null);
			}
			if (flag)
			{
				return false;
			}
			string value;
			if (base.FactionPreventsClaimingOrAdopting(base.Faction, false, out value))
			{
				if (reason != null)
				{
					reason.Append(value);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06002325 RID: 8997 RVA: 0x000D08C0 File Offset: 0x000CEAC0
		public override void SetFaction(Faction newFaction, Pawn recruiter = null)
		{
			if (newFaction == base.Faction)
			{
				Log.Warning("Used SetFaction to change " + this.ToStringSafe<Pawn>() + " to same faction " + newFaction.ToStringSafe<Faction>());
				return;
			}
			Faction faction = base.Faction;
			Pawn_GuestTracker pawn_GuestTracker = this.guest;
			if (pawn_GuestTracker != null)
			{
				pawn_GuestTracker.SetGuestStatus(null, RimWorld.GuestStatus.Guest);
			}
			if (base.Spawned)
			{
				base.Map.mapPawns.DeRegisterPawn(this);
				base.Map.pawnDestinationReservationManager.ReleaseAllClaimedBy(this);
				base.Map.designationManager.RemoveAllDesignationsOn(this, false);
				base.Map.autoSlaughterManager.Notify_PawnChangedFaction();
			}
			if ((newFaction == Faction.OfPlayer || base.Faction == Faction.OfPlayer) && Current.ProgramState == ProgramState.Playing)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
			Lord lord = this.GetLord();
			if (lord != null)
			{
				lord.Notify_PawnLost(this, PawnLostCondition.ChangedFaction, null);
			}
			if (PawnUtility.IsFactionLeader(this))
			{
				Faction factionLeaderFaction = PawnUtility.GetFactionLeaderFaction(this);
				if (newFaction != factionLeaderFaction && !this.HasExtraHomeFaction(factionLeaderFaction) && !this.HasExtraMiniFaction(factionLeaderFaction))
				{
					factionLeaderFaction.Notify_LeaderLost();
				}
			}
			if (newFaction == Faction.OfPlayer && this.RaceProps.Humanlike && !this.IsQuestLodger())
			{
				this.ChangeKind(newFaction.def.basicMemberKind);
			}
			base.SetFaction(newFaction, null);
			PawnComponentsUtility.AddAndRemoveDynamicComponents(this, false);
			if (base.Faction != null && base.Faction.IsPlayer)
			{
				Pawn_WorkSettings pawn_WorkSettings = this.workSettings;
				if (pawn_WorkSettings != null)
				{
					pawn_WorkSettings.EnableAndInitialize();
				}
				Find.StoryWatcher.watcherPopAdaptation.Notify_PawnEvent(this, PopAdaptationEvent.GainedColonist);
			}
			if (this.Drafted)
			{
				this.drafter.Drafted = false;
			}
			ReachabilityUtility.ClearCacheFor(this);
			this.health.surgeryBills.Clear();
			if (base.Spawned)
			{
				base.Map.mapPawns.RegisterPawn(this);
			}
			this.GenerateNecessaryName();
			Pawn_PlayerSettings pawn_PlayerSettings = this.playerSettings;
			if (pawn_PlayerSettings != null)
			{
				pawn_PlayerSettings.ResetMedicalCare();
			}
			this.ClearMind_NewTemp(true, false, true, false);
			if (!this.Dead && this.needs.mood != null)
			{
				this.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
			}
			if (base.Spawned)
			{
				base.Map.attackTargetsCache.UpdateTarget(this);
			}
			Find.GameEnder.CheckOrUpdateGameOver();
			AddictionUtility.CheckDrugAddictionTeachOpportunity(this);
			Pawn_NeedsTracker pawn_NeedsTracker = this.needs;
			if (pawn_NeedsTracker != null)
			{
				pawn_NeedsTracker.AddOrRemoveNeedsAsAppropriate();
			}
			Pawn_PlayerSettings pawn_PlayerSettings2 = this.playerSettings;
			if (pawn_PlayerSettings2 != null)
			{
				pawn_PlayerSettings2.Notify_FactionChanged();
			}
			Pawn_RelationsTracker pawn_RelationsTracker = this.relations;
			if (pawn_RelationsTracker != null)
			{
				pawn_RelationsTracker.Notify_ChangedFaction();
			}
			if (this.IsAnimal && newFaction == Faction.OfPlayer)
			{
				this.training.SetWantedRecursive(TrainableDefOf.Tameness, true);
				this.training.Train(TrainableDefOf.Tameness, recruiter, true);
				if (this.Roamer && this.mindState != null)
				{
					this.mindState.lastStartRoamCooldownTick = new int?(Find.TickManager.TicksGame);
				}
			}
			if (faction == Faction.OfPlayer)
			{
				BillUtility.Notify_ColonistUnavailable(this);
			}
			if (newFaction == Faction.OfPlayer)
			{
				Find.StoryWatcher.statsRecord.UpdateGreatestPopulation();
				Find.World.StoryState.RecordPopulationIncrease();
			}
			if (newFaction != null)
			{
				newFaction.Notify_PawnJoined(this);
			}
			Ideo ideo = this.Ideo;
			if (ideo != null)
			{
				ideo.Notify_MemberChangedFaction(this, faction, newFaction);
			}
			Pawn_AgeTracker pawn_AgeTracker = this.ageTracker;
			if (pawn_AgeTracker != null)
			{
				pawn_AgeTracker.ResetAgeReversalDemand(Pawn_AgeTracker.AgeReversalReason.Recruited, false);
			}
			Pawn_RopeTracker pawn_RopeTracker = this.roping;
			if (pawn_RopeTracker != null)
			{
				pawn_RopeTracker.BreakAllRopes();
			}
			if (ModsConfig.BiotechActive)
			{
				Pawn_MechanitorTracker pawn_MechanitorTracker = this.mechanitor;
				if (pawn_MechanitorTracker != null)
				{
					pawn_MechanitorTracker.Notify_ChangedFaction();
				}
			}
			Pawn_CreepJoinerTracker pawn_CreepJoinerTracker = this.creepjoiner;
			if (pawn_CreepJoinerTracker != null)
			{
				pawn_CreepJoinerTracker.Notify_ChangedFaction();
			}
			if (faction != null)
			{
				Find.FactionManager.Notify_PawnLeftFaction(faction);
			}
		}

		// Token: 0x06002326 RID: 8998 RVA: 0x000D0C38 File Offset: 0x000CEE38
		[Obsolete]
		public void ClearMind(bool ifLayingKeepLaying = false, bool clearInspiration = false, bool clearMentalState = true)
		{
			this.ClearMind_NewTemp(ifLayingKeepLaying, clearInspiration, clearMentalState, false);
		}

		// Token: 0x06002327 RID: 8999 RVA: 0x000D0C44 File Offset: 0x000CEE44
		public void ClearMind_NewTemp(bool ifLayingKeepLaying = false, bool clearInspiration = false, bool clearMentalState = true, bool wasDowned = false)
		{
			Pawn_PathFollower pawn_PathFollower = this.pather;
			if (pawn_PathFollower != null)
			{
				pawn_PathFollower.StopDead();
			}
			Pawn_MindState pawn_MindState = this.mindState;
			if (pawn_MindState != null)
			{
				pawn_MindState.Reset(clearInspiration, clearMentalState, wasDowned);
			}
			Pawn_JobTracker pawn_JobTracker = this.jobs;
			if (pawn_JobTracker != null)
			{
				pawn_JobTracker.StopAll(ifLayingKeepLaying, true);
			}
			this.VerifyReservations(null);
		}

		// Token: 0x06002328 RID: 9000 RVA: 0x000D0C94 File Offset: 0x000CEE94
		public void ClearAllReservations(bool releaseDestinationsOnlyIfObsolete = true)
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (releaseDestinationsOnlyIfObsolete)
				{
					maps[i].pawnDestinationReservationManager.ReleaseAllObsoleteClaimedBy(this);
				}
				else
				{
					maps[i].pawnDestinationReservationManager.ReleaseAllClaimedBy(this);
				}
				maps[i].reservationManager.ReleaseAllClaimedBy(this);
				maps[i].enrouteManager.ReleaseAllClaimedBy(this);
				maps[i].physicalInteractionReservationManager.ReleaseAllClaimedBy(this);
				maps[i].attackTargetReservationManager.ReleaseAllClaimedBy(this);
			}
		}

		// Token: 0x06002329 RID: 9001 RVA: 0x000D0D2C File Offset: 0x000CEF2C
		public void ClearReservationsForJob(Job job)
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				maps[i].pawnDestinationReservationManager.ReleaseClaimedBy(this, job);
				maps[i].reservationManager.ReleaseClaimedBy(this, job);
				maps[i].enrouteManager.ReleaseAllClaimedBy(this);
				maps[i].physicalInteractionReservationManager.ReleaseClaimedBy(this, job);
				maps[i].attackTargetReservationManager.ReleaseClaimedBy(this, job);
			}
		}

		// Token: 0x0600232A RID: 9002 RVA: 0x000D0DB0 File Offset: 0x000CEFB0
		public void VerifyReservations(Job prevJob = null)
		{
			if (this.jobs == null)
			{
				return;
			}
			if (this.CurJob != null || this.jobs.jobQueue.Count > 0 || this.jobs.startingNewJob)
			{
				return;
			}
			bool flag = false;
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				LocalTargetInfo obj = maps[i].reservationManager.FirstReservationFor(this);
				if (obj.IsValid)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(78, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Reservation manager failed to clean up properly; ");
					defaultInterpolatedStringHandler.AppendFormatted(this.ToStringSafe<Pawn>());
					defaultInterpolatedStringHandler.AppendLiteral(" still reserving ");
					defaultInterpolatedStringHandler.AppendFormatted(obj.ToStringSafe<LocalTargetInfo>());
					defaultInterpolatedStringHandler.AppendLiteral(", prev job: ");
					defaultInterpolatedStringHandler.AppendFormatted<Job>(prevJob);
					Log.ErrorOnce(defaultInterpolatedStringHandler.ToStringAndClear(), 97771429 ^ this.thingIDNumber);
					flag = true;
				}
				LocalTargetInfo obj2 = maps[i].physicalInteractionReservationManager.FirstReservationFor(this);
				if (obj2.IsValid)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(108, 2);
					defaultInterpolatedStringHandler2.AppendLiteral("Physical interaction reservation manager failed to clean up properly; ");
					defaultInterpolatedStringHandler2.AppendFormatted(this.ToStringSafe<Pawn>());
					defaultInterpolatedStringHandler2.AppendLiteral(" still reserving ");
					defaultInterpolatedStringHandler2.AppendFormatted(obj2.ToStringSafe<LocalTargetInfo>());
					defaultInterpolatedStringHandler2.AppendLiteral(", prev job: {prevJob}");
					Log.ErrorOnce(defaultInterpolatedStringHandler2.ToStringAndClear(), 19586765 ^ this.thingIDNumber);
					flag = true;
				}
				IAttackTarget attackTarget = maps[i].attackTargetReservationManager.FirstReservationFor(this);
				if (attackTarget != null)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(101, 2);
					defaultInterpolatedStringHandler3.AppendLiteral("Attack target reservation manager failed to clean up properly; ");
					defaultInterpolatedStringHandler3.AppendFormatted(this.ToStringSafe<Pawn>());
					defaultInterpolatedStringHandler3.AppendLiteral(" still reserving ");
					defaultInterpolatedStringHandler3.AppendFormatted(attackTarget.ToStringSafe<IAttackTarget>());
					defaultInterpolatedStringHandler3.AppendLiteral(", prev job: {prevJob}");
					Log.ErrorOnce(defaultInterpolatedStringHandler3.ToStringAndClear(), 100495878 ^ this.thingIDNumber);
					flag = true;
				}
				IntVec3 obj3 = maps[i].pawnDestinationReservationManager.FirstObsoleteReservationFor(this);
				if (obj3.IsValid)
				{
					Job job = maps[i].pawnDestinationReservationManager.FirstObsoleteReservationJobFor(this);
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4 = new DefaultInterpolatedStringHandler(106, 4);
					defaultInterpolatedStringHandler4.AppendLiteral("Pawn destination reservation manager failed to clean up properly; ");
					defaultInterpolatedStringHandler4.AppendFormatted(this.ToStringSafe<Pawn>());
					defaultInterpolatedStringHandler4.AppendLiteral("/");
					defaultInterpolatedStringHandler4.AppendFormatted(job.ToStringSafe<Job>());
					defaultInterpolatedStringHandler4.AppendLiteral("/");
					defaultInterpolatedStringHandler4.AppendFormatted(job.def.ToStringSafe<JobDef>());
					defaultInterpolatedStringHandler4.AppendLiteral(" still reserving ");
					defaultInterpolatedStringHandler4.AppendFormatted(obj3.ToStringSafe<IntVec3>());
					defaultInterpolatedStringHandler4.AppendLiteral(", prev job: {prevJob}");
					Log.ErrorOnce(defaultInterpolatedStringHandler4.ToStringAndClear(), 1958674 ^ this.thingIDNumber);
					flag = true;
				}
			}
			if (flag)
			{
				this.ClearAllReservations(true);
			}
		}

		// Token: 0x0600232B RID: 9003 RVA: 0x000D106C File Offset: 0x000CF26C
		public void DropAndForbidEverything(bool keepInventoryAndEquipmentIfInBed = false, bool rememberPrimary = false)
		{
			if (this.kindDef.destroyGearOnDrop)
			{
				this.equipment.DestroyAllEquipment(DestroyMode.Vanish);
				this.apparel.DestroyAll(DestroyMode.Vanish);
			}
			if (!this.InContainerEnclosed)
			{
				if (base.SpawnedOrAnyParentSpawned)
				{
					Pawn_CarryTracker pawn_CarryTracker = this.carryTracker;
					if (((pawn_CarryTracker != null) ? pawn_CarryTracker.CarriedThing : null) != null)
					{
						Thing thing;
						this.carryTracker.TryDropCarriedThing(base.PositionHeld, ThingPlaceMode.Near, out thing, null);
					}
					if (!keepInventoryAndEquipmentIfInBed || !this.InBed())
					{
						Pawn_EquipmentTracker pawn_EquipmentTracker = this.equipment;
						if (pawn_EquipmentTracker != null)
						{
							pawn_EquipmentTracker.DropAllEquipment(base.PositionHeld, true, rememberPrimary);
						}
						if (this.inventory != null && this.inventory.innerContainer.TotalStackCount > 0)
						{
							this.inventory.DropAllNearPawn(base.PositionHeld, true, false);
						}
					}
				}
				return;
			}
			Pawn_CarryTracker pawn_CarryTracker2 = this.carryTracker;
			if (((pawn_CarryTracker2 != null) ? pawn_CarryTracker2.CarriedThing : null) != null)
			{
				this.carryTracker.innerContainer.TryTransferToContainer(this.carryTracker.CarriedThing, this.holdingOwner, true);
			}
			Pawn_EquipmentTracker pawn_EquipmentTracker2 = this.equipment;
			if (((pawn_EquipmentTracker2 != null) ? pawn_EquipmentTracker2.Primary : null) != null)
			{
				this.equipment.TryTransferEquipmentToContainer(this.equipment.Primary, this.holdingOwner);
			}
			Pawn_InventoryTracker pawn_InventoryTracker = this.inventory;
			if (pawn_InventoryTracker == null)
			{
				return;
			}
			pawn_InventoryTracker.innerContainer.TryTransferAllToContainer(this.holdingOwner, true);
		}

		// Token: 0x0600232C RID: 9004 RVA: 0x000D11B4 File Offset: 0x000CF3B4
		public void GenerateNecessaryName()
		{
			if (this.Name != null)
			{
				return;
			}
			if (base.Faction != Faction.OfPlayer)
			{
				return;
			}
			if (this.RaceProps.Animal || (ModsConfig.BiotechActive && this.RaceProps.IsMechanoid))
			{
				this.Name = PawnBioAndNameGenerator.GeneratePawnName(this, NameStyle.Numeric, null, false, null);
			}
		}

		// Token: 0x0600232D RID: 9005 RVA: 0x000D120C File Offset: 0x000CF40C
		public Verb TryGetAttackVerb(Thing target, bool allowManualCastWeapons = false, bool allowTurrets = false)
		{
			Pawn_EquipmentTracker pawn_EquipmentTracker = this.equipment;
			if (((pawn_EquipmentTracker != null) ? pawn_EquipmentTracker.Primary : null) != null && this.equipment.PrimaryEq.PrimaryVerb.Available() && (!this.equipment.PrimaryEq.PrimaryVerb.verbProps.onlyManualCast || (this.CurJob != null && this.CurJob.def != JobDefOf.Wait_Combat) || allowManualCastWeapons))
			{
				return this.equipment.PrimaryEq.PrimaryVerb;
			}
			if (allowManualCastWeapons && this.apparel != null)
			{
				Verb firstApparelVerb = this.apparel.FirstApparelVerb;
				if (firstApparelVerb != null && firstApparelVerb.Available())
				{
					return firstApparelVerb;
				}
			}
			if (allowTurrets)
			{
				List<ThingComp> allComps = base.AllComps;
				for (int i = 0; i < allComps.Count; i++)
				{
					CompTurretGun compTurretGun = allComps[i] as CompTurretGun;
					if (compTurretGun != null && !compTurretGun.TurretDestroyed && compTurretGun.GunCompEq.PrimaryVerb.Available())
					{
						return compTurretGun.GunCompEq.PrimaryVerb;
					}
				}
			}
			if (this.kindDef.canMeleeAttack)
			{
				return this.meleeVerbs.TryGetMeleeVerb(target);
			}
			return null;
		}

		// Token: 0x0600232E RID: 9006 RVA: 0x000D1328 File Offset: 0x000CF528
		public bool TryStartAttack(LocalTargetInfo targ)
		{
			if (this.stances.FullBodyBusy)
			{
				return false;
			}
			if (this.WorkTagIsDisabled(WorkTags.Violent))
			{
				return false;
			}
			bool allowManualCastWeapons = !this.IsColonist;
			Verb verb = this.TryGetAttackVerb(targ.Thing, allowManualCastWeapons, false);
			return verb != null && verb.TryStartCastOn(verb.verbProps.ai_RangedAlawaysShootGroundBelowTarget ? targ.Cell : targ, false, true, false, false);
		}

		// Token: 0x0600232F RID: 9007 RVA: 0x000D1394 File Offset: 0x000CF594
		public override IEnumerable<Thing> ButcherProducts(Pawn butcher, float efficiency)
		{
			Pawn.<ButcherProducts>d__339 <ButcherProducts>d__ = new Pawn.<ButcherProducts>d__339(-2);
			<ButcherProducts>d__.<>4__this = this;
			<ButcherProducts>d__.<>3__butcher = butcher;
			<ButcherProducts>d__.<>3__efficiency = efficiency;
			return <ButcherProducts>d__;
		}

		// Token: 0x06002330 RID: 9008 RVA: 0x000D13B4 File Offset: 0x000CF5B4
		public TaggedString FactionDesc(TaggedString name, bool extraFactionsInfo, string nameLabel, string genderLabel)
		{
			Pawn.tmpExtraFactions.Clear();
			QuestUtility.GetExtraFactionsFromQuestParts(this, Pawn.tmpExtraFactions, null);
			GuestUtility.GetExtraFactionsFromGuestStatus(this, Pawn.tmpExtraFactions);
			TaggedString taggedString;
			if (base.Faction != null && !base.Faction.Hidden)
			{
				if (Pawn.tmpExtraFactions.Count == 0 && this.SlaveFaction == null)
				{
					taggedString = "PawnMainDescFactionedWrap".Translate(name, base.Faction.NameColored, nameLabel.Named("NAME"), genderLabel.Named("GENDER"));
				}
				else
				{
					taggedString = "PawnMainDescUnderFactionedWrap".Translate(name, base.Faction.NameColored);
				}
			}
			else
			{
				taggedString = name;
			}
			if (extraFactionsInfo)
			{
				for (int i = 0; i < Pawn.tmpExtraFactions.Count; i++)
				{
					if (base.Faction != Pawn.tmpExtraFactions[i].faction && !Pawn.tmpExtraFactions[i].faction.Hidden)
					{
						taggedString += "\n" + Pawn.tmpExtraFactions[i].factionType.GetLabel().CapitalizeFirst() + ": " + Pawn.tmpExtraFactions[i].faction.NameColored.Resolve();
					}
				}
			}
			Pawn.tmpExtraFactions.Clear();
			return taggedString;
		}

		// Token: 0x06002331 RID: 9009 RVA: 0x000D1514 File Offset: 0x000CF714
		public string MainDesc(bool writeFaction, bool writeGender = true)
		{
			bool flag = base.Faction == null || !base.Faction.IsPlayer;
			string text = writeGender ? ((this.gender == Gender.None) ? string.Empty : this.gender.GetLabel(this.AnimalOrWildMan())) : string.Empty;
			string text2 = string.Empty;
			if (this.RaceProps.Animal || this.RaceProps.IsMechanoid)
			{
				text2 = GenLabel.BestKindLabel(this, false, true, false, -1);
				if (this.Name != null)
				{
					if (!text.NullOrEmpty())
					{
						text += " ";
					}
					text += text2;
				}
			}
			if (this.ageTracker != null)
			{
				if (text.Length > 0)
				{
					text += ", ";
				}
				text += "AgeIndicator".Translate(this.ageTracker.AgeNumberString);
			}
			if (this.IsMutant && this.mutant.HasTurned && this.mutant.Def.overrideLabel)
			{
				if (text.Length > 0)
				{
					text += ", ";
				}
				text += this.mutant.Def.label;
			}
			else if (!this.RaceProps.Animal && !this.RaceProps.IsMechanoid && flag && !this.IsCreepJoiner)
			{
				if (text.Length > 0)
				{
					text += ", ";
				}
				text2 = GenLabel.BestKindLabel(this, false, true, false, -1);
				text += text2;
			}
			if (writeFaction)
			{
				text = this.FactionDesc(text, true, text2, this.gender.GetLabel(this.RaceProps.Animal)).Resolve();
			}
			return text.CapitalizeFirst();
		}

		// Token: 0x06002332 RID: 9010 RVA: 0x000D16D8 File Offset: 0x000CF8D8
		public string GetJobReport()
		{
			string result;
			try
			{
				Lord lord = this.GetLord();
				object obj;
				if (lord == null)
				{
					obj = null;
				}
				else
				{
					LordJob lordJob = lord.LordJob;
					obj = ((lordJob != null) ? lordJob.GetJobReport(this) : null);
				}
				object obj2;
				if ((obj2 = obj) == null)
				{
					Pawn_JobTracker pawn_JobTracker = this.jobs;
					if (pawn_JobTracker == null)
					{
						obj2 = null;
					}
					else
					{
						JobDriver curDriver = pawn_JobTracker.curDriver;
						obj2 = ((curDriver != null) ? curDriver.GetReport() : null);
					}
				}
				object obj3 = obj2;
				result = ((obj3 != null) ? obj3.CapitalizeFirst() : null);
			}
			catch (Exception ex)
			{
				string str = "JobDriver.GetReport() exception: ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				result = null;
			}
			return result;
		}

		// Token: 0x06002333 RID: 9011 RVA: 0x000D1768 File Offset: 0x000CF968
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!this.def.hideMainDesc)
			{
				stringBuilder.AppendLine(this.MainDesc(PawnUtility.ShouldDisplayFactionInInspectString(this), true));
			}
			Pawn_RoyaltyTracker pawn_RoyaltyTracker = this.royalty;
			RoyalTitle royalTitle = (pawn_RoyaltyTracker != null) ? pawn_RoyaltyTracker.MostSeniorTitle : null;
			if (royalTitle != null)
			{
				stringBuilder.AppendLine("PawnTitleDescWrap".Translate(royalTitle.def.GetLabelCapFor(this), royalTitle.faction.NameColored).Resolve());
			}
			string inspectString = base.GetInspectString();
			if (!inspectString.NullOrEmpty())
			{
				stringBuilder.AppendLine(inspectString);
			}
			if (this.TraderKind != null)
			{
				stringBuilder.AppendLine(this.TraderKind.LabelCap);
			}
			if (this.InMentalState)
			{
				string inspectLine = this.MentalState.InspectLine;
				if (!string.IsNullOrEmpty(inspectLine))
				{
					stringBuilder.AppendLine(inspectLine);
				}
			}
			Pawn.states.Clear();
			Pawn_HealthTracker pawn_HealthTracker = this.health;
			if (((pawn_HealthTracker != null) ? pawn_HealthTracker.hediffSet : null) != null)
			{
				List<Hediff> hediffs = this.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					Hediff hediff = hediffs[i];
					if (!hediff.def.battleStateLabel.NullOrEmpty())
					{
						Pawn.states.AddUnique(hediff.def.battleStateLabel);
					}
					string inspectString2 = hediff.GetInspectString();
					if (!inspectString2.NullOrEmpty())
					{
						stringBuilder.AppendLine(inspectString2);
					}
				}
			}
			if (Pawn.states.Count > 0)
			{
				Pawn.states.Sort();
				StringBuilder stringBuilder2 = stringBuilder;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("State".Translate());
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted(Pawn.states.ToCommaList(false, false).CapitalizeFirst());
				stringBuilder2.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear());
				Pawn.states.Clear();
			}
			Pawn_FlightTracker pawn_FlightTracker = this.flight;
			string text = (pawn_FlightTracker != null) ? pawn_FlightTracker.GetStatusString() : null;
			if (!text.NullOrEmpty())
			{
				stringBuilder.AppendLine(text);
			}
			Pawn_StanceTracker pawn_StanceTracker = this.stances;
			if (((pawn_StanceTracker != null) ? pawn_StanceTracker.stunner : null) != null && this.stances.stunner.Stunned)
			{
				if (this.stances.stunner.Hypnotized)
				{
					stringBuilder.AppendLine("InTrance".Translate());
				}
				else if (this.stances.stunner.StunFromEMP)
				{
					stringBuilder.AppendLine("StunnedByEMP".Translate() + ": " + this.stances.stunner.StunTicksLeft.ToStringSecondsFromTicks());
				}
				else
				{
					stringBuilder.AppendLine("StunLower".Translate().CapitalizeFirst() + ": " + this.stances.stunner.StunTicksLeft.ToStringSecondsFromTicks());
				}
			}
			Pawn_StanceTracker pawn_StanceTracker2 = this.stances;
			if (((pawn_StanceTracker2 != null) ? pawn_StanceTracker2.stagger : null) != null && this.stances.stagger.Staggered)
			{
				stringBuilder.AppendLine("SlowedByDamage".Translate() + ": " + this.stances.stagger.StaggerTicksLeft.ToStringSecondsFromTicks());
			}
			if (this.Inspired)
			{
				stringBuilder.AppendLine(this.Inspiration.InspectLine);
			}
			Pawn_EquipmentTracker pawn_EquipmentTracker = this.equipment;
			if (((pawn_EquipmentTracker != null) ? pawn_EquipmentTracker.Primary : null) != null)
			{
				stringBuilder.AppendLine("Equipped".TranslateSimple() + ": " + ((this.equipment.Primary != null) ? this.equipment.Primary.Label : "EquippedNothing".TranslateSimple()).CapitalizeFirst());
			}
			if (this.abilities != null)
			{
				for (int j = 0; j < this.abilities.AllAbilitiesForReading.Count; j++)
				{
					string inspectString3 = this.abilities.AllAbilitiesForReading[j].GetInspectString();
					if (!inspectString3.NullOrEmpty())
					{
						stringBuilder.AppendLine(inspectString3);
					}
				}
			}
			Pawn_CarryTracker pawn_CarryTracker = this.carryTracker;
			if (((pawn_CarryTracker != null) ? pawn_CarryTracker.CarriedThing : null) != null && (this.CurJob == null || this.CurJob.showCarryingInspectLine))
			{
				stringBuilder.Append("Carrying".Translate() + ": ");
				stringBuilder.AppendLine(this.carryTracker.CarriedThing.LabelCap);
			}
			Pawn_RopeTracker pawn_RopeTracker = this.roping;
			if (pawn_RopeTracker != null && pawn_RopeTracker.IsRoped)
			{
				stringBuilder.AppendLine(this.roping.InspectLine);
			}
			if (ModsConfig.BiotechActive && this.IsColonyMech && this.needs.energy != null)
			{
				TaggedString taggedString = "MechEnergy".Translate() + ": " + this.needs.energy.CurLevelPercentage.ToStringPercent();
				float maxLevel = this.needs.energy.MaxLevel;
				if (this.IsCharging())
				{
					taggedString += " (+" + "PerDay".Translate((50f / maxLevel).ToStringPercent()) + ")";
				}
				else if (this.IsSelfShutdown())
				{
					taggedString += " (+" + "PerDay".Translate((1f / maxLevel).ToStringPercent()) + ")";
				}
				else
				{
					taggedString += " (-" + "PerDay".Translate((this.needs.energy.FallPerDay / maxLevel).ToStringPercent()) + ")";
				}
				stringBuilder.AppendLine(taggedString);
			}
			string text2 = null;
			if (PawnUtility.ShouldDisplayLordReport(this))
			{
				Lord lord = this.GetLord();
				if (((lord != null) ? lord.LordJob : null) != null)
				{
					text2 = lord.LordJob.GetReport(this);
				}
			}
			if (PawnUtility.ShouldDisplayJobReport(this))
			{
				string jobReport = this.GetJobReport();
				if (text2.NullOrEmpty())
				{
					text2 = jobReport;
				}
				else if (!jobReport.NullOrEmpty())
				{
					text2 = text2 + ": " + jobReport;
				}
			}
			if (!text2.NullOrEmpty())
			{
				stringBuilder.AppendLine(text2.CapitalizeFirst().EndWithPeriod());
			}
			Pawn_JobTracker pawn_JobTracker = this.jobs;
			if (((pawn_JobTracker != null) ? pawn_JobTracker.curJob : null) != null)
			{
				Pawn_JobTracker pawn_JobTracker2 = this.jobs;
				if (pawn_JobTracker2 != null && pawn_JobTracker2.jobQueue.Count > 0)
				{
					try
					{
						string text3 = this.jobs.jobQueue[0].job.GetReport(this).CapitalizeFirst();
						if (this.jobs.jobQueue.Count > 1)
						{
							text3 = text3 + " (+" + (this.jobs.jobQueue.Count - 1).ToString() + ")";
						}
						stringBuilder.AppendLine("Queued".Translate() + ": " + text3);
					}
					catch (Exception ex)
					{
						string str = "JobDriver.GetReport() exception: ";
						Exception ex2 = ex;
						Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
					}
				}
			}
			if (this.IsMutant && this.mutant.Def.overrideInspectString)
			{
				string inspectString4 = this.mutant.GetInspectString();
				if (!inspectString4.NullOrEmpty())
				{
					stringBuilder.AppendLine(inspectString4);
				}
			}
			if (ModsConfig.AnomalyActive)
			{
				Pawn_HealthTracker pawn_HealthTracker2 = this.health;
				if (((pawn_HealthTracker2 != null) ? pawn_HealthTracker2.hediffSet : null) != null)
				{
					Hediff_MetalhorrorImplant firstHediff = this.health.hediffSet.GetFirstHediff<Hediff_MetalhorrorImplant>();
					if (firstHediff != null && firstHediff.Emerging)
					{
						stringBuilder.AppendLine("Emerging".Translate());
					}
				}
				if (this.IsCreepJoiner)
				{
					string inspectString5 = this.creepjoiner.GetInspectString();
					if (!inspectString5.NullOrEmpty())
					{
						stringBuilder.AppendLine(inspectString5);
					}
				}
			}
			if (ModsConfig.BiotechActive)
			{
				Pawn_NeedsTracker pawn_NeedsTracker = this.needs;
				if (((pawn_NeedsTracker != null) ? pawn_NeedsTracker.energy : null) != null && this.needs.energy.IsLowEnergySelfShutdown)
				{
					stringBuilder.AppendLine("MustBeCarriedToRecharger".Translate());
				}
			}
			if (RestraintsUtility.ShouldShowRestraintsInfo(this))
			{
				stringBuilder.AppendLine("InRestraints".Translate());
			}
			if (this.guest != null && !this.guest.Recruitable && !this.IsSubhuman && !this.IsCreepJoiner)
			{
				if (base.Faction == null)
				{
					stringBuilder.AppendLine("UnrecruitableNoFaction".Translate().CapitalizeFirst());
				}
				else if (base.Faction != Faction.OfPlayer || this.IsSlaveOfColony || this.IsPrisonerOfColony)
				{
					stringBuilder.AppendLine("Unrecruitable".Translate().CapitalizeFirst());
				}
			}
			if (Prefs.DevMode && DebugSettings.showLocomotionUrgency && this.CurJob != null)
			{
				stringBuilder.AppendLine("Locomotion Urgency: " + this.CurJob.locomotionUrgency.ToString());
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x06002334 RID: 9012 RVA: 0x000D20AC File Offset: 0x000D02AC
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Pawn.<GetGizmos>d__346 <GetGizmos>d__ = new Pawn.<GetGizmos>d__346(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x06002335 RID: 9013 RVA: 0x000D20BC File Offset: 0x000D02BC
		public virtual IEnumerable<FloatMenuOption> GetExtraFloatMenuOptionsFor(IntVec3 sq)
		{
			return Enumerable.Empty<FloatMenuOption>();
		}

		// Token: 0x06002336 RID: 9014 RVA: 0x000D20C3 File Offset: 0x000D02C3
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			Pawn.<GetFloatMenuOptions>d__348 <GetFloatMenuOptions>d__ = new Pawn.<GetFloatMenuOptions>d__348(-2);
			<GetFloatMenuOptions>d__.<>4__this = this;
			<GetFloatMenuOptions>d__.<>3__selPawn = selPawn;
			return <GetFloatMenuOptions>d__;
		}

		// Token: 0x06002337 RID: 9015 RVA: 0x000D20DC File Offset: 0x000D02DC
		public override TipSignal GetTooltip()
		{
			string value = "";
			if (this.gender != Gender.None)
			{
				if (!this.LabelCap.EqualsIgnoreCase(this.KindLabel))
				{
					value = "PawnTooltipGenderAndKindLabel".Translate(this.GetGenderLabel(), this.KindLabel);
				}
				else
				{
					value = this.GetGenderLabel();
				}
			}
			else if (!this.LabelCap.EqualsIgnoreCase(this.KindLabel))
			{
				value = this.KindLabel;
			}
			string generalConditionLabel = HealthUtility.GetGeneralConditionLabel(this, false);
			bool flag = !string.IsNullOrEmpty(value);
			Pawn_EquipmentTracker pawn_EquipmentTracker = this.equipment;
			string text;
			if (((pawn_EquipmentTracker != null) ? pawn_EquipmentTracker.Primary : null) != null)
			{
				if (flag)
				{
					text = "PawnTooltipWithDescAndPrimaryEquip".Translate(this.LabelCap, value, this.equipment.Primary.LabelCap, generalConditionLabel);
				}
				else
				{
					text = "PawnTooltipWithPrimaryEquipNoDesc".Translate(this.LabelCap, value, generalConditionLabel);
				}
			}
			else if (flag)
			{
				text = "PawnTooltipWithDescNoPrimaryEquip".Translate(this.LabelCap, value, generalConditionLabel);
			}
			else
			{
				text = "PawnTooltipNoDescNoPrimaryEquip".Translate(this.LabelCap, generalConditionLabel);
			}
			return new TipSignal(text, this.thingIDNumber * 152317, TooltipPriority.Pawn);
		}

		// Token: 0x06002338 RID: 9016 RVA: 0x000D224B File Offset: 0x000D044B
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			Pawn.<SpecialDisplayStats>d__350 <SpecialDisplayStats>d__ = new Pawn.<SpecialDisplayStats>d__350(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x06002339 RID: 9017 RVA: 0x000D225B File Offset: 0x000D045B
		public PathingContext GetPathContext(Pathing pathing)
		{
			if (this.Flying)
			{
				return pathing.Flying;
			}
			if (this.ShouldAvoidFences && (this.CurJob == null || !this.CurJob.canBashFences))
			{
				return pathing.FenceBlocked;
			}
			return pathing.Normal;
		}

		// Token: 0x0600233A RID: 9018 RVA: 0x000D2298 File Offset: 0x000D0498
		public bool Sterile()
		{
			if (!this.ageTracker.CurLifeStage.reproductive)
			{
				return true;
			}
			if (this.RaceProps.Humanlike)
			{
				if (!ModsConfig.BiotechActive)
				{
					return true;
				}
				if (this.GetStatValue(StatDefOf.Fertility, true, -1) <= 0f)
				{
					return true;
				}
			}
			return this.health.hediffSet.HasHediffPreventsPregnancy() || this.SterileGenes();
		}

		// Token: 0x0600233B RID: 9019 RVA: 0x000D2304 File Offset: 0x000D0504
		public bool AnythingToStrip()
		{
			if (!this.kindDef.canStrip)
			{
				return false;
			}
			if (this.equipment != null && this.equipment.HasAnything())
			{
				return true;
			}
			if (this.inventory != null && this.inventory.innerContainer.Count > 0)
			{
				return true;
			}
			if (this.apparel != null)
			{
				if (base.Destroyed)
				{
					if (this.apparel.AnyApparel)
					{
						return true;
					}
				}
				else if (this.apparel.AnyApparelUnlocked)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600233C RID: 9020 RVA: 0x000D2384 File Offset: 0x000D0584
		public void Strip(bool notifyFaction = true)
		{
			Caravan caravan = this.GetCaravan();
			if (caravan != null)
			{
				CaravanInventoryUtility.MoveAllInventoryToSomeoneElse(this, caravan.PawnsListForReading, null);
				if (this.apparel != null)
				{
					CaravanInventoryUtility.MoveAllApparelToSomeonesInventory(this, caravan.PawnsListForReading, base.Destroyed);
				}
				if (this.equipment != null)
				{
					CaravanInventoryUtility.MoveAllEquipmentToSomeonesInventory(this, caravan.PawnsListForReading);
				}
			}
			else
			{
				Corpse corpse = this.Corpse;
				IntVec3 pos = (corpse != null) ? corpse.PositionHeld : base.PositionHeld;
				Pawn_EquipmentTracker pawn_EquipmentTracker = this.equipment;
				if (pawn_EquipmentTracker != null)
				{
					pawn_EquipmentTracker.DropAllEquipment(pos, false, false);
				}
				Pawn_ApparelTracker pawn_ApparelTracker = this.apparel;
				if (pawn_ApparelTracker != null)
				{
					pawn_ApparelTracker.DropAll(pos, false, base.Destroyed, null);
				}
				Pawn_InventoryTracker pawn_InventoryTracker = this.inventory;
				if (pawn_InventoryTracker != null)
				{
					pawn_InventoryTracker.DropAllNearPawn(pos, false, false);
				}
			}
			if (notifyFaction && base.Faction != null)
			{
				base.Faction.Notify_MemberStripped(this, Faction.OfPlayer);
			}
		}

		// Token: 0x0600233D RID: 9021 RVA: 0x000D2450 File Offset: 0x000D0650
		public Thought_Memory GiveObservedThought(Pawn observer)
		{
			if (ModsConfig.AnomalyActive && base.Spawned && !this.Downed)
			{
				Pawn_MindState pawn_MindState = this.mindState;
				DutyDef dutyDef;
				if (pawn_MindState == null)
				{
					dutyDef = null;
				}
				else
				{
					PawnDuty duty = pawn_MindState.duty;
					dutyDef = ((duty != null) ? duty.def : null);
				}
				CompChimera compChimera;
				if (dutyDef == DutyDefOf.ChimeraAttack && this.TryGetComp(out compChimera))
				{
					return compChimera.GiveObservedThought(observer);
				}
			}
			return null;
		}

		// Token: 0x0600233E RID: 9022 RVA: 0x000D24AC File Offset: 0x000D06AC
		public HistoryEventDef GiveObservedHistoryEvent(Pawn observer)
		{
			return null;
		}

		// Token: 0x0600233F RID: 9023 RVA: 0x000D24B0 File Offset: 0x000D06B0
		public void HearClamor(Thing source, ClamorDef type)
		{
			if (this.Dead || this.Downed || this.Deathresting || this.IsSelfShutdown())
			{
				return;
			}
			if (type == ClamorDefOf.Movement || type == ClamorDefOf.BabyCry)
			{
				Pawn pawn = source as Pawn;
				if (pawn != null)
				{
					this.CheckForDisturbedSleep(pawn);
				}
				this.NotifyLordOfClamor(source, type);
				return;
			}
			if (type == ClamorDefOf.Harm)
			{
				if (base.Faction != Faction.OfPlayer && !this.Awake() && base.Faction == source.Faction && this.HostFaction == null)
				{
					this.mindState.canSleepTick = Find.TickManager.TicksGame + 1000;
					if (this.CurJob != null)
					{
						this.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
					this.NotifyLordOfClamor(source, type);
					return;
				}
			}
			else if (type == ClamorDefOf.Construction)
			{
				if (base.Faction != Faction.OfPlayer && !this.Awake() && base.Faction != source.Faction && this.HostFaction == null)
				{
					this.mindState.canSleepTick = Find.TickManager.TicksGame + 1000;
					if (this.CurJob != null)
					{
						this.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
					this.NotifyLordOfClamor(source, type);
					return;
				}
			}
			else if (type == ClamorDefOf.Ability)
			{
				if (base.Faction != Faction.OfPlayer && base.Faction != source.Faction && this.HostFaction == null)
				{
					if (!this.Awake())
					{
						this.mindState.canSleepTick = Find.TickManager.TicksGame + 1000;
						if (this.CurJob != null)
						{
							this.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
						}
					}
					this.NotifyLordOfClamor(source, type);
					return;
				}
			}
			else if (type == ClamorDefOf.Impact)
			{
				this.mindState.Notify_ClamorImpact(source);
				if (this.CurJob != null && !this.Awake())
				{
					this.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
				this.NotifyLordOfClamor(source, type);
			}
		}

		// Token: 0x06002340 RID: 9024 RVA: 0x000D26AB File Offset: 0x000D08AB
		private void NotifyLordOfClamor(Thing source, ClamorDef type)
		{
			Lord lord = this.GetLord();
			if (lord == null)
			{
				return;
			}
			lord.Notify_Clamor(source, type);
		}

		// Token: 0x06002341 RID: 9025 RVA: 0x000D26C0 File Offset: 0x000D08C0
		public override void Notify_UsedVerb(Pawn pawn, Verb verb)
		{
			base.Notify_UsedVerb(pawn, verb);
			if (Rand.Chance((this.IsMutant && this.mutant.Def.soundAttackChance > 0f) ? this.mutant.Def.soundAttackChance : this.ageTracker.CurLifeStage.soundAttackChance))
			{
				LifeStageUtility.PlayNearestLifestageSound(pawn, (LifeStageAge lifeStage) => lifeStage.soundAttack, null, (MutantDef mutantDef) => mutantDef.soundAttack, 1f);
			}
			this.UpdatePyroVerbThought(verb);
		}

		// Token: 0x06002342 RID: 9026 RVA: 0x000D276E File Offset: 0x000D096E
		public override void Notify_Explosion(Explosion explosion)
		{
			base.Notify_Explosion(explosion);
			this.mindState.Notify_Explosion(explosion);
		}

		// Token: 0x06002343 RID: 9027 RVA: 0x000D2783 File Offset: 0x000D0983
		public override void Notify_BulletImpactNearby(BulletImpactData impactData)
		{
			Pawn_ApparelTracker pawn_ApparelTracker = this.apparel;
			if (pawn_ApparelTracker == null)
			{
				return;
			}
			pawn_ApparelTracker.Notify_BulletImpactNearby(impactData);
		}

		// Token: 0x06002344 RID: 9028 RVA: 0x000D2798 File Offset: 0x000D0998
		public virtual void Notify_Downed()
		{
			List<ThingComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				allComps[i].Notify_Downed();
			}
		}

		// Token: 0x06002345 RID: 9029 RVA: 0x000D27CC File Offset: 0x000D09CC
		public virtual void Notify_Released()
		{
			List<ThingComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				allComps[i].Notify_Released();
			}
			if (ModsConfig.AnomalyActive)
			{
				Pawn_CreepJoinerTracker pawn_CreepJoinerTracker = this.creepjoiner;
				if (pawn_CreepJoinerTracker == null)
				{
					return;
				}
				pawn_CreepJoinerTracker.Notify_Released();
			}
		}

		// Token: 0x06002346 RID: 9030 RVA: 0x000D2814 File Offset: 0x000D0A14
		public virtual void Notify_PrisonBreakout()
		{
			List<ThingComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				allComps[i].Notify_PrisonBreakout();
			}
			if (ModsConfig.AnomalyActive)
			{
				Pawn_CreepJoinerTracker pawn_CreepJoinerTracker = this.creepjoiner;
				if (pawn_CreepJoinerTracker == null)
				{
					return;
				}
				pawn_CreepJoinerTracker.Notify_PrisonBreakout();
			}
		}

		// Token: 0x06002347 RID: 9031 RVA: 0x000D285C File Offset: 0x000D0A5C
		public virtual void Notify_DuplicatedFrom(Pawn source)
		{
			List<ThingComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				allComps[i].Notify_DuplicatedFrom(source);
			}
			if (ModsConfig.AnomalyActive)
			{
				Pawn_CreepJoinerTracker pawn_CreepJoinerTracker = this.creepjoiner;
				if (pawn_CreepJoinerTracker == null)
				{
					return;
				}
				pawn_CreepJoinerTracker.Notify_DuplicatedFrom(source);
			}
		}

		// Token: 0x06002348 RID: 9032 RVA: 0x000D28A8 File Offset: 0x000D0AA8
		private void CheckForDisturbedSleep(Pawn source)
		{
			if (this.needs.mood == null)
			{
				return;
			}
			if (this.Awake())
			{
				return;
			}
			if (base.Faction != Faction.OfPlayer)
			{
				return;
			}
			if (Find.TickManager.TicksGame < this.lastSleepDisturbedTick + 300)
			{
				return;
			}
			if (this.Deathresting)
			{
				return;
			}
			if (source != null)
			{
				if (LovePartnerRelationUtility.LovePartnerRelationExists(this, source))
				{
					return;
				}
				if (source.RaceProps.petness > 0f)
				{
					return;
				}
				if (source.relations != null)
				{
					if (source.relations.DirectRelations.Any((DirectPawnRelation dr) => dr.def == PawnRelationDefOf.Bond))
					{
						return;
					}
				}
			}
			this.lastSleepDisturbedTick = Find.TickManager.TicksGame;
			this.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SleepDisturbed, null, null);
		}

		// Token: 0x06002349 RID: 9033 RVA: 0x000D2988 File Offset: 0x000D0B88
		public float GetAcceptArrestChance(Pawn arrester)
		{
			if (this.Downed || this.WorkTagIsDisabled(WorkTags.Violent) || (this.guilt != null && this.guilt.IsGuilty && this.IsColonist && !this.IsQuestLodger()))
			{
				return 1f;
			}
			if (ModsConfig.AnomalyActive && this.health.hediffSet.HasHediff(HediffDefOf.RevenantHypnosis, false))
			{
				return 1f;
			}
			if (ModsConfig.BiotechActive && this.genes != null && this.genes.AggroMentalBreakSelectionChanceFactor <= 0f)
			{
				return 1f;
			}
			return (StatDefOf.ArrestSuccessChance.Worker.IsDisabledFor(arrester) ? StatDefOf.ArrestSuccessChance.valueIfMissing : arrester.GetStatValue(StatDefOf.ArrestSuccessChance, true, -1)) * this.kindDef.acceptArrestChanceFactor;
		}

		// Token: 0x0600234A RID: 9034 RVA: 0x000D2A54 File Offset: 0x000D0C54
		public bool CheckAcceptArrest(Pawn arrester)
		{
			float acceptArrestChance = this.GetAcceptArrestChance(arrester);
			Faction homeFaction = this.HomeFaction;
			if (homeFaction != null && homeFaction != arrester.factionInt)
			{
				homeFaction.Notify_MemberCaptured(this, arrester.Faction);
			}
			List<ThingComp> allComps = base.AllComps;
			if (this.Downed || this.WorkTagIsDisabled(WorkTags.Violent) || Rand.Value < acceptArrestChance)
			{
				for (int i = 0; i < allComps.Count; i++)
				{
					allComps[i].Notify_Arrested(true);
					if (ModsConfig.AnomalyActive)
					{
						Pawn_CreepJoinerTracker pawn_CreepJoinerTracker = this.creepjoiner;
						if (pawn_CreepJoinerTracker != null)
						{
							pawn_CreepJoinerTracker.Notify_Arrested(true);
						}
					}
				}
				return true;
			}
			Messages.Message("MessageRefusedArrest".Translate(this.LabelShort, this), this, MessageTypeDefOf.ThreatSmall, true);
			for (int j = 0; j < allComps.Count; j++)
			{
				allComps[j].Notify_Arrested(false);
			}
			if (ModsConfig.AnomalyActive)
			{
				Pawn_CreepJoinerTracker pawn_CreepJoinerTracker2 = this.creepjoiner;
				if (pawn_CreepJoinerTracker2 != null)
				{
					pawn_CreepJoinerTracker2.Notify_Arrested(false);
				}
			}
			if (base.Faction == null || !arrester.HostileTo(this))
			{
				this.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Berserk, null, false, false, false, null, false, false, false);
			}
			return false;
		}

		// Token: 0x0600234B RID: 9035 RVA: 0x000D2B80 File Offset: 0x000D0D80
		public bool ThreatDisabled(IAttackTargetSearcher disabledFor)
		{
			if (!base.Spawned)
			{
				return true;
			}
			if (!this.InMentalState && this.GetTraderCaravanRole() == TraderCaravanRole.Carrier && !(this.jobs.curDriver is JobDriver_AttackMelee))
			{
				return true;
			}
			if (this.mindState.duty != null && this.mindState.duty.def.threatDisabled)
			{
				return true;
			}
			if (!this.mindState.Active)
			{
				return true;
			}
			if (this.IsColonyMechRequiringMechanitor())
			{
				return true;
			}
			Pawn pawn = ((disabledFor != null) ? disabledFor.Thing : null) as Pawn;
			if (this.Downed && (!this.CanAttackWhileCrawling || !this.Crawling))
			{
				if (disabledFor == null)
				{
					return true;
				}
				bool flag;
				if (pawn == null)
				{
					flag = (null != null);
				}
				else
				{
					Pawn_MindState pawn_MindState = pawn.mindState;
					flag = (((pawn_MindState != null) ? pawn_MindState.duty : null) != null);
				}
				if (!flag || !pawn.mindState.duty.attackDownedIfStarving || !pawn.Starving())
				{
					return true;
				}
			}
			CompActivity compActivity;
			return (ModsConfig.AnomalyActive && this.TryGetComp(out compActivity) && compActivity.IsDormant) || this.IsPsychologicallyInvisible() || (this.ThreatDisabledBecauseNonAggressiveRoamer(pawn) || (pawn != null && pawn.ThreatDisabledBecauseNonAggressiveRoamer(this)));
		}

		// Token: 0x0600234C RID: 9036 RVA: 0x000D2CA0 File Offset: 0x000D0EA0
		public bool ThreatDisabledBecauseNonAggressiveRoamer(Pawn otherPawn)
		{
			if (!this.Roamer || base.Faction != Faction.OfPlayer)
			{
				return false;
			}
			Lord lord = (otherPawn != null) ? otherPawn.GetLord() : null;
			return (lord == null || !lord.CurLordToil.AllowAggressiveTargetingOfRoamers) && !this.InAggroMentalState && !this.IsFighting() && Find.TickManager.TicksGame >= this.mindState.lastEngageTargetTick + 360;
		}

		// Token: 0x0600234D RID: 9037 RVA: 0x000D2D14 File Offset: 0x000D0F14
		private void UpdatePyroVerbThought(Verb verb)
		{
			if (this.story == null)
			{
				return;
			}
			if (!this.story.traits.HasTrait(TraitDefOf.Pyromaniac))
			{
				return;
			}
			if (!verb.IsIncendiary_Melee() && !verb.IsIncendiary_Ranged())
			{
				return;
			}
			if (verb.CurrentTarget.Pawn == null || !verb.CurrentTarget.Pawn.Spawned || !this.IsValidPyroThoughtTarget(verb.CurrentTarget.Pawn))
			{
				foreach (IntVec3 c in GenRadial.RadialCellsAround(verb.CurrentTarget.Cell, verb.EffectiveRange, true))
				{
					if (c.InBounds(base.MapHeld))
					{
						foreach (Thing thing in c.GetThingList(base.MapHeld))
						{
							if (this.IsValidPyroThoughtTarget(thing))
							{
								Pawn_NeedsTracker pawn_NeedsTracker = this.needs;
								if (pawn_NeedsTracker == null)
								{
									break;
								}
								Need_Mood mood = pawn_NeedsTracker.mood;
								if (mood == null)
								{
									break;
								}
								ThoughtHandler thoughts = mood.thoughts;
								if (thoughts == null)
								{
									break;
								}
								MemoryThoughtHandler memories = thoughts.memories;
								if (memories == null)
								{
									break;
								}
								memories.TryGainMemory(ThoughtDefOf.PyroUsed, null, null);
								break;
							}
						}
					}
				}
				return;
			}
			Pawn_NeedsTracker pawn_NeedsTracker2 = this.needs;
			if (pawn_NeedsTracker2 == null)
			{
				return;
			}
			Need_Mood mood2 = pawn_NeedsTracker2.mood;
			if (mood2 == null)
			{
				return;
			}
			ThoughtHandler thoughts2 = mood2.thoughts;
			if (thoughts2 == null)
			{
				return;
			}
			MemoryThoughtHandler memories2 = thoughts2.memories;
			if (memories2 == null)
			{
				return;
			}
			memories2.TryGainMemory(ThoughtDefOf.PyroUsed, null, null);
		}

		// Token: 0x0600234E RID: 9038 RVA: 0x000D2EAC File Offset: 0x000D10AC
		private bool IsValidPyroThoughtTarget(Thing thing)
		{
			Pawn pawn = thing as Pawn;
			return pawn != null && !pawn.Downed && !pawn.IsPsychologicallyInvisible() && !pawn.Fogged() && pawn.HostileTo(Faction.OfPlayer);
		}

		// Token: 0x0600234F RID: 9039 RVA: 0x000D2EE8 File Offset: 0x000D10E8
		public List<WorkTypeDef> GetDisabledWorkTypes(bool permanentOnly = false)
		{
			Pawn.<>c__DisplayClass375_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.permanentOnly = permanentOnly;
			if (Scribe.mode != LoadSaveMode.Inactive)
			{
				this.cachedDisabledWorkTypesPermanent = null;
				this.cachedDisabledWorkTypes = null;
			}
			if (CS$<>8__locals1.permanentOnly)
			{
				if (this.cachedDisabledWorkTypesPermanent == null)
				{
					this.cachedDisabledWorkTypesPermanent = new List<WorkTypeDef>();
					this.<GetDisabledWorkTypes>g__FillList|375_0(this.cachedDisabledWorkTypesPermanent, ref CS$<>8__locals1);
				}
				return this.cachedDisabledWorkTypesPermanent;
			}
			if (this.cachedDisabledWorkTypes == null)
			{
				this.cachedDisabledWorkTypes = new List<WorkTypeDef>();
				this.<GetDisabledWorkTypes>g__FillList|375_0(this.cachedDisabledWorkTypes, ref CS$<>8__locals1);
			}
			return this.cachedDisabledWorkTypes;
		}

		// Token: 0x06002350 RID: 9040 RVA: 0x000D2F74 File Offset: 0x000D1174
		public List<string> GetReasonsForDisabledWorkType(WorkTypeDef workType)
		{
			List<string> result;
			if (this.cachedReasonsForDisabledWorkTypes != null && this.cachedReasonsForDisabledWorkTypes.TryGetValue(workType, out result))
			{
				return result;
			}
			List<string> list = new List<string>();
			foreach (BackstoryDef backstoryDef in this.story.AllBackstories)
			{
				foreach (WorkTypeDef workTypeDef in backstoryDef.DisabledWorkTypes)
				{
					if (workType == workTypeDef)
					{
						list.Add("WorkDisabledByBackstory".Translate(backstoryDef.TitleCapFor(this.gender)));
						break;
					}
				}
			}
			for (int i = 0; i < this.story.traits.allTraits.Count; i++)
			{
				Trait trait = this.story.traits.allTraits[i];
				using (List<WorkTypeDef>.Enumerator enumerator2 = trait.GetDisabledWorkTypes().GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current == workType && !trait.Suppressed)
						{
							list.Add("WorkDisabledByTrait".Translate(trait.LabelCap));
							break;
						}
					}
				}
			}
			if (this.royalty != null)
			{
				foreach (RoyalTitle royalTitle in this.royalty.AllTitlesForReading)
				{
					if (royalTitle.conceited)
					{
						foreach (WorkTypeDef workTypeDef2 in royalTitle.def.DisabledWorkTypes)
						{
							if (workType == workTypeDef2)
							{
								list.Add("WorkDisabledByRoyalTitle".Translate(royalTitle.Label));
								break;
							}
						}
					}
				}
			}
			if (ModsConfig.IdeologyActive && this.Ideo != null)
			{
				Precept_Role role = this.Ideo.GetRole(this);
				if (role != null)
				{
					foreach (WorkTypeDef workTypeDef3 in role.DisabledWorkTypes)
					{
						if (workType == workTypeDef3)
						{
							list.Add("WorkDisabledRole".Translate(role.LabelForPawn(this)));
							break;
						}
					}
				}
			}
			if (this.IsMutant)
			{
				foreach (WorkTypeDef workTypeDef4 in this.mutant.Def.DisabledWorkTypes)
				{
					if (workType == workTypeDef4)
					{
						list.Add(this.mutant.Def.LabelCap);
						break;
					}
				}
			}
			foreach (QuestPart_WorkDisabled questPart_WorkDisabled in QuestUtility.GetWorkDisabledQuestPart(this))
			{
				foreach (WorkTypeDef workTypeDef5 in questPart_WorkDisabled.DisabledWorkTypes)
				{
					if (workType == workTypeDef5)
					{
						list.Add("WorkDisabledByQuest".Translate(questPart_WorkDisabled.quest.name));
						break;
					}
				}
			}
			if (this.guest != null && this.guest.IsSlave)
			{
				foreach (WorkTypeDef workTypeDef6 in this.guest.GetDisabledWorkTypes())
				{
					if (workType == workTypeDef6)
					{
						list.Add("WorkDisabledSlave".Translate());
						break;
					}
				}
			}
			if (this.health != null)
			{
				foreach (WorkTypeDef workTypeDef7 in this.health.DisabledWorkTypes)
				{
					if (workType == workTypeDef7)
					{
						list.Add("WorkDisabledHealth".Translate());
						break;
					}
				}
			}
			int value;
			if (this.IsWorkTypeDisabledByAge(workType, out value))
			{
				list.Add("WorkDisabledAge".Translate(this, this.ageTracker.AgeBiologicalYears, workType.labelShort, value));
			}
			if (this.cachedReasonsForDisabledWorkTypes == null)
			{
				this.cachedReasonsForDisabledWorkTypes = new Dictionary<WorkTypeDef, List<string>>();
			}
			this.cachedReasonsForDisabledWorkTypes[workType] = list;
			return list;
		}

		// Token: 0x06002351 RID: 9041 RVA: 0x000D3498 File Offset: 0x000D1698
		public bool WorkTypeIsDisabled(WorkTypeDef w)
		{
			return this.GetDisabledWorkTypes(false).Contains(w);
		}

		// Token: 0x06002352 RID: 9042 RVA: 0x000D34A8 File Offset: 0x000D16A8
		public bool OneOfWorkTypesIsDisabled(List<WorkTypeDef> wts)
		{
			for (int i = 0; i < wts.Count; i++)
			{
				if (this.WorkTypeIsDisabled(wts[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002353 RID: 9043 RVA: 0x000D34D8 File Offset: 0x000D16D8
		public void Notify_DisabledWorkTypesChanged()
		{
			this.cachedDisabledWorkTypes = null;
			this.cachedDisabledWorkTypesPermanent = null;
			this.cachedReasonsForDisabledWorkTypes = null;
			Pawn_WorkSettings pawn_WorkSettings = this.workSettings;
			if (pawn_WorkSettings != null)
			{
				pawn_WorkSettings.Notify_DisabledWorkTypesChanged();
			}
			Pawn_SkillTracker pawn_SkillTracker = this.skills;
			if (pawn_SkillTracker == null)
			{
				return;
			}
			pawn_SkillTracker.Notify_SkillDisablesChanged();
		}

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x06002354 RID: 9044 RVA: 0x000D3510 File Offset: 0x000D1710
		public WorkTags CombinedDisabledWorkTags
		{
			get
			{
				Pawn_StoryTracker pawn_StoryTracker = this.story;
				WorkTags workTags = (pawn_StoryTracker != null) ? pawn_StoryTracker.DisabledWorkTagsBackstoryTraitsAndGenes : WorkTags.None;
				workTags |= this.kindDef.disabledWorkTags;
				if (this.royalty != null)
				{
					foreach (RoyalTitle royalTitle in this.royalty.AllTitlesForReading)
					{
						if (royalTitle.conceited)
						{
							workTags |= royalTitle.def.disabledWorkTags;
						}
					}
				}
				if (ModsConfig.IdeologyActive && this.Ideo != null)
				{
					Precept_Role role = this.Ideo.GetRole(this);
					if (role != null)
					{
						workTags |= role.def.roleDisabledWorkTags;
					}
				}
				Pawn_HealthTracker pawn_HealthTracker = this.health;
				if (((pawn_HealthTracker != null) ? pawn_HealthTracker.hediffSet : null) != null)
				{
					foreach (Hediff hediff in this.health.hediffSet.hediffs)
					{
						HediffStage curStage = hediff.CurStage;
						if (curStage != null)
						{
							workTags |= curStage.disabledWorkTags;
						}
					}
				}
				foreach (QuestPart_WorkDisabled questPart_WorkDisabled in QuestUtility.GetWorkDisabledQuestPart(this))
				{
					workTags |= questPart_WorkDisabled.disabledWorkTags;
				}
				if (this.IsMutant)
				{
					workTags |= this.mutant.Def.workDisables;
					if (!this.mutant.IsPassive)
					{
						workTags &= ~WorkTags.Violent;
					}
				}
				return workTags;
			}
		}

		// Token: 0x06002355 RID: 9045 RVA: 0x000D36B0 File Offset: 0x000D18B0
		public bool WorkTagIsDisabled(WorkTags w)
		{
			return (this.CombinedDisabledWorkTags & w) > WorkTags.None;
		}

		// Token: 0x06002356 RID: 9046 RVA: 0x000D36C0 File Offset: 0x000D18C0
		public override bool PreventPlayerSellingThingsNearby(out string reason)
		{
			if (base.Faction.HostileTo(Faction.OfPlayer) && this.HostFaction == null && !this.Downed && !this.InMentalState)
			{
				reason = "Enemies".Translate();
				return true;
			}
			reason = null;
			return false;
		}

		// Token: 0x06002357 RID: 9047 RVA: 0x000D370E File Offset: 0x000D190E
		public void ChangeKind(PawnKindDef newKindDef)
		{
			if (this.kindDef == newKindDef)
			{
				return;
			}
			this.kindDef = newKindDef;
			if (this.IsWildMan())
			{
				this.mindState.WildManEverReachedOutside = false;
				ReachabilityUtility.ClearCacheFor(this);
			}
		}

		// Token: 0x06002358 RID: 9048 RVA: 0x000D373C File Offset: 0x000D193C
		public bool CompsWantHoldWeapon()
		{
			using (List<ThingComp>.Enumerator enumerator = base.AllComps.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.WantHoldWeapon(this))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002359 RID: 9049 RVA: 0x000D3798 File Offset: 0x000D1998
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.canBeDormant = base.GetComp<CompCanBeDormant>();
				this.activity = base.GetComp<CompActivity>();
			}
			Scribe_Defs.Look<PawnKindDef>(ref this.kindDef, "kindDef");
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.Male, false);
			Scribe_Values.Look<int>(ref this.becameWorldPawnTickAbs, "becameWorldPawnTickAbs", -1, false);
			Scribe_Values.Look<bool>(ref this.teleporting, "teleporting", false, false);
			Scribe_Values.Look<int>(ref this.showNamePromptOnTick, "showNamePromptOnTick", -1, false);
			Scribe_Values.Look<int>(ref this.babyNamingDeadline, "babyNamingDeadline", -1, false);
			Scribe_Values.Look<bool>(ref this.addCorpseToLord, "addCorpseToLord", false, false);
			Scribe_Values.Look<int>(ref this.timesRaisedAsShambler, "timesRaisedAsShambler", 0, false);
			Scribe_Values.Look<int>(ref this.lastSleepDisturbedTick, "lastSleepDisturbedTick", 0, false);
			Scribe_Values.Look<bool>(ref this.dontGivePreArrivalPathway, "dontGivePreArrivalPathway", false, false);
			Scribe_Values.Look<int>(ref this.lastVacuumBurntTick, "lastVacuumBurntTick", 0, false);
			Scribe_Deep.Look<Name>(ref this.nameInt, "name", Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.Saving && GenTicks.TicksGame - this.deadlifeDustFactionTick > 12500)
			{
				this.deadlifeDustFactionTick = 0;
				this.deadlifeDustFaction = null;
			}
			Scribe_Values.Look<int>(ref this.deadlifeDustFactionTick, "deadlifeDustFactionTick", 0, false);
			Scribe_References.Look<Faction>(ref this.deadlifeDustFaction, "deadlifeDustFaction", false);
			Scribe_Deep.Look<Pawn_MindState>(ref this.mindState, "mindState", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_JobTracker>(ref this.jobs, "jobs", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_StanceTracker>(ref this.stances, "stances", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_InfectionVectorTracker>(ref this.infectionVectors, "infectionVectors", new object[]
			{
				this
			});
			Scribe_Deep.Look<VerbTracker>(ref this.verbTracker, "verbTracker", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_NativeVerbs>(ref this.natives, "natives", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_MeleeVerbs>(ref this.meleeVerbs, "meleeVerbs", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_RotationTracker>(ref this.rotationTracker, "rotationTracker", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_PathFollower>(ref this.pather, "pather", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_CarryTracker>(ref this.carryTracker, "carryTracker", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_ApparelTracker>(ref this.apparel, "apparel", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_StoryTracker>(ref this.story, "story", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_EquipmentTracker>(ref this.equipment, "equipment", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_DraftController>(ref this.drafter, "drafter", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_AgeTracker>(ref this.ageTracker, "ageTracker", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_HealthTracker>(ref this.health, "healthTracker", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_RecordsTracker>(ref this.records, "records", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_InventoryTracker>(ref this.inventory, "inventory", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_FilthTracker>(ref this.filth, "filth", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_RopeTracker>(ref this.roping, "roping", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_NeedsTracker>(ref this.needs, "needs", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_GuestTracker>(ref this.guest, "guest", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_GuiltTracker>(ref this.guilt, "guilt", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_RoyaltyTracker>(ref this.royalty, "royalty", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_RelationsTracker>(ref this.relations, "social", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_PsychicEntropyTracker>(ref this.psychicEntropy, "psychicEntropy", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_MutantTracker>(ref this.mutant, "shambler", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_Ownership>(ref this.ownership, "ownership", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_InteractionsTracker>(ref this.interactions, "interactions", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_SkillTracker>(ref this.skills, "skills", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_AbilityTracker>(ref this.abilities, "abilities", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_IdeoTracker>(ref this.ideo, "ideo", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_WorkSettings>(ref this.workSettings, "workSettings", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_TraderTracker>(ref this.trader, "trader", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_OutfitTracker>(ref this.outfits, "outfits", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_DrugPolicyTracker>(ref this.drugs, "drugs", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_FoodRestrictionTracker>(ref this.foodRestriction, "foodRestriction", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_TimetableTracker>(ref this.timetable, "timetable", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_PlayerSettings>(ref this.playerSettings, "playerSettings", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_TrainingTracker>(ref this.training, "training", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_StyleTracker>(ref this.style, "style", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_StyleObserverTracker>(ref this.styleObserver, "styleObserver", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_ConnectionsTracker>(ref this.connections, "connections", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_InventoryStockTracker>(ref this.inventoryStock, "inventoryStock", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_SurroundingsTracker>(ref this.surroundings, "treeSightings", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_Thinker>(ref this.thinker, "thinker", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_MechanitorTracker>(ref this.mechanitor, "mechanitor", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_GeneTracker>(ref this.genes, "genes", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_LearningTracker>(ref this.learning, "learning", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_ReadingTracker>(ref this.reading, "reading", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_CreepJoinerTracker>(ref this.creepjoiner, "creepjoiner", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_DuplicateTracker>(ref this.duplicate, "duplicate", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_FlightTracker>(ref this.flight, "flight", new object[]
			{
				this
			});
			Scribe_Values.Look<bool>(ref this.wasLeftBehindStartingPawn, "wasLeftBehindStartingPawn", false, false);
			Scribe_Values.Look<bool>(ref this.everLostEgo, "everBrainWiped", false, false);
			Scribe_Values.Look<bool>(ref this.wasDraftedBeforeSkip, "wasDraftedBeforeSkip", false, false);
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x0600235A RID: 9050 RVA: 0x000D3E90 File Offset: 0x000D2090
		public override string ToString()
		{
			if (this.story != null)
			{
				return this.LabelShort;
			}
			if (this.thingIDNumber > 0)
			{
				return base.ThingID;
			}
			if (this.kindDef != null)
			{
				return this.KindLabel + "_" + base.ThingID;
			}
			if (this.def != null)
			{
				return base.ThingID;
			}
			return base.GetType().ToString();
		}

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x0600235B RID: 9051 RVA: 0x000D3EF5 File Offset: 0x000D20F5
		public TraderKindDef TraderKind
		{
			get
			{
				Pawn_TraderTracker pawn_TraderTracker = this.trader;
				if (pawn_TraderTracker == null)
				{
					return null;
				}
				return pawn_TraderTracker.traderKind;
			}
		}

		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x0600235C RID: 9052 RVA: 0x000D3F08 File Offset: 0x000D2108
		public TradeCurrency TradeCurrency
		{
			get
			{
				return this.TraderKind.tradeCurrency;
			}
		}

		// Token: 0x17000726 RID: 1830
		// (get) Token: 0x0600235D RID: 9053 RVA: 0x000D3F15 File Offset: 0x000D2115
		public IEnumerable<Thing> Goods
		{
			get
			{
				return this.trader.Goods;
			}
		}

		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x0600235E RID: 9054 RVA: 0x000D3F22 File Offset: 0x000D2122
		public int RandomPriceFactorSeed
		{
			get
			{
				return this.trader.RandomPriceFactorSeed;
			}
		}

		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x0600235F RID: 9055 RVA: 0x000D3F2F File Offset: 0x000D212F
		public string TraderName
		{
			get
			{
				return this.trader.TraderName;
			}
		}

		// Token: 0x17000729 RID: 1833
		// (get) Token: 0x06002360 RID: 9056 RVA: 0x000D3F3C File Offset: 0x000D213C
		public bool CanTradeNow
		{
			get
			{
				return this.trader != null && this.trader.CanTradeNow;
			}
		}

		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x06002361 RID: 9057 RVA: 0x000D3F53 File Offset: 0x000D2153
		public float TradePriceImprovementOffsetForPlayer
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x06002362 RID: 9058 RVA: 0x000D3F5A File Offset: 0x000D215A
		public float BodySize
		{
			get
			{
				return this.ageTracker.CurLifeStage.bodySizeFactor * this.RaceProps.baseBodySize;
			}
		}

		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x06002363 RID: 9059 RVA: 0x000D3F78 File Offset: 0x000D2178
		public float HealthScale
		{
			get
			{
				return this.ageTracker.CurLifeStage.healthScaleFactor * this.RaceProps.baseHealthScale;
			}
		}

		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x06002364 RID: 9060 RVA: 0x000D3F96 File Offset: 0x000D2196
		public IEnumerable<Thing> EquippedWornOrInventoryThings
		{
			get
			{
				IEnumerable<Thing> innerContainer = this.inventory.innerContainer;
				Pawn_ApparelTracker pawn_ApparelTracker = this.apparel;
				IEnumerable<Thing> first = innerContainer.ConcatIfNotNull((pawn_ApparelTracker != null) ? pawn_ApparelTracker.WornApparel : null);
				Pawn_EquipmentTracker pawn_EquipmentTracker = this.equipment;
				return first.ConcatIfNotNull((pawn_EquipmentTracker != null) ? pawn_EquipmentTracker.AllEquipmentListForReading : null);
			}
		}

		// Token: 0x06002365 RID: 9061 RVA: 0x000D3FD1 File Offset: 0x000D21D1
		public IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
		{
			return this.trader.ColonyThingsWillingToBuy(playerNegotiator);
		}

		// Token: 0x06002366 RID: 9062 RVA: 0x000D3FDF File Offset: 0x000D21DF
		public void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			this.trader.GiveSoldThingToTrader(toGive, countToGive, playerNegotiator);
		}

		// Token: 0x06002367 RID: 9063 RVA: 0x000D3FEF File Offset: 0x000D21EF
		public void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			this.trader.GiveSoldThingToPlayer(toGive, countToGive, playerNegotiator);
		}

		// Token: 0x1700072E RID: 1838
		// (get) Token: 0x06002368 RID: 9064 RVA: 0x000D3FFF File Offset: 0x000D21FF
		Thing IAttackTarget.Thing
		{
			get
			{
				return this;
			}
		}

		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x06002369 RID: 9065 RVA: 0x000D4002 File Offset: 0x000D2202
		public float TargetPriorityFactor
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x17000730 RID: 1840
		// (get) Token: 0x0600236A RID: 9066 RVA: 0x000D400C File Offset: 0x000D220C
		public LocalTargetInfo TargetCurrentlyAimingAt
		{
			get
			{
				if (!base.Spawned)
				{
					return LocalTargetInfo.Invalid;
				}
				Stance curStance = this.stances.curStance;
				if (curStance is Stance_Warmup || curStance is Stance_Cooldown)
				{
					return ((Stance_Busy)curStance).focusTarg;
				}
				return LocalTargetInfo.Invalid;
			}
		}

		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x0600236B RID: 9067 RVA: 0x000D4054 File Offset: 0x000D2254
		Thing IAttackTargetSearcher.Thing
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x0600236C RID: 9068 RVA: 0x000D4057 File Offset: 0x000D2257
		public LocalTargetInfo LastAttackedTarget
		{
			get
			{
				return this.mindState.lastAttackedTarget;
			}
		}

		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x0600236D RID: 9069 RVA: 0x000D4064 File Offset: 0x000D2264
		public int LastAttackTargetTick
		{
			get
			{
				return this.mindState.lastAttackTargetTick;
			}
		}

		// Token: 0x17000734 RID: 1844
		// (get) Token: 0x0600236E RID: 9070 RVA: 0x000D4074 File Offset: 0x000D2274
		public Verb CurrentEffectiveVerb
		{
			get
			{
				Building_Turret building_Turret = this.MannedThing() as Building_Turret;
				if (building_Turret != null)
				{
					return building_Turret.AttackVerb;
				}
				return this.TryGetAttackVerb(null, !this.IsColonist, false);
			}
		}

		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x0600236F RID: 9071 RVA: 0x000D40A8 File Offset: 0x000D22A8
		private bool ForceNoDeathNotification
		{
			get
			{
				return this.forceNoDeathNotification || this.kindDef.forceNoDeathNotification;
			}
		}

		// Token: 0x06002370 RID: 9072 RVA: 0x000D40BF File Offset: 0x000D22BF
		string IVerbOwner.UniqueVerbOwnerID()
		{
			return base.GetUniqueLoadID();
		}

		// Token: 0x06002371 RID: 9073 RVA: 0x000D40C7 File Offset: 0x000D22C7
		bool IVerbOwner.VerbsStillUsableBy(Pawn p)
		{
			return p == this;
		}

		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x06002372 RID: 9074 RVA: 0x000D40CD File Offset: 0x000D22CD
		Thing IVerbOwner.ConstantCaster
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x06002373 RID: 9075 RVA: 0x000D40D0 File Offset: 0x000D22D0
		ImplementOwnerTypeDef IVerbOwner.ImplementOwnerTypeDef
		{
			get
			{
				return ImplementOwnerTypeDefOf.Bodypart;
			}
		}

		// Token: 0x06002374 RID: 9076 RVA: 0x000D40D7 File Offset: 0x000D22D7
		public PlanetTile GetRootTile()
		{
			return base.Tile;
		}

		// Token: 0x06002375 RID: 9077 RVA: 0x000D40DF File Offset: 0x000D22DF
		public ThingOwner GetDirectlyHeldThings()
		{
			return null;
		}

		// Token: 0x06002376 RID: 9078 RVA: 0x000D40E4 File Offset: 0x000D22E4
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
			if (this.inventory != null)
			{
				outChildren.Add(this.inventory);
			}
			if (this.carryTracker != null)
			{
				outChildren.Add(this.carryTracker);
			}
			if (this.equipment != null)
			{
				outChildren.Add(this.equipment);
			}
			if (this.apparel != null)
			{
				outChildren.Add(this.apparel);
			}
		}

		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x06002377 RID: 9079 RVA: 0x000D414D File Offset: 0x000D234D
		public BillStack BillStack
		{
			get
			{
				return this.health.surgeryBills;
			}
		}

		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x06002378 RID: 9080 RVA: 0x000D415C File Offset: 0x000D235C
		public override IntVec3 InteractionCell
		{
			get
			{
				Building_Bed building_Bed = this.CurrentBed();
				IntVec3? intVec = (building_Bed != null) ? building_Bed.FindPreferredInteractionCell(base.Position, null) : null;
				if (intVec == null)
				{
					return base.InteractionCell;
				}
				return intVec.GetValueOrDefault();
			}
		}

		// Token: 0x06002379 RID: 9081 RVA: 0x000D41A4 File Offset: 0x000D23A4
		public bool CurrentlyUsableForBills()
		{
			if (!this.InBed())
			{
				JobFailReason.Is(Pawn.NotSurgeryReadyTrans, null);
				return false;
			}
			if (!this.InteractionCell.IsValid)
			{
				JobFailReason.Is(Pawn.CannotReachTrans, null);
				return false;
			}
			return true;
		}

		// Token: 0x0600237A RID: 9082 RVA: 0x000D41E4 File Offset: 0x000D23E4
		public bool UsableForBillsAfterFueling()
		{
			return this.CurrentlyUsableForBills();
		}

		// Token: 0x0600237B RID: 9083 RVA: 0x000D41EC File Offset: 0x000D23EC
		public void Notify_BillDeleted(Bill bill)
		{
			Xenogerm xenogerm = bill.xenogerm;
			if (xenogerm == null)
			{
				return;
			}
			xenogerm.Notify_BillRemoved();
		}

		// Token: 0x0600237C RID: 9084 RVA: 0x000D41FE File Offset: 0x000D23FE
		public bool Equals(Pawn other)
		{
			return this.def.defName == other.def.defName && this.thingIDNumber == other.thingIDNumber;
		}

		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x0600237D RID: 9085 RVA: 0x000D422D File Offset: 0x000D242D
		public ThingOwner SearchableContents
		{
			get
			{
				Pawn_CarryTracker pawn_CarryTracker = this.carryTracker;
				if (pawn_CarryTracker == null)
				{
					return null;
				}
				return pawn_CarryTracker.innerContainer;
			}
		}

		// Token: 0x06002390 RID: 9104 RVA: 0x000D4460 File Offset: 0x000D2660
		[CompilerGenerated]
		private void <GetDisabledWorkTypes>g__FillList|375_0(List<WorkTypeDef> list, ref Pawn.<>c__DisplayClass375_0 A_2)
		{
			if (this.IsMutant && this.mutant.HasTurned)
			{
				List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					foreach (WorkTypeDef item in this.mutant.Def.DisabledWorkTypes)
					{
						if (!list.Contains(item))
						{
							list.Add(item);
						}
					}
				}
				return;
			}
			if (this.story != null && !this.IsSlave)
			{
				foreach (BackstoryDef backstoryDef in this.story.AllBackstories)
				{
					foreach (WorkTypeDef item2 in backstoryDef.DisabledWorkTypes)
					{
						if (!list.Contains(item2))
						{
							list.Add(item2);
						}
					}
				}
				for (int j = 0; j < this.story.traits.allTraits.Count; j++)
				{
					if (!this.story.traits.allTraits[j].Suppressed)
					{
						foreach (WorkTypeDef item3 in this.story.traits.allTraits[j].GetDisabledWorkTypes())
						{
							if (!list.Contains(item3))
							{
								list.Add(item3);
							}
						}
					}
				}
			}
			if (ModsConfig.BiotechActive && this.IsColonyMech)
			{
				List<WorkTypeDef> allDefsListForReading2 = DefDatabase<WorkTypeDef>.AllDefsListForReading;
				for (int k = 0; k < allDefsListForReading2.Count; k++)
				{
					if (!this.RaceProps.mechEnabledWorkTypes.Contains(allDefsListForReading2[k]) && !list.Contains(allDefsListForReading2[k]))
					{
						list.Add(allDefsListForReading2[k]);
					}
				}
			}
			if (!A_2.permanentOnly)
			{
				if (this.health != null)
				{
					foreach (WorkTypeDef item4 in this.health.DisabledWorkTypes)
					{
						if (!list.Contains(item4))
						{
							list.Add(item4);
						}
					}
				}
				if (this.royalty != null && !this.IsSlave)
				{
					foreach (RoyalTitle royalTitle in this.royalty.AllTitlesForReading)
					{
						if (royalTitle.conceited)
						{
							foreach (WorkTypeDef item5 in royalTitle.def.DisabledWorkTypes)
							{
								if (!list.Contains(item5))
								{
									list.Add(item5);
								}
							}
						}
					}
				}
				if (ModsConfig.IdeologyActive && this.Ideo != null)
				{
					Precept_Role role = this.Ideo.GetRole(this);
					if (role != null)
					{
						foreach (WorkTypeDef item6 in role.DisabledWorkTypes)
						{
							if (!list.Contains(item6))
							{
								list.Add(item6);
							}
						}
					}
				}
				if (ModsConfig.BiotechActive && this.genes != null)
				{
					foreach (Gene gene in this.genes.GenesListForReading)
					{
						foreach (WorkTypeDef item7 in gene.DisabledWorkTypes)
						{
							if (!list.Contains(item7))
							{
								list.Add(item7);
							}
						}
					}
				}
				foreach (QuestPart_WorkDisabled questPart_WorkDisabled in QuestUtility.GetWorkDisabledQuestPart(this))
				{
					foreach (WorkTypeDef item8 in questPart_WorkDisabled.DisabledWorkTypes)
					{
						if (!list.Contains(item8))
						{
							list.Add(item8);
						}
					}
				}
				if (this.guest != null)
				{
					foreach (WorkTypeDef item9 in this.guest.GetDisabledWorkTypes())
					{
						if (!list.Contains(item9))
						{
							list.Add(item9);
						}
					}
				}
				for (int l = 0; l < this.RaceProps.lifeStageWorkSettings.Count; l++)
				{
					LifeStageWorkSettings lifeStageWorkSettings = this.RaceProps.lifeStageWorkSettings[l];
					if (lifeStageWorkSettings.IsDisabled(this) && !list.Contains(lifeStageWorkSettings.workType))
					{
						list.Add(lifeStageWorkSettings.workType);
					}
				}
			}
		}

		// Token: 0x040019D3 RID: 6611
		public PawnKindDef kindDef;

		// Token: 0x040019D4 RID: 6612
		private Name nameInt;

		// Token: 0x040019D5 RID: 6613
		public Gender gender;

		// Token: 0x040019D6 RID: 6614
		public Pawn_AgeTracker ageTracker;

		// Token: 0x040019D7 RID: 6615
		public Pawn_HealthTracker health;

		// Token: 0x040019D8 RID: 6616
		public Pawn_RecordsTracker records;

		// Token: 0x040019D9 RID: 6617
		public Pawn_InventoryTracker inventory;

		// Token: 0x040019DA RID: 6618
		public Pawn_MeleeVerbs meleeVerbs;

		// Token: 0x040019DB RID: 6619
		public VerbTracker verbTracker;

		// Token: 0x040019DC RID: 6620
		public Pawn_Ownership ownership;

		// Token: 0x040019DD RID: 6621
		public Pawn_CarryTracker carryTracker;

		// Token: 0x040019DE RID: 6622
		public Pawn_NeedsTracker needs;

		// Token: 0x040019DF RID: 6623
		public Pawn_MindState mindState;

		// Token: 0x040019E0 RID: 6624
		public Pawn_SurroundingsTracker surroundings;

		// Token: 0x040019E1 RID: 6625
		public Pawn_Thinker thinker;

		// Token: 0x040019E2 RID: 6626
		public Pawn_JobTracker jobs;

		// Token: 0x040019E3 RID: 6627
		public Pawn_StanceTracker stances;

		// Token: 0x040019E4 RID: 6628
		public Pawn_InfectionVectorTracker infectionVectors;

		// Token: 0x040019E5 RID: 6629
		public Pawn_DuplicateTracker duplicate;

		// Token: 0x040019E6 RID: 6630
		public Pawn_RotationTracker rotationTracker;

		// Token: 0x040019E7 RID: 6631
		public Pawn_PathFollower pather;

		// Token: 0x040019E8 RID: 6632
		public Pawn_NativeVerbs natives;

		// Token: 0x040019E9 RID: 6633
		public Pawn_FilthTracker filth;

		// Token: 0x040019EA RID: 6634
		public Pawn_RopeTracker roping;

		// Token: 0x040019EB RID: 6635
		public Pawn_FlightTracker flight;

		// Token: 0x040019EC RID: 6636
		public Pawn_EquipmentTracker equipment;

		// Token: 0x040019ED RID: 6637
		public Pawn_ApparelTracker apparel;

		// Token: 0x040019EE RID: 6638
		public Pawn_SkillTracker skills;

		// Token: 0x040019EF RID: 6639
		public Pawn_StoryTracker story;

		// Token: 0x040019F0 RID: 6640
		public Pawn_GuestTracker guest;

		// Token: 0x040019F1 RID: 6641
		public Pawn_GuiltTracker guilt;

		// Token: 0x040019F2 RID: 6642
		public Pawn_RoyaltyTracker royalty;

		// Token: 0x040019F3 RID: 6643
		public Pawn_AbilityTracker abilities;

		// Token: 0x040019F4 RID: 6644
		public Pawn_IdeoTracker ideo;

		// Token: 0x040019F5 RID: 6645
		public Pawn_GeneTracker genes;

		// Token: 0x040019F6 RID: 6646
		public Pawn_CreepJoinerTracker creepjoiner;

		// Token: 0x040019F7 RID: 6647
		public Pawn_WorkSettings workSettings;

		// Token: 0x040019F8 RID: 6648
		public Pawn_TraderTracker trader;

		// Token: 0x040019F9 RID: 6649
		public Pawn_StyleTracker style;

		// Token: 0x040019FA RID: 6650
		public Pawn_StyleObserverTracker styleObserver;

		// Token: 0x040019FB RID: 6651
		public Pawn_ConnectionsTracker connections;

		// Token: 0x040019FC RID: 6652
		public Pawn_TrainingTracker training;

		// Token: 0x040019FD RID: 6653
		public Pawn_CallTracker caller;

		// Token: 0x040019FE RID: 6654
		public Pawn_PsychicEntropyTracker psychicEntropy;

		// Token: 0x040019FF RID: 6655
		public Pawn_MutantTracker mutant;

		// Token: 0x04001A00 RID: 6656
		public Pawn_RelationsTracker relations;

		// Token: 0x04001A01 RID: 6657
		public Pawn_InteractionsTracker interactions;

		// Token: 0x04001A02 RID: 6658
		public Pawn_PlayerSettings playerSettings;

		// Token: 0x04001A03 RID: 6659
		public Pawn_OutfitTracker outfits;

		// Token: 0x04001A04 RID: 6660
		public Pawn_DrugPolicyTracker drugs;

		// Token: 0x04001A05 RID: 6661
		public Pawn_FoodRestrictionTracker foodRestriction;

		// Token: 0x04001A06 RID: 6662
		public Pawn_TimetableTracker timetable;

		// Token: 0x04001A07 RID: 6663
		public Pawn_InventoryStockTracker inventoryStock;

		// Token: 0x04001A08 RID: 6664
		public Pawn_MechanitorTracker mechanitor;

		// Token: 0x04001A09 RID: 6665
		public Pawn_LearningTracker learning;

		// Token: 0x04001A0A RID: 6666
		public Pawn_ReadingTracker reading;

		// Token: 0x04001A0B RID: 6667
		public Pawn_DraftController drafter;

		// Token: 0x04001A0C RID: 6668
		public Lord lord;

		// Token: 0x04001A0D RID: 6669
		public bool markedForDiscard;

		// Token: 0x04001A0E RID: 6670
		private Pawn_DrawTracker drawer;

		// Token: 0x04001A0F RID: 6671
		public int becameWorldPawnTickAbs = -1;

		// Token: 0x04001A10 RID: 6672
		public bool teleporting;

		// Token: 0x04001A11 RID: 6673
		public bool forceNoDeathNotification;

		// Token: 0x04001A12 RID: 6674
		public int showNamePromptOnTick = -1;

		// Token: 0x04001A13 RID: 6675
		public int babyNamingDeadline = -1;

		// Token: 0x04001A14 RID: 6676
		private Sustainer sustainerAmbient;

		// Token: 0x04001A15 RID: 6677
		private Sustainer sustainerMoving;

		// Token: 0x04001A16 RID: 6678
		public bool addCorpseToLord;

		// Token: 0x04001A17 RID: 6679
		public int timesRaisedAsShambler;

		// Token: 0x04001A18 RID: 6680
		private int lastSleepDisturbedTick;

		// Token: 0x04001A19 RID: 6681
		public int lastVacuumBurntTick;

		// Token: 0x04001A1A RID: 6682
		public Map prevMap;

		// Token: 0x04001A1B RID: 6683
		private Faction deadlifeDustFaction;

		// Token: 0x04001A1C RID: 6684
		private int deadlifeDustFactionTick;

		// Token: 0x04001A1D RID: 6685
		public bool wasLeftBehindStartingPawn;

		// Token: 0x04001A1E RID: 6686
		public bool debugMaxMoveSpeed;

		// Token: 0x04001A1F RID: 6687
		public bool wasDraftedBeforeSkip;

		// Token: 0x04001A20 RID: 6688
		public bool dontGivePreArrivalPathway;

		// Token: 0x04001A21 RID: 6689
		public bool everLostEgo;

		// Token: 0x04001A22 RID: 6690
		private const float HumanSizedHeatOutput = 0.3f;

		// Token: 0x04001A23 RID: 6691
		private const float AnimalHeatOutputFactor = 0.6f;

		// Token: 0x04001A24 RID: 6692
		public const int DefaultBabyNamingPeriod = 60000;

		// Token: 0x04001A25 RID: 6693
		public const int DefaultGrowthMomentChoicePeriod = 120000;

		// Token: 0x04001A26 RID: 6694
		private const int SleepDisturbanceMinInterval = 300;

		// Token: 0x04001A27 RID: 6695
		private const int DeadlifeFactionExpiryTicks = 12500;

		// Token: 0x04001A28 RID: 6696
		private const float HeatPushMaxTemperature = 40f;

		// Token: 0x04001A29 RID: 6697
		public const int MaxMoveTicks = 450;

		// Token: 0x04001A2A RID: 6698
		private static string NotSurgeryReadyTrans;

		// Token: 0x04001A2B RID: 6699
		private static string CannotReachTrans;

		// Token: 0x04001A2C RID: 6700
		[Unsaved(false)]
		private CompOverseerSubject overseerSubject;

		// Token: 0x04001A2D RID: 6701
		[Unsaved(false)]
		public CompCanBeDormant canBeDormant;

		// Token: 0x04001A2E RID: 6702
		[Unsaved(false)]
		public CompActivity activity;

		// Token: 0x04001A2F RID: 6703
		private static List<ExtraFaction> tmpExtraFactions = new List<ExtraFaction>();

		// Token: 0x04001A30 RID: 6704
		private static List<string> states = new List<string>();

		// Token: 0x04001A31 RID: 6705
		private List<WorkTypeDef> cachedDisabledWorkTypes;

		// Token: 0x04001A32 RID: 6706
		private List<WorkTypeDef> cachedDisabledWorkTypesPermanent;

		// Token: 0x04001A33 RID: 6707
		private Dictionary<WorkTypeDef, List<string>> cachedReasonsForDisabledWorkTypes;
	}
}
