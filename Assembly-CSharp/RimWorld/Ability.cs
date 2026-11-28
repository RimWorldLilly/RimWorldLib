using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020013C9 RID: 5065
	public class Ability : IVerbOwner, IExposable, ILoadReferenceable
	{
		// Token: 0x1700157D RID: 5501
		// (get) Token: 0x06007DB1 RID: 32177 RVA: 0x002CE8EC File Offset: 0x002CCAEC
		public Verb verb
		{
			get
			{
				return this.verbTracker.PrimaryVerb;
			}
		}

		// Token: 0x06007DB2 RID: 32178 RVA: 0x002CE8F9 File Offset: 0x002CCAF9
		public string UniqueVerbOwnerID()
		{
			return this.GetUniqueLoadID();
		}

		// Token: 0x06007DB3 RID: 32179 RVA: 0x002CE901 File Offset: 0x002CCB01
		public bool VerbsStillUsableBy(Pawn p)
		{
			return true;
		}

		// Token: 0x1700157E RID: 5502
		// (get) Token: 0x06007DB4 RID: 32180 RVA: 0x002CE904 File Offset: 0x002CCB04
		// (set) Token: 0x06007DB5 RID: 32181 RVA: 0x002CE90C File Offset: 0x002CCB0C
		public List<Tool> Tools { get; private set; }

		// Token: 0x1700157F RID: 5503
		// (get) Token: 0x06007DB6 RID: 32182 RVA: 0x002CE915 File Offset: 0x002CCB15
		public Thing ConstantCaster
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x17001580 RID: 5504
		// (get) Token: 0x06007DB7 RID: 32183 RVA: 0x002CE91D File Offset: 0x002CCB1D
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return new List<VerbProperties>
				{
					this.def.verbProperties
				};
			}
		}

		// Token: 0x17001581 RID: 5505
		// (get) Token: 0x06007DB8 RID: 32184 RVA: 0x002CE935 File Offset: 0x002CCB35
		public ImplementOwnerTypeDef ImplementOwnerTypeDef
		{
			get
			{
				return ImplementOwnerTypeDefOf.NativeVerb;
			}
		}

		// Token: 0x17001582 RID: 5506
		// (get) Token: 0x06007DB9 RID: 32185 RVA: 0x002CE93C File Offset: 0x002CCB3C
		public int CooldownTicksRemaining
		{
			get
			{
				if (this.inCooldown)
				{
					return Mathf.Max(this.cooldownEndTick - GenTicks.TicksGame, 0);
				}
				return 0;
			}
		}

		// Token: 0x17001583 RID: 5507
		// (get) Token: 0x06007DBA RID: 32186 RVA: 0x002CE95A File Offset: 0x002CCB5A
		public int CooldownTicksTotal
		{
			get
			{
				return this.cooldownDuration;
			}
		}

		// Token: 0x17001584 RID: 5508
		// (get) Token: 0x06007DBB RID: 32187 RVA: 0x002CE962 File Offset: 0x002CCB62
		public bool UsesCharges
		{
			get
			{
				return this.maxCharges > 0;
			}
		}

		// Token: 0x17001585 RID: 5509
		// (get) Token: 0x06007DBC RID: 32188 RVA: 0x002CE96D File Offset: 0x002CCB6D
		// (set) Token: 0x06007DBD RID: 32189 RVA: 0x002CE975 File Offset: 0x002CCB75
		public int RemainingCharges
		{
			get
			{
				return this.charges;
			}
			set
			{
				this.charges = value;
			}
		}

		// Token: 0x17001586 RID: 5510
		// (get) Token: 0x06007DBE RID: 32190 RVA: 0x002CE980 File Offset: 0x002CCB80
		public string GizmoExtraLabel
		{
			get
			{
				if (!this.UsesCharges)
				{
					return null;
				}
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 2);
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.charges);
				defaultInterpolatedStringHandler.AppendLiteral(" / ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.maxCharges);
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}

		// Token: 0x17001587 RID: 5511
		// (get) Token: 0x06007DBF RID: 32191 RVA: 0x002CE9CD File Offset: 0x002CCBCD
		public VerbTracker VerbTracker
		{
			get
			{
				if (this.verbTracker == null)
				{
					this.verbTracker = new VerbTracker(this);
				}
				return this.verbTracker;
			}
		}

		// Token: 0x17001588 RID: 5512
		// (get) Token: 0x06007DC0 RID: 32192 RVA: 0x002CE9EC File Offset: 0x002CCBEC
		public bool HasCooldown
		{
			get
			{
				return this.def.cooldownTicksRange != default(IntRange) || (this.def.groupDef != null && this.def.groupDef.cooldownTicks > 0) || this.def.hasExternallyHandledCooldown;
			}
		}

		// Token: 0x17001589 RID: 5513
		// (get) Token: 0x06007DC1 RID: 32193 RVA: 0x002CEA41 File Offset: 0x002CCC41
		public bool OnCooldown
		{
			get
			{
				return this.HasCooldown && this.inCooldown;
			}
		}

		// Token: 0x1700158A RID: 5514
		// (get) Token: 0x06007DC2 RID: 32194 RVA: 0x002CEA54 File Offset: 0x002CCC54
		public virtual AcceptanceReport CanCast
		{
			get
			{
				if (!this.comps.NullOrEmpty<AbilityComp>())
				{
					for (int i = 0; i < this.comps.Count; i++)
					{
						if (!this.comps[i].CanCast)
						{
							return false;
						}
					}
				}
				if (this.inCooldown)
				{
					return this.def.cooldownPerCharge && this.charges > 0;
				}
				if (this.UsesCharges)
				{
					return this.charges > 0;
				}
				if (this.pawn.MapHeld != null)
				{
					if (this.pawn.MapHeld.IsPocketMap)
					{
						if (this.VerbProperties.Any((VerbProperties vp) => !vp.useableInPocketMaps))
						{
							return "CannotUseReason_PocketMap".Translate(this.pawn.MapHeld.generatorDef.label);
						}
					}
					if (this.pawn.MapHeld.Biome.inVacuum)
					{
						if (this.VerbProperties.Any((VerbProperties vp) => !vp.useableInVacuum))
						{
							return "CannotFunctionInVacuum".Translate();
						}
					}
					PlanetTile tile = this.pawn.MapHeld.Tile;
					if (this.LayerPreventsCast(this.pawn.MapHeld.Tile))
					{
						PlanetLayerDef layerDef = tile.LayerDef;
						return "CannotPerformPlanetLayer".Translate(layerDef.gerundLabel.Named("GERUND"), layerDef.label.Named("LAYER"));
					}
				}
				return true;
			}
		}

		// Token: 0x06007DC3 RID: 32195 RVA: 0x002CEC10 File Offset: 0x002CCE10
		private bool LayerPreventsCast(PlanetTile tile)
		{
			if (!tile.Valid)
			{
				return false;
			}
			foreach (VerbProperties verbProperties in this.VerbProperties)
			{
				if (!verbProperties.layerWhitelist.NullOrEmpty<PlanetLayerDef>() && !verbProperties.layerWhitelist.Contains(tile.LayerDef))
				{
					return true;
				}
				if (!verbProperties.layerBlacklist.NullOrEmpty<PlanetLayerDef>() && verbProperties.layerBlacklist.Contains(tile.LayerDef))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x1700158B RID: 5515
		// (get) Token: 0x06007DC4 RID: 32196 RVA: 0x002CECB4 File Offset: 0x002CCEB4
		public bool Casting
		{
			get
			{
				if (!this.verb.WarmingUp)
				{
					Pawn_JobTracker jobs = this.pawn.jobs;
					if (!(((jobs != null) ? jobs.curDriver : null) is JobDriver_CastAbilityWorld) || this.pawn.CurJob.ability != this)
					{
						Pawn pawn = this.pawn;
						return ((pawn != null) ? pawn.CurJobDef : null) != null && this.pawn.CurJobDef.abilityCasting && this.pawn.CurJob.ability == this;
					}
				}
				return true;
			}
		}

		// Token: 0x1700158C RID: 5516
		// (get) Token: 0x06007DC5 RID: 32197 RVA: 0x002CED3C File Offset: 0x002CCF3C
		public bool CanCooldown
		{
			get
			{
				if (!this.HasCooldown)
				{
					return false;
				}
				if (this.def.waitForJobEnd)
				{
					Pawn_JobTracker jobs = this.pawn.jobs;
					JobDef jobDef;
					if (jobs == null)
					{
						jobDef = null;
					}
					else
					{
						Job curJob = jobs.curJob;
						jobDef = ((curJob != null) ? curJob.def : null);
					}
					return jobDef != this.def.jobDef;
				}
				return true;
			}
		}

		// Token: 0x1700158D RID: 5517
		// (get) Token: 0x06007DC6 RID: 32198 RVA: 0x002CED98 File Offset: 0x002CCF98
		public virtual string Tooltip
		{
			get
			{
				string text = this.def.GetTooltip(this.pawn);
				if (this.UsesCharges)
				{
					if (this.def.cooldownPerCharge)
					{
						string str = text;
						string str2 = "\n\n";
						string str3 = "Uses".Translate().ToString();
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(5, 2);
						defaultInterpolatedStringHandler.AppendLiteral(": ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(this.charges);
						defaultInterpolatedStringHandler.AppendLiteral(" / ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(this.maxCharges);
						text = str + str2 + str3 + defaultInterpolatedStringHandler.ToStringAndClear();
					}
					else
					{
						string str4 = text;
						string str5 = "\n\n";
						string str6 = "Charges".Translate().ToString();
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(5, 2);
						defaultInterpolatedStringHandler2.AppendLiteral(": ");
						defaultInterpolatedStringHandler2.AppendFormatted<int>(this.charges);
						defaultInterpolatedStringHandler2.AppendLiteral(" / ");
						defaultInterpolatedStringHandler2.AppendFormatted<int>(this.maxCharges);
						text = str4 + str5 + str6 + defaultInterpolatedStringHandler2.ToStringAndClear();
					}
				}
				if (this.EffectComps != null)
				{
					foreach (CompAbilityEffect compAbilityEffect in this.EffectComps)
					{
						string text2 = compAbilityEffect.ExtraTooltipPart();
						if (!text2.NullOrEmpty())
						{
							text = text + "\n\n" + text2;
						}
					}
				}
				return text;
			}
		}

		// Token: 0x1700158E RID: 5518
		// (get) Token: 0x06007DC7 RID: 32199 RVA: 0x002CEF04 File Offset: 0x002CD104
		public virtual bool CanQueueCast
		{
			get
			{
				if (!this.CanCast)
				{
					return false;
				}
				if (this.pawn.jobs == null)
				{
					return false;
				}
				int num = 0;
				foreach (Job j in this.pawn.jobs.AllJobs())
				{
					if (this.<get_CanQueueCast>g__SameForQueueing|63_0(j))
					{
						num++;
						if (!this.UsesCharges || num >= this.charges)
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		// Token: 0x1700158F RID: 5519
		// (get) Token: 0x06007DC8 RID: 32200 RVA: 0x002CEF9C File Offset: 0x002CD19C
		public List<CompAbilityEffect> EffectComps
		{
			get
			{
				if (this.effectComps == null)
				{
					IEnumerable<CompAbilityEffect> enumerable = this.CompsOfType<CompAbilityEffect>();
					this.effectComps = ((enumerable == null) ? new List<CompAbilityEffect>() : enumerable.ToList<CompAbilityEffect>());
				}
				return this.effectComps;
			}
		}

		// Token: 0x06007DC9 RID: 32201 RVA: 0x002CEFD4 File Offset: 0x002CD1D4
		public Ability()
		{
		}

		// Token: 0x06007DCA RID: 32202 RVA: 0x002CF038 File Offset: 0x002CD238
		public Ability(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06007DCB RID: 32203 RVA: 0x002CF0A4 File Offset: 0x002CD2A4
		public Ability(Pawn pawn, Precept sourcePrecept)
		{
			this.pawn = pawn;
			this.sourcePrecept = sourcePrecept;
		}

		// Token: 0x06007DCC RID: 32204 RVA: 0x002CF118 File Offset: 0x002CD318
		public Ability(Pawn pawn, AbilityDef def)
		{
			this.pawn = pawn;
			this.def = def;
			this.Initialize();
		}

		// Token: 0x06007DCD RID: 32205 RVA: 0x002CF190 File Offset: 0x002CD390
		public Ability(Pawn pawn, Precept sourcePrecept, AbilityDef def)
		{
			this.pawn = pawn;
			this.def = def;
			this.sourcePrecept = sourcePrecept;
			this.Initialize();
		}

		// Token: 0x06007DCE RID: 32206 RVA: 0x002CF210 File Offset: 0x002CD410
		public virtual bool CanApplyOn(LocalTargetInfo target)
		{
			if (this.effectComps != null)
			{
				using (List<CompAbilityEffect>.Enumerator enumerator = this.effectComps.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.CanApplyOn(target, null))
						{
							return false;
						}
					}
				}
			}
			return this.def.verbProperties.targetable || !this.def.verbProperties.targetParams.canTargetSelf || target == this.pawn;
		}

		// Token: 0x06007DCF RID: 32207 RVA: 0x002CF2B4 File Offset: 0x002CD4B4
		public virtual bool CanApplyOn(GlobalTargetInfo target)
		{
			if (this.EffectComps != null)
			{
				using (List<CompAbilityEffect>.Enumerator enumerator = this.EffectComps.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.CanApplyOn(target))
						{
							return false;
						}
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x06007DD0 RID: 32208 RVA: 0x002CF318 File Offset: 0x002CD518
		public virtual bool AICanTargetNow(LocalTargetInfo target)
		{
			if (!this.def.aiCanUse || !this.CanCast)
			{
				return false;
			}
			if (!this.CanApplyOn(target))
			{
				return false;
			}
			if (this.EffectComps != null)
			{
				using (List<CompAbilityEffect>.Enumerator enumerator = this.EffectComps.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.AICanTargetNow(target))
						{
							return false;
						}
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x06007DD1 RID: 32209 RVA: 0x002CF3A4 File Offset: 0x002CD5A4
		public virtual LocalTargetInfo AIGetAOETarget()
		{
			if (this.def.ai_SearchAOEForTargets)
			{
				foreach (Thing thing in GenRadial.RadialDistinctThingsAround(this.pawn.Position, this.pawn.Map, this.verb.EffectiveRange, true))
				{
					if (this.ValidAOEAffectedTarget(thing))
					{
						bool flag = true;
						using (List<CompAbilityEffect>.Enumerator enumerator2 = this.EffectComps.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								if (!enumerator2.Current.AICanTargetNow(thing))
								{
									flag = false;
									break;
								}
							}
						}
						if (flag)
						{
							return thing;
						}
					}
				}
			}
			return LocalTargetInfo.Invalid;
		}

		// Token: 0x06007DD2 RID: 32210 RVA: 0x002CF488 File Offset: 0x002CD688
		public Window ConfirmationDialog(LocalTargetInfo target, Action action)
		{
			if (this.EffectComps != null)
			{
				foreach (CompAbilityEffect compAbilityEffect in this.effectComps)
				{
					Window window = compAbilityEffect.ConfirmationDialog(target, action);
					if (window != null)
					{
						return window;
					}
				}
			}
			if (!this.def.confirmationDialogText.NullOrEmpty())
			{
				return Dialog_MessageBox.CreateConfirmation(this.def.confirmationDialogText.Formatted(this.pawn.Named("PAWN")), action, false, null, WindowLayer.Dialog);
			}
			return null;
		}

		// Token: 0x06007DD3 RID: 32211 RVA: 0x002CF52C File Offset: 0x002CD72C
		public Window ConfirmationDialog(GlobalTargetInfo target, Action action)
		{
			if (this.EffectComps != null)
			{
				foreach (CompAbilityEffect compAbilityEffect in this.EffectComps)
				{
					Window window = compAbilityEffect.ConfirmationDialog(target, action);
					if (window != null)
					{
						return window;
					}
				}
			}
			if (!this.def.confirmationDialogText.NullOrEmpty())
			{
				return Dialog_MessageBox.CreateConfirmation(this.def.confirmationDialogText.Formatted(this.pawn.Named("PAWN")), action, false, null, WindowLayer.Dialog);
			}
			return null;
		}

		// Token: 0x06007DD4 RID: 32212 RVA: 0x002CF5D0 File Offset: 0x002CD7D0
		protected virtual void PreActivate(LocalTargetInfo? target)
		{
			if (this.UsesCharges)
			{
				this.charges--;
			}
			if (this.HasCooldown)
			{
				if (this.def.groupDef != null)
				{
					int num = this.def.overrideGroupCooldown ? this.def.cooldownTicksRange.RandomInRange : this.def.groupDef.cooldownTicks;
					foreach (Ability ability in this.pawn.abilities.AllAbilitiesForReading)
					{
						ability.Notify_GroupStartedCooldown(this.def.groupDef, num);
					}
					if (this.pawn.Ideo == null)
					{
						goto IL_191;
					}
					using (IEnumerator<Precept_Ritual> enumerator2 = this.pawn.Ideo.PreceptsListForReading.OfType<Precept_Ritual>().GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							Precept_Ritual precept_Ritual = enumerator2.Current;
							if (precept_Ritual.def.useCooldownFromAbilityGroupDef == this.def.groupDef)
							{
								precept_Ritual.Notify_CooldownFromAbilityStarted(num);
							}
						}
						goto IL_191;
					}
				}
				if (this.UsesCharges)
				{
					if (this.def.cooldownPerCharge)
					{
						if (this.charges < this.maxCharges && this.CooldownTicksRemaining == 0)
						{
							this.StartCooldown(this.def.cooldownTicksRange.RandomInRange);
						}
					}
					else if (this.charges <= 0)
					{
						this.StartCooldown(this.def.cooldownTicksRange.RandomInRange);
					}
				}
				else
				{
					this.StartCooldown(this.def.cooldownTicksRange.RandomInRange);
				}
			}
			IL_191:
			Pawn pawn = this.ConstantCaster as Pawn;
			if (pawn != null)
			{
				Pawn_EquipmentTracker equipment = pawn.equipment;
				if (equipment != null)
				{
					equipment.Notify_AbilityUsed(this);
				}
			}
			if (this.def.writeCombatLog)
			{
				Find.BattleLog.Add(new BattleLogEntry_AbilityUsed(this.pawn, (target != null) ? target.GetValueOrDefault().Thing : null, this.def, RulePackDefOf.Event_AbilityUsed));
			}
			this.customWarmupMotes.Clear();
		}

		// Token: 0x06007DD5 RID: 32213 RVA: 0x002CF800 File Offset: 0x002CDA00
		public virtual bool Activate(LocalTargetInfo target, LocalTargetInfo dest)
		{
			this.PreActivate(new LocalTargetInfo?(target));
			if (this.def.hostile && this.pawn.mindState != null)
			{
				this.pawn.mindState.lastCombatantTick = Find.TickManager.TicksGame;
			}
			if (this.EffectComps.Any<CompAbilityEffect>())
			{
				this.affectedTargetsCached.Clear();
				this.affectedTargetsCached.AddRange(this.GetAffectedTargets(target));
				this.ApplyEffects(this.EffectComps, this.affectedTargetsCached, dest);
			}
			this.preCastActions.Clear();
			return true;
		}

		// Token: 0x06007DD6 RID: 32214 RVA: 0x002CF898 File Offset: 0x002CDA98
		public virtual bool Activate(GlobalTargetInfo target)
		{
			this.PreActivate(null);
			if (this.def.hostile && this.pawn.mindState != null)
			{
				this.pawn.mindState.lastCombatantTick = Find.TickManager.TicksGame;
			}
			if (this.EffectComps.Any<CompAbilityEffect>())
			{
				this.ApplyEffects(this.EffectComps, target);
			}
			this.preCastActions.Clear();
			return true;
		}

		// Token: 0x06007DD7 RID: 32215 RVA: 0x002CF90E File Offset: 0x002CDB0E
		public IEnumerable<LocalTargetInfo> GetAffectedTargets(LocalTargetInfo target)
		{
			Ability.<GetAffectedTargets>d__81 <GetAffectedTargets>d__ = new Ability.<GetAffectedTargets>d__81(-2);
			<GetAffectedTargets>d__.<>4__this = this;
			<GetAffectedTargets>d__.<>3__target = target;
			return <GetAffectedTargets>d__;
		}

		// Token: 0x06007DD8 RID: 32216 RVA: 0x002CF928 File Offset: 0x002CDB28
		private bool ValidAOEAffectedTarget(Thing target)
		{
			if (!this.verb.targetParams.CanTarget(target, null))
			{
				return false;
			}
			if (target.Fogged())
			{
				return false;
			}
			for (int i = 0; i < this.EffectComps.Count; i++)
			{
				if (!this.EffectComps[i].Valid(target, false))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06007DD9 RID: 32217 RVA: 0x002CF990 File Offset: 0x002CDB90
		public virtual void QueueCastingJob(LocalTargetInfo target, LocalTargetInfo destination)
		{
			if (!this.CanQueueCast || !this.CanApplyOn(target))
			{
				return;
			}
			if (this.verb.verbProps.nonInterruptingSelfCast)
			{
				this.verb.TryStartCastOn(this.verb.Caster, false, true, false, false);
				return;
			}
			this.ShowCastingConfirmationIfNeeded(target, delegate()
			{
				this.needToRecacheWarmupMotes = true;
				this.pawn.jobs.TryTakeOrderedJob(this.GetJob(target, destination), new JobTag?(JobTag.Misc), false);
			});
		}

		// Token: 0x06007DDA RID: 32218 RVA: 0x002CFA1C File Offset: 0x002CDC1C
		public virtual Job GetJob(LocalTargetInfo target, LocalTargetInfo destination)
		{
			Job job = JobMaker.MakeJob(this.def.jobDef ?? JobDefOf.CastAbilityOnThing);
			job.verbToUse = this.verb;
			job.targetA = target;
			job.targetB = destination;
			job.ability = this;
			this.needToRecacheWarmupMotes = true;
			return job;
		}

		// Token: 0x06007DDB RID: 32219 RVA: 0x002CFA6C File Offset: 0x002CDC6C
		public virtual void QueueCastingJob(GlobalTargetInfo target)
		{
			if (!this.CanQueueCast || !this.CanApplyOn(target))
			{
				return;
			}
			this.ShowCastingConfirmationIfNeeded(target, delegate()
			{
				if (!this.pawn.IsCaravanMember())
				{
					Job job = JobMaker.MakeJob(this.def.jobDef ?? JobDefOf.CastAbilityOnWorldTile);
					job.verbToUse = this.verb;
					job.globalTarget = target;
					job.ability = this;
					this.needToRecacheWarmupMotes = true;
					this.pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
					return;
				}
				this.Activate(target);
			});
		}

		// Token: 0x06007DDC RID: 32220 RVA: 0x002CFABC File Offset: 0x002CDCBC
		private void ShowCastingConfirmationIfNeeded(LocalTargetInfo target, Action cast)
		{
			Window window = this.ConfirmationDialog(target, cast);
			if (window == null)
			{
				cast();
				return;
			}
			Find.WindowStack.Add(window);
		}

		// Token: 0x06007DDD RID: 32221 RVA: 0x002CFAE8 File Offset: 0x002CDCE8
		private void ShowCastingConfirmationIfNeeded(GlobalTargetInfo target, Action cast)
		{
			Window window = this.ConfirmationDialog(target, cast);
			if (window == null)
			{
				cast();
				return;
			}
			Find.WindowStack.Add(window);
		}

		// Token: 0x06007DDE RID: 32222 RVA: 0x002CFB14 File Offset: 0x002CDD14
		public bool ValidateGlobalTarget(GlobalTargetInfo target)
		{
			for (int i = 0; i < this.EffectComps.Count; i++)
			{
				if (!this.EffectComps[i].Valid(target, true))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06007DDF RID: 32223 RVA: 0x002CFB50 File Offset: 0x002CDD50
		public virtual bool GizmoDisabled(out string reason)
		{
			if (this.CanCooldown && this.OnCooldown && (!this.def.cooldownPerCharge || this.charges == 0))
			{
				reason = "AbilityOnCooldown".Translate(this.CooldownTicksRemaining.ToStringTicksToPeriod(true, false, true, true, false)).Resolve();
				return true;
			}
			if (this.UsesCharges && this.charges <= 0)
			{
				reason = "AbilityNoCharges".Translate();
				return true;
			}
			if (!this.comps.NullOrEmpty<AbilityComp>())
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					if (this.comps[i].GizmoDisabled(out reason))
					{
						return true;
					}
				}
			}
			AcceptanceReport canCast = this.CanCast;
			if (!canCast.Accepted)
			{
				reason = canCast.Reason;
				return true;
			}
			Lord lord = this.pawn.GetLord();
			if (lord != null)
			{
				AcceptanceReport report = lord.AbilityAllowed(this);
				if (!report)
				{
					reason = report.Reason;
					return true;
				}
			}
			if (!this.pawn.Drafted && this.def.disableGizmoWhileUndrafted && this.pawn.GetCaravan() == null && !DebugSettings.ShowDevGizmos)
			{
				reason = "AbilityDisabledUndrafted".Translate();
				return true;
			}
			if (this.pawn.DevelopmentalStage.Baby())
			{
				reason = "IsIncapped".Translate(this.pawn.LabelShort, this.pawn);
				return true;
			}
			if (this.pawn.Downed)
			{
				reason = "CommandDisabledUnconscious".TranslateWithBackup("CommandCallRoyalAidUnconscious").Formatted(this.pawn);
				return true;
			}
			if (this.pawn.Deathresting)
			{
				reason = "CommandDisabledDeathresting".Translate(this.pawn);
				return true;
			}
			if (this.def.casterMustBeCapableOfViolence && this.pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				reason = "IsIncapableOfViolence".Translate(this.pawn.LabelShort, this.pawn);
				return true;
			}
			if (!this.CanQueueCast)
			{
				reason = "AbilityAlreadyQueued".Translate();
				return true;
			}
			reason = null;
			return false;
		}

		// Token: 0x06007DE0 RID: 32224 RVA: 0x002CFDA0 File Offset: 0x002CDFA0
		public virtual void AbilityTick()
		{
			this.VerbTracker.VerbsTick();
			if (this.def.emittedFleck != null && this.Casting && this.pawn.IsHashIntervalTick(this.def.emissionInterval))
			{
				FleckMaker.ThrowMetaIcon(this.verb.CurrentTarget.Cell, this.pawn.Map, this.def.emittedFleck, 0.75f);
			}
			this.MoteTick();
			this.EffectersTick();
			this.WarmupTick();
			this.CastingTick();
			this.SoundTick();
			this.CooldownTick();
			if (!this.comps.NullOrEmpty<AbilityComp>())
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].CompTick();
				}
			}
			if (this.wasCastingOnPrevTick && !this.Casting)
			{
				this.lastCastTick = Find.TickManager.TicksGame;
			}
			this.wasCastingOnPrevTick = this.Casting;
		}

		// Token: 0x06007DE1 RID: 32225 RVA: 0x002CFE9C File Offset: 0x002CE09C
		private void MoteTick()
		{
			if ((this.def.warmupMote == null && this.def.WarmupMoteSocialSymbol == null) || !this.Casting)
			{
				return;
			}
			Vector3 vector = this.pawn.DrawPos + this.def.moteDrawOffset;
			vector += (this.verb.CurrentTarget.CenterVector3 - vector) * this.def.moteOffsetAmountTowardsTarget;
			if (this.warmupMote != null && !this.warmupMote.Destroyed)
			{
				if (!(this.warmupMote is MoteAttached))
				{
					this.warmupMote.exactPosition = vector;
				}
				this.warmupMote.Maintain();
				return;
			}
			if (!(this.def.WarmupMoteSocialSymbol == null))
			{
				this.warmupMote = MoteMaker.MakeInteractionBubble(this.pawn, this.verb.CurrentTarget.Pawn, ThingDefOf.Mote_Speech, this.def.WarmupMoteSocialSymbol, null);
				return;
			}
			if (this.def.warmupMote.thingClass != typeof(MoteAttached))
			{
				this.warmupMote = MoteMaker.MakeStaticMote(vector, this.pawn.Map, this.def.warmupMote, 1f, false, 0f);
				return;
			}
			this.warmupMote = MoteMaker.MakeAttachedOverlay((this.verb.CurrentTarget.Thing as Corpse) ?? this.verb.CurrentTarget.Thing, this.def.warmupMote, Vector3.zero, 1f, -1f);
		}

		// Token: 0x06007DE2 RID: 32226 RVA: 0x002D0054 File Offset: 0x002CE254
		private void EffectersTick()
		{
			for (int i = this.maintainedEffecters.Count - 1; i >= 0; i--)
			{
				Effecter item = this.maintainedEffecters[i].Item1;
				if (item.ticksLeft > 0)
				{
					TargetInfo item2 = this.maintainedEffecters[i].Item2;
					TargetInfo item3 = this.maintainedEffecters[i].Item3;
					item.EffectTick(item2, item3);
					item.ticksLeft--;
				}
				else
				{
					item.Cleanup();
					this.maintainedEffecters.RemoveAt(i);
				}
			}
		}

		// Token: 0x06007DE3 RID: 32227 RVA: 0x002D00E4 File Offset: 0x002CE2E4
		private void CooldownTick()
		{
			if (!this.inCooldown || !this.CanCooldown || this.CooldownTicksRemaining > 0)
			{
				return;
			}
			this.inCooldown = false;
			if (this.UsesCharges && this.def.cooldownPerCharge)
			{
				int a2 = this.charges + 1;
				this.charges = a2;
				this.charges = Mathf.Min(a2, this.maxCharges);
				if (this.charges < this.maxCharges)
				{
					this.StartCooldown(this.def.cooldownTicksRange.RandomInRange);
				}
			}
			if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				if (this.def.sendLetterOnCooldownComplete)
				{
					Find.LetterStack.ReceiveLetter("AbilityReadyLabel".Translate(this.def.LabelCap), "AbilityReadyText".Translate(this.pawn, this.def.label), LetterDefOf.NeutralEvent, new LookTargets(this.pawn), null, null, null, null, 0, true);
				}
				if (this.def.sendMessageOnCooldownComplete && !this.pawn.IsPrisoner)
				{
					Messages.Message("AbilityReadyLabel".Translate(this.def.LabelCap, this.pawn.Named("PAWN")), this.pawn, MessageTypeDefOf.NeutralEvent, true);
				}
			}
			if (this.def.groupDef != null && this.def.groupDef.sendMessageOnCooldownComplete && MessagesRepeatAvoider.MessageShowAllowed(this.def.groupDef.label + "_offcooldown", 1f))
			{
				string value = (from a in this.pawn.abilities.AllAbilitiesForReading
				where a.def.groupDef == this.def.groupDef
				select a.def.label).ToCommaList(false, false);
				Messages.Message("AbilitiesReadyLabel".Translate(value, this.pawn.Named("PAWN")), this.pawn, MessageTypeDefOf.NeutralEvent, true);
			}
		}

		// Token: 0x06007DE4 RID: 32228 RVA: 0x002D0320 File Offset: 0x002CE520
		private void CastingTick()
		{
			if (!this.Casting)
			{
				if (this.warmupEffecter != null)
				{
					this.warmupEffecter.Cleanup();
					this.warmupEffecter = null;
				}
				return;
			}
			this.SetupWarmupEffecter(this.verb.CurrentTarget);
			Effecter effecter = this.warmupEffecter;
			if (effecter != null)
			{
				effecter.EffectTick(this.verbTargetInfoTmp, this.verbTargetInfoTmp);
			}
			if (this.needToRecacheWarmupMotes && !this.EffectComps.NullOrEmpty<CompAbilityEffect>() && this.verb.CurrentTarget.Thing != null)
			{
				this.customWarmupMotes.Clear();
				foreach (CompAbilityEffect compAbilityEffect in this.effectComps)
				{
					foreach (Mote item in compAbilityEffect.CustomWarmupMotes(this.verb.CurrentTarget))
					{
						this.customWarmupMotes.Add(item);
					}
				}
				this.needToRecacheWarmupMotes = false;
			}
			foreach (Mote mote in this.customWarmupMotes)
			{
				mote.Maintain();
			}
		}

		// Token: 0x06007DE5 RID: 32229 RVA: 0x002D048C File Offset: 0x002CE68C
		private void WarmupTick()
		{
			if (!this.verb.WarmingUp)
			{
				return;
			}
			if (!(this.def.targetWorldCell ? this.CanApplyOn(this.pawn.CurJob.globalTarget) : this.CanApplyOn(this.verb.CurrentTarget)) && this.def.targetRequired)
			{
				if (this.def.targetWorldCell)
				{
					this.pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
				}
				Stance_Warmup warmupStance = this.verb.WarmupStance;
				if (warmupStance != null)
				{
					warmupStance.Interrupt();
				}
				this.verb.Reset();
				this.customWarmupMotes.Clear();
				this.preCastActions.Clear();
				return;
			}
			for (int i = this.preCastActions.Count - 1; i >= 0; i--)
			{
				if (this.preCastActions[i].ticksAwayFromCast >= this.verb.WarmupTicksLeft)
				{
					this.preCastActions[i].action(this.verb.CurrentTarget, this.verb.CurrentDestination);
					this.preCastActions.RemoveAt(i);
				}
			}
		}

		// Token: 0x06007DE6 RID: 32230 RVA: 0x002D05B4 File Offset: 0x002CE7B4
		private void SoundTick()
		{
			if (!this.pawn.Spawned || !this.Casting)
			{
				return;
			}
			if (this.def.warmupSound != null)
			{
				if (this.soundCast == null || this.soundCast.Ended)
				{
					this.soundCast = this.def.warmupSound.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(this.pawn.Position, this.pawn.Map, false), MaintenanceType.PerTick));
				}
				else
				{
					this.soundCast.Maintain();
				}
			}
			if (!this.wasCastingOnPrevTick && this.def.warmupStartSound != null)
			{
				this.def.warmupStartSound.PlayOneShot(new TargetInfo(this.pawn.Position, this.pawn.Map, false));
			}
			if (this.def.warmupPreEndSound != null && this.verb.WarmupTicksLeft == this.def.warmupPreEndSoundSeconds.SecondsToTicks())
			{
				this.def.warmupPreEndSound.PlayOneShot(new TargetInfo(this.pawn.Position, this.pawn.Map, false));
			}
		}

		// Token: 0x06007DE7 RID: 32231 RVA: 0x002D06E0 File Offset: 0x002CE8E0
		private void SetupWarmupEffecter(LocalTargetInfo target)
		{
			if (this.warmupEffecter != null || this.def.warmupEffecter == null)
			{
				return;
			}
			if (!this.def.useAverageTargetPositionForWarmupEffecter || this.EffectComps.NullOrEmpty<CompAbilityEffect>())
			{
				if (target.Thing != null)
				{
					this.warmupEffecter = this.def.warmupEffecter.SpawnAttached(target.Thing, this.pawn.MapHeld, 1f);
					this.verbTargetInfoTmp = target.Thing;
				}
				else
				{
					this.warmupEffecter = this.def.warmupEffecter.Spawn(target.Cell, this.pawn.MapHeld, 1f);
					this.verbTargetInfoTmp = new TargetInfo(target.Cell, this.pawn.MapHeld, false);
				}
			}
			else
			{
				Vector3 vector = Vector3.zero;
				IEnumerable<LocalTargetInfo> affectedTargets = this.GetAffectedTargets(target);
				foreach (LocalTargetInfo localTargetInfo in affectedTargets)
				{
					vector += localTargetInfo.Cell.ToVector3Shifted();
				}
				vector /= (float)affectedTargets.Count<LocalTargetInfo>();
				this.warmupEffecter = this.def.warmupEffecter.Spawn(vector.ToIntVec3(), this.pawn.MapHeld, 1f);
				this.verbTargetInfoTmp = new TargetInfo(vector.ToIntVec3(), this.pawn.MapHeld, false);
			}
			this.warmupEffecter.Trigger(this.verbTargetInfoTmp, this.verbTargetInfoTmp, -1);
		}

		// Token: 0x06007DE8 RID: 32232 RVA: 0x002D0888 File Offset: 0x002CEA88
		public void Notify_StartedCasting()
		{
			for (int i = 0; i < this.EffectComps.Count; i++)
			{
				this.preCastActions.AddRange(this.EffectComps[i].GetPreCastActions());
			}
		}

		// Token: 0x06007DE9 RID: 32233 RVA: 0x002D08C8 File Offset: 0x002CEAC8
		public void DrawEffectPreviews(LocalTargetInfo target)
		{
			for (int i = 0; i < this.EffectComps.Count; i++)
			{
				this.EffectComps[i].DrawEffectPreview(target);
			}
		}

		// Token: 0x06007DEA RID: 32234 RVA: 0x002D0900 File Offset: 0x002CEB00
		public bool GizmosVisible()
		{
			if (!this.EffectComps.NullOrEmpty<CompAbilityEffect>())
			{
				using (List<CompAbilityEffect>.Enumerator enumerator = this.EffectComps.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.ShouldHideGizmo)
						{
							return false;
						}
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x06007DEB RID: 32235 RVA: 0x002D0968 File Offset: 0x002CEB68
		public virtual IEnumerable<Command> GetGizmos()
		{
			Ability.<GetGizmos>d__102 <GetGizmos>d__ = new Ability.<GetGizmos>d__102(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x06007DEC RID: 32236 RVA: 0x002D0978 File Offset: 0x002CEB78
		public virtual IEnumerable<Gizmo> GetGizmosExtra()
		{
			Ability.<GetGizmosExtra>d__103 <GetGizmosExtra>d__ = new Ability.<GetGizmosExtra>d__103(-2);
			<GetGizmosExtra>d__.<>4__this = this;
			return <GetGizmosExtra>d__;
		}

		// Token: 0x06007DED RID: 32237 RVA: 0x002D0988 File Offset: 0x002CEB88
		public string GetInspectString()
		{
			if (this.comps.NullOrEmpty<AbilityComp>())
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this.comps.Count; i++)
			{
				string text = this.comps[i].CompInspectStringExtra();
				if (!text.NullOrEmpty())
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.Append(text);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06007DEE RID: 32238 RVA: 0x002D09F8 File Offset: 0x002CEBF8
		private void ApplyEffects(IEnumerable<CompAbilityEffect> effects, List<LocalTargetInfo> targets, LocalTargetInfo dest)
		{
			foreach (LocalTargetInfo target in targets)
			{
				this.ApplyEffects(effects, target, dest);
			}
			foreach (CompAbilityEffect compAbilityEffect in effects)
			{
				compAbilityEffect.PostApplied(targets, this.pawn.MapHeld);
			}
		}

		// Token: 0x06007DEF RID: 32239 RVA: 0x002D0A88 File Offset: 0x002CEC88
		public void StartCooldown(int ticks)
		{
			this.inCooldown = true;
			this.cooldownEndTick = GenTicks.TicksGame + ticks;
			this.cooldownDuration = ticks;
			if (!this.def.cooldownPerCharge)
			{
				this.charges = this.maxCharges;
			}
		}

		// Token: 0x06007DF0 RID: 32240 RVA: 0x002D0ABE File Offset: 0x002CECBE
		public void ResetCooldown()
		{
			this.inCooldown = false;
		}

		// Token: 0x06007DF1 RID: 32241 RVA: 0x002D0AC7 File Offset: 0x002CECC7
		public void Notify_GroupStartedCooldown(AbilityGroupDef group, int ticks)
		{
			if (group == this.def.groupDef)
			{
				this.StartCooldown(ticks);
			}
		}

		// Token: 0x06007DF2 RID: 32242 RVA: 0x002D0AE0 File Offset: 0x002CECE0
		protected virtual void ApplyEffects(IEnumerable<CompAbilityEffect> effects, LocalTargetInfo target, LocalTargetInfo dest)
		{
			foreach (CompAbilityEffect compAbilityEffect in effects)
			{
				compAbilityEffect.Apply(target, dest);
			}
		}

		// Token: 0x06007DF3 RID: 32243 RVA: 0x002D0B28 File Offset: 0x002CED28
		protected virtual void ApplyEffects(IEnumerable<CompAbilityEffect> effects, GlobalTargetInfo target)
		{
			foreach (CompAbilityEffect compAbilityEffect in effects)
			{
				compAbilityEffect.Apply(target);
			}
		}

		// Token: 0x06007DF4 RID: 32244 RVA: 0x002D0B70 File Offset: 0x002CED70
		public virtual void OnGizmoUpdate()
		{
			foreach (CompAbilityEffect compAbilityEffect in this.EffectComps)
			{
				compAbilityEffect.OnGizmoUpdate();
			}
		}

		// Token: 0x06007DF5 RID: 32245 RVA: 0x002D0BC0 File Offset: 0x002CEDC0
		public IEnumerable<T> CompsOfType<T>() where T : AbilityComp
		{
			List<AbilityComp> list = this.comps;
			if (list == null)
			{
				return null;
			}
			return (from c in list
			where c is T
			select c).Cast<T>();
		}

		// Token: 0x06007DF6 RID: 32246 RVA: 0x002D0BF7 File Offset: 0x002CEDF7
		public T CompOfType<T>() where T : AbilityComp
		{
			List<AbilityComp> list = this.comps;
			object obj;
			if (list == null)
			{
				obj = null;
			}
			else
			{
				obj = list.FirstOrDefault((AbilityComp c) => c is T);
			}
			return obj as T;
		}

		// Token: 0x06007DF7 RID: 32247 RVA: 0x002D0C34 File Offset: 0x002CEE34
		public void Initialize()
		{
			if (this.def.comps.Any<AbilityCompProperties>())
			{
				this.comps = new List<AbilityComp>();
				for (int i = 0; i < this.def.comps.Count; i++)
				{
					AbilityComp abilityComp = null;
					try
					{
						abilityComp = (AbilityComp)Activator.CreateInstance(this.def.comps[i].compClass);
						abilityComp.parent = this;
						this.comps.Add(abilityComp);
						abilityComp.Initialize(this.def.comps[i]);
					}
					catch (Exception ex)
					{
						string str = "Could not instantiate or initialize an AbilityComp: ";
						Exception ex2 = ex;
						Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
						this.comps.Remove(abilityComp);
					}
				}
			}
			if (this.Id == -1)
			{
				this.Id = Find.UniqueIDsManager.GetNextAbilityID();
			}
			IAbilityVerb abilityVerb = this.VerbTracker.PrimaryVerb as IAbilityVerb;
			if (abilityVerb != null)
			{
				abilityVerb.Ability = this;
			}
			if (this.def.charges > 0)
			{
				this.maxCharges = this.def.charges;
				this.charges = this.maxCharges;
			}
		}

		// Token: 0x06007DF8 RID: 32248 RVA: 0x002D0D6C File Offset: 0x002CEF6C
		public float FinalPsyfocusCost(LocalTargetInfo target)
		{
			if (this.def.AnyCompOverridesPsyfocusCost)
			{
				foreach (AbilityComp abilityComp in this.comps)
				{
					if (abilityComp.props.OverridesPsyfocusCost)
					{
						return abilityComp.PsyfocusCostForTarget(target);
					}
				}
			}
			return this.def.PsyfocusCost;
		}

		// Token: 0x06007DF9 RID: 32249 RVA: 0x002D0DEC File Offset: 0x002CEFEC
		public float HemogenCost()
		{
			if (this.comps != null)
			{
				foreach (AbilityComp abilityComp in this.comps)
				{
					CompAbilityEffect_HemogenCost compAbilityEffect_HemogenCost = abilityComp as CompAbilityEffect_HemogenCost;
					if (compAbilityEffect_HemogenCost != null)
					{
						return compAbilityEffect_HemogenCost.Props.hemogenCost;
					}
				}
			}
			return 0f;
		}

		// Token: 0x06007DFA RID: 32250 RVA: 0x002D0E60 File Offset: 0x002CF060
		public TaggedString WorldMapExtraLabel(GlobalTargetInfo t)
		{
			foreach (CompAbilityEffect compAbilityEffect in this.EffectComps)
			{
				string text = compAbilityEffect.WorldMapExtraLabel(t);
				if (text != null)
				{
					return text;
				}
			}
			return null;
		}

		// Token: 0x06007DFB RID: 32251 RVA: 0x002D0EC8 File Offset: 0x002CF0C8
		public void AddEffecterToMaintain(Effecter eff, IntVec3 pos, int ticks, Map map = null)
		{
			eff.ticksLeft = ticks;
			TargetInfo targetInfo = new TargetInfo(pos, map ?? this.pawn.Map, false);
			this.maintainedEffecters.Add(new Tuple<Effecter, TargetInfo, TargetInfo>(eff, targetInfo, targetInfo));
		}

		// Token: 0x06007DFC RID: 32252 RVA: 0x002D0F0C File Offset: 0x002CF10C
		public void AddEffecterToMaintain(Effecter eff, IntVec3 posA, IntVec3 posB, int ticks, Map map = null)
		{
			eff.ticksLeft = ticks;
			TargetInfo item = new TargetInfo(posA, map ?? this.pawn.Map, false);
			TargetInfo item2 = new TargetInfo(posB, map ?? this.pawn.Map, false);
			this.maintainedEffecters.Add(new Tuple<Effecter, TargetInfo, TargetInfo>(eff, item, item2));
		}

		// Token: 0x06007DFD RID: 32253 RVA: 0x002D0F68 File Offset: 0x002CF168
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<AbilityDef>(ref this.def, "def");
			if (this.def == null)
			{
				return;
			}
			Scribe_Values.Look<int>(ref this.Id, "Id", -1, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (this.Id == -1)
				{
					this.Id = Find.UniqueIDsManager.GetNextAbilityID();
				}
				this.Initialize();
			}
			Scribe_References.Look<Precept>(ref this.sourcePrecept, "sourcePrecept", false);
			Scribe_Deep.Look<VerbTracker>(ref this.verbTracker, "verbTracker", new object[]
			{
				this
			});
			Scribe_Values.Look<bool>(ref this.inCooldown, "inCooldown", false, false);
			Scribe_Values.Look<int>(ref this.cooldownEndTick, "cooldownEndTick", 0, false);
			Scribe_Values.Look<int>(ref this.cooldownDuration, "cooldownDuration", 0, false);
			Scribe_Values.Look<int>(ref this.maxCharges, "maxCharges", this.def.charges, false);
			Scribe_Values.Look<int>(ref this.charges, "charges", this.def.charges, false);
			Scribe_Values.Look<int>(ref this.lastCastTick, "lastCastTick", -99999, false);
			this.ExposeBackCompatability();
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostExposeData();
				}
			}
		}

		// Token: 0x06007DFE RID: 32254 RVA: 0x002D10AC File Offset: 0x002CF2AC
		private void ExposeBackCompatability()
		{
			int num = 0;
			int num2 = 0;
			Scribe_Values.Look<int>(ref num, "cooldownTicks", 0, false);
			Scribe_Values.Look<int>(ref num2, "cooldownTicksDuration", 0, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars && num != 0 && num2 != 0)
			{
				this.inCooldown = true;
				this.cooldownDuration = num2;
				this.cooldownEndTick = GenTicks.TicksGame + num;
			}
		}

		// Token: 0x06007DFF RID: 32255 RVA: 0x002D1102 File Offset: 0x002CF302
		public string GetUniqueLoadID()
		{
			return "Ability_" + this.Id.ToString();
		}

		// Token: 0x06007E00 RID: 32256 RVA: 0x002D111C File Offset: 0x002CF31C
		[CompilerGenerated]
		private bool <get_CanQueueCast>g__SameForQueueing|63_0(Job j)
		{
			return j.verbToUse == this.verb || (this.def.groupDef != null && j.ability != null && j.ability.def.groupDef == this.def.groupDef);
		}

		// Token: 0x04004F18 RID: 20248
		public int Id = -1;

		// Token: 0x04004F19 RID: 20249
		public Pawn pawn;

		// Token: 0x04004F1A RID: 20250
		public AbilityDef def;

		// Token: 0x04004F1B RID: 20251
		public List<AbilityComp> comps;

		// Token: 0x04004F1C RID: 20252
		protected Command gizmo;

		// Token: 0x04004F1D RID: 20253
		private VerbTracker verbTracker;

		// Token: 0x04004F1E RID: 20254
		private bool inCooldown;

		// Token: 0x04004F1F RID: 20255
		private int cooldownDuration;

		// Token: 0x04004F20 RID: 20256
		private int cooldownEndTick;

		// Token: 0x04004F21 RID: 20257
		private Mote warmupMote;

		// Token: 0x04004F22 RID: 20258
		private Effecter warmupEffecter;

		// Token: 0x04004F23 RID: 20259
		private Sustainer soundCast;

		// Token: 0x04004F24 RID: 20260
		private bool wasCastingOnPrevTick;

		// Token: 0x04004F25 RID: 20261
		private int charges;

		// Token: 0x04004F26 RID: 20262
		public int maxCharges;

		// Token: 0x04004F27 RID: 20263
		public int lastCastTick = -99999;

		// Token: 0x04004F28 RID: 20264
		public Precept sourcePrecept;

		// Token: 0x04004F29 RID: 20265
		private List<PreCastAction> preCastActions = new List<PreCastAction>();

		// Token: 0x04004F2A RID: 20266
		private List<Tuple<Effecter, TargetInfo, TargetInfo>> maintainedEffecters = new List<Tuple<Effecter, TargetInfo, TargetInfo>>();

		// Token: 0x04004F2B RID: 20267
		private List<Mote> customWarmupMotes = new List<Mote>();

		// Token: 0x04004F2C RID: 20268
		private bool needToRecacheWarmupMotes = true;

		// Token: 0x04004F2D RID: 20269
		private List<CompAbilityEffect> effectComps;

		// Token: 0x04004F2F RID: 20271
		private readonly List<LocalTargetInfo> affectedTargetsCached = new List<LocalTargetInfo>();

		// Token: 0x04004F30 RID: 20272
		private TargetInfo verbTargetInfoTmp = null;
	}
}
