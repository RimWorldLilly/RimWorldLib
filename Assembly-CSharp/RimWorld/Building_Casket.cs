using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AA2 RID: 6818
	public class Building_Casket : Building, IThingHolder, IOpenable, ISearchableContents
	{
		// Token: 0x17001EA6 RID: 7846
		// (get) Token: 0x0600AC2A RID: 44074 RVA: 0x003CC659 File Offset: 0x003CA859
		public virtual int OpenTicks
		{
			get
			{
				return 300;
			}
		}

		// Token: 0x0600AC2B RID: 44075 RVA: 0x003CC660 File Offset: 0x003CA860
		public Building_Casket()
		{
			this.innerContainer = new ThingOwner<Thing>(this, false, LookMode.Deep, true);
		}

		// Token: 0x17001EA7 RID: 7847
		// (get) Token: 0x0600AC2C RID: 44076 RVA: 0x003CC677 File Offset: 0x003CA877
		public bool HasAnyContents
		{
			get
			{
				return this.innerContainer.Count > 0;
			}
		}

		// Token: 0x17001EA8 RID: 7848
		// (get) Token: 0x0600AC2D RID: 44077 RVA: 0x003CC687 File Offset: 0x003CA887
		public Thing ContainedThing
		{
			get
			{
				if (this.innerContainer.Count != 0)
				{
					return this.innerContainer[0];
				}
				return null;
			}
		}

		// Token: 0x17001EA9 RID: 7849
		// (get) Token: 0x0600AC2E RID: 44078 RVA: 0x003CC6A4 File Offset: 0x003CA8A4
		public virtual bool CanOpen
		{
			get
			{
				return this.HasAnyContents;
			}
		}

		// Token: 0x17001EAA RID: 7850
		// (get) Token: 0x0600AC2F RID: 44079 RVA: 0x003CC6AC File Offset: 0x003CA8AC
		public ThingOwner SearchableContents
		{
			get
			{
				return this.innerContainer;
			}
		}

		// Token: 0x0600AC30 RID: 44080 RVA: 0x003CC6B4 File Offset: 0x003CA8B4
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x0600AC31 RID: 44081 RVA: 0x003CC6BC File Offset: 0x003CA8BC
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x0600AC32 RID: 44082 RVA: 0x003CC6CC File Offset: 0x003CA8CC
		public virtual void Open()
		{
			if (!this.HasAnyContents)
			{
				return;
			}
			this.EjectContents();
			if (!this.openedSignal.NullOrEmpty())
			{
				Find.SignalManager.SendSignal(new Signal(this.openedSignal, this.Named("SUBJECT")));
			}
			base.DirtyMapMesh(base.Map);
		}

		// Token: 0x0600AC33 RID: 44083 RVA: 0x003CC721 File Offset: 0x003CA921
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Building_Casket.<GetGizmos>d__17 <GetGizmos>d__ = new Building_Casket.<GetGizmos>d__17(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600AC34 RID: 44084 RVA: 0x003CC734 File Offset: 0x003CA934
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Values.Look<bool>(ref this.contentsKnown, "contentsKnown", false, false);
			Scribe_Values.Look<string>(ref this.openedSignal, "openedSignal", null, false);
		}

		// Token: 0x0600AC35 RID: 44085 RVA: 0x003CC785 File Offset: 0x003CA985
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (base.Faction != null && base.Faction.IsPlayer)
			{
				this.contentsKnown = true;
			}
		}

		// Token: 0x0600AC36 RID: 44086 RVA: 0x003CC7AB File Offset: 0x003CA9AB
		public override AcceptanceReport ClaimableBy(Faction fac)
		{
			if (this.innerContainer.Any && !this.contentsKnown)
			{
				return false;
			}
			return base.ClaimableBy(fac);
		}

		// Token: 0x0600AC37 RID: 44087 RVA: 0x003CC7D0 File Offset: 0x003CA9D0
		public virtual bool Accepts(Thing thing)
		{
			return this.innerContainer.CanAcceptAnyOf(thing, true);
		}

		// Token: 0x0600AC38 RID: 44088 RVA: 0x003CC7E0 File Offset: 0x003CA9E0
		public virtual bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
		{
			if (!this.Accepts(thing))
			{
				return false;
			}
			bool flag;
			if (thing.holdingOwner != null)
			{
				thing.holdingOwner.TryTransferToContainer(thing, this.innerContainer, thing.stackCount, true);
				flag = true;
			}
			else
			{
				flag = this.innerContainer.TryAdd(thing, true);
			}
			if (flag)
			{
				if (thing.Faction != null && thing.Faction.IsPlayer)
				{
					this.contentsKnown = true;
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600AC39 RID: 44089 RVA: 0x003CC850 File Offset: 0x003CAA50
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.Destroy(mode);
			if (this.innerContainer.Count > 0 && (mode == DestroyMode.Deconstruct || mode == DestroyMode.KillFinalize))
			{
				if (mode != DestroyMode.Deconstruct)
				{
					List<Pawn> list = new List<Pawn>();
					foreach (Thing thing in ((IEnumerable<Thing>)this.innerContainer))
					{
						Pawn pawn = thing as Pawn;
						if (pawn != null)
						{
							list.Add(pawn);
						}
					}
					foreach (Pawn p in list)
					{
						HealthUtility.DamageUntilDowned(p, true, null, null, null);
					}
				}
				this.innerContainer.TryDropAll(base.Position, map, ThingPlaceMode.Near, null, null, true);
			}
			this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
		}

		// Token: 0x0600AC3A RID: 44090 RVA: 0x003CC93C File Offset: 0x003CAB3C
		public virtual void EjectContents()
		{
			this.innerContainer.TryDropAll(this.InteractionCell, base.Map, ThingPlaceMode.Near, null, null, true);
			this.contentsKnown = true;
		}

		// Token: 0x0600AC3B RID: 44091 RVA: 0x003CC964 File Offset: 0x003CAB64
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			string str;
			if (!this.contentsKnown)
			{
				str = "UnknownLower".Translate();
			}
			else
			{
				str = this.innerContainer.ContentsString;
			}
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			return text + ("CasketContains".Translate() + ": " + str.CapitalizeFirst());
		}

		// Token: 0x0400641B RID: 25627
		protected ThingOwner innerContainer;

		// Token: 0x0400641C RID: 25628
		protected bool contentsKnown;

		// Token: 0x0400641D RID: 25629
		public string openedSignal;
	}
}
