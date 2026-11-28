using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019DB RID: 6619
	public class CompWasteProducer : ThingComp
	{
		// Token: 0x17001D20 RID: 7456
		// (get) Token: 0x0600A6A3 RID: 42659 RVA: 0x003B6785 File Offset: 0x003B4985
		private CompProperties_WasteProducer Props
		{
			get
			{
				return (CompProperties_WasteProducer)this.props;
			}
		}

		// Token: 0x17001D21 RID: 7457
		// (get) Token: 0x0600A6A4 RID: 42660 RVA: 0x003B6792 File Offset: 0x003B4992
		public Thing Waste
		{
			get
			{
				return this.parent.TryGetInnerInteractableThingOwner().FirstOrFallback((Thing t) => t.def == ThingDefOf.Wastepack, null);
			}
		}

		// Token: 0x17001D22 RID: 7458
		// (get) Token: 0x0600A6A5 RID: 42661 RVA: 0x003B67C4 File Offset: 0x003B49C4
		public bool CanEmptyNow
		{
			get
			{
				Building_MechGestator building_MechGestator = this.parent as Building_MechGestator;
				return (building_MechGestator == null || building_MechGestator.GestatingMech == null) && this.Waste != null;
			}
		}

		// Token: 0x0600A6A6 RID: 42662 RVA: 0x003B67F3 File Offset: 0x003B49F3
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!ModLister.CheckBiotech("Waste producer"))
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostSpawnSetup(respawningAfterLoad);
		}

		// Token: 0x0600A6A7 RID: 42663 RVA: 0x003B6818 File Offset: 0x003B4A18
		public void ProduceWaste(int amountToMake)
		{
			int num = Mathf.CeilToInt((float)amountToMake / (float)ThingDefOf.Wastepack.stackLimit);
			for (int i = 0; i < num; i++)
			{
				Thing thing = ThingMaker.MakeThing(ThingDefOf.Wastepack, null);
				thing.stackCount = Mathf.Min(amountToMake, ThingDefOf.Wastepack.stackLimit);
				if (!this.parent.TryGetInnerInteractableThingOwner().TryAdd(thing, true))
				{
					break;
				}
				amountToMake -= thing.stackCount;
			}
		}

		// Token: 0x0600A6A8 RID: 42664 RVA: 0x003B6888 File Offset: 0x003B4A88
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (this.Props.showContentsInInspectPane && this.Waste != null)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				int num = 0;
				foreach (Thing thing in ((IEnumerable<Thing>)this.parent.TryGetInnerInteractableThingOwner()))
				{
					if (thing.def == ThingDefOf.Wastepack)
					{
						num += thing.stackCount;
					}
				}
				text = text + ("Contents".Translate() + ": " + this.Waste.LabelCapNoCount + " x") + num.ToString();
			}
			return text;
		}
	}
}
