using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200190A RID: 6410
	public class CompMetalhorror : ThingComp
	{
		// Token: 0x17001BAB RID: 7083
		// (get) Token: 0x0600A0A3 RID: 41123 RVA: 0x00397554 File Offset: 0x00395754
		public CompCanBeDormant DormantComp
		{
			get
			{
				CompCanBeDormant result;
				if ((result = this.intDormantComp) == null)
				{
					result = (this.intDormantComp = this.parent.GetComp<CompCanBeDormant>());
				}
				return result;
			}
		}

		// Token: 0x17001BAC RID: 7084
		// (get) Token: 0x0600A0A4 RID: 41124 RVA: 0x00397580 File Offset: 0x00395780
		public LordJob_Metalhorror LordJob
		{
			get
			{
				LordJob_Metalhorror result;
				if ((result = this.intLordJob) == null)
				{
					result = (this.intLordJob = (LordJob_Metalhorror)this.Pawn.GetLord().LordJob);
				}
				return result;
			}
		}

		// Token: 0x17001BAD RID: 7085
		// (get) Token: 0x0600A0A5 RID: 41125 RVA: 0x003975B5 File Offset: 0x003957B5
		public bool Hibernating
		{
			get
			{
				return this.DormantComp.Awake;
			}
		}

		// Token: 0x17001BAE RID: 7086
		// (get) Token: 0x0600A0A6 RID: 41126 RVA: 0x003975C2 File Offset: 0x003957C2
		public int Biosignature
		{
			get
			{
				ImplantSource implantSource = this.implantSource;
				if (implantSource == null)
				{
					return 0;
				}
				return implantSource.Biosignature;
			}
		}

		// Token: 0x17001BAF RID: 7087
		// (get) Token: 0x0600A0A7 RID: 41127 RVA: 0x003975D5 File Offset: 0x003957D5
		public string BiosignatureName
		{
			get
			{
				ImplantSource implantSource = this.implantSource;
				return ((implantSource != null) ? implantSource.BiosignatureName : null) ?? "INVALID";
			}
		}

		// Token: 0x17001BB0 RID: 7088
		// (get) Token: 0x0600A0A8 RID: 41128 RVA: 0x003975F2 File Offset: 0x003957F2
		public CompProperties_Metalhorror Props
		{
			get
			{
				return (CompProperties_Metalhorror)this.props;
			}
		}

		// Token: 0x17001BB1 RID: 7089
		// (get) Token: 0x0600A0A9 RID: 41129 RVA: 0x003975FF File Offset: 0x003957FF
		public Pawn Pawn
		{
			get
			{
				return (Pawn)this.parent;
			}
		}

		// Token: 0x0600A0AA RID: 41130 RVA: 0x0039760C File Offset: 0x0039580C
		public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (totalDamageDealt <= 0f)
			{
				return;
			}
			Pawn pawn = dinfo.Instigator as Pawn;
			if (pawn != null)
			{
				this.lastDamagedBy = pawn;
			}
		}

		// Token: 0x0600A0AB RID: 41131 RVA: 0x0039763C File Offset: 0x0039583C
		public override void PostPostMake()
		{
			this.speedBoostTimer = CompMetalhorror.SpeedBoostTicksRange.RandomInRange;
		}

		// Token: 0x0600A0AC RID: 41132 RVA: 0x0039765C File Offset: 0x0039585C
		public override void Notify_Killed(Map prevMap, DamageInfo? dinfo = null)
		{
			FilthMaker.TryMakeFilth(this.Pawn.PositionHeld, prevMap, ThingDefOf.Filth_MetalhorrorDebris, 1, FilthSourceFlags.None, true);
		}

		// Token: 0x0600A0AD RID: 41133 RVA: 0x00397678 File Offset: 0x00395878
		public void FindOrCreateEmergedLord()
		{
			foreach (Lord lord in this.Pawn.MapHeld.lordManager.lords)
			{
				LordJob_Metalhorror lordJob_Metalhorror = lord.LordJob as LordJob_Metalhorror;
				if (lordJob_Metalhorror != null && lordJob_Metalhorror.biosignature == this.implantSource.Biosignature)
				{
					lord.AddPawn(this.Pawn);
					this.intLordJob = (LordJob_Metalhorror)lord.LordJob;
					return;
				}
			}
			Lord lord2 = LordMaker.MakeNewLord(Faction.OfEntities, new LordJob_Metalhorror(), this.Pawn.MapHeld, null);
			this.intLordJob = (LordJob_Metalhorror)lord2.LordJob;
			this.intLordJob.biosignature = this.implantSource.Biosignature;
			lord2.AddPawn(this.Pawn);
		}

		// Token: 0x0600A0AE RID: 41134 RVA: 0x00397764 File Offset: 0x00395964
		public override void CompTick()
		{
			if (this.Hibernating)
			{
				return;
			}
			this.FilthTick();
			this.SpeedBoostTick();
		}

		// Token: 0x0600A0AF RID: 41135 RVA: 0x0039777C File Offset: 0x0039597C
		public override void PostPreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			absorbed = false;
			float amount = 0.2f * dinfo.Amount;
			if (dinfo.Def == DamageDefOf.Burn)
			{
				amount = 2f * dinfo.Amount;
			}
			dinfo.SetAmount(amount);
		}

		// Token: 0x0600A0B0 RID: 41136 RVA: 0x003977BC File Offset: 0x003959BC
		public override string CompInspectStringExtra()
		{
			if (this.implantSource == null)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (this.emergedFrom != null)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("EmergedFrom".Translate());
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted(this.emergedFrom.LabelShort);
				stringBuilder2.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			stringBuilder.Append(this.implantSource.GetSourceDesc().CapitalizeFirst() ?? "");
			if (Prefs.DevMode && DebugSettings.godMode && this.Pawn.GetLord() != null)
			{
				StringBuilder stringBuilder3 = stringBuilder;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(32, 2);
				defaultInterpolatedStringHandler2.AppendLiteral("\nDEV: Biosignature: ");
				defaultInterpolatedStringHandler2.AppendFormatted(this.BiosignatureName);
				defaultInterpolatedStringHandler2.AppendLiteral(" (");
				defaultInterpolatedStringHandler2.AppendFormatted<int>(this.Pawn.GetLord().ownedPawns.Count - 1);
				defaultInterpolatedStringHandler2.AppendLiteral(" siblings)");
				stringBuilder3.Append(defaultInterpolatedStringHandler2.ToStringAndClear());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600A0B1 RID: 41137 RVA: 0x003978D0 File Offset: 0x00395AD0
		private void SpeedBoostTick()
		{
			this.speedBoostTimer--;
			if (this.speedBoostTimer <= 0)
			{
				this.speedBoostTimer = CompMetalhorror.SpeedBoostTicksRange.RandomInRange;
				if (this.Pawn.health.hediffSet.HasHediff(HediffDefOf.MetalhorrorSpeedBoost, false))
				{
					return;
				}
				this.Pawn.health.AddHediff(HediffDefOf.MetalhorrorSpeedBoost, null, null, null);
			}
		}

		// Token: 0x0600A0B2 RID: 41138 RVA: 0x00397948 File Offset: 0x00395B48
		private void FilthTick()
		{
			if (!this.Pawn.Spawned)
			{
				return;
			}
			this.filthTimer--;
			if (this.filthTimer <= 0)
			{
				CompMetalhorror.filthOptions.Clear();
				this.filthTimer = CompMetalhorror.DropFilthTicksRange.RandomInRange;
				int found = 0;
				this.Pawn.Map.floodFiller.FloodFill(this.Pawn.Position, (IntVec3 x) => x.WalkableBy(this.Pawn.Map, this.Pawn) && !x.InHorDistOf(this.Pawn.Position, 4f) && !PawnUtility.AnyPawnBlockingPathAt(x, this.Pawn, true, false, false, false), delegate(IntVec3 cell)
				{
					List<Thing> thingList = cell.GetThingList(this.Pawn.Map);
					for (int i = 0; i < thingList.Count; i++)
					{
						if (thingList[i].def == ThingDefOf.Filth_GrayFleshNoticeable || thingList[i].def == ThingDefOf.Filth_GrayFlesh)
						{
							int found = found;
							found++;
							if (found >= 12)
							{
								return true;
							}
						}
						else
						{
							CompMetalhorror.filthOptions.Add(cell);
						}
					}
					return false;
				}, int.MaxValue, false, null);
				if (found < 12 && CompMetalhorror.filthOptions.Any<IntVec3>())
				{
					FilthMaker.TryMakeFilth(CompMetalhorror.filthOptions.RandomElement<IntVec3>(), this.Pawn.Map, ThingDefOf.Filth_GrayFlesh, 1, FilthSourceFlags.None, true);
				}
				CompMetalhorror.filthOptions.Clear();
			}
		}

		// Token: 0x0600A0B3 RID: 41139 RVA: 0x00397A30 File Offset: 0x00395C30
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.filthTimer, "filthTimer", 0, false);
			Scribe_Values.Look<int>(ref this.speedBoostTimer, "speedBoostTimer", 0, false);
			Scribe_References.Look<Pawn>(ref this.lastDamagedBy, "lastDamagedBy", false);
			Scribe_References.Look<Pawn>(ref this.emergedFrom, "emergedFrom", true);
			Scribe_Deep.Look<ImplantSource>(ref this.implantSource, "implantSource", Array.Empty<object>());
		}

		// Token: 0x04005ED7 RID: 24279
		public ImplantSource implantSource;

		// Token: 0x04005ED8 RID: 24280
		public Pawn emergedFrom;

		// Token: 0x04005ED9 RID: 24281
		private Pawn lastDamagedBy;

		// Token: 0x04005EDA RID: 24282
		private int filthTimer;

		// Token: 0x04005EDB RID: 24283
		private int speedBoostTimer;

		// Token: 0x04005EDC RID: 24284
		private CompCanBeDormant intDormantComp;

		// Token: 0x04005EDD RID: 24285
		private LordJob_Metalhorror intLordJob;

		// Token: 0x04005EDE RID: 24286
		private static readonly IntRange DropFilthTicksRange = new IntRange(600, 1800);

		// Token: 0x04005EDF RID: 24287
		private static readonly IntRange SpeedBoostTicksRange = new IntRange(1200, 2400);

		// Token: 0x04005EE0 RID: 24288
		private const int FilthSearchRadius = 4;

		// Token: 0x04005EE1 RID: 24289
		private const int MaxFilth = 12;

		// Token: 0x04005EE2 RID: 24290
		private static readonly List<IntVec3> filthOptions = new List<IntVec3>();
	}
}
