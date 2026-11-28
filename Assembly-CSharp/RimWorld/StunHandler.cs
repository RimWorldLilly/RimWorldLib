using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B2E RID: 6958
	public class StunHandler : IExposable
	{
		// Token: 0x17002030 RID: 8240
		// (get) Token: 0x0600B26D RID: 45677 RVA: 0x003ECEE9 File Offset: 0x003EB0E9
		public bool Stunned
		{
			get
			{
				return this.stunTicksLeft > 0 || this.Hypnotized;
			}
		}

		// Token: 0x17002031 RID: 8241
		// (get) Token: 0x0600B26E RID: 45678 RVA: 0x003ECEFC File Offset: 0x003EB0FC
		public bool StunFromEMP
		{
			get
			{
				return this.stunFromEMP;
			}
		}

		// Token: 0x17002032 RID: 8242
		// (get) Token: 0x0600B26F RID: 45679 RVA: 0x003ECF04 File Offset: 0x003EB104
		public int StunTicksLeft
		{
			get
			{
				return this.stunTicksLeft;
			}
		}

		// Token: 0x17002033 RID: 8243
		// (get) Token: 0x0600B270 RID: 45680 RVA: 0x003ECF0C File Offset: 0x003EB10C
		public bool DisableRotation
		{
			get
			{
				return this.disableRotation || this.Hypnotized;
			}
		}

		// Token: 0x17002034 RID: 8244
		// (get) Token: 0x0600B271 RID: 45681 RVA: 0x003ECF1E File Offset: 0x003EB11E
		public bool Hypnotized
		{
			get
			{
				return ModsConfig.AnomalyActive && this.parentPawn != null && Find.Anomaly.IsPawnHypnotized(this.parentPawn);
			}
		}

		// Token: 0x0600B272 RID: 45682 RVA: 0x003ECF41 File Offset: 0x003EB141
		public StunHandler(Thing parent)
		{
			this.parent = parent;
			this.parentPawn = (parent as Pawn);
			this.stunnableComp = parent.TryGetComp<CompStunnable>();
			this.adaptationTicksLeft = new Dictionary<DamageDef, int>();
		}

		// Token: 0x0600B273 RID: 45683 RVA: 0x003ECF7C File Offset: 0x003EB17C
		public void StunHandlerTick()
		{
			if (this.adaptationTicksLeft.Count > 0)
			{
				StunHandler.tmpAdaptationTicksLeft.Clear();
				StunHandler.tmpAdaptationTicksLeft.AddRange(this.adaptationTicksLeft);
				foreach (KeyValuePair<DamageDef, int> keyValuePair in StunHandler.tmpAdaptationTicksLeft)
				{
					DamageDef damageDef;
					int num;
					keyValuePair.Deconstruct(out damageDef, out num);
					DamageDef damageDef2 = damageDef;
					if (num > 0)
					{
						Dictionary<DamageDef, int> dictionary = this.adaptationTicksLeft;
						damageDef = damageDef2;
						num = dictionary[damageDef];
						dictionary[damageDef] = num - 1;
					}
				}
			}
			if (this.stunTicksLeft > 0)
			{
				this.stunTicksLeft--;
				if (this.showStunMote && (this.moteStun == null || this.moteStun.Destroyed) && !this.Hypnotized)
				{
					this.moteStun = MoteMaker.MakeStunOverlay(this.parent);
				}
				Pawn pawn = this.parent as Pawn;
				if (pawn != null && pawn.Downed)
				{
					this.stunTicksLeft = 0;
				}
				Mote mote = this.moteStun;
				if (mote != null)
				{
					mote.Maintain();
				}
				if (this.stunFromEMP)
				{
					if (this.empEffecter == null)
					{
						if (this.stunnableComp != null && this.stunnableComp.UseLargeEMPEffecter)
						{
							this.empEffecter = this.CreateLargeEffecter();
						}
						else
						{
							this.empEffecter = EffecterDefOf.DisabledByEMP.Spawn();
						}
					}
					this.empEffecter.EffectTick(this.parent, this.parent);
					return;
				}
			}
			else
			{
				if (this.empEffecter != null)
				{
					this.empEffecter.Cleanup();
					this.empEffecter = null;
					this.stunFromEMP = false;
				}
				this.disableRotation = false;
			}
		}

		// Token: 0x0600B274 RID: 45684 RVA: 0x003ED130 File Offset: 0x003EB330
		public void Notify_DamageApplied(DamageInfo dinfo)
		{
			if (!this.CanBeStunnedByDamage(dinfo.Def))
			{
				return;
			}
			int? constantStunDurationTicks = dinfo.Def.constantStunDurationTicks;
			float num = (constantStunDurationTicks != null) ? ((float)constantStunDurationTicks.GetValueOrDefault()) : (dinfo.Amount * 30f);
			if (this.CanAdaptToDamage(dinfo.Def))
			{
				int num2;
				if ((this.adaptationTicksLeft.TryGetValue(dinfo.Def, out num2) ? num2 : 0) <= 0)
				{
					this.adaptationTicksLeft.SetOrAdd(dinfo.Def, dinfo.Def.stunAdaptationTicks);
				}
				else
				{
					if (dinfo.Def.displayAdaptedTextMote)
					{
						Vector3 loc = new Vector3((float)this.parent.Position.x + 1f, (float)this.parent.Position.y, (float)this.parent.Position.z + 1f);
						string text = dinfo.Def.adaptedText ?? "Adapted".Translate();
						MoteMaker.ThrowText(loc, this.parent.Map, text, Color.white, -1f);
					}
					num = 0f;
				}
			}
			if (num > 0f)
			{
				if (dinfo.Def.stunResistStat != null && !dinfo.Def.stunResistStat.Worker.IsDisabledFor(this.parent))
				{
					num *= Mathf.Clamp01(1f - this.parent.GetStatValue(dinfo.Def.stunResistStat, true, -1));
				}
				if (dinfo.Def == DamageDefOf.EMP)
				{
					this.stunFromEMP = true;
				}
				this.StunFor(Mathf.RoundToInt(num), dinfo.Instigator, true, true, false);
			}
		}

		// Token: 0x0600B275 RID: 45685 RVA: 0x003ED2E8 File Offset: 0x003EB4E8
		public void StunFor(int ticks, Thing instigator, bool addBattleLog = true, bool showMote = true, bool disableRotation = false)
		{
			this.stunTicksLeft = Mathf.Max(this.stunTicksLeft, ticks);
			this.showStunMote = showMote;
			this.disableRotation = disableRotation;
			if (addBattleLog)
			{
				Find.BattleLog.Add(new BattleLogEntry_Event(this.parent, RulePackDefOf.Event_Stun, instigator));
			}
		}

		// Token: 0x0600B276 RID: 45686 RVA: 0x003ED335 File Offset: 0x003EB535
		public void StopStun()
		{
			this.stunTicksLeft = 0;
		}

		// Token: 0x0600B277 RID: 45687 RVA: 0x003ED340 File Offset: 0x003EB540
		private Effecter CreateLargeEffecter()
		{
			Effecter effecter = EffecterDefOf.DisabledByEMPLarge.Spawn();
			for (int i = 0; i < effecter.children.Count; i++)
			{
				SubEffecter_SprayerChance subEffecter_SprayerChance = effecter.children[i] as SubEffecter_SprayerChance;
				if (subEffecter_SprayerChance != null)
				{
					if (this.stunnableComp.EMPEffecterDimensions != null || this.stunnableComp.EMPEffecterOffset != null)
					{
						subEffecter_SprayerChance.spawnLocOverride = new MoteSpawnLocType?(MoteSpawnLocType.OnSource);
					}
					subEffecter_SprayerChance.dimensionsOverride = this.stunnableComp.EMPEffecterDimensions;
					subEffecter_SprayerChance.offsetOverride = this.stunnableComp.EMPEffecterOffset;
					subEffecter_SprayerChance.chanceOverride = this.stunnableComp.EMPChancePerTick;
				}
			}
			return effecter;
		}

		// Token: 0x0600B278 RID: 45688 RVA: 0x003ED3F4 File Offset: 0x003EB5F4
		private bool CanBeStunnedByDamage(DamageDef def)
		{
			if (!def.causeStun)
			{
				return false;
			}
			if (this.stunnableComp != null && !this.stunnableComp.CanBeStunnedByDamage(def))
			{
				return false;
			}
			Pawn pawn = this.parent as Pawn;
			if (pawn != null)
			{
				return !pawn.Downed && !pawn.Dead && (!ModsConfig.AnomalyActive || !pawn.health.hediffSet.HasHediff(HediffDefOf.AwokenCorpse, false)) && (def == DamageDefOf.Stun || (def == DamageDefOf.EMP && !pawn.RaceProps.IsFlesh) || (ModsConfig.BiotechActive && def == DamageDefOf.MechBandShockwave && pawn.RaceProps.IsMechanoid) || (def == DamageDefOf.NerveStun && !pawn.RaceProps.IsMechanoid));
			}
			return def != DamageDefOf.NerveStun;
		}

		// Token: 0x0600B279 RID: 45689 RVA: 0x003ED4CC File Offset: 0x003EB6CC
		private bool CanAdaptToDamage(DamageDef def)
		{
			if (def.stunAdaptationTicks <= 0)
			{
				return false;
			}
			if (this.stunnableComp != null && this.stunnableComp.CanAdaptToDamage(def))
			{
				return true;
			}
			Pawn pawn = this.parent as Pawn;
			if (pawn != null)
			{
				if (def == DamageDefOf.EMP)
				{
					return (ModsConfig.AnomalyActive && (pawn.kindDef == PawnKindDefOf.Revenant || pawn.kindDef == PawnKindDefOf.Nociosphere)) || pawn.RaceProps.IsMechanoid || pawn.RaceProps.IsDrone;
				}
				if (ModsConfig.AnomalyActive && def == DamageDefOf.NerveStun)
				{
					return !pawn.RaceProps.IsMechanoid && !pawn.RaceProps.IsDrone;
				}
			}
			return false;
		}

		// Token: 0x0600B27A RID: 45690 RVA: 0x003ED580 File Offset: 0x003EB780
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.stunTicksLeft, "stunTicksLeft", 0, false);
			Scribe_Values.Look<bool>(ref this.showStunMote, "showStunMote", false, false);
			Scribe_Values.Look<bool>(ref this.stunFromEMP, "stunFromEMP", false, false);
			Scribe_Values.Look<bool>(ref this.disableRotation, "disableRotation", false, false);
			Scribe_Collections.Look<DamageDef, int>(ref this.adaptationTicksLeft, "adaptationTicksLeft", LookMode.Def, LookMode.Value);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (this.adaptationTicksLeft == null)
				{
					this.adaptationTicksLeft = new Dictionary<DamageDef, int>();
				}
				int num = 0;
				Scribe_Values.Look<int>(ref num, "EMPAdaptedTicksLeft", 0, false);
				if (num != 0)
				{
					this.adaptationTicksLeft.SetOrAdd(DamageDefOf.EMP, num);
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.adaptationTicksLeft.RemoveAll((KeyValuePair<DamageDef, int> x) => x.Value <= 0);
			}
		}

		// Token: 0x040066F6 RID: 26358
		public Thing parent;

		// Token: 0x040066F7 RID: 26359
		private Pawn parentPawn;

		// Token: 0x040066F8 RID: 26360
		private int stunTicksLeft;

		// Token: 0x040066F9 RID: 26361
		private Mote moteStun;

		// Token: 0x040066FA RID: 26362
		private bool showStunMote = true;

		// Token: 0x040066FB RID: 26363
		private Effecter empEffecter;

		// Token: 0x040066FC RID: 26364
		private bool stunFromEMP;

		// Token: 0x040066FD RID: 26365
		private Dictionary<DamageDef, int> adaptationTicksLeft;

		// Token: 0x040066FE RID: 26366
		private bool disableRotation;

		// Token: 0x040066FF RID: 26367
		private CompStunnable stunnableComp;

		// Token: 0x04006700 RID: 26368
		public const float StunDurationTicksPerDamage = 30f;

		// Token: 0x04006701 RID: 26369
		private static readonly List<KeyValuePair<DamageDef, int>> tmpAdaptationTicksLeft = new List<KeyValuePair<DamageDef, int>>();
	}
}
