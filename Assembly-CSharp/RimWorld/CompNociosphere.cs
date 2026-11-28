using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001915 RID: 6421
	public class CompNociosphere : CompInteractable, IActivity, IRoofCollapseAlert
	{
		// Token: 0x17001BBF RID: 7103
		// (get) Token: 0x0600A0E3 RID: 41187 RVA: 0x00398598 File Offset: 0x00396798
		public new CompProperties_Nociosphere Props
		{
			get
			{
				return (CompProperties_Nociosphere)this.props;
			}
		}

		// Token: 0x17001BC0 RID: 7104
		// (get) Token: 0x0600A0E4 RID: 41188 RVA: 0x003985A8 File Offset: 0x003967A8
		public CompCauseHediff_AoE AoEComp
		{
			get
			{
				CompCauseHediff_AoE result;
				if ((result = this.aoeComp) == null)
				{
					result = (this.aoeComp = this.Pawn.GetComp<CompCauseHediff_AoE>());
				}
				return result;
			}
		}

		// Token: 0x17001BC1 RID: 7105
		// (get) Token: 0x0600A0E5 RID: 41189 RVA: 0x003985D4 File Offset: 0x003967D4
		public CompPushable Pushable
		{
			get
			{
				CompPushable result;
				if ((result = this.pushComp) == null)
				{
					result = (this.pushComp = this.Pawn.GetComp<CompPushable>());
				}
				return result;
			}
		}

		// Token: 0x17001BC2 RID: 7106
		// (get) Token: 0x0600A0E6 RID: 41190 RVA: 0x00398600 File Offset: 0x00396800
		public CompStudyUnlocks StudyUnlocks
		{
			get
			{
				CompStudyUnlocks result;
				if ((result = this.studyUnlocks) == null)
				{
					result = (this.studyUnlocks = this.Pawn.GetComp<CompStudyUnlocks>());
				}
				return result;
			}
		}

		// Token: 0x17001BC3 RID: 7107
		// (get) Token: 0x0600A0E7 RID: 41191 RVA: 0x0039862C File Offset: 0x0039682C
		public CompActivity Activity
		{
			get
			{
				CompActivity result;
				if ((result = this.activityComp) == null)
				{
					result = (this.activityComp = this.Pawn.GetComp<CompActivity>());
				}
				return result;
			}
		}

		// Token: 0x17001BC4 RID: 7108
		// (get) Token: 0x0600A0E8 RID: 41192 RVA: 0x00398657 File Offset: 0x00396857
		public Pawn Pawn
		{
			get
			{
				return (Pawn)this.parent;
			}
		}

		// Token: 0x17001BC5 RID: 7109
		// (get) Token: 0x0600A0E9 RID: 41193 RVA: 0x00398664 File Offset: 0x00396864
		public bool CanSend
		{
			get
			{
				return this.Pawn.IsOnHoldingPlatform && this.StudyUnlocks.Progress >= 2;
			}
		}

		// Token: 0x17001BC6 RID: 7110
		// (get) Token: 0x0600A0EA RID: 41194 RVA: 0x00398686 File Offset: 0x00396886
		public bool IsUnstable
		{
			get
			{
				return this.unstableTick > 0 && Find.TickManager.TicksGame > this.unstableTick;
			}
		}

		// Token: 0x17001BC7 RID: 7111
		// (get) Token: 0x0600A0EB RID: 41195 RVA: 0x003986A5 File Offset: 0x003968A5
		public bool IsBecomingUnstable
		{
			get
			{
				return this.unstableTick > 0 && Find.TickManager.TicksGame < this.unstableTick;
			}
		}

		// Token: 0x17001BC8 RID: 7112
		// (get) Token: 0x0600A0EC RID: 41196 RVA: 0x003986C4 File Offset: 0x003968C4
		public override string ExposeKey
		{
			get
			{
				return "Nociosphere";
			}
		}

		// Token: 0x0600A0ED RID: 41197 RVA: 0x003986CB File Offset: 0x003968CB
		public override void Notify_KilledLeavingsLeft(List<Thing> leavings)
		{
			if (this.thingsIgnoredByExplosion == null)
			{
				this.thingsIgnoredByExplosion = new List<Thing>();
			}
			else
			{
				this.thingsIgnoredByExplosion.Clear();
			}
			this.thingsIgnoredByExplosion.AddRange(leavings);
		}

		// Token: 0x0600A0EE RID: 41198 RVA: 0x003986FC File Offset: 0x003968FC
		public override void Notify_Killed(Map prevMap, DamageInfo? dinfo = null)
		{
			IntVec3 positionHeld = this.Pawn.PositionHeld;
			float radius = 4.9f;
			DamageDef vaporize = DamageDefOf.Vaporize;
			Thing pawn = this.Pawn;
			int damAmount = -1;
			float armorPenetration = -1f;
			SoundDef explosionSound = null;
			ThingDef weapon = null;
			ThingDef projectile = null;
			Thing intendedTarget = null;
			ThingDef postExplosionSpawnThingDef = null;
			float postExplosionSpawnChance = 0f;
			int postExplosionSpawnThingCount = 1;
			List<Thing> ignoredThings = this.thingsIgnoredByExplosion;
			GenExplosion.DoExplosion(positionHeld, prevMap, radius, vaporize, pawn, damAmount, armorPenetration, explosionSound, weapon, projectile, intendedTarget, postExplosionSpawnThingDef, postExplosionSpawnChance, postExplosionSpawnThingCount, null, null, 255, false, null, 0f, 1, 0f, false, null, ignoredThings, null, true, 1f, 0f, true, null, 1f, null, null, null, null);
			Find.LetterStack.ReceiveLetter("NociosphereDefeatedLabel".Translate(), "NociosphereDefeatedText".Translate(), LetterDefOf.NeutralEvent, new TargetInfo(this.parent.Position, prevMap, false), null, null, null, null, 0, true);
		}

		// Token: 0x0600A0EF RID: 41199 RVA: 0x003987D0 File Offset: 0x003969D0
		public override void CompTick()
		{
			float num = 0f;
			if (this.Activity.IsDormant)
			{
				this.segScaleGoal = 0f;
			}
			else
			{
				if (!this.IsUnstable && this.ShouldGoPassive())
				{
					this.Activity.EnterPassiveState();
				}
				if (this.unstableTick > 0)
				{
					this.unstableTick++;
				}
				num = 0.5f + Mathf.Sin((float)GenTicks.TicksAbs / 60f) * 0.2f;
				if (this.segScale >= 0.99f)
				{
					this.segScaleGoal = 0f;
				}
				foreach (Ability ability in this.Pawn.abilities.abilities)
				{
					if (ability.Casting && ability.verb.verbProps.warmupTime > 0f)
					{
						num += ability.verb.WarmupProgress * 0.7f;
						break;
					}
				}
			}
			this.segScale = Mathf.SmoothDamp(this.segScale, Mathf.Clamp01(this.segScaleGoal + num), ref this.segScaleVel, 0.1f, 10f, 0.016666668f);
			if (this.becomingUnstableTick <= 0)
			{
				this.becomingUnstableTick = Find.TickManager.TicksGame + Mathf.RoundToInt(CompNociosphere.BecomingUnstableRangeDays.RandomInRange * 60000f);
			}
			else if (Find.TickManager.TicksGame > this.becomingUnstableTick && this.Activity.IsDormant)
			{
				if (this.unstableTick <= 0)
				{
					this.unstableTick = Find.TickManager.TicksGame + Mathf.RoundToInt(600000f);
					TaggedString taggedString = "NociosphereBecomingUnstableText".Translate();
					if (this.CanSend)
					{
						taggedString += "\n\n" + "NociosphereBecomingUnstableTextExtraSendable".Translate();
					}
					else
					{
						taggedString += "\n\n" + "NociosphereBecomingUnstableTextExtraNotSendable".Translate();
					}
					Find.LetterStack.ReceiveLetter("NociosphereBecomingUnstableLabel".Translate(), taggedString, LetterDefOf.NeutralEvent, this.parent, null, null, null, null, 0, true);
				}
				foreach (int num2 in CompNociosphere.WarningDays)
				{
					if (this.unstableTick - Find.TickManager.TicksGame == 60000 * num2)
					{
						Messages.Message(this.Props.unstableWarning.Formatted(num2), MessageTypeDefOf.NeutralEvent, true);
					}
				}
			}
			if (Find.TickManager.TicksGame == this.unstableTick && this.Activity.IsDormant)
			{
				TaggedString taggedString2 = "NociosphereUnstableText".Translate();
				if (this.CanSend)
				{
					taggedString2 += "\n\n" + "NociosphereUnstableTextExtraSendable".Translate();
				}
				else
				{
					taggedString2 += "\n\n" + "NociosphereUnstableTextExtraNotSendable".Translate();
				}
				Find.LetterStack.ReceiveLetter("NociosphereUnstableLabel".Translate(), taggedString2, LetterDefOf.ThreatSmall, this.parent, null, null, null, null, 0, true);
			}
			if (base.OnCooldown)
			{
				if (this.CanCooldown)
				{
					this.cooldownTicks--;
				}
				if (this.cooldownTicks <= 0)
				{
					this.CooldownEnded(true);
				}
			}
		}

		// Token: 0x0600A0F0 RID: 41200 RVA: 0x00398B48 File Offset: 0x00396D48
		public override void Notify_UsedVerb(Pawn pawn, Verb verb)
		{
			Verb_CastAbility verb_CastAbility = verb as Verb_CastAbility;
			if (verb_CastAbility != null && verb_CastAbility.ability.def == AbilityDefOf.EntitySkip)
			{
				return;
			}
			this.segScaleGoal = 1.2f;
		}

		// Token: 0x0600A0F1 RID: 41201 RVA: 0x00398B80 File Offset: 0x00396D80
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.Pawn.GetComp<CompActivity>().IsActive)
			{
				int num = Mathf.Max(this.endOnslaughtTick - GenTicks.TicksGame, 0);
				stringBuilder.Append(this.Props.onslaughtInspectText.Formatted(num / 60));
			}
			else if (this.IsBecomingUnstable)
			{
				int numTicks = Mathf.Max(this.unstableTick - GenTicks.TicksGame, 0);
				stringBuilder.AppendLineIfNotEmpty().Append((this.Props.becomingUnstableInspectText.Formatted(Array.Empty<NamedArgument>()) + ": " + numTicks.ToStringTicksToPeriod(true, false, true, true, false)).Colorize(ColoredText.WarningColor));
			}
			else if (this.IsUnstable)
			{
				stringBuilder.AppendLineIfNotEmpty().Append(this.Props.unstableInspectText.Colorize(ColoredText.WarningColor));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600A0F2 RID: 41202 RVA: 0x00398C74 File Offset: 0x00396E74
		public override void Notify_Downed()
		{
			this.Pawn.Kill(null, null);
		}

		// Token: 0x0600A0F3 RID: 41203 RVA: 0x00398C96 File Offset: 0x00396E96
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompNociosphere.<CompGetGizmosExtra>d__50 <CompGetGizmosExtra>d__ = new CompNociosphere.<CompGetGizmosExtra>d__50(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0600A0F4 RID: 41204 RVA: 0x00398CA8 File Offset: 0x00396EA8
		public void OnActivityActivated()
		{
			if (this.Pawn.MapHeld == null)
			{
				return;
			}
			if (this.Pawn.IsOnHoldingPlatform)
			{
				Building_HoldingPlatform building_HoldingPlatform = (Building_HoldingPlatform)this.Pawn.ParentHolder;
				Thing thing;
				building_HoldingPlatform.innerContainer.TryDrop(this.Pawn, building_HoldingPlatform.Position, building_HoldingPlatform.Map, ThingPlaceMode.Direct, 1, out thing, null, null);
				CompHoldingPlatformTarget compHoldingPlatformTarget = this.Pawn.TryGetComp<CompHoldingPlatformTarget>();
				if (compHoldingPlatformTarget != null)
				{
					compHoldingPlatformTarget.targetHolder = null;
				}
			}
			this.nociosphereVictims = 0;
			this.Pushable.canBePushed = false;
			this.AoEComp.range = (float)Mathf.Max(this.Pawn.MapHeld.Size.x, this.Pawn.MapHeld.Size.z);
			this.startedOnslaught = GenTicks.TicksGame;
			this.endOnslaughtTick = GenTicks.TicksGame + Mathf.RoundToInt(this.Props.sentOnslaughtDurationSeconds.RandomInRange * 60f);
			if (!this.sentOnslaught)
			{
				this.toHarm = Mathf.Max(Mathf.RoundToInt((float)this.Pawn.MapHeld.mapPawns.ColonistCount * CompNociosphere.ColonistHarmRange.RandomInRange), 1);
				this.sentFromLocation = this.Pawn.PositionHeld;
			}
			this.Pawn.MapHeld.attackTargetsCache.UpdateTarget(this.Pawn);
			Find.TickManager.slower.SignalForceNormalSpeed();
		}

		// Token: 0x0600A0F5 RID: 41205 RVA: 0x00398E14 File Offset: 0x00397014
		public void OnPassive()
		{
			if (this.IsBecomingUnstable || this.IsUnstable)
			{
				Find.LetterStack.ReceiveLetter(this.Props.leftLetterLabel, this.Props.leftLetterText, LetterDefOf.NeutralEvent, null, 0, true);
				NociosphereUtility.SkipTo(this.Pawn, this.Pawn.Position);
				if (!this.Pawn.DestroyedOrNull())
				{
					this.Pawn.DeSpawn(DestroyMode.Vanish);
					Lord lord = this.Pawn.GetLord();
					if (lord != null)
					{
						lord.Notify_PawnLost(this.Pawn, PawnLostCondition.ExitedMap, null);
					}
					Find.WorldPawns.PassToWorld(this.Pawn, PawnDiscardDecideMode.Discard);
					return;
				}
			}
			else
			{
				IntVec3 cell;
				if (CellFinder.TryFindRandomCellNear(this.sentFromLocation, this.Pawn.Map, 3, (IntVec3 c) => c.Walkable(this.Pawn.Map) && !c.Fogged(this.Pawn.Map) && c.GetFirstPawn(this.Pawn.Map) == null && c.GetRoom(this.Pawn.Map) == this.sentFromLocation.GetRoom(this.Pawn.Map), out cell, -1))
				{
					NociosphereUtility.SkipTo(this.Pawn, cell);
				}
				this.Deactivate();
				this.sentOnslaught = false;
				this.AoEComp.range = this.AoEComp.Props.range;
				Messages.Message(this.Props.onslaughtEndedMessage, this.Pawn, MessageTypeDefOf.NeutralEvent, true);
				this.Pawn.MapHeld.attackTargetsCache.UpdateTarget(this.Pawn);
				this.Activity.SetActivity(0f, false);
				this.StartCooldown();
			}
		}

		// Token: 0x0600A0F6 RID: 41206 RVA: 0x00398F80 File Offset: 0x00397180
		public bool ShouldGoPassive()
		{
			if (!this.Pawn.Spawned)
			{
				return false;
			}
			if (GenTicks.TicksGame < this.startedOnslaught + this.Props.minOnslaughtTicks)
			{
				return false;
			}
			if (GenTicks.TicksGame >= this.endOnslaughtTick)
			{
				return true;
			}
			if (!this.sentOnslaught && !this.Pawn.Map.mapPawns.AnyColonistSpawned)
			{
				return true;
			}
			List<Pawn> list = this.Pawn.Map.mapPawns.FreeHumanlikesOfFaction(Faction.OfPlayer);
			if (!this.sentOnslaught)
			{
				int num = 0;
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].DeadOrDowned)
					{
						num++;
					}
					if (num >= this.toHarm)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600A0F7 RID: 41207 RVA: 0x0039903B File Offset: 0x0039723B
		public bool CanBeSuppressed()
		{
			return !this.IsUnstable;
		}

		// Token: 0x0600A0F8 RID: 41208 RVA: 0x00399046 File Offset: 0x00397246
		public bool CanActivate()
		{
			return true;
		}

		// Token: 0x0600A0F9 RID: 41209 RVA: 0x00399049 File Offset: 0x00397249
		public string ActivityTooltipExtra()
		{
			if (this.IsUnstable)
			{
				return this.Props.unstableInspectText.Colorize(ColoredText.WarningColor);
			}
			return "";
		}

		// Token: 0x0600A0FA RID: 41210 RVA: 0x00399070 File Offset: 0x00397270
		protected override void OnInteracted(Pawn caster)
		{
			this.sentOnslaught = true;
			this.sentFromLocation = this.Pawn.PositionHeld;
			NociosphereUtility.SkipTo(this.Pawn, this.sendLocation);
			this.Activity.EnterActiveState();
			Messages.Message("CommandSent".Translate(this.Pawn.Named("PAWN")), this.Pawn, MessageTypeDefOf.NeutralEvent, true);
		}

		// Token: 0x0600A0FB RID: 41211 RVA: 0x003990E8 File Offset: 0x003972E8
		public override AcceptanceReport CanInteract(Pawn activateBy = null, bool checkOptionalItems = true)
		{
			AcceptanceReport result = base.CanInteract(activateBy, checkOptionalItems);
			if (!result.Accepted)
			{
				return result;
			}
			if (!this.CanSend)
			{
				return "CannotActivateEntity".Translate();
			}
			if (!this.Pawn.IsOnHoldingPlatform)
			{
				return "CannotActivateEntityPlatform".Translate();
			}
			return true;
		}

		// Token: 0x0600A0FC RID: 41212 RVA: 0x00399144 File Offset: 0x00397344
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			if (base.ValidateTarget(target, false))
			{
				this.TargetLocation(target.Pawn);
			}
		}

		// Token: 0x0600A0FD RID: 41213 RVA: 0x00399160 File Offset: 0x00397360
		private void TargetLocation(Pawn caster)
		{
			TargetingParameters targetingParameters = TargetingParameters.ForCell();
			targetingParameters.mapBoundsContractedBy = 1;
			targetingParameters.validator = ((TargetInfo c) => c.Cell.InBounds(caster.Map) && !c.Cell.Fogged(caster.Map));
			Find.Targeter.BeginTargeting(targetingParameters, delegate(LocalTargetInfo target)
			{
				this.sendLocation = target.Cell;
				this.<>n__1(caster);
			}, delegate(LocalTargetInfo t)
			{
				Widgets.MouseAttachedLabel("ChooseNociosphereDest".Translate(), 0f, 0f, null);
			});
		}

		// Token: 0x0600A0FE RID: 41214 RVA: 0x003991D6 File Offset: 0x003973D6
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			CompNociosphere.<CompFloatMenuOptions>d__61 <CompFloatMenuOptions>d__ = new CompNociosphere.<CompFloatMenuOptions>d__61(-2);
			<CompFloatMenuOptions>d__.<>4__this = this;
			<CompFloatMenuOptions>d__.<>3__selPawn = selPawn;
			return <CompFloatMenuOptions>d__;
		}

		// Token: 0x0600A0FF RID: 41215 RVA: 0x003991F0 File Offset: 0x003973F0
		public RoofCollapseResponse Notify_OnBeforeRoofCollapse()
		{
			IntVec3 cell;
			if (RCellFinder.TryFindRandomCellNearWith(this.Pawn.Position, (IntVec3 c) => CompNociosphere.IsValidCell(c, this.Pawn.MapHeld), this.Pawn.MapHeld, out cell, 10, 2147483647))
			{
				NociosphereUtility.SkipTo(this.Pawn, cell);
				this.Activity.AdjustActivity(this.Props.activityOnRoofCollapsed);
			}
			return RoofCollapseResponse.RemoveThing;
		}

		// Token: 0x0600A100 RID: 41216 RVA: 0x00399252 File Offset: 0x00397452
		private static bool IsValidCell(IntVec3 cell, Map map)
		{
			return cell.InBounds(map) && cell.Walkable(map);
		}

		// Token: 0x0600A101 RID: 41217 RVA: 0x00399268 File Offset: 0x00397468
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.nociosphereVictims, "nociosphereVictims", 0, false);
			Scribe_Values.Look<int>(ref this.toHarm, "toHarm", 0, false);
			Scribe_Values.Look<bool>(ref this.sentOnslaught, "sentOnslaught", false, false);
			Scribe_Values.Look<int>(ref this.endOnslaughtTick, "endOnslaughtTick", 0, false);
			Scribe_Values.Look<int>(ref this.startedOnslaught, "startedOnslaught", 0, false);
			Scribe_Values.Look<IntVec3>(ref this.sendLocation, "sendLocation", default(IntVec3), false);
			Scribe_Values.Look<IntVec3>(ref this.sentFromLocation, "sentFromLocation", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.becomingUnstableTick, "becomingUnstableTick", 0, false);
			Scribe_Values.Look<int>(ref this.unstableTick, "unstableTick", 0, false);
			Scribe_Values.Look<float>(ref this.segScale, "segScale", 0f, false);
			Scribe_Values.Look<float>(ref this.segScaleVel, "segScaleVel", 0f, false);
			Scribe_Values.Look<float>(ref this.segScaleGoal, "segScaleGoal", 0f, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.Pawn.IsWorldPawn())
			{
				Find.WorldPawns.RemoveAndDiscardPawnViaGC(this.Pawn);
			}
		}

		// Token: 0x0600A102 RID: 41218 RVA: 0x00399394 File Offset: 0x00397594
		public override void PostDrawExtraSelectionOverlays()
		{
			if (!this.Activity.IsActive)
			{
				GenDraw.DrawRadiusRing(this.Pawn.PositionHeld, this.AoEComp.range);
			}
		}

		// Token: 0x04005F09 RID: 24329
		private int nociosphereVictims;

		// Token: 0x04005F0A RID: 24330
		private int toHarm;

		// Token: 0x04005F0B RID: 24331
		public bool sentOnslaught;

		// Token: 0x04005F0C RID: 24332
		private int endOnslaughtTick;

		// Token: 0x04005F0D RID: 24333
		private int startedOnslaught;

		// Token: 0x04005F0E RID: 24334
		private IntVec3 sendLocation;

		// Token: 0x04005F0F RID: 24335
		private IntVec3 sentFromLocation;

		// Token: 0x04005F10 RID: 24336
		private int becomingUnstableTick = -1;

		// Token: 0x04005F11 RID: 24337
		private int unstableTick = -1;

		// Token: 0x04005F12 RID: 24338
		public float segScale;

		// Token: 0x04005F13 RID: 24339
		private float segScaleVel;

		// Token: 0x04005F14 RID: 24340
		private float segScaleGoal;

		// Token: 0x04005F15 RID: 24341
		private CompCauseHediff_AoE aoeComp;

		// Token: 0x04005F16 RID: 24342
		private CompPushable pushComp;

		// Token: 0x04005F17 RID: 24343
		private CompActivity activityComp;

		// Token: 0x04005F18 RID: 24344
		private CompStudyUnlocks studyUnlocks;

		// Token: 0x04005F19 RID: 24345
		private List<Thing> thingsIgnoredByExplosion;

		// Token: 0x04005F1A RID: 24346
		private static readonly FloatRange ColonistHarmRange = new FloatRange(0.4f, 0.6f);

		// Token: 0x04005F1B RID: 24347
		private static readonly FloatRange BecomingUnstableRangeDays = new FloatRange(20f, 30f);

		// Token: 0x04005F1C RID: 24348
		private const float UnstableDays = 10f;

		// Token: 0x04005F1D RID: 24349
		private static readonly int[] WarningDays = new int[]
		{
			2,
			5
		};

		// Token: 0x04005F1E RID: 24350
		private const float SmoothTime = 0.1f;

		// Token: 0x04005F1F RID: 24351
		private const float ActivityPostOnslaught = 0f;

		// Token: 0x04005F20 RID: 24352
		private const int SendableStudyIndex = 2;
	}
}
