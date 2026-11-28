using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020018A9 RID: 6313
	public class CompGoldenCube : CompInteractable, IRoofCollapseAlert
	{
		// Token: 0x17001B15 RID: 6933
		// (get) Token: 0x06009E09 RID: 40457 RVA: 0x0038BB26 File Offset: 0x00389D26
		public new CompProperties_GoldenCube Props
		{
			get
			{
				return (CompProperties_GoldenCube)this.props;
			}
		}

		// Token: 0x17001B16 RID: 6934
		// (get) Token: 0x06009E0A RID: 40458 RVA: 0x0038BB34 File Offset: 0x00389D34
		public CompStudyUnlocks StudyUnlocks
		{
			get
			{
				CompStudyUnlocks result;
				if ((result = this.studyUnlocksInt) == null)
				{
					result = (this.studyUnlocksInt = this.parent.GetComp<CompStudyUnlocks>());
				}
				return result;
			}
		}

		// Token: 0x17001B17 RID: 6935
		// (get) Token: 0x06009E0B RID: 40459 RVA: 0x0038BB5F File Offset: 0x00389D5F
		public bool Deactivatable
		{
			get
			{
				return this.StudyUnlocks.Completed;
			}
		}

		// Token: 0x06009E0C RID: 40460 RVA: 0x0038BB6C File Offset: 0x00389D6C
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			if (base.ValidateTarget(target, false))
			{
				this.OrderDeactivation(target.Pawn);
			}
		}

		// Token: 0x06009E0D RID: 40461 RVA: 0x0038BB88 File Offset: 0x00389D88
		public override void CompTickInterval(int delta)
		{
			base.CompTickInterval(delta);
			if (!this.parent.IsHashIntervalTick(2500, delta))
			{
				return;
			}
			Pawn pawn;
			if ((!this.AnyPawnHasHediff() || Rand.MTBEventOccurs(12f, 60000f, 2500f)) && QuestUtility.TryGetIdealColonist(out pawn, this.parent.MapHeld, new Func<Pawn, bool>(this.ValidatePawn)))
			{
				this.GiveHediff(pawn);
			}
		}

		// Token: 0x06009E0E RID: 40462 RVA: 0x0038BBF8 File Offset: 0x00389DF8
		private void GiveHediff(Pawn pawn)
		{
			pawn.health.AddHediff(HediffDefOf.CubeInterest, null, null, null);
			Messages.Message("MessageGoldenCubeInterest".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.NeutralEvent, true);
		}

		// Token: 0x06009E0F RID: 40463 RVA: 0x0038BC4C File Offset: 0x00389E4C
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			CompGoldenCube.<CompFloatMenuOptions>d__12 <CompFloatMenuOptions>d__ = new CompGoldenCube.<CompFloatMenuOptions>d__12(-2);
			<CompFloatMenuOptions>d__.<>4__this = this;
			<CompFloatMenuOptions>d__.<>3__selPawn = selPawn;
			return <CompFloatMenuOptions>d__;
		}

		// Token: 0x06009E10 RID: 40464 RVA: 0x0038BC63 File Offset: 0x00389E63
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompGoldenCube.<CompGetGizmosExtra>d__13 <CompGetGizmosExtra>d__ = new CompGoldenCube.<CompGetGizmosExtra>d__13(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x06009E11 RID: 40465 RVA: 0x0038BC73 File Offset: 0x00389E73
		public override string CompInspectStringExtra()
		{
			return "";
		}

		// Token: 0x06009E12 RID: 40466 RVA: 0x0038BC7C File Offset: 0x00389E7C
		protected override void OnInteracted(Pawn caster)
		{
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			foreach (Pawn pawn in Find.WorldPawns.AllPawnsAlive)
			{
				if (pawn.health.hediffSet.HasHediff(HediffDefOf.CubeInterest, false) && pawn.IsCaravanMember() && pawn.Faction == Faction.OfPlayer)
				{
					PawnBanishUtility.Banish(pawn, false);
					list2.Add(pawn.LabelShort);
				}
				this.CurePawn(pawn);
			}
			foreach (Map map in Find.Maps)
			{
				for (int i = map.mapPawns.AllPawns.Count - 1; i >= 0; i--)
				{
					Pawn pawn2 = map.mapPawns.AllPawns[i];
					if (pawn2.health.hediffSet.HasHediff(HediffDefOf.CubeInterest, false))
					{
						PawnUtility.ForceEjectFromContainer(pawn2);
						pawn2.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Berserk, null, true, false, false, null, true, false, false);
						list.Add(pawn2.LabelShort);
					}
					this.CurePawn(pawn2);
				}
			}
			if (list.Any<string>() || list2.Any<string>())
			{
				TaggedString taggedString = "CubeDeactivationBerserkDesc".Translate(caster.Named("PAWN"));
				if (list.Any<string>())
				{
					taggedString += "\n\n" + "CubeDeactivationBerserkPawns".Translate() + ":\n" + list.ToLineList("- ");
				}
				if (list2.Any<string>())
				{
					taggedString += "\n\n" + "CubeDeactivationLostPawns".Translate() + ":\n" + list2.ToLineList("- ");
				}
				Find.LetterStack.ReceiveLetter("CubeDeactivationBerserk".Translate(), taggedString, list.Any<string>() ? LetterDefOf.ThreatBig : LetterDefOf.NegativeEvent, null, 0, true);
			}
			this.parent.Kill(null, null);
		}

		// Token: 0x06009E13 RID: 40467 RVA: 0x0038BED8 File Offset: 0x0038A0D8
		private bool AnyPawnHasHediff()
		{
			if (this.parent.SpawnedOrAnyParentSpawned)
			{
				foreach (Pawn pawn in this.parent.MapHeld.mapPawns.AllPawns)
				{
					if (pawn.IsColonist && pawn.health.hediffSet.HasHediff(HediffDefOf.CubeInterest, false))
					{
						return true;
					}
				}
			}
			foreach (Map map in Find.Maps)
			{
				if (map != this.parent.MapHeld)
				{
					foreach (Pawn pawn2 in map.mapPawns.AllPawns)
					{
						Hediff hediff;
						if (pawn2.IsColonist && pawn2.health.hediffSet.TryGetHediff(HediffDefOf.CubeInterest, out hediff))
						{
							return true;
						}
					}
				}
			}
			foreach (Pawn pawn3 in Find.WorldPawns.AllPawnsAlive)
			{
				Hediff hediff;
				if (pawn3.IsColonist && pawn3.health.hediffSet.TryGetHediff(HediffDefOf.CubeInterest, out hediff))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06009E14 RID: 40468 RVA: 0x0038C08C File Offset: 0x0038A28C
		public override void Notify_Killed(Map prevMap, DamageInfo? dinfo = null)
		{
			Thing thing = ThingMaker.MakeThing(ThingDefOf.Gold, null);
			thing.stackCount = CompGoldenCube.GoldRange.RandomInRange;
			GenPlace.TryPlaceThing(thing, this.parent.PositionHeld, prevMap, ThingPlaceMode.Near, null, null, null, 1);
		}

		// Token: 0x06009E15 RID: 40469 RVA: 0x0038C0D8 File Offset: 0x0038A2D8
		private void CurePawn(Pawn pawn)
		{
			Hediff hediff;
			if (pawn.health.hediffSet.TryGetHediff(HediffDefOf.CubeInterest, out hediff))
			{
				pawn.health.RemoveHediff(hediff);
			}
			Hediff hediff2;
			if (pawn.health.hediffSet.TryGetHediff(HediffDefOf.CubeWithdrawal, out hediff2))
			{
				pawn.health.RemoveHediff(hediff2);
			}
			Hediff hediff3;
			if (pawn.health.hediffSet.TryGetHediff(HediffDefOf.CubeComa, out hediff3))
			{
				pawn.health.RemoveHediff(hediff3);
			}
		}

		// Token: 0x06009E16 RID: 40470 RVA: 0x0038C154 File Offset: 0x0038A354
		public override AcceptanceReport CanInteract(Pawn activateBy = null, bool checkOptionalItems = true)
		{
			AcceptanceReport result = base.CanInteract(activateBy, checkOptionalItems);
			if (!result.Accepted)
			{
				return result;
			}
			if (activateBy != null)
			{
				if (activateBy.health.hediffSet.HasHediff(HediffDefOf.CubeInterest, false))
				{
					return "CannotDisableCube".Translate(activateBy.Named("PAWN"));
				}
				if (checkOptionalItems && !activateBy.HasReserved(ThingDefOf.Shard) && !activateBy.CanReserveAndReachableOfDef(ThingDefOf.Shard, PathEndMode.Touch, Danger.None, 1, -1, null, false))
				{
					return "NoItemReservedOrReachable".Translate(ThingDefOf.Shard.label);
				}
			}
			else if (checkOptionalItems && !ReservationUtility.ExistsUnreservedAmountOfDef(this.parent.MapHeld, ThingDefOf.Shard, Faction.OfPlayer, 1, null))
			{
				return "NoItemReserved".Translate(ThingDefOf.Shard.label);
			}
			return true;
		}

		// Token: 0x06009E17 RID: 40471 RVA: 0x0038C234 File Offset: 0x0038A434
		private void OrderDeactivation(Pawn pawn)
		{
			TaggedString taggedString = "CubeDeactivationConfirmation".Translate();
			List<Pawn> list = Find.Maps.SelectMany((Map m) => from p in m.mapPawns.AllPawns
			where p.Faction == Faction.OfPlayer && p.health.hediffSet.HasHediff(HediffDefOf.CubeInterest, false)
			select p).ToList<Pawn>();
			List<Pawn> list2 = Find.WorldObjects.Caravans.SelectMany((Caravan c) => from p in c.pawns.InnerListForReading
			where p.Faction == Faction.OfPlayer && p.health.hediffSet.HasHediff(HediffDefOf.CubeInterest, false)
			select p).ToList<Pawn>();
			if (list.Any<Pawn>())
			{
				taggedString += "\n\n" + "CubeDeactivationConfirmationPawnsBerserk".Translate() + ":\n" + (from p in list
				select p.LabelShort).ToLineList("- ", false);
			}
			if (list2.Any<Pawn>())
			{
				taggedString += "\n\n" + "CubeDeactivationConfirmationPawnsLost".Translate() + ":\n" + (from p in list2
				select p.LabelShort).ToLineList("- ", false);
			}
			taggedString += "\n\n" + "CubeDeactivationConfirmationEnd".Translate();
			Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(taggedString, delegate()
			{
				Thing t;
				if (!pawn.TryFindReserveAndReachableOfDef(ThingDefOf.Shard, out t, PathEndMode.Touch, Danger.None, 1, -1, null, false))
				{
					return;
				}
				Job job = JobMaker.MakeJob(JobDefOf.InteractThing, this.parent, t);
				job.count = 1;
				job.playerForced = true;
				pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, false, null, WindowLayer.Dialog));
		}

		// Token: 0x06009E18 RID: 40472 RVA: 0x0038C3C4 File Offset: 0x0038A5C4
		private bool ValidatePawn(Pawn pawn)
		{
			if (pawn.health.hediffSet.HasHediff(HediffDefOf.CubeInterest, false) || pawn.health.hediffSet.HasHediff(HediffDefOf.CubeComa, false))
			{
				return false;
			}
			if (pawn.MapHeld != this.parent.MapHeld)
			{
				return false;
			}
			if (this.parent.IsInCaravan())
			{
				Caravan caravan = pawn.GetCaravan();
				bool? flag;
				if (caravan == null)
				{
					flag = null;
				}
				else
				{
					IEnumerable<Thing> allThings = caravan.AllThings;
					flag = ((allThings != null) ? new bool?(allThings.Contains(this.parent)) : null);
				}
				bool? flag2 = flag;
				return flag2.GetValueOrDefault();
			}
			return true;
		}

		// Token: 0x06009E19 RID: 40473 RVA: 0x0038C468 File Offset: 0x0038A668
		public RoofCollapseResponse Notify_OnBeforeRoofCollapse()
		{
			IntVec3 cell;
			if (RCellFinder.TryFindRandomCellNearWith(this.parent.Position, (IntVec3 c) => CompGoldenCube.IsValidCell(c, this.parent.MapHeld), this.parent.MapHeld, out cell, 10, 2147483647))
			{
				SkipUtility.SkipTo(this.parent, cell, this.parent.MapHeld);
			}
			return RoofCollapseResponse.RemoveThing;
		}

		// Token: 0x06009E1A RID: 40474 RVA: 0x0038C4C0 File Offset: 0x0038A6C0
		private static bool IsValidCell(IntVec3 cell, Map map)
		{
			return cell.InBounds(map) && cell.Walkable(map);
		}

		// Token: 0x04005D85 RID: 23941
		private CompStudyUnlocks studyUnlocksInt;

		// Token: 0x04005D86 RID: 23942
		private const float EnthrallMTBDays = 12f;

		// Token: 0x04005D87 RID: 23943
		private static readonly IntRange GoldRange = new IntRange(125, 175);
	}
}
