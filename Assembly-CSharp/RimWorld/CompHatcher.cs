using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020018BC RID: 6332
	public class CompHatcher : ThingComp
	{
		// Token: 0x17001B37 RID: 6967
		// (get) Token: 0x06009EB5 RID: 40629 RVA: 0x0038F382 File Offset: 0x0038D582
		public CompProperties_Hatcher Props
		{
			get
			{
				return (CompProperties_Hatcher)this.props;
			}
		}

		// Token: 0x17001B38 RID: 6968
		// (get) Token: 0x06009EB6 RID: 40630 RVA: 0x0038F38F File Offset: 0x0038D58F
		private CompTemperatureRuinable FreezerComp
		{
			get
			{
				return this.parent.GetComp<CompTemperatureRuinable>();
			}
		}

		// Token: 0x17001B39 RID: 6969
		// (get) Token: 0x06009EB7 RID: 40631 RVA: 0x0038F39C File Offset: 0x0038D59C
		public bool TemperatureDamaged
		{
			get
			{
				return this.FreezerComp != null && this.FreezerComp.Ruined;
			}
		}

		// Token: 0x06009EB8 RID: 40632 RVA: 0x0038F3B4 File Offset: 0x0038D5B4
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.gestateProgress, "gestateProgress", 0f, false);
			Scribe_References.Look<Pawn>(ref this.hatcheeParent, "hatcheeParent", false);
			Scribe_References.Look<Pawn>(ref this.otherParent, "otherParent", false);
			Scribe_References.Look<Faction>(ref this.hatcheeFaction, "hatcheeFaction", false);
		}

		// Token: 0x06009EB9 RID: 40633 RVA: 0x0038F410 File Offset: 0x0038D610
		public override void CompTick()
		{
			if (!this.TemperatureDamaged)
			{
				float num = 1f / (this.Props.hatcherDaystoHatch * 60000f);
				this.gestateProgress += num;
				if (this.gestateProgress > 1f)
				{
					this.Hatch();
					return;
				}
			}
			else
			{
				PawnKindDef hatcherPawn = this.Props.hatcherPawn;
				CompProperties_EggLayer compProperties_EggLayer;
				if (hatcherPawn == null)
				{
					compProperties_EggLayer = null;
				}
				else
				{
					ThingDef race = hatcherPawn.race;
					compProperties_EggLayer = ((race != null) ? race.GetCompProperties<CompProperties_EggLayer>() : null);
				}
				CompProperties_EggLayer compProperties_EggLayer2 = compProperties_EggLayer;
				if (compProperties_EggLayer2 != null)
				{
					Pawn_CarryTracker pawn_CarryTracker = this.parent.ParentHolder as Pawn_CarryTracker;
					if (pawn_CarryTracker != null)
					{
						Thing thing;
						pawn_CarryTracker.TryDropCarriedThing(this.parent.Position, ThingPlaceMode.Near, out thing, null);
					}
					CompEggContainer compEggContainer = this.parent.ParentHolder as CompEggContainer;
					if (compEggContainer != null)
					{
						int stackCount = this.parent.stackCount;
						compEggContainer.innerContainer.Remove(this.parent);
						this.parent.Destroy(DestroyMode.Vanish);
						Thing thing2 = ThingMaker.MakeThing(compProperties_EggLayer2.eggUnfertilizedDef, null);
						thing2.stackCount = stackCount;
						compEggContainer.innerContainer.TryAdd(thing2, true);
						return;
					}
					Map mapHeld = this.parent.MapHeld;
					IntVec3 positionHeld = this.parent.PositionHeld;
					int stackCount2 = this.parent.stackCount;
					this.parent.Destroy(DestroyMode.Vanish);
					GenSpawn.Spawn(compProperties_EggLayer2.eggUnfertilizedDef, positionHeld, mapHeld, WipeMode.Vanish).stackCount = stackCount2;
				}
			}
		}

		// Token: 0x06009EBA RID: 40634 RVA: 0x0038F568 File Offset: 0x0038D768
		public void Hatch()
		{
			try
			{
				PawnGenerationRequest request = new PawnGenerationRequest(this.Props.hatcherPawn, this.hatcheeFaction, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Newborn, null, null, null, false, false, false, -1, 0, false);
				Pawn_CarryTracker pawn_CarryTracker = this.parent.ParentHolder as Pawn_CarryTracker;
				if (pawn_CarryTracker != null)
				{
					Thing thing;
					pawn_CarryTracker.TryDropCarriedThing(this.parent.PositionHeld, ThingPlaceMode.Near, out thing, null);
				}
				for (int i = 0; i < this.parent.stackCount; i++)
				{
					Pawn pawn = PawnGenerator.GeneratePawn(request);
					if (PawnUtility.TrySpawnHatchedOrBornPawn(pawn, this.parent, null))
					{
						if (pawn != null)
						{
							if (this.hatcheeParent != null)
							{
								if (pawn.playerSettings != null && this.hatcheeParent.playerSettings != null && this.hatcheeParent.Faction == this.hatcheeFaction)
								{
									pawn.playerSettings.AreaRestrictionInPawnCurrentMap = this.hatcheeParent.playerSettings.AreaRestrictionInPawnCurrentMap;
								}
								if (pawn.RaceProps.IsFlesh)
								{
									pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, this.hatcheeParent);
								}
							}
							if (this.otherParent != null && (this.hatcheeParent == null || this.hatcheeParent.gender != this.otherParent.gender) && pawn.RaceProps.IsFlesh)
							{
								pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, this.otherParent);
							}
						}
						if (this.parent.Spawned)
						{
							FilthMaker.TryMakeFilth(this.parent.Position, this.parent.Map, ThingDefOf.Filth_AmnioticFluid, 1, FilthSourceFlags.None, true);
						}
					}
					else
					{
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
					}
				}
			}
			finally
			{
				this.parent.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06009EBB RID: 40635 RVA: 0x0038F7B0 File Offset: 0x0038D9B0
		public override bool AllowStackWith(Thing other)
		{
			CompHatcher comp = ((ThingWithComps)other).GetComp<CompHatcher>();
			return this.TemperatureDamaged == comp.TemperatureDamaged && base.AllowStackWith(other);
		}

		// Token: 0x06009EBC RID: 40636 RVA: 0x0038F7E0 File Offset: 0x0038D9E0
		public override void PreAbsorbStack(Thing otherStack, int count)
		{
			float t = (float)count / (float)(this.parent.stackCount + count);
			float b = ((ThingWithComps)otherStack).GetComp<CompHatcher>().gestateProgress;
			this.gestateProgress = Mathf.Lerp(this.gestateProgress, b, t);
		}

		// Token: 0x06009EBD RID: 40637 RVA: 0x0038F823 File Offset: 0x0038DA23
		public override void PostSplitOff(Thing piece)
		{
			CompHatcher comp = ((ThingWithComps)piece).GetComp<CompHatcher>();
			comp.gestateProgress = this.gestateProgress;
			comp.hatcheeParent = this.hatcheeParent;
			comp.otherParent = this.otherParent;
			comp.hatcheeFaction = this.hatcheeFaction;
		}

		// Token: 0x06009EBE RID: 40638 RVA: 0x0038F85F File Offset: 0x0038DA5F
		public override void PrePreTraded(TradeAction action, Pawn playerNegotiator, ITrader trader)
		{
			base.PrePreTraded(action, playerNegotiator, trader);
			if (action == TradeAction.PlayerBuys)
			{
				this.hatcheeFaction = Faction.OfPlayer;
				return;
			}
			if (action == TradeAction.PlayerSells)
			{
				this.hatcheeFaction = trader.Faction;
			}
		}

		// Token: 0x06009EBF RID: 40639 RVA: 0x0038F88A File Offset: 0x0038DA8A
		public override void PostPostGeneratedForTrader(TraderKindDef trader, PlanetTile forTile, Faction forFaction)
		{
			base.PostPostGeneratedForTrader(trader, forTile, forFaction);
			this.hatcheeFaction = forFaction;
		}

		// Token: 0x06009EC0 RID: 40640 RVA: 0x0038F89C File Offset: 0x0038DA9C
		public override string CompInspectStringExtra()
		{
			if (!this.TemperatureDamaged)
			{
				return "EggProgress".Translate() + ": " + this.gestateProgress.ToStringPercent() + "\n" + "HatchesIn".Translate() + ": " + "PeriodDays".Translate((this.Props.hatcherDaystoHatch * (1f - this.gestateProgress)).ToString("F1"));
			}
			return null;
		}

		// Token: 0x04005DEA RID: 24042
		private float gestateProgress;

		// Token: 0x04005DEB RID: 24043
		public Pawn hatcheeParent;

		// Token: 0x04005DEC RID: 24044
		public Pawn otherParent;

		// Token: 0x04005DED RID: 24045
		public Faction hatcheeFaction;
	}
}
