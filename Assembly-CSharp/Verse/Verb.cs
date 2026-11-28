using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using Verse.AI;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000735 RID: 1845
	public abstract class Verb : ITargetingSource, IExposable, ILoadReferenceable
	{
		// Token: 0x17000B49 RID: 2889
		// (get) Token: 0x06003A3C RID: 14908 RVA: 0x00173DA4 File Offset: 0x00171FA4
		public IVerbOwner DirectOwner
		{
			get
			{
				return this.verbTracker.directOwner;
			}
		}

		// Token: 0x17000B4A RID: 2890
		// (get) Token: 0x06003A3D RID: 14909 RVA: 0x00173DB1 File Offset: 0x00171FB1
		public ImplementOwnerTypeDef ImplementOwnerType
		{
			get
			{
				return this.verbTracker.directOwner.ImplementOwnerTypeDef;
			}
		}

		// Token: 0x17000B4B RID: 2891
		// (get) Token: 0x06003A3E RID: 14910 RVA: 0x00173DC3 File Offset: 0x00171FC3
		public CompEquippable EquipmentCompSource
		{
			get
			{
				return this.DirectOwner as CompEquippable;
			}
		}

		// Token: 0x17000B4C RID: 2892
		// (get) Token: 0x06003A3F RID: 14911 RVA: 0x00173DD0 File Offset: 0x00171FD0
		public CompApparelReloadable ReloadableCompSource
		{
			get
			{
				return this.DirectOwner as CompApparelReloadable;
			}
		}

		// Token: 0x17000B4D RID: 2893
		// (get) Token: 0x06003A40 RID: 14912 RVA: 0x00173DDD File Offset: 0x00171FDD
		public CompApparelVerbOwner_Charged VerbOwner_ChargedCompSource
		{
			get
			{
				return this.DirectOwner as CompApparelVerbOwner_Charged;
			}
		}

		// Token: 0x17000B4E RID: 2894
		// (get) Token: 0x06003A41 RID: 14913 RVA: 0x00173DEA File Offset: 0x00171FEA
		public ThingWithComps EquipmentSource
		{
			get
			{
				if (this.EquipmentCompSource != null)
				{
					return this.EquipmentCompSource.parent;
				}
				if (this.ReloadableCompSource != null)
				{
					return this.ReloadableCompSource.parent;
				}
				if (this.VerbOwner_ChargedCompSource != null)
				{
					return this.VerbOwner_ChargedCompSource.parent;
				}
				return null;
			}
		}

		// Token: 0x17000B4F RID: 2895
		// (get) Token: 0x06003A42 RID: 14914 RVA: 0x00173E29 File Offset: 0x00172029
		public HediffComp_VerbGiver HediffCompSource
		{
			get
			{
				return this.DirectOwner as HediffComp_VerbGiver;
			}
		}

		// Token: 0x17000B50 RID: 2896
		// (get) Token: 0x06003A43 RID: 14915 RVA: 0x00173E36 File Offset: 0x00172036
		public Hediff HediffSource
		{
			get
			{
				if (this.HediffCompSource == null)
				{
					return null;
				}
				return this.HediffCompSource.parent;
			}
		}

		// Token: 0x17000B51 RID: 2897
		// (get) Token: 0x06003A44 RID: 14916 RVA: 0x00173E4D File Offset: 0x0017204D
		public Pawn_MeleeVerbs_TerrainSource TerrainSource
		{
			get
			{
				return this.DirectOwner as Pawn_MeleeVerbs_TerrainSource;
			}
		}

		// Token: 0x17000B52 RID: 2898
		// (get) Token: 0x06003A45 RID: 14917 RVA: 0x00173E5A File Offset: 0x0017205A
		public TerrainDef TerrainDefSource
		{
			get
			{
				if (this.TerrainSource == null)
				{
					return null;
				}
				return this.TerrainSource.def;
			}
		}

		// Token: 0x17000B53 RID: 2899
		// (get) Token: 0x06003A46 RID: 14918 RVA: 0x00173E71 File Offset: 0x00172071
		public virtual Thing Caster
		{
			get
			{
				return this.caster;
			}
		}

		// Token: 0x17000B54 RID: 2900
		// (get) Token: 0x06003A47 RID: 14919 RVA: 0x00173E79 File Offset: 0x00172079
		public virtual Pawn CasterPawn
		{
			get
			{
				return this.caster as Pawn;
			}
		}

		// Token: 0x17000B55 RID: 2901
		// (get) Token: 0x06003A48 RID: 14920 RVA: 0x00173E86 File Offset: 0x00172086
		public virtual Verb GetVerb
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000B56 RID: 2902
		// (get) Token: 0x06003A49 RID: 14921 RVA: 0x00173E89 File Offset: 0x00172089
		public virtual bool CasterIsPawn
		{
			get
			{
				return this.caster is Pawn;
			}
		}

		// Token: 0x17000B57 RID: 2903
		// (get) Token: 0x06003A4A RID: 14922 RVA: 0x00173E99 File Offset: 0x00172099
		public virtual bool Targetable
		{
			get
			{
				return this.verbProps.targetable;
			}
		}

		// Token: 0x17000B58 RID: 2904
		// (get) Token: 0x06003A4B RID: 14923 RVA: 0x00173EA6 File Offset: 0x001720A6
		public virtual bool MultiSelect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000B59 RID: 2905
		// (get) Token: 0x06003A4C RID: 14924 RVA: 0x00173EA9 File Offset: 0x001720A9
		public virtual bool HidePawnTooltips
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000B5A RID: 2906
		// (get) Token: 0x06003A4D RID: 14925 RVA: 0x00173EAC File Offset: 0x001720AC
		public LocalTargetInfo CurrentTarget
		{
			get
			{
				return this.currentTarget;
			}
		}

		// Token: 0x17000B5B RID: 2907
		// (get) Token: 0x06003A4E RID: 14926 RVA: 0x00173EB4 File Offset: 0x001720B4
		public LocalTargetInfo CurrentDestination
		{
			get
			{
				return this.currentDestination;
			}
		}

		// Token: 0x17000B5C RID: 2908
		// (get) Token: 0x06003A4F RID: 14927 RVA: 0x00173EBC File Offset: 0x001720BC
		public int LastShotTick
		{
			get
			{
				return this.lastShotTick;
			}
		}

		// Token: 0x17000B5D RID: 2909
		// (get) Token: 0x06003A50 RID: 14928 RVA: 0x00173EC4 File Offset: 0x001720C4
		public virtual TargetingParameters targetParams
		{
			get
			{
				return this.verbProps.targetParams;
			}
		}

		// Token: 0x17000B5E RID: 2910
		// (get) Token: 0x06003A51 RID: 14929 RVA: 0x00173ED1 File Offset: 0x001720D1
		public virtual ITargetingSource DestinationSelector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000B5F RID: 2911
		// (get) Token: 0x06003A52 RID: 14930 RVA: 0x00173ED4 File Offset: 0x001720D4
		protected virtual int ShotsPerBurst
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000B60 RID: 2912
		// (get) Token: 0x06003A53 RID: 14931 RVA: 0x00173ED8 File Offset: 0x001720D8
		public virtual Texture2D UIIcon
		{
			get
			{
				if (this.verbProps.commandIcon != null)
				{
					if (this.commandIconCached == null)
					{
						this.commandIconCached = ContentFinder<Texture2D>.Get(this.verbProps.commandIcon, true);
					}
					return this.commandIconCached;
				}
				if (this.EquipmentSource != null)
				{
					return this.EquipmentSource.def.uiIcon;
				}
				return BaseContent.BadTex;
			}
		}

		// Token: 0x17000B61 RID: 2913
		// (get) Token: 0x06003A54 RID: 14932 RVA: 0x00173F3C File Offset: 0x0017213C
		public bool Bursting
		{
			get
			{
				return this.burstShotsLeft > 0;
			}
		}

		// Token: 0x17000B62 RID: 2914
		// (get) Token: 0x06003A55 RID: 14933 RVA: 0x00173F47 File Offset: 0x00172147
		public virtual bool IsMeleeAttack
		{
			get
			{
				return this.verbProps.IsMeleeAttack;
			}
		}

		// Token: 0x17000B63 RID: 2915
		// (get) Token: 0x06003A56 RID: 14934 RVA: 0x00173F54 File Offset: 0x00172154
		public bool BuggedAfterLoading
		{
			get
			{
				return this.verbProps == null;
			}
		}

		// Token: 0x17000B64 RID: 2916
		// (get) Token: 0x06003A57 RID: 14935 RVA: 0x00173F5F File Offset: 0x0017215F
		public bool WarmingUp
		{
			get
			{
				return this.WarmupStance != null;
			}
		}

		// Token: 0x17000B65 RID: 2917
		// (get) Token: 0x06003A58 RID: 14936 RVA: 0x00173F6C File Offset: 0x0017216C
		public Stance_Warmup WarmupStance
		{
			get
			{
				if (this.CasterPawn == null || !this.CasterPawn.Spawned)
				{
					return null;
				}
				Stance_Warmup stance_Warmup = this.CasterPawn.stances.curStance as Stance_Warmup;
				if (stance_Warmup == null || stance_Warmup.verb != this)
				{
					return null;
				}
				return stance_Warmup;
			}
		}

		// Token: 0x17000B66 RID: 2918
		// (get) Token: 0x06003A59 RID: 14937 RVA: 0x00173FB5 File Offset: 0x001721B5
		public virtual float WarmupTime
		{
			get
			{
				return this.verbProps.warmupTime;
			}
		}

		// Token: 0x17000B67 RID: 2919
		// (get) Token: 0x06003A5A RID: 14938 RVA: 0x00173FC2 File Offset: 0x001721C2
		public int WarmupTicksLeft
		{
			get
			{
				if (this.WarmupStance == null)
				{
					return 0;
				}
				return this.WarmupStance.ticksLeft;
			}
		}

		// Token: 0x17000B68 RID: 2920
		// (get) Token: 0x06003A5B RID: 14939 RVA: 0x00173FD9 File Offset: 0x001721D9
		public float WarmupProgress
		{
			get
			{
				return 1f - this.WarmupTicksLeft.TicksToSeconds() / this.WarmupTime;
			}
		}

		// Token: 0x17000B69 RID: 2921
		// (get) Token: 0x06003A5C RID: 14940 RVA: 0x00173FF3 File Offset: 0x001721F3
		public virtual string ReportLabel
		{
			get
			{
				return this.verbProps.label;
			}
		}

		// Token: 0x17000B6A RID: 2922
		// (get) Token: 0x06003A5D RID: 14941 RVA: 0x00174000 File Offset: 0x00172200
		public virtual float EffectiveRange
		{
			get
			{
				return this.verbProps.AdjustedRange(this, this.Caster);
			}
		}

		// Token: 0x17000B6B RID: 2923
		// (get) Token: 0x06003A5E RID: 14942 RVA: 0x00174014 File Offset: 0x00172214
		public virtual float? AimAngleOverride
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000B6C RID: 2924
		// (get) Token: 0x06003A5F RID: 14943 RVA: 0x0017402A File Offset: 0x0017222A
		public bool NonInterruptingSelfCast
		{
			get
			{
				return this.verbProps.nonInterruptingSelfCast || this.nonInterruptingSelfCast;
			}
		}

		// Token: 0x17000B6D RID: 2925
		// (get) Token: 0x06003A60 RID: 14944 RVA: 0x00174044 File Offset: 0x00172244
		public int TicksBetweenBurstShots
		{
			get
			{
				if (this.cachedTicksBetweenBurstShots == null)
				{
					float num = (float)this.verbProps.ticksBetweenBurstShots;
					CompUniqueWeapon compUniqueWeapon;
					if (this.EquipmentSource != null && this.EquipmentSource.TryGetComp(out compUniqueWeapon))
					{
						foreach (WeaponTraitDef weaponTraitDef in compUniqueWeapon.TraitsListForReading)
						{
							num /= weaponTraitDef.burstShotSpeedMultiplier;
						}
					}
					this.cachedTicksBetweenBurstShots = new int?(Mathf.RoundToInt(num));
				}
				return this.cachedTicksBetweenBurstShots.Value;
			}
		}

		// Token: 0x17000B6E RID: 2926
		// (get) Token: 0x06003A61 RID: 14945 RVA: 0x001740E8 File Offset: 0x001722E8
		public int BurstShotCount
		{
			get
			{
				if (this.cachedBurstShotCount == null)
				{
					float num = (float)this.verbProps.burstShotCount;
					CompUniqueWeapon compUniqueWeapon;
					if (this.EquipmentSource != null && this.EquipmentSource.TryGetComp(out compUniqueWeapon))
					{
						foreach (WeaponTraitDef weaponTraitDef in compUniqueWeapon.TraitsListForReading)
						{
							num *= weaponTraitDef.burstShotCountMultiplier;
						}
					}
					this.cachedBurstShotCount = new int?(Mathf.CeilToInt(num));
				}
				return this.cachedBurstShotCount.Value;
			}
		}

		// Token: 0x06003A62 RID: 14946 RVA: 0x0017418C File Offset: 0x0017238C
		public bool IsStillUsableBy(Pawn pawn)
		{
			return this.Available() && this.DirectOwner.VerbsStillUsableBy(pawn) && this.verbProps.GetDamageFactorFor(this, pawn) != 0f && (!pawn.IsSubhuman || this.verbProps.category != VerbCategory.Ignite);
		}

		// Token: 0x06003A63 RID: 14947 RVA: 0x001741E2 File Offset: 0x001723E2
		public virtual bool IsUsableOn(Thing target)
		{
			return true;
		}

		// Token: 0x06003A64 RID: 14948 RVA: 0x001741E8 File Offset: 0x001723E8
		public virtual void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.loadID, "loadID", null, false);
			Scribe_Values.Look<VerbState>(ref this.state, "state", VerbState.Idle, false);
			Scribe_TargetInfo.Look(ref this.currentTarget, "currentTarget");
			Scribe_TargetInfo.Look(ref this.currentDestination, "currentDestination");
			Scribe_Values.Look<int>(ref this.burstShotsLeft, "burstShotsLeft", 0, false);
			Scribe_Values.Look<int>(ref this.ticksToNextBurstShot, "ticksToNextBurstShot", 0, false);
			Scribe_Values.Look<int>(ref this.lastShotTick, "lastShotTick", 0, false);
			Scribe_Values.Look<bool>(ref this.surpriseAttack, "surpriseAttack", false, false);
			Scribe_Values.Look<bool>(ref this.canHitNonTargetPawnsNow, "canHitNonTargetPawnsNow", false, false);
			Scribe_Values.Look<bool>(ref this.preventFriendlyFire, "preventFriendlyFire", false, false);
			Scribe_Values.Look<bool>(ref this.nonInterruptingSelfCast, "nonInterruptingSelfCast", false, false);
		}

		// Token: 0x06003A65 RID: 14949 RVA: 0x001742B7 File Offset: 0x001724B7
		public string GetUniqueLoadID()
		{
			return "Verb_" + this.loadID;
		}

		// Token: 0x06003A66 RID: 14950 RVA: 0x001742C9 File Offset: 0x001724C9
		public static string CalculateUniqueLoadID(IVerbOwner owner, Tool tool, ManeuverDef maneuver)
		{
			return string.Format("{0}_{1}_{2}", owner.UniqueVerbOwnerID(), (tool != null) ? tool.id : "NT", (maneuver != null) ? maneuver.defName : "NM");
		}

		// Token: 0x06003A67 RID: 14951 RVA: 0x001742FB File Offset: 0x001724FB
		public static string CalculateUniqueLoadID(IVerbOwner owner, int index)
		{
			return string.Format("{0}_{1}", owner.UniqueVerbOwnerID(), index);
		}

		// Token: 0x06003A68 RID: 14952 RVA: 0x00174313 File Offset: 0x00172513
		public bool TryStartCastOn(LocalTargetInfo castTarg, bool surpriseAttack = false, bool canHitNonTargetPawns = true, bool preventFriendlyFire = false, bool nonInterruptingSelfCast = false)
		{
			return this.TryStartCastOn(castTarg, LocalTargetInfo.Invalid, surpriseAttack, canHitNonTargetPawns, preventFriendlyFire, nonInterruptingSelfCast);
		}

		// Token: 0x06003A69 RID: 14953 RVA: 0x00174328 File Offset: 0x00172528
		public virtual bool TryStartCastOn(LocalTargetInfo castTarg, LocalTargetInfo destTarg, bool surpriseAttack = false, bool canHitNonTargetPawns = true, bool preventFriendlyFire = false, bool nonInterruptingSelfCast = false)
		{
			if (this.caster == null)
			{
				Log.Error("Verb " + this.GetUniqueLoadID() + " needs caster to work (possibly lost during saving/loading).");
				return false;
			}
			if (!this.caster.Spawned)
			{
				return false;
			}
			if (this.state == VerbState.Bursting || !this.CanHitTarget(castTarg))
			{
				return false;
			}
			if (this.CausesTimeSlowdown(castTarg))
			{
				Find.TickManager.slower.SignalForceNormalSpeed();
			}
			this.surpriseAttack = surpriseAttack;
			this.canHitNonTargetPawnsNow = canHitNonTargetPawns;
			this.preventFriendlyFire = preventFriendlyFire;
			this.nonInterruptingSelfCast = nonInterruptingSelfCast;
			this.currentTarget = castTarg;
			this.currentDestination = destTarg;
			if (this.CasterIsPawn && this.WarmupTime > 0f)
			{
				ShootLine newShootLine;
				if (!this.TryFindShootLineFromTo(this.caster.Position, castTarg, out newShootLine, false))
				{
					return false;
				}
				this.CasterPawn.Drawer.Notify_WarmingCastAlongLine(newShootLine, this.caster.Position);
				float statValue = this.CasterPawn.GetStatValue(StatDefOf.AimingDelayFactor, true, -1);
				int ticks = (this.WarmupTime * statValue).SecondsToTicks();
				this.CasterPawn.stances.SetStance(new Stance_Warmup(ticks, castTarg, this));
				if (this.verbProps.stunTargetOnCastStart && castTarg.Pawn != null)
				{
					castTarg.Pawn.stances.stunner.StunFor(ticks, null, false, true, false);
				}
			}
			else
			{
				Ability ability = this.verbTracker.directOwner as Ability;
				if (ability != null)
				{
					ability.lastCastTick = Find.TickManager.TicksGame;
				}
				this.WarmupComplete();
			}
			return true;
		}

		// Token: 0x06003A6A RID: 14954 RVA: 0x001744AB File Offset: 0x001726AB
		public virtual void WarmupComplete()
		{
			this.burstShotsLeft = this.ShotsPerBurst;
			this.state = VerbState.Bursting;
			this.TryCastNextBurstShot();
		}

		// Token: 0x06003A6B RID: 14955 RVA: 0x001744C8 File Offset: 0x001726C8
		public void VerbTick()
		{
			if (this.state == VerbState.Bursting)
			{
				if (this.caster.Spawned)
				{
					Pawn pawn = this.caster as Pawn;
					if (pawn == null || !pawn.stances.stunner.Stunned)
					{
						this.ticksToNextBurstShot--;
						if (this.ticksToNextBurstShot <= 0)
						{
							this.TryCastNextBurstShot();
						}
						this.BurstingTick();
						goto IL_62;
					}
				}
				this.Reset();
			}
			IL_62:
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

		// Token: 0x06003A6C RID: 14956 RVA: 0x001745BA File Offset: 0x001727BA
		public virtual void BurstingTick()
		{
		}

		// Token: 0x06003A6D RID: 14957 RVA: 0x001745BC File Offset: 0x001727BC
		public void AddEffecterToMaintain(Effecter eff, IntVec3 pos, int ticks, Map map = null)
		{
			eff.ticksLeft = ticks;
			TargetInfo targetInfo = new TargetInfo(pos, map ?? this.caster.Map, false);
			this.maintainedEffecters.Add(new Tuple<Effecter, TargetInfo, TargetInfo>(eff, targetInfo, targetInfo));
		}

		// Token: 0x06003A6E RID: 14958 RVA: 0x00174600 File Offset: 0x00172800
		public void AddEffecterToMaintain(Effecter eff, IntVec3 posA, IntVec3 posB, int ticks, Map map = null)
		{
			eff.ticksLeft = ticks;
			TargetInfo item = new TargetInfo(posA, map ?? this.caster.Map, false);
			TargetInfo item2 = new TargetInfo(posB, map ?? this.caster.Map, false);
			this.maintainedEffecters.Add(new Tuple<Effecter, TargetInfo, TargetInfo>(eff, item, item2));
		}

		// Token: 0x06003A6F RID: 14959 RVA: 0x0017465C File Offset: 0x0017285C
		public virtual bool Available()
		{
			if (this.verbProps.consumeFuelPerShot > 0f)
			{
				CompRefuelable compRefuelable = this.caster.TryGetComp<CompRefuelable>();
				if (compRefuelable != null && compRefuelable.Fuel < this.verbProps.consumeFuelPerShot)
				{
					return false;
				}
			}
			ThingWithComps equipmentSource = this.EquipmentSource;
			CompApparelVerbOwner compApparelVerbOwner = (equipmentSource != null) ? equipmentSource.GetComp<CompApparelVerbOwner>() : null;
			string text;
			return (compApparelVerbOwner == null || compApparelVerbOwner.CanBeUsed(out text)) && (!this.CasterIsPawn || this.EquipmentSource == null || !EquipmentUtility.RolePreventsFromUsing(this.CasterPawn, this.EquipmentSource, out text));
		}

		// Token: 0x06003A70 RID: 14960 RVA: 0x001746EC File Offset: 0x001728EC
		protected void TryCastNextBurstShot()
		{
			LocalTargetInfo localTargetInfo = this.currentTarget;
			if (this.Available() && this.TryCastShot())
			{
				if (this.verbProps.muzzleFlashScale > 0.01f)
				{
					FleckMaker.Static(this.caster.Position, this.caster.Map, FleckDefOf.ShotFlash, this.verbProps.muzzleFlashScale);
				}
				if (this.verbProps.soundCast != null)
				{
					this.verbProps.soundCast.PlayOneShot(new TargetInfo(this.caster.Position, this.caster.MapHeld, false));
				}
				if (this.verbProps.soundCastTail != null)
				{
					this.verbProps.soundCastTail.PlayOneShotOnCamera(this.caster.Map);
				}
				if (this.CasterIsPawn)
				{
					this.CasterPawn.Notify_UsedVerb(this.CasterPawn, this);
					if (this.CasterPawn.thinker != null && localTargetInfo == this.CasterPawn.mindState.enemyTarget)
					{
						this.CasterPawn.mindState.Notify_EngagedTarget();
					}
					if (this.CasterPawn.mindState != null)
					{
						this.CasterPawn.mindState.Notify_AttackedTarget(localTargetInfo);
					}
					if (this.CasterPawn.MentalState != null)
					{
						this.CasterPawn.MentalState.Notify_AttackedTarget(localTargetInfo);
					}
					if (this.TerrainDefSource != null)
					{
						this.CasterPawn.meleeVerbs.Notify_UsedTerrainBasedVerb();
					}
					if (this.CasterPawn.health != null)
					{
						this.CasterPawn.health.Notify_UsedVerb(this, localTargetInfo);
					}
					if (this.EquipmentSource != null)
					{
						this.EquipmentSource.Notify_UsedWeapon(this.CasterPawn);
					}
					if (!this.CasterPawn.Spawned)
					{
						this.Reset();
						return;
					}
				}
				if (this.verbProps.consumeFuelPerShot > 0f)
				{
					CompRefuelable compRefuelable = this.caster.TryGetComp<CompRefuelable>();
					if (compRefuelable != null)
					{
						compRefuelable.ConsumeFuel(this.verbProps.consumeFuelPerShot);
					}
				}
				this.burstShotsLeft--;
			}
			else
			{
				this.burstShotsLeft = 0;
			}
			if (this.burstShotsLeft > 0)
			{
				this.ticksToNextBurstShot = this.TicksBetweenBurstShots;
				if (this.CasterIsPawn && !this.NonInterruptingSelfCast)
				{
					this.CasterPawn.stances.SetStance(new Stance_Cooldown(this.TicksBetweenBurstShots + 1, this.currentTarget, this));
					return;
				}
			}
			else
			{
				this.state = VerbState.Idle;
				if (this.CasterIsPawn && !this.NonInterruptingSelfCast)
				{
					this.CasterPawn.stances.SetStance(new Stance_Cooldown(this.verbProps.AdjustedCooldownTicks(this, this.CasterPawn), this.currentTarget, this));
				}
				if (this.castCompleteCallback != null)
				{
					this.castCompleteCallback();
				}
				if (this.verbProps.consumeFuelPerBurst > 0f)
				{
					CompRefuelable compRefuelable2 = this.caster.TryGetComp<CompRefuelable>();
					if (compRefuelable2 != null)
					{
						compRefuelable2.ConsumeFuel(this.verbProps.consumeFuelPerBurst);
					}
				}
			}
		}

		// Token: 0x06003A71 RID: 14961 RVA: 0x001749D4 File Offset: 0x00172BD4
		public virtual void OrderForceTarget(LocalTargetInfo target)
		{
			if (this.verbProps.IsMeleeAttack)
			{
				Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, target);
				job.playerForced = true;
				Pawn pawn = target.Thing as Pawn;
				if (pawn != null)
				{
					job.killIncappedTarget = pawn.Downed;
				}
				this.CasterPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
				return;
			}
			float num = this.verbProps.EffectiveMinRange(target, this.CasterPawn);
			if ((float)this.CasterPawn.Position.DistanceToSquared(target.Cell) < num * num && this.CasterPawn.Position.AdjacentTo8WayOrInside(target.Cell))
			{
				Messages.Message("MessageCantShootInMelee".Translate(), this.CasterPawn, MessageTypeDefOf.RejectInput, false);
				return;
			}
			Job job2 = JobMaker.MakeJob(this.verbProps.ai_IsWeapon ? JobDefOf.AttackStatic : JobDefOf.UseVerbOnThing);
			job2.verbToUse = this;
			job2.targetA = target;
			job2.endIfCantShootInMelee = true;
			this.CasterPawn.jobs.TryTakeOrderedJob(job2, new JobTag?(JobTag.Misc), false);
		}

		// Token: 0x06003A72 RID: 14962
		protected abstract bool TryCastShot();

		// Token: 0x06003A73 RID: 14963 RVA: 0x00174AF3 File Offset: 0x00172CF3
		public void Notify_PickedUp()
		{
			this.Reset();
		}

		// Token: 0x06003A74 RID: 14964 RVA: 0x00174AFC File Offset: 0x00172CFC
		public virtual void Reset()
		{
			this.state = VerbState.Idle;
			this.currentTarget = null;
			this.currentDestination = null;
			this.burstShotsLeft = 0;
			this.ticksToNextBurstShot = 0;
			this.castCompleteCallback = null;
			this.surpriseAttack = false;
			this.preventFriendlyFire = false;
		}

		// Token: 0x06003A75 RID: 14965 RVA: 0x00174B4C File Offset: 0x00172D4C
		public virtual void Notify_EquipmentLost()
		{
			if (this.CasterIsPawn)
			{
				Pawn casterPawn = this.CasterPawn;
				if (casterPawn.Spawned)
				{
					Stance_Warmup stance_Warmup = casterPawn.stances.curStance as Stance_Warmup;
					if (stance_Warmup != null && stance_Warmup.verb == this)
					{
						casterPawn.stances.CancelBusyStanceSoft();
					}
					if (casterPawn.CurJob != null && casterPawn.CurJob.def == JobDefOf.AttackStatic)
					{
						casterPawn.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
					}
				}
			}
		}

		// Token: 0x06003A76 RID: 14966 RVA: 0x00174BC0 File Offset: 0x00172DC0
		public virtual float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = false;
			return 0f;
		}

		// Token: 0x06003A77 RID: 14967 RVA: 0x00174BCC File Offset: 0x00172DCC
		private bool CausesTimeSlowdown(LocalTargetInfo castTarg)
		{
			if (!this.verbProps.CausesTimeSlowdown)
			{
				return false;
			}
			if (!castTarg.HasThing)
			{
				return false;
			}
			Thing thing = castTarg.Thing;
			if (thing.def.category != ThingCategory.Pawn && (thing.def.building == null || !thing.def.building.IsTurret))
			{
				return false;
			}
			Pawn pawn = thing as Pawn;
			bool flag = pawn != null && pawn.Downed;
			return (this.CasterPawn == null || this.CasterPawn.Faction != Faction.OfPlayer || !this.CasterPawn.IsShambler) && (pawn == null || pawn.Faction != Faction.OfPlayer || !pawn.IsShambler) && ((thing.Faction == Faction.OfPlayer && this.caster.HostileTo(Faction.OfPlayer)) || (this.caster.Faction == Faction.OfPlayer && thing.HostileTo(Faction.OfPlayer) && !flag));
		}

		// Token: 0x06003A78 RID: 14968 RVA: 0x00174CC5 File Offset: 0x00172EC5
		public virtual bool CanHitTarget(LocalTargetInfo targ)
		{
			return this.caster != null && this.caster.Spawned && (targ == this.caster || this.CanHitTargetFrom(this.caster.Position, targ));
		}

		// Token: 0x06003A79 RID: 14969 RVA: 0x00174D08 File Offset: 0x00172F08
		public virtual bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			if (this.CasterIsPawn)
			{
				Pawn pawn = target.Thing as Pawn;
				if (pawn != null && (pawn.InSameExtraFaction(this.caster as Pawn, ExtraFactionType.HomeFaction, null) || pawn.InSameExtraFaction(this.caster as Pawn, ExtraFactionType.MiniFaction, null)))
				{
					return false;
				}
			}
			if (this.CasterIsPawn)
			{
				Pawn pawn2 = target.Thing as Pawn;
				if (pawn2 != null && HistoryEventUtility.IsKillingInnocentAnimal(this.CasterPawn, pawn2) && !new HistoryEvent(HistoryEventDefOf.KilledInnocentAnimal, this.CasterPawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo("MessagePawnUnwillingToDoDueToIdeo"))
				{
					return false;
				}
			}
			if (this.CasterIsPawn)
			{
				Pawn pawn3 = target.Thing as Pawn;
				if (pawn3 != null && this.CasterPawn.Ideo != null && this.CasterPawn.Ideo.IsVeneratedAnimal(pawn3) && !new HistoryEvent(HistoryEventDefOf.HuntedVeneratedAnimal, this.CasterPawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo("MessagePawnUnwillingToDoDueToIdeo"))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003A7A RID: 14970 RVA: 0x00174E08 File Offset: 0x00173008
		public virtual void DrawHighlight(LocalTargetInfo target)
		{
			this.verbProps.DrawRadiusRing(this.caster.Position, this);
			if (target.IsValid)
			{
				GenDraw.DrawTargetHighlight(target);
				this.DrawHighlightFieldRadiusAroundTarget(target);
			}
		}

		// Token: 0x06003A7B RID: 14971 RVA: 0x00174E38 File Offset: 0x00173038
		protected void DrawHighlightFieldRadiusAroundTarget(LocalTargetInfo target)
		{
			bool flag;
			float num = this.HighlightFieldRadiusAroundTarget(out flag);
			ShootLine shootLine;
			if (num > 0.2f && this.TryFindShootLineFromTo(this.caster.Position, target, out shootLine, false))
			{
				if (flag)
				{
					GenExplosion.RenderPredictedAreaOfEffect(shootLine.Dest, num, this.verbProps.explosionRadiusRingColor);
					return;
				}
				GenDraw.DrawFieldEdges((from x in GenRadial.RadialCellsAround(shootLine.Dest, num, true)
				where x.InBounds(Find.CurrentMap)
				select x).ToList<IntVec3>(), this.verbProps.explosionRadiusRingColor, null, null, 2900);
			}
		}

		// Token: 0x06003A7C RID: 14972 RVA: 0x00174EE4 File Offset: 0x001730E4
		public virtual void OnGUI(LocalTargetInfo target)
		{
			Texture2D icon;
			if (target.IsValid)
			{
				if (this.UIIcon != BaseContent.BadTex)
				{
					icon = this.UIIcon;
				}
				else
				{
					icon = TexCommand.Attack;
				}
			}
			else
			{
				icon = TexCommand.CannotShoot;
			}
			GenUI.DrawMouseAttachment(icon);
		}

		// Token: 0x06003A7D RID: 14973 RVA: 0x00174F2C File Offset: 0x0017312C
		public virtual bool CanHitTargetFrom(IntVec3 root, LocalTargetInfo targ)
		{
			if (targ.Thing != null && targ.Thing == this.caster)
			{
				return this.targetParams.canTargetSelf;
			}
			ShootLine shootLine;
			return (targ.Pawn == null || !targ.Pawn.IsPsychologicallyInvisible() || !this.caster.HostileTo(targ.Pawn)) && !this.ApparelPreventsShooting() && this.TryFindShootLineFromTo(root, targ, out shootLine, false);
		}

		// Token: 0x06003A7E RID: 14974 RVA: 0x00174F9F File Offset: 0x0017319F
		public bool ApparelPreventsShooting()
		{
			return this.FirstApparelPreventingShooting() != null;
		}

		// Token: 0x06003A7F RID: 14975 RVA: 0x00174FAC File Offset: 0x001731AC
		public Apparel FirstApparelPreventingShooting()
		{
			if (this.CasterIsPawn && this.CasterPawn.apparel != null)
			{
				List<Apparel> wornApparel = this.CasterPawn.apparel.WornApparel;
				for (int i = 0; i < wornApparel.Count; i++)
				{
					if (!wornApparel[i].AllowVerbCast(this))
					{
						return wornApparel[i];
					}
				}
			}
			return null;
		}

		// Token: 0x06003A80 RID: 14976 RVA: 0x00175008 File Offset: 0x00173208
		public bool TryFindShootLineFromTo(IntVec3 root, LocalTargetInfo targ, out ShootLine resultingLine, bool ignoreRange = false)
		{
			if (targ.HasThing && targ.Thing.Map != this.caster.Map)
			{
				resultingLine = default(ShootLine);
				return false;
			}
			if (this.verbProps.IsMeleeAttack || this.EffectiveRange <= 1.42f)
			{
				resultingLine = new ShootLine(root, targ.Cell);
				return ReachabilityImmediate.CanReachImmediate(root, targ, this.caster.Map, PathEndMode.Touch, null);
			}
			CellRect occupiedRect = targ.HasThing ? targ.Thing.OccupiedRect() : CellRect.SingleCell(targ.Cell);
			if (!ignoreRange && this.OutOfRange(root, targ, occupiedRect))
			{
				resultingLine = new ShootLine(root, targ.Cell);
				return false;
			}
			if (!this.verbProps.requireLineOfSight)
			{
				resultingLine = new ShootLine(root, targ.Cell);
				return true;
			}
			if (this.CasterIsPawn)
			{
				IntVec3 dest;
				if (this.CanHitFromCellIgnoringRange(root, targ, out dest))
				{
					resultingLine = new ShootLine(root, dest);
					return true;
				}
				ShootLeanUtility.LeanShootingSourcesFromTo(root, occupiedRect.ClosestCellTo(root), this.caster.Map, Verb.tempLeanShootSources);
				for (int i = 0; i < Verb.tempLeanShootSources.Count; i++)
				{
					IntVec3 intVec = Verb.tempLeanShootSources[i];
					if (this.CanHitFromCellIgnoringRange(intVec, targ, out dest))
					{
						resultingLine = new ShootLine(intVec, dest);
						return true;
					}
				}
			}
			else
			{
				foreach (IntVec3 intVec2 in this.caster.OccupiedRect())
				{
					IntVec3 dest;
					if (this.CanHitFromCellIgnoringRange(intVec2, targ, out dest))
					{
						resultingLine = new ShootLine(intVec2, dest);
						return true;
					}
				}
			}
			resultingLine = new ShootLine(root, targ.Cell);
			return false;
		}

		// Token: 0x06003A81 RID: 14977 RVA: 0x001751EC File Offset: 0x001733EC
		public bool OutOfRange(IntVec3 root, LocalTargetInfo targ, CellRect occupiedRect)
		{
			float num = this.verbProps.EffectiveMinRange(targ, this.caster);
			float num2 = occupiedRect.ClosestDistSquaredTo(root);
			return num2 > this.EffectiveRange * this.EffectiveRange || num2 < num * num;
		}

		// Token: 0x06003A82 RID: 14978 RVA: 0x00175230 File Offset: 0x00173430
		private bool CanHitFromCellIgnoringRange(IntVec3 sourceCell, LocalTargetInfo targ, out IntVec3 goodDest)
		{
			if (targ.Thing != null)
			{
				if (targ.Thing.Map != this.caster.Map)
				{
					goodDest = IntVec3.Invalid;
					return false;
				}
				ShootLeanUtility.CalcShootableCellsOf(Verb.tempDestList, targ.Thing, sourceCell);
				for (int i = 0; i < Verb.tempDestList.Count; i++)
				{
					if (this.CanHitCellFromCellIgnoringRange(sourceCell, Verb.tempDestList[i], targ.Thing.def.Fillage == FillCategory.Full))
					{
						goodDest = Verb.tempDestList[i];
						return true;
					}
				}
			}
			else if (this.CanHitCellFromCellIgnoringRange(sourceCell, targ.Cell, false))
			{
				goodDest = targ.Cell;
				return true;
			}
			goodDest = IntVec3.Invalid;
			return false;
		}

		// Token: 0x06003A83 RID: 14979 RVA: 0x00175300 File Offset: 0x00173500
		private bool CanHitCellFromCellIgnoringRange(IntVec3 sourceSq, IntVec3 targetLoc, bool includeCorners = false)
		{
			if (this.verbProps.mustCastOnOpenGround && (!targetLoc.Standable(this.caster.Map) || this.caster.Map.thingGrid.CellContains(targetLoc, ThingCategory.Pawn)))
			{
				return false;
			}
			if (this.verbProps.requireLineOfSight)
			{
				if (!includeCorners)
				{
					if (!GenSight.LineOfSight(sourceSq, targetLoc, this.caster.Map, true, null, 0, 0))
					{
						return false;
					}
				}
				else if (!GenSight.LineOfSightToEdges(sourceSq, targetLoc, this.caster.Map, true, null))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003A84 RID: 14980 RVA: 0x0017538C File Offset: 0x0017358C
		public override string ToString()
		{
			string text;
			if (this.verbProps == null)
			{
				text = "null";
			}
			else if (!this.verbProps.label.NullOrEmpty())
			{
				text = this.verbProps.label;
			}
			else if (this.HediffCompSource != null)
			{
				text = this.HediffCompSource.Def.label;
			}
			else if (this.EquipmentSource != null)
			{
				text = this.EquipmentSource.def.label;
			}
			else if (this.verbProps.AdjustedLinkedBodyPartsGroup(this.tool) != null)
			{
				text = this.verbProps.AdjustedLinkedBodyPartsGroup(this.tool).defName;
			}
			else
			{
				text = "unknown";
			}
			if (this.tool != null)
			{
				text = text + "/" + this.loadID;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
			defaultInterpolatedStringHandler.AppendFormatted<Type>(base.GetType());
			defaultInterpolatedStringHandler.AppendLiteral("(");
			defaultInterpolatedStringHandler.AppendFormatted(text);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0400267F RID: 9855
		public VerbProperties verbProps;

		// Token: 0x04002680 RID: 9856
		public VerbTracker verbTracker;

		// Token: 0x04002681 RID: 9857
		public ManeuverDef maneuver;

		// Token: 0x04002682 RID: 9858
		public Tool tool;

		// Token: 0x04002683 RID: 9859
		public Thing caster;

		// Token: 0x04002684 RID: 9860
		public MechanitorControlGroup controlGroup;

		// Token: 0x04002685 RID: 9861
		public string loadID;

		// Token: 0x04002686 RID: 9862
		public VerbState state;

		// Token: 0x04002687 RID: 9863
		protected LocalTargetInfo currentTarget;

		// Token: 0x04002688 RID: 9864
		protected LocalTargetInfo currentDestination;

		// Token: 0x04002689 RID: 9865
		protected int burstShotsLeft;

		// Token: 0x0400268A RID: 9866
		protected int ticksToNextBurstShot;

		// Token: 0x0400268B RID: 9867
		protected int lastShotTick = -999999;

		// Token: 0x0400268C RID: 9868
		protected bool surpriseAttack;

		// Token: 0x0400268D RID: 9869
		protected bool canHitNonTargetPawnsNow = true;

		// Token: 0x0400268E RID: 9870
		public bool preventFriendlyFire;

		// Token: 0x0400268F RID: 9871
		protected bool nonInterruptingSelfCast;

		// Token: 0x04002690 RID: 9872
		public Action castCompleteCallback;

		// Token: 0x04002691 RID: 9873
		private Texture2D commandIconCached;

		// Token: 0x04002692 RID: 9874
		private readonly List<Tuple<Effecter, TargetInfo, TargetInfo>> maintainedEffecters = new List<Tuple<Effecter, TargetInfo, TargetInfo>>();

		// Token: 0x04002693 RID: 9875
		private int? cachedTicksBetweenBurstShots;

		// Token: 0x04002694 RID: 9876
		private int? cachedBurstShotCount;

		// Token: 0x04002695 RID: 9877
		private static readonly List<IntVec3> tempLeanShootSources = new List<IntVec3>();

		// Token: 0x04002696 RID: 9878
		private static readonly List<IntVec3> tempDestList = new List<IntVec3>();
	}
}
