using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004BA RID: 1210
	public class Hediff_MetalhorrorImplant : HediffWithComps
	{
		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x06002192 RID: 8594 RVA: 0x000C75C2 File Offset: 0x000C57C2
		// (set) Token: 0x06002193 RID: 8595 RVA: 0x000C75CA File Offset: 0x000C57CA
		public ImplantSource ImplantSource
		{
			get
			{
				return this.implantSource;
			}
			set
			{
				this.implantSource = value;
			}
		}

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x06002194 RID: 8596 RVA: 0x000C75D3 File Offset: 0x000C57D3
		public int Biosignature
		{
			get
			{
				return this.implantSource.Biosignature;
			}
		}

		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x06002195 RID: 8597 RVA: 0x000C75E0 File Offset: 0x000C57E0
		public string BiosignatureName
		{
			get
			{
				return this.implantSource.BiosignatureName;
			}
		}

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x06002196 RID: 8598 RVA: 0x000C75ED File Offset: 0x000C57ED
		public bool Emerging
		{
			get
			{
				return this.emerging;
			}
		}

		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x06002197 RID: 8599 RVA: 0x000C75F5 File Offset: 0x000C57F5
		public bool KnowsDetected
		{
			get
			{
				return this.knowsDetected;
			}
		}

		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x06002198 RID: 8600 RVA: 0x000C75FD File Offset: 0x000C57FD
		// (set) Token: 0x06002199 RID: 8601 RVA: 0x000C7605 File Offset: 0x000C5805
		public bool LiedAboutInspection
		{
			get
			{
				return this.liedAboutInspection;
			}
			set
			{
				this.liedAboutInspection = (value || this.liedAboutInspection);
			}
		}

		// Token: 0x0600219A RID: 8602 RVA: 0x000C761C File Offset: 0x000C581C
		public override void PostAdd(DamageInfo? dinfo)
		{
			if (!ModLister.CheckAnomaly("Metalhorror implant"))
			{
				this.pawn.health.RemoveHediff(this);
				return;
			}
			base.PostAdd(dinfo);
			this.filthTimer = Mathf.CeilToInt(Hediff_MetalhorrorImplant.InitialFilthDelayDays.RandomInRange * 60000f);
		}

		// Token: 0x0600219B RID: 8603 RVA: 0x000C766C File Offset: 0x000C586C
		public override void PostTickInterval(int delta)
		{
			base.PostTickInterval(delta);
			if (!this.pawn.SpawnedOrAnyParentSpawned)
			{
				return;
			}
			this.DoPassiveTransmission();
			this.FilthTick(delta);
			if (this.Emerging && GenTicks.TicksGame >= this.emergeAt)
			{
				MetalhorrorUtility.SpawnMetalhorror(this.pawn, this);
			}
			if (this.knowsDetected)
			{
				this.Emerge("", false);
				return;
			}
			if (MetalhorrorUtility.ShouldRandomEmerge(this.pawn, delta))
			{
				this.Emerge("MetalhorrorReasonRandom".Translate(this.pawn.Named("INFECTED")), false);
			}
		}

		// Token: 0x0600219C RID: 8604 RVA: 0x000C7706 File Offset: 0x000C5906
		public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
		{
			this.Emerge("MetalhorrorReasonHostDied".Translate(this.pawn.Named("INFECTED")), false);
		}

		// Token: 0x0600219D RID: 8605 RVA: 0x000C7730 File Offset: 0x000C5930
		public void Emerge(string reason, bool isSympathetic = false)
		{
			if (this.emerging || !this.pawn.SpawnedOrAnyParentSpawned)
			{
				return;
			}
			Find.Anomaly.emergedBiosignatures.Add(this.Biosignature);
			base.SetVisible();
			this.emerging = true;
			this.knowsDetected = true;
			this.sympatheticEmergence = (isSympathetic || this.sympatheticEmergence);
			if (string.IsNullOrEmpty(this.emergeReason))
			{
				this.emergeReason = reason;
			}
			if (this.sympatheticEmergence)
			{
				this.emergeAt = GenTicks.TicksGame + Hediff_MetalhorrorImplant.TicksToEmergeSympathetic.RandomInRange;
			}
			else
			{
				this.emergeAt = GenTicks.TicksGame + Hediff_MetalhorrorImplant.TicksToEmerge.RandomInRange;
			}
			if (!this.pawn.DeadOrDowned)
			{
				HealthUtility.DamageUntilDowned(this.pawn, true, DamageDefOf.Cut, ThingDefOf.Metalhorror, BodyPartGroupDefOf.LeftBlade);
			}
			else if (!this.pawn.Dead)
			{
				DamageWorker.DamageResult damageResult = this.pawn.TakeDamage(new DamageInfo(DamageDefOf.Cut, 20f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
				if (damageResult.hediffs != null)
				{
					foreach (Hediff hediff in damageResult.hediffs)
					{
						hediff.sourceDef = ThingDefOf.Metalhorror;
						hediff.sourceBodyPartGroup = BodyPartGroupDefOf.LeftBlade;
					}
				}
			}
			if (!this.sympatheticEmergence)
			{
				Hediff_MetalhorrorImplant.TmpEmerging.Clear();
				foreach (Pawn target in this.pawn.MapHeld.mapPawns.AllHumanlike)
				{
					this.TryTriggerSympatheticEmergence(target, Hediff_MetalhorrorImplant.TmpEmerging);
				}
				foreach (Thing thing in this.pawn.MapHeld.listerThings.ThingsInGroup(ThingRequestGroup.Corpse))
				{
					Corpse corpse = thing as Corpse;
					if (corpse != null && corpse.InnerPawn != null)
					{
						this.TryTriggerSympatheticEmergence(corpse.InnerPawn, Hediff_MetalhorrorImplant.TmpEmerging);
					}
				}
				Hediff_MetalhorrorImplant.TmpEmerging.SortByDescending(([TupleElementNames(new string[]
				{
					"hediff",
					"message"
				})] ValueTuple<Hediff_MetalhorrorImplant, string> tuple) => tuple.Item1.ageTicks);
				StringBuilder stringBuilder = new StringBuilder();
				StringBuilder stringBuilder2 = new StringBuilder();
				foreach (ValueTuple<Hediff_MetalhorrorImplant, string> valueTuple in Hediff_MetalhorrorImplant.TmpEmerging)
				{
					Hediff_MetalhorrorImplant item = valueTuple.Item1;
					string item2 = valueTuple.Item2;
					stringBuilder.AppendLine(item2);
					if (item.LiedAboutInspection)
					{
						if (stringBuilder2.Length == 0)
						{
							StringBuilder stringBuilder3 = stringBuilder2;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 1);
							defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("MetalhorrorSurgicalInspectionDetails".Translate());
							defaultInterpolatedStringHandler.AppendLiteral(":\n");
							stringBuilder3.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear());
						}
						StringBuilder stringBuilder4 = stringBuilder2;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(3, 1);
						defaultInterpolatedStringHandler2.AppendLiteral(" - ");
						defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>(item.pawn.NameShortColored);
						stringBuilder4.AppendLine(defaultInterpolatedStringHandler2.ToStringAndClear());
					}
				}
				TaggedString taggedString;
				if (Hediff_MetalhorrorImplant.TmpEmerging.Count == 1)
				{
					taggedString = "MetalhorrorEmergingDesc".Translate();
				}
				else
				{
					taggedString = "MetalhorrorEmergingPluralDesc".Translate();
				}
				TaggedString t = taggedString;
				string str = "\n\n";
				StringBuilder stringBuilder5 = stringBuilder;
				taggedString = t + (str + ((stringBuilder5 != null) ? stringBuilder5.ToString() : null));
				if (stringBuilder2.Length > 0)
				{
					TaggedString t2 = taggedString;
					string str2 = "\n";
					StringBuilder stringBuilder6 = stringBuilder2;
					taggedString = t2 + (str2 + ((stringBuilder6 != null) ? stringBuilder6.ToString() : null));
				}
				taggedString += "\n" + this.emergeReason;
				taggedString += "\n\n" + "MetalhorrorEmergingDescAppended".Translate();
				Find.LetterStack.ReceiveLetter("MetalhorrorEmergingLabel".Translate(), taggedString.Resolve(), LetterDefOf.ThreatBig, (from hd in Hediff_MetalhorrorImplant.TmpEmerging
				select hd.Item1.pawn).ToList<Pawn>(), null, null, null, null, 0, true);
				Find.AnalysisManager.RemoveAnalysisDetails(this.Biosignature);
				Hediff_MetalhorrorImplant.TmpEmerging.Clear();
			}
			this.DoEmergingEffects();
			if (this.pawn.Dead)
			{
				MetalhorrorUtility.SpawnMetalhorror(this.pawn, this);
			}
		}

		// Token: 0x0600219E RID: 8606 RVA: 0x000C7BD0 File Offset: 0x000C5DD0
		private void TryTriggerSympatheticEmergence(Pawn target, [TupleElementNames(new string[]
		{
			"hediff",
			"message"
		})] List<ValueTuple<Hediff_MetalhorrorImplant, string>> list)
		{
			Hediff_MetalhorrorImplant firstHediff = target.health.hediffSet.GetFirstHediff<Hediff_MetalhorrorImplant>();
			if (firstHediff == null || firstHediff.Biosignature != this.Biosignature)
			{
				return;
			}
			if (target != this.pawn)
			{
				MetalhorrorUtility.TryEmerge(target, null, true);
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(7, 2);
			defaultInterpolatedStringHandler.AppendLiteral("  - ");
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>(target.NameShortColored);
			defaultInterpolatedStringHandler.AppendLiteral(" (");
			defaultInterpolatedStringHandler.AppendFormatted(firstHediff.ImplantSource.GetSourceDesc());
			defaultInterpolatedStringHandler.AppendLiteral(")");
			string item = defaultInterpolatedStringHandler.ToStringAndClear();
			list.Add(new ValueTuple<Hediff_MetalhorrorImplant, string>(firstHediff, item));
		}

		// Token: 0x0600219F RID: 8607 RVA: 0x000C7C74 File Offset: 0x000C5E74
		private void DoEmergingEffects()
		{
			EffecterDefOf.MetalhorrorEmerging.Spawn(this.pawn.PositionHeld, this.pawn.MapHeld, 1f).Cleanup();
			IntVec3 positionHeld = this.pawn.PositionHeld;
			Map mapHeld = this.pawn.MapHeld;
			CellRect cellRect = new CellRect(positionHeld.x, positionHeld.z, 3, 3);
			for (int i = 0; i < 5; i++)
			{
				IntVec3 randomCell = cellRect.RandomCell;
				if (randomCell.InBounds(mapHeld) && GenSight.LineOfSight(randomCell, positionHeld, mapHeld))
				{
					FilthMaker.TryMakeFilth(randomCell, mapHeld, (i % 2 == 0) ? ThingDefOf.Filth_Blood : ThingDefOf.Filth_GrayFlesh, 1, FilthSourceFlags.None, true);
				}
			}
		}

		// Token: 0x060021A0 RID: 8608 RVA: 0x000C7D1F File Offset: 0x000C5F1F
		public void Detect(string reason, bool noticed)
		{
			base.SetVisible();
			this.knowsDetected = noticed;
			if (this.knowsDetected)
			{
				this.Emerge(reason, false);
			}
		}

		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x060021A1 RID: 8609 RVA: 0x000C7D3E File Offset: 0x000C5F3E
		public override string TipStringExtra
		{
			get
			{
				return this.implantSource.GetSourceDesc().CapitalizeFirst();
			}
		}

		// Token: 0x060021A2 RID: 8610 RVA: 0x000C7D50 File Offset: 0x000C5F50
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Hediff_MetalhorrorImplant.<GetGizmos>d__42 <GetGizmos>d__ = new Hediff_MetalhorrorImplant.<GetGizmos>d__42(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x060021A3 RID: 8611 RVA: 0x000C7D60 File Offset: 0x000C5F60
		private void DoPassiveTransmission()
		{
			if (this.pawn.InBed())
			{
				foreach (Pawn pawn in this.pawn.CurrentBed().CurOccupants)
				{
					if (pawn != this.pawn)
					{
						MetalhorrorUtility.Infect(pawn, this.pawn, "SleepImplant", null);
					}
				}
			}
		}

		// Token: 0x060021A4 RID: 8612 RVA: 0x000C7DD8 File Offset: 0x000C5FD8
		private void FilthTick(int delta)
		{
			this.UpdateFilthTimer(delta);
			if (this.filthTimer <= 0)
			{
				if (!FilthMaker.CanMakeFilth(this.pawn.PositionHeld, this.pawn.MapHeld, ThingDefOf.Filth_GrayFleshNoticeable, FilthSourceFlags.None))
				{
					this.filthTimer = 600;
				}
				else
				{
					this.filthTimer = Mathf.CeilToInt(Hediff_MetalhorrorImplant.FilthDelayDays.RandomInRange * 60000f);
					this.spawnFilthAt = GenTicks.TicksGame + Mathf.CeilToInt(Hediff_MetalhorrorImplant.SpawnFilthDelayHours.RandomInRange * 2500f);
					this.filthMap = this.pawn.MapHeld;
					this.filthPos = this.pawn.PositionHeld;
				}
			}
			this.TryPlaceFilth();
		}

		// Token: 0x060021A5 RID: 8613 RVA: 0x000C7E94 File Offset: 0x000C6094
		private void TryPlaceFilth()
		{
			if (!this.filthPos.IsValid)
			{
				return;
			}
			if (this.filthMap != null && !this.filthMap.Disposed)
			{
				if (GenTicks.TicksGame >= this.spawnFilthAt)
				{
					Filth filth;
					if (FilthMaker.TryMakeFilth(this.filthPos, this.filthMap, ThingDefOf.Filth_GrayFleshNoticeable, out filth, 1, FilthSourceFlags.None, true))
					{
						((FilthGrayFleshNoticeable)filth).biosignature = this.Biosignature;
					}
					this.filthPos = IntVec3.Invalid;
					this.filthMap = null;
				}
				return;
			}
			if (!this.pawn.SpawnedOrAnyParentSpawned)
			{
				return;
			}
			this.filthMap = this.pawn.MapHeld;
			this.filthPos = this.pawn.PositionHeld;
			this.spawnFilthAt = GenTicks.TicksGame + 2500;
		}

		// Token: 0x060021A6 RID: 8614 RVA: 0x000C7F54 File Offset: 0x000C6154
		public override string GetInspectString()
		{
			if (Prefs.DevMode && DebugSettings.godMode)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 3);
				defaultInterpolatedStringHandler.AppendLiteral("DEV: Bio: ");
				defaultInterpolatedStringHandler.AppendFormatted(this.BiosignatureName);
				defaultInterpolatedStringHandler.AppendLiteral(", mark filth: ");
				defaultInterpolatedStringHandler.AppendFormatted<float>(this.filthTimer.TicksToSeconds(), "0");
				defaultInterpolatedStringHandler.AppendLiteral("s ");
				string value;
				if (this.spawnFilthAt >= GenTicks.TicksGame)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(12, 1);
					defaultInterpolatedStringHandler2.AppendLiteral(" (spawns: ");
					defaultInterpolatedStringHandler2.AppendFormatted<float>((this.spawnFilthAt - GenTicks.TicksGame).TicksToSeconds(), "0");
					defaultInterpolatedStringHandler2.AppendLiteral("s)");
					value = defaultInterpolatedStringHandler2.ToStringAndClear();
				}
				else
				{
					value = "";
				}
				defaultInterpolatedStringHandler.AppendFormatted(value);
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
			return base.GetInspectString();
		}

		// Token: 0x060021A7 RID: 8615 RVA: 0x000C8036 File Offset: 0x000C6236
		private void UpdateFilthTimer(int delta)
		{
			if (this.pawn.InBed() || this.pawn.GetPosture().Laying() || this.pawn.Downed)
			{
				return;
			}
			this.filthTimer -= delta;
		}

		// Token: 0x060021A8 RID: 8616 RVA: 0x000C8074 File Offset: 0x000C6274
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.emerging, "emerging", false, false);
			Scribe_Values.Look<int>(ref this.emergeAt, "emergeAt", 0, false);
			Scribe_Values.Look<int>(ref this.filthTimer, "filthTimer", 0, false);
			Scribe_Values.Look<bool>(ref this.knowsDetected, "knowsDetected", false, false);
			Scribe_Values.Look<bool>(ref this.sympatheticEmergence, "sympatheticEmergence", false, false);
			Scribe_Values.Look<bool>(ref this.sentMaturingMessage, "sentMaturingMessage", false, false);
			Scribe_Values.Look<int>(ref this.spawnFilthAt, "spawnFilthAt", 0, false);
			Scribe_Values.Look<IntVec3>(ref this.filthPos, "filthPos", default(IntVec3), false);
			Scribe_Values.Look<bool>(ref this.liedAboutInspection, "liedAboutInspection", false, false);
			Scribe_References.Look<Map>(ref this.filthMap, "filthMap", false);
			Scribe_Deep.Look<ImplantSource>(ref this.implantSource, "implantSource", Array.Empty<object>());
		}

		// Token: 0x04001979 RID: 6521
		private ImplantSource implantSource = new ImplantSource();

		// Token: 0x0400197A RID: 6522
		private bool emerging;

		// Token: 0x0400197B RID: 6523
		private bool knowsDetected;

		// Token: 0x0400197C RID: 6524
		private bool sympatheticEmergence;

		// Token: 0x0400197D RID: 6525
		private bool sentMaturingMessage;

		// Token: 0x0400197E RID: 6526
		private string emergeReason;

		// Token: 0x0400197F RID: 6527
		private int emergeAt;

		// Token: 0x04001980 RID: 6528
		private bool liedAboutInspection;

		// Token: 0x04001981 RID: 6529
		private int filthTimer;

		// Token: 0x04001982 RID: 6530
		private int spawnFilthAt;

		// Token: 0x04001983 RID: 6531
		private IntVec3 filthPos = IntVec3.Invalid;

		// Token: 0x04001984 RID: 6532
		private Map filthMap;

		// Token: 0x04001985 RID: 6533
		public bool debugDiscoverNextInteraction;

		// Token: 0x04001986 RID: 6534
		private static readonly IntRange TicksToEmerge = new IntRange(60, 180);

		// Token: 0x04001987 RID: 6535
		private static readonly IntRange TicksToEmergeSympathetic = new IntRange(60, 600);

		// Token: 0x04001988 RID: 6536
		private static readonly FloatRange InitialFilthDelayDays = new FloatRange(1.5f, 2.5f);

		// Token: 0x04001989 RID: 6537
		private static readonly FloatRange FilthDelayDays = new FloatRange(3f, 10f);

		// Token: 0x0400198A RID: 6538
		private static readonly FloatRange SpawnFilthDelayHours = new FloatRange(1f, 12f);

		// Token: 0x0400198B RID: 6539
		[TupleElementNames(new string[]
		{
			"hediff",
			"message"
		})]
		private static readonly List<ValueTuple<Hediff_MetalhorrorImplant, string>> TmpEmerging = new List<ValueTuple<Hediff_MetalhorrorImplant, string>>();
	}
}
