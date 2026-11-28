using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020018EA RID: 6378
	public class CompMechCarrier : ThingComp, IThingHolder
	{
		// Token: 0x17001B8C RID: 7052
		// (get) Token: 0x06009FD6 RID: 40918 RVA: 0x0039449A File Offset: 0x0039269A
		public CompProperties_MechCarrier Props
		{
			get
			{
				return (CompProperties_MechCarrier)this.props;
			}
		}

		// Token: 0x17001B8D RID: 7053
		// (get) Token: 0x06009FD7 RID: 40919 RVA: 0x003944A8 File Offset: 0x003926A8
		public AcceptanceReport CanSpawn
		{
			get
			{
				Pawn pawn = this.parent as Pawn;
				if (pawn != null)
				{
					if (pawn.IsSelfShutdown())
					{
						return "SelfShutdown".Translate();
					}
					if (pawn.Faction == Faction.OfPlayer && !pawn.IsColonyMechPlayerControlled)
					{
						return false;
					}
					if (!pawn.Awake() || pawn.Downed || pawn.Dead || !pawn.Spawned)
					{
						return false;
					}
				}
				if (this.MaxCanSpawn <= 0)
				{
					return "MechCarrierNotEnoughResources".Translate();
				}
				if (this.cooldownTicksRemaining > 0)
				{
					return "CooldownTime".Translate() + " " + this.cooldownTicksRemaining.ToStringSecondsFromTicks();
				}
				return true;
			}
		}

		// Token: 0x17001B8E RID: 7054
		// (get) Token: 0x06009FD8 RID: 40920 RVA: 0x00394571 File Offset: 0x00392771
		public int IngredientCount
		{
			get
			{
				return this.innerContainer.TotalStackCountOfDef(this.Props.fixedIngredient);
			}
		}

		// Token: 0x17001B8F RID: 7055
		// (get) Token: 0x06009FD9 RID: 40921 RVA: 0x00394589 File Offset: 0x00392789
		public int AmountToAutofill
		{
			get
			{
				return Mathf.Max(0, this.maxToFill - this.IngredientCount);
			}
		}

		// Token: 0x17001B90 RID: 7056
		// (get) Token: 0x06009FDA RID: 40922 RVA: 0x0039459E File Offset: 0x0039279E
		public int MaxCanSpawn
		{
			get
			{
				return Mathf.Min(Mathf.FloorToInt((float)(this.IngredientCount / this.Props.costPerPawn)), this.Props.maxPawnsToSpawn);
			}
		}

		// Token: 0x17001B91 RID: 7057
		// (get) Token: 0x06009FDB RID: 40923 RVA: 0x003945C8 File Offset: 0x003927C8
		public bool LowIngredientCount
		{
			get
			{
				return this.IngredientCount < 250;
			}
		}

		// Token: 0x17001B92 RID: 7058
		// (get) Token: 0x06009FDC RID: 40924 RVA: 0x003945D7 File Offset: 0x003927D7
		public float PercentageFull
		{
			get
			{
				return (float)this.IngredientCount / (float)this.Props.maxIngredientCount;
			}
		}

		// Token: 0x06009FDE RID: 40926 RVA: 0x0039460B File Offset: 0x0039280B
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!ModLister.CheckBiotech("Mech carrier"))
			{
				this.parent.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06009FDF RID: 40927 RVA: 0x00394628 File Offset: 0x00392828
		public override void PostPostMake()
		{
			this.innerContainer = new ThingOwner<Thing>(this, false, LookMode.Deep, true);
			if (this.Props.startingIngredientCount > 0)
			{
				Thing thing = ThingMaker.MakeThing(this.Props.fixedIngredient, null);
				thing.stackCount = this.Props.startingIngredientCount;
				this.innerContainer.TryAdd(thing, this.Props.startingIngredientCount, true);
			}
			this.maxToFill = this.Props.startingIngredientCount;
		}

		// Token: 0x06009FE0 RID: 40928 RVA: 0x003946A0 File Offset: 0x003928A0
		public void TrySpawnPawns()
		{
			int maxCanSpawn = this.MaxCanSpawn;
			if (maxCanSpawn <= 0)
			{
				return;
			}
			PawnGenerationRequest request = new PawnGenerationRequest(this.Props.spawnPawnKind, this.parent.Faction, PawnGenerationContext.NonPlayer, null, true, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Newborn, null, null, null, false, false, false, -1, 0, false);
			this.tmpResources.Clear();
			this.tmpResources.AddRange(this.innerContainer);
			Pawn pawn = this.parent as Pawn;
			Lord lord = (pawn != null) ? pawn.GetLord() : null;
			for (int i = 0; i < maxCanSpawn; i++)
			{
				Pawn pawn2 = PawnGenerator.GeneratePawn(request);
				GenSpawn.Spawn(pawn2, this.parent.Position, this.parent.Map, WipeMode.Vanish);
				this.spawnedPawns.Add(pawn2);
				if (lord != null)
				{
					lord.AddPawn(pawn2);
				}
				int num = this.Props.costPerPawn;
				for (int j = 0; j < this.tmpResources.Count; j++)
				{
					Thing thing = this.innerContainer.Take(this.tmpResources[j], Mathf.Min(this.tmpResources[j].stackCount, this.Props.costPerPawn));
					num -= thing.stackCount;
					thing.Destroy(DestroyMode.Vanish);
					if (num <= 0)
					{
						break;
					}
				}
				if (this.Props.spawnedMechEffecter != null)
				{
					Effecter effecter = new Effecter(this.Props.spawnedMechEffecter);
					effecter.Trigger(this.Props.attachSpawnedMechEffecter ? pawn2 : new TargetInfo(pawn2.Position, pawn2.Map, false), TargetInfo.Invalid, -1);
					effecter.Cleanup();
				}
			}
			this.tmpResources.Clear();
			this.cooldownTicksRemaining = this.Props.cooldownTicks;
			if (this.Props.spawnEffecter != null)
			{
				Effecter effecter2 = new Effecter(this.Props.spawnEffecter);
				effecter2.Trigger(this.Props.attachSpawnedEffecter ? this.parent : new TargetInfo(this.parent.Position, this.parent.Map, false), TargetInfo.Invalid, -1);
				effecter2.Cleanup();
			}
		}

		// Token: 0x06009FE1 RID: 40929 RVA: 0x0039493E File Offset: 0x00392B3E
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompMechCarrier.<CompGetGizmosExtra>d__25 <CompGetGizmosExtra>d__ = new CompMechCarrier.<CompGetGizmosExtra>d__25(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x06009FE2 RID: 40930 RVA: 0x0039494E File Offset: 0x00392B4E
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06009FE3 RID: 40931 RVA: 0x0039495C File Offset: 0x00392B5C
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06009FE4 RID: 40932 RVA: 0x00394964 File Offset: 0x00392B64
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			return text + ("CasketContains".Translate() + ": " + this.innerContainer.ContentsString.CapitalizeFirst());
		}

		// Token: 0x06009FE5 RID: 40933 RVA: 0x003949C4 File Offset: 0x00392BC4
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			ThingOwner thingOwner = this.innerContainer;
			if (thingOwner != null)
			{
				thingOwner.ClearAndDestroyContents(DestroyMode.Vanish);
			}
			for (int i = 0; i < this.spawnedPawns.Count; i++)
			{
				if (!this.spawnedPawns[i].Dead)
				{
					this.spawnedPawns[i].Kill(null, null);
				}
			}
		}

		// Token: 0x06009FE6 RID: 40934 RVA: 0x00394A30 File Offset: 0x00392C30
		public override void PostDrawExtraSelectionOverlays()
		{
			if (Find.Selector.IsSelected(this.parent))
			{
				for (int i = 0; i < this.spawnedPawns.Count; i++)
				{
					if (!this.spawnedPawns[i].Dead)
					{
						GenDraw.DrawLineBetween(this.parent.TrueCenter(), this.spawnedPawns[i].TrueCenter());
					}
				}
			}
		}

		// Token: 0x06009FE7 RID: 40935 RVA: 0x00394A9C File Offset: 0x00392C9C
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Values.Look<int>(ref this.cooldownTicksRemaining, "cooldownTicksRemaining", 0, false);
			Scribe_Values.Look<int>(ref this.maxToFill, "maxToFill", 0, false);
			Scribe_Collections.Look<Pawn>(ref this.spawnedPawns, "spawnedPawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.spawnedPawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06009FE8 RID: 40936 RVA: 0x00394B36 File Offset: 0x00392D36
		public override void CompTick()
		{
			if (this.cooldownTicksRemaining > 0)
			{
				this.cooldownTicksRemaining--;
			}
		}

		// Token: 0x04005E88 RID: 24200
		private const int LowIngredientCountThreshold = 250;

		// Token: 0x04005E89 RID: 24201
		private int cooldownTicksRemaining;

		// Token: 0x04005E8A RID: 24202
		private ThingOwner innerContainer;

		// Token: 0x04005E8B RID: 24203
		private List<Pawn> spawnedPawns = new List<Pawn>();

		// Token: 0x04005E8C RID: 24204
		private MechCarrierGizmo gizmo;

		// Token: 0x04005E8D RID: 24205
		public int maxToFill;

		// Token: 0x04005E8E RID: 24206
		private List<Thing> tmpResources = new List<Thing>();
	}
}
