using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001928 RID: 6440
	public class CompOverseerSubject : ThingComp
	{
		// Token: 0x17001BE8 RID: 7144
		// (get) Token: 0x0600A18F RID: 41359 RVA: 0x0039BE71 File Offset: 0x0039A071
		public CompProperties_OverseerSubject Props
		{
			get
			{
				return (CompProperties_OverseerSubject)this.props;
			}
		}

		// Token: 0x17001BE9 RID: 7145
		// (get) Token: 0x0600A190 RID: 41360 RVA: 0x0039BE7E File Offset: 0x0039A07E
		public Pawn Parent
		{
			get
			{
				return (Pawn)this.parent;
			}
		}

		// Token: 0x17001BEA RID: 7146
		// (get) Token: 0x0600A191 RID: 41361 RVA: 0x0039BE8B File Offset: 0x0039A08B
		public OverseerSubjectState State
		{
			get
			{
				Pawn overseer = this.Overseer;
				if (((overseer != null) ? overseer.mechanitor : null) == null)
				{
					return OverseerSubjectState.RequiresOverseer;
				}
				if (this.Overseer.mechanitor.ControlledPawns.Contains(this.Parent))
				{
					return OverseerSubjectState.Overseen;
				}
				return OverseerSubjectState.RequiresBandwidth;
			}
		}

		// Token: 0x17001BEB RID: 7147
		// (get) Token: 0x0600A192 RID: 41362 RVA: 0x0039BEC3 File Offset: 0x0039A0C3
		private Pawn Overseer
		{
			get
			{
				Pawn pawn = (Pawn)this.parent;
				if (pawn == null)
				{
					return null;
				}
				Pawn_RelationsTracker relations = pawn.relations;
				if (relations == null)
				{
					return null;
				}
				return relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Overseer, null);
			}
		}

		// Token: 0x17001BEC RID: 7148
		// (get) Token: 0x0600A193 RID: 41363 RVA: 0x0039BEEC File Offset: 0x0039A0EC
		public int DelayUntilFeralCheckTicks
		{
			get
			{
				return this.delayUntilFeralCheck;
			}
		}

		// Token: 0x0600A194 RID: 41364 RVA: 0x0039BEF4 File Offset: 0x0039A0F4
		public void Notify_DisconnectedFromOverseer()
		{
			if (this.Parent.Drafted)
			{
				this.Parent.drafter.Drafted = false;
			}
			Pawn_CarryTracker carryTracker = this.Parent.carryTracker;
			if (((carryTracker != null) ? carryTracker.CarriedThing : null) != null)
			{
				Thing thing;
				this.Parent.carryTracker.TryDropCarriedThing(this.Parent.Position, ThingPlaceMode.Near, out thing, null);
			}
			Pawn_JobTracker jobs = this.Parent.jobs;
			if (jobs != null)
			{
				jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
			Pawn_JobTracker jobs2 = this.Parent.jobs;
			if (jobs2 != null)
			{
				jobs2.CheckForJobOverride(0f, true);
			}
			this.delayUntilFeralCheck = this.Props.delayUntilFeralCheck;
		}

		// Token: 0x0600A195 RID: 41365 RVA: 0x0039BF9F File Offset: 0x0039A19F
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad)
			{
				this.delayUntilFeralCheck = this.Props.delayUntilFeralCheck;
			}
		}

		// Token: 0x0600A196 RID: 41366 RVA: 0x0039BFB5 File Offset: 0x0039A1B5
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			if (!ModLister.CheckBiotech("Overseer subject"))
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			Effecter effecter = this.effect;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.effect = null;
			base.PostDeSpawn(map, mode);
		}

		// Token: 0x0600A197 RID: 41367 RVA: 0x0039BFF0 File Offset: 0x0039A1F0
		public override void CompTick()
		{
			base.CompTick();
			if (!CompOverseerSubject.debugDisableNeedsOverseerEffect && CompOverseerSubject.CanGoFeral(this.Parent))
			{
				if (this.Props.needsOverseerEffect != null && this.effect == null)
				{
					this.effect = this.Props.needsOverseerEffect.SpawnAttached(this.parent, this.parent.Map, 1f);
				}
			}
			else
			{
				Effecter effecter = this.effect;
				if (effecter != null)
				{
					effecter.Cleanup();
				}
				this.effect = null;
			}
			Effecter effecter2 = this.effect;
			if (effecter2 != null)
			{
				effecter2.EffectTick(this.parent, this.parent);
			}
			if (CompOverseerSubject.CanGoFeral(this.Parent))
			{
				if (this.delayUntilFeralCheck > 0)
				{
					this.delayUntilFeralCheck--;
				}
				if (this.delayUntilFeralCheck <= 0 && Rand.MTBEventOccurs((float)this.Props.feralMtbDays, 60000f, 1f))
				{
					this.TryMakeFeral();
				}
			}
		}

		// Token: 0x0600A198 RID: 41368 RVA: 0x0039C0EA File Offset: 0x0039A2EA
		private static bool CanGoFeral(Pawn pawn)
		{
			return pawn.Spawned && pawn.Awake() && pawn.IsColonyMechRequiringMechanitor();
		}

		// Token: 0x0600A199 RID: 41369 RVA: 0x0039C104 File Offset: 0x0039A304
		public bool TryMakeFeral()
		{
			this.tmpFeralPawns.Clear();
			this.tmpFeralPawns.Add(this.Parent);
			foreach (Thing thing in GenRadial.RadialDistinctThingsAround(this.parent.Position, this.parent.Map, (float)this.Props.feralCascadeRadialDistance, true))
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null && !this.tmpFeralPawns.Contains(pawn) && CompOverseerSubject.CanGoFeral(pawn))
				{
					this.tmpFeralPawns.Add(pawn);
				}
			}
			for (int i = 0; i < this.tmpFeralPawns.Count; i++)
			{
				this.tmpFeralPawns[i].OverseerSubject.ForceFeral();
			}
			IEnumerable<IGrouping<PawnKindDef, Pawn>> source = from p in this.tmpFeralPawns
			group p by p.kindDef;
			Find.LetterStack.ReceiveLetter("LetterLabelMechsFeral".Translate(), "LetterMechsFeral".Translate(Faction.OfMechanoids, (from g in source
			select g.Key.LabelCap + " x" + g.Count<Pawn>().ToString()).ToLineList(" - ", false)), LetterDefOf.ThreatBig, this.tmpFeralPawns, null, null, null, null, 0, true);
			for (int j = 0; j < this.tmpFeralPawns.Count; j++)
			{
				Lord lord = this.tmpFeralPawns[j].GetLord();
				if (lord != null)
				{
					lord.Notify_PawnLost(this.tmpFeralPawns[j], PawnLostCondition.ForcedToJoinOtherLord, null);
				}
			}
			LordMaker.MakeNewLord(Faction.OfMechanoids, new LordJob_ExitMapBest(LocomotionUrgency.Jog, false, true), this.parent.MapHeld, this.tmpFeralPawns);
			for (int k = 0; k < this.tmpFeralPawns.Count; k++)
			{
				if (this.tmpFeralPawns[k].CurJob != null)
				{
					this.tmpFeralPawns[k].jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			}
			this.tmpFeralPawns.Clear();
			return true;
		}

		// Token: 0x0600A19A RID: 41370 RVA: 0x0039C348 File Offset: 0x0039A548
		private void ForceFeral()
		{
			Pawn overseer = this.Overseer;
			if (overseer != null)
			{
				Messages.Message("MessageMechanitorLostControlOfMech".Translate(overseer, this.Parent) + ": " + this.Parent.LabelShortCap, new LookTargets(new Pawn[]
				{
					this.Parent,
					overseer
				}), MessageTypeDefOf.NeutralEvent, true);
				this.Parent.relations.RemoveDirectRelation(PawnRelationDefOf.Overseer, overseer);
			}
			this.parent.SetFaction(Faction.OfMechanoids, null);
		}

		// Token: 0x0600A19B RID: 41371 RVA: 0x0039C3E4 File Offset: 0x0039A5E4
		public override string CompInspectStringExtra()
		{
			if (this.parent.Faction == Faction.OfPlayer)
			{
				StringBuilder stringBuilder = new StringBuilder();
				Pawn overseer = this.Overseer;
				TaggedString taggedString = "Overseer".Translate();
				if (((overseer != null) ? overseer.mechanitor : null) != null)
				{
					taggedString += ": " + this.Overseer.LabelShort;
					if (!overseer.mechanitor.ControlledPawns.Contains(this.parent))
					{
						taggedString += " (" + "InsufficientBandwidth".Translate() + ")";
					}
				}
				else
				{
					taggedString += ": " + "OverseerNone".Translate();
				}
				stringBuilder.Append(taggedString);
				if (this.State != OverseerSubjectState.Overseen)
				{
					TaggedString ts = (this.delayUntilFeralCheck > 0) ? ("Uncontrolled".Translate() + " (" + (this.Props.delayUntilFeralCheck - this.delayUntilFeralCheck).ToStringTicksToPeriod(true, true, true, true, false) + ")") : ("Danger".Translate() + ": " + "MayGoFeral".Translate());
					stringBuilder.AppendInNewLine(ts.Colorize(ColorLibrary.RedReadable));
				}
				return stringBuilder.ToString();
			}
			return null;
		}

		// Token: 0x0600A19C RID: 41372 RVA: 0x0039C544 File Offset: 0x0039A744
		public override void Notify_AbandonedAtTile(PlanetTile tile)
		{
			base.Notify_AbandonedAtTile(tile);
			Pawn overseer = this.Overseer;
			if (overseer != null)
			{
				this.Parent.relations.RemoveDirectRelation(PawnRelationDefOf.Overseer, overseer);
			}
		}

		// Token: 0x0600A19D RID: 41373 RVA: 0x0039C578 File Offset: 0x0039A778
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompOverseerSubject.<CompGetGizmosExtra>d__23 <CompGetGizmosExtra>d__ = new CompOverseerSubject.<CompGetGizmosExtra>d__23(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0600A19E RID: 41374 RVA: 0x0039C588 File Offset: 0x0039A788
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.delayUntilFeralCheck, "delayUntilFeralCheck", 0, false);
		}

		// Token: 0x04005F78 RID: 24440
		private int delayUntilFeralCheck;

		// Token: 0x04005F79 RID: 24441
		private Effecter effect;

		// Token: 0x04005F7A RID: 24442
		public static bool debugDisableNeedsOverseerEffect;

		// Token: 0x04005F7B RID: 24443
		private List<Pawn> tmpFeralPawns = new List<Pawn>();
	}
}
