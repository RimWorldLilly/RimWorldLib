using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CF4 RID: 7412
	public class TransferableImmutable : Transferable
	{
		// Token: 0x170022BF RID: 8895
		// (get) Token: 0x0600BF11 RID: 48913 RVA: 0x00452F92 File Offset: 0x00451192
		public override Thing AnyThing
		{
			get
			{
				if (!this.HasAnyThing)
				{
					return null;
				}
				return this.things[0];
			}
		}

		// Token: 0x170022C0 RID: 8896
		// (get) Token: 0x0600BF12 RID: 48914 RVA: 0x00452FAA File Offset: 0x004511AA
		public override ThingDef ThingDef
		{
			get
			{
				if (!this.HasAnyThing)
				{
					return null;
				}
				return this.AnyThing.def;
			}
		}

		// Token: 0x170022C1 RID: 8897
		// (get) Token: 0x0600BF13 RID: 48915 RVA: 0x00452FC1 File Offset: 0x004511C1
		public override bool HasAnyThing
		{
			get
			{
				return this.things.Count != 0;
			}
		}

		// Token: 0x170022C2 RID: 8898
		// (get) Token: 0x0600BF14 RID: 48916 RVA: 0x00452FD1 File Offset: 0x004511D1
		public override string Label
		{
			get
			{
				return this.AnyThing.LabelNoCount;
			}
		}

		// Token: 0x170022C3 RID: 8899
		// (get) Token: 0x0600BF15 RID: 48917 RVA: 0x00452FDE File Offset: 0x004511DE
		public override bool Interactive
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170022C4 RID: 8900
		// (get) Token: 0x0600BF16 RID: 48918 RVA: 0x00452FE1 File Offset: 0x004511E1
		public override TransferablePositiveCountDirection PositiveCountDirection
		{
			get
			{
				return TransferablePositiveCountDirection.Destination;
			}
		}

		// Token: 0x170022C5 RID: 8901
		// (get) Token: 0x0600BF17 RID: 48919 RVA: 0x00452FE4 File Offset: 0x004511E4
		public override string TipDescription
		{
			get
			{
				if (!this.HasAnyThing)
				{
					return "";
				}
				return this.AnyThing.DescriptionDetailed;
			}
		}

		// Token: 0x170022C6 RID: 8902
		// (get) Token: 0x0600BF18 RID: 48920 RVA: 0x00452FFF File Offset: 0x004511FF
		// (set) Token: 0x0600BF19 RID: 48921 RVA: 0x00453002 File Offset: 0x00451202
		public override int CountToTransfer
		{
			get
			{
				return 0;
			}
			protected set
			{
				if (value != 0)
				{
					throw new InvalidOperationException("immutable transferable");
				}
			}
		}

		// Token: 0x170022C7 RID: 8903
		// (get) Token: 0x0600BF1A RID: 48922 RVA: 0x00453014 File Offset: 0x00451214
		public string LabelWithTotalStackCount
		{
			get
			{
				string text = this.Label;
				int totalStackCount = this.TotalStackCount;
				if (totalStackCount != 1)
				{
					text = text + " x" + totalStackCount.ToStringCached();
				}
				return text;
			}
		}

		// Token: 0x170022C8 RID: 8904
		// (get) Token: 0x0600BF1B RID: 48923 RVA: 0x00453046 File Offset: 0x00451246
		public string LabelCapWithTotalStackCount
		{
			get
			{
				return this.LabelWithTotalStackCount.CapitalizeFirst(this.ThingDef);
			}
		}

		// Token: 0x170022C9 RID: 8905
		// (get) Token: 0x0600BF1C RID: 48924 RVA: 0x0045305C File Offset: 0x0045125C
		public int TotalStackCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.things.Count; i++)
				{
					num += this.things[i].stackCount;
				}
				return num;
			}
		}

		// Token: 0x0600BF1D RID: 48925 RVA: 0x00453096 File Offset: 0x00451296
		public override int GetMinimumToTransfer()
		{
			return 0;
		}

		// Token: 0x0600BF1E RID: 48926 RVA: 0x00453099 File Offset: 0x00451299
		public override int GetMaximumToTransfer()
		{
			return 0;
		}

		// Token: 0x0600BF1F RID: 48927 RVA: 0x0045309C File Offset: 0x0045129C
		public override AcceptanceReport OverflowReport()
		{
			return false;
		}

		// Token: 0x0600BF20 RID: 48928 RVA: 0x004530A4 File Offset: 0x004512A4
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.things.RemoveAll((Thing x) => x.Destroyed);
			}
			Scribe_Collections.Look<Thing>(ref this.things, "things", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.things.RemoveAll((Thing x) => x == null) != 0)
				{
					Log.Warning("Some of the things were null after loading.");
				}
			}
		}

		// Token: 0x04006DAC RID: 28076
		public List<Thing> things = new List<Thing>();
	}
}
