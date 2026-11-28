using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001AB1 RID: 6833
	[StaticConstructorOnStartup]
	public class Building_GeneAssembler : Building, IThingHolder
	{
		// Token: 0x17001EE5 RID: 7909
		// (get) Token: 0x0600ACF9 RID: 44281 RVA: 0x003CF5CA File Offset: 0x003CD7CA
		public float ProgressPercent
		{
			get
			{
				return this.workDone / this.totalWorkRequired;
			}
		}

		// Token: 0x17001EE6 RID: 7910
		// (get) Token: 0x0600ACFA RID: 44282 RVA: 0x003CF5D9 File Offset: 0x003CD7D9
		public bool Working
		{
			get
			{
				return this.workingInt;
			}
		}

		// Token: 0x17001EE7 RID: 7911
		// (get) Token: 0x0600ACFB RID: 44283 RVA: 0x003CF5E4 File Offset: 0x003CD7E4
		private CompPowerTrader PowerTraderComp
		{
			get
			{
				CompPowerTrader result;
				if ((result = this.cachedPowerComp) == null)
				{
					result = (this.cachedPowerComp = this.TryGetComp<CompPowerTrader>());
				}
				return result;
			}
		}

		// Token: 0x17001EE8 RID: 7912
		// (get) Token: 0x0600ACFC RID: 44284 RVA: 0x003CF60A File Offset: 0x003CD80A
		public bool PowerOn
		{
			get
			{
				return this.PowerTraderComp.PowerOn;
			}
		}

		// Token: 0x17001EE9 RID: 7913
		// (get) Token: 0x0600ACFD RID: 44285 RVA: 0x003CF617 File Offset: 0x003CD817
		public List<Thing> ConnectedFacilities
		{
			get
			{
				return this.TryGetComp<CompAffectedByFacilities>().LinkedFacilitiesListForReading;
			}
		}

		// Token: 0x17001EEA RID: 7914
		// (get) Token: 0x0600ACFE RID: 44286 RVA: 0x003CF624 File Offset: 0x003CD824
		public int ArchitesCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.innerContainer.Count; i++)
				{
					if (this.innerContainer[i].def == ThingDefOf.ArchiteCapsule)
					{
						num += this.innerContainer[i].stackCount;
					}
				}
				return num;
			}
		}

		// Token: 0x17001EEB RID: 7915
		// (get) Token: 0x0600ACFF RID: 44287 RVA: 0x003CF676 File Offset: 0x003CD876
		public int ArchitesRequiredNow
		{
			get
			{
				return this.architesRequired - this.ArchitesCount;
			}
		}

		// Token: 0x17001EEC RID: 7916
		// (get) Token: 0x0600AD00 RID: 44288 RVA: 0x003CF688 File Offset: 0x003CD888
		private HashSet<Thing> UsedFacilities
		{
			get
			{
				this.tmpUsedFacilities.Clear();
				if (!this.genepacksToRecombine.NullOrEmpty<Genepack>())
				{
					List<Thing> connectedFacilities = this.ConnectedFacilities;
					for (int i = 0; i < this.genepacksToRecombine.Count; i++)
					{
						for (int j = 0; j < connectedFacilities.Count; j++)
						{
							if (!this.tmpUsedFacilities.Contains(connectedFacilities[j]))
							{
								CompGenepackContainer compGenepackContainer = connectedFacilities[j].TryGetComp<CompGenepackContainer>();
								if (compGenepackContainer != null && compGenepackContainer.ContainedGenepacks.Contains(this.genepacksToRecombine[i]))
								{
									this.tmpUsedFacilities.Add(connectedFacilities[j]);
									break;
								}
							}
						}
					}
				}
				return this.tmpUsedFacilities;
			}
		}

		// Token: 0x17001EED RID: 7917
		// (get) Token: 0x0600AD01 RID: 44289 RVA: 0x003CF738 File Offset: 0x003CD938
		public AcceptanceReport CanBeWorkedOnNow
		{
			get
			{
				if (!this.Working)
				{
					return false;
				}
				if (this.ArchitesRequiredNow > 0)
				{
					return false;
				}
				if (!this.PowerOn)
				{
					return "NoPower".Translate().CapitalizeFirst();
				}
				foreach (Thing thing in this.UsedFacilities)
				{
					CompPowerTrader compPowerTrader = thing.TryGetComp<CompPowerTrader>();
					if (compPowerTrader != null && !compPowerTrader.PowerOn)
					{
						return "GenebankUnpowered".Translate();
					}
				}
				if (this.MaxComplexity() < this.TotalGCX)
				{
					return "GeneProcessorUnpowered".Translate();
				}
				return true;
			}
		}

		// Token: 0x17001EEE RID: 7918
		// (get) Token: 0x0600AD02 RID: 44290 RVA: 0x003CF80C File Offset: 0x003CDA0C
		private int TotalGCX
		{
			get
			{
				if (!this.Working)
				{
					return 0;
				}
				if (this.cachedComplexity == null)
				{
					this.cachedComplexity = new int?(0);
					if (!this.genepacksToRecombine.NullOrEmpty<Genepack>())
					{
						List<GeneDefWithType> list = new List<GeneDefWithType>();
						for (int i = 0; i < this.genepacksToRecombine.Count; i++)
						{
							if (this.genepacksToRecombine[i].GeneSet != null)
							{
								for (int j = 0; j < this.genepacksToRecombine[i].GeneSet.GenesListForReading.Count; j++)
								{
									list.Add(new GeneDefWithType(this.genepacksToRecombine[i].GeneSet.GenesListForReading[j], true));
								}
							}
						}
						List<GeneDef> list2 = list.NonOverriddenGenes();
						for (int k = 0; k < list2.Count; k++)
						{
							this.cachedComplexity += list2[k].biostatCpx;
						}
					}
				}
				return this.cachedComplexity.Value;
			}
		}

		// Token: 0x0600AD03 RID: 44291 RVA: 0x003CF934 File Offset: 0x003CDB34
		public override void PostPostMake()
		{
			if (!ModLister.CheckBiotech("Gene assembler"))
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostPostMake();
			this.innerContainer = new ThingOwner<Thing>(this);
		}

		// Token: 0x0600AD04 RID: 44292 RVA: 0x003CF95C File Offset: 0x003CDB5C
		protected override void Tick()
		{
			base.Tick();
			if (this.IsHashIntervalTick(250))
			{
				bool flag = this.lastWorkedTick + 250 + 2 >= Find.TickManager.TicksGame;
				this.PowerTraderComp.PowerOutput = (flag ? (-base.PowerComp.Props.PowerConsumption) : (-base.PowerComp.Props.idlePowerDraw));
			}
			if (this.Working && this.IsHashIntervalTick(180))
			{
				this.CheckAllContainersValid();
			}
		}

		// Token: 0x0600AD05 RID: 44293 RVA: 0x003CF9E8 File Offset: 0x003CDBE8
		public void Start(List<Genepack> packs, int architesRequired, string xenotypeName, XenotypeIconDef iconDef)
		{
			this.Reset();
			this.genepacksToRecombine = packs;
			this.architesRequired = architesRequired;
			this.xenotypeName = xenotypeName;
			this.iconDef = iconDef;
			this.workingInt = true;
			this.totalWorkRequired = GeneTuning.ComplexityToCreationHoursCurve.Evaluate((float)this.TotalGCX) * 2500f;
		}

		// Token: 0x0600AD06 RID: 44294 RVA: 0x003CFA3C File Offset: 0x003CDC3C
		public void DoWork(float workAmount)
		{
			this.workDone += workAmount;
			this.lastWorkAmount = workAmount;
			this.lastWorkedTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600AD07 RID: 44295 RVA: 0x003CFA64 File Offset: 0x003CDC64
		public void Finish()
		{
			if (!this.genepacksToRecombine.NullOrEmpty<Genepack>())
			{
				SoundDefOf.GeneAssembler_Complete.PlayOneShot(SoundInfo.InMap(this, MaintenanceType.None));
				Xenogerm xenogerm = (Xenogerm)ThingMaker.MakeThing(ThingDefOf.Xenogerm, null);
				xenogerm.Initialize(this.genepacksToRecombine, this.xenotypeName, this.iconDef);
				if (GenPlace.TryPlaceThing(xenogerm, this.InteractionCell, base.Map, ThingPlaceMode.Near, null, null, null, 1))
				{
					Messages.Message("MessageXenogermCompleted".Translate(), xenogerm, MessageTypeDefOf.PositiveEvent, true);
				}
			}
			if (this.architesRequired > 0)
			{
				for (int i = this.innerContainer.Count - 1; i >= 0; i--)
				{
					if (this.innerContainer[i].def == ThingDefOf.ArchiteCapsule)
					{
						Thing thing = this.innerContainer[i].SplitOff(Mathf.Min(this.innerContainer[i].stackCount, this.architesRequired));
						this.architesRequired -= thing.stackCount;
						thing.Destroy(DestroyMode.Vanish);
						if (this.architesRequired <= 0)
						{
							break;
						}
					}
				}
			}
			this.Reset();
		}

		// Token: 0x0600AD08 RID: 44296 RVA: 0x003CFB94 File Offset: 0x003CDD94
		public List<Genepack> GetGenepacks(bool includePowered, bool includeUnpowered)
		{
			this.tmpGenepacks.Clear();
			List<Thing> connectedFacilities = this.ConnectedFacilities;
			if (connectedFacilities != null)
			{
				foreach (Thing thing in connectedFacilities)
				{
					CompGenepackContainer compGenepackContainer = thing.TryGetComp<CompGenepackContainer>();
					if (compGenepackContainer != null)
					{
						CompPowerTrader compPowerTrader = thing.TryGetComp<CompPowerTrader>();
						bool flag = compPowerTrader == null || compPowerTrader.PowerOn;
						if ((includePowered && flag) || (includeUnpowered && !flag))
						{
							this.tmpGenepacks.AddRange(compGenepackContainer.ContainedGenepacks);
						}
					}
				}
			}
			return this.tmpGenepacks;
		}

		// Token: 0x0600AD09 RID: 44297 RVA: 0x003CFC38 File Offset: 0x003CDE38
		public CompGenepackContainer GetGeneBankHoldingPack(Genepack pack)
		{
			List<Thing> connectedFacilities = this.ConnectedFacilities;
			if (connectedFacilities != null)
			{
				foreach (Thing thing in connectedFacilities)
				{
					CompGenepackContainer compGenepackContainer = thing.TryGetComp<CompGenepackContainer>();
					if (compGenepackContainer != null)
					{
						using (List<Genepack>.Enumerator enumerator2 = compGenepackContainer.ContainedGenepacks.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								if (enumerator2.Current == pack)
								{
									return compGenepackContainer;
								}
							}
						}
					}
				}
			}
			return null;
		}

		// Token: 0x0600AD0A RID: 44298 RVA: 0x003CFCD8 File Offset: 0x003CDED8
		public int MaxComplexity()
		{
			int num = 6;
			List<Thing> connectedFacilities = this.ConnectedFacilities;
			if (connectedFacilities != null)
			{
				foreach (Thing thing in connectedFacilities)
				{
					CompPowerTrader compPowerTrader = thing.TryGetComp<CompPowerTrader>();
					if (compPowerTrader == null || compPowerTrader.PowerOn)
					{
						num += (int)thing.GetStatValue(StatDefOf.GeneticComplexityIncrease, true, -1);
					}
				}
			}
			return num;
		}

		// Token: 0x0600AD0B RID: 44299 RVA: 0x003CFD54 File Offset: 0x003CDF54
		private void Reset()
		{
			this.workingInt = false;
			this.genepacksToRecombine = null;
			this.xenotypeName = null;
			this.cachedComplexity = null;
			this.iconDef = XenotypeIconDefOf.Basic;
			this.workDone = 0f;
			this.lastWorkedTick = -999;
			this.architesRequired = 0;
			this.innerContainer.TryDropAll(this.def.hasInteractionCell ? this.InteractionCell : base.Position, base.Map, ThingPlaceMode.Near, null, null, true);
		}

		// Token: 0x0600AD0C RID: 44300 RVA: 0x003CFDDC File Offset: 0x003CDFDC
		private void CheckAllContainersValid()
		{
			if (this.genepacksToRecombine.NullOrEmpty<Genepack>())
			{
				return;
			}
			List<Thing> connectedFacilities = this.ConnectedFacilities;
			for (int i = 0; i < this.genepacksToRecombine.Count; i++)
			{
				bool flag = false;
				for (int j = 0; j < connectedFacilities.Count; j++)
				{
					CompGenepackContainer compGenepackContainer = connectedFacilities[j].TryGetComp<CompGenepackContainer>();
					if (compGenepackContainer != null && compGenepackContainer.ContainedGenepacks.Contains(this.genepacksToRecombine[i]))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					Messages.Message("MessageXenogermCancelledMissingPack".Translate(this), this, MessageTypeDefOf.NegativeEvent, true);
					this.Reset();
					return;
				}
			}
		}

		// Token: 0x0600AD0D RID: 44301 RVA: 0x003CFE8A File Offset: 0x003CE08A
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x0600AD0E RID: 44302 RVA: 0x003CFE98 File Offset: 0x003CE098
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x0600AD0F RID: 44303 RVA: 0x003CFEA0 File Offset: 0x003CE0A0
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Building_GeneAssembler.<GetGizmos>d__49 <GetGizmos>d__ = new Building_GeneAssembler.<GetGizmos>d__49(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600AD10 RID: 44304 RVA: 0x003CFEB0 File Offset: 0x003CE0B0
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (this.Working)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text = text + ("CreatingXenogerm".Translate() + ": " + this.xenotypeName.CapitalizeFirst() + "\n" + "ComplexityTotal".Translate() + ": ") + this.TotalGCX.ToString();
				text += "\n" + "Progress".Translate() + ": " + this.ProgressPercent.ToStringPercent();
				int numTicks = Mathf.RoundToInt((this.totalWorkRequired - this.workDone) / ((this.lastWorkAmount > 0f) ? this.lastWorkAmount : this.GetStatValue(StatDefOf.AssemblySpeedFactor, true, -1)));
				text = text + " (" + "DurationLeft".Translate(numTicks.ToStringTicksToPeriod(true, false, true, true, false)).Resolve() + ")";
				AcceptanceReport canBeWorkedOnNow = this.CanBeWorkedOnNow;
				if (!canBeWorkedOnNow.Accepted && !canBeWorkedOnNow.Reason.NullOrEmpty())
				{
					text = text + "\n" + ("AssemblyPaused".Translate() + ": " + canBeWorkedOnNow.Reason).Colorize(ColorLibrary.RedReadable);
				}
				if (this.architesRequired > 0)
				{
					text = string.Concat(new string[]
					{
						text,
						"\n" + "ArchitesRequired".Translate() + ": ",
						this.ArchitesCount.ToString(),
						" / ",
						this.architesRequired.ToString()
					});
				}
			}
			return text;
		}

		// Token: 0x0600AD11 RID: 44305 RVA: 0x003D00A4 File Offset: 0x003CE2A4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Collections.Look<Genepack>(ref this.genepacksToRecombine, "genepacksToRecombine", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.workingInt, "workingInt", false, false);
			Scribe_Values.Look<float>(ref this.workDone, "workDone", 0f, false);
			Scribe_Values.Look<float>(ref this.totalWorkRequired, "totalWorkRequired", 0f, false);
			Scribe_Values.Look<int>(ref this.lastWorkedTick, "lastWorkedTick", -999, false);
			Scribe_Values.Look<int>(ref this.architesRequired, "architesRequired", 0, false);
			Scribe_Values.Look<string>(ref this.xenotypeName, "xenotypeName", null, false);
			Scribe_Defs.Look<XenotypeIconDef>(ref this.iconDef, "iconDef");
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.iconDef == null)
			{
				this.iconDef = XenotypeIconDefOf.Basic;
			}
		}

		// Token: 0x04006458 RID: 25688
		public ThingOwner innerContainer;

		// Token: 0x04006459 RID: 25689
		private List<Genepack> genepacksToRecombine;

		// Token: 0x0400645A RID: 25690
		private int architesRequired;

		// Token: 0x0400645B RID: 25691
		private bool workingInt;

		// Token: 0x0400645C RID: 25692
		private int lastWorkedTick = -999;

		// Token: 0x0400645D RID: 25693
		private float workDone;

		// Token: 0x0400645E RID: 25694
		private float totalWorkRequired;

		// Token: 0x0400645F RID: 25695
		public string xenotypeName;

		// Token: 0x04006460 RID: 25696
		public XenotypeIconDef iconDef;

		// Token: 0x04006461 RID: 25697
		[Unsaved(false)]
		private float lastWorkAmount = -1f;

		// Token: 0x04006462 RID: 25698
		[Unsaved(false)]
		private CompPowerTrader cachedPowerComp;

		// Token: 0x04006463 RID: 25699
		[Unsaved(false)]
		private List<Genepack> tmpGenepacks = new List<Genepack>();

		// Token: 0x04006464 RID: 25700
		[Unsaved(false)]
		private HashSet<Thing> tmpUsedFacilities = new HashSet<Thing>();

		// Token: 0x04006465 RID: 25701
		[Unsaved(false)]
		private int? cachedComplexity;

		// Token: 0x04006466 RID: 25702
		private const int CheckContainersInterval = 180;

		// Token: 0x04006467 RID: 25703
		private static readonly Texture2D CancelIcon = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);

		// Token: 0x04006468 RID: 25704
		private static readonly CachedTexture RecombineIcon = new CachedTexture("UI/Gizmos/RecombineGenes");
	}
}
