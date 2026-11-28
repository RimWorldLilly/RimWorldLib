using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020018ED RID: 6381
	public class CompMechGestatorTank : ThingComp, IThingGlower
	{
		// Token: 0x17001B93 RID: 7059
		// (get) Token: 0x06009FF6 RID: 40950 RVA: 0x0039500D File Offset: 0x0039320D
		private CompProperties_MechGestatorTank Props
		{
			get
			{
				return (CompProperties_MechGestatorTank)this.props;
			}
		}

		// Token: 0x06009FF7 RID: 40951 RVA: 0x0039501C File Offset: 0x0039321C
		public override string CompInspectStringExtra()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("Contains".Translate());
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>(((this.state == CompMechGestatorTank.TankState.Empty) ? "Nothing" : "Unknown").Translate().CapitalizeFirst());
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06009FF8 RID: 40952 RVA: 0x0039507E File Offset: 0x0039327E
		public bool ShouldBeLitNow()
		{
			return this.state > CompMechGestatorTank.TankState.Empty;
		}

		// Token: 0x06009FF9 RID: 40953 RVA: 0x00395089 File Offset: 0x00393289
		public override bool DontDrawParent()
		{
			return true;
		}

		// Token: 0x17001B94 RID: 7060
		// (get) Token: 0x06009FFA RID: 40954 RVA: 0x0039508C File Offset: 0x0039328C
		// (set) Token: 0x06009FFB RID: 40955 RVA: 0x00395094 File Offset: 0x00393294
		public CompMechGestatorTank.TankState State
		{
			get
			{
				return this.state;
			}
			set
			{
				this.state = value;
				if (this.parent.Spawned)
				{
					this.parent.DirtyMapMesh(this.parent.Map);
					CompGlower compGlower = this.parent.TryGetComp<CompGlower>();
					if (compGlower == null)
					{
						return;
					}
					compGlower.UpdateLit(this.parent.Map);
				}
			}
		}

		// Token: 0x06009FFC RID: 40956 RVA: 0x003950EB File Offset: 0x003932EB
		public override void PostPostMake()
		{
			this.triggerRadius = this.Props.triggerRadiusRange.RandomInRange;
		}

		// Token: 0x06009FFD RID: 40957 RVA: 0x00395103 File Offset: 0x00393303
		public override void CompTick()
		{
			if (this.state != CompMechGestatorTank.TankState.Proximity)
			{
				return;
			}
			if (this.parent.IsHashIntervalTick(250))
			{
				this.CheckTrigger();
			}
		}

		// Token: 0x06009FFE RID: 40958 RVA: 0x00395128 File Offset: 0x00393328
		private void CheckTrigger()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			foreach (IntVec3 intVec in GenRadial.RadialCellsAround(this.parent.Position, this.triggerRadius, false))
			{
				if (intVec.InBounds(this.parent.Map) && GenSight.LineOfSight(this.parent.Position, intVec, this.parent.Map))
				{
					foreach (Thing thing in intVec.GetThingList(this.parent.Map))
					{
						Pawn pawn = thing as Pawn;
						if (pawn != null && pawn.IsColonist)
						{
							this.Trigger(this.parent.Map);
							return;
						}
					}
				}
			}
		}

		// Token: 0x06009FFF RID: 40959 RVA: 0x00395230 File Offset: 0x00393430
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			if (mode != DestroyMode.WillReplace && this.state != CompMechGestatorTank.TankState.Empty)
			{
				this.Trigger(previousMap);
			}
		}

		// Token: 0x0600A000 RID: 40960 RVA: 0x00395248 File Offset: 0x00393448
		private void Trigger(Map map)
		{
			CompMechGestatorTank.<>c__DisplayClass17_0 CS$<>8__locals1 = new CompMechGestatorTank.<>c__DisplayClass17_0();
			CS$<>8__locals1.map = map;
			if (this.state == CompMechGestatorTank.TankState.Empty)
			{
				return;
			}
			this.State = CompMechGestatorTank.TankState.Empty;
			IntVec3 loc = this.parent.OccupiedRect().ExpandedBy(1).EdgeCells.Where(new Func<IntVec3, bool>(CS$<>8__locals1.<Trigger>g__Standable|0)).RandomElementWithFallback(IntVec3.Invalid);
			ScatterDebrisUtility.ScatterFilthAroundThing(this.parent, CS$<>8__locals1.map, ThingDefOf.Filth_GestationFluid, CompMechGestatorTank.GestationFluidFilthRange, 1, null);
			if (!loc.IsValid)
			{
				return;
			}
			Pawn pawn = PawnGenerator.GeneratePawn(this.Props.mechKindOptions.RandomElementByWeight((PawnKindDefWeight x) => x.weight).kindDef, Faction.OfMechanoids, null);
			GenSpawn.Spawn(pawn, loc, CS$<>8__locals1.map, WipeMode.Vanish);
			Pawn_StanceTracker stances = pawn.stances;
			if (stances != null)
			{
				StunHandler stunner = stances.stunner;
				if (stunner != null)
				{
					stunner.StunFor(180, null, false, true, false);
				}
			}
			LordJob_AssaultColony lordJob_AssaultColony;
			if (!CS$<>8__locals1.map.lordManager.TryGetLordByJob<LordJob_AssaultColony>(Faction.OfMechanoids, out lordJob_AssaultColony))
			{
				LordMaker.MakeNewLord(Faction.OfMechanoids, new LordJob_AssaultColony(Faction.OfMechanoids, false, false, false, false, false, false, false), CS$<>8__locals1.map, new List<Pawn>
				{
					pawn
				});
			}
			else
			{
				lordJob_AssaultColony.lord.AddPawn(pawn);
			}
			Messages.Message(this.Props.triggeredMessage.Formatted(pawn), pawn, MessageTypeDefOf.NegativeEvent, true);
			this.Props.triggerSound.PlayOneShot(this.parent);
		}

		// Token: 0x0600A001 RID: 40961 RVA: 0x003953EC File Offset: 0x003935EC
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompMechGestatorTank.<CompGetGizmosExtra>d__18 <CompGetGizmosExtra>d__ = new CompMechGestatorTank.<CompGetGizmosExtra>d__18(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0600A002 RID: 40962 RVA: 0x003953FC File Offset: 0x003935FC
		public override void PostPrintOnto(SectionLayer layer)
		{
			((this.state == CompMechGestatorTank.TankState.Empty) ? this.Props.emptyGraphic.Graphic : this.Props.dormantGraphic.Graphic).Print(layer, this.parent, 0f);
		}

		// Token: 0x0600A003 RID: 40963 RVA: 0x00395439 File Offset: 0x00393639
		public override void PostExposeData()
		{
			Scribe_Values.Look<CompMechGestatorTank.TankState>(ref this.state, "state", CompMechGestatorTank.TankState.Empty, false);
			Scribe_Values.Look<float>(ref this.triggerRadius, "triggerRadius", 0f, false);
		}

		// Token: 0x04005EA0 RID: 24224
		private CompMechGestatorTank.TankState state;

		// Token: 0x04005EA1 RID: 24225
		private float triggerRadius;

		// Token: 0x04005EA2 RID: 24226
		private const int StunTicks = 180;

		// Token: 0x04005EA3 RID: 24227
		private static readonly IntRange GestationFluidFilthRange = new IntRange(2, 4);

		// Token: 0x0200366A RID: 13930
		public enum TankState
		{
			// Token: 0x0400D97F RID: 55679
			Empty,
			// Token: 0x0400D980 RID: 55680
			Dormant,
			// Token: 0x0400D981 RID: 55681
			Proximity
		}
	}
}
